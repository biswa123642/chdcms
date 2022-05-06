using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Models
{
    public class FacetModel
    {
        public string FacetIdentifier { get; set; }
        public string FacetName { get; set; }
        public string FacetField { get; set; }
        public int FacetThreshold { get; set; }
        public FacetType FacetType { get; set; }
    }
}