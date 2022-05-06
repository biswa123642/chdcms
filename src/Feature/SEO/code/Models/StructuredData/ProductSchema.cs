using Newtonsoft.Json;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class ProductSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; } = "https://schema.org";
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Product";
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public BrandSchema Brand { get; set; }
        public string Sku { get; set; }
        public string Gtin12 { get; set; }
    }
}