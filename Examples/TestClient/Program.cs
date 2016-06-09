using System;
using SignalR.Request.Response.Client;
using SignalR.Request.Response.Shared.Logging;
using Test.ClientServer.Shared;

namespace TestClient
{
    class Program
    {
        private static void Main()
        {
            Program p = new Program();
            p.Run();
            do
            {
            } while (true);
        }

        private async void Run()
        {
            Console.WriteLine("Client Start");

            var connection = new SignalRConnection();

            IClientLogger myLogger = new ClientConsoleLogger();
            var connectionOptions = new ConnectionOptions("http://127.0.0.1:15117/signalr", myLogger);
            await connection.Connect(connectionOptions);

            var requestReceiver = new RequestReceiver(connection);
            var requestExecutor = new RequestExecutor(connection);
            do
            {
                var response = await requestReceiver.ReadAsync<TestResponse>(new TestRequest { Test = "Hello Server!" });
                requestExecutor.Execute(new MyRequest { Hessage = "Hello Server with responseless request" });
                Console.WriteLine(response.Text);
            } while (true);

            Console.WriteLine("Client End");
        }
    }
}