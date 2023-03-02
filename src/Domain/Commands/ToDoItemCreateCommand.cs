using Domain.ToDoItems;

namespace Domain.Commands;

public class ToDoItemCreateCommand
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
}

public class UpdateCommad : ToDoItemCreateCommand
{
    public Status Status { get; set; }
}