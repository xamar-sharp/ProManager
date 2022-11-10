using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProManager.Models;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Services
{
    public sealed class ProjectHint : IProjectHint
    {
        private readonly Repository _repos;
        public ProjectHint(Repository repos)
        {
            _repos = repos;
        }
        public async Task<IEnumerable<Project>> DisplayAsync()
        {
            return await _repos.Projects.AsNoTracking().ToListAsync();
        }
    }
}
