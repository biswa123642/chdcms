using CGP.Feature.Search.Models;
using CGP.Feature.Search.Repositories;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CGP.Feature.Search.Controllers
{
    public class GlobalSearchController : SitecoreController
    {
        private IGlobalSearchRepository _globalSearchRepository { get; set; }

        public GlobalSearchController(IGlobalSearchRepository globalSearchRepository)
        {
            this._globalSearchRepository = globalSearchRepository;
        }

        public ContentResult GetSuggestions(string searchTerm, string searchItemId, string indexName = null)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var result = _globalSearchRepository.GetSuggestions(searchTerm, searchItemId, indexName);
                return Content(result.ToString(), "application/json");
            }
            return new ContentResult();
        }

        public ActionResult GlobalSearchBox()
        {
            return View("~/Views/OneWeb/Search/GlobalSearchBox.cshtml");
        }

        public ActionResult GlobalSearch()
        {
            var searchParameters = ParseQueryStringParameters();
            // if (searchParameters != null && !string.IsNullOrEmpty(searchParameters.SearchTerm))
            {
                var globalResults = _globalSearchRepository.GetGlobalSearchResults(searchParameters);
                //globalResults = SetActiveFacets(globalResults, searchParameters.Filters);
                if (globalResults != null)
                {
                    return View("~/Views/OneWeb/Search/GlobalSearch.cshtml", globalResults);
                }
                else
                {
                    return View("~/Views/OneWeb/Search/GlobalSearchBox.cshtml");
                }
            }
        }

        private InputParameters ParseQueryStringParameters(string searchTerm = null, string filterString = null, string currentItemId = null)
        {
            InputParameters inputParameters = new InputParameters();
            Item currentItem = Sitecore.Context.Item;
            if (currentItem == null && currentItemId != null)
            {
                currentItem = Sitecore.Context.Database.GetItem(currentItemId);
            }
            //var globalFacets = SearchExtensions.GetGlobalFacets(currentItem);
            searchTerm = searchTerm ?? Request.QueryString["q"];
            filterString = filterString ?? Request.QueryString["f"];
            inputParameters.SearchTerm = !string.IsNullOrWhiteSpace(searchTerm)
                ? Convert.ToString(searchTerm)
                : string.Empty;
            inputParameters.FilterString = filterString;
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var filterCollection = filterString.Split(';');
                if (filterCollection.Any())
                {
                    foreach (var filter in filterCollection)
                    {
                        var filterSections = filter.Split(':');
                        var filterKey = filterSections[0];
                        var filterValues = filterSections[1];
                        if (!string.IsNullOrEmpty(filterKey) && !string.IsNullOrEmpty(filterValues))
                        {
                            //var parsedFacet = globalFacets.FirstOrDefault(x =>
                            //    x.FacetIdentifier.Equals(filterKey, StringComparison.InvariantCultureIgnoreCase));
                            //if (parsedFacet != null)
                            //{
                            //    var filterObject = new InputFilter()
                            //    {
                            //        FilterName = parsedFacet.FacetName,
                            //        FilterKey = parsedFacet?.FacetField,
                            //        FilterValues = filterValues.Split('|').ToList(),
                            //        IsRangeFilter = parsedFacet.FacetType.Equals(FacetType.Range),
                            //    };
                            //    inputParameters.Filters.Add(filterObject);
                            //}
                        }
                    }
                }
            }
            return inputParameters;
        }

        private GlobalSearchResult SetActiveFacets(GlobalSearchResult searchResult, List<InputFilter> filters)
        {
            foreach (var searchFacet in searchResult?.Facets)
            {
                var matchedFilter = filters.FirstOrDefault(x => x.FilterName.Equals(searchFacet.FacetName, StringComparison.InvariantCultureIgnoreCase));
                if (matchedFilter != null)
                {
                    foreach (var facet in searchFacet.FacetValues)
                    {
                        if (searchFacet.FacetType.Equals(FacetType.Range))
                        {
                            searchFacet.RangeActiveValue = matchedFilter.FilterValues.FirstOrDefault();
                        }
                        else
                        {
                            if (matchedFilter.FilterValues.Contains(facet.Key))
                            {
                                facet.IsActive = true;
                            }
                        }
                    }
                }
            }

            return searchResult;
        }
    }
}