using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;

namespace ZeroG.MultiplayerClient.PacketProcessor
{
    public class AllPlayersLoadedProcessor
    {
        public static void Process(AllPlayersLoaded packet)
        {
            if (packet.AllLoaded)
            {
                Main.AllPlayersLoaded = true;
            }
        }
    }
}
