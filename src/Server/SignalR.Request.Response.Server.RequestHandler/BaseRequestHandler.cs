using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Server.RequestHandler
{
    public abstract class BaseRequestHandler<TRequest> : IRequestHandler where TRequest : BaseRequest
    {
        public bool IsResponsibleFor(string typename)
        {
            var type = typeof(TRequest).Name;
            return type == typename;
        }

        public async Task<SignalRResponse> ExecuteAsync(SignalRRequest signalRRequest)
        {
            await Prepare();

            var request = Serializer.DeserializeObject<TRequest>(signalRRequest.SerializedRequest);
            var response = await ExecuteAsync(request);
            var serializedResponse = response.SerializeObject();

            var signalRResponse = new SignalRResponse
            {
                SerializedResponse = serializedResponse,
                RequestId = signalRRequest.RequestId
            };
            return signalRResponse;
        }

        protected virtual async Task Prepare()
        {
        }

        public virtual bool IsAuthorized()
        {
            return true;
        }

        public RequestHandlerContext Context { get; set; }

        protected abstract Task<BaseResponse> ExecuteAsync(TRequest request);
    }
}