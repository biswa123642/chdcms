using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public static class SitecoreUtil
    {
        public static Logger _logger = new Logger();
        private static Sitecore.Data.Database _database = Sitecore.Context.Database;
        #region public static methods

        /// <summary>
        /// GetChildren() method used to get all the children of the item whose ItemId is being passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static List<Item> GetChildren(this Item item, ID childTemplateId, Language lng = null)
        {
            if (item != null)
            {
                if (item.Children != null)
                {
                    lng = lng == null ? Context.Language : lng;
                    if (ID.IsNullOrEmpty(childTemplateId))
                    {
                        return item.Children.Where(child => !child.TemplateID.Equals("FolderTemplateID")
                                                           && child.Language == lng && child.Versions.Count > 0).ToList();
                    }
                    else
                    {
                        return item.Children.Where(child => child.TemplateID.Equals(childTemplateId)
                                                         && child.Language == lng && child.Versions.Count > 0).ToList();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get Current Sitecore Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Item GetItem(ID id, Database db = null, Language lang = null)
        {
            Item item = db == null ? ((lang == null) ? Context.Database.GetItem(id) : Context.Database.GetItem(id, lang)) : ((lang == null) ? db.GetItem(id) : db.GetItem(id, lang));
            return item;
        }

        /// <summary>
        /// Returns true if the passed item has a version in language: languageName
        /// </summary>
        /// <param name="item"></param>
        /// <param name="contextLanguage"></param>
        /// <returns></returns>
        public static bool HasLanguageVersion(this Item item, Language contextLanguage, Database db = null)
        {
            if (item != null && contextLanguage != null)
            {
                try
                {
                    Item languageSpecificItem = null;
                    if (item.Language == contextLanguage)
                    {
                        languageSpecificItem = item;
                    }
                    if (db == null && Context.Database != null)
                    {
                        languageSpecificItem = Context.Database.GetItem(item.ID, contextLanguage);
                    }
                    else if (db != null)
                    {
                        languageSpecificItem = db.GetItem(item.ID, contextLanguage);
                    }
                    if (languageSpecificItem != null && languageSpecificItem.Versions != null && languageSpecificItem.Versions.Count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in SitecoreUtil.HasLanguageVersion()", ex);
                    return false;
                }
            }
            return false;
        }
        public static bool HasLanguageVersion(this Item item, Language contextLanguage, out Item languageSpecificItem, Database db = null)
        {
            languageSpecificItem = null;

            if (item != null && contextLanguage != null)
            {
                try
                {
                    if (item.Language == contextLanguage)
                    {
                        languageSpecificItem = item;
                    }

                    if (db == null && Context.Database != null)
                    {
                        languageSpecificItem = Context.Database.GetItem(item.ID, contextLanguage);
                    }
                    else if (db != null)
                    {
                        languageSpecificItem = db.GetItem(item.ID, contextLanguage);
                    }
                    if (languageSpecificItem != null && languageSpecificItem.Versions != null && languageSpecificItem.Versions.Count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in SitecoreUtil.HasLanguageVersion()", ex);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Get item Short ID for Search
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>string</returns>
        public static string GetItemShortId(ID itemId)
        {
            return itemId.ToShortID().ToString().ToLower();
        }

        /// <summary>
        /// GetItem() used to get the Sitecore Item whose ItemId(in string format) is being passed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Item GetItem(string id, Database db = null)
        {
            Item item = null;

            if (!string.IsNullOrWhiteSpace(id) && (ID.IsID(id) || ShortID.IsShortID(id)))
            {
                if (db != null)
                {
                    item = db.GetItem(new ID(id));
                }
                else if (Context.Database != null)
                {
                    item = Context.Database.GetItem(new ID(id));
                }
            }
            return item;
        }

        /// <summary>
        /// function is used to get media item Url by link type
        /// </summary>
        /// <param name="linkField">object of link field</param>
        /// <returns></returns>
        public static string GetLinkUrl(LinkField linkField)
        {
            try
            {
                if (linkField != null)
                {
                    string videoUrl = string.Empty;
                    string linkType = linkField.LinkType.ToLower();
                    switch (linkType)
                    {
                        case "internal":
                            // Use LinkMananger for internal links, if link is not empty
                            return linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                        case "media":
                            // Use MediaManager for media links, if link is not empty
                            var url = linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                            return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);
                        case "external":
                            // Just return external links
                            if (!string.IsNullOrEmpty(linkField.Url))
                            {
                                videoUrl = linkField.Url;
                                if (videoUrl.Trim().ToLower().Contains("www.youtube.com/embed"))
                                {
                                    videoUrl = videoUrl.Trim().Replace("www.youtube.com/embed", "www.youtube-nocookie.com/embed");
                                }
                            }
                            return videoUrl;
                        case "anchor":
                            // Prefix anchor link with # if link if not empty
                            return !string.IsNullOrEmpty(linkField.Url) ? "#" + linkField.Anchor : string.Empty;
                        case "mailto":
                            // Just return mailto link
                            return linkField.Url;
                        case "javascript":
                            // Just return javascript
                            return linkField.Url;
                        default:
                            // Just please the compiler, this
                            // condition will never be met
                            return linkField.Url;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.HasLanguageVersion()", ex);
                return string.Empty;
            }
            return string.Empty;
        }


        /// <summary>
        /// function is used to get media item Url by link type
        /// </summary>
        /// <param name="linkField">object of link field</param>
        /// <returns></returns>
        public static string GetVideoLink(LinkField linkField)
        {
            try
            {
                if (linkField != null)
                {
                    string videoUrl = string.Empty;
                    string linkType = linkField.LinkType.ToLower();
                    switch (linkType)
                    {
                        case "internal":
                            // Use MediaManager for internal video links, if link is not empty
                            MediaItem video = Sitecore.Context.Database.GetItem(linkField.TargetID);
                            return Sitecore.Resources.Media.MediaManager.GetMediaUrl(video);
                        case "media":
                            // Use MediaManager for media links, if link is not empty
                            var url = linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                            return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);
                        case "external":
                            // Just return external links
                            if (!string.IsNullOrEmpty(linkField.Url))
                            {
                                videoUrl = linkField.Url;
                                if (videoUrl.Trim().ToLower().Contains("www.youtube.com/embed"))
                                {
                                    videoUrl = videoUrl.Trim().Replace("www.youtube.com/embed", "www.youtube-nocookie.com/embed");
                                }
                            }
                            return videoUrl;
                        case "anchor":
                            // Prefix anchor link with # if link if not empty
                            return !string.IsNullOrEmpty(linkField.Url) ? "#" + linkField.Anchor : string.Empty;
                        case "mailto":
                            // Just return mailto link
                            return linkField.Url;
                        case "javascript":
                            // Just return javascript
                            return linkField.Url;
                        default:
                            // Just please the compiler, this
                            // condition will never be met
                            return linkField.Url;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.GetVideoLink()", ex);
                return string.Empty;
            }

            return string.Empty;
        }
        /// <summary>
        /// function is used to get media item Url by link type
        /// </summary>
        /// <param name="target">target of link</param>
        /// <param name="linkField">object of link field</param>
        /// <returns></returns>
        public static string GetLinkUrl(out string target, LinkField linkField)
        {
            target = "_self";
            try
            {
                if (linkField != null)
                {
                    string videoUrl = string.Empty;
                    string linkType = linkField.LinkType.ToLower();
                    switch (linkType)
                    {
                        case "internal":
                            // Use LinkMananger for internal links, if link is not empty
                            return linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                        case "media":
                            // Use MediaManager for media links, if link is not empty
                            var url = linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                            return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);
                        case "external":
                            // Just return external links
                            target = "_blank";
                            if (!string.IsNullOrEmpty(linkField.Url))
                            {
                                videoUrl = linkField.Url;
                                if (videoUrl.Trim().ToLower().Contains("www.youtube.com/embed"))
                                {
                                    videoUrl = videoUrl.Trim().Replace("www.youtube.com/embed", "www.youtube-nocookie.com/embed");
                                }
                            }
                            return videoUrl;
                        case "anchor":
                            // Prefix anchor link with # if link if not empty
                            return !string.IsNullOrEmpty(linkField.Url) ? "#" + linkField.Anchor : string.Empty;
                        case "mailto":
                            // Just return mailto link
                            return linkField.Url;
                        case "javascript":
                            // Just return javascript
                            return linkField.Url;
                        default:
                            // Just please the compiler, this
                            // condition will never be met
                            return linkField.Url;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.HasLanguageVersion()", ex);
                return string.Empty;
            }

            return string.Empty;
        }
        /// <summary>
        /// function is used to get media item Url by link type
        /// </summary>
        /// <param name="linkField">object of link field</param>
        /// <param name="linkTitle"> </param>
        /// <returns></returns>
        public static string GetLinkUrl(LinkField linkField, out string linkTitle)
        {
            linkTitle = string.Empty;
            try
            {
                if (linkField != null)
                {
                    string videoUrl = string.Empty;
                    var linkType = linkField.LinkType.ToLower();
                    linkTitle = linkField.Title;

                    if (!string.IsNullOrWhiteSpace(linkTitle))
                    {
                        linkTitle = linkTitle.Replace(@"\", "\\");
                    }

                    switch (linkType)
                    {
                        case "internal":
                            // Use LinkMananger for internal links, if link is not empty
                            if (linkField.TargetItem != null)
                            {
                                //linkTitle = SitecoreUtil.ItemTitle(linkField.TargetItem);
                            }
                            return linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                        case "media":
                            // Use MediaManager for media links, if link is not empty
                            var url = linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                            return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);

                        case "external":
                            // Just return external links
                            if (!string.IsNullOrEmpty(linkField.Url))
                            {
                                videoUrl = linkField.Url;
                                if (videoUrl.Trim().ToLower().Contains("www.youtube.com/embed"))
                                {
                                    videoUrl = videoUrl.Trim().Replace("www.youtube.com/embed", "www.youtube-nocookie.com/embed");
                                }
                            }
                            return videoUrl;
                        case "anchor":
                            // Prefix anchor link with # if link if not empty
                            return !string.IsNullOrEmpty(linkField.Url) ? "#" + linkField.Anchor : string.Empty;
                        case "mailto":
                            // Just return mailto link
                            return linkField.Url;
                        case "javascript":
                            // Just return javascript
                            return linkField.Url;
                        default:
                            // Just please the compiler, this
                            // condition will never be met
                            return linkField.Url;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.HasLanguageVersion()", ex);
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// get url from Item's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetItemUrlById(string id)
        {
            Item item = GetItem(id);
            return GetItemUrl(item);
        }
        /// <summary>
        /// get url from sitecore item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>url</returns>
        public static string GetItemUrl(Item item, bool includeServerUrl = false)
        {
            string url = string.Empty;

            try
            {
                if (item != null)
                {
                    var options = LinkManager.GetDefaultUrlBuilderOptions();
                    options.AlwaysIncludeServerUrl = false;

                    if (includeServerUrl)
                    {
                        options.AlwaysIncludeServerUrl = true;
                    }
                    url = LinkManager.GetItemUrl(item, options);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.GetItemUrl()", ex);
            }
            return url;
        }

        /// <summary>
        /// get context Sitecore Language
        /// </summary>
        /// <returns>language string</returns>
        public static string GetContextLanguage()
        {
            return Context.Language.CultureInfo.ToString();
        }

        /// <summary>
        /// Returns the url of the media item of which the ImageField is passed
        /// </summary>
        /// <param name="imageField"></param>
        /// <returns></returns>
        public static string GetMediaItemUrl(ImageField imageField)
        {
            if (imageField != null)
            {
                if (imageField.MediaItem != null)
                {
                    var url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem);
                    return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the url of the media item of which the ImageField is passed
        /// </summary>
        /// <param name="imageField"></param>
        /// <param name="alt"> </param>
        /// <returns></returns>
        public static string GetMediaItemUrl(ImageField imageField, out string alt)
        {
            alt = string.Empty;
            if (imageField != null)
            {
                if (imageField.MediaItem != null)
                {
                    alt = imageField.Alt;

                    var url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem);
                    return Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(url);
                }
            }
            return string.Empty;
        }

        public static string GetMediaImage(Item item, ImageField imageField, string fieldValue, out string alt, bool isEditable = false)
        {
            string image = string.Empty;
            alt = string.Empty;
            if (Context.PageMode.IsExperienceEditorEditing && isEditable)
            {
                if (item != null && !string.IsNullOrWhiteSpace(fieldValue))
                {
                    image = FieldRenderer.Render(item, fieldValue);
                }
            }
            else
            {
                if (imageField != null)
                {
                    string imageAlt;
                    image = GetMediaItemUrl(imageField, out imageAlt);
                    alt = imageAlt;
                }
            }
            return image;
        }

        /// <summary>
        ///  Checks Item is navigational or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool DoesSitecoreItemHavePresentation(this Item item)
        {
            bool isHavePresentation = item.Fields[FieldIDs.LayoutField] != null
                && item.Fields[FieldIDs.LayoutField].Value != String.Empty;
            if (isHavePresentation)
            {
                LayoutItem layoutItem = item.Visualization.GetLayout(Context.Device);
                if (layoutItem != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static string RemoveHtmlFromTitle(string title)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    title = Regex.Replace(title = WebUtility.HtmlDecode(title), @"<[^>]+>|&nbsp;", "").Trim();
                    // To Remove other special characters use this (Remove all except .,@, and -) Regex.Replace(title, @"[^\w\.@-]", "", RegexOptions.None); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.RemoveHtmlFromTitle()", ex);
                return string.Empty;
            }
            return title;
        }
        public static bool IsValidItemForMenu(Item item)
        {
            bool valid = true;
            try
            {
                if (!item.HasChildren && !item.DoesSitecoreItemHavePresentation())
                {
                    valid = false;
                }
                else if (!item.Children.Any(x => x.DoesSitecoreItemHavePresentation()))
                {
                    valid = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.IsValidItemForMenu()", ex);
                return valid;
            }
            return valid;
        }

        /// <summary>
        /// this function publish the items ant their subitems programatically
        /// </summary>
        /// <param name="item"> the item from sitecore</param>
        /// <param name="isParent"></param>
        /// <param name="publishToProduction"></param>
        /// <returns></returns>
        public static string PublishItemWithSubItems(Sitecore.Data.Items.Item item, bool isParent = false, bool publishToProduction = false, bool publishRelatedItems = true)
        {
            List<string> database = new List<string>();

            if (item != null)
            {
                try
                {
                    var webDatabasePresent = WebConfigurationManager.AppSettings["PublishingTarget-CA"];
                    var cdWebDatabasePresent = WebConfigurationManager.AppSettings["PublishingTarget-CD"];
                    if (!String.IsNullOrEmpty(webDatabasePresent))
                    {
                        database.Add(webDatabasePresent);
                    }

                    if (publishToProduction && !String.IsNullOrEmpty(cdWebDatabasePresent))
                    {
                        database.Add(cdWebDatabasePresent);
                    }

                    foreach (var databaseType in database)
                    {
                        // The publishOptions determine the source and target database,
                        // the publish mode and language, and the publish date
                        Sitecore.Publishing.PublishOptions publishOptions =
                          new Sitecore.Publishing.PublishOptions(item.Database,
                                                                 Database.GetDatabase(databaseType),
                                                                 Sitecore.Publishing.PublishMode.SingleItem,
                                                                 item.Language,
                                                                 System.DateTime.Now);  // Create a publisher with the publishoptions
                        Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

                        // Choose where to publish from
                        publisher.Options.RootItem = item;

                        if (!isParent)
                        {
                            // Publish children as well?
                            publisher.Options.Deep = true;

                            if (publishRelatedItems)
                            {
                                publisher.Options.PublishRelatedItems = true;
                            }
                        }

                        // Do the publish!
                        publisher.Publish();
                        Sitecore.Diagnostics.Log.Info("---- Publish for database and item ----" + databaseType + item, typeof(SitecoreUtil));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in SitecoreUtil.PublishItemWithSubItems()", ex);
                    return string.Empty;
                }
            }
            return "";
        }
        public static string GetRenderingDataSourceItemId()
        {
            Sitecore.Mvc.Presentation.Rendering renderingItem = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering;
            return renderingItem.DataSource;
        }
        public static string GetRenderingDataSourceItemId(out string cssClass)
        {
            Sitecore.Mvc.Presentation.Rendering renderingItem = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering;
            cssClass = renderingItem.Parameters["cssClass"];
            return renderingItem.DataSource;
        }

        /// <summary>
        /// Create Sitecore item for global forlder - Redirect module
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="redirectModuleTemplate"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static Item CreateItem(Item parentItem, TemplateItem redirectModuleTemplate, string itemName)
        {
            Item itm = null;
            try
            {
                using (new SecurityDisabler())
                {
                    if (parentItem != null)
                    {
                        //Now we can add the new item as a child to the parent
                        itm = parentItem.Add(itemName, redirectModuleTemplate);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.CreateItem()", ex);
                return itm;
            }
            return itm;
        }

        /// <summary>
        /// Update Redirect To External Link
        /// </summary>
        /// <param name="createdItem"></param>
        /// <param name="requestedUrl"></param>
        /// <param name="redirectToExternalLink"></param>
        public static void UpdateRedirectToExternalLink(Item createdItem, string requestedUrl, string redirectToExternalLink)
        {
            try
            {
                //Again we need to handle security
                //In this example we just disable it
                using (new SecurityDisabler())
                {
                    if (createdItem != null)
                    {
                        createdItem.Editing.BeginEdit();
                        LinkField linkField = new LinkField(createdItem.Fields["RedirectToExternalLink"]);
                        linkField.LinkType = "external";
                        linkField.Url = redirectToExternalLink;
                        createdItem["RequestedUrl"] = requestedUrl;
                        createdItem.Editing.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.UpdateRedirectToExternalLink()", ex);
            }
        }

        /// <summary>
        /// Create Item in Specific Language
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="redirectModuleTemplate"></param>
        /// <param name="itemName"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Item CreateItemInSpecificLanguage(Item parentItem, TemplateItem redirectModuleTemplate, string itemName, Language language)
        {
            Database master = Database.GetDatabase("master");
            Item itm = null;
            if (master != null)
            {
                try
                {
                    using (new LanguageSwitcher(language))
                    {
                        var rootItem = master.GetItem(parentItem.ID);
                        if (rootItem != null)
                        {
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                itm = rootItem.Add(itemName, redirectModuleTemplate);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in SitecoreUtil.UpdateRedirectToExternalLink()", ex);
                    return itm;
                }
            }
            return itm;
        }

        /// <summary>
        /// Mark Item As Never Publish If All Language Versions Are Hidden
        /// </summary>
        /// <param name="item"></param>
        public static bool MarkItemAsNeverPublishIfAllLanguageVersionsAreHidden(Item item)
        {
            Database master = Database.GetDatabase("master");
            bool isHideVersionChecked = false;
            bool isItemToBePublished = false;
            try
            {
                if (master != null)
                {
                    Item langversionNeverPublish = null;
                    foreach (Language lang in item.Languages)
                    {
                        var langitem = master.GetItem(item.ID, lang);
                        if (langitem.Versions.Count > 0)
                        {
                            if (langitem.Fields["__Hide version"] != null)
                            {
                                CheckboxField hideVersion = langitem.Fields["__Hide version"];
                                if (hideVersion.Checked == true)
                                {
                                    langversionNeverPublish = langitem;
                                    isHideVersionChecked = true;
                                    continue;
                                }
                                else
                                {
                                    isHideVersionChecked = false;
                                    break;
                                }
                            }

                        }
                    }
                    if (isHideVersionChecked)
                    {
                        langversionNeverPublish.Editing.BeginEdit();
                        langversionNeverPublish.Publishing.NeverPublish = true;
                        langversionNeverPublish.Editing.EndEdit();
                        isItemToBePublished = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.MarkItemAsNeverPublishIfAllLanguageVersionsAreHidden()", ex);
                return isItemToBePublished;
            }

            return isItemToBePublished;
        }

        /// <summary>
        /// For Getting Canonical Url of Item
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        public static string GetCanonicalUrlForSimilarContent(Item itm)
        {
            string canonicalUrl = string.Empty;
            try
            {
                if (itm.Fields["CanonicalURL"] != null)
                {
                    Sitecore.Data.Fields.ReferenceField linkField = itm.Fields["CanonicalURL"];
                    if (linkField != null && linkField.TargetItem != null)
                    {
                        Item canonicalUrlItm = linkField.TargetItem;
                        canonicalUrl = SitecoreUtil.GetItemUrl(canonicalUrlItm);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.GetCanonicalUrlForSimilarContent()", ex);
                return canonicalUrl;
            }
            return canonicalUrl;
        }
        public static Item GetItemByShortId(string itemShortId, Database db = null)
        {
            Item item = null;
            try
            {
                if (db == null)
                    db = Context.ContentDatabase ?? Context.Database;


                if (!String.IsNullOrEmpty(itemShortId) && db != null)
                {
                    var itemId = new ID(itemShortId);
                    item = Context.Database.GetItem(itemId);

                    return item;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SitecoreUtil.GetItemByShortId()", ex);
                return item;
            }
            return item;
        }

        public static bool IsDerived(this Item item, ID templateId)
        {
            return !ID.IsNullOrEmpty(templateId) && item != null && TemplateManager.GetTemplate(item).IsDerived(templateId);
        }

        /// <summary>
        /// Get the data from MultiList fields
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static List<Item> GetMultiListFieldValues(Item item, string fieldName)
        {
            List<Item> results = new List<Item>();

            if (results != null)
            {
                MultilistField msFld = item.Fields[fieldName];

                results = (msFld != null && msFld.Items.Count() > 0) ? msFld.GetItems().ToList() : new List<Item>();
            }
            return results;
        }
        /// <summary>
        /// Get the Field Value
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetFieldValue(Item item, string fieldName)
        {
            string fieldValue = String.Empty;

            if (item != null)
            {
                fieldValue = item.Fields[fieldName] != null && !String.IsNullOrEmpty(item.Fields[fieldName].Value) ? item.Fields[fieldName].Value : String.Empty;
            }

            return fieldValue;
        }
        public static string DefaultDictionaryValue(string key)
        {
            return Sitecore.Globalization.Translate.Text(key);
        }
        /// <summary>
        /// Get the Url of MediaItem
        /// </summary>
        /// <param name="item"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetMediaItemUrl(MediaItem item, MediaUrlBuilderOptions options = null)
        {
            if (item != null)
            {
                //if (options != null)
                //{
                //    options = new MediaUrlBuilderOptions();
                //}

                //string mediaUrl = MediaManager.GetMediaUrl(item, options);
                string mediaUrl = MediaManager.GetMediaUrl(item);
                if (!string.IsNullOrEmpty(mediaUrl))
                {
                    mediaUrl = mediaUrl.Replace("/sitecore/shell", "");
                    mediaUrl = HashingUtils.ProtectAssetUrl(mediaUrl);
                    return mediaUrl;
                }
            }
            return string.Empty;
        }
        #endregion
    }
}

