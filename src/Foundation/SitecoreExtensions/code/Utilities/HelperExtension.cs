using Sitecore.Sites;
using System;
using Sitecore;
using Sitecore.Data.Items;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore.Data;
using System.Linq;
using CGP.Foundation.SitecoreExtensions.Constants;
using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public static class HelperExtension
    {
        public static Logger _logger = new Logger();

        /// <summary>
        /// Get Context Site
        /// </summary>
        /// <returns></returns>
        public static SiteContext GetContextSite()
        {
            if (Context.PageMode.IsExperienceEditor || Context.PageMode.IsPreview)
            {
                // item ID for page editor and front-end preview mode
                string id = Sitecore.Web.WebUtil.GetQueryString("sc_itemid");

                // by default, get the item assuming Presentation Preview tool (embedded preview in shell)
                var item = Context.Item;

                // if a query string ID was found, get the item for page editor and front-end preview mode
                if (!string.IsNullOrEmpty(id))
                {
                    item = Context.Database.GetItem(id);
                }

                // loop through all configured sites
                foreach (var site in Sitecore.Configuration.Factory.GetSiteInfoList())
                {
                    // get this site's home page item
                    var homePage = Context.Database.GetItem(site.RootPath + site.StartItem);

                    // if the item lives within this site, this is our context site
                    if (homePage != null && item != null && homePage.Axes.IsAncestorOf(item))
                    {
                        return Sitecore.Configuration.Factory.GetSite(site.Name);
                    }
                }

                // fallback and assume context site
                return Context.Site;
            }
            else
            {
                // standard context site resolution via hostname, virtual/physical path, and port number
                return Context.Site;
            }
        }

        /// <summary>
        /// Get item details by item id or path
        /// </summary>
        /// <param name="item">Passing sitecore item id or path</param>
        /// <returns>Return item</returns>
        public static Item GetItem(string item)
        {
            try
            {
                if (string.IsNullOrEmpty(item) && Context.Database == null)
                { return null; }
                return Context.Database.GetItem(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in HelperExtension.GetItem()", ex);
                return null;
            }
        }
        /// <summary>
        /// Get sitecore root item
        /// </summary>
        /// <returns>Return sitecore item</returns>
        public static Item GetSiteRootItem()
        {
            return GetItem(Context.Site.RootPath);
        }

        /// <summary>
        /// Get child item by parent item context and template id
        /// </summary>
        /// <param name="item">Passing sitecore parent item</param>
        /// <param name="templateId">Passing child template id</param>
        /// <returns>Return child item</returns>
        public static Item GetChildItem(Item item, ID templateId)
        {
            try
            {
                return item.Children.Where(x => x.TemplateID.Equals(templateId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in HelperExtension.GetSiteSettingItem()", ex);
                return null;
            }
        }

        public static Item GetHomeItem()
        {
            return GetItem(Context.Site.StartPath);
        }
        public static string GetHostName()
        {
            return Sitecore.Links.LinkManager.GetItemUrl(GetHomeItem(), new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
        }
        public static Item GetSiteConfigurationItem(ID iD)
        {
            Logger logger = new Logger();
            try
            {
                var currentRootItem = HelperExtension.GetSiteRootItem();

                if (currentRootItem != null)
                {
                    var settingItem = HelperExtension.GetChildItem(currentRootItem, Templates.SiteSettingTemplateId);
                    if (settingItem != null)
                    {
                        var brandSettingItem = HelperExtension.GetChildItem(settingItem, iD);
                        return brandSettingItem != null ? brandSettingItem : null;
                    }
                    else
                    { return null; }

                }
                else { return null; }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in HelperExtension.GetSiteConfigurationItem() ", ex);
                return null;
            }
        }
        /// <summary>
        ///  Gets product variants on the basis of variant grouping item
        /// </summary>
        /// <param name="variantGroupingItem"></param>
        /// <returns></returns>
        public static List<Item> GetProductVariants(Item variantGroupingItem)
        {
            Logger logger = new Logger();
            List<Item> variantList = new List<Item>();
            try
            {
                if (variantGroupingItem != null)
                {
                    MultilistField selectedProductVariant = variantGroupingItem.Fields[Constants.Constants.ChooseVariant];

                    if (selectedProductVariant.GetItems().Length < 1)
                    {
                        variantList = variantGroupingItem.Children.ToList();
                    }
                    else
                    {
                        variantList = selectedProductVariant.GetItems().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in HelperExtension.GetProductVariants() ", ex);
            }
            return variantList;
        }

        public static string GetUrlFromImageField(Item item, string fieldName)
        {
            string mediaUrl = string.Empty;
            try
            {
                if (item != null && !string.IsNullOrWhiteSpace(fieldName))
                {
                    ImageField imageField = item.Fields[fieldName];
                    if (imageField != null && imageField.MediaItem != null)
                    {
                        mediaUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem);
                        mediaUrl = mediaUrl.Contains("/sitecore/shell") ? mediaUrl.Replace("/sitecore/shell", string.Empty) : mediaUrl;
                        mediaUrl = HashingUtils.ProtectAssetUrl(mediaUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR in HelperExtension.GetUrlFromImageField() |", ex);
            }
            return mediaUrl;
        }
    }
}