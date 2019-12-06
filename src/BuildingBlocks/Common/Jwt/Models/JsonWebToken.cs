using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace U.Common.Jwt
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        [JsonProperty("userId")]
        public string Id { get; set; }
        [JsonProperty("userRole")]
        public string Role { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}