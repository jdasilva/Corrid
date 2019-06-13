﻿#region Apache License Notice

// Copyright © 2019, Silverlake Software LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;

namespace Corrid
{
    public static class CorridPolicies
    {
        static Func<CorridContext> _corridContextFactory;

        static CorridPolicies()
        {
            _corridContextFactory = DefaultCorridContextFactory;
        }

        public static CorridContext DefaultCorridContextFactory() => new DefaultCorridContext();

        public static void SetCorridContextFactory(Func<CorridContext> value) => _corridContextFactory = value;

        public static CorridContext CreateCorridContext() => _corridContextFactory();
    }
}