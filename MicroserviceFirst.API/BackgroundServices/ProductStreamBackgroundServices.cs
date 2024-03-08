using Confluent.Kafka;
using JsonConsole;
using MicroserviceFirst.API.Products.ProductStream;
using MicroserviceFirst.API.Products.ProductStream.Events;
using static Confluent.Kafka.ConfigPropertyNames;

namespace MicroserviceFirst.API.BackgroundServices
{
    public class ProductStreamBackgroundServices(IConfiguration configuration) : BackgroundService
    {
        private IConsumer<string, EventBase>? _consumer;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var vehicleConsumer = new JsonConsumer<ProductCreatedEvent>(
                configuration.GetSection("Kafka")["BootstrapServers"]!, "http://localhost:8081",
                KafkaProductStream.ProductStreamGroupName, KafkaProductStream.ProductStreamTopic);


            vehicleConsumer.Build();


            while (!stoppingToken.IsCancellationRequested)
            {
                //var consumeResult = vehicleConsumer.Consume(stoppingToken);

                var consumeResult = vehicleConsumer.Consumer.Consume(1000);


                if (consumeResult is not null)
                {
                    var productCreatedEvent = consumeResult.Message.Value;


                    var name = productCreatedEvent.Name;
                    ;
                    //var a = consumeResult.Message.Value as ProductCreatedEvent;


                    //var productCreatedEvent = consumeResult.Message.Value;


                    //Console.WriteLine(productCreatedEvent);
                    //_consumer.Commit(consumeResult);
                }

                Console.WriteLine("While");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}