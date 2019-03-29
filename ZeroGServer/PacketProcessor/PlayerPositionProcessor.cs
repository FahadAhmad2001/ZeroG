using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using LiteNetLib;

namespace ZeroGServer.PacketProcessor
{
    public class PlayerPositionProcessor
    {
        public static void Process(ZeroGPacket packet, NetPeer sender)
        {
            Server serverInst = InstanceKeeper.GetServerInstance();
            serverInst.SendToOthers(packet, DeliveryMethod.Unreliable, sender);
        }
    }
}
