using ProManager.ViewModels;
namespace ProManager.Services
{
    public sealed class TaskDtoValidator : IModelValidator<TaskDto>
    {
        public bool IsValid(TaskDto dto)
        {
            return dto.IsProject ? true : dto.CancelDate != default && dto.CancelDate > dto.StartDate;
        }
    }
}
