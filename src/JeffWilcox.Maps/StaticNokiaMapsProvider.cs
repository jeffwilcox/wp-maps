//
// Copyright (c) 2012 Jeff Wilcox
// Parts of this class by Christopher Maneu
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

//
// Important API Information:
// http://api.maps.nokia.com
//
// If you use the Nokia Maps static map provider, please confirm that you are
// using the control appropriately and within the API terms for the
// service (https://www.developer.nokia.com/Develop/Maps/TC.html).
//
namespace JeffWilcox.Controls
{
    public class StaticNokiaMapsProvider : StaticMapProvider
    {

        private const string StaticNokiaMapsKeyName = "NokiaMapsKey";

        private string _key;

        private const string StaticMapsUrlFormat =
        "http://m.nok.it/?" +
        "t={0}" +              // map type
        "&c={1},{2}" +          // lat,long
        "&z={3}" +              // zoomLevel
        "&w={4}&h={5}" +        // width, height
        "&app_id={6}";          // App key

        public override Uri GetStaticMap()
        {
            RequireCenter();

            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                StaticMapsUrlFormat + "&nord",
                TranslateMapMode(this.MapMode),
                Center.Latitude,
                Center.Longitude,
                ZoomLevel,
                Width,
                Height,
                _key
                ), UriKind.Absolute);
            return uri;
        }

        private string TranslateMapMode(StaticMapMode mapMode)
        {
            string mapType = string.Empty;

            switch (mapMode)
            {
                case StaticMapMode.Map:
                    mapType = "0";
                    break;
                case StaticMapMode.Satellite:
                    mapType = "1";
                    break;
                case StaticMapMode.Hybrid:
                    mapType = "3";
                    break;
                default:
                    mapType = "0";
                    break;
            }

            return mapType;
        }

        public override void Validate()
        {
            base.Validate();

            _key = this.ProviderKey;

            if (string.IsNullOrEmpty(_key))
            {
                _key = Application.Current.GetStaticResourceString(StaticNokiaMapsKeyName);

                if (string.IsNullOrEmpty(_key))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "APP DEVELOPER: You must provide an Application-wide Resource, a string, with the key '{0}' in order to use the StaticNokiaMapsProvider.", StaticNokiaMapsKeyName));
                }
            }
        }

        public override Uri GetWebBrowserMap()
        {
            // Nokia service automatically redirect to web browser version if opened in a web browser,
            // when "nord" parameter is missing
            RequireCenter();

            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                StaticMapsUrlFormat + "&nord",
                TranslateMapMode(this.MapMode),
                Center.Latitude,
                Center.Longitude,
                ZoomLevel,
                Width,
                Height,
                _key
                ), UriKind.Absolute);
            return uri;
        }
    }
}
