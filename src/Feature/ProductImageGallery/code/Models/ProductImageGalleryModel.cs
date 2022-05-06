using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace CGP.Feature.ProductImageGallery.Models
{
    public class ProductVariant
    {
        public string VariantSKU { get; set; }
        public List<ProductMedia> ProductMediaList { get; set; }
    }
    public class ProductMedia
    {
        public string YoutubeId { get; set; }
        public string MediaURL { get; set; }
        public string MediaType { get; set; }
    }
    public class ProductImageGalleryModel
    {
        public bool IsCarousel { get; set; }
        public List<ProductVariant> ProductVariantList { get; set; }
    }

    public class ProductImageGalleryViewModel : VariantsRenderingModel
    {
        public string JSONData { get; set; }
        public ProductImageGalleryModel ProductImageGalleryModel { get; set; }
    }
}