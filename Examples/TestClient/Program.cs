using System;
using SignalR.Request.Response.Client;
using Test.ClientServer.Shared;

namespace TestClient
{
    class Program
    {
        static void Main()
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

            IRequestReceiver requestReceiver = new RequestReceiver();
            RequestExecutor requestExecutor = new RequestExecutor();

            Connection.ResponseReceived += requestReceiver.OnResponseReceived;

            var connectionOptions = new ConnectionOptions("http://127.0.0.1:15117/signalr");
            await Connection.Connect(connectionOptions);
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