namespace Domain.Contracts.Services;

public interface IToDoListPublisher<in T>: IEventPublisher<T> where T : class, IEventBase
{
    
}