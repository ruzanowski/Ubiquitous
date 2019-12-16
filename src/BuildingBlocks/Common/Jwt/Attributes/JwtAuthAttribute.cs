using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace U.Common.Jwt.Attributes
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}