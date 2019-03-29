using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class AllPlayersLoaded : GamePacket
    {
        public bool AllLoaded { get; set; }
        public void Generate(bool allLoaded)
        {
            AllLoaded = allLoaded;
        }
    }
}
