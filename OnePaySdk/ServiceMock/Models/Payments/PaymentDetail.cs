//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    using Microsoft.OnePay.Models.Storage;
    using System;

    public class PaymentDetail
    {
        /// <summary>
        /// The account number for the payment instrument
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The billing address for the payment instrument
        /// </summary>
        public Address BillingAddress { get; set; }

        /// <summary>
        /// The cvv for the payment instrument
        /// </summary>
        public ushort Cvv { get; set; }

        /// <summary>
        /// The expiration date for the payment
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// The type of payment
        /// </summary>
        public Storage.Type Type { get; set; }
    }
}