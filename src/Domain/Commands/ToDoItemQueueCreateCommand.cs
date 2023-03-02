using Domain.Contracts.Services;

namespace Domain.Commands;

public class ToDoItemQueueCreateCommand : ToDoItemCreateCommand,  IEventBase
{
    public Guid CorrelationId { get; set; }
    public string RoutingKey => "Create.Routing";
 
    public static ToDoItemQueueCreateCommand Create(ToDoItemCreateCommand createCommand, Guid correlationId)
    {
        return new()
        {
            CorrelationId = correlationId, Deadline = createCommand.Deadline, Name = createCommand.Name
        };
    }
}