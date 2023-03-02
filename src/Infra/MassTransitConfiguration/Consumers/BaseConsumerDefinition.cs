using MassTransit;

namespace Infra.MassTransitConfiguration.Consumers;

public abstract class BaseConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    private readonly IServiceProvider _provider;
    private readonly string _exchange;
    private readonly string _routingKey;
    private readonly string _exchangeType;

    protected BaseConsumerDefinition(IServiceProvider provider, string exchange, string routingKey, string endipoint,
        string exchangeType = RabbitMQ.Client.ExchangeType.Direct)
    {
        _provider = provider;
        _exchange = exchange;
        _routingKey = routingKey;
        EndpointName = endipoint;
        _exchangeType = exchangeType;

        // _logger 

        Endpoint(x =>
        {
            var preFetchCount = 50;
            //_logger
            x.PrefetchCount = preFetchCount;
        });
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        if (!(endpointConfigurator is MassTransit.IRabbitMqReceiveEndpointConfigurator rmq)) return;

        ConfigureDeserializer(rmq);
        ConfigureRetry(rmq);
        BindExchange(rmq);
    }

    private void BindExchange(IRabbitMqReceiveEndpointConfigurator rmq)
    {
        rmq.Bind(_exchange, x =>
        {
            x.RoutingKey = _routingKey;
            x.ExchangeType = _exchangeType;
        });
    }

    private void ConfigureRetry(IRabbitMqReceiveEndpointConfigurator rmq)
    {
        rmq.UseRetry(retry =>
        {
            retry.Incremental(3, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            retry.Ignore<Microsoft.EntityFrameworkCore.DbUpdateException>(e =>
                e.Message.Contains("Cannot insert duplicate key"));
        });
    }

    private void ConfigureDeserializer(IRabbitMqReceiveEndpointConfigurator rmq)
    {
        rmq.AddDeserializer(new ApplicatrionJsonTypeDeserialize());
    }
}