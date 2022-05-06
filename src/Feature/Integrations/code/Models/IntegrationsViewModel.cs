using Newtonsoft.Json;

namespace CGP.Feature.Integrations.Models
{
    public class IntegrationsViewModel
    {
        public WhereToBuy WhereToBuy { get; set; }
        public bool EnableWhereToBuy { get; set; }
        public string PowerReviews { get; set; }
        public bool EnablePowerReviews { get; set; }
        public bool IsProductDetailPage { get; set; }
    }
    public class WhereToBuy
    {
        public string Account { get; set; }
        public string Config { get; set; }
        public string Country { get; set; }
    }
}