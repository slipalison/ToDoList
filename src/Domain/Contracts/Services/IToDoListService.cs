using Domain.Commands;
using Domain.ToDoItems;
using Responses;

namespace Domain.Contracts.Services;

public interface IToDoListService
{
    Task<List<ToDoItemEntity>> GetAll();
    Task<ToDoItemEntity> Create(ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default);
    Task CreateWithQueue(ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default);
    Task<Result<ToDoItemEntity>> Update(Guid id, UpdateCommad updateCommad, CancellationToken cancellationToken);
    Task<Result> Delete(Guid id, CancellationToken cancellationToken);
}