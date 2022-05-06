using System;
using System.Collections.Generic;
using Sitecore.ContentSearch.Linq;

namespace CGP.Foundation.Search.Models
{
    [Serializable]
    public class ContentSearchResponse : SearchResponse
    {
        public IEnumerable<SolrField> ResultList { get; set; }
    }
}