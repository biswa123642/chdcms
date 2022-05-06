using Newtonsoft.Json;
using Sitecore.Data.Items;

namespace CGP.Feature.SEO.Repositories
{
    public interface ISeoSchemaGenerator
    {
        string CreateBreadcrumbStructuredData(JsonSerializerSettings jsonSettings);
        string CreateFAQStructuredData(JsonSerializerSettings jsonSettings);
        string CreateOrganizationStructuredData(JsonSerializerSettings jsonsettings, string brandLogoUrl);
        string CreateProductStructuredData(JsonSerializerSettings jsonSettings, string brandName, string brandLogoUrl, Item variant);
        string CreateWebsiteStructuredData(JsonSerializerSettings jsonSettings, string brandName);
    }
}