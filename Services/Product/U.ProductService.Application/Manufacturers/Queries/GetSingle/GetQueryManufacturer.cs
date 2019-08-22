using System;
using MediatR;
using U.ProductService.Application.Manufacturers.Models;

namespace U.ProductService.Application
{
    public class QueryManufacturer : IRequest<ManufacturerViewModel>
    {
        public Guid Id { get; private set; }

        public QueryManufacturer(Guid id)
        {
            Id = id;
        }
    }    
}