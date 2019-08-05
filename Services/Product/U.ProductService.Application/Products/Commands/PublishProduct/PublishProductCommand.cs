using System;
using MediatR;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.PublishProduct
{
    //marker
    public class PublishProductCommand : IRequest
    {
        public PublishProductCommand(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; private set; }
    }
}
