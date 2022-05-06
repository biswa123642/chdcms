using CGP.Foundation.SitecoreExtensions.Repositories;
using System.Web.Mvc;
using Sitecore.XA.Foundation.Mvc.Controllers;
using CGP.Feature.Integrations.Models;
using System.Text;
using Newtonsoft.Json;
using CGP.Feature.Integrations.Repositories;

namespace CGP.Feature.Integrations.Controllers
{
    public class IntegrationsController : StandardController
    {
        private readonly ISiteConfiguration siteConfiguration;
        private readonly IIntegrationsRepository integrationsRepository;
        public IntegrationsController(ISiteConfiguration siteConfiguration,IIntegrationsRepository integrationsRepository)
        {
            this.siteConfiguration = siteConfiguration;
            this.integrationsRepository = integrationsRepository;
        }
        public ActionResult LoadIntegrationHeadScripts()
        {
            IntegrationsViewModel integrationsViewModel = new IntegrationsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            if (getSiteConfiguration.PowerReview.EnablePowerReviews && Sitecore.Context.Item.TemplateID.Equals(Templates.ProductDetailPageTemplateId))  //check current page template
            {
                integrationsViewModel.EnablePowerReviews = true;
                integrationsViewModel.IsProductDetailPage = true;
                integrationsViewModel.PowerReviews = integrationsRepository.GetPowerReviewIntegrationSettings();
            }
            if (getSiteConfiguration.PriceSpider.EnablePriceSpider)
            {
                integrationsViewModel.EnableWhereToBuy = true;
                integrationsViewModel.WhereToBuy = new WhereToBuy
                {
                    Account = getSiteConfiguration.PriceSpider.PriceSpiderAccount,
                    Config = getSiteConfiguration.PriceSpider.PriceSpiderConfig,
                    Country = getSiteConfiguration.PriceSpider.PriceSpiderCountry
                };
            }
            if (getSiteConfiguration.SeoSettings.EnableSeoSchema)
            {
                //Enable SeoSchema
                //other logics
            }
            return View("~/Views/OneWeb/Integrations/IntegrationsHead.cshtml", integrationsViewModel);
        }

        public ActionResult WhereToBuy()
        {
            if (siteConfiguration.GetSiteConfiguration().PriceSpider.EnablePriceSpider)
            {
                ViewBag.PSConfig = siteConfiguration.GetSiteConfiguration().PriceSpider.PriceSpiderConfig;
                return View("~/Views/OneWeb/Integrations/WhereToBuy.cshtml");
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
