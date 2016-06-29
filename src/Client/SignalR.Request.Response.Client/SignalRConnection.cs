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
    public class SignalRConnection : ISignalRConnection
    {
        private HubConnection m_requestResponseConnection;
        private IHubProxy m_proxy;
        public EventHandler<SignalRResponse> ResponseReceived { get; set; }

        public IClientLogger Logger
        {
            get
            {
                if (!IsInitialized)
                {
                    return new NoLogClientLogger();
                }
                return Options.Logger;
            }
        }

        public bool IsInitialized
        {
            get
            {
                return Options != null;
            }
        }

        public ConnectionOptions Options { get; private set; }

        /// <summary>
        /// Open a connection to a destination server
        /// </summary>
        /// <param name="connectionOptions">The options for the connection</param>
        public async Task Connect(ConnectionOptions connectionOptions)
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
        public async Task Reconnect()
        {
            if(!IsInitialized)
            {
                Logger.LogAndThrow(new ConnectionCouldNotBeEstablishedException("Reconnection failed. You haven't been connected to a server yet.", null));
            }
            await Connect();
        }

        private async Task Connect()
        {
            Logger.LogInfo(string.Format("Trying to connect to {0}", Options.Uri));

            try
            {
                m_requestResponseConnection = new HubConnection(Options.Uri)
                {
                    TransportConnectTimeout = TimeSpan.FromSeconds(Options.TransportConnectTimeout)
                };
                m_proxy = m_requestResponseConnection.CreateHubProxy("RequestResponseHub");
                m_proxy.On<SignalRResponse>("OnResponseReceived", OnResponseReceived);
                await m_requestResponseConnection.Start();
            }
            catch (HttpRequestException e)
            {
                Logger.LogAndThrow(new ConnectionCouldNotBeEstablishedException("Could not connect to server.", e));
            }

            Logger.LogInfo(string.Format("Connection successfully established to {0}", Options.Uri));
        }

        private void OnResponseReceived(SignalRResponse response)
        {
            ResponseReceived?.Invoke(null, response);
        }

        public IDictionary<string,string> Headers => m_requestResponseConnection.Headers;

        public void Close()
        {
            if (m_requestResponseConnection != null)
            {
                m_requestResponseConnection.Stop();
                m_requestResponseConnection.Dispose();
            }
        }

        public void SendReceive(SignalRRequest request)
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
                    Logger.LogAndThrow(new ConnectionLostException("The connection to the server was lost.", e));
                }
                throw e;
            }
        }

        public void SendExecute(SignalRRequest request)
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
                    Logger.LogAndThrow(new ConnectionLostException("The connection to the server was lost.", e));
                }
                throw e;
            }
        }
    }
}