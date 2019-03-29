using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FrameworkZeroG.CustomUnitySerializable;

namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class PlayerPosition : GamePacket
    {
        public string PlayerName { get; set; }
        public SerializableVector3 Position { get; set; }
        public SerializableQuaternion Rotation { get; set; }

        public void Generate(string playerName, SerializableVector3 position, SerializableQuaternion rotation)
        {
            PlayerName = playerName;
            Position = position;
            Rotation = rotation;
        }
    }
}
