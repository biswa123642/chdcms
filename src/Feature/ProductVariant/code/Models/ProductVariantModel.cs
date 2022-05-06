using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace CGP.Feature.ProductVariant.Models
{
    public class ProductVariantDetails
    {
        public string Primary { get; set; }
        public string Secondary { get; set; }
        public string Tertiary { get; set; }
        public string VariantSKU { get; set; }
        public string VariantDescription { get; set; }
    }

    public class ProductVariantModel
    {
        public string PrimaryTitle { get; set; }
        public string SecondryTitle { get; set; }
        public string TertiaryTitle { get; set; }
        public List<ProductVariantDetails> ProductVariantList { get; set; }
    }
    public class ProductVariantViewModel : VariantsRenderingModel
    {
        public ProductVariantModel ProductVariantModel { get; set; }
        public string JSONData { get; set; }
    }
}