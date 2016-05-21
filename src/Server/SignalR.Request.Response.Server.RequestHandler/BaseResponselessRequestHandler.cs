using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Server.RequestHandler
{
    public abstract class BaseResponselessRequestHandler<TRequest> : IResponselessRequestHandler where TRequest : BaseRequest
    {
        public async Task ExecuteAsync(SignalRRequest signalRRequest)
        {
            var request = Serializer.DeserializeObject<TRequest>(signalRRequest.SerializedRequest);
            await ExecuteAsync(request);
        }

        public bool IsResponsibleFor(string typename)
        {
            var type = typeof(TRequest).Name;
            return type == typename;
        }

        public virtual bool IsAuthorized()
        {
            return true;
        }

        public RequestHandlerContext Context { get; set; }

        protected abstract Task ExecuteAsync(TRequest request);
    }
}