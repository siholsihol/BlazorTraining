using DataDummyProvider.Services;
using DataProvider.Extensions;
using DataProvider.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DataDummyProvider.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataDummyProvider(this IServiceCollection services)
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
