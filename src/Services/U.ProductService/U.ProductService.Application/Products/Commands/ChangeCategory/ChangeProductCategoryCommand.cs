using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.ProductService.Application.Products.Commands.ChangeCategory
{
    public class ChangeProductCategoryCommand : IRequest
    {
        [FromRoute] public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
    }
}