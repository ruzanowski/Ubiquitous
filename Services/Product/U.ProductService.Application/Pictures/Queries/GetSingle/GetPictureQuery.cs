using System;
using MediatR;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Queries.QueryPicture
{
    public class GetPictureQuery : IRequest<PictureViewModel>
    {
        public Guid Id { get; private set; }

        public GetPictureQuery(Guid id)
        {
            Id = id;
        }
    }    
}