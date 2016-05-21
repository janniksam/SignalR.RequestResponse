using System;

namespace SignalR.Request.Response.Client.ExceptionHandling.Exceptions
{
    public class RequestAbortedException : Exception
    {
        public RequestAbortedException(string message) : base (message)
        {
        }
    }
}
