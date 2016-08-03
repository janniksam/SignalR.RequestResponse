using System;
using System.Threading.Tasks;
using SignalR.Request.Response.Shared;

namespace SignalR.Request.Response.Client
{
    public interface ISignalRConnection
    {
        bool IsInitialized { get; }
        ConnectionOptions Options { get; }
        EventHandler<SignalRResponse> ResponseReceived { get; set; }
        void SendExecute(SignalRRequest wrappedRequest);
        void SendReceive(SignalRRequest wrappedRequest);

        Task Connect(ConnectionOptions connectionOptions);
        Task Reconnect();
        void Close();

        void RemoveHeader(string key);
        void AddOrUpdateHeader(string key, string value);
    }
}
