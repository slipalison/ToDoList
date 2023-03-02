using Domain.ToDoItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.Databases.SqlServers.ToDoListData.Configurations;

public class ToDoItemEntityConfiguration : IEntityTypeConfiguration<ToDoItemEntity>
{
    public void Configure(EntityTypeBuilder<ToDoItemEntity> builder)
    {
        builder.ToTable("ToDoList");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnType("nvarchar(250)").IsRequired();
        builder.Property(x => x.Status)
            .HasConversion(new EnumToStringConverter<Status>())
            .HasColumnType("nvarchar(100)").IsRequired();

        builder.Property(x => x.CreateAt).HasColumnType("datetime2").IsRequired();
        builder.Property(x => x.Deadline).HasColumnType("datetime2");
        

        // builder.HasData(Return());
    }
    
}