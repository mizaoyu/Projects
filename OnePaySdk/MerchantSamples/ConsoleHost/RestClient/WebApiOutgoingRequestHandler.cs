//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace ConsoleHost.RestClient
{
    internal sealed class WebApiOutgoingRequestHandler : MessageProcessingHandler
    {
        private readonly WebApiClientParameters parameters;

        public WebApiOutgoingRequestHandler(HttpMessageHandler handler, WebApiClientParameters parameters = null)
            : base(handler)
        {
            this.parameters = parameters;
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // If a custom message inspector has been added (typically for additional logging), call it
            if (this.parameters.RequestInspector != null)
            {
                try
                {
                    this.parameters.RequestInspector(request);
                }
                catch
                {
                    // Log & swallow
                    // there is not a common logging interface for this class to use among
                    // all caller environments, the custom inspector should log and handle
                    // exceptions
                    // this.LogInspectorWarning("Request", request.RequestUri, exception);
                }
            }

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            // If a custom message inspector has been added (typically for additional logging), call it.
            // Note the exceuction order of the custom inspectors is not the same as would be obtained by chaining MessageProcessingHandler's
            if (this.parameters.ResponseInspector != null)
            {
                try
                {
                    this.parameters.ResponseInspector(response);
                }
                catch
                {
                    // Log & swallow
                    // there is not a common logging interface for this class to use among
                    // all caller environments, the custom inspector should log and handle
                    // exceptions
                    // this.LogInspectorWarning("Response", response.RequestMessage.RequestUri, exception);
                }
            }

            return response;
        }

        /// <summary>Logs a warning message about an inspector failure
        /// there is not a common logging interface for this class to use among
        /// all caller environments, the custom inspector should log and handle
        /// exceptions
        /// restore this in case necessary
        /// </summary>
        /// <param name="inspectorType">Inspector type</param>
        /// <param name="uri">URI that was requested</param>
        /// <param name="exception">The exception that was thrown</param>
        //private void LogInspectorWarning(string inspectorType, Uri uri, Exception exception)
        //{
            //string message = string.Format(
            //    CultureInfo.InvariantCulture,
            //    "{0} inspector threw an exception. URI: {1}{2}Exception: {3}",
            //    inspectorType,
            //    uri == null ? "[null]" : uri.ToString(),
            //    Environment.NewLine,
            //    exception);
            //this.commonTracer.Warning(message);
        //}
    }
}
