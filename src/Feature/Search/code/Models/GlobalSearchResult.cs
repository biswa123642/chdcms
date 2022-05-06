using CGP.Foundation.Search;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Feature.Search.Models
{
    public class GlobalSearchResult
    {
        public GlobalSearchResult(SolrSearchResponse solrResponse)
        {
            Products = new GlobalSearchGroup();
            Articles = new GlobalSearchGroup();
            Others = new GlobalSearchGroup();

            if (solrResponse != null)
            {
                SearchTerm = solrResponse.SearchTerm;
                if (solrResponse.TotalResultCount > 0)
                {
                    var resultCount = SearchExtensions.GetSearchResultCounts();
                    var searchGroups = solrResponse.QueryResults.Grouping.Values.FirstOrDefault()?.Groups;
                    if (searchGroups != null && searchGroups.Any())
                    {
                        TotalResultCount = solrResponse.QueryResults.Grouping.Values.FirstOrDefault()?.Matches ?? 0;

                        var productGroup = searchGroups.FirstOrDefault(x => x.GroupValue == StringUtil.RemoveSpecialCharacters(Templates.ProductDetailPageIdString.ToLower()));
                        if (productGroup != null)
                        {
                            Products = new GlobalSearchGroup()
                            {
                                GroupIdentifier = productGroup.GroupValue,
                                GroupCount = productGroup.NumFound,
                                GroupItems = productGroup.Documents.Take(resultCount.ProductResultCount).Select(x => new GlobalResultItem()
                                {
                                    ResultItem = x,
                                }).ToList(),
                            };
                        }

                        var articleGroup = searchGroups.FirstOrDefault(x => x.GroupValue == StringUtil.RemoveSpecialCharacters(Templates.ArticlePageIdString.ToLower()));
                        if (articleGroup != null)
                        {
                            Articles = new GlobalSearchGroup()
                            {
                                GroupIdentifier = articleGroup.GroupValue,
                                GroupCount = articleGroup.NumFound,
                                GroupItems = articleGroup.Documents.Take(resultCount.ArticleResultCount).Select(x => new GlobalResultItem()
                                {
                                    ResultItem = x,
                                }).ToList(),
                            };
                        }

                        var otherGroup = searchGroups.FirstOrDefault(x => x.GroupValue == StringUtil.RemoveSpecialCharacters(Templates.ContentPageIdString.ToLower()));
                        if (otherGroup != null)
                        {
                            Others = new GlobalSearchGroup()
                            {
                                GroupIdentifier = otherGroup.GroupValue,
                                GroupCount = otherGroup.NumFound,
                                GroupItems = otherGroup.Documents.Take(resultCount.OtherResultCount).Select(x => new GlobalResultItem()
                                {
                                    ResultItem = x,
                                }).ToList(),
                            };
                        }
                    }

                    //var facetFields = solrResponse.QueryResults.FacetFields;
                    //if (facetFields != null)
                    //{
                    //    var globalFacets = SearchExtensions.GetGlobalFacets();
                    //    foreach (var facet in globalFacets)
                    //    {
                    //        if (facetFields.ContainsKey(facet.FacetField))
                    //        {
                    //            var facetCategory = new FacetResults()
                    //            {
                    //                FacetIdentifier = facet.FacetIdentifier,
                    //                FacetField = facet.FacetField,
                    //                FacetName = facet.FacetName,
                    //                FacetValues = facetFields[facet.FacetField].Select(x =>
                    //                new FacetValue
                    //                {
                    //                    Key = x.Key,
                    //                    Value = x.Value,
                    //                }).ToArray(),
                    //                FacetType = facet.FacetType,
                    //            };
                    //            Facets.Add(facetCategory);
                    //        }
                    //    }
                    //}
                }
            }
        }

        public GlobalSearchGroup Products { get; set; }
        public GlobalSearchGroup Articles { get; set; }
        public GlobalSearchGroup Others { get; set; }
        public List<FacetResults> Facets { get; set; }
        public int TotalResultCount { get; set; }
        public string FilterString { get; set; }
        public string SearchTerm { get; set; }
        public List<string> UserSaves { get; set; }
        public List<string> Suggestions { get; set; }
    }
}