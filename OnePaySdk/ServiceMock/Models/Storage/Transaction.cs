//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Storage
{
    using Microsoft.OnePay.Models.Payments;

    public class Transaction
    {
        public string Id { get; set; }

        public double Total { get; set; }

        public CurrencyCode CurrencyCode { get; set; }

        public Merchant Merchant { get; set; }

        public Account Account { get; set; }

        public string Token { get; set; }

        public Payments.State State { get; set; }

        public int ChallengeCode { get; set; }
    }
}