//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Domain
{
    using ConsoleHost.RestClient;
    using Microsoft.OnePay.Models.Payments;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading.Tasks;

    public class OnePayClient : WebApiClient, IOnePayClient
    {
        /// <summary>
        /// Uri template constants
        /// </summary>
        public const string OnePayBasePath = "/api/v1";
        public const string OnePayPaymentsBase = OnePayBasePath + "/payments";
        public const string OnePayPaymentsCreate = OnePayPaymentsBase + "/create";

        public OnePayClient(WebApiClientParameters parameters)
            : base(parameters)
        {
            if (string.IsNullOrEmpty(parameters.Endpoint))
            {
                throw new ArgumentNullException("Endpoint");
            }

            BaseUri = new Uri(parameters.Endpoint);
        }

        /// <summary>
        /// Base URI for tools service
        /// </summary>
        public Uri BaseUri
        {
            get;
            private set;
        }

        public Task<CreatePaymentResponse> CreatePaymentSession(CreatePaymentRequest request)
        {
            var url = new Uri(BaseUri, OnePayPaymentsCreate);
            return PostAsync<CreatePaymentRequest, CreatePaymentResponse>(url, request, "CreatePaymentSession");
        }
    }
}