using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.Integrations.Models
{
    public class PowerReviews
    {
        [JsonProperty(PropertyName = "api_key")]
        public string Apikey { get; set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        [JsonProperty(PropertyName = "merchant_group_id")]
        public string MerchantGroupID { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public string MerchantID { get; set; }

        [JsonProperty(PropertyName = "page_id")]
        public string PageID { get; set; }

        [JsonProperty(PropertyName = "review_wrapper_url")]
        public string ReviewWrapperURL { get; set; }

        [JsonProperty(PropertyName = "sendProductInfo")]
        public string SendProductInfo { get; set; }

        [JsonProperty(PropertyName = "components")]
        public Components Components { get; set; }

    }
    public class Components
    {
        public string ReviewDisplay { get; set; }
        public string ReviewSnippet { get; set; }
        public string Write { get; set; }
    }

}