
using Domain.Interfaces;

namespace Domain.Services
{
    public class ConsoleEventPublisher : IEventPublisher
    {
        public Task PublishAsync<TEvent>(TEvent @event)
        {
            Console.WriteLine($"Event published: {typeof(TEvent).Name}");
            return Task.CompletedTask;
        }
    }
}
