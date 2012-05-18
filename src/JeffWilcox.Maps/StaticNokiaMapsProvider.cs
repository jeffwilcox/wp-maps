using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JeffWilcox.Controls
{
    public class StaticNokiaMapsProvider : StaticMapProvider
    {

        private const string StaticNokiaMapsKeyName = "NokiaMapsKey";

        private string _key;

        private const string StaticMapsUrlFormat =
    "http://m.nok.it/?" +
    "t={0}/" + // map type
    "&c={1},{2}" + // lat,long
    "&z={3}" + // zoomLevel // int from 1 to 22, 15 seems normal-ish
    "&w={4}&h={5}" + // width, height
  //  PushPinFormat + // lat,long of pushpin
    "&app_id={6}&nord"; // dev key

        public override Uri GetStaticMap()
        {
            RequireCenter();

     //       var format = StaticMapsUrlFormat.Replace(PushPinFormat, string.Empty);
            var uri = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                StaticMapsUrlFormat,
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
