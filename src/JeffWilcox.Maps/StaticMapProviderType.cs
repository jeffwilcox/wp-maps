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

// Google Maps is a trademark of Google, Inc.
// Bing Maps is a trademark of Microsoft Corporation

using System.ComponentModel;
namespace JeffWilcox.Controls
{
    /// <summary>
    /// Represents an Internet static maps provider.
    /// </summary>
    public enum StaticMapProviderType
    {
        /// <summary>
        /// Bing Maps.
        /// </summary>
        [Description("Bing Maps")]
        Bing,

        /// <summary>
        /// Google Maps.
        /// </summary>
        [Description("Google Maps")]
        Google,
    }
}
