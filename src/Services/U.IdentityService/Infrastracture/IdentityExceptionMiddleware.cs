using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using U.IdentityService.Domain.Exceptions;

namespace U.IdentityService.Infrastracture
{
    public static class IdentityExceptionMiddleware
    {

        public static IApplicationBuilder AddIdentityErrorsHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp => errorApp.Run(async x => await ExceptionPipeline(x)));
            return app;
        }

        private static async Task ExceptionPipeline(HttpContext context)
        {
            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = errorFeature.Error;

            var errorCode = "error";
            var statusCode = HttpStatusCode.BadRequest;
            var message = "There was an error.";
            switch (exception)
            {
                case IdentityException e:
                    errorCode = e.Code;
                    message = e.Message;
                    break;
            }

            var response = new {code = errorCode, message = exception.Message};
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            await context.Response.WriteAsync(payload);
        }
    }
}