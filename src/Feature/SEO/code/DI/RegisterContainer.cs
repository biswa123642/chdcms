using CGP.Feature.SEO.Controllers;
using CGP.Feature.SEO.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.SEO.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<SeoSchemaController>();
            serviceCollection.AddSingleton<ISeoSchemaGenerator, SeoSchemaGenerator>();
        }
    }
}