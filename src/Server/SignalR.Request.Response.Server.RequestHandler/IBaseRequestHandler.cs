namespace SignalR.Request.Response.Server.RequestHandler
{
    public interface IBaseRequestHandler
    {
        bool IsResponsibleFor(string typename);
        bool IsAuthorized();
        RequestHandlerContext Context { get; set; }
    }
}