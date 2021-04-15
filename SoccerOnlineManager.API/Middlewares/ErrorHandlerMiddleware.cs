using Microsoft.AspNetCore.Http;
using SoccerOnlineManager.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                int statusCode;
                string message = string.Empty;

                switch (error)
                {
                    case ApiException e:
                        // custom application error
                        message = e.Message;
                        statusCode = e.StatusCode;
                        break;
                    case AppValidationException e:
                        // custom application error
                        message = e.Message;
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        message = ExceptionCodes.SomethingWentWrong;
                        break;
                }

                response.StatusCode = statusCode;
                var responseModel = new FrontError(statusCode, message, (error as AppValidationException)?.ValidationFailures);

                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(result);
            }
        }
    }
}
