using System;
using System.Net;

namespace SocketChat.Domain.Exceptions
{ 
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public override string Message { get; }

        public AppException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
    }
}
