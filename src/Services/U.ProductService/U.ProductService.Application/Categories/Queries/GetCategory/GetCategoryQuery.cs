using System;
using MediatR;
using U.ProductService.Application.Categories.Models;

namespace U.ProductService.Application.Categories.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<CategoryViewModel>
    {
        public Guid Id { get; private set; }

        public GetCategoryQuery(Guid id)
        {
            Id = id;
        }
    }
}