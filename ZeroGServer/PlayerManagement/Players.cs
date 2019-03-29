using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroGServer.PlayerManagement
{
    public struct Player
    {
        public string PlayerName { get; set; }
        public int ID { get; set; }
    }
    public class Players
    {
        public List<Player> PlayersList;
        public List<Player> LoadingPlayers;
        public int MaxPlayers = 1;
        public PlayerEvents eventSender;

        public int PlayersCount()
        {
            return PlayersList.Count;
        }
        public void PlayerLoaded(string playerName)
        {
            Player playerToRemove = new Player();
            foreach (Player player in LoadingPlayers)
            {
                if (player.PlayerName == playerName)
                {
                    playerToRemove = player;
                }
            }
            LoadingPlayers.Remove(playerToRemove);
            if (LoadingPlayers.Count == 0)
            {
                eventSender.SendAllLoaded();
            }
        }
        public void Start()
        {
            PlayersList = new List<Player>();
            LoadingPlayers = new List<Player>();
        }
        public bool AddPlayer(string Name, int PlayerID)
        {
            if (PlayersList.Count + 1 > MaxPlayers)
            {
                return false;
            }
            foreach (Player player in PlayersList)
            {
                if (player.PlayerName == Name)
                {
                    return false;
                }
            }
            Player newPlayer = new Player();
            newPlayer.PlayerName = Name;
            newPlayer.ID = PlayerID;
            PlayersList.Add(newPlayer);
            LoadingPlayers.Add(newPlayer);
            if (PlayersList.Count == MaxPlayers)
            {
                eventSender.SendMaxPlayers();
            }
            Console.WriteLine("added player");
            Console.WriteLine("listing players:");
            foreach (Player player in PlayersList)
            {
                Console.WriteLine(player.PlayerName);
            }
            return true;
        }
        public bool RemovePlayer(int playerID)
        {
            foreach (Player player in PlayersList)
            {
                if (player.ID == playerID)
                {
                    PlayersList.Remove(player);
                    if (PlayersList.Count == 0)
                    {
                        eventSender.SendNoneConnected();
                    }
                    return true;
                }
            }
            return false;
        }
    }
    public class PlayerEvents
    {
        public event EventHandler RequiredPlayersReached;
        public event EventHandler FirstPlayerConnected;
        public event EventHandler NoPlayersConnected;
        public event EventHandler AllPlayersLoaded;
        public void SendNoneConnected()
        {
            if (NoPlayersConnected != null)
            {
                NoPlayersConnected(this, null);
            }
        }
        public void SendAllLoaded()
        {
            if (AllPlayersLoaded != null)
            {
                AllPlayersLoaded(this, null);
            }
        }
        public void SendFirstConnected()
        {
            if (FirstPlayerConnected != null)
            {
                FirstPlayerConnected(this, null);
            }
        }
        public void SendMaxPlayers()
        {
            if (RequiredPlayersReached != null)
            {
                RequiredPlayersReached(this, null);
            }
        }
    }
}
