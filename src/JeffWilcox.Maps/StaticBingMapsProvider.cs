//
// Copyright (c) 2012 Jeff Wilcox
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Globalization;
using System.Windows;

// Make sure that you review the API terms and conditions for using the Bing
// Maps Static Map API.

namespace JeffWilcox.Controls
{
    public class StaticBingMapsProvider : StaticMapProvider
    {
        private const string StaticBingMapsKeyName = "BingMapsKey";

        private string _key;

        private const string PushPinFormat = "&pp={5},{6};{7}"; // lat, long, style

        private const string StaticMapsUrlFormat =
            "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/" +
            "{0},{1}" + // lat,long
            "/{2}" + // zoomLevel // int from 1 to 22, 15 seems normal-ish
            "?mapSize=" +
            "{3},{4}" + // width, height
            PushPinFormat + // lat,long of pushpin
            "&mapVersion=v1&key={8}"; // dev key

        public override Uri GetStaticMap()
        {
            RequireCenter();

            var format = StaticMapsUrlFormat.Replace(PushPinFormat, string.Empty);
            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                format,
                Center.Latitude,
                Center.Longitude,
                BingMapsHelper.ClampZoomLevel(ZoomLevel),
                Width,
                Height,
                Center.Latitude,
                Center.Longitude,
                36, // http://msdn.microsoft.com/en-us/library/ff701719.aspx    (pin styles)
                _key
                ), UriKind.Absolute);
            return uri;
        }

        public override void Validate()
        {
            base.Validate();

            if (_key == null)
            {
                _key = Application.Current.GetStaticResourceString(StaticBingMapsKeyName);

                if (string.IsNullOrEmpty(_key))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "APP DEVELOPER: You must provide an Application-wide Resource, a string, with the key '{0}' in order to use the StaticBingMapsProvider.", StaticBingMapsKeyName));
                }
            }
        }

        public override Uri GetWebBrowserMap()
        {
            RequireCenter();

            return new Uri(string.Format(
                CultureInfo.InvariantCulture,
                "http://maps.live.com/?v=2&cp={0},{1}&lvl={2}",
                Center.Latitude,
                Center.Longitude,
                BingMapsHelper.ClampZoomLevel(ZoomLevel)),
                UriKind.Absolute);
        }
    }
}
