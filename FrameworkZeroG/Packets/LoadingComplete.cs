using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteNetLib.Utils;

namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class LoadingComplete : GamePacket //: INetSerializable
    {
        public bool isComplete { get; set; }
        public string userName { get; set; }
        public void Generate(bool isFinished, string playerName)
        {
            isComplete = isFinished;
            userName = playerName;
        }
        //   public void Serialize(NetDataWriter writer)
        //   {
        //       writer.Put(isComplete);
        //       writer.Put(userName);
        //   }
        //   public void Deserialize(NetDataReader reader)
        //  {
        //      isComplete = reader.GetBool();
        //      userName = reader.GetString();
        //   }
    }
}
