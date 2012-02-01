//
// Copyright (c) 2010-2011 Jeff Wilcox
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
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using JeffWilcox.Controls;
using System.Windows.Controls;

namespace StaticMapSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Message(string text)
        {
            MessageBox.Show(text, "Map problem", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var map = button.Content as StaticMap;
                if (map != null)
                {
                    map.NavigateToMapsApplication();
                }
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ApplicationBarMenuItem;
            if (item != null)
            {
                switch (item.Text)
                {
                        // this isn't a perfect example, I'm sending Bing to Bing only.
                    case "open in maps app":
                        if (!_bingMap.NavigateToMapsApplication())
                        {
                            Message("Unable to open in the maps app right now.");
                        }
                        break;

                        // Depending on how you read the Google Maps terms and
                        // conditions, this is required - offering to open a
                        // Google Map in the phone's web browser if there is no
                        // Google Maps app built into the phone that can be 
                        // launched instead.
                    case "open in browser":
                        if (!_googleMap.NavigateBrowserToMap())
                        {
                            Message("Unable to figure out where to go on the web right now.");
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
