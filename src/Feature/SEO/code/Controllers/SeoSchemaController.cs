using CGP.Feature.SEO.Repositories;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Text;
using System.Web.Mvc;

namespace CGP.Feature.SEO.Controllers
{
    public class SeoSchemaController : StandardController
    {
        private readonly ISeoSchemaGenerator seoSchemaGenerator;
        private readonly ISiteConfiguration siteConfiguration;
        private readonly ILogger logger;
        
        public SeoSchemaController(ISeoSchemaGenerator seoSchemaGenerator, ISiteConfiguration siteConfiguration, ILogger logger)
        {
            this.seoSchemaGenerator = seoSchemaGenerator;
            this.logger = logger;
            this.siteConfiguration = siteConfiguration;
        }
        public ActionResult GetSeoSchema()
        {
            try
            {
                var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
                if (getSiteConfiguration.SeoSettings.EnableSeoSchema)
                {
                    JsonSerializerSettings jsonsettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore };
                    StringBuilder stringBuilder = new StringBuilder();

                    if (getSiteConfiguration != null && getSiteConfiguration.SeoSettings?.SelectSeoSchema.Count > 0)
                    {
                        foreach (var schemaName in getSiteConfiguration.SeoSettings.SelectSeoSchema)
                        {
                            switch (schemaName)
                            {
                                case Constants.SeoSchema.OrganizationSchema:
                                    if (Context.Item.TemplateID.ToString() == Template.Templates.HomeTemplateId)
                                    {
                                        ScriptBuilder(stringBuilder, seoSchemaGenerator.CreateOrganizationStructuredData(jsonsettings, getSiteConfiguration.BrandSettings.BrandLogo));
                                    }
                                    break;

                                case Constants.SeoSchema.WebsiteSchema:
                                    ScriptBuilder(stringBuilder, seoSchemaGenerator.CreateWebsiteStructuredData(jsonsettings, getSiteConfiguration.BrandSettings.BrandName));
                                    break;

                                case Constants.SeoSchema.FAQSchema:
                                    if (Context.Item.TemplateID.ToString() != Template.Templates.HomeTemplateId)
                                    {
                                        ScriptBuilder(stringBuilder, seoSchemaGenerator.CreateFAQStructuredData(jsonsettings));
                                    }
                                    break;

                                case Constants.SeoSchema.BreadCrumbSchema:
                                    if (Context.Item.TemplateID.ToString() != Template.Templates.HomeTemplateId)
                                    {
                                        ScriptBuilder(stringBuilder, seoSchemaGenerator.CreateBreadcrumbStructuredData(jsonsettings));
                                    }
                                    break;

                                case Constants.SeoSchema.ProductSchema:
                                    if (Context.Item.TemplateID.ToString() == Template.Templates.ProductTemplateId)
                                    {
                                        var productVariants = HelperExtension.GetProductVariants(HelperExtension.GetChildItem(Sitecore.Context.Item, Template.Templates.VariantGroupingTemplateId));
                                        if (productVariants.Count > 0)
                                        {
                                            foreach (Item variant in productVariants)
                                            {
                                                ScriptBuilder(stringBuilder, seoSchemaGenerator.CreateProductStructuredData(jsonsettings, getSiteConfiguration.BrandSettings.BrandName, getSiteConfiguration.BrandSettings.BrandLogo, variant));
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ViewBag.StrSchema = stringBuilder;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR : OrgSchemaController.Index() ", ex);
            }
            return View("~/Views/OneWeb/SEO/Schema.cshtml");
        }

        private static void ScriptBuilder(StringBuilder stringBuilder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                stringBuilder.Append("<script type='application/ld+json'>");
                stringBuilder.Append(data);
                stringBuilder.Append("</script>");
                stringBuilder.AppendLine();
            }
        }
    }
}