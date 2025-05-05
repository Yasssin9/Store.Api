﻿using System.Net;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.Api.MiddleWares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int) HttpStatusCode.NotFound)
                    await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = $"The End Point {httpContext.Request.Path} Is Not Found",
                StatusCode = (int)HttpStatusCode.NotFound,
            }.ToString();

            await httpContext.Response.WriteAsync(response);
        }

        private async Task HandleExceptionAsync(HttpContext httpContext,Exception ex)
        {
            //httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message,
                
            };

            //TODO =>
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundExceptions => (int)HttpStatusCode.NotFound,
                ValidationException validationException => HandleValidationException(validationException, response),
                _ => (int)HttpStatusCode.InternalServerError,
            };

            response.StatusCode = httpContext.Response.StatusCode;

            await httpContext.Response.WriteAsync(response.ToString());
        }
        private int  HandleValidationException(ValidationException ex,ErrorDetails details)
        {
            details.Errors=ex.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}