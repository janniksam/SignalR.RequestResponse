using System;

namespace SignalR.Request.Response.Shared
{
    public class SignalRResponse
    {
        public string SerializedResponse { get; set; }
        public Guid RequestId { get; set; }
        public bool Aborted { get; set; }
    }
}