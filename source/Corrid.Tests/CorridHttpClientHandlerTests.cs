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

// Created by Jamie da Silva on 6/18/2019 1:12 AM

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Corrid.Http;
using NSubstitute;
using NUnit.Framework;

namespace Corrid.Tests
{
    [TestFixture]
    public class CorridHttpClientHandlerTests
    {
        [Test]
        public async Task SendsHeader()
        {
            const string fakeid = "FakeId";

            var context = Substitute.For<ICorridContext>();
            context.Id.Returns(fakeid);

            var handler = new DelegateMessageHandler(
                (request, ct) =>
                {
                    Assert.That(request.Headers.GetValues(CorridConstants.XCorrelationIdHeader).Single()==fakeid, Is.True);
                    return Task.FromResult(new HttpResponseMessage());
                });

            var sut = new CorridHttpClientHandler(handler, context);
            var client = new HttpClient(sut);
            await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://example.com"), CancellationToken.None);
        }

        public class DelegateMessageHandler : HttpMessageHandler
        {
            readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsyncFunc;

            public DelegateMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncFunc)
            {
                _sendAsyncFunc = sendAsyncFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
                _sendAsyncFunc(request, cancellationToken);
        }
    }
}