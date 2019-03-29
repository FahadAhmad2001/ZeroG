using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using ZeroGServer.PlayerManagement;

namespace ZeroGServer.PacketProcessor
{
    public class LoadingCompleteProcessor
    {
        public static void Process(LoadingComplete packet)
        {
            PlayerManager manager = InstanceKeeper.playerManager;
            manager.PlayerLoadingFinished(packet.userName);
            InstanceKeeper.playerManager = manager;
        }
    }
}
