using System.Collections.Generic;

namespace SignalR.Request.Response.Server.RequestHandler
{
    public class RequestHandlerContext
    {
        public Dictionary<string,string> Headers { get; set; } 
        public string ConnectionId { get; set; }
    }
}