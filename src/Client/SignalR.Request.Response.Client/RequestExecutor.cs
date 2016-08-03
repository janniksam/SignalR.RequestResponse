using System;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Client
{
    public class RequestExecutor : IRequestExecutor
    {
        private readonly ISignalRConnection m_signalRConnection;

        public RequestExecutor(ISignalRConnection signalRConnection)
        {
            m_signalRConnection = signalRConnection;
        }

        public void Execute<TRequest>(TRequest request) where TRequest : BaseRequest
        {
            var serializedRequest = request.SerializeObject();

            var wrappedRequest = new SignalRRequest
            {
                SerializedRequest = serializedRequest,
                TypeName = request.GetType().Name,
                RequestId = Guid.NewGuid()
            };

            m_signalRConnection.Logger.LogInfo(string.Format("Sending request: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
            m_signalRConnection.SendExecute(wrappedRequest);
            m_signalRConnection.Logger.LogInfo(string.Format("Request sent: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
        }
    }
}
