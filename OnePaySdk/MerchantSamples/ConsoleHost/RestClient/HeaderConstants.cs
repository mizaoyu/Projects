//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// Constants class defining Headers
    /// </summary>
    public static class HeaderConstants
    {
        /// <summary>
        /// Accept Header Key
        /// </summary>
        public static readonly string Accept = "Accept";

        /// <summary>
        /// Accept Language Header Key
        /// </summary>
        public static readonly string AcceptLanguage = "Accept-Language";

        /// <summary>
        /// Authorization Header Key
        /// </summary>
        public static readonly string Authorization = "Authorization";

        /// <summary>
        /// Cache Control Header Key
        /// </summary>
        public static readonly string CacheControl = "Cache-Control";

        /// <summary>
        /// Content Length Header Key
        /// </summary>
        public static readonly string ContentLength = "Content-Length";

        /// <summary>
        /// Content Type Header Key
        /// </summary>
        public static readonly string ContentType = "Content-Type";

        /// <summary>
        /// ETag Header Key
        /// </summary>
        public static readonly string ETag = "ETag";

        /// <summary>
        /// If Match Header Key
        /// </summary>
        public static readonly string IfMatch = "If-Match";

        /// <summary>
        /// If Modified Since Header Key
        /// </summary>
        public static readonly string IfModifiedSince = "If-Modified-Since";

        /// <summary>
        /// If None Match Header Key
        /// </summary>
        public static readonly string IfNoneMatch = "If-None-Match";

        /// <summary>
        /// Last Modified Header Key
        /// </summary>
        public static readonly string LastModified = "Last-Modified";

        /// <summary>
        /// Version Header Key
        /// </summary>
        public static readonly string XmsVersion = "x-ms-version";

        /// <summary>
        /// Server Request Id (OperationTrackingId) Header Key 
        /// </summary>
        public static readonly string XmsRequestId = "x-ms-request-id";

        /// <summary>
        /// Server Request Id (OperationTrackingId) Header Key
        /// </summary>
        public static readonly string XmsTrackingId = "x-ms-tracking-id";

        /// <summary>
        /// Server Correlation Id Header Key
        /// </summary>
        public static readonly string XmsCorrelationId = "x-ms-correlation-id";

        /// <summary>
        /// Server Correlation Request Id Header Key
        /// </summary>
        public static readonly string XmsCorrelationRequestId = "x-ms-correlation-request-id";

        /// <summary>
        /// Server Routing Request Id Header Key 
        /// </summary>
        public static readonly string XmsRoutingRequestId = "x-ms-routing-request-id";

        /// <summary>
        /// Continuation Token Header key
        /// </summary>
        public static readonly string XmsContinuationToken = "x-ms-continuation-token";

        /// <summary>
        /// Client Id Header Key
        /// </summary>
        public static readonly string XmsClientId = "x-ms-client-request-id";

        /// <summary>
        /// Page Request Id Header Key
        /// </summary>
        public static readonly string XmsClientSessionId = "x-ms-client-session-id";

        /// <summary>
        /// Flags Header Key
        /// </summary>
        public static readonly string XmsFlags = "x-ms-extension-flags";

        /// <summary>
        /// The Content Type options Header Key.
        /// </summary>
        public static readonly string ContentTypeOptions = "x-content-type-options";

        /// <summary>
        /// The NoSniff Content Type options Header Value.
        /// </summary>
        public static readonly string ContentTypeOptionsNoSniffValue = "nosniff";

        /// <summary>
        /// Strict Transport Security header.
        /// </summary>
        public static readonly string StrictTransportSecurity = "Strict-Transport-Security";

        /// <summary>
        /// Strict Transport Security value.
        /// </summary>
        public static readonly string StrictTransportSecurityValue = "max-age=2592000; includeSubDomains";

        /// <summary>
        /// The cross site scripting protection header.
        /// </summary>
        public static readonly string XssProtection = "X-XSS-Protection";

        /// <summary>
        /// The cross site scripting protection header value.
        /// </summary>
        public static readonly string XssProtectionValue = "1; mode=block";

        /// <summary>
        /// Allow framing header.
        /// </summary>
        public static readonly string XFrameHeader = "X-Frame-Options";

        /// <summary>
        /// Allow framing header value.
        /// </summary>
        public static readonly string XFrameHeaderValue = "SAMEORIGIN";

        /// <summary>
        /// Azure active directory authorization header
        /// </summary>
        public static readonly string XmsAzureActiveDirectoryAuthorization = "x-ms-aad-authorization";
    }
}
