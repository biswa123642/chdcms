using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ContentType of individual item
    /// </summary>
    public class ContentType : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);

            if (indexableItem != null)
            {
                switch (indexableItem.TemplateID.ToString())
                {
                    case Templates.ArticlePageIdString:
                        return Translate.Text("Article");
                    case Templates.ProductDetailPageIdString:
                        return Translate.Text("Product");
                    case Templates.ContentPageIdString:
                        return Translate.Text("Other");
                }
            }
            return null;
        }
    }
}