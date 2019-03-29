using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using LiteNetLib;
using LiteNetLib.Utils;
using ZeroGServer.PlayerManagement;
using ZeroGServer.PacketProcessor;
using System.Threading;

namespace ZeroGServer
{
    public class Server
    {
        EventBasedNetListener listener;
        NetManager server;
        NetPacketProcessor localPacketProcessor;
        NewPlayerInfo newPlayerData;
        ProcessPacket zeroPacketProcessor;
        public bool Start(NewPlayerInfo playerInfo, int port)
        {
            listener = new EventBasedNetListener();
            server = new NetManager(listener);
            localPacketProcessor = new NetPacketProcessor();
            Console.WriteLine("started instances...");
            localPacketProcessor.SubscribeReusable<ZeroGPacket, NetPeer>(OnDataRecieve);
            if (!server.Start(port))
            {
                Console.WriteLine("Failed to start server");
                //return false;
                throw new Exception("Failed to start server...");
            }
            newPlayerData = playerInfo;
            listener.ConnectionRequestEvent += Listener_ConnectionRequestEvent;
            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
            listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
            listener.PeerDisconnectedEvent += Listener_PeerDisconnectedEvent;
            zeroPacketProcessor = new ProcessPacket();
            InstanceKeeper.SetServerinstance(this);
            GetEvents();
            // while (/*!Console.KeyAvailable*/1-1==0)
            //{
            //    server.PollEvents();
            //    Thread.Sleep(15);
            // }
            // this.server.Stop();
            return true;
        }
        public void GetEvents()
        {
            do
            {
                server.PollEvents();
                Thread.Sleep(15);
            } while (1 - 1 == 0);

        }
        private void Listener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            // throw new NotImplementedException();
            Console.WriteLine("Client with ID " + peer.Id + " and IP " + peer.EndPoint + " has disconnected. Reason: " + disconnectInfo.Reason);
            InstanceKeeper.players.RemovePlayer(peer.Id);
        }

        private void Listener_PeerConnectedEvent(NetPeer peer)
        {
            // throw new NotImplementedException();
            Console.WriteLine("Client connected from " + peer.EndPoint + " with id " + peer.Id + " and ping " + peer.Ping);
            // Console.WriteLine("Sending new player info");
            //  peer.Send(localPacketProcessor.Write(newPlayerData.SendNewPlayerInfo()), DeliveryMethod.ReliableOrdered);
        }

        private void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            // throw new NotImplementedException();
            try
            {
                //   Console.WriteLine("Attempting to deserialize recieved data");
                localPacketProcessor.ReadAllPackets(reader, peer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error1: " + ex.Message + ex.StackTrace);
            }
        }

        private void Listener_ConnectionRequestEvent(ConnectionRequest request)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Recieved connection request from " + request.RemoteEndPoint);
            request.AcceptIfKey("ZeroG");
        }
        public void SendToAll(ZeroGPacket packet, DeliveryMethod deliveryType)
        {
            server.SendToAll(localPacketProcessor.Write(packet), deliveryType);
        }
        public void SendToOthers(ZeroGPacket packet, DeliveryMethod deliveryType, NetPeer sender)
        {
            foreach (NetPeer peer in server.GetPeers())
            {
                if (peer != sender)
                {
                    peer.Send(localPacketProcessor.Write(packet), deliveryType);
                }
            }
        }
        public void OnDataRecieve(ZeroGPacket packet, NetPeer peer)
        {
            try
            {
                //  Console.WriteLine("recieved data from client " + peer.Id + " at " + peer.EndPoint + ", processing");
                zeroPacketProcessor.Process(packet, peer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
            }
        }
    }
}
