using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteNetLib;
using Harmony;
using ZeroG.Logger;
using LiteNetLib.Utils;
using ZeroG.MultiplayerClient.PacketProcessor;
using FrameworkZeroG;
using FrameworkZeroG.Packets;
using System.Threading;
using System.IO;

namespace ZeroG.MultiplayerClient
{

    public class GameClient
    {


        NetManager client;
        EventBasedNetListener listener;
        NetPacketProcessor netPackProc;
        ProcessPacket packetProcessor;
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public void Start()
        {
            try
            {

                packetProcessor = new ProcessPacket();
                listener = new EventBasedNetListener();
                client = new NetManager(listener);
                netPackProc = new NetPacketProcessor();
                netPackProc.SubscribeReusable<ZeroGPacket, NetPeer>(ProcessPacket);
                listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
                listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
                listener.PeerDisconnectedEvent += Listener_PeerDisconnectedEvent;
                WriteLog.General("Created NetworkRecieveEvent and PeerConnectedEvent");
                WriteLog.General("Created eventbased listener and client");
                WriteLog.Debug("Attempting to connect to game server");
                client.UpdateTime = 10;
                client.UnsyncedEvents = true;
                client.Start();
                if (File.Exists("config.ini"))
                {
                    string text = File.ReadAllText("config.ini");
                    string[] parts = text.Split(':');
                    client.Connect(parts[0], int.Parse(parts[1]), "ZeroG");
                }
                else
                {
                    client.Connect("127.0.0.1", 6001, "ZeroG");
                }
                resetEvent.WaitOne(2000);
                resetEvent.Reset();


            }
            catch (Exception ex)
            {
                WriteLog.Error("Error while trying to connect: " + ex.Message + ex.StackTrace);
            }
        }
        public void Send(ZeroGPacket packet, DeliveryMethod deliveryMethod)
        {
            NetPeer[] peers = client.GetPeers();
            foreach (NetPeer peer in peers)
            {
                peer.Send(netPackProc.Write(packet), deliveryMethod);
            }
        }
        private void Listener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            //throw new NotImplementedException();
            WriteLog.Debug("Lost connection to game server: " + peer.EndPoint + ". Reason: " + disconnectInfo.Reason + ". Socket Error Code: " + disconnectInfo.SocketErrorCode + ". Additional data: " + disconnectInfo.AdditionalData);
            Main.IsConnected = false;
        }

        private void Listener_PeerConnectedEvent(NetPeer peer)
        {
            //throw new NotImplementedException();
            resetEvent.Set();
            WriteLog.Debug("Successfully connected to server, sending info: " + peer.EndPoint);
            ClientInfo packet = new ClientInfo();
            packet.Generate(File.ReadAllText("username.txt"));
            ZeroGPacket mainPacket = PacketGenerator.Generate("ClientInfo", packet);
            peer.Send(netPackProc.Write(mainPacket), DeliveryMethod.ReliableOrdered);
        }
        void ProcessPacket(ZeroGPacket packet, NetPeer peer)
        {
            packetProcessor.Process(packet);
        }
        private void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netPackProc.ReadAllPackets(reader, peer);
            // throw new NotImplementedException();
        }
    }
}
