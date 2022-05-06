using CGP.Foundation.SitecoreExtensions.Model;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using CGP.Foundation.SitecoreExtensions.Constants;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore.Data.Fields;
using System.Collections.Specialized;

namespace CGP.Foundation.SitecoreExtensions.Repositories
{
    public class SiteConfiguration : ISiteConfiguration
    {
        private readonly ILogger logger;

        public SiteConfiguration(ILogger logger)
        {
            this.logger = logger;
        }
        public SiteConfigurationModel GetSiteConfiguration()
        {
            SiteConfigurationModel siteConfigurationModel = InitializeSiteConfigurationModel();

            try
            {
                // var allSiteConfigurationSettings = CacheExtensions.GetObjectCacheVariable($"All_SiteConfiguration_{item.ID}_{item.Language.Name}_{fieldName}") as Dictionary<string, string>;

                Item siteConfigurationItem = HelperExtension.GetSiteConfigurationItem(Templates.SiteConfiguration.Id);
                if (siteConfigurationItem != null)
                {
                    siteConfigurationModel = new SiteConfigurationModel()
                    {
                        PriceSpider =
                        new PriceSpider
                        {
                            EnablePriceSpider = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnablePriceSpider.ToString()),
                            PriceSpiderAccount = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderAccount),
                            PriceSpiderConfig = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderConfig),
                            PriceSpiderCountry = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderCountry)
                        },
                        PowerReview = new PowerReview
                        {
                            EnablePowerReviews = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnablePowerReviews.ToString()),
                            PowerReviewsAPIKey = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsAPIKey),
                            PowerReviewsLocale = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsLocale),
                            PowerReviewsMapperURL = GetPowerReviewItemUrl(Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMapperURL)),
                            PowerReviewsMerchantGroupID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMerchantGroupID),
                            PowerReviewsMerchantID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMerchantID),
                            PowerReviewsSendProductInfo = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsSendProductInfo)
                        },
                        SeoSettings = new SeoSettings
                        {
                            EnableSeoSchema = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnableSeoSchema.ToString()),
                            SelectSeoSchema = GetSelectedSchema(siteConfigurationItem)
                        },
                        BrandSettings = new BrandSettings
                        {
                            BrandName = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.BrandName),
                            BrandLogo = GetBrandLogoUrl(siteConfigurationItem)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetBrandLogoUri() ", ex);

            }
            return siteConfigurationModel;
        }

        private string GetBrandLogoUrl(Item siteConfigurationItem)
        {
            try
            {
                Sitecore.Data.Fields.ImageField imageField = siteConfigurationItem.Fields[Templates.SiteConfiguration.Fields.BrandLogo];
                return Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem);
            }
            catch (Exception ex)
            {

                logger.LogError("ERROR in SiteConfiguration.GetBrandLogoUri() ", ex);
                return string.Empty;
            }
        }

        private List<string> GetSelectedSchema(Item siteConfigurationItem)
        {
            List<string> schemaList = new List<string>();
            try
            {
                Sitecore.Data.Fields.MultilistField selectedSchemas = siteConfigurationItem.Fields[Templates.SiteConfiguration.Fields.SelectSeoSchema];
                foreach (Item schema in selectedSchemas.GetItems())
                {
                    schemaList.Add(schema.Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetSelectedSchema() ", ex);

            }
            return schemaList;
        }

        private string GetPowerReviewItemUrl(string powerReviewItemId)
        {
            string powerReviewItemUrl = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(powerReviewItemId))
                {
                    var powerReviewItem = HelperExtension.GetItem(powerReviewItemId);
                    powerReviewItemUrl = Sitecore.Links.LinkManager.GetItemUrl(powerReviewItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetPowerReviewItemUrl() ", ex);
            }
            return powerReviewItemUrl;
        }

        private SiteConfigurationModel InitializeSiteConfigurationModel()
        {
            return new SiteConfigurationModel
            {
                BrandSettings = new BrandSettings(),
                PowerReview = new PowerReview(),
                PriceSpider = new PriceSpider(),
                SeoSettings = new SeoSettings { SelectSeoSchema = new List<string>() }
            };
        }

        public NameValueCollection GetAutoSuggestedTemplate(Item currentItem = null)
        {
            try
            {
                Item siteConfigurationItem = HelperExtension.GetSiteConfigurationItem(Templates.SiteConfiguration.Id);
                var autoSuggestedField = siteConfigurationItem?.Fields["AutoSuggestedTemplates"];
                if (autoSuggestedField != null && !string.IsNullOrWhiteSpace(autoSuggestedField.Value))
                {
                    return ((NameValueListField)autoSuggestedField).NameValues;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetPowerReviewItemUrl() ", ex);
            }
            return new NameValueCollection();
        }
    }
}