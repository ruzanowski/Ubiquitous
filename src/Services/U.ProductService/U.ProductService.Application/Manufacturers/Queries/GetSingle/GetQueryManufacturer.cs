using System;
using MediatR;
using U.ProductService.Application.Manufacturers.Models;

namespace U.ProductService.Application.Manufacturers.Queries.GetSingle
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