using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using U.Common.Jwt.Claims;

namespace U.Common.Jwt.Service
{
    public class JwtService : IJwtService
    {
        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtClaimsTypes.Sub,
            JwtClaimsTypes.UniqueName,
            JwtClaimsTypes.Jti,
            JwtClaimsTypes.Iat,
            JwtClaimsTypes.Role
        };

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public JwtService(JwtOptions options, IDistributedCache cache, IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions)
        {
            _options = options;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issuerSigningKey,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.ValidAudience,
                ValidateAudience = _options.ValidateAudience,
                ValidateLifetime = _options.ValidateLifetime
            };
        }

        public JsonWebToken CreateToken(string userId, string role = null, IList<Claim> claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User id claim can not be empty.", nameof(userId));
            }

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtClaimsTypes.Sub, userId),
                new Claim(JwtClaimsTypes.UniqueName, userId),
                new Claim(JwtClaimsTypes.Name, userId),
                new Claim(JwtClaimsTypes.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtClaimsTypes.Iat, now.ToTimestamp().ToString())
            };
            if (!string.IsNullOrWhiteSpace(role))
            {
                jwtClaims.Add(new Claim(JwtClaimsTypes.Role, role));
            }

            var customClaims = claims?.ToList() ?? new List<Claim>();

            jwtClaims.AddRange(customClaims);
            var expires = now.AddMinutes(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );
            var token = new JwtSecurityTokenHandler();
            token.InboundClaimTypeMap.Clear();
            var jwtToken = token.WriteToken(jwt);

            customClaims.Add(new Claim(JwtClaimsTypes.AccessToken, jwtToken));

            return new JsonWebToken
            {
                AccessToken = jwtToken,
                RefreshToken = string.Empty,
                Expires = expires,
                Id = userId,
                Role = role ?? string.Empty,
                Claims = customClaims.ToDictionary(c => c.Type, c => c.Value)
            };
        }

        public JsonWebTokenPayload GetTokenPayload(string accessToken)
        {
            _jwtSecurityTokenHandler.ValidateToken(
                accessToken,
                _tokenValidationParameters,
                out var validatedSecurityToken);

            if (!(validatedSecurityToken is JwtSecurityToken jwt))
            {
                return null;
            }

            return new JsonWebTokenPayload
            {
                Subject = jwt.Subject,
                Role = jwt.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                Expires = jwt.ValidTo,
                Claims = jwt.Claims.Where(x => !DefaultClaims.Contains(x.Type))
                    .ToDictionary(k => k.Type, v => v.Value)
            };
        }

        public JsonWebTokenPayload GetTokenCurrentPayload()
        {
            var currentToken = GetCurrentAsync();

            var currentTokenPayload = GetTokenPayload(currentToken);
            return currentTokenPayload;
        }

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            var cached = await _cache.GetStringAsync(GetKey(token));

            return string.IsNullOrEmpty(cached);
        }

        public async Task DeactivateAsync(string token)
        {
            await _cache.SetStringAsync(GetKey(token),
                "deactivated", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                });
        }

        private string GetCurrentAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if(httpContext.Request.Path.StartsWithSegments("/signalr"))
            {
                var accessToken = httpContext.Request.Query["access_token"];

                return accessToken;
            }

            var authorizationHeader = httpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();


        }

        private static string GetKey(string token) => $"tokens:{token}";
    }
}