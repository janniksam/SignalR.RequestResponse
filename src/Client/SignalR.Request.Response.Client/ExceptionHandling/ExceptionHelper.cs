using System;

namespace SignalR.Request.Response.Client.ExceptionHandling
{
    public static class ExceptionHelper
    {
        public static void LogAndThrow(this Exception exception)
        {
            Connection.Logger.LogError(exception.Message);
            throw exception;
        }
    }
}
