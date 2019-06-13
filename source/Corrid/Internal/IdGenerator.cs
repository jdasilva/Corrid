#region Apache License Notice

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

// This file contains a modified version of content that is:
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// from https://github.com/aspnet/KestrelHttpServer/blob/6fde01a825cffc09998d3f8a49464f7fbe40f9c4/src/Kestrel.Core/Internal/Infrastructure/CorrelationIdGenerator.cs

using System;
using System.Threading;

namespace Corrid.Internal
{
    public static class IdGenerator
    {
        // Base64 encoding - in ascii sort order for easy text based sorting
        private static readonly string _encode64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#";

        // Seed the _lastConnectionId for this application instance with
        // the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001
        // for a roughly increasing _lastId over restarts
        private static long _lastId = DateTime.UtcNow.Ticks;

        public static string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));

        private static unsafe string GenerateId(long id)
        {
            // The following routine is ~630% faster than calling long.ToString() on x64
            // and ~600% faster than calling long.ToString() on x86 in tight loops of 1 million+ iterations
            // See: https://github.com/aspnet/Hosting/pull/385

            // stackalloc to allocate array on stack rather than heap
            char* charBuffer = stackalloc char[11];

            charBuffer[0] = _encode64Chars[(int)(id >> 60) & 63];
            charBuffer[1] = _encode64Chars[(int)(id >> 54) & 63];
            charBuffer[2] = _encode64Chars[(int)(id >> 48) & 63];
            charBuffer[3] = _encode64Chars[(int)(id >> 42) & 63];
            charBuffer[4] = _encode64Chars[(int)(id >> 36) & 63];
            charBuffer[5] = _encode64Chars[(int)(id >> 30) & 63];
            charBuffer[6] = _encode64Chars[(int)(id >> 24) & 63];
            charBuffer[7] = _encode64Chars[(int)(id >> 18) & 63];
            charBuffer[8] = _encode64Chars[(int)(id >> 12) & 63];
            charBuffer[9] = _encode64Chars[(int)(id >> 6) & 63];
            charBuffer[10] = _encode64Chars[(int)id & 63];

            // string ctor overload that takes char*
            return new string(charBuffer, 0, 11);
        }
    }
}