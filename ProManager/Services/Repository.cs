using System.Reflection;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Services
{
    public sealed class Repository : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public Repository(DbContextOptions<Repository> repos) : base(repos)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}