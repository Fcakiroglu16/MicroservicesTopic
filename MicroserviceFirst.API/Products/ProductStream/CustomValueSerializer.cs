using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace MicroserviceFirst.API.Products.ProductStream
{
    internal class CustomValueSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, typeof(T),
                new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}