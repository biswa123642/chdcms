using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sitecore.Data.Items;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class BreadCrumbListSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; } = "https://schema.org";

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "BreadcrumbList";

        public List<ListItem> ItemListElement { get; set; }
    }
    public class ListItem
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = "ListItem";

        public string Position { get; set; }

        public string Name { get; set; }

        public string Item { get; set; }
    }
}