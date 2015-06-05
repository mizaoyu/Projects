//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.RestClient
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// Safe Uri Generator
    /// Example usages:
    /// 1. <code>SafeUriGenerator.GetSafeUri(baseUri, "services/webspaces/{1}/sites/{2}", webspace, siteName);</code>
    /// 2. <code>SafeUriGenerator.GetSafeUriFromTemplate(baseUri, new UriTemplate("services/sqlservers/{serverName}"), "serverName");</code> 
    /// </summary>
    public static class SafeUriGenerator
    {
        private const string SafeUriReservedCharacterValidator = "^[^?/;=]*$";
        private const string SafeUriDoubleDotValidator = "(^\\.\\.$)";

        private static Lazy<Regex> regexReservedCharValidator =
                new Lazy<Regex>(() => new Regex(SafeUriReservedCharacterValidator), LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<Regex> regexDoubleDotValidator =
                new Lazy<Regex>(() => new Regex(SafeUriDoubleDotValidator), LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets Safe Uri
        /// </summary>
        /// <param name="baseUri">Base Uri</param>
        /// <param name="format">Uri Format</param>
        /// <param name="args">Arguments array</param>
        /// <returns>Safe Uri or throws an Argument Exception</returns>
        public static Uri GetSafeUri(string baseUri, string format, params string[] args)
        {
            return GetSafeUri(new Uri(baseUri), format, args);
        }

        /// <summary>
        /// Gets Safe Uri
        /// </summary>
        /// <param name="baseUri">Base Uri</param>
        /// <param name="format">Uri Format</param>
        /// <param name="args">Arguments array</param>
        /// <returns>Safe Uri or throws an Argument Exception</returns>
        public static Uri GetSafeUri(Uri baseUri, string format, params string[] args)
        {
            string relativepath = format;

            if (args != null)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    args[i].ValidUriSegment();
                }

                relativepath = string.Format(CultureInfo.InvariantCulture, format, args);
            }

            return new Uri(baseUri, new Uri(relativepath, UriKind.Relative));
        }

        /// <summary>
        /// Gets Safe Uri
        /// </summary>
        /// <param name="baseUri">Base Uri</param>        
        /// <param name="uriTemplate">Uri Format template</param>
        /// <param name="args">Arguments array</param>
        /// <returns>Safe Uri or throws an Argument Exception</returns>
        public static Uri GetSafeUriFromTemplate(Uri baseUri, UriTemplate uriTemplate, params string[] args)
        {
            if (args != null)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    args[i].ValidUriSegment();
                }

                return uriTemplate.BindByPosition(baseUri, args);
            }

            return uriTemplate.BindByPosition(baseUri);
        }

        private static void ValidUriSegment(this string stringValue)
        {
            if (regexDoubleDotValidator.Value.IsMatch(stringValue))
            {
                throw new ArgumentException("Invalid Uri segment", stringValue);
            }
        }
    }
}
