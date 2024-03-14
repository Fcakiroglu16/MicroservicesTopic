using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace MicroserviceFirst.API.KafkaServiceBus.Producer;

public class AvroProducer<T> : ProducerBase<T>
    where T : class
{
    public AvroProducer(string bootstrapServers, string schemaRegistryUrl, string topic)
        : base(bootstrapServers, schemaRegistryUrl, topic)
    {
    }

    public void Build()
    {
        AddSchemaRegistry();

        Producer =
            new ProducerBuilder<string, T>(ProducerConfig)
                .SetValueSerializer(new AvroSerializer<T>(SchemaRegistry, new AvroSerializerConfig
                {
                    SubjectNameStrategy = SubjectNameStrategy.Record
                }))
                .Build();
    }
}