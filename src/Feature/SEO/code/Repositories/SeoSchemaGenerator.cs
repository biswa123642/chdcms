using CGP.Feature.SEO.Models.StructuredData;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Feature.SEO.Repositories
{
    public class SeoSchemaGenerator : ISeoSchemaGenerator
    {
        private readonly ILogger logger;
        private readonly ISiteConfiguration siteConfiguration;
        public SeoSchemaGenerator(ILogger logger, ISiteConfiguration siteConfiguration)
        {
            this.logger = logger;
            this.siteConfiguration = siteConfiguration;
        }
        public string CreateBreadcrumbStructuredData(JsonSerializerSettings jsonSettings)
        {
            BreadCrumbListSchema breadCrumbListSchema = new BreadCrumbListSchema();
            try
            {
                var homeItem = HelperExtension.GetHomeItem();
                var contextItem = Sitecore.Context.Item;
                if (homeItem != null)
                {
                    var breadcrumbItems = new List<Item>();
                    breadcrumbItems.Add(contextItem);
                    while (!contextItem.ID.Equals(homeItem.ID))
                    {
                        contextItem = contextItem.Parent;
                        if (contextItem.Visualization.Layout != null)
                        {
                            breadcrumbItems.Add(contextItem);
                        }
                    }
                    int counter = 0;
                    breadcrumbItems.Reverse();
                    breadCrumbListSchema.ItemListElement = breadcrumbItems.Select(item => new ListItem
                    {
                        Name = item.DisplayName ?? item.Name,
                        Position = (++counter).ToString(),
                        Item = Sitecore.Links.LinkManager.GetItemUrl(item, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true }),
                    }).ToList();
                }
                return JsonConvert.SerializeObject(breadCrumbListSchema, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in OrgSchemaGenerator.CreateBreadcrumbStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateFAQStructuredData(JsonSerializerSettings jsonSettings)
        {
            try
            {
                var contextItem = Sitecore.Context.Item;
                List<Sitecore.Layouts.RenderingReference> faqComponent = contextItem.Visualization.GetRenderings(Sitecore.Context.Device, false)
                    .Where(i => i.WebEditDisplayName == Constants.SeoSchema.Accordian && i.Settings.DataSource != null).ToList();

                if (faqComponent.Count > 0)
                {
                    FAQPageSchema faqPageSchema = new FAQPageSchema
                    {
                        MainEntity = new List<Question>()
                    };
                    for (int i = 0; i < faqComponent.Count; i++)
                    {
                        Item faqDataSourceItem = Sitecore.Context.Database.GetItem(faqComponent[i].Settings.DataSource);
                        foreach (Item accordianItems in faqDataSourceItem.GetChildren())
                        {
                            if (accordianItems != null)
                            {
                                faqPageSchema.MainEntity.Add(new Question()
                                {
                                    Name = accordianItems.Fields[Constants.SeoSchema.Heading].Value,
                                    AcceptedAnswer = new AcceptedAnswer()
                                    {
                                        Text = accordianItems.Fields[Constants.SeoSchema.Content].Value,
                                    },
                                });
                            }
                        }
                    }
                    return (faqPageSchema.MainEntity != null) ? JsonConvert.SerializeObject(faqPageSchema, jsonSettings) : string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in OrgSchemaGenerator.CreateFAQStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateOrganizationStructuredData(JsonSerializerSettings jsonsettings, string logoUrl)
        {
            try
            {
                var contextItem = Sitecore.Context.Item;
                OrganizationSchema organizationSchema = new OrganizationSchema();
                organizationSchema.Name = Sitecore.Context.GetSiteName();
                organizationSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(contextItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                organizationSchema.Logo = HelperExtension.GetHostName() + logoUrl;
                return JsonConvert.SerializeObject(organizationSchema, jsonsettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in OrgSchemaGenerator.CreateFAQStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateProductStructuredData(JsonSerializerSettings jsonSettings, string brandName, string brandLogoUrl, Item variant)
        {
            try
            {
                List<ProductSchema> productSchemas = new List<ProductSchema>();
                ProductSchema productSchema = new ProductSchema();
                productSchema.Name = variant.Fields[Constants.SeoSchema.VariantTitle].Value.ToString();
                productSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(variant, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                productSchema.Description = variant.Fields[Constants.SeoSchema.VariantDescription].Value.ToString();
                productSchema.Sku = variant.Fields[Constants.SeoSchema.VariantSKU].Value.ToString();
                productSchema.Gtin12 = variant.Fields[Constants.SeoSchema.VariantUPC].Value.ToString();
                productSchema.Brand = new BrandSchema()
                {
                    Name = brandName,
                    Logo = HelperExtension.GetHostName() + brandLogoUrl
                };
                productSchemas.Add(productSchema);
                return JsonConvert.SerializeObject(productSchemas, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in OrgSchemaGenerator.CreateProductStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateWebsiteStructuredData(JsonSerializerSettings jsonSettings, string brandName)
        {
            try
            {
                WebsiteSchema webSiteSchema = new WebsiteSchema();
                webSiteSchema.Name = Sitecore.Context.GetSiteName();
                webSiteSchema.Alternatename = brandName;
                webSiteSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(Sitecore.Context.Item, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                webSiteSchema.potentialAction = new PotentialAction()
                {
                    Target = webSiteSchema.Url + Constants.SeoSchema.SearchUrl
                };
                return JsonConvert.SerializeObject(webSiteSchema, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in OrgSchemaGenerator.CreateWebsiteStructuredData() ", ex);
                return string.Empty;
            }
        }
    }
}