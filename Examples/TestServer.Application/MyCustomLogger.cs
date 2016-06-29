using System;
using SignalR.Request.Response.Shared.Logging;

namespace TestServer.Application
{
    public class MyCustomLogger : IServerLogger
    {
        public void LogInfo(string text)
        {
            Console.WriteLine("INFO: " + text);
        }

        public void LogWarning(string text)
        {
            Console.WriteLine("WARNING: " + text);
        }

        public void LogError(string text)
        {
            Console.WriteLine("ERROR: " + text);
        }
    }
}