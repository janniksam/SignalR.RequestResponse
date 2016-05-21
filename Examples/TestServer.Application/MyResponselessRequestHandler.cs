using System;
using System.Threading.Tasks;
using SignalR.Request.Response.Server.RequestHandler;
using Test.ClientServer.Shared;

namespace TestServer.Application
{
    public class MyResponselessRequestHandler : BaseResponselessRequestHandler<MyRequest>
    {
        protected override async Task ExecuteAsync(MyRequest request)
        {
            if (request.Hessage != "Hello")
            {
                throw new ArgumentException();
            }
        }
    }
}