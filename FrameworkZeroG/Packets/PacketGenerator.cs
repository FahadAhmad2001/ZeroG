using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FrameworkZeroG.Packets
{
    public class PacketGenerator
    {
        public static ZeroGPacket Generate(string PacketType, GamePacket packet)
        {
            byte[] byteArray;
            if (packet == null || PacketType == null)
            {
                return null;
            }
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, packet);
                byteArray = ms.ToArray();
            }
            ZeroGPacket newPacket = new ZeroGPacket();
            newPacket.PacketType = PacketType;
            newPacket.InnerData = byteArray;
            return newPacket;
        }
        public static GamePacket Decompile(ZeroGPacket packet)
        {
            if (packet == null)
            {
                return null;
            }
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(packet.InnerData, 0, packet.InnerData.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                GamePacket newPacket = (GamePacket)binForm.Deserialize(memStream);
                return newPacket;
            }
        }
    }
}
