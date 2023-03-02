using Domain.Commands;
using Domain.ToDoItems;

namespace Domain.Contracts.Services;

public interface IToDoListService
{
    Task<List<ToDoItemEntity>> GetAll();
    Task<ToDoItemEntity> Create(ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default);
    Task CreateWithQueue(ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default);
}