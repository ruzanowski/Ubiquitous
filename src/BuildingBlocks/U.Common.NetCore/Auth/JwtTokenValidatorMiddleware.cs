using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using U.Common.NetCore.Auth.Service;

namespace U.Common.NetCore.Auth
{
    public class JwtTokenValidatorMiddleware : IMiddleware
    {
        private readonly IJwtService _jwtService;

        public JwtTokenValidatorMiddleware(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _jwtService.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }

            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}