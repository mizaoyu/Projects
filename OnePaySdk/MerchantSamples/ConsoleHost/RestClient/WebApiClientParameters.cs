//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleHost.RestClient
{
    /// <summary>WebApiClient request inspector</summary>
    /// <param name="request">The request</param>
    public delegate void WebApiClientRequestInspector(HttpRequestMessage request);

    /// <summary>WebApiClient response inspector</summary>
    /// <param name="response">The response</param>
    public delegate void WebApiClientResponseInspector(HttpResponseMessage response);

    /// <summary>
    /// Parameters used to create WebAPI clients and set headers
    /// </summary>
    public sealed class WebApiClientParameters
    {
        /// <summary>
        /// endpoint (base uri)
        /// </summary>
        public string Endpoint { get; set; }

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
        /// Gets or sets the MediaType
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Gets or sets the HttpClientTimeout
        /// </summary>
        public TimeSpan HttpClientTimeout { get; set; }

        /// <summary>
        /// Gets or sets the Media Type Formatter. If not specified then will default to DataContractSerializer
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the auth token will never be sent
        /// </summary>
        public bool NeverSendAuthToken { get; set; }

        /// <summary>
        /// Gets or sets the ClientName
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the optional callback that can inspect the outgoing requests
        /// </summary>
        /// <remarks>Optional. If set, will be called after the request has been constructed, but before the request is logged and sent.
        /// Exceptions thrown by your inspector will be logged as warnings. If success of your inspector is critical, it should include
        /// its own error logging.</remarks>
        public WebApiClientRequestInspector RequestInspector { get; set; }

        /// <summary>
        /// Gets or sets the optional callback that can inspect the response to the request
        /// </summary>
        /// <remarks>Optional. If set, will be called after the response has been received and logged.
        /// Exceptions thrown by your inspector will be logged as warnings. If success of your inspector is critical, it should include
        /// its own error logging.</remarks>
        public WebApiClientResponseInspector ResponseInspector { get; set; }

        /// <summary>
        /// Gets or sets the connection lease timeout (in seconds)
        /// </summary>
        public int ConnectionLeaseTimeoutInSeconds { get; set; }

        /// <summary>
        /// Initializes a new instance of the WebApiClientParameters class with defaults
        /// </summary>
        /// <remarks>Indicate this instance can be used in non-user context (for example, a polling operation triggered by a periodic timer). 
        /// Non-user context does NOT include cases where you have lost track of the user&apos; HttpContext.</remarks>
        public WebApiClientParameters()
        {
            if (this.Headers == null)
            {
                this.Headers = new ClientHeaders();
            }

            // TODO: Defaults should come from config
            this.MaxResponseContentBufferSizeInBytes = 1048576;

            this.HttpClientTimeout = TimeSpan.FromSeconds(90);
            this.ConnectionLeaseTimeoutInSeconds = 60;
            this.MediaType = MediaTypeConstants.ApplicationJson;
            this.ClientName = "WebApiClient";
        }
    }
}
