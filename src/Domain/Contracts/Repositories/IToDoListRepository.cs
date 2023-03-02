using Domain.ToDoItems;

namespace Domain.Contracts.Repositories;

public interface IToDoListRepository
{
    Task<List<ToDoItemEntity>> GetAll();
    Task<ToDoItemEntity> Create(ToDoItemEntity accountPlanEntity);
}