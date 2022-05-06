using Sitecore.Configuration;
using Sitecore.Data;

namespace CGP.Foundation.SitecoreExtensions.Constants
{
    public class Templates
    {
        //OneWeb Template IDs
        public static readonly string OneWebDefaultSiteItemID = Settings.GetSetting("OneWebDefaultSiteItemID");
        public static readonly string OneWebTenantTemplateID = Settings.GetSetting("OneWebTenantTemplateID");
        public static readonly string OneWebSiteTemplateID = Settings.GetSetting("OneWebSiteTemplateID");
        public static readonly ID SiteSettingTemplateId = new ID("{3845A334-0D01-4D82-80CF-FFAE3EBD8754}");
        public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
        public static readonly ID ProductVariantTemplateId = new ID("{6659B02D-BD4B-4821-B452-77FCA1DA6B43}");


        public struct SiteConfiguration
        {
            /// <summary>
            /// Defines the Id.
            /// </summary>
            public static readonly ID Id = ID.Parse("{77820909-82A2-4308-9E62-F54E15110EF3}");
            public struct Fields
            {
                public static ID BrandName = new ID("{4762F3C8-900D-44FF-BA2E-D44E57E8F5FB}");
                public static ID BrandLogo = new ID("{C07BE27B-FCB2-44EB-9162-A1E183EE79B9}");
                public static ID EnablePowerReviews = new ID("{ED38D73E-A875-424C-8F31-8E0B0C0D197E}");
                public static ID PowerReviewsAPIKey = new ID("{7BE09131-F96B-4F0E-ACB8-2D50AA27C6E2}");
                public static ID PowerReviewsLocale = new ID("{AFCFBB0D-D8AF-4442-9083-D0F0712ED0A2}");
                public static ID PowerReviewsMerchantGroupID = new ID("{D30967E5-F8C5-4EE6-A5A0-559A6CF5D639}");
                public static ID PowerReviewsMerchantID = new ID("{8F0F4B7C-759C-4871-91AE-D6166BF319EA}");
                public static ID PowerReviewsMapperURL = new ID("{B40EBD0E-FF54-4EE7-8D12-E95A86743552}");
                public static ID PowerReviewsSendProductInfo = new ID("{12773A99-51F6-4C4E-9344-B88BC60A1721}");
                public static ID PriceSpiderAccount = new ID("{9B7BB2EA-CC16-4EF0-862A-B5DA3537A938}");
                public static ID EnablePriceSpider = new ID("{BF6B073F-2C03-4D90-BDA3-FD070FE69665}");
                public static ID PriceSpiderConfig = new ID("{BE547B09-1B5F-4ED9-8772-2F79B5E64D83}");
                public static ID PriceSpiderCountry = new ID("{3D8C2876-9123-4724-92B2-58E7A7DB0452}");
                public static ID SelectSeoSchema = new ID("{F6A9E708-ACDF-411A-A685-9D9CBF9252C1}");
                public static ID EnableSeoSchema = new ID("{EC3719F3-58B4-4504-8545-DF1AF5AEF83D}");
            }
        }
    }
    public class Constants
    {
        public static readonly string ChooseVariant = "ChooseVariant";
    }
}