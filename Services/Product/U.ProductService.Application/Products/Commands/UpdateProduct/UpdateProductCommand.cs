using System;
using MediatR;
using Newtonsoft.Json;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid ProductId { get; set; }
        public string Name { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public Dimensions Dimensions { get;  set; }

        public UpdateProductCommand(Guid productId, string name, decimal price, string description, Dimensions dimensions)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }

        public UpdateProductCommand BindProductId(Guid productId)
        {
            ProductId = productId;
            return this;
        }
    }
}
