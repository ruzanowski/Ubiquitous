using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.Publish
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
