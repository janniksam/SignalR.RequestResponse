using System;

namespace SignalR.Request.Response.Client.ExceptionHandling
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException(string message, Exception innerException)
            : base (message, innerException)
        {
        }
    }
}
