using System;
using System.Composition;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Client
{
    [Export(typeof(IRequestExecutor))]
    public class RequestExecutor : IRequestExecutor
    {
        public void Execute<TRequest>(TRequest request) where TRequest : BaseRequest
        {
            var serializedRequest = request.SerializeObject();

            var wrappedRequest = new SignalRRequest
            {
                SerializedRequest = serializedRequest,
                TypeName = request.GetType().Name,
                RequestId = Guid.NewGuid()
            };
            Connection.SendExecute(wrappedRequest);
        }
    }
}
