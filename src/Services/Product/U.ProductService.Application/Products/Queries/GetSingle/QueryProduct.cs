using System;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetSingle
{
    public class QueryProduct : IRequest<ProductViewModel>
    {
        public Guid Id { get; private set; }

        public QueryProduct(Guid id)
        {
            Id = id;
        }
    }    
}