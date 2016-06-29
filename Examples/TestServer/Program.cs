using System;
using SignalR.Request.Response.Server;
using TestServer.Application;

namespace TestServer
{
    public class Program
    {
        private const string SERVER_URL = "http://*:15117/";

        public static void Main()
        {
            var myLogger = new MyCustomLogger();
            myLogger.LogInfo("Server starting...");

            RequestResponseServer requestResponseServer = new RequestResponseServer();
            requestResponseServer.Init(new SimpleRequestHandlerFactory(), myLogger);
            requestResponseServer.Run(SERVER_URL);

            myLogger.LogInfo("Server is shutting down...");

            Console.ReadKey();
        }
    }
}
