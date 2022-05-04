using System.ComponentModel.DataAnnotations;
using System.Net;
using Fayble.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fayble;

public interface IProblemDetailsFactory
{
    ProblemDetails CreateProblemDetails(HttpContext context, Exception ex, bool includeStackTraceInResponse);
}

public class ProblemDetailsFactory : IProblemDetailsFactory
{
        public ProblemDetails CreateProblemDetails(HttpContext context, Exception ex, bool includeStackTraceInResponse)
        {
            var result = new ProblemDetails
            {
                Detail = includeStackTraceInResponse ? ex.ToString() : ex.Message,
            };
        
            switch (ex)
            {
                case NotFoundException _:
                    result.Status = context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    result.Title = "Not Found";
                    result.Type = ex?.GetType().Name;
                break;
                case NotAuthorisedException _:
                    result.Status = context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    result.Title = "Not Authorised";
                    result.Type = ex?.GetType().Name;
                break;
                case ValidationException _:
                    result.Status = context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    result.Title = "Invalid";
                    result.Type = ex?.GetType().Name;
                break;
                default:
                    result.Status = context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result.Title = "Unexpected error occurred";
                    result.Type = ex?.GetType().Name;
                break;
            }

            return result;
        }
}

