using DataProvider.Extensions;
using DataProvider.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DataNorthwindHttpProvider.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataHttpProvider(this IServiceCollection services)
        {
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ISupplierService, SupplierService>();

            services.AddCaching();

            return services;
        }
    }
}
