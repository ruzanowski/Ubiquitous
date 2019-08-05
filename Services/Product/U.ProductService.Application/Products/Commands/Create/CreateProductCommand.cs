using System;
using MediatR;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get;  set; }
        public string BarCode { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public Dimensions Dimensions { get;  set; }

        public CreateProductCommand(string name, string barCode, decimal price, string description, Dimensions dimensions)
        {
            Name = name;
            BarCode = barCode;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }
    }
}
