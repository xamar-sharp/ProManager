using ProManager.ViewModels;
namespace ProManager.Services
{
    public sealed class TaskCommentDtoValidator : IModelValidator<TaskCommentDto>
    {
        public bool IsValid(TaskCommentDto dto)
        {
            return dto.File != null || !string.IsNullOrWhiteSpace(dto.Text);
        }
    }
}
