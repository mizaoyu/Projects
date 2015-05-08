using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHost
{
    using ConsoleHost.Service;

    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine(String.Format("Service started at Base Address: {0}", baseAddress));
                var input = String.Empty;
                while (String.Equals(input, "exit") == false)
                {
                    input = Console.ReadLine();
                }
            }
        }
    }
}
