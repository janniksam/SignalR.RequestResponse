using System;
using Microsoft.Owin.Hosting;
using SignalR.Request.Response.Server.Interfaces;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Server
{
    public class RequestResponseServer : IRequestResponseServer
    {
        private bool m_initialized;

        public void Init(IRequestHandlerFactory requestHandlerFactory, IServerLogger logger = null)
        {
            if (m_initialized)
            {
                throw new Exception("Already initialized.");
            }
            if (requestHandlerFactory == null)
            {
                throw new ArgumentNullException(nameof(requestHandlerFactory));
            }

            GDInternalResources.Init(requestHandlerFactory, logger);
            m_initialized = true;
        }

        public void Run(string url)
        {
            if (!m_initialized)
            {
               throw new AccessViolationException("Please call IRequestResponseServer.Init(), before using this method.");
            }

            try
            {
                using (WebApp.Start(url))
                {
                    GDInternalResources.Logger.LogInfo(string.Format("... Server started on {0}", url));
                    string line;
                    do
                    {
                        line = Console.ReadLine();
                    } while (line != "quit");
                }
            }
            catch (Exception exception)
            {
                GDInternalResources.Logger.LogError(exception.ToString());
            }
        }
    }
}
