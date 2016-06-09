using SignalR.Request.Response.Shared.Logging;
using System;

namespace SignalR.Request.Response.Client.ExceptionHandling
{
    public static class ExceptionHelper
    {
        public static void LogAndThrow(this IClientLogger logger, Exception exception)
        {
            logger.LogError(exception.Message);
            throw exception;
        }
    }
}
