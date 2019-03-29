using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG;
using FrameworkZeroG.Packets;
using ZeroG.Logger;

namespace ZeroG.MultiplayerClient.PacketProcessor
{
    public class ProcessPacket
    {
        public void Process(ZeroGPacket packet)
        {
            GamePacket origPacket = PacketGenerator.Decompile(packet);
            if (packet.PacketType == "LevelInfo" && (origPacket.GetType() == typeof(GameLevelInfo)))
            {
                GameLevelInfo newPacket = (GameLevelInfo)origPacket;
                LevelInfoProcessor.Process(newPacket);
            }
            else if (packet.PacketType == "PlayerNames" && (origPacket.GetType() == typeof(PlayerNames)))
            {
                PlayerNames newpacket = (PlayerNames)origPacket;
                PlayerNamesProcessor.Process(newpacket);
            }
            else if (packet.PacketType == "PlayerPosition" && (origPacket.GetType() == typeof(PlayerPosition)))
            {
                PlayerPosition newpacket = (PlayerPosition)origPacket;
                PlayerPositionProcessor.Process(newpacket);
            }
            else if (packet.PacketType == "AllLoaded" && (origPacket.GetType() == typeof(AllPlayersLoaded)))
            {
                AllPlayersLoaded newpacket = (AllPlayersLoaded)origPacket;
                AllPlayersLoadedProcessor.Process(newpacket);
            }
        }
    }
}
