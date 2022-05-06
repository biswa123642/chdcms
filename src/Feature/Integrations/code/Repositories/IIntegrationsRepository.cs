using Newtonsoft.Json;

namespace CGP.Feature.Integrations.Repositories
{
    public interface IIntegrationsRepository
    {
        string GetPowerReviewIntegrationSettings();
    }
}
