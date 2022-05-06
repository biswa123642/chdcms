using CGP.Foundation.SitecoreExtensions.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Foundation.SitecoreExtensions.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISiteConfiguration, SiteConfiguration>();
        }
    }
}