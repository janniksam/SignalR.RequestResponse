namespace SignalR.Request.Response.Client
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string uri)
        {
            Uri = uri;
            Timeout = 60;
        }

        public string Uri { get; set; }
        public int Timeout { get; set; }
    }
}