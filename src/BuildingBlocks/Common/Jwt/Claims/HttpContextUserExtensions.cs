using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using U.NotificationService.Application.Services.Users;

namespace U.Common.Jwt.Claims
{
    public static class HttpContextClaimsExtensions
    {
        public static bool IsAuthenticated(this HttpContext httpContext) => httpContext?.User?.Identity?.IsAuthenticated ?? false;

        public static bool IsAdmin(this HttpContext httpContext) => httpContext?.User?.IsInRole("admin") ?? false;

        public static string GetId(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Sub)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetNickname(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Nickname)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetEmail(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Email)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetRole(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Role)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetAccessToken(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.AccessToken)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static UserDto GetUser(this HttpContext callerContext)
        {
            var userId = callerContext.GetId();

            var user = new UserDto
            {
                Id = userId is null? Guid.Empty : Guid.Parse(userId),
                Email = callerContext.GetEmail(),
                Nickname = callerContext.GetNickname(),
                Role = callerContext.GetRole(),
                AccessToken = callerContext.GetAccessToken()

            };

            return user;
        }
    }
}