using System;

namespace SignalR.Request.Response.Client.ExceptionHandling.Exceptions
{
    public class ConnectionCouldNotBeEstablishedException : Exception
    {
        public ConnectionCouldNotBeEstablishedException(string message, Exception innerException) 
            : base (message, innerException)
        {
        }
    }
}
