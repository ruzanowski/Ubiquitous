using System;
using MediatR;
using U.IdentityService.Application.Models;

namespace U.IdentityService.Application.Queries.GetMyAccount
{
    public class GetMyProfile : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }
}