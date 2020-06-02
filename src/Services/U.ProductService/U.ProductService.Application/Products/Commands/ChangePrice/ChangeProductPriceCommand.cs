using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    public class ChangeProductPriceCommand : IRequest
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }

        public ChangeProductPriceCommand(Guid id, decimal price)
        {
            Id = id;
            Price = price;
        }
    }
}