using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;

namespace BusyBeekeeper
{
    internal sealed class DebugConsoleTickDumper
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public void Dump(BeeWorldManager worldManager)
        {
            var lPlayerManager = worldManager.PlayerManager;
            var lPlayer = lPlayerManager.Player;

            Console.WriteLine(string.Concat("Time: ", worldManager.Time, " (", worldManager.ElapsedTime, ")"));
        }
    }
}
