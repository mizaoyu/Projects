//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    using System.Collections.Generic;

    /// <summary>
    /// A request to initiate a payment session
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// The id of the merchant making the request
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// The display name of the merchant making the request
        /// </summary>
        public string MerchantDisplayName { get; set; }

        /// <summary>
        /// The currency code for the request transaction
        /// </summary>
        public CurrencyCode CurrencyCode { get; set; }

        /// <summary>
        /// The total amount of the transaction
        /// </summary>
        public double TransactionTotal { get; set; }

        /// <summary>
        /// The line items for the transaction
        /// </summary>
        public IEnumerable<LineItem> LineItems { get; set; }
    }
}
