using Domain.ToDoItems;
using Infra.Databases.SqlServers.ToDoListData.Configurations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Infra.Databases.SqlServers.ToDoListData;

public class ToDoListContext : DbContext
{
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options)
    {
    }

    public DbSet<ToDoItemEntity> AccountPlanEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ToDoItemEntityConfiguration());
    }
}