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

using Microsoft.Extensions.Logging;

namespace Corrid.AspNetCore
{
    public class CorridContextUpdaterLogger : ICorridContextEventHandler
    {
        readonly ILogger _logger;
        readonly LogLevel _logLevel;

        public CorridContextUpdaterLogger(ILogger logger, LogLevel logLevel = LogLevel.Information)
        {
            _logger = logger;
            _logLevel = logLevel;
        }

        public void OnBeginExecutionScope(string id)
        {
            if (!_logger.IsEnabled(_logLevel))
                return;
            _logger.Log(_logLevel, new EventId(), id, null, (id, _) => $"Begin Execution Scope Id={id}");
        }

        public void OnBeginExecutionScope(string externalId, string id)
        {
            if (!_logger.IsEnabled(_logLevel))
                return;
            _logger.Log(_logLevel, new EventId(), id, null, (id, _) => $"Begin Execution Scope ExternalId={externalId}, Id={id}");
        }

        public void OnEndExecutionScope(string id)
        {
            _logger.Log(_logLevel, new EventId(), id, null, (id, _) => $"End Execution Scope ID={id}");
        }
    }
}