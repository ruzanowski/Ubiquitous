using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetSingleByExternalTuple
{

    /// <summary>
    /// External tuple is: External Source Name && External Source Id
    /// </summary>
    public class QueryProductByExternalTuple : IRequest<ProductViewModel>
    {
        public string ExternalSourceName { get; set; }
        public string ExternalSourceId { get; set; }
    }
}