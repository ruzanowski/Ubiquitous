using System;
using Newtonsoft.Json;

namespace U.IdentityService.Application.Commands
{
    public class RevokeAccessToken
    {
        public Guid UserId { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeAccessToken(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}