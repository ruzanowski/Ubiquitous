using System;
using System.Data;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U.SmartStoreAdapter.Application.Common.Exceptions;
using U.SmartStoreAdapter.Extensions;

#pragma warning disable 1998

namespace U.SmartStoreAdapter.Middleware
{
    /// <summary>
    /// Exception Middleware that catches all exceptions
    /// </summary>
    public static class ExceptionMiddleware
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder AddExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(ExceptionMiddlewarePipeline);
            });
            return app;
        }

        private static async Task ExceptionMiddlewarePipeline(HttpContext context)
        {
            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = errorFeature.Error;

            // the IsTrusted() extension method doesn't exist and
            // you should implement your own as you may want to interpret it differently
            // i.e. based on the current principal

            var problemDetails = new ProblemDetails {Instance = $"instance:SmartStoreAdapter:error:{Guid.NewGuid()}"};


            switch (exception)
            {
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
                case MissingMethodException missingMethodException:
                    problemDetails.Title = nameof(missingMethodException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = missingMethodException.Message;
                    break;
                case ValidationException validationException:
                    problemDetails.Title = nameof(validationException);
                    problemDetails.Status = 400;
                    problemDetails.Detail = validationException.Message;
                    break;
                case ObjectNotFoundException objectNotFoundException:
                    problemDetails.Title = nameof(objectNotFoundException);
                    problemDetails.Status = 404;
                    problemDetails.Detail = objectNotFoundException.Message;
                    break;
                default:
                    problemDetails.Title = "An unexpected error occurred!";
                    problemDetails.Status = 500;
                    problemDetails.Detail = exception.StackTrace;
                    break;
            }
            // log the exception etc..
            // logger.LogInformation(problemDetails.ToString());

            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.WriteJson(problemDetails, "application/problem+json");
        }
    }
}