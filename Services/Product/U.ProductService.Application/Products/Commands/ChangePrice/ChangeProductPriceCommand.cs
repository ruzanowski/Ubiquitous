using System;
using MediatR;
using Newtonsoft.Json;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    public class ChangeProductPriceCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public decimal Price { get;  set; }

        public ChangeProductPriceCommand(Guid productId, decimal price)
        {
            ProductId = productId;
            Price = price;
        }

        public ChangeProductPriceCommand BindProductId(Guid productId)
        {
            ProductId = productId;
            return this;
        }
    }
}
