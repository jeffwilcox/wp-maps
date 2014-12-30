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
    public class StaticMapStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new status of the static map.
        /// </summary>
        public StaticMapStatus Status { get; private set; }

        /// <summary>
        /// Gets the exception that is associated to a <code>Failed</code> status.
        /// </summary>
        /// <value><code>null</code> if <paramref name="Status"/> is not <code>Failed</code>, an exception
        /// object otherwise.</value>
        public Exception ErrorException { get; private set; }

        public StaticMapStatusChangedEventArgs(StaticMapStatus newStatus, Exception exception = null)
        {
            Status = newStatus;
            ErrorException = newStatus == StaticMapStatus.Failed ? exception : null;
        }
        
    }
}
