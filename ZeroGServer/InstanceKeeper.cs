using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroGServer.PacketProcessor;
using ZeroGServer.PlayerManagement;

namespace ZeroGServer
{
    public class InstanceKeeper
    {
        public static Players players { get; set; }
        public static PlayerManager playerManager { get; set; }
        public static Server serverInst { get; set; }
        public static void SetServerinstance(Server servInst)
        {
            serverInst = servInst;
        }
        public static Server GetServerInstance()
        {
            return serverInst;
        }
    }
}
