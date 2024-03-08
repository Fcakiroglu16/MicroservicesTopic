using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace Common
{
    public abstract class ProducerBase<T>
        where T : class
    {
        protected readonly string Topic;
        protected readonly ProducerConfig ProducerConfig;
        protected readonly SchemaRegistryConfig SchemaRegistryConfig;
        protected CachedSchemaRegistryClient SchemaRegistry;
        protected IProducer<string, T> Producer;


        public ProducerBase(string bootstrapServers, string schemaRegistryUrl, string topic)
        {
            Topic = topic;

            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            SchemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            };
        }

        protected void AddSchemaRegistry()
        {
            SchemaRegistry = new CachedSchemaRegistryClient(SchemaRegistryConfig);
        }

        public async Task ProduceAsync(T message)
        {
            await Producer.ProduceAsync(Topic, new Message<string, T> { Value = message });
        }

        public void Close()
        {
            Producer.Dispose();
            SchemaRegistry.Dispose();
        }
    }
}