using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;


namespace U.Common.Jwt.Claims
{
    public static class HubCallerUserExtensions
    {
        public static bool IsAuthenticated(this HubCallerContext callerContext) =>
            callerContext?.User?.Identity?.IsAuthenticated ?? false;

        public static bool IsAdmin(this HubCallerContext callerContext) =>
            callerContext?.User?.IsInRole("admin") ?? false;

        public static string GetId(this HubCallerContext callerContext)
        {
            return callerContext.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Id)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetNickname(this HubCallerContext callerContext)
        {
            return callerContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Nickname)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetEmail(this HubCallerContext callerContext)
        {
            return callerContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Email)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetRole(this HubCallerContext callerContext)
        {
            return callerContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Role)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static string GetAccessToken(this HubCallerContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.AccessToken)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static UserDto GetUserOrThrow(this HubCallerContext callerContext)
        {
            var userId = callerContext.GetId();

            if (userId is null)
            {
                throw new UnauthorizedAccessException("Not found claims!!!!");
            }

            var user = new UserDto
            {
                Id = Guid.Parse(userId),
                Email = callerContext.GetEmail(),
                Nickname = callerContext.GetNickname(),
                Role = callerContext.GetRole(),
                AccessToken = callerContext.GetAccessToken()
            };

            return user;
        }
    }
}