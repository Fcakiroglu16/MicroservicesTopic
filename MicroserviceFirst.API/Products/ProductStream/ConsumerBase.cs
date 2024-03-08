using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace MicroserviceFirst.API.Products.ProductStream
{
    public abstract class ConsumerBase<T>
        where T : class
    {
        protected readonly string Topic;
        protected readonly ConsumerConfig ConsumerConfig;
        protected readonly SchemaRegistryConfig SchemaRegistryConfig;
        protected CachedSchemaRegistryClient SchemaRegistry;
        public IConsumer<string, T> Consumer;

        protected ConsumerBase()
        {
        }

        public ConsumerBase(string bootstrapServers, string schemaRegistryUrl, string consumerGroup, string topic)
        {
            Topic = topic;

            ConsumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = consumerGroup
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

        public T ConsumeA(CancellationToken token)
        {
            try
            {
                var cr = Consumer.Consume(token);
                return cr.Message.Value;
            }
            catch (OperationCanceledException)
            {
                Consumer.Close();
            }

            return default(T);
        }

        public void Close()
        {
            Consumer.Close();
            SchemaRegistry.Dispose();
        }
    }
}