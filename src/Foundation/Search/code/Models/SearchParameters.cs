using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Models
{
    public class SearchParameters
    {
        public Item CurrentItem { get; set; }
        public string SiteName { get; set; }
        public string IndexName { get; set; }
        public bool? IncludeOtherBrands { get; set; }
    }
}