using CGP.Feature.ProductImageGallery.Controllers;
using CGP.Feature.ProductImageGallery.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.ProductImageGallery.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ProductImageGalleryController>();
            serviceCollection.AddSingleton<IProductImageGalleryRepository, ProductImageGalleryRepository>();
        }
    }
}