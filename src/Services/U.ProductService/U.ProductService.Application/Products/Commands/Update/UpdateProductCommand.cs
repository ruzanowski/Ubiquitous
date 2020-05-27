using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Update
{
    public class UpdateProductCommand : IRequest, IQueueable
    {
        [FromRoute] public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DimensionsDto Dimensions { get; set; }

        public QueuedJob QueuedJob { get; private set; }
        public UpdateProductCommand()
        {

        }

        public UpdateProductCommand(Guid productId, string name, decimal price, string description, DimensionsDto dimensions)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }

        public void SetAsQueueable()
        {
            QueuedJob = new QueuedJob
            {
                AutoSave = true,
                DateTimeQueued = DateTime.UtcNow
            };
        }
    }
}