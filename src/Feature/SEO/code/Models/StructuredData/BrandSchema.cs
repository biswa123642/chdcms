using Newtonsoft.Json;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class BrandSchema
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Brand";
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}