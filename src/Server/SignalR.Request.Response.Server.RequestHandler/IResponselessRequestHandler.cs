using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Server.RequestHandler
{
    public interface IResponselessRequestHandler : IBaseRequestHandler
    {
        Task ExecuteAsync(SignalRRequest request);
    }
}