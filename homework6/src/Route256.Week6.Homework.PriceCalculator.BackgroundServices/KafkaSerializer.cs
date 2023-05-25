using Confluent.Kafka;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices;

internal class KafkaSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            throw new ArgumentNullException("argument was null");
        }
        return JsonSerializer.Deserialize<T>(data);
    }
}
