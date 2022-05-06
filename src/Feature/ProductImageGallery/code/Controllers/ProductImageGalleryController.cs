using CGP.Feature.ProductImageGallery.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;

namespace CGP.Feature.ProductImageGallery.Controllers
{
    public class ProductImageGalleryController : VariantsController
    {
        private readonly IProductImageGalleryRepository productImageGalleryRepository;

        public ProductImageGalleryController(IProductImageGalleryRepository productImageGalleryRepository)
        {
            this.productImageGalleryRepository = productImageGalleryRepository;
        }

        protected override object GetModel()
        {
            return productImageGalleryRepository.GetProductImageGalleryModel();
        }
    }
}
