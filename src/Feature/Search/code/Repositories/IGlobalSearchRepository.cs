using CGP.Feature.Search.Models;
using CGP.Foundation.Search.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CGP.Feature.Search.Repositories
{
    public interface IGlobalSearchRepository
    {
        GlobalSearchResult GetGlobalSearchResults(InputParameters inputParameters);
        List<SolrModel> GetLoadMoreResults(InputParameters inputParameters);
        JObject GetSuggestions(string searchTerm, string currentItemId, string indexName = null);
    }
}