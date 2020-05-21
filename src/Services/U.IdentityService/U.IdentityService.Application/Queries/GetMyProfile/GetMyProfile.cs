using System;
using MediatR;
using U.IdentityService.Application.Models;

namespace U.IdentityService.Application.Queries.GetMyProfile
{
    public class GetMyProfile : IRequest<UserDto>
    {
        public GetMyProfile(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; private set; }
    }
}