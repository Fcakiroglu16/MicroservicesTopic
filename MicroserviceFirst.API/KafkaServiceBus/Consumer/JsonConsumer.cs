using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

namespace MicroserviceFirst.API.KafkaServiceBus.Consumer
{
    internal class JsonConsumer<T>(
        string bootstrapServers,
        string schemaRegistryUrl,
        string consumerGroup,
        string topic)
        : ConsumerBase<T>(bootstrapServers, schemaRegistryUrl, consumerGroup, topic)
        where T : class
    {
        public void Build()
        {
            AddSchemaRegistry();

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