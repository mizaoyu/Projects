﻿//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartup(typeof(ConsoleHost.Service.Startup))]

namespace ConsoleHost.Service
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Owin;
    using System.Web.Http;
    using Microsoft.Owin.StaticFiles;
    using Microsoft.Owin.FileSystems;
    using System.IO;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "{controller}"
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;

            const string rootFolder = ".";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem,
                EnableDirectoryBrowsing = true
            };
            app.UseFileServer(options);
            
            string contentPath = Path.Combine(Environment.CurrentDirectory, @"..\..");

            app.UseStaticFiles(new Microsoft.Owin.StaticFiles.StaticFileOptions()
            {
                RequestPath = new PathString(),
                FileSystem = new PhysicalFileSystem(contentPath)
            });

            app.UseWebApi(config);
        }
    }
}
