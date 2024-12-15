using auth_service.Models;
using MassTransit;

namespace auth_service.Consumers;

public class UserConsumer : IConsumer<UserRabbitMQ>
{
    public async Task Consume(ConsumeContext<UserRabbitMQ> context)
    {
        var user = context.Message;
        Console.WriteLine($"User received: {user.Email}");
    }
    
}