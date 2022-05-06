using SolrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Models
{
    public class SolrSearchResponse : SearchResponse
    {
        public SolrQueryResults<SolrModel> QueryResults { get; set; }
    }
}