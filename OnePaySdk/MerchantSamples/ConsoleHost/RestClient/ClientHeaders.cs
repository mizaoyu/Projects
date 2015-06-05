//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace ConsoleHost.RestClient
{
    /// <summary>
    /// A header collection with strongly typed wrappers for basic common headers.
    /// </summary>
    [Serializable]
    public class ClientHeaders : NameValueCollection
    {
        #region Standard

        /// <summary>
        /// Gets or sets the Accept header value.
        /// </summary>
        public string Accept
        {
            get
            {
                return this.Get(HeaderConstants.Accept);
            }

            set
            {
                this.Set(HeaderConstants.Accept, value);
            }
        }

        /// <summary>
        /// Gets or sets the AcceptLanguage header value.
        /// </summary>
        public string AcceptLanguage
        {
            get
            {
                return this.Get(HeaderConstants.AcceptLanguage);
            }

            set
            {
                this.Set(HeaderConstants.AcceptLanguage, value);
            }
        }

        /// <summary>
        /// Gets or sets the Authorization header value.
        /// </summary>
        public string Authorization
        {
            get
            {
                return this.Get(HeaderConstants.Authorization);
            }

            set
            {
                this.Set(HeaderConstants.Authorization, value);
            }
        }

        /// <summary>
        /// Gets or sets the CacheControl header value.
        /// </summary>
        public string CacheControl
        {
            get
            {
                return this.Get(HeaderConstants.CacheControl);
            }

            set
            {
                this.Set(HeaderConstants.CacheControl, value);
            }
        }

        /// <summary>
        /// Gets or sets the ETag header value.
        /// </summary>
        public string ETag
        {
            get
            {
                return this.Get(HeaderConstants.ETag);
            }

            set
            {
                this.Set(HeaderConstants.ETag, value);
            }
        }

        /// <summary>
        /// Gets or sets the IfMatch header value.
        /// </summary>
        public string IfMatch
        {
            get
            {
                return this.Get(HeaderConstants.IfMatch);
            }

            set
            {
                this.Set(HeaderConstants.IfMatch, value);
            } 
        }

        /// <summary>
        /// Gets or sets the IfModifiedSince header value.
        /// </summary>
        public string IfModifiedSince
        {
            get
            {
                return this.Get(HeaderConstants.IfModifiedSince);
            }

            set
            {
                this.Set(HeaderConstants.IfModifiedSince, value);
            }
        }

        /// <summary>
        /// Gets or sets the IfNoneMatch header value.
        /// </summary>
        public string IfNoneMatch
        {
            get
            {
                return this.Get(HeaderConstants.IfNoneMatch);
            }

            set
            {
                this.Set(HeaderConstants.IfNoneMatch, value);
            }
        }

        /// <summary>
        /// Gets or sets the LastModified header value.
        /// </summary>
        public string LastModified
        {
            get
            {
                return this.Get(HeaderConstants.LastModified);
            }

            set
            {
                this.Set(HeaderConstants.LastModified, value);
            }
        }

        #endregion

        #region Non-standard

        /// <summary>
        /// Gets or sets the Version header value.
        /// </summary>
        public string Version
        {
            get
            {
                return this.Get(HeaderConstants.XmsVersion);
            }

            set
            {
                this.Set(HeaderConstants.XmsVersion, value);
            }
        }

        /// <summary>
        /// Gets or sets the ContinuationToken header value.
        /// </summary>
        public string ContinuationToken
        {
            get
            {
                return this.Get(HeaderConstants.XmsContinuationToken);
            }

            set
            {
                this.Set(HeaderConstants.XmsContinuationToken, value);
            }
        }

        /// <summary>
        /// Gets or sets the RequestId header value.
        /// </summary>
        public string RequestId
        {
            get
            {
                return this.Get(HeaderConstants.XmsRequestId);
            }

            set
            {
                this.Set(HeaderConstants.XmsRequestId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Azure active directory authorization header value.
        /// </summary>
        public string AzureActiveDirectoryAuthorization
        {
            get
            {
                return this.Get(HeaderConstants.XmsAzureActiveDirectoryAuthorization);
            }

            set
            {
                this.Set(HeaderConstants.XmsAzureActiveDirectoryAuthorization, value);
            }
         }

        #endregion

        /// <summary>
        /// Converts a NameValueCollection into a new ClientHeaders collection.
        /// </summary>
        /// <param name="headers">The NameValueCollection to convert</param>
        /// <returns>A new ClientHeaders collection</returns>
        public static ClientHeaders FromNameValueCollection(NameValueCollection headers)
        {
            var clientHeaders = new ClientHeaders();

            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    clientHeaders.Set(headers.GetKey(i), headers.Get(i));
                }
            }

            return clientHeaders;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHeaders"/> class.
        /// </summary>
        public ClientHeaders()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHeaders"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
        protected ClientHeaders(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
