using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.ToDoItems;

namespace Domain.Services;

public class ToDoListService : IToDoListService 
{
    private readonly IToDoListRepository _toDoListRepository;

    public ToDoListService(IToDoListRepository toDoListRepository)
    {
        _toDoListRepository = toDoListRepository;
    }

    public  Task<List<ToDoItemEntity>> GetAll()
    {
        return _toDoListRepository.GetAll();
    }
}