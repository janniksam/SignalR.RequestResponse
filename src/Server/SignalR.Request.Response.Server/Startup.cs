using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SignalR.Request.Response.Server;
using SignalR.Request.Response.Server.Interfaces;
using SignalR.Request.Response.Shared.Logging;

[assembly: OwinStartup(typeof(Startup))]
namespace SignalR.Request.Response.Server
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var requestHandlerFactory = GDInternalResources.RequestHandlerFactory;
            var logger = GDInternalResources.Logger;

            GlobalHost.DependencyResolver.Register(typeof(RequestResponseHub), () => new RequestResponseHub(requestHandlerFactory, logger));
            
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}