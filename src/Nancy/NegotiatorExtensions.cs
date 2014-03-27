namespace Nancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.Responses.Negotiation;
    using Cookies;

    /// <summary>
    /// Extension methods for the <see cref="Negotiator"/> class.
    /// </summary>
    public static class NegotiatorExtensions
    {
        /// <summary>
        /// Add a cookie to the response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/> instance.</param>
        /// <param name="cookie">The <see cref="INancyCookie"/> instance that should be added.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator Cookie(this Negotiator negotiator, INancyCookie cookie)
        {
            negotiator.NegotiationContext.Cookies.Add(cookie);
            return negotiator;
        }

        /// <summary>
        /// Add a cookie to the response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/> instance.</param>
        /// <param name="cookie">The <see cref="INancyCookie"/> instance that should be added.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with Cookie and will be removed in a subsequent release.")]
        public static Negotiator WithCookie(this Negotiator negotiator, INancyCookie cookie)
        {
            return negotiator.Cookie(cookie);
        }

        /// <summary>
        /// Add a collection of cookies to the response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/> instance.</param>
        /// <param name="cookies">The <see cref="INancyCookie"/> instances that should be added.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator Cookies(this Negotiator negotiator, IEnumerable<INancyCookie> cookies)
        {
            foreach (var cookie in cookies)
            {
                negotiator.Cookie(cookie);
            }
            
            return negotiator;
        }

        /// <summary>
        /// Add a collection of cookies to the response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/> instance.</param>
        /// <param name="cookies">The <see cref="INancyCookie"/> instances that should be added.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with Cookies and will be removed in a subsequent release.")]
        public static Negotiator WithCookies(this Negotiator negotiator, IEnumerable<INancyCookie> cookies)
        {
            return negotiator.Cookies(cookies);
        }

        /// <summary>
        /// Add a header to the response
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="header">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator Header(this Negotiator negotiator, string header, string value)
        {
            return negotiator.Headers(new { Header = header, Value = value });
        }

        /// <summary>
        /// Add a header to the response
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="header">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with Header and will be removed in a subsequent release.")]
        public static Negotiator WithHeader(this Negotiator negotiator, string header, string value)
        {
            return negotiator.Header(header, value);
        }

        /// <summary>
        /// Add a content type to the response
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="contentType">Content type value</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator ContentType(this Negotiator negotiator, string contentType)
        {
          return negotiator.Headers(new { Header = "Content-Type", Value = contentType });
        }

        /// <summary>
        /// Add a content type to the response
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="contentType">Content type value</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with ContentType and will be removed in a subsequent release.")]
        public static Negotiator WithContentType(this Negotiator negotiator, string contentType)
        {
            return negotiator.ContentType(contentType);
        }

        /// <summary>
        /// Adds headers to the response using anonymous types
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="headers">
        /// Array of headers - each header should be an anonymous type with two string properties 
        /// 'Header' and 'Value' to represent the header name and its value.
        /// </param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator Headers(this Negotiator negotiator, params object[] headers)
        {
            return negotiator.Headers(headers.Select(GetTuple).ToArray());
        }

        /// <summary>
        /// Adds headers to the response using anonymous types
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="headers">
        /// Array of headers - each header should be an anonymous type with two string properties 
        /// 'Header' and 'Value' to represent the header name and its value.
        /// </param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with Headers and will be removed in a subsequent release.")]
        public static Negotiator WithHeaders(this Negotiator negotiator, params object[] headers)
        {
            return negotiator.Headers(headers);
        }

        /// <summary>
        /// Adds headers to the response using anonymous types
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="headers">
        /// Array of headers - each header should be a Tuple with two string elements 
        /// for header name and header value
        /// </param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator Headers(this Negotiator negotiator, params Tuple<string, string>[] headers)
        {
            foreach (var keyValuePair in headers)
            {
                negotiator.NegotiationContext.Headers[keyValuePair.Item1] = keyValuePair.Item2;
            }

            return negotiator;
        }

        /// <summary>
        /// Adds headers to the response using anonymous types
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="headers">
        /// Array of headers - each header should be a Tuple with two string elements 
        /// for header name and header value
        /// </param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with Headers and will be removed in a subsequent release.")]
        public static Negotiator WithHeaders(this Negotiator negotiator, params Tuple<string, string>[] headers)
        {
            return negotiator.Headers(headers);
        }

        /// <summary>
        /// Allows the response to be negotiated with any processors available for any content type
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator FullNegotiation(this Negotiator negotiator)
        {
            negotiator.NegotiationContext.PermissableMediaRanges.Clear();
            negotiator.NegotiationContext.PermissableMediaRanges.Add("*/*");

            return negotiator;
        }

        /// <summary>
        /// Allows the response to be negotiated with any processors available for any content type
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with FullNegotiation and will be removed in a subsequent release.")]
        public static Negotiator WithFullNegotiation(this Negotiator negotiator)
        {
            return negotiator.FullNegotiation();
        }

        /// <summary>
        /// Allows the response to be negotiated with a specific media range
        /// This will remove the wildcard range if it is already specified
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="mediaRange">Media range to add</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator AllowedMediaRange(this Negotiator negotiator, MediaRange mediaRange)
        {
            if (negotiator.NegotiationContext.PermissableMediaRanges.Contains(mediaRange))
            {
                return negotiator;
            }

            var wildcards =
                negotiator.NegotiationContext.PermissableMediaRanges.Where(
                    mr => mr.Type.IsWildcard && mr.Subtype.IsWildcard).ToArray();

            foreach (var wildcard in wildcards)
            {
                negotiator.NegotiationContext.PermissableMediaRanges.Remove(wildcard);
            }

            negotiator.NegotiationContext.PermissableMediaRanges.Add(mediaRange);

            return negotiator;
        }

        /// <summary>
        /// Allows the response to be negotiated with a specific media range
        /// This will remove the wildcard range if it is already specified
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="mediaRange">Media range to add</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with AllowedMediaRange and will be removed in a subsequent release.")]
        public static Negotiator WithAllowedMediaRange(this Negotiator negotiator, MediaRange mediaRange)
        {
            return negotiator.AllowedMediaRange(mediaRange);
        }

        /// <summary>
        /// Uses the specified model as the default model for negotiation
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="model">Model object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator DefaultModel(this Negotiator negotiator, dynamic model)
        {
            negotiator.NegotiationContext.DefaultModel = model;

            return negotiator;
        }

        /// <summary>
        /// Uses the specified model as the default model for negotiation
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="model">Model object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with DefaultModel and will be removed in a subsequent release.")]
        public static Negotiator WithModel(this Negotiator negotiator, dynamic model)
        {
            return negotiator.DefaultModel((object)model);
        }

        /// <summary>
        /// Uses the specified view for html output
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="viewName">View name</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator View(this Negotiator negotiator, string viewName)
        {
            negotiator.NegotiationContext.ViewName = viewName;

            return negotiator;
        }

        /// <summary>
        /// Uses the specified view for html output
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="viewName">View name</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with View and will be removed in a subsequent release.")]
        public static Negotiator WithView(this Negotiator negotiator, string viewName)
        {
            return negotiator.View(viewName);
        }

        /// <summary>
        /// Sets the model to use for a particular media range.
        /// Will also add the <see cref="MediaRange"/> to the allowed list
        /// </summary>
        /// <typeparam name="TModel">The <see cref="Type"/> of the model.</typeparam>
        /// <param name="negotiator"><see cref="Negotiator"/> object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="model">Model object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator MediaRangeModel<TModel>(this Negotiator negotiator, MediaRange range, TModel model)
        {
            negotiator.AllowedMediaRange(range);
            negotiator.NegotiationContext.MediaRangeModelMappings.Add(range, new MediaRangeModel(typeof(TModel), () => model));

            return negotiator;
        }

        /// <summary>
        /// Sets the model to use for a particular media range.
        /// Will also add the <see cref="MediaRange"/> to the allowed list
        /// </summary>
        /// <typeparam name="TModel">The <see cref="Type"/> of the model.</typeparam>
        /// <param name="negotiator"><see cref="Negotiator"/> object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="model">Model object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with MediaRangeModel and will be removed in a subsequent release.")]
        public static Negotiator WithMediaRangeModel<TModel>(this Negotiator negotiator, MediaRange range, TModel model)
        {
            return negotiator.MediaRangeModel(range, model);
        }

        /// <summary>
        /// Sets the model to use for a particular media range.
        /// Will also add the <see cref="MediaRange"/> to the allowed list
        /// </summary>
        /// <typeparam name="TModel">The <see cref="Type"/> of the model.</typeparam>
        /// <param name="negotiator"><see cref="Negotiator"/> object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="modelFactory">Factory that will produce a model instance.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator MediaRangeModel<TModel>(this Negotiator negotiator, MediaRange range, Func<TModel> modelFactory)
        {
            negotiator.AllowedMediaRange(range);
            negotiator.NegotiationContext.MediaRangeModelMappings.Add(range, new MediaRangeModel(typeof(TModel), modelFactory));

            return negotiator;
        }

        /// <summary>
        /// Sets the model to use for a particular media range.
        /// Will also add the <see cref="MediaRange"/> to the allowed list
        /// </summary>
        /// <typeparam name="TModel">The <see cref="Type"/> of the model.</typeparam>
        /// <param name="negotiator"><see cref="Negotiator"/> object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="modelFactory">Factory that will produce a model instance.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with MediaRangeModel and will be removed in a subsequent release.")]
        public static Negotiator WithMediaRangeModel<TModel>(this Negotiator negotiator, MediaRange range, Func<TModel> modelFactory)
        {
            return negotiator.MediaRangeModel(range, modelFactory);
        }

        /// <summary>
        /// Sets the <see cref="Response"/> to use for a particular media range.
        /// Will also add the MediaRange to the allowed list
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="response">A <see cref="Response"/> object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator MediaRangeResponse(this Negotiator negotiator, MediaRange range, Response response)
        {
            return negotiator.MediaRangeModel(range, response);
        }

        /// <summary>
        /// Sets the <see cref="Response"/> to use for a particular media range.
        /// Will also add the MediaRange to the allowed list
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="response">A <see cref="Response"/> object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with MediaRangeResponse and will be removed in a subsequent release.")]
        public static Negotiator WithMediaRangeResponse(this Negotiator negotiator, MediaRange range, Response response)
        {
            return negotiator.MediaRangeResponse(range, response);
        }

        /// <summary>
        /// Sets the <see cref="Response"/> to use for a particular media range.
        /// Will also add the MediaRange to the allowed list
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="responseFactory">Factory for returning the <see cref="Response"/> object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator MediaRangeResponse(this Negotiator negotiator, MediaRange range, Func<Response> responseFactory)
        {
            return negotiator.MediaRangeModel(range, responseFactory);
        }

        /// <summary>
        /// Sets the <see cref="Response"/> to use for a particular media range.
        /// Will also add the MediaRange to the allowed list
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="range">Range to match against</param>
        /// <param name="responseFactory">Factory for returning the <see cref="Response"/> object</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with MediaRangeResponse and will be removed in a subsequent release.")]
        public static Negotiator WithMediaRangeResponse(this Negotiator negotiator, MediaRange range, Func<Response> responseFactory)
        {
            return negotiator.MediaRangeResponse(range, responseFactory);
        }

        /// <summary>
        /// Sets the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="statusCode">The status code that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator StatusCode(this Negotiator negotiator, int statusCode)
        {
            negotiator.NegotiationContext.StatusCode = (HttpStatusCode)statusCode;
            return negotiator;
        }

        /// <summary>
        /// Sets the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="statusCode">The status code that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with StatusCode and will be removed in a subsequent release.")]
        public static Negotiator WithStatusCode(this Negotiator negotiator, int statusCode)
        {
            return negotiator.StatusCode((HttpStatusCode)statusCode);
        }

        /// <summary>
        /// Sets the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="statusCode">The status code that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator StatusCode(this Negotiator negotiator, HttpStatusCode statusCode)
        {
            negotiator.NegotiationContext.StatusCode = statusCode;
            return negotiator;
        }

        /// <summary>
        /// Sets the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="statusCode">The status code that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with StatusCode and will be removed in a subsequent release.")]
        public static Negotiator WithStatusCode(this Negotiator negotiator, HttpStatusCode statusCode)
        {
            return negotiator.StatusCode(statusCode);
        }

        /// <summary>
        /// Sets the description of the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="reasonPhrase">The status code description that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        public static Negotiator ReasonPhrase(this Negotiator negotiator, string reasonPhrase)
        {
            negotiator.NegotiationContext.ReasonPhrase = reasonPhrase;
            return negotiator;
        }

        /// <summary>
        /// Sets the description of the status code that should be assigned to the final response.
        /// </summary>
        /// <param name="negotiator">Negotiator object</param>
        /// <param name="reasonPhrase">The status code description that should be used.</param>
        /// <returns>Reference to the updated <see cref="Negotiator"/> instance.</returns>
        [Obsolete("This method has been replaced with ReasonPhrase and will be removed in a subsequent release.")]
        public static Negotiator WithReasonPhrase(this Negotiator negotiator, string reasonPhrase)
        {
            return negotiator.ReasonPhrase(reasonPhrase);
        }

        private static Tuple<string, string> GetTuple(object header)
        {
            var properties = header.GetType()
                .GetProperties()
                .Where(prop => prop.CanRead && prop.PropertyType == typeof(string))
                .ToArray();

            var headerProperty = 
                properties.FirstOrDefault(p => string.Equals(p.Name, "Header", StringComparison.InvariantCultureIgnoreCase));

            var valueProperty = 
                properties.FirstOrDefault(p => string.Equals(p.Name, "Value", StringComparison.InvariantCultureIgnoreCase));

            if (headerProperty == null || valueProperty == null)
            {
                throw new ArgumentException("Unable to extract 'Header' or 'Value' properties from anonymous type.");
            }

            return Tuple.Create(
                (string)headerProperty.GetValue(header, null),
                (string)valueProperty.GetValue(header, null));
        }
    }
}