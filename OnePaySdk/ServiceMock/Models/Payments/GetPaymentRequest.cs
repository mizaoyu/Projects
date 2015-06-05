//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    public class GetPaymentRequest
    {
        /// <summary>
        /// The id for the payment
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// The merchant id
        /// </summary>
        public string MerchantId { get; set; }
    }
}
