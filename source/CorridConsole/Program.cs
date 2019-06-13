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

using System;
using System.Linq;
using Corrid;
using Corrid.AspNetCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace CorridConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleRun();

//            OutputRangeChart();

//            ShowDateExample();

//            for (int i = 0; i < 100; ++i)
//            {
//                Console.WriteLine(IdGenerator.GetNextId());
//            }
        }

        static void ShowDateExample()
        {
            var baseDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var baseTicks = baseDateTime.Ticks;
            Console.WriteLine($"Base Ticks = {baseTicks}");
            Console.WriteLine($"Base Ticks = {baseTicks:X}");

            var currentTicks = DateTime.UtcNow.Ticks;
            Console.WriteLine($"Current Ticks = {currentTicks}");
            Console.WriteLine($"Current Ticks = {currentTicks:X}");
            Console.WriteLine($"Max Long     = {long.MaxValue:X}");
            var maxTicks = DateTime.MaxValue.Ticks;
            var maxDateTime = new DateTime(maxTicks);
            Console.WriteLine(maxDateTime);
            Console.WriteLine($"Max Ticks = {maxTicks}");
            Console.WriteLine($"Max Ticks = {maxTicks:X}");
            Console.WriteLine($"Max Long  = {long.MaxValue:X}");

            var tryMaxTicks = (1L << 60) - 1; // 0xFFF_FFFF_FFFF_FFFFL;
            Console.WriteLine($"{tryMaxTicks:X}");
            var tryMaxDateTime = new DateTime(tryMaxTicks + baseTicks);
            Console.WriteLine(tryMaxDateTime);

            var timeSpan = tryMaxDateTime - baseDateTime;

            var years = timeSpan.Days / 365;
            var days = timeSpan.Days - (years * 365);
            Console.WriteLine($"{years} years and {days} days of uniqueness @ 10million per second.");
            Console.WriteLine(timeSpan);
        }

        static void OutputRangeChart()
        {
            Console.WriteLine("Unique IDs can be represented @ 10 million per second.");
            Console.WriteLine("Digits|Years|Days|Hours");
            for (int i = 1; i < 11; ++i)
                OutputRangeRow(i);
        }

        static void OutputRangeRow(int digits)
        {
            int bits = Math.Min(digits * 6, 64);
            var tickRange = (1L << bits) - 1;
            var timeSpan = new TimeSpan(tickRange);
            var years = timeSpan.Days / 365;
            var days = timeSpan.Days - (years * 365);
            var hours = timeSpan.Hours;
            Console.WriteLine($"{digits,6}|{years,5}|{days,4}|{hours,5}");
        }

        static void SimpleRun()
        {
            var options = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
            var optionsFactory =
                new OptionsFactory<ConsoleLoggerOptions>(new[] {options}, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory,
                Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());
            using (var consoleLoggerProvider = new ConsoleLoggerProvider(optionsMonitor))
            using (var loggerFactory = new LoggerFactory(new[] {consoleLoggerProvider}, new LoggerFilterOptions {MinLevel = LogLevel.Information}))
            {
                var logger = loggerFactory.CreateLogger("Test");

                var updater = CorridContext.Default as ICorridContextUpdater;
                (CorridContext.Default as DefaultCorridContext)?.AddEventHandler(new CorridContextUpdaterLogger(logger));
                updater.BeginExecutionScope("X1234");

                updater.EndExecutionScope();
            }
        }
    }
}