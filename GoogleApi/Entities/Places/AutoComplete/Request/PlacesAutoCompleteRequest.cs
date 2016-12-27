﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Common.Interfaces;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Extensions;
using GoogleApi.Helpers;

namespace GoogleApi.Entities.Places.AutoComplete.Request
{
	/// <summary>
	/// The Google Places API is a service that returns information about a "place" (hereafter referred to as a Place) — defined within this API as an establishment, a geographic location, or prominent point of interest — using an HTTP request. Place requests specify locations as latitude/longitude coordinates.
	/// Two basic Place requests are available: a Place Search request and a Place Details request. Generally, a Place Search request is used to return candidate matches, while a Place Details request returns more specific information about a Place.
	/// This service is designed for processing place requests generated by a user for placement of application content on a map; this service is not designed to respond to batch of offline queries, which are a violation of its terms of use.
	/// </summary>
    public class PlacesAutoCompleteRequest : BasePlacesRequest, IQueryStringRequest
	{
        /// <summary>
        /// The text string on which to search. The Place service will return candidate matches based on this string and order results based on their perceived relevance.
        /// </summary>
        public virtual string Input { get; set; } 

        /// <summary>
        /// The character position in the input term at which the service uses text for predictions. For example, if the input is 'Googl' and the completion point is 3, the service will match on 'Goo'. The offset should generally be set to the position of the text caret. If no offset is supplied, the service will use the entire term.        
        /// </summary>
        public virtual string Offset { get; set; }

        /// <summary>
        /// The point around which you wish to retrieve Place information. Must be specified as latitude,longitude.
		/// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// The distance (in meters) within which to return Place results. Note that setting a radius biases results to the indicated area, but may not fully restrict results to the specified area. See Location Biasing below.
		/// </summary>
        public virtual double? Radius { get; set; }

		/// <summary>
        /// The language in which to return results. See the supported list of domain languages. Note that we often update supported languages so this list may not be exhaustive. If language is not supplied, the Place service will attempt to use the native language of the domain from which the request is sent.
		/// </summary>
        public virtual string Language { get; set; }

        /// <summary>
        /// The types of Place results to return. See Place Types below. If no type is specified, all types will be returned.
        /// https://developers.google.com/places/supported_types#table3
        /// </summary>
        public virtual IEnumerable<RestrictPlaceType> Types { get; set; }

        [DataMember(Name = "types")]
        private IEnumerable<string> TypesStr
        {
            get
            {
                return this.Types.Select(_x => _x.ToEnumString());
            }
            set
            {
                this.Types = value.Select(_x => _x.ToEnum<RestrictPlaceType>());
            }
        }

        /// <summary>
        /// The component filters, separated by a pipe (|). Each component filter consists of a component:value pair and will fully restrict the results from the geocoder. For more information see Component Filtering.
		/// </summary>
        public virtual Dictionary<Component, string> Components { get; set; }

        /// <summary>
        /// BaseUrl property overridden.
        /// </summary>
        protected internal override string BaseUrl
        {
            get
            {
                return base.BaseUrl + "autocomplete/json";
            }
        }

        /// <summary>
        /// Get the query string collection of added parameters for the request.
        /// </summary>
        /// <returns></returns>
	    protected override QueryStringParametersList GetQueryStringParameters()
		{
            if (string.IsNullOrEmpty(this.Key))
                throw new ArgumentException("ApiKey must not null or empty");

            if (string.IsNullOrEmpty(this.Input))
                throw new ArgumentException("Input must not null or empty");

            if (this.Radius.HasValue && (this.Radius > 50000 || this.Radius < 1))
				throw new ArgumentException("Radius must be greater than or equal to 1 and less than or equal to 50000.");

			var _parameters = base.GetQueryStringParameters();

            _parameters.Add("key", this.Key);
            _parameters.Add("input", this.Input);

            if (!string.IsNullOrEmpty(this.Offset))
                _parameters.Add("offset", this.Offset);

            if (this.Location != null)
                _parameters.Add("location", this.Location.ToString());
	
            if (this.Radius.HasValue)
				_parameters.Add("radius", this.Radius.Value.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(this.Language))
                _parameters.Add("language", this.Language);

            if (this.Types != null && this.Types.Any())
                _parameters.Add("types", string.Join("|", this.Types.Select(_x => string.Format("{0}", _x.ToString().ToLower()))));

            if (this.Components != null && this.Components.Any())
                _parameters.Add("components", string.Join("|", this.Components.Select(_x => string.Format("{0}:{1}", _x.Key.ToString().ToLower(), _x.Value))));
            
            return _parameters;
		}
	}
}
