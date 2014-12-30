//
// Copyright (c) 2012 Jeff Wilcox
// Parts of this class by Brice Clocher
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffWilcox.Controls
{
    /// <summary>
    /// Represents the status of download of the static map.
    /// </summary>
    public enum StaticMapStatus
    {
        /// <summary>
        /// The map is not initialized yet, and no download has been started.
        /// </summary>
        None,

        /// <summary>
        /// The map image is being downloaded.
        /// </summary>
        Downloading,

        /// <summary>
        /// The map image failed to be downloaded.
        /// </summary>
        Failed,

        /// <summary>
        /// The map image is downloaded and on-screen.
        /// </summary>
        Done
    }
}
