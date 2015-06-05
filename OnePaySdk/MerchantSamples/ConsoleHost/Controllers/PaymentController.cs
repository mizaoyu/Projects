//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Service.Controllers
{
    using ConsoleHost.Domain;
    using ConsoleHost.RestClient;
    using Microsoft.OnePay.Models.Payments;
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class PaymentController : ApiController
    {
        [HttpPost]
        public async Task<CreatePaymentResponse> Create(CreatePaymentRequest request)
        {
            Console.WriteLine("request.Id:{0}", request.MerchantId);
            Console.WriteLine("request.Id:{0}", request.MerchantDisplayName);
            Console.WriteLine("request.Id:{0}", request.CurrencyCode);
            Console.WriteLine("request.Id:{0}", request.TransactionTotal);
            foreach (LineItem li in request.LineItems)
            {
                Console.WriteLine("request.Id:{0}", li.Description);
            }

            using (var client = new OnePayClient(GetClientParameters()))
            {
                return await client.CreatePaymentSession(request);
            }
        }

        private static WebApiClientParameters GetClientParameters()
        {
            return new WebApiClientParameters()
            {
                Endpoint = "http://localhost:9001"
            };
        }
    }
}
