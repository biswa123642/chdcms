using CGP.Foundation.SitecoreExtensions.Model;
using Sitecore.Data.Items;
using System.Collections.Specialized;

namespace CGP.Foundation.SitecoreExtensions.Repositories
{
    public interface ISiteConfiguration
    {
        SiteConfigurationModel GetSiteConfiguration();

        NameValueCollection GetAutoSuggestedTemplate(Item current = null);
    }
}
