namespace Domain.Contracts.Services;

public interface IEventBase
{
    Guid CorrelationId { get; }
    string RoutingKey { get; }
}

public interface ICorrelationContextService
{
    Guid GetCorrelationId();
    void SetCorrelationId(Guid correlationId);
}

public class CorrelationContextService : ICorrelationContextService
{
    private Guid? _correlationId;

    public Guid GetCorrelationId()
    {
        _correlationId ??= Guid.NewGuid();
        return _correlationId.Value;
    }

    public void SetCorrelationId(Guid correlationId)
    {
        _correlationId = correlationId;
    }
}