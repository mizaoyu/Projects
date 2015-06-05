//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay
{
    using Microsoft.OnePay.ServiceMock;
    using Microsoft.Owin.Hosting;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var baseAddress = "http://+:9001/";
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
