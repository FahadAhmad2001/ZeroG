using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using LiteNetLib;

namespace ZeroGServer.PacketProcessor
{
    public class ProcessPacket
    {
        public void Process(ZeroGPacket packet, NetPeer peer)
        {
            GamePacket origPacket = PacketGenerator.Decompile(packet);
            if (packet.PacketType == "LoadingComplete" && (origPacket.GetType() == typeof(LoadingComplete)))
            {
                LoadingComplete newPacket = (LoadingComplete)origPacket;
                // Console.WriteLine("Player " + newPacket.userName + " has set loading complete status to " + newPacket.isComplete);
                LoadingCompleteProcessor.Process(newPacket);
            }
            else if (packet.PacketType == "ClientInfo" && (origPacket.GetType() == typeof(ClientInfo)))
            {
                ClientInfo newPacket = (ClientInfo)origPacket;
                ClientInfoProcessor.Process(newPacket, peer.Id);
            }
            else if (packet.PacketType == "PlayerPosition" && (origPacket.GetType() == typeof(PlayerPosition)))
            {
                PlayerPositionProcessor.Process(packet, peer);
            }
        }
    }
}
