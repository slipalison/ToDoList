using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Databases.SqlServers.ToDoListData;

public class ToDoListDesignContext : IDesignTimeDbContextFactory<ToDoListContext>
{
    public ToDoListContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ToDoListContext>();

        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MsSqlLocalDb;initial catalog=ProductsDbDev;Integrated Security=True; MultipleActiveResultSets=True");

        return new ToDoListContext(optionsBuilder.Options);
    }
}