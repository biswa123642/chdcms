using CGP.Feature.ProductVariant.Controllers;
using CGP.Feature.ProductVariant.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.ProductVariant.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ProductVariantController>();
            serviceCollection.AddSingleton<IProductVariantRepository, ProductVariantRepository>();
        }
    }
}