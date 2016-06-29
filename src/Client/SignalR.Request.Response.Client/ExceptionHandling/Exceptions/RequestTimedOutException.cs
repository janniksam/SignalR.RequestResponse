using System;

namespace SignalR.Request.Response.Client.ExceptionHandling.Exceptions
{
    public class RequestTimedOutException : Exception
    {
        public RequestTimedOutException(string message) 
            : base (message)
        {
        }
    }
}
