using CGP.Foundation.Search.Models;
using CGP.Foundation.Search.Services;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Extensions
{
    public class SearchExtensions
    {
        public static List<string> GetSearchableTemplates()
        {
            return new List<string>()
            {
                StringUtil.RemoveSpecialCharacters(Templates.ProductDetailPageIdString),
                StringUtil.RemoveSpecialCharacters(Templates.ArticlePageIdString),
                StringUtil.RemoveSpecialCharacters(Templates.ContentPageIdString),
            };
        }
        public static SolrSearchResponse GetSearchResults(SolrSearchParameters searchParameters)
        {
            var searchableTemplates = GetSearchableTemplates();
            //  var globalFacets = SiteExtensions.GetGlobalFacets(); Todo
            var globalFacets = new List<FacetModel>();

            var searchTerm = StringUtil.RemoveSpecialCharactersExceptSpaceQuotes(searchParameters.Keyword).Trim();
            var searchFields = new[]
            {
                "title_t",
                "opengraphtitle_t",
                "opengraphdescription_t",
                "content_t"
            };

            AbstractSolrQuery solrQuery = new SolrQuery(string.Empty);

            foreach (var field in searchFields)
            {
                AbstractSolrQuery fieldQuery = new SolrQueryByField(field, StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm));
                if (field.Equals("title_t"))
                {
                    fieldQuery = new SolrQueryBoost(new SolrQueryByField(field, StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm)), 50f);
                }

                AbstractSolrQuery termQuery = new SolrQuery(string.Empty);
                if (!string.IsNullOrWhiteSpace(searchTerm) && !(searchTerm[0].Equals('"') && searchTerm[searchTerm.Length - 1].Equals('"')) && StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm).Split(' ').Length > 0)
                {
                    foreach (var term in StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm).Split(' '))
                    {
                        string tempTerm = term.Trim();
                        if (field.Equals("title_t"))
                        {
                            termQuery = termQuery || new SolrQueryBoost(new SolrQueryByFieldRegex(field, "/.*" + tempTerm + ".*/"), 5f);
                        }
                        else
                        {
                            termQuery = termQuery || new SolrQueryByFieldRegex(field, "/.*" + tempTerm + ".*/");
                        }
                    }
                }

                fieldQuery = fieldQuery || termQuery;
                solrQuery = solrQuery || fieldQuery;
            }

            var languageQuery = new SolrQueryByField("_language", Sitecore.Context.Language.Name);

            var completeQuery = new SolrQueryInList("_template", searchableTemplates) && solrQuery && languageQuery;

            if (!searchParameters.IsLoadMore)
            {
                searchParameters.QueryOptions.Grouping = new GroupingParameters()
                {
                    Fields = new[] { "_template" },
                    Limit = 8,
                };
                searchParameters.QueryOptions.Facet = new FacetParameters()
                {
                    Limit = 10,
                    Queries =
                        globalFacets.Select(x => (ISolrFacetQuery)new SolrFacetFieldQuery(x.FacetField)).ToList(),
                };
                searchParameters.QueryOptions.SpellCheck = new SpellCheckingParameters()
                {
                    Collate = true,
                    Count = 5,
                    Query = $"_name:({searchTerm})",
                };
            }

            searchParameters.Query = completeQuery;

            var searchService = new SearchService(ParseContextIndex(searchParameters));
            SolrSearchResponse retObj = searchService.ContentSearchIndex(searchParameters);
            return retObj;
        }

        private static string ParseContextIndex(SearchParameters searchParameters)
        {
            var indexName = string.Empty;
            if (searchParameters == null)
                return indexName;

            if (!string.IsNullOrEmpty(searchParameters.IndexName))
                indexName = searchParameters.IndexName;
            else
            {
                if (!string.IsNullOrEmpty(searchParameters.SiteName))
                {
                    //TODO:- make it dynamic after testing
                    // indexName = searchParameters.SiteName.ToLower() + "_index";
                    indexName = "pennington_index";
                }
                else
                {
                    if (searchParameters.CurrentItem != null)
                    {
                        var currentSite = Factory.GetSiteInfoList().Where(s => s.RootPath != "" && searchParameters.CurrentItem.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                            .OrderByDescending(s => s.RootPath.Length)
                            .FirstOrDefault();
                        var siteName = currentSite?.Name;
                        if (!string.IsNullOrEmpty(siteName))
                        {
                            //TODO:- make it dynamic after testing
                            //indexName = siteName.ToLower() + "_index";
                            indexName = "pennington_index";
                        }
                    }
                }

                if (string.IsNullOrEmpty(indexName))
                {
                    var contextSiteName = Sitecore.Context.Site?.Name;
                    if (!string.IsNullOrEmpty(contextSiteName))
                    {
                        //TODO:- make it dynamic after testing
                        indexName = contextSiteName.ToLower() + "_index";
                        indexName = "pennington_index";
                    }
                }
            }

            return indexName;
        }

        public static SearchResultCounts GetSearchResultCounts()
        {
            var homeItem = HelperExtension.GetHomeItem();
            if (homeItem != null)
            {
                int.TryParse("2", out var productCount);
                int.TryParse("3", out var articleCount);
                int.TryParse("4", out var otherCount);

                return new SearchResultCounts()
                {
                    ProductResultCount = productCount != 0 ? productCount : 4,
                    ArticleResultCount = articleCount != 0 ? articleCount : 2,
                    OtherResultCount = otherCount != 0 ? otherCount : 3,
                };
            }

            return null;
        }

        public static List<FacetModel> GetGlobalFacets(Item currentItem = null)
        {
            var homeItem = HelperExtension.GetHomeItem();
            if (homeItem != null)
            {
                var facetTemplates =
                    SitecoreUtil.GetMultiListFieldValues(homeItem, "");
                if (facetTemplates.Any())
                {
                    return ParseGlobalFacets(facetTemplates);
                }
            }

            return new List<FacetModel>();
        }
        private static List<FacetModel> ParseGlobalFacets(List<Item> facetItems)
        {
            return facetItems.Where(x => x != null).Select(x => new FacetModel()
            {
                FacetName = SitecoreUtil.GetFieldValue(x, "Name"),
                FacetIdentifier = StringUtil.RemoveSpecialCharacters(SitecoreUtil.GetFieldValue(x, "Name")).ToLower(),
                FacetField = SitecoreUtil.GetFieldValue(x, "Parameters"),
                FacetThreshold = int.Parse(SitecoreUtil.GetFieldValue(x, "Minimum Number of Items")),
            }).ToList();
        }

        public static SolrSearchResponse GetAutoSuggestResults(SolrSearchParameters searchParameters, NameValueCollection autoSuggestTemplates)
        {
            var searchTerm = searchParameters.Keyword;
            if (autoSuggestTemplates != null)
            {
                var searchFields = new[]
                {
                    "title_t",
                    "opengraphtitle_t",
                };

                AbstractSolrQuery solrQuery = new SolrQuery("");

                foreach (var field in searchFields)
                {
                    AbstractSolrQuery fieldQuery = new SolrQueryBoost(new SolrQueryByField(field, searchTerm), 5);
                    AbstractSolrQuery termQuery = new SolrQuery("*:*");
                    foreach (var term in searchTerm.Split(' '))
                    {
                        termQuery = termQuery && new SolrQueryByFieldRegex(field, "/.*" + term + ".*/");
                    }

                    fieldQuery = fieldQuery || termQuery;

                    solrQuery = solrQuery || fieldQuery;
                }

                var languageQuery = new SolrQueryByField("_language", Sitecore.Context.Language.Name);

                var completeQuery = new SolrQueryInList(
                    "_template",
                    autoSuggestTemplates.AllKeys.Select(x => StringUtil.RemoveSpecialCharacters(x).ToLower()))
                                    && solrQuery && languageQuery;


                searchParameters.QueryOptions = new QueryOptions()
                {
                    Grouping = new GroupingParameters()
                    {
                        Fields = new[] { "_template" },
                        Limit = 4,
                    },
                    SpellCheck = new SpellCheckingParameters()
                    {
                        Collate = true,
                        Count = 5,
                        Query = $"_name:({searchTerm})",
                    },
                };
                searchParameters.Query = completeQuery;

                var searchService = new SearchService(ParseContextIndex(searchParameters));
                return searchService.ContentSearchIndex(searchParameters);
            }

            return new SolrSearchResponse();
        }

    }
}