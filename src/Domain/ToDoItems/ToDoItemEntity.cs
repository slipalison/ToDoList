using Domain.Commands;

namespace Domain.ToDoItems;

public class ToDoItemEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Status Status { get; set; } = Status.ToDo;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }


    public static ToDoItemEntity Create(string name, DateTime? deadline)
    {
        return new()
        {
            Name = name, Deadline = deadline, Status = Status.ToDo
        };
    }
    
    public static ToDoItemEntity Update(UpdateCommad updateCommad, Guid id)
    {
        return new()
        {
            Name = updateCommad.Name, Deadline = updateCommad.Deadline, Status = updateCommad.Status, Id = id
        };
    }
}