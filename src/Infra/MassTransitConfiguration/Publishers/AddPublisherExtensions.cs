using System.Diagnostics.CodeAnalysis;
using Domain.Contracts.Services;
using MassTransit;
using MassTransit.Transports.Fabric;

namespace Infra.MassTransitConfiguration.Publishers;

[ExcludeFromCodeCoverage]
public static class AddPublisherExtensions
{
    private static readonly Dictionary<ExchangeType, string> ExchangeTypes = new()
    {
        { ExchangeType.Direct , "direct"},
        { ExchangeType.Topic , "topic"},
        { ExchangeType.FanOut , "fanout" }
    };

    public static IRabbitMqBusFactoryConfigurator AddDirectPublisher<T>(this IRabbitMqBusFactoryConfigurator configurator, string exchangeName) where T : class, IEventBase
    {
        configurator.AddMassTransitPublisher<T>(exchangeName, ExchangeType.Direct);
        return configurator;
    }
    
    public static IRabbitMqBusFactoryConfigurator AddFanOutPublisher<T>(this IRabbitMqBusFactoryConfigurator configurator, string exchangeName) where T : class, IEventBase
    {
        configurator.AddMassTransitPublisher<T>(exchangeName, ExchangeType.FanOut);
        return configurator;
    }
    
    public static IRabbitMqBusFactoryConfigurator AddTopicPublisher<T>(this IRabbitMqBusFactoryConfigurator configurator, string exchangeName) where T : class, IEventBase
    {
        configurator.AddMassTransitPublisher<T>(exchangeName, ExchangeType.Topic);
        return configurator;
    }

    public static IRabbitMqBusFactoryConfigurator AddMassTransitPublisher<T>(this IRabbitMqBusFactoryConfigurator configurator,
        string exchangeName, ExchangeType exchangeType) where T : class, IEventBase
    {
        configurator.Message<T>(x=> x.SetEntityName(exchangeName));
        configurator.Send<T>(x=> x.UseCorrelationId(p=> p.CorrelationId));
        configurator.Publish<T>(x=> x.ExchangeType = ExchangeTypes[exchangeType]);

        return configurator;
    }
}