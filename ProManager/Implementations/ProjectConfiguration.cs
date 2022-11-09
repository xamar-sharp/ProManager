using System;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ProManager.Implementations
{
    public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasData(new Project() { Id = Guid.Parse("1A1A1A1A-1A1A-1A1A-1A1A-1A1A1A1A1A1A"), ProjectName = "Web-Site", CreateAt = DateTime.Now, UpdateAt = DateTime.Now });
            builder.Property(ent => ent.CreateAt).HasDefaultValueSql("'GETDATE()'");
            builder.Property(ent => ent.UpdateAt).HasDefaultValue("'GETDATE()'");
        }
    }
}
