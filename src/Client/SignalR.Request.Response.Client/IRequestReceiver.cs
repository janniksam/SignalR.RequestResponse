using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Client
{
    public interface IRequestReceiver
    {
        Task<TResponse> ReadAsync<TResponse>(BaseRequest request) where TResponse : BaseResponse;
        void OnResponseReceived(object sender, SignalRResponse response);
    }
}