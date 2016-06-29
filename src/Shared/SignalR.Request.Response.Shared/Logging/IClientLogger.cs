namespace SignalR.Request.Response.Shared.Logging
{
    public interface IClientLogger
    {
        void LogInfo(string text);
        void LogWarning(string text);
        void LogError(string text);
    }
}