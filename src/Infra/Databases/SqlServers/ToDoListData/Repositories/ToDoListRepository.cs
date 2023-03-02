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

    public Task<List<ToDoItemEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return _context.AccountPlanEntities.ToListAsync(cancellationToken);
    }

    public async Task<ToDoItemEntity> Create(ToDoItemEntity accountPlanEntity, CancellationToken cancellationToken = default)
    {
        var ent = await _context.AddAsync(accountPlanEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ent.Entity;
    }

}