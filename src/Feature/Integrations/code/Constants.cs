using Sitecore.Data;

namespace CGP.Feature.Integrations
{
    public class Templates
    {
        public static readonly ID WriteReviewPageTemplateId = new ID("{574F54F5-6F3D-4CF6-AC3E-225FDD39E963}");
        public static readonly ID ProductDetailPageTemplateId = new ID("{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}");
    }

    public class Constants
    {
        public class PowerReview
        {
            public static readonly string ReviewSnippet = "pr-reviewsnippet";
            public static readonly string ReviewDisplay = "pr-reviewdisplay";
            public static readonly string Write = "pr-write";
            public static readonly string LegacyProductGUID = "LegacyProductGUID";
        }
    }
}