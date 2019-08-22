using System;
using MediatR;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Queries.QueryPicture
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