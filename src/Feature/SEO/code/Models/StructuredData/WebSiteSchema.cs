using Newtonsoft.Json;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class WebsiteSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; } = "https://schema.org";

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "WebSite";
        public string Name { get; set; }
        public string Url { get; set; }
        public string Alternatename { get; set; }
        public PotentialAction potentialAction { get; set; }
    }

    public class PotentialAction
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "SearchAction";
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }
        public string Query { get; set; }
    }
    
}