using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Infra.MassTransitConfiguration.Consumers;

public class ToDoConsumerDefinition : BaseConsumerDefinition<ToDoConsumer>
{
    public ToDoConsumerDefinition(ILogger<ToDoConsumerDefinition> logger)
        : base(logger, "Create.Exchange", "Create.Routing", "Create.Queue", ExchangeType.Fanout)
    {
    }
}