using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Server.Interfaces
{
    public interface IRequestResponseServer
    {
        void Init(IRequestHandlerFactory requestHandlerFactory, IServerLogger logger = null);
        void Run(string url);
    }
}