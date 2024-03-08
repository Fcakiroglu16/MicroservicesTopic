using Common;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using MicroserviceFirst.API.Products.ProductStream;

namespace JsonConsole
{
    internal class JsonConsumer<T> : ConsumerBase<T>
        where T : class
    {
        public JsonConsumer(string bootstrapServers, string schemaRegistryUrl, string consumerGroup, string topic)
            : base(bootstrapServers, schemaRegistryUrl, consumerGroup, topic)
        {
        }

        public void Build()
        {
            base.AddSchemaRegistry();

            Consumer =
                new ConsumerBuilder<string, T>(ConsumerConfig)
                    .SetKeyDeserializer(Deserializers.Utf8)
                    .SetValueDeserializer(new JsonDeserializer<T>().AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();

            Consumer.Subscribe(Topic);
        }
    }
}