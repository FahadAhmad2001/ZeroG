using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace FrameworkZeroG.Packets
{
    public enum GamePacketType
    {
        GameLevelInfo = 0
    }
    [Serializable]
    public abstract class GamePacket
    {
        //  public abstract void Generate(object[] args);
        // public GameLevelInfo gLevelInfoPacket { get; set; }
        // public LoadingComplete loadingFinish { get; set; }
        // public string packetType { get; set; }
        //  public void Generate(string packetName,GameLevelInfo gLvlInfo = null, LoadingComplete loadingFinished = null)
        //  {
        //      packetType = packetName;
        //      gLevelInfoPacket = gLvlInfo;
        //      loadingFinish = loadingFinished;
        //  }
    }

}
