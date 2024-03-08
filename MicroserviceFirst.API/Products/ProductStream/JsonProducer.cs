using Common;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

namespace JsonConsole
{
    public class JsonProducer<T>(string bootstrapServers, string schemaRegistryUrl, string topic)
        : ProducerBase<T>(bootstrapServers, schemaRegistryUrl, topic)
        where T : class
    {
        public void Build()
        {
            base.AddSchemaRegistry();

            Producer =
                new ProducerBuilder<string, T>(ProducerConfig)
                    .SetValueSerializer(new JsonSerializer<T>(SchemaRegistry))
                    .Build();
        }
    }
}