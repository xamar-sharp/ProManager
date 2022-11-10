using ProManager.ViewModels;
namespace ProManager.Services
{
    public sealed class SortDtoValidator:IModelValidator<SortDto>
    {
        public bool IsValid(SortDto dto)
        {
            return true;
        }
    }
}
