using ProManager.ViewModels;
namespace ProManager.Services
{
    public sealed class TaskDtoValidator : IModelValidator<TaskDto>
    {
        public bool IsValid(TaskDto dto)
        {
            return dto.IsProject ? true : !string.IsNullOrWhiteSpace(dto.ProjectName) && dto.CancelDate != default && dto.StartDate != default && dto.CancelDate > dto.StartDate;
        }
    }
}
