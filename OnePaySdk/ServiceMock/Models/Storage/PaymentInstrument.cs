//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class PaymentInstrument : INotifyPropertyChanged
    {
        /// <summary>
        /// The account number for the payment instrument
        /// </summary>
        private string accountNumber;

        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }

            set
            {
                accountNumber = value;
                NotifyChange("AccountNumber");
            }
        }

        /// <summary>
        /// The billing address for the payment instrument
        /// </summary>
        private Address billingAddress;

        public Address BillingAddress
        {
            get
            {
                return billingAddress;
            }

            set
            {
                billingAddress = value;
                NotifyChange("BillingAddress");
            }
        }

        /// <summary>
        /// The cvv for the payment instrument
        /// </summary>
        private ushort cvv;

        public ushort Cvv
        {
            get
            {
                return cvv;
            }

            set
            {
                cvv = value;
                NotifyChange("Cvv");
            }
        }

        /// <summary>
        /// The expiration date for the payment
        /// </summary>
        private DateTime expirationDate;

        public DateTime ExpirationDate
        {
            get
            {
                return expirationDate;
            }

            set
            {
                expirationDate = value;
                NotifyChange("ExpirationDate");
            }
        }

        /// <summary>
        /// The type of payment
        /// </summary>
        private Type type;

        public Type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                NotifyChange("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyChange(string propertyName)
        {
            if (argsCache != null)
            {
                if (!argsCache.ContainsKey(propertyName))
                {
                    argsCache[propertyName] = new PropertyChangedEventArgs(propertyName);
                }

                NotifyChange(argsCache[propertyName]);
            }
        }

        private void NotifyChange(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private readonly Dictionary<string, PropertyChangedEventArgs> argsCache = new Dictionary<string, PropertyChangedEventArgs>();
    }
}
