using System;
using ProManager.Services;
using ProManager.Models;
using ProManager.ViewModels;
namespace ProManager
{
    public static class ModelValidatorTaskExtensions
    {
        public static bool IsValidModel(this IModelValidator<TaskDto> dto, TaskModel model)
        {
            return model.StartDate != default && model.CancelDate != default && model.StartDate <= model.CancelDate;
        }
    }
}
