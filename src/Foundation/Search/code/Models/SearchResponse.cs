using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Models
{
    public class SearchResponse
    {
        public Item CurrentItem { get; set; }
        public string SearchTerm { get; set; }
        public int TotalResultCount { get; set; }
        public int CurrentResultCount { get; set; }
        public int IteratedResultCount { get; set; }
        public int RemainingResultCount { get; set; }
    }
}