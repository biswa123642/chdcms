using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ComputedMobileImage for Search
    /// </summary>
    public class ComputedMobileImage : IComputedIndexField
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
                    case Templates.ContentPageIdString:
                        return GetMediaUrl(indexableItem);
                }
            }
            return null;
        }

        private string GetMediaUrl(Item item)
        {
            var imgUrl = HelperExtension.GetUrlFromImageField(item, Constants.MobileImage);
            imgUrl = !string.IsNullOrWhiteSpace(imgUrl) ? HelperExtension.GetUrlFromImageField(item, Constants.DesktopImage) : imgUrl;
            return imgUrl;
        }
    }
}