using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkZeroG.Packets
{
    [Serializable]
    public class PlayerNames : GamePacket
    {
        public List<string> PlayerNamesList { get; set; }

        public void Generate(List<string> playerNames)
        {
            PlayerNamesList = new List<string>();
            PlayerNamesList = playerNames;
        }
    }
}
