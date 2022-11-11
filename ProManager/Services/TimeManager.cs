using System;
using System.Linq;
using System.Collections.Generic;
using ProManager.Models;
namespace ProManager.Services
{
    public sealed class TimeManager : ITimeManager
    {
        public TimeSpan GetAmountTime(TaskModel model)
        {
            if (model.CancelDate == default)
            {
                return TimeSpan.Zero;
            }
            return model.CancelDate - model.StartDate;
        }
        public TimeSpan GetTotalTime(IEnumerable<TaskModel> models)
        {
            if(models.Count() == 0)
            {
                return TimeSpan.Zero;
            }
            return models.Select(ent => GetAmountTime(ent)).Aggregate((prev, next) => prev + next);
        }
    }
}
