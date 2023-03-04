using Domain.Contracts.Services;
using Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Infra.MassTransitConfiguration.Publishers;

public abstract class AbstractPublisher<T> : IEventPublisher<T> where T : class, IEventBase
{
    private readonly ILogger _logger;
    private readonly IBus _bus;
    private readonly ICorrelationContextService _correlationContextService;
    private readonly AsyncRetryPolicy _retry;

    protected AbstractPublisher(ILogger logger, IBus bus, ICorrelationContextService correlationContextService)
    {
        _logger = logger;
        _bus = bus;
        _correlationContextService = correlationContextService;

        _retry = Policy.Handle<Exception>()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(4),
                TimeSpan.FromSeconds(8),
                TimeSpan.FromSeconds(16)
            });
    }

    public virtual Task PublishAsync(T @event)
    {
        var correlationId = _correlationContextService.GetCorrelationId();
        try
        {
            return _retry.ExecuteAsync(async () =>
            {
                await _bus.Publish(@event, x =>
                {
                    x.SetRoutingKey(@event.RoutingKey);
                    x.CorrelationId = correlationId;
                });
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro ao publicar o evento, {Type}, RoutingKey {Routing}, CorrelationId: {Correlation}",
                typeof(T).FullName, @event.RoutingKey, @event.CorrelationId);
        }

        return Task.CompletedTask;
    }
}