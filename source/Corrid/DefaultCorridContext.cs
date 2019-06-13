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

using System.Collections.Generic;
using System.Threading;

namespace Corrid
{
    public class DefaultCorridContext : CorridContext
    {
        readonly List<ICorridContextEventHandler> _eventHandlers = new List<ICorridContextEventHandler>();
        readonly AsyncLocal<LocalCorridContext> _context = new AsyncLocal<LocalCorridContext>();

        public override void BeginExecutionScope()
        {
            var localContext = new LocalCorridContext();
            var id = IdGenerator.GetNextId();
            localContext.RootId = id;
            localContext.IncomingId = id;
            localContext.Id = id;
            _context.Value = localContext;
            _eventHandlers.ForEach(eh => eh.OnBeginExecutionScope(id));
        }

        public override void BeginExecutionScope(string incomingId)
        {
            var localContext = new LocalCorridContext();
            var id = IdGenerator.GetNextId();
            localContext.RootId = incomingId;
            localContext.IncomingId = incomingId;
            localContext.Id = id;
            _context.Value = localContext;
            _eventHandlers.ForEach(eh => eh.OnBeginExecutionScope(incomingId, id));
        }

        public override void EndExecutionScope()
        {
            _eventHandlers.ForEach(eh => eh.OnEndExecutionScope(_context.Value.Id));
            _context.Value = null;
        }

        public void AddEventHandler(ICorridContextEventHandler eventHandler)
        {
            _eventHandlers.Add(eventHandler);
        }
    }
}