using CGP.Feature.Search.Models;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Feature.Search.Repositories
{
    public class GlobalSearchRepository : IGlobalSearchRepository
    {
        private readonly ISiteConfiguration _siteConfiguration;
        public GlobalSearchRepository(ISiteConfiguration siteConfiguration)
        {
            _siteConfiguration = siteConfiguration;
        }
        public GlobalSearchResult GetGlobalSearchResults(InputParameters inputParameters)
        {
            SolrSearchResponse solrSearchResponse = InitiateSearch(inputParameters, false);
            if (solrSearchResponse.QueryResults.Count > 0)
            {
                GlobalSearchResult globalSearchResult = new GlobalSearchResult(solrSearchResponse);
                globalSearchResult.FilterString = inputParameters.FilterString;
                return globalSearchResult;
            }
            else
            {
                GlobalSearchResult globalSearchResult = new GlobalSearchResult(solrSearchResponse);
                var suggestions = solrSearchResponse.QueryResults.SpellChecking.FirstOrDefault()?.Suggestions;
                if (suggestions != null && suggestions.Any())
                {
                    globalSearchResult.Suggestions = suggestions.ToList();
                }

                return globalSearchResult;
            }
        }

        public List<SolrModel> GetLoadMoreResults(InputParameters inputParameters)
        {
            return InitiateSearch(inputParameters, true).QueryResults;
        }

        public JObject GetSuggestions(string searchTerm, string searchItemId, string indexName = null)
        {
            var contextDb = Sitecore.Context.Database;
            var resultObject = new JObject();
            if (!string.IsNullOrEmpty(searchTerm))
            {

                var solrParameters = new SolrSearchParameters()
                {
                    Keyword = searchTerm,
                };

                solrParameters.CurrentItem = string.IsNullOrEmpty(searchItemId)
                    ? Sitecore.Context.Item
                    : Sitecore.Context.Database.GetItem(searchItemId);

                var autoSuggestTemplates = _siteConfiguration.GetAutoSuggestedTemplate(solrParameters.CurrentItem);
                if (autoSuggestTemplates.Count <= 0)
                {
                    return new JObject();
                }

                var searchResults = SearchExtensions.GetAutoSuggestResults(solrParameters, autoSuggestTemplates);

                if (searchResults.QueryResults.Count > 0)
                {
                    var searchGroups = searchResults.QueryResults?.Grouping?.Values?.FirstOrDefault()?.Groups;
                    var groupArray = new JArray();
                    if (searchGroups != null)
                    {
                        foreach (var templateKey in autoSuggestTemplates.AllKeys)
                        {
                            var currentTemplateId = StringUtil.RemoveSpecialCharacters(templateKey);

                            var currentTemplateName = autoSuggestTemplates[templateKey];
                            var currentGroup = searchGroups.FirstOrDefault(x =>
                               x.GroupValue.Equals(currentTemplateId, StringComparison.InvariantCultureIgnoreCase));

                            if (currentGroup?.Documents != null)
                            {
                                //TODO Manipulate the Product, Article and Other Page Count
                            }
                        }
                        int order = 0;
                        foreach (var templateKey in autoSuggestTemplates.AllKeys)
                        {
                            int documentToShowCount = 0;
                            order++;
                            var templateString = StringUtil.RemoveSpecialCharacters(templateKey);
                            var templateId = ID.Parse(templateKey);
                            var currentGroup = searchGroups.FirstOrDefault(x =>
                                x.GroupValue.Equals(templateString, StringComparison.InvariantCultureIgnoreCase));

                            if (StringUtil.IdEqualGuid(templateId, "ProductTemplateIDString"))
                            {
                                documentToShowCount = 5;
                            }

                            if (StringUtil.IdEqualGuid(templateId, "OtherTemplateIDString"))
                            {
                                documentToShowCount = 2;
                            }

                            if (StringUtil.IdEqualGuid(templateId, "ArticleTemplateIDString"))
                            {
                                documentToShowCount = 3;
                            }

                            var groupObject = new JObject
                            {
                                ["order"] = order,
                                ["group"] = templateString,
                                ["total_count"] = 0,
                                ["type"] = autoSuggestTemplates[templateKey],
                            };
                            groupArray.Add(groupObject);
                        }
                    }
                    resultObject["results"] = groupArray;

                }
                else
                {
                    var suggestions = searchResults.QueryResults.SpellChecking.FirstOrDefault()?.Suggestions;
                    if (suggestions != null && suggestions.Any())
                    {
                        resultObject["suggestions"] = new JArray(suggestions);
                    }
                }
            }

            return resultObject;
        }

        private SolrSearchResponse InitiateSearch(InputParameters inputParameters, bool isLoadMore)
        {
            var searchTerm = inputParameters.SearchTerm;
            if (searchTerm.Length > 2)
            {
                if (searchTerm[searchTerm.Length - 1].Equals('s'))
                {
                    searchTerm = searchTerm.Remove(searchTerm.Length - 1, 1);
                }

                if (searchTerm[0].Equals('"'))
                {
                    if (!searchTerm[searchTerm.Length - 1].Equals('"'))
                    {
                        searchTerm = searchTerm.Remove(0, 1);
                    }
                }
            }

            var filters = inputParameters.Filters.Where(x => !x.IsRangeFilter).Select(x => (ISolrQuery)new SolrQueryInList(x.FilterKey, x.FilterValues)).ToList();
            filters.AddRange(inputParameters.Filters.Where(x => x.IsRangeFilter).Select(x => (ISolrQuery)new SolrQueryByRange<int>(x.FilterKey, 0, x.FilterValueAsInt())));
            var solrParameters = new SolrSearchParameters()
            {
                Keyword = searchTerm,
                CurrentItem = string.IsNullOrEmpty(inputParameters.CurrentItemId) ? Sitecore.Context.Item : Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId),
                QueryOptions = new QueryOptions()
                {
                    FilterQueries = new List<ISolrQuery>(filters),
                    StartOrCursor = new StartOrCursor.Start(inputParameters.SkipCount),
                },
                IsLoadMore = isLoadMore,
            };

            if (inputParameters.SearchCount != 0)
            {
                solrParameters.QueryOptions.Rows = inputParameters.SearchCount;
            }

            SolrSearchResponse solrSearchResponse = SearchExtensions.GetSearchResults(solrParameters);
            return solrSearchResponse;
        }
    }
}