using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

namespace MicroserviceFirst.API.KafkaServiceBus.Consumer;

public class AvroConsumer<T> : ConsumerBase<T>
    where T : class
{
    public AvroConsumer(string bootstrapServers, string schemaRegistryUrl, string consumerGroup, string topic)
        : base(bootstrapServers, schemaRegistryUrl, consumerGroup, topic)
    {
    }

    public void Build()
    {
        AddSchemaRegistry();
        Consumer =
            new ConsumerBuilder<string, T>(ConsumerConfig)
                .SetValueDeserializer(new AvroDeserializer<T>(SchemaRegistry).AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();

        Consumer.Subscribe(Topic);
    }
}