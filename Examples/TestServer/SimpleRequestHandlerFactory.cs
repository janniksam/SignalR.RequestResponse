using SignalR.Request.Response.Server.Interfaces;
using SignalR.Request.Response.Server.RequestHandler;
using TestServer.Application;

namespace TestServer
{
    internal class SimpleRequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly IRequestHandler m_testRequestHandler = new TestRequestHandler();
        
        public IRequestHandler GetHandlerFor(string typeName)
        {
            if (m_testRequestHandler.IsResponsibleFor(typeName))
            {
                return m_testRequestHandler;
            }
            return null;
        }

        public IResponselessRequestHandler GetResponselessHandlerFor(string typeName)
        {
            return null;
        }
    }
}