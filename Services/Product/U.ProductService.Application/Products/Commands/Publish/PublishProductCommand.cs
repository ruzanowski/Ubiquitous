using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands
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
