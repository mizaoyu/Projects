//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    public class LineItem
    {
        /// <summary>
        /// The description of the item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The total for the line item
        /// </summary>
        public decimal LineTotal { get; set; }

        /// <summary>
        /// The total discount applied
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// The description of the discount
        /// </summary>
        public string DiscountDescription { get; set; }
    }
}
