using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DeveloperPath.Domain.Shared.ProblemDetails;

namespace DeveloperPath.WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger _logger;

        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute(IHostEnvironment environment, ILogger<ApiExceptionFilterAttribute> logger)
        {
            _environment = environment;
            _logger = logger;
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ConflictException), HandleConflictException},
                { typeof(TaskCanceledException), HandleRequestCanceledException},
                { typeof(OperationCanceledException), HandleRequestCanceledException}
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                _logger.LogWarning($"Handled error: {context.Exception.Message}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
            }

            HandleUnknownException(context);
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException exception)
            {
                var details = new NotFoundProblemDetails(exception.Message, exception.ErrorKey)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Detail = "See the errors property for details",
                    Instance = context.HttpContext.Request.Path,
                };
           
                context.Result = new NotFoundObjectResult(details)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ContentTypes = { "application/problem+json" }
                };
                
                context.ExceptionHandled = true;
            }
        }

        private static void HandleConflictException(ExceptionContext context)
        {
            if (context.Exception is ConflictException exception)
            {
                var details = new ConflictProblemDetails(exception.Message, exception.ErrorKey)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    Detail = "See the errors property for details",
                    Instance = context.HttpContext.Request.Path,
                };

                context.Result = new ConflictObjectResult(details)
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    ContentTypes = { "application/problem+json" }
                };

                context.ExceptionHandled = true;
            }
        }

        private static void HandleRequestCanceledException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException exception)
            {
                var details = new RequestCanceledProblemDetails(exception.Message)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Detail = "See the errors property for details",
                    Instance = context.HttpContext.Request.Path,
                };

                context.Result = new ObjectResult(details)
                {
                    StatusCode = 499,
                    ContentTypes = { "application/problem+json" }
                };
        
                context.ExceptionHandled = true;
            }
        }

        private static void HandleValidationException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                var details = new UnprocessableEntityProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Detail = "See the errors property for details",
                    Instance = context.HttpContext.Request.Path,
                    Errors = exception.Errors
                };

                context.Result = new UnprocessableEntityObjectResult(details)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    ContentTypes = {"application/problem+json"}
                };
            
                context.ExceptionHandled = true;
            }
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            //var x = context.ModelState.Select(x=>x.Key+":"+string.Join(x.Value.Errors));
            var details = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = "See the errors property for details",
                Instance = context.HttpContext.Request.Path
            };

            context.Result = new UnprocessableEntityObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentTypes = { "application/problem+json" }
            };
            
            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            ProblemDetailsBase details = default;
            
            details = new ProblemDetailsBase
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Instance = context.HttpContext.Request.Path
            };

            if (_environment.IsDevelopment())
            {
                details.Detail = context.Exception.ToString();
            }

            _logger.LogError($"Unhandled error: {context.Exception}");
            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            
            context.ExceptionHandled = true;
        }
    }
}
