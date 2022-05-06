using Sitecore.Data;

namespace CGP.Foundation.Search
{
    public class Constants
    {
        public static readonly string MobileImage = "MobileImage";
        public static readonly string DesktopImage = "DesktopImage";
    }
    public class Templates
    {
        public const string ContentPageIdString = "{AA977B10-C924-4446-8B15-A7D26AA6C93F}";
        public static readonly ID StandardBasePageID = new ID(ContentPageIdString);

        public const string ProductDetailPageIdString = "{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}";
        public static readonly ID ProductDetailPageID = new ID(ProductDetailPageIdString);

        public const string ArticlePageIdString = "{057AF325-C948-4918-A4E4-2C3F6CF04524}";
        public static readonly ID ArticlePageID = new ID(ArticlePageIdString);

        public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
        public static readonly ID VideoTemplateId = new ID("{1E9F56B7-FD80-4EAE-9FD4-8AB6242C6DB8}");
    }
}