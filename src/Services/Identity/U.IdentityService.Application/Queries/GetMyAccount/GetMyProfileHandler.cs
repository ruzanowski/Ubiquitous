using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.IdentityService.Application.Models;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Queries.GetMyProfile
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfile, UserDto>
    {
        private readonly IUserRepository _repository;

        public GetMyProfileHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(GetMyProfile request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAsync(request.UserId);

            var userDto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Role = user.Role,
                Nickname = user.Nickname,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userDto;
        }
    }
}