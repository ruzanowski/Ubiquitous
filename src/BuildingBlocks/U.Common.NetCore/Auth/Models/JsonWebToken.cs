using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace U.Common.NetCore.Auth.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        [JsonPropertyName("userId")]
        public string Id { get; set; }
        [JsonPropertyName("userRole")]
        public string Role { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}