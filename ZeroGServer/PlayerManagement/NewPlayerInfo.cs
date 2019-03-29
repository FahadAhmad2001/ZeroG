using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG;
using FrameworkZeroG.Packets;

namespace ZeroGServer.PlayerManagement
{
    public class NewPlayerInfo
    {
        // int count = 0;
        public ZeroGPacket SendNewPlayerInfo()
        {
            try
            {
                GameLevelInfo levelnfo = new GameLevelInfo();
                levelnfo.Generate("Urban_02", 4, 2, false);
                //  GamePacket packet = new GamePacket();
                //packet.packetType = "LevelInfo";
                //   LoadingComplete packetSecond = new LoadingComplete();
                //   packetSecond.Generate(true, "yo");
                //   if(count == 2)
                //  {
                //      packet.Generate("LevelInfo", levelnfo, packetSecond);
                //  }
                //   else
                //  {
                //      packet.Generate("LevelInfo", levelnfo,packetSecond);
                //  }
                //  count = count + 1;
                ZeroGPacket packet = PacketGenerator.Generate("LevelInfo", levelnfo);
                return packet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                return null;
            }
        }

        //    public GamePacket SendSecondPlayerInfo()
        //    {
        //        GameLevelInfo levelnfo = new GameLevelInfo();
        //        levelnfo.Generate("Urban_02",4,2, false);
        //        GamePacket packet = new GamePacket();
        //packet.packetType = "LevelInfo";
        //        LoadingComplete packetSecond = new LoadingComplete();
        //packetSecond.Generate(true, "yo");
        //   if(count == 2)
        //  {
        //      packet.Generate("LevelInfo", levelnfo, packetSecond);
        //  }
        //   else
        //  {
        //        packet.Generate("LevelInfo", levelnfo);
        //  }
        //  count = count + 1;
        //        return packet;
        //    }
    }
}
