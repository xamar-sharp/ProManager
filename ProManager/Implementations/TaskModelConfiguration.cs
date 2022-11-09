using System;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ProManager.Implementations
{
    public sealed class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasOne(ent => ent.Project).WithMany(ent => ent.Tasks).HasForeignKey("ProjectId").OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ent => ent.TaskComment).WithOne(ent => ent.Task).HasForeignKey<TaskComment>(comment => comment.TaskId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(ent => ent.StartDate).HasDefaultValueSql("'GETDATE()'");
            builder.Property(ent => ent.UpdateDate).HasDefaultValueSql("'GETDATE()'");
            builder.Property(ent => ent.CreateDate).HasDefaultValueSql("'GETDATE()'");
            builder.HasData(new TaskModel() { Id = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A"), TaskName = "Make Database API", ProjectId = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A"), CreateDate = DateTime.Now, StartDate = DateTime.Now, UpdateDate = DateTime.Now },
                new TaskModel() { Id = Guid.Parse("2A2A2A2A-2A2A-2A2A-2A2A-2A2A2A2A2A2A"), TaskName = "Select site background-image!", ProjectId = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A"), CreateDate = DateTime.Now, StartDate = DateTime.Now, UpdateDate = DateTime.Now });
        }
    }
}
