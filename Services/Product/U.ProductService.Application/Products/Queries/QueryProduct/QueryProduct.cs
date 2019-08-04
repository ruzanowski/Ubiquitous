using System;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.QueryProduct
{
    public class QueryProduct : IRequest<ProductViewModel>
    {
        public Guid Id { get; set; }
    }    
}