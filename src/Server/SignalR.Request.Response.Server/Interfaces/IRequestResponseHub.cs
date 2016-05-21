using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Server.Interfaces
{
    internal interface IRequestResponseHub
    {
        void OnRequestReceived(SignalRRequest request);
        void OnRequestExecuteReceived(SignalRRequest request);
    }
}