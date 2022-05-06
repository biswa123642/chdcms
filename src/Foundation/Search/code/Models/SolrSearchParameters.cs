using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Models
{
    public class SolrSearchParameters : SearchParameters
    {
        public SolrSearchParameters()
        {
            QueryOptions = new QueryOptions();
        }
        public string Keyword { get; set; }
        //public string IndexName { get; set; }
        public ISolrQuery Query { get; set; }
        public QueryOptions QueryOptions { get; set; }
        public bool IsLoadMore { get; set; }
    }
}