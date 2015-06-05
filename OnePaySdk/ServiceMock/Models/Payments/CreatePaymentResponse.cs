//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    public class CreatePaymentResponse
    {
        /// <summary>
        /// The identifier for the payment session
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// The uri that should be used to redirect the user to
        /// </summary>
        public string OnePayCheckoutUri { get; set; }
    }
}
