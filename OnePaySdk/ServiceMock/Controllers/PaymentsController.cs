//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Controllers
{
    using Microsoft.OnePay.Models.Payments;
    using Microsoft.OnePay.Models.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;

    [RoutePrefix("api/v1/payments")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentsController : ApiController
    {
        /// <summary>
        /// Options request handler.
        /// </summary>
        /// <returns>Returns the options for the api</returns>
        [AllowAnonymous]
        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "POST");

            return resp;
        }

        /// <summary>
        /// Initiates a new payments request to the controller
        /// </summary>
        /// <param name="request">The create request data.</param>
        /// <returns>The session\redirect information</returns>
        [AllowAnonymous]
        [Route("create")]
        [HttpPost]
        public CreatePaymentResponse CreatePaymentSession(CreatePaymentRequest request)
        {
            string paymentId = Guid.NewGuid().ToString();

            var transaction = new Transaction
            {
                Id = paymentId,
                Merchant = new Merchant
                {
                    Id = request.MerchantId,
                    Name = request.MerchantDisplayName,
                },
                CurrencyCode = request.CurrencyCode,
                Total = request.TransactionTotal,
            };
            DataAccess.SetTransaction(transaction);

            return new CreatePaymentResponse
            {
                OnePayCheckoutUri = "http://localhost:9001/Content/LaunchUAP.html?id=" + paymentId + "&price=" + request.TransactionTotal,
                PaymentId = paymentId,
            };
        }

        /// <summary>
        /// Options request handler.
        /// </summary>
        /// <returns>Returns the options for the api</returns>
        [AllowAnonymous]
        [AcceptVerbs("OPTIONS")]
        [Route("create")]
        public HttpResponseMessage CreateOptions()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "POST");

            return resp;
        }

        /// <summary>
        /// Gets the payment authorization result
        /// </summary>
        /// <param name="paymentId">The payment id</param>
        /// <param name="request">The get payment request</param>
        /// <returns>The payment data</returns>
        [AllowAnonymous]
        [Route("{paymentId}")]
        [HttpPost]
        public GetPaymentResponse GetPayment(string paymentId, GetPaymentRequest request)
        {
            var transaction = DataAccess.GetTransaction(paymentId);
            Console.WriteLine(paymentId);
            if (transaction == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid id"));
            }
            long tick = 1000000000000;
            DateTime date = new DateTime(tick);
            return new GetPaymentResponse
            {
                //EmailAddress = transaction.Account.EmailAddress,
                EmailAddress = "t-jins@microsoft.com",
                ErrorMessage = null,
                PaymentDetail = new PaymentDetail
                {
                    AccountNumber = "1234-1234-1234-1234",
                    //BillingAddress = transaction.Account.PaymentInstrument.BillingAddress,
                    BillingAddress = null,
                    //Cvv = transaction.Account.PaymentInstrument.Cvv,
                    Cvv = 0,
                    //ExpirationDate = transaction.Account.PaymentInstrument.ExpirationDate,
                    ExpirationDate = date,
                    //Type = transaction.Account.PaymentInstrument.Type,
                    //Type = null,
                },
                //ShippingAddress = transaction.Account.ShippingAddress,
                ShippingAddress = null,
                State = transaction.State,
            };
        }

        /// <summary>
        /// Options request handler.
        /// </summary>
        /// <returns>Returns the options for the api</returns>
        [AcceptVerbs("OPTIONS")]
        [AllowAnonymous]
        [Route("{paymentId}")]
        public HttpResponseMessage GetPaymentOptions()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "POST");

            return resp;
        }

        [AllowAnonymous]
        [Route("list")]
        [HttpPost]
        public IEnumerable<GetPaymentResponse> ListPayments()
        {
            var transactions = DataAccess.GetTransactions();
            if (transactions == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid id"));
            }

            return transactions.Select(transaction => new GetPaymentResponse
            {
                EmailAddress = transaction.Account != null ? transaction.Account.EmailAddress : string.Empty,
                ErrorMessage = null,
                PaymentDetail = transaction.Account != null && transaction.Account.PaymentInstrument != null ?
                new PaymentDetail
                {
                    AccountNumber = "1234-1234-1234-1234",
                    BillingAddress = transaction.Account.PaymentInstrument.BillingAddress,
                    Cvv = transaction.Account.PaymentInstrument.Cvv,
                    ExpirationDate = transaction.Account.PaymentInstrument.ExpirationDate,
                    Type = transaction.Account.PaymentInstrument.Type,
                } : null,
                ShippingAddress = transaction.Account != null ? transaction.Account.ShippingAddress : null,
                State = transaction.State,
            });
        }
    }
}