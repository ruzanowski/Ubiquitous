using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using U.Common.NetCore.Mvc;
using U.ProductService.Application.Common.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

#pragma warning disable 1998

namespace U.ProductService.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionMiddleware>();
    }

    /// <summary>
    /// Exception Middleware that catches all exceptions
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ISelfInfoService _selfInfoServiceId;

        public ExceptionMiddleware(RequestDelegate next,
            ISelfInfoService selfInfoServiceId,
            ILogger<ExceptionMiddleware> logger)
        {
            _selfInfoServiceId = selfInfoServiceId;
            _logger = logger;
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = $"{_selfInfoServiceId.Name}:{_selfInfoServiceId.Id}"
            };

            switch (exception.InnerException)
            {
                case ArgumentNullException argumentNullException:
                    problemDetails.Title = nameof(argumentNullException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = argumentNullException.Message;
                    break;
                case ArgumentException argumentException:
                    problemDetails.Title = nameof(argumentException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = argumentException.Message;
                    break;
                case DuplicateNameException duplicateNameException:
                    problemDetails.Title = nameof(duplicateNameException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = duplicateNameException.Message;
                    break;
                case FormatException formatException:
                    problemDetails.Title = nameof(formatException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = formatException.Message;
                    break;
                case InvalidOperationException invalidOperationException:
                    problemDetails.Title = nameof(invalidOperationException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = invalidOperationException.Message;
                    break;
                case ProductNotFoundException productNotFoundException:
                    problemDetails.Title = nameof(productNotFoundException);
                    problemDetails.Status = 404;
                    problemDetails.Detail = productNotFoundException.Message;
                    break;
                case CategoryNotFoundException categoryNotFoundException:
                    problemDetails.Title = nameof(categoryNotFoundException);
                    problemDetails.Status = 404;
                    problemDetails.Detail = categoryNotFoundException.Message;
                    break;
                case ManufacturerNotFoundException manufacturerNotFoundException:
                    problemDetails.Title = nameof(manufacturerNotFoundException);
                    problemDetails.Status = 404;
                    problemDetails.Detail = manufacturerNotFoundException.Message;
                    break;
                case PictureNotFoundException pictureNotFoundException:
                    problemDetails.Title = nameof(pictureNotFoundException);
                    problemDetails.Status = 404;
                    problemDetails.Detail = pictureNotFoundException.Message;
                    break;
                case ProductDuplicatedException productDuplicatedException:
                    problemDetails.Title = nameof(productDuplicatedException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = productDuplicatedException.Message;
                    break;
                default:
                    problemDetails.Title = "An unexpected error occurred!";
                    problemDetails.Status = 500;
                    problemDetails.Detail = exception.StackTrace;
                    break;
            }

            if (problemDetails.Status.Value >= 500)
                _logger.LogError(exception, exception.Message);
            else
                _logger.LogDebug(exception, exception.Message);

            context.Response.StatusCode = problemDetails.Status.Value;
            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }


}