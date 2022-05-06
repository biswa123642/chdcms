using CGP.Foundation.SitecoreExtensions.DataItems;
using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the list of Product Attribute List
    /// </summary>
    public class ProductAttributeList : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null && indexableItem.TemplateID.ToString().Equals(Templates.ProductDetailPageIdString, StringComparison.OrdinalIgnoreCase))
            {
                ProductBaseItem productBaseItem = new ProductBaseItem(indexableItem);

                IDictionary<string, IEnumerable<string>> attributeValues = productBaseItem == null || productBaseItem.Attributes == null || !productBaseItem.Attributes.Any() ? null : productBaseItem.Attributes.AttributeValues;
                if (attributeValues != null)
                {
                    return JsonConvert.SerializeObject(attributeValues);
                }
            }
            return string.Empty;
        }
    }
}