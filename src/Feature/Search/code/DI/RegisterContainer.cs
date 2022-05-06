using CGP.Feature.Search.Controllers;
using CGP.Feature.Search.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.Search.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<GlobalSearchController>();
            serviceCollection.AddSingleton<IGlobalSearchRepository, GlobalSearchRepository>();
        }
    }
}