using System.Collections.Generic;
using MediatR;
using U.IdentityService.Application.Models;

namespace U.IdentityService.Application.Queries.GetUsersAccounts
{
    public class GetUsersProfiles : IRequest<IList<UserDto>>
    {
    }
}