using System;
using MediatR;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public Guid? ProductId { get; set; }
        public string Name { get;  set; }
        public string AlternateId { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public Dimensions Dimensions { get;  set; }

        public UpdateProductCommand(Guid? productId, string name, string alternateId, decimal price, string description, Dimensions dimensions)
        {
            ProductId = productId;
            Name = name;
            AlternateId = alternateId;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }
    }
}
