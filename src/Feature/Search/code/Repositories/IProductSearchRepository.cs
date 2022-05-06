using CGP.Feature.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Feature.Search.Repositories
{
    public interface IProductSearchRepository
    {
        ProductSearchViewModel GetProductListing(FilterQueryFields filterQueryFields);

       // List<IResult> GetProductListing(FilterQueryFields filterQueryFields, string IndexName);
    }
}
