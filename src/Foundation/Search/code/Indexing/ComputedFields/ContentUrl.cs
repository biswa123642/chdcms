using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the link of content items
    /// </summary>
    public class ContentUrl : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        ILogger logger = new Logger();
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null)
            {
                switch (indexableItem.TemplateID.ToString())
                {
                    case Templates.ProductDetailPageIdString:
                    case Templates.ArticlePageIdString:
                    case Templates.ContentPageIdString:
                        return GetRelativeUrl(indexableItem);
                }
            }
            return null;
        }

        private string GetRelativeUrl(Item item)
        {
            var itemurl = string.Empty;
            try
            {
                SiteInfo currentSite = Factory.GetSiteInfoList().Where(s => s.RootPath != "" && item.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                                                                        .OrderByDescending(s => s.RootPath.Length)
                                                                        .FirstOrDefault();

                var website = Sitecore.Configuration.Factory.GetSite(currentSite.Name);
                using (new SiteContextSwitcher(website))
                {
                    itemurl=  SitecoreUtil.GetItemUrl(item, true);
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError("ContentUrl.GetRelativeUrl() | ", ex);
            }
            return itemurl;
        }
    }
}