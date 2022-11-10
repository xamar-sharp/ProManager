using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
namespace ProManager.Services
{
    public sealed class TaskNavigator : ITaskNavigator
    {
        private readonly ConfigurationMap _config;
        public TaskNavigator(IOptions<ConfigurationMap> config)
        {
            _config = config.Value;
        }
        public int TasksLoaded { get; private set; }
        public void IncreaseLoadedCount(HttpContext ctx, int count)
        {
            int counter = TasksLoaded + count;
            ctx.Response.Cookies.Append(_config.CounterCookie, counter.ToString());
            TasksLoaded = counter;
        }
    }
}
