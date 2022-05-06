using System;
using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace CGP.Foundation.Search.Models
{
    public class SolrField : SearchResultItem
    {
        [IndexField("_created")]
        public DateTime DateCreated { get; set; }

        [IndexField("releasedate")]
        public DateTime ReleasedDate { get; set; }

        [IndexField("_path")]
        public List<Guid> PathGuid { get; set; }

        [IndexField("contenturl_t")]
        public string ContentUrl { get; set; }

        [IndexField("title")]
        public string Title { get; set; }

        [IndexField("opengraphtitle")]
        public string OpengraphTitle { get; set; }

        [IndexField("opengraphdescription")]
        public string OpengraphDescription { get; set; }

        [IndexField("productimage_t")]
        public string ProductImage { get; set; }

        [IndexField("contentdesktopimage_t")]
        public string ContentDesktopImage { get; set; }

        [IndexField("contentmobileimage_t")]
        public string ContentMobileImage { get; set; }

        [IndexField("contenttype_t")]
        public string ContentType { get; set; }

        [IndexField("page_attributes_sm")]
        public IEnumerable<string> PageAttributes { get; set; }

        [IndexField("product_attributes_sm")]
        public IEnumerable<string> ProductAttribute { get; set; }

        [IndexField("search_facet_attributes_sm")]
        public IEnumerable<string> SearchFacetAttributes { get; set; }

        [IndexField("facet_attributes_sm")]
        public IEnumerable<string> FacetAttributes { get; set; }

        [IndexField("product_details_attributes_sm")]
        public IEnumerable<string> ProductDetailsAttributes { get; set; }

        [IndexField("secondary_categories_sm")]
        public IEnumerable<string> SecondaryCategories { get; set; }

        [IndexField("hideinsearchresults")]
        public bool HideInSearchResults { get; set; }
    }
}