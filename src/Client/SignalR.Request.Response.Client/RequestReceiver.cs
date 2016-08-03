using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;
using SignalR.Request.Response.Client.ExceptionHandling;
using SignalR.Request.Response.Client.ExceptionHandling.Exceptions;
using System.Diagnostics;

namespace SignalR.Request.Response.Client
{
    public class RequestReceiver : IRequestReceiver
    {
        private readonly Dictionary<Guid, SignalRResponse> m_responseDictionary;
        private readonly IClientLogger m_logger;
        private readonly ISignalRConnection m_signalRConnection;
        
        public RequestReceiver(ISignalRConnection signalRConnection)
        {
            m_signalRConnection = signalRConnection;
            m_signalRConnection.ResponseReceived += OnResponseReceived;
            m_logger = signalRConnection.Options.Logger;
            m_responseDictionary = new Dictionary<Guid, SignalRResponse>();
        }

        ~RequestReceiver()
        {
            if (m_signalRConnection != null)
            {
                m_signalRConnection.ResponseReceived -= OnResponseReceived;
            }
        }

        public async Task<TResponse> ReadAsync<TResponse>(BaseRequest request) where TResponse : BaseResponse
        {
            var serializedRequest = request.SerializeObject();
            var wrappedRequest = new SignalRRequest
            {
                SerializedRequest = serializedRequest,
                TypeName = request.GetType().Name,
                RequestId = Guid.NewGuid()
            };

            m_logger.LogInfo(string.Format("Sending request: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
            m_signalRConnection.SendReceive(wrappedRequest);

            m_logger.LogInfo(string.Format("Waiting for response: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
            var response = await GetResponse<TResponse>(wrappedRequest.RequestId);
            return response;
        }

        public void OnResponseReceived(object sender, SignalRResponse response)
        {
            lock (m_responseDictionary)
            {
                m_responseDictionary.Add(response.RequestId, response);
            }
        }

        private async Task<TResponse> GetResponse<TResponse>(Guid requestId) where TResponse : BaseResponse
        {
            if(!m_signalRConnection.IsInitialized)
            {
                m_logger.LogAndThrow(new ConnectionLostException(string.Format("The connection was lost during the request with ID={0}", requestId), null));
            }

            int timeoutMs = m_signalRConnection.Options.TimeoutWaitingForResponse * 1000;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            SignalRResponse response;
            do
            {
                lock (m_responseDictionary)
                {
                    if (m_responseDictionary.TryGetValue(requestId, out response))
                    {
                        m_responseDictionary.Remove(requestId);
                    }
                }

                if (stopWatch.ElapsedMilliseconds > timeoutMs)
                {
                    m_logger.LogAndThrow(new RequestTimedOutException(string.Format("Request with the Id={0} timed out.", requestId)));
                }
                 
                await Task.Delay(30);
            } while (response == null);

            if (response.Aborted)
            {
                m_logger.LogAndThrow(new RequestAbortedException(string.Format("The request {0} was aborted by the server, ID: {1}", typeof(TResponse).Name, requestId)));
            }

            m_logger.LogInfo(string.Format("Response received: {0}, RequestID: {1}", typeof(TResponse).Name, requestId));
            var serializedResponse = Serializer.DeserializeObject<TResponse>(response.SerializedResponse);
            return serializedResponse;
        }

        public SignalRConnection Connection { get; }
    }
}
