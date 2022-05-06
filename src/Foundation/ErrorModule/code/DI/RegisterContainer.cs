using CGP.Foundation.ErrorModule.Repositiories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Foundation.ErrorModule.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILogger, Logger>();
            serviceCollection.AddSingleton<IAudit, Audit>();
        }
    }
}