using System.Threading.Tasks;
using SignalR.Request.Response.Server.RequestHandler;
using SignalR.Request.Response.Shared;
using Test.ClientServer.Shared;

namespace TestServer.Application
{
    public class TestRequestHandler : BaseRequestHandler<TestRequest>
    {
        protected override async Task<BaseResponse> ExecuteAsync(TestRequest request)
        { 
            return new TestResponse { Text = "Hello from Server!" };
        }
    }
}
