using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class ClientInfo : GamePacket
    {
        public string Name { get; set; }

        public void Generate(string name)
        {
            Name = name;
        }
    }
}
