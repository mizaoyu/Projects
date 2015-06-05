//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// HTTP request object for a WebApi call that does not need to be disposed.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Related classes")]
    public class WebApiClientRequest
    {
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public HttpMethod Method { get; protected set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        public Uri Uri { get; protected set; }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        public string MediaType { get; protected set; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public ClientHeaders Headers { get; private set; }

        /// <summary>
        /// Gets the MediaTypeFormatter.
        /// </summary>
        public MediaTypeFormatter Formatter { get; private set; }

        /// <summary>
        /// Gets the client name.
        /// </summary>
        public string ClientName { get; private set; }

        /// <summary>
        /// Gets the operation name.
        /// </summary>
        public string OperationName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the WebApiClientRequest class.
        /// </summary>
        /// <param name="httpMethod">The HTTP method</param>
        /// <param name="uri">The URI</param>
        /// <param name="mediaType">The media type</param>
        /// <param name="headers">The request headers</param>
        /// <param name="formatter">Media Type Formatter</param>
        /// <param name="clientName">Client Name</param>
        /// <param name="operationName">Operation Name</param>
        public WebApiClientRequest(HttpMethod httpMethod, Uri uri, string mediaType, NameValueCollection headers = null, MediaTypeFormatter formatter = null, string clientName = null, string operationName = null)
        {
            this.Method = httpMethod;
            this.Uri = uri;
            this.MediaType = mediaType;
            this.Headers = ClientHeaders.FromNameValueCollection(headers);
            this.Formatter = formatter;
            this.ClientName = clientName;
            this.OperationName = operationName;
        }

        /// <summary>
        /// Creates an HttpRequestMessage that corresponds with this request.
        /// Caller needs to dispose the HttpRequestMessage
        /// </summary>
        /// <returns>An HttpRequestMessage object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller needs to dispose")]
        protected internal virtual HttpRequestMessage CreateHttpRequestMessage()
        {
            var request = new HttpRequestMessage(this.Method, this.Uri);

            // Set custom headers if specified
            if (this.Headers != null)
            {
                request.SetHeaders(this.Headers);
            }

            //request.Properties.AddAsString(LoggingPropertyConstants.PortalClientName, this.ClientName);
            //request.Properties.AddAsString(LoggingPropertyConstants.PortalOperationName, this.OperationName);

            return request;
        }
    }

    /// <summary>
    /// HTTP request object for a WebApi call that does not need to be disposed.
    /// </summary>
    /// <typeparam name="T">The request content type.</typeparam>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Related classes")]
    public class WebApiClientRequest<T> : WebApiClientRequest
    {
        /// <summary>
        /// Gets or sets the request body content.
        /// </summary>
        public T Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the WebApiClientRequest class.
        /// </summary>
        /// <param name="httpMethod">The HTTP method</param>
        /// <param name="uri">The URI</param>
        /// <param name="mediaType">The media type</param>
        /// <param name="content">The request body content</param>
        /// <param name="headers">The request headers</param>
        /// <param name="formatter">Media Type Formatter</param>
        /// <param name="clientName">Client Name</param>
        /// <param name="operationName">Operation Name</param>
        public WebApiClientRequest(HttpMethod httpMethod, Uri uri, string mediaType, T content, NameValueCollection headers = null, MediaTypeFormatter formatter = null, string clientName = null, string operationName = null)
            : base(httpMethod, uri, mediaType, headers, formatter, clientName, operationName)
        {
            this.Content = content;
        }

        /// <summary>
        /// Creates an HttpRequestMessage that corresponds with this request.
        /// </summary>
        /// <returns>An HttpRequestMessage object</returns>
        protected internal override HttpRequestMessage CreateHttpRequestMessage()
        {
            HttpRequestMessage request = base.CreateHttpRequestMessage();

            if (typeof(T) == typeof(string))
            {
                var content = (string)(object)this.Content;
                request.Content = content == null
                                      ? new StringContent(string.Empty, Encoding.UTF8, this.MediaType)
                                      : new StringContent(content);
            }
            else
            {
                MediaTypeFormatter formatter = this.Formatter != null ? this.Formatter : this.CreateMediaTypeFormatter();
                request.Content = new ObjectContent<T>(this.Content, formatter, this.MediaType);
            }

            if (!string.IsNullOrEmpty(this.MediaType))
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(this.MediaType);
            }

            return request;
        }

        private MediaTypeFormatter CreateMediaTypeFormatter()
        {
            // If Formatter is null then create one which will work with the media type
            if (string.Equals(this.MediaType, MediaTypeConstants.ApplicationJson, StringComparison.OrdinalIgnoreCase))
            {
                var formatter = new JsonMediaTypeFormatter();

                // Serialize dictionaries as 
                //  {"a":1,"b":2}
                //  Instead of 
                //  [{"Key":"a","Value":1},{"Key":"b","Value":2}]
                formatter.SerializerSettings.Converters.Add(new KeyValuePairConverter());

                // Obmit nulls completely rather than emitting
                //  {a: null }
                formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                return formatter;
            }
            else if (
                string.Equals(this.MediaType, MediaTypeConstants.TextXml, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(this.MediaType, MediaTypeConstants.ApplicationXml, StringComparison.OrdinalIgnoreCase))
            {
                var formatter = new XmlMediaTypeFormatter();
                formatter.SetSerializer<T>(new DataContractSerializer(typeof(T)));
                return formatter;
            }
            else
            {
                return null;
            }
        }
    }
}
