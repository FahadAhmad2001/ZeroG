using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using ZeroG.Logger;

namespace ZeroG.MultiplayerClient.PacketProcessor
{
    public class LevelInfoProcessor
    {
        public static void Process(GameLevelInfo packet)
        {
            WriteLog.Verbose("Attempting to load game level " + packet.LevelName + " with players " + packet.PlayersLaps[0] + " and laps " + packet.PlayersLaps[1] + " and isReverse " + packet.isReverse);
            Main.opponents = packet.PlayersLaps[0];
            Main.LoadGame(packet.LevelName, packet.PlayersLaps[1], packet.PlayersLaps[0], packet.isReverse);
        }
    }
}
