using Microsoft.AspNetCore.Http;
namespace ProManager.Services
{
    public interface ITaskNavigator
    {
        int TasksLoaded { get; }
        void IncreaseLoadedCount(HttpContext ctx, int count);
    }
}
