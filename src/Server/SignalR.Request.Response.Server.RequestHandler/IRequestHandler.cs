using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Server.RequestHandler
{
    public interface IRequestHandler : IBaseRequestHandler
    {
        Task<SignalRResponse> ExecuteAsync(SignalRRequest request);
    }
}