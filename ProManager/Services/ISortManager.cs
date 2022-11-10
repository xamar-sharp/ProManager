using System;
using System.Linq;
using System.Collections.Generic;
using ProManager.Models;
namespace ProManager.Services
{
    public interface ISortManager
    {
        IEnumerable<TaskModel> SortByProject(IEnumerable<TaskModel> models);
        IEnumerable<TaskModel> SortByCreateDate(IEnumerable<TaskModel> models);
    }
}
