using System;
using System.Collections.Generic;
using ProManager.Models;
namespace ProManager.Services
{
    public interface ITimeManager
    {
        TimeSpan GetAmountTime(TaskModel model);
        TimeSpan GetTotalTime(IEnumerable<TaskModel> models);
    }
}
