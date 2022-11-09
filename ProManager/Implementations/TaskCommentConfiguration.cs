using System;
using System.Text;
using System.IO;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ProManager.Implementations
{
    public sealed class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
    {
        public string GetTestImagePath()
        {
            return Path.Combine($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}test-images{Path.DirectorySeparatorChar}", "test_socketwork.jpg");
        }
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.HasData(new TaskComment() { CommentType = 0, Content = Encoding.UTF8.GetBytes("Architecture declaring!"), Id = Guid.NewGuid(), TaskId = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A") },
                new TaskComment() { CommentType = 0, Content = Encoding.UTF8.GetBytes("DAL modeling"), Id = Guid.NewGuid(), TaskId = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A") },
                new TaskComment() { CommentType = 1, Content = File.ReadAllBytes(GetTestImagePath()), Id = Guid.NewGuid(), TaskId = Guid.Parse("2A2A2A2A-2A2A-2A2A-2A2A-2A2A2A2A2A2A") });
        }
    }
}
