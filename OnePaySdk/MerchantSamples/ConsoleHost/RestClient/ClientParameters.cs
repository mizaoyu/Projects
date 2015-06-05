//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Security.Cryptography.X509Certificates;

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// Parameters used to create channels and set headers
    /// </summary>
    public abstract class ClientParameters
    {
        /// <summary>
        /// Gets or sets the maximum number of bytes to buffer when reading the response content.
        /// </summary>
        public int MaxResponseContentBufferSizeInBytes { get; set; }

        /// <summary>
        /// Gets or sets the Certificate that should be attached
        /// </summary>
        public X509Certificate2 Certificate { internal get; set; }

        /// <summary>
        /// Gets the Headers to add to each request.
        /// </summary>
        public ClientHeaders Headers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ClientParameters class
        /// </summary>
        protected ClientParameters()
        {
            if (this.Headers == null)
            {
                this.Headers = new ClientHeaders();
            }

            // TODO mayuro: Defaults should come from config
            this.MaxResponseContentBufferSizeInBytes = 1048576;
            // this.Headers.Version = "2013-08-01";
        }
    }
}
