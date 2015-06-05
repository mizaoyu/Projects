//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Payments
{
    public enum State
    {
        Submitted = 0,
        MerchantAuthenticated,
        DeviceAuthenticated,
        UserAuthenticated,
        BankAuthorized,
        AuthorizationComplete,
        Canceled,
        Expired,
        Aborted,
    }
}