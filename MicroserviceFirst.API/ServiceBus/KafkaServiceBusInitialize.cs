using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MicroserviceFirst.API.Products.ProductStream;
using MicroserviceFirst.API.Products.ProductStream.Events;

namespace MicroserviceFirst.API.ServiceBus
{
    public class KafkaServiceBusInitialize(IConfiguration configuration, ILogger<KafkaServiceBusInitialize> logger)
    {
        public async Task CreateTopics()
        {
            //https://www.baeldung.com/kafka-message-retention
            //We can notice here that the default retention time is seven days.
            //168 hours = 7 days
            //log.retention.hours
            //log.retention.minutes
            //log.retention.ms


            using var adminClient = new AdminClientBuilder(new AdminClientConfig
                { BootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"] }).Build();
            try
            {
                var topicConfig = new Dictionary<string, string>
                {
                    // Specify the message retention time in milliseconds (e.g., 1 day)
                    { "retention.ms", "-1" },
                };


                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = KafkaProductStream.ProductStreamTopic, ReplicationFactor = 1, NumPartitions = 12,
                        Configs = topicConfig
                    }
                });
            }
            catch (CreateTopicsException e)
            {
                logger.LogWarning(e, "An error occured creating topics");
            }
        }
    }
}