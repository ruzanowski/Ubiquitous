﻿using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Update
{
    public class UpdateProductCommand : IRequest
    {
        [FromRoute] public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DimensionsDto Dimensions { get; set; }
        public Product Product { get; set; }

        [JsonConstructor]
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
        
        public UpdateProductCommand(Product product, string name, decimal price, string description, DimensionsDto dimensions)
        {
            Product = product;
            Name = name;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }
        
    }
}