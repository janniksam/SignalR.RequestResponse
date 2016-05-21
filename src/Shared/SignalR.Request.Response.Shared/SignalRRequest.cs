using System;

namespace SignalR.Request.Response.Shared
{
    public class SignalRRequest
    {
        public string SerializedRequest { get; set; }
        public string TypeName { get; set; }
        public Guid RequestId { get; set; }
    }
}