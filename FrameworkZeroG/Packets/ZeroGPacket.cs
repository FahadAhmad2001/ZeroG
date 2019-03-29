using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkZeroG.Packets
{
    public class ZeroGPacket
    {
        public byte[] InnerData { get; set; }
        public string PacketType { get; set; }
    }
}
