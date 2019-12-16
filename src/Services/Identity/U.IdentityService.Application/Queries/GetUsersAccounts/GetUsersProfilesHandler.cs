using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.IdentityService.Application.Models;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Queries.GetUsersAccounts
{
    public class GetUsersProfilesHandler : IRequestHandler<GetUsersProfiles, IList<UserDto>>
    {
        private readonly IUserRepository _repository;

        public GetUsersProfilesHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<UserDto>> Handle(GetUsersProfiles request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetUsersAsync();
            return users
                .Select(user => new UserDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Role = user.Role,
                    Nickname = user.Nickname,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                })
                .ToList();
        }
    }
}