using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace MicroserviceFirst.API.KafkaServiceBus.Producer
{
    public class JsonProducer<T>(string bootstrapServers, string schemaRegistryUrl, string topic)
        : ProducerBase<T>(bootstrapServers, schemaRegistryUrl, topic)
        where T : class
    {
        public void Build()
        {
            AddSchemaRegistry();

            Producer =
                new ProducerBuilder<string, T>(ProducerConfig)
                    .SetValueSerializer(new JsonSerializer<T>(SchemaRegistry,
                        new JsonSerializerConfig() { SubjectNameStrategy = SubjectNameStrategy.Record }))
                    .Build();
        }
    }
}