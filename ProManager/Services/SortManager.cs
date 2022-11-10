using System;
using System.Linq;
using System.Collections.Generic;
using ProManager.Models;
using ProManager.Implementations;
namespace ProManager.Services
{
    public sealed class SortManager : ISortManager
    {
        public IEnumerable<TaskModel> SortByProject(IEnumerable<TaskModel> source)
        {
            return source.OrderBy((model) => model.Project, ProjectComparer.SingleTone).ToList();
        }
        public IEnumerable<TaskModel> SortByCreateDate(IEnumerable<TaskModel> source)
        {
            return source.OrderByDescending(model => model.UpdateDate).ToList();
        }
    }
}
