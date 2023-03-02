namespace Domain.Services;

public interface ICorrelationContextService
{
    Guid GetCorrelationId();
    void SetCorrelationId(Guid correlationId);
}