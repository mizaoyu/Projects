//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay.Models.Storage
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Address : INotifyPropertyChanged
    {
        /// <summary>
        /// The addressee
        /// </summary>
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

        /// <summary>
        /// The first address line
        /// </summary>
        private string line1;

        public string Line1
        {
            get
            {
                return line1;
            }

            set
            {
                line1 = value;
                NotifyChange("Line1");
            }
        }

        /// <summary>
        /// The second address line
        /// </summary>
        private string line2;

        public string Line2
        {
            get
            {
                return line2;
            }

            set
            {
                line2 = value;
                NotifyChange("Line2");
            }
        }

        /// <summary>
        /// The city
        /// </summary>
        private string city;

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                NotifyChange("City");
            }
        }

        /// <summary>
        /// The state
        /// </summary>
        private string state;

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                NotifyChange("State");
            }
        }

        /// <summary>
        /// The zip code
        /// </summary>
        private string zipCode;

        public string ZipCode
        {
            get
            {
                return zipCode;
            }

            set
            {
                zipCode = value;
                NotifyChange("ZipCode");
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
