//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    using Microsoft.OnePay.Models.Storage;

    public class GetPaymentResponse
    {
        /// <summary>
        /// The email address for the customer
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The shipping address for the transaction
        /// </summary>
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// The payment detail
        /// </summary>
        public PaymentDetail PaymentDetail { get; set; }

        /// <summary>
        /// The state of the transaction
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// The error message for the transaction
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
