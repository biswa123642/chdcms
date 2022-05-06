using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SolrNetExtension;
using Sitecore.ContentSearch.SolrProvider.SolrNetIntegration;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CGP.Foundation.Search.Models;

namespace CGP.Foundation.Search.Services
{
    public class SearchService
    {
        private ISearchIndex index;
        public SearchService(string IndexName)
        {
            index = ContentSearchManager.GetIndex(IndexName);
        }
        public SearchService(Item ContextItem)
        {
            index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(ContextItem));
        }
        public List<T> ContentSearchIndex<T>(Expression<Func<T, bool>> predicate)
        {
            using (IProviderSearchContext context = index.CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck))
            {
                return context.GetQueryable<T>().Where(predicate).ToList();
            }
        }

        public ContentSearchResponse ContentSearchIndex(Expression<Func<SolrField, bool>> searchConditions, Expression<Func<SolrField, object>> orderBy, int pageSize, int pagesToSkip, bool orderByDescending = false, bool isAjax = false)
        {
            using (var context = index.CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions.DisableSecurityCheck))
            {
                var totalSearchResults = context.GetQueryable<SolrField>().Where(searchConditions);

                var totalSearchResultCount = totalSearchResults.Count();
                var currentSearchResults = isAjax ? totalSearchResults.Take(pageSize) : totalSearchResults.Skip(pagesToSkip * pageSize).Take(pageSize);
                if (orderBy != null)
                {
                    if (orderByDescending)
                    {
                        currentSearchResults = currentSearchResults.OrderByDescending(orderBy);
                    }
                    else
                    {
                        currentSearchResults = currentSearchResults.OrderBy(orderBy);
                    }
                }
                var currentSearchResultCount = currentSearchResults.Count();
                var searchResultsIterated = totalSearchResultCount > pageSize * (pagesToSkip + 1) ? pageSize * (pagesToSkip + 1) : totalSearchResultCount;

                return new ContentSearchResponse()
                {
                    TotalResultCount = totalSearchResultCount,
                    ResultList = currentSearchResults.ToList(),
                    CurrentResultCount = currentSearchResultCount,
                    IteratedResultCount = searchResultsIterated,
                    RemainingResultCount = totalSearchResultCount - searchResultsIterated,
                };
            }
        }

        public SolrSearchResponse ContentSearchIndex(SolrSearchParameters searchParameters)
        {
            SolrSearchResponse retObj = new SolrSearchResponse();
            using (var context = index.CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck))
            {
                var queryResult = context.Query<SolrModel>(searchParameters.Query, searchParameters.QueryOptions);
                if (queryResult != null)
                {
                    var iteratedResults = searchParameters.QueryOptions.Rows + queryResult.Count ?? 0;
                    retObj = new SolrSearchResponse()
                    {
                        CurrentItem = searchParameters.CurrentItem,
                        SearchTerm = searchParameters.Keyword,
                        CurrentResultCount = queryResult.Count,
                        IteratedResultCount = iteratedResults,
                        RemainingResultCount = queryResult.NumFound - iteratedResults,
                        TotalResultCount = queryResult.Count,
                        QueryResults = queryResult,
                    };
                }
            }

            return retObj;
        }

        public string GetSearchAutoSuggetions(string SearchTerm, int CntResult = 0)
        {
            using (IProviderSearchContext context = index.CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck))
            {

                var results = context.GetSpellCheck(new SolrQuery(string.Format("_name:{0}", SearchTerm)), new SpellCheckHandlerQueryOptions()
                {
                    SpellCheck = new SpellCheckingParameters()
                    {
                        Count = 10,
                        Build = true
                    }
                });

                if (results != null && results.SpellChecking != null && results.SpellChecking.Count > 0)
                {
                    string ret = results.SpellChecking.Select(x => x.Suggestions).FirstOrDefault().Select(x => Convert.ToString(x)).FirstOrDefault();
                    return ret;
                }
                return CntResult.ToString();
            }
        }
    }
}