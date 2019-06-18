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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Corrid.AspNetCore.Tests
{
    [TestFixture]
    public class CorridMiddlewareTests
    {
        [Test]
        public async Task MiddlewareUsesIdFromHeader()
        {
            var contextUpdater = Substitute.For<ICorridContextUpdater>();

            var context = new DefaultHttpContext();
            var fakeId = "ThisIsAnId";
            context.Request.Headers.Append(CorridConstants.XCorrelationIdHeader, fakeId);

            var sut = new CorridMiddleware(_ =>
            {
                contextUpdater.Received().BeginExecutionScope(Arg.Is(fakeId));
                contextUpdater.DidNotReceive().EndExecutionScope();
                return Task.CompletedTask;
            }, contextUpdater);

            await sut.Invoke(context);

            contextUpdater.Received().EndExecutionScope();
        }

        [Test]
        public async Task MiddlewareStartsNewId()
        {
            var contextUpdater = Substitute.For<ICorridContextUpdater>();

            var context = new DefaultHttpContext();

            var sut = new CorridMiddleware(_ =>
            {
                contextUpdater.Received().BeginExecutionScope();
                contextUpdater.DidNotReceive().EndExecutionScope();
                return Task.CompletedTask;
            }, contextUpdater);

            await sut.Invoke(context);

            contextUpdater.Received().EndExecutionScope();
        }
    }
}