using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token
{
    public class TokenBaseHandler
    {
        protected readonly IDistributedCache Cache;
        protected readonly IOptions<JwtOptions> JwtOptions;
        protected readonly IRefreshTokenRepository RefreshTokenRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IJwtHandler JwtHandler;
        protected readonly IClaimsProvider ClaimsProvider;
        protected readonly IEventBus BusPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenBaseHandler(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor, IDistributedCache cache, IRefreshTokenRepository refreshTokenRepository, IJwtHandler jwtHandler, IUserRepository userRepository, IClaimsProvider claimsProvider, IEventBus busPublisher)
        {
            JwtOptions = jwtOptions;
            _httpContextAccessor = httpContextAccessor;
            Cache = cache;
            RefreshTokenRepository = refreshTokenRepository;
            JwtHandler = jwtHandler;
            UserRepository = userRepository;
            ClaimsProvider = claimsProvider;
            BusPublisher = busPublisher;
        }

        protected string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();
        }

        protected static string GetKey(string token)
            => $"tokens:{token}";

        private async Task<bool> IsActiveAsync(string token) =>
            string.IsNullOrWhiteSpace(await Cache.GetStringAsync(GetKey(token)));

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAsync(GetCurrentAsync());
    }
}