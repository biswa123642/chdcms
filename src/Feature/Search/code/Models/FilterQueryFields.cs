namespace CGP.Feature.Search.Models
{
    public class FilterQueryFields
    {
        public FilterQueryFields()
        {
        }

        public FilterQueryFields(string searchTerm, int pageNum, int pagePerRecord, string productType = "", string productCategoryType = "")
        {
            this.SearchTerm = searchTerm;
            this.PageNum = pageNum;
            this.PagePerRecord = pagePerRecord;
            this.ProductType = productType;
            this.ProductCategoryType = productCategoryType;
        }

        public string SearchTerm { get; set; }
        public int PageNum { get; set; }
        public int PagePerRecord { get; set; }
        public string ProductType { get; set; }
        public string ProductCategoryType { get; set; }
    }
}