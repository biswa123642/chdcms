using Sitecore.Data;

namespace CGP.Feature.SEO
{
    public static class Template
    {
        public static class Templates
        {
            public static readonly ID SiteSettingTemplateId = new ID("{3845A334-0D01-4D82-80CF-FFAE3EBD8754}");
            public static readonly string HomeTemplateId = "{3BC22E05-E1FF-45A6-B8C3-FCF4C8F84A43}";
            public static readonly string ProductTemplateId = "{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}";
            public static readonly string ProductVariantTemplateId = "{6659B02D-BD4B-4821-B452-77FCA1DA6B43}";
            public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
        }
    }

    public static class Constants
    {
        public static class SeoSchema
        {
            public static readonly string SearchUrl = "/search";
            public static readonly string BrandName = "BrandName";
            public static readonly string BrandLogo = "BrandLogo";
            public static readonly string Schema = "SeoSchema";
            public static readonly string Accordian = "Accordion";
            public static readonly string Heading = "Heading";
            public static readonly string Content = "Content";
            public static readonly string VariantTitle = "VariantTitle";
            public static readonly string VariantDescription = "VariantDescription";
            public static readonly string VariantSKU = "VariantSKU";
            public static readonly string VariantUPC = "VariantUPC";
            public const string OrganizationSchema = "Organization Schema";
            public const string WebsiteSchema = "Website Schema";
            public const string FAQSchema = "FAQ Schema";
            public const string BreadCrumbSchema = "BreadCrumb Schema";
            public const string ProductSchema = "Product Schema";
        }
    }
}