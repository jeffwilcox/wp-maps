//
// Copyright (c) 2010-2012 Jeff Wilcox
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
using System.Device.Location;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// TODO: Consider exposing IsSecure true/false to use HTTPS instead.
// CONSIDER: Should the map provider key be a property, or is it OK in the hidden App.xaml resource method that I'm going with here?

namespace JeffWilcox.Controls
{
    /// <summary>
    /// A simple static maps control for Windows Phone.
    /// </summary>
    [TemplatePart(Name = ImagePartName, Type = typeof(Image))]
    public class StaticMap : Control
    {
        private const string ImagePartName = "_image";

        private double _width;
        private double _height;
        private StaticMapProvider _mapProvider;
        private Image _image;

        #region public GeoCoordinate MapCenter
        /// <summary>
        /// Gets or sets the map center.
        /// </summary>
        public GeoCoordinate MapCenter
        {
            get { return GetValue(MapCenterProperty) as GeoCoordinate; }
            set { SetValue(MapCenterProperty, value); }
        }

        /// <summary>
        /// Identifies the MapCenter dependency property.
        /// </summary>
        public static readonly DependencyProperty MapCenterProperty =
            DependencyProperty.Register(
                "MapCenter",
                typeof(GeoCoordinate),
                typeof(StaticMap),
                new PropertyMetadata(null, OnMapCenterPropertyChanged));

        /// <summary>
        /// MapCenterProperty property changed handler.
        /// </summary>
        /// <param name="d">StaticMap that changed its MapCenter.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnMapCenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaticMap source = d as StaticMap;
            source.UpdateMap();
        }
        #endregion public GeoCoordinate MapCenter

        #region public int ZoomLevel
        /// <summary>
        /// Gets or sets the zoom level, a value from 1 to 22, where 1 will
        /// show the entire world, and 22 will show at the street level
        /// approximately. Default value is 15.
        /// </summary>
        public int ZoomLevel
        {
            get { return (int)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        /// <summary>
        /// Identifies the ZoomLevel dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register(
                "ZoomLevel",
                typeof(int),
                typeof(StaticMap),
                new PropertyMetadata(15, OnZoomLevelPropertyChanged));

        /// <summary>
        /// ZoomLevelProperty property changed handler.
        /// </summary>
        /// <param name="d">StaticMap that changed its ZoomLevel.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnZoomLevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaticMap source = d as StaticMap;
            source.UpdateMap();
        }
        #endregion public int ZoomLevel

        #region public StaticMapProviderType Provider
        /// <summary>
        /// Gets or sets the type of map provider to use for rendering static
        /// maps.
        /// </summary>
        public StaticMapProviderType Provider
        {
            get { return (StaticMapProviderType)GetValue(ProviderProperty); }
            set { SetValue(ProviderProperty, value); }
        }

        /// <summary>
        /// Identifies the Provider dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.Register(
                "Provider",
                typeof(StaticMapProviderType),
                typeof(StaticMap),
                new PropertyMetadata(StaticMapProviderType.Bing, OnProviderPropertyChanged));

        /// <summary>
        /// ProviderProperty property changed handler.
        /// </summary>
        /// <param name="d">StaticMap that changed its Provider.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnProviderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaticMap source = d as StaticMap;
            source.UpdateProvider();
            source.UpdateMap();
        }
        #endregion public StaticMapProviderType Provider

        #region public StaticMapMode MapMode
        /// <summary>
        /// Gets or sets the visual map mode for the map image.
        /// </summary>
        public StaticMapMode MapMode
        {
            get { return (StaticMapMode)GetValue(MapModeProperty); }
            set { SetValue(MapModeProperty, value); }
        }

        /// <summary>
        /// Identifies the MapMode DependencyProperty
        /// </summary>
        public static readonly DependencyProperty MapModeProperty =
            DependencyProperty.Register("MapMode", typeof(StaticMapMode), typeof(StaticMap), new PropertyMetadata(StaticMapMode.Map, OnMapModePropertyChanged));

        /// <summary>
        /// MapModeProperty property changed handler
        /// </summary>
        /// <param name="d">StaticMap that changed its Provider.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnMapModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaticMap source = d as StaticMap;
            source.UpdateProvider();
            source.UpdateMap();
        }

        #endregion public StaticMapMode MapMode

        #region public Uri ActualImageSource
        /// <summary>
        /// Sets the actual image source. Only exposes the getter to
        /// try and reduce errors by end user developers who may want to set
        /// the computed value.
        /// </summary>
        public Uri ActualImageSource
        {
            get { return GetValue(ActualImageSourceProperty) as Uri; }
            private set { SetValue(ActualImageSourceProperty, value); }
        }

        /// <summary>
        /// Identifies the ActualImageSource dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualImageSourceProperty =
            DependencyProperty.Register(
                "ActualImageSource",
                typeof(Uri),
                typeof(StaticMap),
                new PropertyMetadata(null));
        #endregion public Uri ActualImageSource

        #region public Visibility MapCenterVisibility
        /// <summary>
        /// Gets or sets a value indicating whether the center point on the 
        /// map is visible or not.
        /// </summary>
        public Visibility MapCenterVisibility
        {
            get { return (Visibility)GetValue(MapCenterVisibilityProperty); }
            set { SetValue(MapCenterVisibilityProperty, value); }
        }

        /// <summary>
        /// Identifies the MapCenterVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty MapCenterVisibilityProperty =
            DependencyProperty.Register(
                "MapCenterVisibility",
                typeof(Visibility),
                typeof(StaticMap),
                new PropertyMetadata(Visibility.Visible));
        #endregion public Visibility MapCenterVisibility

        #region public bool IsSensorCoordinate
        /// <summary>
        /// Gets or sets a value indicating whether a sensor's coordinate is
        /// being used for the MapCenter point of the StaticMap. This may need
        /// to be reported to the underlying static map API.
        /// </summary>
        public bool IsSensorCoordinate
        {
            get { return (bool)GetValue(IsSensorCoordinateProperty); }
            set { SetValue(IsSensorCoordinateProperty, value); }
        }

        /// <summary>
        /// Identifies the IsSensorCoordinate dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSensorCoordinateProperty =
            DependencyProperty.Register(
                "IsSensorCoordinate",
                typeof(bool),
                typeof(StaticMap),
                new PropertyMetadata(true, OnIsSensorCoordinatePropertyChanged));

        /// <summary>
        /// IsSensorCoordinateProperty property changed handler.
        /// </summary>
        /// <param name="d">StaticMap that changed its IsSensorCoordinate.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsSensorCoordinatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaticMap source = d as StaticMap;
            if (source._mapProvider != null)
            {
                source._mapProvider.IsSensor = (bool)e.NewValue;
            }
        }
        #endregion public bool IsSensorCoordinate

        public StaticMap()
        {
            DefaultStyleKey = typeof(StaticMap);
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height > 0.0 && e.NewSize.Width > 0.0)
            {
                _width = e.NewSize.Width;
                _height = e.NewSize.Height;
                UpdateMap();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetTemplateChild(ImagePartName) as Image;

            UpdateMap();
        }

        private void UpdateProvider()
        {
            switch (Provider)
            {
                case StaticMapProviderType.Bing:
                    _mapProvider = new StaticBingMapsProvider();
                    break;

                case StaticMapProviderType.Google:
                    _mapProvider = new StaticGoogleMapsProvider();
                    break;

                case StaticMapProviderType.MapQuest:
                    _mapProvider = new StaticMapQuestProvider();
                    break;

                case StaticMapProviderType.OpenStreetMap:
                    _mapProvider = new StaticOpenStreetMapProvider();
                    break;

                default:
                    throw new InvalidOperationException("The provider type requested is not supported.");
            }
        }

        public bool NavigateBrowserToMap()
        {
            if (_mapProvider != null)
            {
                return _mapProvider.NavigateBrowserToMap();
            }

            return false;
        }

        public bool NavigateToMapsApplication()
        {
            if (_mapProvider != null)
            {
                return _mapProvider.NavigateToMapsApplication();
            }

            return false;
        }

        private void UpdateMap()
        {
            if (_mapProvider == null)
            {
                UpdateProvider();
            }

            if (_height > 1 && _width > 1 && _mapProvider != null)
            {
                int width = (int)Math.Ceiling(_width);
                int height = (int)Math.Ceiling(_height);

                _mapProvider.SetSize(width, height);
                _mapProvider.ZoomLevel = ZoomLevel;
                _mapProvider.IsSensor = IsSensorCoordinate;
                _mapProvider.Center = MapCenter;
                _mapProvider.MapMode = MapMode;
                _mapProvider.Validate();

                ActualImageSource = _mapProvider.GetStaticMap();
                _image.Source = new BitmapImage(ActualImageSource);
            }
        }
    }
}
