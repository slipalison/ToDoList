using Domain.Contracts.Repositories;
using Domain.ToDoItems;
using Microsoft.EntityFrameworkCore;

namespace Infra.Databases.SqlServers.ToDoListData.Repositories;

public class ToDoListRepository : IToDoListRepository
{
    private readonly ToDoListContext _context;

    public ToDoListRepository(ToDoListContext toDoListContext)
    {
        _context = toDoListContext;
    }

    public Task<List<ToDoItemEntity>> GetAll()
    {
        return _context.AccountPlanEntities.ToListAsync();
    }

    public async Task<ToDoItemEntity> Create(ToDoItemEntity accountPlanEntity)
    {
        var ent = await _context.AddAsync(accountPlanEntity);
        await _context.SaveChangesAsync();

        return ent.Entity;
    }

}