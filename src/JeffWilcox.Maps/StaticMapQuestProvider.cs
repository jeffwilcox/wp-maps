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

//
// Important API Information:
// http://www.mapquestapi.com/staticmap/
//
// If you use the MapQuest static map provider, please confirm that you are
// using the control appropriately and within the API guidelines for the
// service.
//

namespace JeffWilcox.Controls
{
    public class StaticMapQuestProvider : StaticMapProvider
    {
        private const string StaticMapQuestKeyName = "MapQuestKey";

        private string _key;

        // http://www.mapquestapi.com/staticmap/v3/getmap?key=YOUR_KEY_HERE&size=400,200&zoom=7&center=40.0378,-76.305801
        private const string StaticMapsUrlFormat =
            "http://www.mapquestapi.com/staticmap/v3/getmap?" +
            "key={0}" + // dev key
            "&size={1},{2}" + // width, height
            "&zoom={3}" + // zoom
            "&center={4},{5}" + // lat, long
            "&type={6}" + //map type
            "&imageType=png"; // image type

        public override Uri GetStaticMap()
        {
            RequireCenter();

            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                StaticMapsUrlFormat,
                _key,
                Width,
                Height,
                BingMapsHelper.ClampZoomLevel(ZoomLevel),
                Center.Latitude,
                Center.Longitude,
                TranslateMapMode(this.MapMode)
                ), UriKind.Absolute);

            return uri;
        }

        private string TranslateMapMode(StaticMapMode mode)
        {
            string mapType = string.Empty;

            switch (mode)
            {
                case StaticMapMode.Map:
                    mapType = "map";
                    break;
                case StaticMapMode.Satellite:
                    mapType = "sat";
                    break;
                case StaticMapMode.Hybrid:
                    mapType = "hyb";
                    break;
                default:
                    mapType = "map";
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
                _key = Application.Current.GetStaticResourceString(StaticMapQuestKeyName);

                if (string.IsNullOrEmpty(_key))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "APP DEVELOPER: You must provide an Application-wide Resource, a string, with the key '{0}' in order to use the StaticMapQuestKeyName.", StaticMapQuestKeyName));
                }
            }

            if ((ZoomLevel < 1) || (ZoomLevel > 16))
            {
                throw new ArgumentOutOfRangeException("APP DEVELOPER: ZoomLevel can only be between 1 and 16 for MapQuest provider.");
            }
        }
    }
}
