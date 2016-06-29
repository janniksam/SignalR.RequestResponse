using SignalR.Request.Response.Server.RequestHandler;

namespace SignalR.Request.Response.Server.Interfaces
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler GetHandlerFor(string typeName);
        IResponselessRequestHandler GetResponselessHandlerFor(string typeName);
    }
}