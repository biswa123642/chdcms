using Newtonsoft.Json;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class OrganizationSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; } = "https://schema.org";

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Organization";

        public string Name { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }
    }
}