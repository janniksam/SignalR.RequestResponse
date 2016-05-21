using System;
using SignalR.Request.Response.Server.Interfaces;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Server
{
    internal static class GDInternalResources
    {
        private static IRequestHandlerFactory m_requestHandlerFactory;
        private static IServerLogger m_logger;

        internal static void Init(IRequestHandlerFactory requestHandlerFactory, IServerLogger logger = null)
        {
            m_requestHandlerFactory = requestHandlerFactory;
            m_logger = logger;
        }

        internal static IRequestHandlerFactory RequestHandlerFactory
        {
            get
            {
                if (m_requestHandlerFactory == null)
                {
                    throw new NullReferenceException("You have to initialize this library with a IRequestHandlerFactory implementation first. " +
                                                     "Use IRequestResponseServer.Init() first.");
                }
                return m_requestHandlerFactory;
            }  
        }

        internal static IServerLogger Logger
        {
            get
            {
                if (m_logger != null)
                {
                    return m_logger;
                }
                return new NoLogServerLogger();
            }
        }
    }
}