using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;
using SignalR.Request.Response.Client.ExceptionHandling;
using SignalR.Request.Response.Client.ExceptionHandling.Exceptions;
using System.Net.Http;

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

        public static ConnectionOptions Options { get; private set; }

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
        public static async Task Connect(ConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions));
            }

            Options = connectionOptions;
            await Connect();
        }

        /// <summary>
        /// Use this method to reconnect to the server after an existing connection was lost
        /// </summary>
        public static async Task Reconnect()
        {
            if(Options == null)
            {
                new ConnectionCouldNotBeEstablishedException("Reconnection failed. You haven't been connected to a server yet.", null).LogAndThrow();
            }
            await Connect();
        }

        private static async Task Connect()
        {
            Logger.LogInfo(string.Format("Trying to connect to {0}", Options.Uri));

            try
            {
                m_requestResponseConnection = new HubConnection(Options.Uri);
                m_requestResponseConnection.TransportConnectTimeout = TimeSpan.FromSeconds(Options.TransportConnectTimeout);
                m_proxy = m_requestResponseConnection.CreateHubProxy("RequestResponseHub");
                m_proxy.On<SignalRResponse>("OnResponseReceived", OnResponseReceived);
                await m_requestResponseConnection.Start();
            }
            catch (HttpRequestException e)
            {
                new ConnectionCouldNotBeEstablishedException("Could not connect to server.", e).LogAndThrow();
            }

            Logger.LogInfo(string.Format("Connection successfully established to {0}", Options.Uri));
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
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request)); 
            }

            if (request.RequestId == default(Guid))
            {
                throw new Exception("RequestId cannot be null");
            }

            try
            {
                m_proxy.Invoke("OnRequestReceived", request);
            }
            catch(InvalidOperationException e)
            {
                if (e.Message.Contains("Data cannot be sent because the connection is in the disconnected state. Call start before sending any data."))
                {
                    new ConnectionLostException("The connection to the server was lost.", e).LogAndThrow();
                }
                throw e;
            }
        }

        public static void SendExecute(SignalRRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.RequestId == default(Guid))
            {
                throw new Exception("RequestId cannot be null");
            }

            try
            {
                m_proxy.Invoke("OnRequestExecuteReceived", request);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Contains("Data cannot be sent because the connection is in the disconnected state. Call start before sending any data."))
                {
                    new ConnectionLostException("The connection to the server was lost.", e).LogAndThrow();
                }
                throw e;
            }
        }
    }
}