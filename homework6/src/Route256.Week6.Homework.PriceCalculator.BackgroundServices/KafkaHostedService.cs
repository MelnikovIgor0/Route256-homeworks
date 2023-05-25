using System.Runtime.CompilerServices;
using Microsoft.Extensions;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Route256.Week6.Homework.PriceCalculator.BackgroundServices.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices;

public class KafkaHostedService : BackgroundService
{
    private const string BROKER = "localhost:9092";
    private const string TOPIC_READING_GOODS = "good_price_calc_requests";
    private const string TOPIC_WRITING_RESULTS = "good_price_calc";
    private const string TOPIC_INVALID_MESSAGES = "good_price_calc_requests_dlq";
    private IProducer<long, CalculationResult> _producer;
    private IProducer<long, object> _producerInvalid;
    private IConsumer<long, GoodEntity> _consumer;
    private ICalculationService _priceCalculationService =
        new CalculationService(new CalculationRepositoryPlug(), new GoodRepositoryPlug());
    private int _numberInvalidMessages = 0;

    public KafkaHostedService()
    {
        _producer = new ProducerBuilder<long, CalculationResult>(new ProducerConfig
        {
            BootstrapServers = BROKER,
            Acks = Acks.All,
        }).SetKeySerializer(new KafkaSerializer<long>())
        .SetValueSerializer(new KafkaSerializer<CalculationResult>()).Build();
        
        _producerInvalid = new ProducerBuilder<long, object>(new ProducerConfig
        {
            BootstrapServers = BROKER,
            Acks = Acks.All
        }).SetKeySerializer(new KafkaSerializer<long>())
        .SetValueSerializer(new KafkaSerializer<object>()).Build();

        _consumer = new ConsumerBuilder<long, GoodEntity>(new ConsumerConfig
        {
            BootstrapServers = BROKER,
            GroupId = "reading-goods",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnableAutoOffsetStore = false
        }).SetKeyDeserializer(new KafkaSerializer<long>())
        .SetValueDeserializer(new KafkaSerializer<GoodEntity>()).Build();
        _consumer.Subscribe(TOPIC_READING_GOODS);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested &&
            _consumer.Consume(TimeSpan.FromSeconds(60)) is { } good)
        {
            //try
            //{
            var volumePrice = _priceCalculationService.CalculatePriceByVolume(new [] {
                new GoodModel(
                    good.Message.Value.Height,
                    good.Message.Value.Length,
                    good.Message.Value.Width,
                    good.Message.Value.Weight)
            }, out var volume);
            var weightPrice = _priceCalculationService.CalculatePriceByWeight(new[] {
                new GoodModel(
                    good.Message.Value.Height,
                    good.Message.Value.Length,
                    good.Message.Value.Width,
                    good.Message.Value.Weight)
            }, out var weight);
            var result = Math.Max(volumePrice, weightPrice);
            await _producer.ProduceAsync(TOPIC_WRITING_RESULTS,
                new Message<long, CalculationResult>
                {
                    Key = good.Message.Value.Id,
                    Value = new CalculationResult(good.Message.Value.Id, (double)result)
                }, default);
            /*}
            catch (Exception ignored)
            {
                await _producerInvalid.ProduceAsync(TOPIC_INVALID_MESSAGES,
                    new Message<long, object>
                    {
                        Key = _numberInvalidMessages++,
                        Value = good.Message
                    }, default);
            }*/
        }
    }
}