using System;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<Guid>, IQueueable
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public DimensionsDto Dimensions { get; set; }
        public ExternalCreation ExternalProperties { get; set; }
        public QueuedJob QueuedJob { get; private set; }

        public CreateProductCommand()
        {
        }

        public CreateProductCommand(string name,
            string barCode,
            decimal price,
            string description,
            DimensionsDto dimensions,
            ExternalCreation externalProperties = null,
            Guid? manufacturerId = null,
            Guid? categoryId = null)
        {
            Name = name;
            BarCode = barCode;
            Price = price;
            Description = description;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
            Dimensions = dimensions;
            ExternalProperties = externalProperties;
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