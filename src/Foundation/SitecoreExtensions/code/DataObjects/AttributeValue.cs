using Newtonsoft.Json;
using Sitecore.Data;

namespace CGP.Foundation.SitecoreExtensions.DataObjects
{
    public class AttributeValue
    {
        [JsonConverter(typeof(string))]
        public ID Id { get; set; }

        public string Title { get; set; }

        public string Value { get; set; }
    }
}