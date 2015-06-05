//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// WebAPI Client
    /// </summary>
    public class WebApiClient : IWebApiClient
    {
        private static Random random = new Random();
        private WebApiClientParameters clientParameters;
        private MessageProcessingHandler messageHandler;
        private HttpClient httpClient;
        private WebRequestHandler webRequestHandler;

        /// <summary>Dictionary key for optional Headers collection in HttpExceptions</summary>
        public const string HttpExceptionDataHeaders = "Headers";
        public const string HttpExceptionDataReason = "ReasonPhrase";

        /// <summary>Dictionary key for the request message content in HttpExceptions</summary>
        public const string HttpExceptionDataRequestMessageContent = "RequestMessage.Content";

        /// <summary>
        /// Initializes a new instance of the WebApiClient class
        /// </summary>
        public WebApiClient() :
            this(new WebApiClientParameters())
        {
        }

        /// <summary>
        /// Initializes a new instance of the WebApiClient class.
        /// </summary>
        /// <param name="clientParameters">Custom WebAPI Client parameters</param>
        public WebApiClient(WebApiClientParameters clientParameters)
        {
            this.clientParameters = clientParameters;

            this.webRequestHandler = new WebRequestHandler();
            if (clientParameters.Certificate != null)
            {
                this.webRequestHandler.ClientCertificates.Add(clientParameters.Certificate);
            }

            this.messageHandler = new WebApiOutgoingRequestHandler(this.webRequestHandler, clientParameters);
            this.httpClient = CreateHttpClient(clientParameters, this.messageHandler);
        }

        /// <summary>
        /// Dispose called by the user
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var httpClient = this.httpClient;
                if (httpClient != null)
                {
                    httpClient.Dispose();
                    this.httpClient = null;
                }

                var messageHandler = this.messageHandler;
                if (messageHandler != null)
                {
                    messageHandler.Dispose();
                    this.messageHandler = null;
                }

                var webRequestHandler = this.webRequestHandler;
                if (webRequestHandler != null)
                {
                    webRequestHandler.Dispose();
                    this.webRequestHandler = null;
                }
            }
        }

        /// <summary>
        /// Handle error response and allow customized error response handling
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual async Task<WebApiClientResponse<T>> ErrorResponseHandler<T>(
            HttpResponseMessage response, HttpRequestMessage request, string mediaType, MediaTypeFormatter formatter = null)
        {
            string errorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            HttpException exception = new HttpException((int)response.StatusCode, errorMessage);
            exception.Data.Add(HttpExceptionDataHeaders, WebApiClientExtensions.ToNameValueCollection(response.Headers));
            exception.Data.Add(HttpExceptionDataReason, response.ReasonPhrase);
            throw exception;
        }

        /// <summary>
        /// Performs an HTTP GET against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<T> GetAsync<T>(Uri uri, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest request = this.CreateRequest(HttpMethod.Get, uri, operationName, headers);
            WebApiClientResponse<T> response = await this.SendAsync<T>(request).ConfigureAwait(false);
            return response.Content;
        }

        /// <summary>
        /// Performs an HTTP DELETE against a specified URI.
        /// </summary>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>Void task</returns>
        public async Task DeleteAsync(Uri uri, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest request = this.CreateRequest(HttpMethod.Delete, uri, operationName, headers);
            await this.SendAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP DELETE against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<T> DeleteAsync<T>(Uri uri, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest request = this.CreateRequest(HttpMethod.Delete, uri, operationName, headers);
            WebApiClientResponse<T> response = await this.SendAsync<T>(request).ConfigureAwait(false);
            return response.Content;
        }

        /// <summary>
        /// Performs an HTTP POST against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<T> PostAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null)
        {
            return await this.PostAsync<T, T>(uri, content, operationName, headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP POST against a specified URI.
        /// </summary>
        /// <typeparam name="TRequest">The request body type</typeparam>
        /// <typeparam name="TResponse">The response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest<TRequest> request = this.CreateRequest(HttpMethod.Post, uri, content, operationName, headers);
            WebApiClientResponse<TResponse> response = await this.SendAsync<TResponse>(request).ConfigureAwait(false);
            return response.Content;
        }

        /// <summary>
        /// Performs an HTTP PUT against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<T> PutAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null)
        {
            return await this.PutAsync<T, T>(uri, content, operationName, headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP PUT against a specified URI.
        /// </summary>
        /// <typeparam name="TRequest">The request body type</typeparam>
        /// <typeparam name="TResponse">The response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<TResponse> PutAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest<TRequest> request = this.CreateRequest(HttpMethod.Put, uri, content, operationName, headers);
            WebApiClientResponse<TResponse> response = await this.SendAsync<TResponse>(request).ConfigureAwait(false);
            return response.Content;
        }

        /// <summary>
        /// Performs an HTTP PATCH against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<T> PatchAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null)
        {
            return await this.PatchAsync<T, T>(uri, content, operationName, headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP PATCH against a specified URI.
        /// </summary>
        /// <typeparam name="TRequest">The request body type</typeparam>
        /// <typeparam name="TResponse">The response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The operation name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        public async Task<TResponse> PatchAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null)
        {
            WebApiClientRequest<TRequest> request = this.CreateRequest(new HttpMethod("PATCH"), uri, content, operationName, headers);
            WebApiClientResponse<TResponse> response = await this.SendAsync<TResponse>(request).ConfigureAwait(false);
            return response.Content;
        }

        /// <summary>
        /// Sends a client request.
        /// </summary>
        /// <param name="request">The request to send</param>
        /// <returns>A client response</returns>
        public async Task<WebApiClientResponse> SendAsync(WebApiClientRequest request)
        {
            return await this.SendAsync<object>(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a client request.
        /// </summary>
        /// <typeparam name="T">The response body type</typeparam>
        /// <param name="request">The request to send</param>
        /// <returns>A client response</returns>
        public async Task<WebApiClientResponse<T>> SendAsync<T>(WebApiClientRequest request)
        {
            this.SetConnectionLeaseTimeout(request.Uri);

            using (var httpRequestMessage = request.CreateHttpRequestMessage())
            {
                // Record the expected timeout so we can improve the exception thrown / logging
                DateTimeOffset timeoutExpectedAt = DateTimeOffset.UtcNow.Add(this.httpClient.Timeout);

                // Capture request body in case we need it for error logging purposes (as calling httpClient.SendAsync disposes it)
                string requestBody = httpRequestMessage.Content == null ? string.Empty : await httpRequestMessage.Content.ReadAsStringAsync();
                try
                {
                    using (var httpResponseMessage = await this.httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false))
                    {
                        if (!httpResponseMessage.IsSuccessStatusCode)
                        {
                            return await ErrorResponseHandler<T>(httpResponseMessage, httpRequestMessage, this.clientParameters.MediaType, this.clientParameters.Formatter).ConfigureAwait(false);
                        }

                        return await HandleResponseAsync<T>(httpResponseMessage, this.clientParameters.MediaType, this.clientParameters.Formatter).ConfigureAwait(false);
                    }
                }
                catch (TaskCanceledException exception)
                {
                    // .NET throws TaskCanceledExceptions for certain types of commonly seen timeouts. Throw a WebException if we think this is the case here.
                    this.ThrowWebExceptionIfTimeout(exception, timeoutExpectedAt, httpRequestMessage.RequestUri, requestBody);

                    // Since ThrowIfTimeout didn't throw, we have a genuine TaskCanceledException. Add request detail for error logging purposes & rethrow
                    exception.Data.Add(HttpExceptionDataRequestMessageContent, requestBody);
                    throw;
                }
                catch (OperationCanceledException exception)
                {
                    // .NET throws OperationCanceledException for certain types of commonly seen timeouts. Throw a WebException if we think this is the case here.
                    this.ThrowWebExceptionIfTimeout(exception, timeoutExpectedAt, httpRequestMessage.RequestUri, requestBody);

                    // Since ThrowIfTimeout didn't throw, we have a genuine OperationCanceledException. Add request detail for error logging purposes & rethrow
                    exception.Data.Add(HttpExceptionDataRequestMessageContent, requestBody);
                    throw;
                }
                catch (Exception exception)
                {
                    exception.Data.Add(HttpExceptionDataRequestMessageContent, requestBody);
                    throw;
                }
            }
        }

        /// <summary>Checks to see if the request has exceeded its timeout limit. If it has, it wraps the passed exception in a timeout WebException and throws.
        /// If the request has not exceeded its timeout limit, this method does nothing.</summary>
        /// <param name="exception">The exception being examined</param>
        /// <param name="timeoutExpectedAt">The time at which the request would be expected to timeout</param>
        /// <param name="requestUri">The Uri being requested. Used to construct the thrown exception</param>
        /// <param name="requestBody">The body of the request. Used to construct the thrown exception</param>
        /// <exception cref="WebException">A timeout WebException (wrapping the original exception) is thrown if the cause of the exception is likely to be timeout.</exception>
        private void ThrowWebExceptionIfTimeout(Exception exception, DateTimeOffset timeoutExpectedAt, Uri requestUri, string requestBody)
        {
            if (this.httpClient.Timeout > TimeSpan.Zero && DateTimeOffset.UtcNow >= timeoutExpectedAt)
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Heuristics indicate WebApiClient request timed out. Uri: {0}\r\nTimeout: {1}",
                    requestUri == null ? "[null]" : requestUri.ToString(),
                    this.httpClient.Timeout);
                WebException wrapper = new WebException(message, exception, WebExceptionStatus.Timeout, null);
                wrapper.Data.Add(HttpExceptionDataRequestMessageContent, requestBody);
                throw wrapper;
            }
        }

        /// <summary>
        /// Creates a request object using the client parameters associated with the client.
        /// </summary>
        /// <param name="httpMethod">The HTTP method</param>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The operation name</param>
        /// <param name="headers">The request headers</param>
        /// <returns>A WebApiClientRequest object</returns>
        public WebApiClientRequest CreateRequest(HttpMethod httpMethod, Uri uri, string operationName, NameValueCollection headers = null)
        {
            return new WebApiClientRequest(httpMethod, uri, this.clientParameters.MediaType, headers, this.clientParameters.Formatter, this.clientParameters.ClientName, operationName);
        }

        /// <summary>
        /// Creates a request object using the client parameters associated with the client.
        /// </summary>
        /// <typeparam name="T">The request body type</typeparam>
        /// <param name="httpMethod">The HTTP method verb</param>
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The operation name</param>
        /// <param name="headers">The request headers</param>
        /// <returns>A WebApiClientRequest object</returns>
        public WebApiClientRequest<T> CreateRequest<T>(HttpMethod httpMethod, Uri uri, T content, string operationName, NameValueCollection headers = null)
        {
            return new WebApiClientRequest<T>(httpMethod, uri, this.clientParameters.MediaType, content, headers, this.clientParameters.Formatter, this.clientParameters.ClientName, operationName);
        }

        /// <summary>
        /// Handle successful response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpResponseMessage"></param>
        /// <param name="mediaType"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        private static async Task<WebApiClientResponse<T>> HandleResponseAsync<T>(HttpResponseMessage httpResponseMessage, string mediaType, MediaTypeFormatter formatter = null)
        {
            // TODO mayuro: workaround RDFE bug that the response's header is fixed 'application/xml'
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            T content = default(T);

            if (typeof(T) == typeof(string))
            {
                string response = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                content = (T)Convert.ChangeType(response, typeof(T));
            }
            else
            {
                if (formatter != null)
                {
                    List<MediaTypeFormatter> formatters = new List<MediaTypeFormatter>();
                    formatters.Add(formatter);

                    content = await httpResponseMessage.Content.ReadAsAsync<T>(formatters).ConfigureAwait(false);
                }
                else
                {
                    content = await httpResponseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false);
                }
            }

            var result = new WebApiClientResponse<T>
            {
                HttpStatusCode = httpResponseMessage.StatusCode,
                Content = content
            };

            result.Headers.Add(httpResponseMessage.Headers.ToNameValueCollection());
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is a private method and all callers correctly dispose of returned object")]
        private static HttpClient CreateHttpClient(WebApiClientParameters clientParameters, MessageProcessingHandler handler)
        {
            var httpClient = new HttpClient(handler)
            {
                // MaxResponseContentBufferSize = clientParameters.MaxResponseContentBufferSizeInBytes,
                // Timeout = clientParameters.HttpClientTimeout
            };

            httpClient.DefaultRequestHeaders.Remove(HttpRequestHeader.Authorization.ToString());
            var mediaTypeWithQualityHeaderValue = new MediaTypeWithQualityHeaderValue(clientParameters.MediaType);
            httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);

            // httpClient.DefaultRequestHeaders.Add(HeaderConstants.XmsVersion, clientParameters.Headers.Version);

            return httpClient;
        }

        /// <summary>Helper function to set the connection lease timeout</summary>
        /// <param name="endpointUri">Endpoint for which the connection lease should be set</param>
        private void SetConnectionLeaseTimeout(Uri endpointUri)
        {
            // Improve load balancing by overriding the connection lease timeout if it has its default value of -1 (-1 = no timeout)
            // If we keep connections open all the time then only a subset of servers we connected to get our traffic. Setting the LeaseTimeout property causes 
            // the connection to be dropped at a pseudo-regular interval, giving the load balancer a chance to connecting us to a different server when we reconnect.
            // Note: We apply a slight amount of randomness to the timeout to prevent leases from batch operations all expiring at once.
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(endpointUri);
            if (servicePoint.ConnectionLeaseTimeout == -1)
            {
                servicePoint.ConnectionLeaseTimeout = (this.clientParameters.ConnectionLeaseTimeoutInSeconds < 0) ?
                    -1 : this.clientParameters.ConnectionLeaseTimeoutInSeconds * (750 + WebApiClient.random.Next(500));
            }
        }
    }
}