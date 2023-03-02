namespace Domain.Contracts.Services;

public interface IEventPublisher<in T> where T : class, IEventBase
{
    Task PublishAsync(T @event);
}