using Confluent.Kafka;
using System.Text.Json;

namespace MicroserviceFirst.API.Products.ProductStream
{
    internal class CustomValueDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<T>(data)!;
        }
    }
}