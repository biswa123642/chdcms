using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ProductImage for Search
    /// </summary>
    public class ProductImage : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null)
            {
                switch (indexableItem.TemplateID.ToString())
                {
                    case Templates.ProductDetailPageIdString:
                        return GetProductDefaultImageUrl(indexableItem);
                }
            }
            return null;
        }
        /// <summary>
        /// Get the Default Image Url for the Product
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        private string GetProductDefaultImageUrl(Item productItem)
        {
            Item variantGroupingItem = HelperExtension.GetChildItem(productItem, Templates.VariantGroupingTemplateId);

            Item defaultVariant = HelperExtension.GetProductVariants(variantGroupingItem).FirstOrDefault();

            if (defaultVariant != null)
            {
                List<Item> mediaItem = SitecoreUtil.GetMultiListFieldValues(defaultVariant, "MediaList");

                if (mediaItem.Any())
                {
                    return SitecoreUtil.GetMediaItemUrl(mediaItem.FirstOrDefault(x => x.TemplateID.ToString() != Templates.VideoTemplateId.ToString()));
                }
            }
            return String.Empty;
        }
    }
}