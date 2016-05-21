namespace SignalR.Request.Response.Shared.Logging
{
    public interface IServerLogger
    {
        void LogInfo(string text);
        void LogWarning(string text);
        void LogError(string text);
    }
}