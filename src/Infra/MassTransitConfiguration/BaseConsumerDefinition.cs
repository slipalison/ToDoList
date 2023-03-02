using System.Net.Mime;
using MassTransit;
using MassTransit.Serialization;

namespace Infra.MassTransitConfiguration;

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

internal class ApplicatrionJsonTypeDeserialize : ISerializerFactory
{
    public IMessageSerializer CreateSerializer()
    {
        return new ApplicatrionJsonDeserialize();
    }

    public IMessageDeserializer CreateDeserializer()
    {
        return new ApplicatrionJsonDeserialize();
    }

    public ContentType ContentType => new("application/json");
}

internal class ApplicatrionJsonDeserialize : SystemTextJsonRawMessageSerializer, IMessageSerializer,
    IMessageDeserializer
{
    public ApplicatrionJsonDeserialize()
    {
    }

    public void Probe(ProbeContext context)
    {
        Task.Delay(1);
    }

    public ConsumeContext Deserialize(ReceiveContext receiveContext)
    {
        //_log 
        return base.Deserialize(receiveContext);
    }

    public SerializerContext Deserialize(MessageBody body, Headers headers, Uri? destinationAddress = null)
    {
        //_log
        return base.Deserialize(body, headers, destinationAddress);
    }

    public MessageBody GetMessageBody<T>(SendContext<T> context) where T : class
    {
        //_log
        return base.GetMessageBody(context);
    }
    public ContentType ContentType => new("application/json");
    
}