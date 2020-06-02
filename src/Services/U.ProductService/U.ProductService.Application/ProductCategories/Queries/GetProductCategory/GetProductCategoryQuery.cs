using System;
using MediatR;
using U.ProductService.Application.ProductCategories.Models;

namespace U.ProductService.Application.ProductCategories.Queries.GetProductCategory
{
    public class GetProductCategoryQuery : IRequest<ProductCategoryViewModel>
    {
        public Guid Id { get; private set; }

        public GetProductCategoryQuery(Guid id)
        {
            Id = id;
        }
    }
}