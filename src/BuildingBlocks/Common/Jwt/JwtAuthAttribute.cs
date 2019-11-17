using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace U.Common.Jwt
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}