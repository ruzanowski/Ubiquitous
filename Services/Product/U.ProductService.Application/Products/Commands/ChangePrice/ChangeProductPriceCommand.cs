using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    public class ChangeProductPriceCommand : IRequest
    {
        [FromRoute] public Guid ProductId { get; set; }
        public decimal Price { get; set; }
    }
}