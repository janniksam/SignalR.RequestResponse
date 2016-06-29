using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.Request.Response.Server.Interfaces;
using SignalR.Request.Response.Server.RequestHandler;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Server
{
    [HubName("RequestResponseHub")]
    public class RequestResponseHub : Hub, IRequestResponseHub
    {
        private readonly IRequestHandlerFactory m_requestHandlerFactoryFactory;
        private readonly IServerLogger m_serverLogger;

        public RequestResponseHub(IRequestHandlerFactory requestHandlerFactory, 
                                  IServerLogger serverLogger)
        {
            m_requestHandlerFactoryFactory = requestHandlerFactory;
            m_serverLogger = serverLogger;
            m_serverLogger.LogInfo("RequestResponseHub has been instantiated");
        }

        public override Task OnConnected()
        {
            m_serverLogger.LogInfo(string.Format("Client ({0}) connected to RequestResponse hub...", Context.ConnectionId));
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            m_serverLogger.LogInfo(string.Format("Client ({0}) disconnected...", Context.ConnectionId));
            return base.OnDisconnected(stopCalled);
        }

        public async void OnRequestReceived(SignalRRequest request)
        {
            m_serverLogger.LogInfo(string.Format("New request received: {0}", request.TypeName));
            var handler = m_requestHandlerFactoryFactory.GetHandlerFor(request.TypeName);
            if (handler == null)
            {
                m_serverLogger.LogError(string.Format("No RequestHandler found for {0}", request.TypeName));
                return;
            }
            AddContextToRequestHandler(handler);

            if (!handler.IsAuthorized())
            {
                m_serverLogger.LogWarning(string.Format("Request not authorized, Request: {0}, ConnectionId: {1}", request.TypeName, Context.ConnectionId));
                Clients.Caller.OnResponse(new SignalRResponse { RequestId = request.RequestId, Aborted = true });
                return;
            }

            m_serverLogger.LogInfo(string.Format("Executing request: {0}", request.TypeName));
            var response = await handler.ExecuteAsync(request);
            m_serverLogger.LogInfo(string.Format("Request executed: {0}", request.TypeName));
            Clients.Caller.OnResponseReceived(response);
        }

        private void AddContextToRequestHandler(IBaseRequestHandler handler)
        {
            handler.Context = new RequestHandlerContext
            {
                ConnectionId = Context.ConnectionId,
                Headers = new Dictionary<string, string>()
            };
            foreach (var header in Context.Headers)
            {
                handler.Context.Headers.Add(header.Key, header.Value);
            }
        }

        public async void OnRequestExecuteReceived(SignalRRequest request)
        {
            m_serverLogger.LogInfo(string.Format("New request received: {0}", request.TypeName));
            var handler = m_requestHandlerFactoryFactory.GetResponselessHandlerFor(request.TypeName);
            if (handler == null)
            {
                m_serverLogger.LogError(string.Format("No RequestHandler found for {0}", request.TypeName));
            }
            else
            {
                AddContextToRequestHandler(handler);
                m_serverLogger.LogInfo(string.Format("Executing request: {0}", request.TypeName));
                await handler.ExecuteAsync(request);
            }
        }
    }
}