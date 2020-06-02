using System;
using MediatR;
using Newtonsoft.Json;
using U.Common.NetCore.Swagger;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Commands.Update
{
    public class UpdateProductCommand : IRequest, IQueueable
    {
        [JsonIgnore]
        [SwaggerExclude]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DimensionsDto Dimensions { get; set; }
        public QueuedJob QueuedJob { get; private set; }

        public UpdateProductCommand()
        {

        }

        public UpdateProductCommand(Guid id, string name, decimal price, string description, DimensionsDto dimensions)
        {
            Id = id;
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