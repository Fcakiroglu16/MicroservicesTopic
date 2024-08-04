using MassTransit;
using SharedEvents;

namespace MicroserviceSecond.API.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            await Task.Delay(5000);
            await context.RespondAsync(new UserCreatedEventResult("userId:123"));
        }
    }
}