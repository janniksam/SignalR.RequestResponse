using System;
using SignalR.Request.Response.Shared;
using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Client
{
    public class RequestExecutor : IRequestExecutor
    {
        private readonly ISignalRConnection m_signalRConnection;
        private readonly IClientLogger m_logger;

        public RequestExecutor(ISignalRConnection signalRConnection)
        {
            m_signalRConnection = signalRConnection;
            m_logger = signalRConnection.Options.Logger;
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

            m_logger.LogInfo(string.Format("Sending request: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
            m_signalRConnection.SendExecute(wrappedRequest);
            m_logger.LogInfo(string.Format("Request sent: {0}, ID: {1}", wrappedRequest.TypeName, wrappedRequest.RequestId));
        }
    }
}
