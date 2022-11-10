namespace ProManager.Services
{
    public interface IModelValidator<T> where T : class
    {
        bool IsValid(T model);
    }
}
