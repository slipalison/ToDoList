using Domain.ToDoItems;

namespace Domain.Contracts.Repositories;

public interface IToDoListRepository
{
    Task<List<ToDoItemEntity>> GetAll(CancellationToken cancellationToken = default);
    Task<ToDoItemEntity> Create(ToDoItemEntity accountPlanEntity, CancellationToken cancellationToken = default);
}