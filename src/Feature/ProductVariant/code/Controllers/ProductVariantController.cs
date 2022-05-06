using CGP.Feature.ProductVariant.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;


namespace CGP.Feature.ProductVariant.Controllers
{
    public class ProductVariantController : VariantsController
    {
        private readonly IProductVariantRepository productVariantRepository;

        public ProductVariantController(IProductVariantRepository productVariantRepository)
        {
            this.productVariantRepository = productVariantRepository;
        }
        protected override object GetModel()
        {
            return productVariantRepository.GetProductVariantDetails();
        }
    }
}