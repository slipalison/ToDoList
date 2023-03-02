using Domain.Contracts.Services;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Infra.MassTransitConfiguration.Publishers;

public class ToDoListPublisher<T> : AbstractPublisher<T>, IToDoListPublisher<T> where T : class, IEventBase
{
    public ToDoListPublisher(ILogger<ToDoListPublisher<T>> logger, IToDoListBus bus,
        ICorrelationContextService correlationContextService) : base(logger, bus, correlationContextService)
    {
    }
}