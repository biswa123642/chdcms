using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.SitecoreExtensions.Model
{
    public class SiteConfigurationModel
    {
        public PowerReview PowerReview { get; set; }
        public PriceSpider PriceSpider { get; set; }
        public SeoSettings SeoSettings { get; set; }
        public BrandSettings BrandSettings { get; set; }
    }
    public class PowerReview
    {
        public bool EnablePowerReviews { get; set; }
        public string PowerReviewsAPIKey { get; set; }
        public string PowerReviewsLocale { get; set; }
        public string PowerReviewsMerchantGroupID { get; set; }
        public string PowerReviewsMerchantID { get; set; }
        public string PowerReviewsMapperURL { get; set; }
        public string PowerReviewsSendProductInfo { get; set; }
    }
    public class PriceSpider
    {
        public bool EnablePriceSpider { get; set; }
        public string PriceSpiderAccount { get; set; }
        public string PriceSpiderConfig { get; set; }
        public string PriceSpiderCountry { get; set; }
    }
    public class SeoSettings
    {
        public bool EnableSeoSchema { get; set; }
        public List<string> SelectSeoSchema { get; set; }
    }
    public class BrandSettings
    { 
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
    }
}