//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Domain
{
    using Microsoft.OnePay.Models.Payments;
    using System.Threading.Tasks;

    public interface IOnePayClient
    {
        Task<CreatePaymentResponse> CreatePaymentSession(CreatePaymentRequest request);
    }
}
