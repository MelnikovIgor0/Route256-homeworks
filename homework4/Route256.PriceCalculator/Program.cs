using System.Threading.Channels;
using Route256.Domain.Services.Interfaces;
using Route256.Domain.Services;
using Route256.Domain.Models.PriceCalculator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Route256.PriceCalculator;

internal class Program
{
    private static readonly Channel<GoodModel> _goodsChannel = 
        Channel.CreateUnbounded<GoodModel>();
    
    private static readonly Channel<LoggedEntity> _loggedEntityChannel = 
        Channel.CreateUnbounded<LoggedEntity>();

    private static ConfigData _configuration;
    private static SemaphoreSlim _semaphore;
    private static Mutex _mutexGoods = new Mutex();
    private static Mutex _mutexLogged = new Mutex();
    private static Mutex _mutexConsole = new Mutex();

    private static int _totalNumberGoods = 0;
    private static int _numberCalculated = 0;
    private static IPriceCalculatorService _priceCalculatorService;
    private static int _totalProcessed = 0;
    private const int BATCH_SIZE = 8;
    private static StreamReader _fileReader;
    private static StreamWriter _fileWriter;
    private static int _progressRead = 0;
    private static int _progressCalculate = 0;
    private static int _progressWrite = 0;

    private static void ReadGoodModels()
    {
        _totalNumberGoods = 0;
        for (int i = 0; i < BATCH_SIZE; ++i)
        {
            if (_fileReader.EndOfStream)
            {
                break;
            }
            string line = _fileReader.ReadLine();
            if (line.Equals(""))
            {
                break;
            }
            _mutexGoods.WaitOne();
            _goodsChannel.Writer.TryWrite(GoodModel.Parse(line));
            _mutexGoods.ReleaseMutex();
            ++_totalNumberGoods;
            ++_progressRead;
            DrawProgress();
        }
    }

    private static async void CalculateAndWritePriceOfModelAsync(GoodModel model)
    {
        _semaphore.Wait();
        LoggedEntity result = new LoggedEntity(model.Id, _priceCalculatorService.CalculatePrice(
            new CalculateRequest(new GoodModel[] {model}, 1)));
        _mutexLogged.WaitOne();
        _loggedEntityChannel.Writer.TryWrite(result);
        int currentNumber = ++_numberCalculated;
        _mutexLogged.ReleaseMutex();
        ++_progressCalculate;
        DrawProgress();
        _semaphore.Release();
    }

    private static void CalculatePrices()
    {
        _numberCalculated = 0;
        List<Task> tasks = new List<Task>();
        int numberCalled = 0;
        GoodModel model;
        while (_goodsChannel.Reader.TryPeek(out model))
        {
            tasks.Add(Task.Run(() => CalculateAndWritePriceOfModelAsync(model)));
            ++numberCalled;
            if (numberCalled >= _totalNumberGoods)
            {
                break;
            }
        }
        Task.WaitAll(tasks.ToArray());
    }

    private static async void WriteLoggerEntities()
    {
        int numberWritten = 0;
        LoggedEntity loggerEntity;
        while (_loggedEntityChannel.Reader.TryPeek(out loggerEntity))
        {
            _fileWriter.WriteLine(loggerEntity.Id + ", " +
                                    loggerEntity.DeliveryPrice.ToString().Replace(",", "."));
            ++numberWritten;
            ++_progressWrite;
            DrawProgress();
            if (numberWritten >= _totalNumberGoods)
            {
                break;
            }
        }
    }

    private static void DrawProgress()
    {
        _mutexConsole.WaitOne();
        Console.Clear();
        Console.WriteLine("[" + new string('=', _progressRead % 20) + 
            new string(' ', 20 - (_progressRead % 20)) + $"] Прочитано {_progressRead}");
        Console.WriteLine("[" + new string('=', _progressCalculate % 20) +
            new string(' ', 20 - (_progressCalculate % 20)) + $"] Рассчитано {_progressCalculate}");
        Console.WriteLine("[" + new string('=', _progressWrite % 20) +
            new string(' ', 20 - (_progressWrite % 20)) + $"] Записано {_progressWrite}");
        Thread.Sleep(1);
        _mutexConsole.ReleaseMutex();
    }

    private static void SetConfig()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() +
                $"..{Path.DirectorySeparatorChar}" +
                $"..{Path.DirectorySeparatorChar}" +
                $"..{Path.DirectorySeparatorChar}" +
                $"..{Path.DirectorySeparatorChar}")
                .AddJsonFile("appconfig.json",
                             optional: false, reloadOnChange: true);
        IConfiguration config = builder.Build();
        _configuration = config.GetSection("ConfigData").Get<ConfigData>();
        _priceCalculatorService = new PriceCalculatorService(new Domain.PriceCalculatorOptions
        {
            VolumeToPriceRatio = (decimal)_configuration.VolumeToPriceRatio,
            WeightToPriceRatio = (decimal)_configuration.WeightToPriceRatio
        }, new StoragePass());
    }

    private static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddOptions<ConfigData>().Bind(configuration.GetSection(SetupOptions.ConfigKey));
        serviceCollection.AddTransient<Program>();
        SetConfig();
        using (_fileReader = new StreamReader(_configuration.InputFile))
        {
            using (_fileWriter = new StreamWriter(_configuration.OutputFile))
            {
                _fileReader.ReadLine();
                _fileWriter.WriteLine("id, delivery_price");
                while (!_fileReader.EndOfStream)
                {
                    _semaphore = new SemaphoreSlim(_configuration.Parallelism);
                    ReadGoodModels();
                    CalculatePrices();
                    WriteLoggerEntities();
                    _totalProcessed += BATCH_SIZE;
                }
                _fileReader.Close();
                _fileWriter.Close();
            }
        }
    }
}
