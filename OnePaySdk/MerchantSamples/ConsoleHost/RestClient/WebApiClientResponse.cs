//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// HTTP response object from a WebApiClient call that does not need to be disposed.
    /// </summary>
    public class WebApiClientResponse
    {
        /// <summary>
        /// Gets or sets the response HTTP status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public ClientHeaders Headers { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiClientResponse" /> class.
        /// </summary>
        public WebApiClientResponse()
        {
            this.Headers = new ClientHeaders();
        }
    }

    /// <summary>
    /// HTTP response object from a WebApiClient call that does not need to be disposed.
    /// </summary>
    /// <typeparam name="T">The response content type.</typeparam>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Related classes")]
    public class WebApiClientResponse<T> : WebApiClientResponse
    {
        /// <summary>
        /// Gets or sets the response content.
        /// </summary>
        public T Content { get; set; }
    }
}
