using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Services
{
    public sealed class Repository : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        private readonly IContentManager _contentManager;
        public Repository(DbContextOptions<Repository> repos, IContentManager contentManager) : base(repos)
        {
            _contentManager = contentManager;
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public async Task<bool> UpdateTaskState(string taskName)
        {
            var task = await FindTaskByName(taskName);
            if (task is null)
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
            var project = await Projects.AsNoTracking().Include(ent => ent.Tasks).FirstOrDefaultAsync(ent => ent.Tasks.Any(task => task.TaskName == taskName));
            if (project is null)
            {
                return false;
            }
            project.UpdateAt = DateTime.Now;
            return await TrySaveChangesAsync();
        }
        public async Task<TaskModel> FindTaskByName(string taskName)
        {
            return await Tasks.AsNoTracking().Include(ent=>ent.Project).Include(ent=>ent.TaskComments).FirstOrDefaultAsync(ent => ent.TaskName == taskName);
        }
        public async Task<TaskComment> FindCommentById(Guid guid)
        {
            return await TaskComments.FindAsync(guid);
        }
        public async Task<Project> FindProjectByName(string projectName)
        {
            return await Projects.AsNoTracking().FirstOrDefaultAsync(ent => ent.ProjectName == projectName);
        }
        public async Task<bool> DeleteComment(Guid id)
        {
            var comment = await FindCommentById(id);
            if (comment is null)
            {
                return false;
            }
            TaskComments.Remove(comment);
            return await TrySaveChangesAsync();
        }
        public async Task<bool> CreateNewProject(string projectName)
        {
            var project = await FindProjectByName(projectName);
            if (project is not null)
            {
                return false;
            }
            project = new Project()
            {
                CreateAt = DateTime.Now,
                ProjectName = projectName,
                UpdateAt = DateTime.Now
            };
            await Projects.AddAsync(project);
            return await TrySaveChangesAsync();
        }
        public async Task<bool> CreateNewTask(string projectName, string taskName, DateTime startDate, DateTime cancelDate)
        {
            var task = await FindTaskByName(taskName);
            if (task is not null)
            {
                return false;
            }
            task = new TaskModel()
            {
                TaskName = taskName,
                StartDate = startDate,
                CancelDate = cancelDate,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Project = await FindProjectByName(projectName)
            };
            await Tasks.AddAsync(task);
            return await TrySaveChangesAsync();
        }
        public async Task<bool> CreateNewTaskComment(string taskName, object content)
        {
            var task = await FindTaskByName(taskName);
            if (task is null)
            {
                return false;
            }
            var bytes = _contentManager.Serialize(content, out bool isFile);
            TaskComment comment = new TaskComment()
            {
                Content = bytes,
                CommentType = isFile ? Convert.ToByte(1) : Convert.ToByte(0),
                Task = task
            };
            await TaskComments.AddAsync(comment);
            return await TrySaveChangesAsync();
        }
        public async Task<bool> UpdateTask(string taskName, DateTime startDate, DateTime cancelDate)
        {
            var task = await FindTaskByName(taskName);
            if (task is null)
            {
                return false;
            }
            task.StartDate = startDate;
            task.CancelDate = cancelDate;
            return await TrySaveChangesAsync();
        }
        public async Task<IList<TaskModel>> GetTasks(int skip, int fetch) => await Tasks.Include(ent => ent.Project).Include(ent => ent.TaskComments).AsNoTracking().Skip(skip).Take(fetch).ToListAsync();
        public async Task<IList<TaskModel>> GetAllTasks() => await Tasks.Include(ent=>ent.Project).Include(ent=>ent.TaskComments).AsNoTracking().ToListAsync();
        private async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}