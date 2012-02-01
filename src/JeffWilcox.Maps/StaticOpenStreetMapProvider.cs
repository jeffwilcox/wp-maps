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
// http://wiki.openstreetmap.org/wiki/Static_Maps_API
//
// If you use the OpenStreetMap Static Maps API provider, please confirm that you are
// using the control appropriately and within the API guidelines for the
// service.
//

namespace JeffWilcox.Controls
{
    public class StaticOpenStreetMapProvider : StaticMapProvider
    {
        // http://pafciu17.dev.openstreetmap.org/?module=map&center=-122.142013549805,47.6450881958008&zoom=7&width=400&height=400
        private const string StaticMapsUrlFormat =
            "http://pafciu17.dev.openstreetmap.org/?module=map" +
            "&center={0},{1}" + // lon,lat
            "&zoom={2}" + // zoom
            "&width={3}" + // width
            "&height={4}"; // height


        public override Uri GetStaticMap()
        {
            RequireCenter();

            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                StaticMapsUrlFormat,
                Center.Longitude,
                Center.Latitude,
                BingMapsHelper.ClampZoomLevel(ZoomLevel),
                Width,
                Height
                ), UriKind.Absolute);

            return uri;
        }

        public override void Validate()
        {
            base.Validate();

            if (this.MapMode != StaticMapMode.Map)
            {
                throw new NotSupportedException("APP DEVELOPER: MapMode is not supported at this time with the OpenStreetMap provider.");
            }

            if (this.ZoomLevel > 18)
            {
                throw new ArgumentOutOfRangeException("APP DEVELOPER: ZoomLevel can only be between 0 and 18 for OpenStreetMap provider.");
            }
        }
    }
}