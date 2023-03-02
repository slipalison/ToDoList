namespace Domain.ToDoItems;

public class ToDoItemEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Status Status { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }


    public static ToDoItemEntity Create(string name, DateTime? deadline)
    {
        return new()
        {
            Name = name, Deadline = deadline, Status = Status.ToDo
        };
    }
}