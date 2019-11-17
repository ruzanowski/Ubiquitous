using Newtonsoft.Json;

namespace U.IdentityService.Application.Commands
{
    public class RefreshAccessToken
    {
        public string Token { get; }

        [JsonConstructor]
        public RefreshAccessToken(string token)
        {
            Token = token;
        }
    }
}