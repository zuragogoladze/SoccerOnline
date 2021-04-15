using System;
using System.Net;

namespace SoccerOnlineManager.Application.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; protected set; }

        public ApiException(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        public ApiException(string message) : base(message)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public ApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
        }
    }
}
