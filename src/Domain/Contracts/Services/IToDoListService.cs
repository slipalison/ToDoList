using Domain.ToDoItems;

namespace Domain.Contracts.Services;

public interface IToDoListService
{
    Task<List<ToDoItemEntity>> GetAll();
}