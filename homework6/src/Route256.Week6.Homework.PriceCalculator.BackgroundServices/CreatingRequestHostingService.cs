using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

using Route256.Week6.Homework.PriceCalculator.BackgroundServices.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices;

public class CreatingRequestHostingService : BackgroundService
{
    private const string BROKER = "localhost:9092";
    private const string TOPIC_READING_GOODS = "good_price_calc_requests";
    private IProducer<long, GoodEntity> _producer;
    private long _lastId = 0;

    public CreatingRequestHostingService()
    {
        _producer = new ProducerBuilder<long, GoodEntity>(new ProducerConfig
        {
            BootstrapServers = BROKER,
            Acks = Acks.All,
        }).SetKeySerializer(new KafkaSerializer<long>())
        .SetValueSerializer(new KafkaSerializer<GoodEntity>()).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random rnd = new Random();
        while (!stoppingToken.IsCancellationRequested)
        {
            await _producer.ProduceAsync(TOPIC_READING_GOODS,
                    new Message<long, GoodEntity>
                    {
                        Key = _lastId,
                        Value = new GoodEntity(_lastId, rnd.Next(1, 50), 
                        rnd.Next(1, 50), rnd.Next(1, 50), rnd.Next(10000))
                    }, default);
            ++_lastId;
            await Task.Delay(100);
        }
    }
}