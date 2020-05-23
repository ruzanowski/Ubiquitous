using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace U.Common.NetCore.Auth.Attributes
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}