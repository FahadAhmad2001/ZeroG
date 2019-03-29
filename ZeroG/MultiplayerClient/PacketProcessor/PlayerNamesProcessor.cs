using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;

namespace ZeroG.MultiplayerClient.PacketProcessor
{
    public class PlayerNamesProcessor
    {
        public static void Process(PlayerNames packet)
        {
            Main clientInst = InstanceKeeper.GetMainClient();
            clientInst.opponentsNames = packet.PlayerNamesList;
            InstanceKeeper.SetMainClient(clientInst);
        }
    }
}
