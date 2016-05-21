using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Client
{
    public static class Connection
    {
        private static IClientLogger m_logger;
        private static HubConnection m_requestResponseConnection;
        private static IHubProxy m_proxy;

        public static EventHandler<SignalRResponse> ResponseReceived;

        public static IClientLogger Logger
        {
            get
            {
                if (m_logger == null)
                {
                    return new NoLogClientLogger();
                }
                return m_logger;
            }
        }

        public static void InitializeLogger(IClientLogger logger)
        {
            if (logger != null)
            {
                m_logger = logger;
            }
        }

        /// <summary>
        /// Open a connection to a destination server
        /// </summary>
        /// <param name="connectionOptions">The options for the connection</param>
        /// <returns></returns>
        public static async Task Connect(ConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions));
            }
            Logger.LogInfo(string.Format("Trying to connect to {0}", connectionOptions.Uri));
            m_requestResponseConnection = new HubConnection(connectionOptions.Uri);
            m_requestResponseConnection.TransportConnectTimeout = TimeSpan.FromSeconds(connectionOptions.Timeout);
            m_proxy = m_requestResponseConnection.CreateHubProxy("RequestResponseHub");
            m_proxy.On<SignalRResponse>("OnResponseReceived", OnResponseReceived);
            await m_requestResponseConnection.Start();
            Logger.LogInfo(string.Format("Connection successfully established to {0}", connectionOptions.Uri));
        }

        private static void OnResponseReceived(SignalRResponse response)
        {
            ResponseReceived?.Invoke(null, response);
        }

        public static IDictionary<string,string> Headers => m_requestResponseConnection.Headers;

        public static void Close()
        {
            if (m_requestResponseConnection != null)
            {
                m_requestResponseConnection.Stop();
                m_requestResponseConnection.Dispose();
            }
        }

        public static void SendReceive(SignalRRequest request)
        {
            if(request.RequestId == default(Guid))
            {
                Logger.LogError("RequestId cannot be null");
                throw new Exception("RequestId cannot be null");
            }
            m_proxy.Invoke("OnRequestReceived", request);
        }

        public static void SendExecute(SignalRRequest request)
        {
            m_proxy.Invoke("OnRequestExecuteReceived", request);
        }
    }
}