//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// Extension methods for WebApiClient and related types.
    /// </summary>
    public static class WebApiClientExtensions
    {
        /// <summary>
        /// Sets request headers on an HttpRequestMessage object.
        /// </summary>
        /// <param name="request">The HttpRequestMessage object.</param>
        /// <param name="headers">The headers to set.</param>
        public static void SetHeaders(this HttpRequestMessage request, HttpHeaders headers)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            foreach (var header in headers)
            {
                request.Headers.Set(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Sets request headers on an HttpRequestMessage object.
        /// </summary>
        /// <param name="request">The HttpRequestMessage object.</param>
        /// <param name="headers">The headers to set.</param>
        public static void SetHeaders(this HttpRequestMessage request, NameValueCollection headers)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            for (int i = 0; i < headers.Count; i++)
            {
                string key = headers.GetKey(i);
                IEnumerable<string> values = headers.Get(i) != null ? headers.Get(i).Split(',') : null;

                request.Headers.Set(key, values);
            }
        }

        /// <summary>
        /// Converts a HttpHeaders collection to a ClientHeaders collection.
        /// </summary>
        /// <param name="headers">The HttpHeaders to convert.</param>
        /// <returns>A ClientHeaders collection corresponding to the specified headers.</returns>
        public static ClientHeaders ToNameValueCollection(this HttpHeaders headers)
        {
            var newHeaders = new ClientHeaders();

            foreach (var header in headers)
            {
                foreach (string headerValue in header.Value)
                {
                    newHeaders.Add(header.Key, headerValue);
                }
            }

            return newHeaders;
        } 

        /// <summary>
        /// Sets the value of an entry in an HttpHeaders collection. If the header does not already exist, it will add
        /// it. If it does exist, it will overwrite the existing value.
        /// </summary>
        /// <param name="headers">The HttpHeaders collection</param>
        /// <param name="key">The header key to set</param>
        /// <param name="values">The header values to set</param>
        private static void Set(this HttpHeaders headers, string key, IEnumerable<string> values)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (headers.Contains(key))
            {
                headers.Remove(key);
            }

            headers.Add(key, values);
        }

        /// <summary>
        /// Returns the first value for the provided key in HTTP Headers (or null)
        /// </summary>
        /// <param name="headers">HTTP Headers</param>
        /// <param name="name">Key name</param>
        /// <returns>first value in provided header key or null</returns>
        internal static string GetFirstValueOrDefault(this HttpHeaders headers, string name)
        {
            if (headers != null)
            {
                IEnumerable<string> values = null;
                if (headers.TryGetValues(name, out values))
                {
                    return values.Select(v => v).FirstOrDefault<string>();
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the value for the provided key in HTTP Request Properties (or null)
        /// </summary>
        /// <param name="properties">HTTP Request Properties</param>
        /// <param name="name">Key name</param>
        /// <returns>value in provided property key or null</returns>
        internal static object ValueOrNull(this IDictionary<string, object> properties, string name)
        {
            if (properties != null)
            {
                object value = null;
                if (properties.TryGetValue(name, out value))
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the removed property for the provided key in HTTP Request Properties (or null)
        /// </summary>
        /// <param name="properties">HTTP Request Properties</param>
        /// <param name="name">Key name</param>
        /// <returns>value in provided property key or null</returns>
        internal static object RemoveProperty(this IDictionary<string, object> properties, string name)
        {
            if (properties != null)
            {
                object value = null;
                if (properties.TryGetValue(name, out value))
                {
                    properties.Remove(name);
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Add the string value to HttpRequest Properties.
        /// </summary>
        /// <param name="properties">HTTP Request Properties</param>
        /// <param name="key">Key name</param>
        /// <param name="value">The property value to set</param>
        internal static void AddAsString(this IDictionary<string, object> properties, string key, string value)
        {
            if (properties != null && !string.IsNullOrEmpty(value))
            {
                properties[key] = value;
            }
        }
    }
}
