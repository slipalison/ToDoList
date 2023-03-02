using Domain.Contracts.Repositories;
using Domain.ToDoItems;
using Microsoft.EntityFrameworkCore;
using Responses;

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
        return _context.ToDos.ToListAsync(cancellationToken);
    }

    public async Task<ToDoItemEntity> Create(ToDoItemEntity accountPlanEntity,
        CancellationToken cancellationToken = default)
    {
        var ent = await _context.AddAsync(accountPlanEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ent.Entity;
    }

    public async Task<Result> Update(ToDoItemEntity entity, CancellationToken cancellationToken = default)
    {
        var entitie =
            await _context.ToDos.FindAsync(new object?[] { entity.Id },
                cancellationToken: cancellationToken);

        if (entitie == null) return Result.Fail("404", "Tarefa não encontrada");

        entitie.Deadline = entity.Deadline;
        entitie.Name = entity.Name;
        entitie.Status = entity.Status;
        _context.ToDos.Update(entitie);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entitie =
            await _context.ToDos.FindAsync(new object?[] { id },
                cancellationToken: cancellationToken);

        if (entitie == null) return Result.Fail("404", "Tarefa não encontrada");

        _context.ToDos.Remove(entitie);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}