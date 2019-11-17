using System;
using Newtonsoft.Json;

namespace U.IdentityService.Application.Commands
{
    public class RevokeRefreshToken
    {
        public Guid UserId { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeRefreshToken(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}