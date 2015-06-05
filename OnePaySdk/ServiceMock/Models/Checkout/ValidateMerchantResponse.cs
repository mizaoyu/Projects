//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Checkout
{
    public class ValidateMerchantResponse
    {
        public string TransactionId { get; set; }

        public string MerchantId { get; set; }

        public string MerchantName { get; set; }

        public double TransactionTotal { get; set; }
    }
}