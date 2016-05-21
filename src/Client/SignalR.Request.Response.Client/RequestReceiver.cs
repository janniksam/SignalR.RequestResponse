using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Client
{
    [Export(typeof(IRequestReceiver))]
    public class RequestReceiver : IRequestReceiver
    {
        private readonly Dictionary<Guid, SignalRResponse> m_responseDictionary;
        private readonly IClientLogger m_logger;

        public RequestReceiver()
        {
            m_logger = Connection.Logger;
            m_responseDictionary = new Dictionary<Guid, SignalRResponse>();
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
            Connection.SendReceive(wrappedRequest);

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
                await Task.Delay(50);
            } while (response == null);

            if (response.Aborted)
            {
                m_logger.LogError(string.Format("The request {0} was aborted by the server, ID: {1}",  typeof(TResponse).Name, requestId));
                throw new Exception(string.Format("The request {0} was aborted by the server, ID: {1}", typeof(TResponse).Name, requestId));
            }

            m_logger.LogInfo(string.Format("Response received: {0}, ID: {1}", typeof(TResponse).Name, requestId));
            var serializedResponse = Serializer.DeserializeObject<TResponse>(response.SerializedResponse);
            return serializedResponse;
        }
    }
}
