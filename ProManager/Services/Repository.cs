using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        public async Task<bool> UpdateTaskState(string taskName)
        {
            var task = await Tasks.AsNoTracking().FirstOrDefaultAsync(ent => ent.TaskName == taskName);
            if(task is null)
            {
                return false;
            }
            task.UpdateDate = DateTime.Now;
            try
            {
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return await UpdateProjectState(taskName);
        }
        public async Task<bool> UpdateProjectState(string taskName)
        {
            var project = await Projects.AsNoTracking().Include(ent=>ent.Tasks).FirstOrDefaultAsync(ent => ent.Tasks.Any(task => task.TaskName == taskName));
            if(project is null)
            {
                return false;
            }
            project.UpdateAt = DateTime.Now;
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}