//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.RestClient
{
    using System;
    using System.Collections.Specialized;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// WebAPI Client Interface
    /// </summary>
    public interface IWebApiClient : IDisposable
    {
        /// <summary>
        /// Performs an HTTP GET against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<T> GetAsync<T>(Uri uri, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP DELETE against a specified URI.
        /// </summary>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>Void task</returns>
        Task DeleteAsync(Uri uri, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP DELETE against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<T> DeleteAsync<T>(Uri uri, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP POST against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<T> PostAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null);

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
        Task<TResponse> PostAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP PUT against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<T> PutAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null);

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
        Task<TResponse> PutAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP PATCH against a specified URI.
        /// </summary>
        /// <typeparam name="T">The request and response body type</typeparam> 
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<T> PatchAsync<T>(Uri uri, T content, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Performs an HTTP PATCH against a specified URI.
        /// </summary>
        /// <typeparam name="TRequest">The request body type</typeparam>
        /// <typeparam name="TResponse">The response body type</typeparam>
        /// <param name="uri">The URI</param>
        /// <param name="content">The request body content</param>
        /// <param name="operationName">The Operation Name</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns>The response body content</returns>
        Task<TResponse> PatchAsync<TRequest, TResponse>(Uri uri, TRequest content, string operationName, NameValueCollection headers = null);

        /// <summary>
        /// Sends a client request.
        /// </summary>
        /// <param name="request">The request to send</param>
        /// <returns>A client response</returns>
        Task<WebApiClientResponse> SendAsync(WebApiClientRequest request);

        /// <summary>
        /// Sends a client request.
        /// </summary>
        /// <typeparam name="T">The response body type</typeparam>
        /// <param name="request">The request to send</param>
        /// <returns>A client response</returns>
        Task<WebApiClientResponse<T>> SendAsync<T>(WebApiClientRequest request);

        /// <summary>
        /// Creates a request object using the client parameters associated with the client.
        /// </summary>
        /// <param name="httpMethod">The HTTP method</param>
        /// <param name="uri">The URI</param>
        /// <param name="operationName">The operation name</param>
        /// <param name="headers">The request headers</param>
        /// <returns>A WebApiClientRequest object</returns>
        WebApiClientRequest CreateRequest(HttpMethod httpMethod, Uri uri, string operationName, NameValueCollection headers = null);

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
        WebApiClientRequest<T> CreateRequest<T>(HttpMethod httpMethod, Uri uri, T content, string operationName, NameValueCollection headers = null);
    }
}