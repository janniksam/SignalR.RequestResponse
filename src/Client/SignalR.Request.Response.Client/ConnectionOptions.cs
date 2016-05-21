namespace SignalR.Request.Response.Client
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string uri)
        {
            Uri = uri;
            TransportConnectTimeout = 60;
            TimeoutWaitingForResponse = 60;
        }

        public string Uri { get; set; }
        public int TransportConnectTimeout { get; set; }
        public int TimeoutWaitingForResponse { get; set; }
    }
}