﻿using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Manufacturers.Commands.AddPicture
{
    public class AttachManufacturerPictureCommand : IRequest
    {
        [FromRoute] public Guid ManufacturerId { get; set; }

        public Guid PictureId { get; set; }
    }
}