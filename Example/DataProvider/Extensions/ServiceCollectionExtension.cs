using DataProvider.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace DataProvider.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddTransient<ICacheService, LocalCacheService>();

            return services;
        }
    }
}
