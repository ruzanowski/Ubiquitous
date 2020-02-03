using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetSingleByAlternativeKey
{
    public class QueryProductByAlternativeKey : IRequest<ProductViewModel>
    {
        public string AlternativeKey { get; set; }
    }    
}