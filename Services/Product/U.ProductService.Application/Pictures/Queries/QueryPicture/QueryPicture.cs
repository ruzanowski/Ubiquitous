using System;
using MediatR;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Queries.QueryPicture
{
    public class QueryPicture : IRequest<PictureViewModel>
    {
        public Guid Id { get; private set; }

        public QueryPicture(Guid id)
        {
            Id = id;
        }
    }    
}