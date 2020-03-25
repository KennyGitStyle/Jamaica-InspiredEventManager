using System.Net;
using System;

namespace Jam_Inspired_Application.ErrorHandling
{
    public class RestExceptionHandling : Exception
    {
        public HttpStatusCode Status { get; }
        public object Error { get; }
        public RestExceptionHandling(HttpStatusCode status, object error =null)
        {
            Status = status;
            Error = error;
        }
    }
}