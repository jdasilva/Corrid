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

// Created by Jamie da Silva on 6/12/2019 9:39 PM

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Corrid.HttpClient
{
    public class CorridHttpClientHandler : DelegatingHandler
    {
        const string XCorrelationIdHeader = "X-Correlation-ID";

        readonly ICorridContext _context;

        public CorridHttpClientHandler() : this(CorridContext.Default) { }

        public CorridHttpClientHandler(ICorridContext context)
        {
            _context = context;
        }

        public CorridHttpClientHandler(HttpMessageHandler innerHandler) : this(innerHandler, CorridContext.Default) { }

        public CorridHttpClientHandler(HttpMessageHandler innerHandler, ICorridContext context) : base(innerHandler)
        {
            _context = context;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(XCorrelationIdHeader, _context.Id);
            return base.SendAsync(request, cancellationToken);
        }
    }
}