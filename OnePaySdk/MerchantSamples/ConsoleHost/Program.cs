//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost
{
    using ConsoleHost.Service;
    using Microsoft.Owin.Hosting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using ConsoleHost.Models;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing and seeding database...");
            Database.SetInitializer(new CartDBInitializer());
            Database.SetInitializer(new ItemsDBInitializer());
            Database.SetInitializer(new TransactionDBInitializer());
            var db = new ItemsDBContext();
            int count = db.Items.Count();
            Console.WriteLine("Initializing and seeding database with {0} item records...", count);

            var baseAddress = "http://+:9000/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Service started at Base Address: {0}", baseAddress);

                var input = string.Empty;
                while (input != "exit")
                {
                    input = Console.ReadLine();
                }
            }
        }
    }
}