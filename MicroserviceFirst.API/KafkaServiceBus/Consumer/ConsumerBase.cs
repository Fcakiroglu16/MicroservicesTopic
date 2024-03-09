using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace MicroserviceFirst.API.KafkaServiceBus.Consumer
{
    public abstract class ConsumerBase<T>(
        string bootstrapServers,
        string schemaRegistryUrl,
        string consumerGroup,
        string topic)
        where T : class
    {
        protected readonly string Topic = topic;

        protected readonly ConsumerConfig ConsumerConfig = new()
        {
            BootstrapServers = bootstrapServers,
            GroupId = consumerGroup,
            EnableAutoCommit = false
        };

        protected readonly SchemaRegistryConfig SchemaRegistryConfig = new()
        {
            Url = schemaRegistryUrl
        };

        protected CachedSchemaRegistryClient? SchemaRegistry;


        public IConsumer<string, T>? Consumer;


        protected void AddSchemaRegistry()
        {
            SchemaRegistry = new CachedSchemaRegistryClient(SchemaRegistryConfig);
        }


        public void Close()
        {
            Consumer?.Close();
            SchemaRegistry?.Dispose();
        }
    }
}