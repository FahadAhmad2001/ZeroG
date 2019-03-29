using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroGServer.PlayerManagement;
using ZeroGServer.PacketProcessor;

namespace ZeroGServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----ZeroG Server----");
            Console.WriteLine("Developed by DarkShadow");
            Console.WriteLine("Starting server...");
            Server server = new Server();
            NewPlayerInfo npi = new NewPlayerInfo();
            Players players = new Players();
            InstanceKeeper.players = players;
            PlayerManager pManager = new PlayerManager(server, players, "Urban_02", true, 2);
            InstanceKeeper.playerManager = pManager;
            server.Start(npi, 6001);
            Console.ReadLine();
            //Currently, all the server code is dumped in one place, ill fix it when i get time
        }
    }
}
