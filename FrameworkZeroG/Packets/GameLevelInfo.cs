using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteNetLib.Utils;

namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class GameLevelInfo : GamePacket
    {
        public string LevelName { get; set; }
        //  public int nPlayers { get; set; }
        //public float nLaps { get; set; }
        public int[] PlayersLaps { get; set; }
        public bool isReverse { get; set; }
        public void Generate(string lvlName, int players, int laps, bool isReverse)
        {
            LevelName = lvlName;
            //PlayersLaps = new int[2] { players, laps };
            //nPlayers = players;
            // nLaps = laps;
            PlayersLaps = new int[2] { players, laps };
            this.isReverse = isReverse;
        }
        //     public void Serialize(NetDataWriter writer)
        //     {
        //         writer.Put(LevelName);
        //         writer.PutArray(PlayersLaps);
        //writer.Put(nPlayers);
        //writer.Put(PlayersLaps);
        //  writer.Put(nLaps);
        //         writer.Put(isReverse);
        //     }
        //     public void Deserialize(NetDataReader reader)
        //     {
        // File.AppendAllText("leveldata.txt", reader.GetString() + reader.GetIntArray()[0].ToString() + reader.GetIntArray()[1].ToString() + reader.GetBool());
        //         LevelName = reader.GetString();
        //         PlayersLaps = reader.GetIntArray();
        //  nLaps = reader.GetFloat();
        //  nPlayers = reader.GetInt();
        //nPlayers = reader.PeekInt();
        //        isReverse = reader.GetBool();
        //   }
    }
}
