using RabbitMQ.Client;

namespace Infra.MassTransitConfiguration.Consumers;

public class ToDoConsumerDefinition : BaseConsumerDefinition<ToDoConsumer>
{
    public ToDoConsumerDefinition()
        : base( "Create.Exchange", "Create.Routing", "Create.Queue", ExchangeType.Fanout)
    {
    }
}