namespace Domain.Commands;

public class ToDoItemCreateCommand
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
}