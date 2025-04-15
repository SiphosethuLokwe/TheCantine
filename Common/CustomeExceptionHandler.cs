using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Application.Common
{
    public  class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            int status = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                TimeoutException => StatusCodes.Status408RequestTimeout,
                InvalidOperationException => StatusCodes.Status500InternalServerError,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
            httpContext.Response.StatusCode = status;

            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = "An error occurred",
                Type = exception.GetType().Name,
                Detail = exception.Message
            };

            problemDetails.Extensions.TryAdd("requestId", httpContext.TraceIdentifier);
            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            problemDetails.Extensions.TryAdd("traceId", activity?.Id);

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


            return true;
        }
    }
}
