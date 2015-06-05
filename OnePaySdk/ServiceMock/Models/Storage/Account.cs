//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Storage
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Account : INotifyPropertyChanged
    {
        public string Id { get; set; }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotifyChange("Name");
            }
        }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        private Address shippingAddress;

        public Address ShippingAddress
        {
            get
            {
                return shippingAddress;
            }

            set
            {
                shippingAddress = value;
                NotifyChange("ShippingAddress");
            }
        }

        private PaymentInstrument paymentInstrument;

        public PaymentInstrument PaymentInstrument
        {
            get
            {
                return paymentInstrument;
            }

            set
            {
                paymentInstrument = value;
                NotifyChange("PaymentInstrument");
            }
        }

        private IEnumerable<Device> devices;

        public IEnumerable<Device> Devices
        {
            get
            {
                return devices;
            }

            set
            {
                devices = value;
                NotifyChange("Devices");
            }
        }

        private bool twoFactorAuthenticationRequested;

        public bool TwoFactorAuthenticationRequested
        {
            get
            {
                return twoFactorAuthenticationRequested;
            }

            set
            {
                twoFactorAuthenticationRequested = value;
                NotifyChange("TwoFactorAuthenticationRequested");
            }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }

            set
            {
                phoneNumber = value;
                NotifyChange("PhoneNumber");
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
