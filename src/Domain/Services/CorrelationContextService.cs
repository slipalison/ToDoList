using Domain.Services;

namespace Domain.Contracts.Services;

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