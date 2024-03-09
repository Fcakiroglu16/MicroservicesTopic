using MicroserviceFirst.API.KafkaServiceBus.Consumer;
using MicroserviceFirst.API.Products.ProductStream;
using MicroserviceFirst.API.Products.ProductStream.Events;
using MicroserviceFirst.API.ServiceBus;
using Newtonsoft.Json.Linq;

namespace MicroserviceFirst.API.BackgroundServices
{
    public class ProductStreamBackgroundServices(IConfiguration configuration) : BackgroundService
    {
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
            //you can write generic Json record


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
                    //Console.WriteLine($"{productCreatedEvent.Name}");

                    vehicleConsumer.Consumer.Commit(consumeResult);
                }


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}