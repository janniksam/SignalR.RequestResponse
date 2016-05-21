using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Client
{
    public interface IRequestExecutor
    {
        void Execute<TRequest>(TRequest request) where TRequest : BaseRequest;
    }
}