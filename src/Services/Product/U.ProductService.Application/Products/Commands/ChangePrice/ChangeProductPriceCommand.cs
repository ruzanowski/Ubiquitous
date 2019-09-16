using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    public class ChangeProductPriceCommand : IRequest
    {
        [FromRoute] public Guid ProductId { get; set; }
        public decimal Price { get; set; }
    }
}