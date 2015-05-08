using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.StaticFiles;

[assembly: OwinStartup(typeof(ConsoleHost.Service.Startup))]

namespace ConsoleHost.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}"
            );
            
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;

            app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false });
            app.UseWebApi(config);
        }
    }
}
