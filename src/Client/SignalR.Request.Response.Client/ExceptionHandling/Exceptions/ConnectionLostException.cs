using System;

namespace SignalR.Request.Response.Client.ExceptionHandling.Exceptions
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException(string message, Exception innerException)
            : base (message, innerException)
        {
        }
    }
}
