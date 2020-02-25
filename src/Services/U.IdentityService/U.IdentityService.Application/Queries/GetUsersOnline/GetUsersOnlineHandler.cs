using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Queries.GetUsersOnline
{
    public class GetUsersOnlineHandler : IRequestHandler<GetUsersOnline, int>
    {
        private readonly IRefreshTokenRepository _repository;

        public GetUsersOnlineHandler(IRefreshTokenRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(GetUsersOnline request, CancellationToken cancellationToken)
        {
            var refreshTokens = await _repository.GetActiveAsync();

            return refreshTokens?.Count ?? 0;
        }
    }
}