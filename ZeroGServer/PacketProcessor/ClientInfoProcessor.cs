using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using ZeroGServer.PlayerManagement;

namespace ZeroGServer.PacketProcessor
{
    public class ClientInfoProcessor
    {
        public static void Process(ClientInfo packet, int peerID)
        {
            Players players = InstanceKeeper.players;
            players.AddPlayer(packet.Name, peerID);
            InstanceKeeper.players = players;
        }
    }
}
