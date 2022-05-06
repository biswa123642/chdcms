using System.Collections.Generic;
using Newtonsoft.Json;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class FAQPageSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; } = "https://schema.org";
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = "FAQPage";
        public List<Question> MainEntity { get; set; }
    }
    public class Question
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = "Question";

        public string Name { get; set; }

        public AcceptedAnswer AcceptedAnswer { get; set; }
    }
    public class AcceptedAnswer
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; } = "Answer";
        public string Text { get; set; }
    }
}
