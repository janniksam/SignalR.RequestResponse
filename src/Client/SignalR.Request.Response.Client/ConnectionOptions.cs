using SignalR.Request.Response.Shared.Logging;

namespace SignalR.Request.Response.Client
{
    public class ConnectionOptions
    {
        private IClientLogger m_logger;

        public ConnectionOptions(string uri, IClientLogger logger = null)
        {
            Uri = uri;
            TransportConnectTimeout = 60;
            TimeoutWaitingForResponse = 60;
            Logger = logger;
        }

        public string Uri { get; set; }
        public int TransportConnectTimeout { get; set; }
        public int TimeoutWaitingForResponse { get; set; }

        public IClientLogger Logger
        {
            get
            {
                if(m_logger == null)
                {
                    m_logger = new NoLogClientLogger();
                }
                return m_logger;
            }
            set
            {
                m_logger = value;
            }
        }
    }
}