namespace Domain.Contracts.Services;

public interface IEventBase
{
    Guid CorrelationId { get; }
    string RoutingKey { get; }
}