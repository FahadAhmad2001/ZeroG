using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Timers;
using FrameworkZeroG.Packets;
using LiteNetLib;

namespace ZeroGServer.PlayerManagement
{
    public class PlayerManager
    {
        private Server serverInst;
        private PlayerEvents eventSender;
        private Timer countDownTimer;
        private Players players;
        private string levelname;
        private bool isReverse;
        private List<Player> loadingPlayers = new List<Player>();
        public PlayerManager(Server server, Players players, string levelName, bool isLevelReverse, int maxPlayers)
        {
            Console.WriteLine("level name is " + levelName);
            levelname = levelName;
            isReverse = isLevelReverse;
            this.players = players;
            serverInst = server;
            this.players.MaxPlayers = maxPlayers;
            eventSender = new PlayerEvents();
            eventSender.RequiredPlayersReached += OnMaxPlayersReached;
            eventSender.FirstPlayerConnected += FirstPlayerConnected;
            eventSender.NoPlayersConnected += NoPlayersConnected;
            eventSender.AllPlayersLoaded += EventSender_AllPlayersLoaded;
            countDownTimer = new Timer();
            countDownTimer.Enabled = false;
            countDownTimer.AutoReset = false;
            countDownTimer.Interval = 60000;
            countDownTimer.Elapsed += CountDownTimer_Elapsed;
            this.players.Start();
            this.players.eventSender = eventSender;
        }

        private void EventSender_AllPlayersLoaded(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            AllPlayersLoaded packet = new AllPlayersLoaded();
            packet.Generate(true);
            ZeroGPacket fullPacket = PacketGenerator.Generate("AllLoaded", packet);
            serverInst.SendToAll(fullPacket, DeliveryMethod.ReliableOrdered);
        }

        public void PlayerLoadingFinished(string playerName)
        {
            players.PlayerLoaded(playerName);
        }
        private void CountDownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //  throw new NotImplementedException();
            Console.WriteLine("60 sec elapsed, starting game");
            GameLevelInfo packet = new GameLevelInfo();
            packet.Generate(levelname, players.PlayersCount() - 1, 3, false);
            ZeroGPacket fullPacket = PacketGenerator.Generate("LevelInfo", packet);
            serverInst.SendToAll(fullPacket, DeliveryMethod.ReliableOrdered);
            countDownTimer.Stop();
        }

        private void FirstPlayerConnected(object sender, EventArgs e)
        {
            Console.WriteLine("First player connected");
            countDownTimer.Start();
        }
        private void OnMaxPlayersReached(object sender, EventArgs e)
        {
            Console.WriteLine("max players reached");
            PlayerNames packet2 = new PlayerNames();
            List<string> playerName = new List<string>();
            foreach (Player player in players.PlayersList)
            {
                playerName.Add(player.PlayerName);
            }
            packet2.Generate(playerName);
            ZeroGPacket mainPacket2 = PacketGenerator.Generate("PlayerNames", packet2);
            serverInst.SendToAll(mainPacket2, DeliveryMethod.ReliableOrdered);
            GameLevelInfo packet = new GameLevelInfo();
            packet.Generate(levelname, players.PlayersCount() - 1, 3, isReverse);
            ZeroGPacket fullPacket = PacketGenerator.Generate("LevelInfo", packet);
            serverInst.SendToAll(fullPacket, DeliveryMethod.ReliableOrdered);
            countDownTimer.Stop();
        }
        private void NoPlayersConnected(object sender, EventArgs e)
        {
            Console.WriteLine("No players connected");
            countDownTimer.Stop();
        }
    }
}
