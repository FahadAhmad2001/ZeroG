using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroG.Logger;
using System.IO;
using System.Reflection;
using FrameworkZeroG.Packets;
using UnityEngine;
using FrameworkZeroG.CustomUnitySerializable;

namespace ZeroG.MultiplayerClient
{
    public class Main
    {
        public static bool IsPatched = false;
        public static bool IsConnected = false;
        public static GameObject player;
        public static int opponents;
        public List<string> opponentsNames = new List<string>();
        public List<GameObject> playerShips = new List<GameObject>();
        public static bool AllPlayersLoaded = false;
        GameClient gClient;
        string userName = File.ReadAllText("username.txt");
        // public static Client netClient;
        public void Start()
        {
            Connect();
            //gClient.
        }
        public void SetOpponentsNames(List<string> namesList)
        {
            opponentsNames = namesList;
        }
        public static void LoadGame(string levelName, int laps, int players, bool isReverse)
        {
            try
            {
                WriteLog.Debug("Starting generating and loading multiplayer scene...");
                GameSetupScript.Instance.ReverseTrack = isReverse;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentGameMode = GameManager.GameMode.QuickRace;
                    GameManager.Instance.CurrentGameType = GameManager.GameType.SinglePlayer;
                    GameManager.Instance.CurrentRaceType = GameManager.RaceType.Single;
                    WriteLog.Verbose("Setting Game mode/type/racetype to QuickRace/singleplayer/single");
                }
                LevelData levelData = LevelDatabase.Instance.LevelDataList.Find((LevelData level) => level.BuildName == levelName && (level.IsReverseLevel == isReverse));
                if (levelData == null)
                {
                    WriteLog.Debug("Potential error, levelData is null");
                }
                GameSetupScript.Instance.SetCurrentLevel(levelData);
                GameSetupScript.Instance.GetCurrentLevel().RaceMode = GameManager.RaceType.Single;
                GameSetupScript.Instance.EnableTutorial = false;
                GameSetupScript.Instance.Bots = players;
                GameSetupScript.Instance.Laps = laps;
                GameSetupScript.Instance.NumberOfPlayers = 1;
                WriteLog.Verbose("Starting to load game scene");
                LoadingScreenManager.LoadScene(GameSetupScript.Instance.GetCurrentLevel().BuildName, GameSetupScript.Instance.GetCurrentLevel().BuildIndex);
            }
            catch (Exception ex)
            {
                WriteLog.Error("Error while trying to load game: " + ex.Message + ex.StackTrace);
            }
        }
        public void Connect()
        {
            WriteLog.Verbose("Attempting to start client");
            gClient = new GameClient();
            WriteLog.General("Successfully instantiated client");
            gClient.Start();
            WriteLog.Debug("Client started successfully");
            InstanceKeeper.SetGameClient(gClient);
            IsConnected = true;
        }
        public void SendPlayerUpdate(Vector3 position, Quaternion rotation)
        {
            PlayerPosition packet = new PlayerPosition();
            packet.Generate(userName, ConvertCustomTypes.ConvertVectorSerializable(position), ConvertCustomTypes.ConvertQuaternionSerializable(rotation));
            ZeroGPacket mainPacket = PacketGenerator.Generate("PlayerPosition", packet);
            gClient.Send(mainPacket, LiteNetLib.DeliveryMethod.Unreliable);
        }
        public void DoNothingUseful()
        {
            //File.WriteAllText("WHY.TXT", "whyyyyyy");
            StreamWriter writer = new StreamWriter("shouldntwork.txt");
            writer.WriteLine("THIS SHOULD NOT WORK");
            writer.Close();
        }
        public void SendLoadingComplete()
        {
            WriteLog.Verbose("Sending loading complete message to server");
            LoadingComplete completePacket = new LoadingComplete();
            completePacket.Generate(true, userName);
            // GamePacket mainPacket = new GamePacket();
            // mainPacket.Generate("LoadingComplete", null, completePacket);
            ZeroGPacket packet = PacketGenerator.Generate("LoadingComplete", completePacket);
            gClient.Send(packet, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }
        public static bool TryConnect(string ip = "localhost", int port = 6000)
        {
            try
            {
                //    netClient.Init();
                //   netClient.Start();
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Error("Error while connecting..." + ex.Message + ex.StackTrace);
                return false;
            }
        }

    }
}
