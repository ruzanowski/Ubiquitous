using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.UnPublish
{
    //marker
    public class UnPublishProductCommand : IRequest
    {
        public UnPublishProductCommand(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; private set; }
    }
}
