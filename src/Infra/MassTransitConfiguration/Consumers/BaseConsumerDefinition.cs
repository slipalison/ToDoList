using MassTransit;
using MassTransit.Serialization;
using Microsoft.Extensions.Logging;

namespace Infra.MassTransitConfiguration.Consumers;

public abstract class BaseConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    private readonly string _exchange;
    private readonly string _routingKey;
    private readonly string _exchangeType;
    private readonly ApplicatrionJsonTypeDeserialize _serialize;

    protected BaseConsumerDefinition(ILogger logger, string exchange, string routingKey, string endipoint,
        string exchangeType = RabbitMQ.Client.ExchangeType.Direct)
    {
        _exchange = exchange;
        _routingKey = routingKey;
        EndpointName = endipoint;
        _exchangeType = exchangeType;
        _serialize = new ApplicatrionJsonTypeDeserialize(logger);

        Endpoint(x =>
        {
            var preFetchCount = 50;
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
        rmq.UseRawJsonSerializer(RawSerializerOptions.All);
        rmq.AddDeserializer(_serialize);
        rmq.AddSerializer(_serialize);
    }
}