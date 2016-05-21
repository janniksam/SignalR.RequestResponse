namespace SignalR.Request.Response.Shared.Logging
{ 
    public class NoLogServerLogger : IServerLogger
    {
        public void LogInfo(string text)
        {
        }

        public void LogWarning(string text)
        {
        }

        public void LogError(string text)
        {
        }
    }
}