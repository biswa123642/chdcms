using CGP.Feature.ProductVariant.Models;

namespace CGP.Feature.ProductVariant.Repositories
{
    public interface IProductVariantRepository
    {
        ProductVariantViewModel GetProductVariantDetails();
    }
}