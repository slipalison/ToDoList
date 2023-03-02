namespace Domain.Contracts.Services;

public interface IToDoListPublisher<in T>: IEventPublisher<T> where T : class, IEventBase
{
    
}

public interface IEventPublisher<in T> where T : class, IEventBase
{
    Task PublishAsync(T @event);
}