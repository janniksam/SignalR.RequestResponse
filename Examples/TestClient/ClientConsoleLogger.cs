using System;
using SignalR.Request.Response.Shared.Logging;

namespace TestClient
{
    internal class ClientConsoleLogger : IClientLogger
    {
        public void LogInfo(string text)
        {
            Console.WriteLine("INFO: {0}", text);
        }

        public void LogWarning(string text)
        {
            Console.WriteLine("WARNING: {0}", text);
        }

        public void LogError(string text)
        {
            Console.WriteLine("ERROR: {0}", text);
        }
    }
}