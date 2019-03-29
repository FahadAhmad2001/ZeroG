using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkZeroG.Packets;
using UnityEngine;
using ZeroG.MultiplayerClient.CustomComponents;
using ZeroG.Logger;
using FrameworkZeroG.CustomUnitySerializable;

namespace ZeroG.MultiplayerClient.PacketProcessor
{
    public class PlayerPositionProcessor
    {
        public static void Process(PlayerPosition packet)
        {
            Main clientInst = InstanceKeeper.GetMainClient();
            foreach (GameObject gameobject in clientInst.playerShips)
            {
                ZeroGPlayer playerName = gameobject.GetComponent<ZeroGPlayer>();
                try
                {
                    if (playerName.PlayerName == packet.PlayerName)
                    {
                        gameobject.GetComponentInChildren<PlayerObjectConfiguration>().GetShip().transform.position = ConvertCustomTypes.ConvertVectorOriginal(packet.Position);
                        gameobject.GetComponentInChildren<PlayerObjectConfiguration>().GetShip().transform.rotation = ConvertCustomTypes.ConvertQuaternionOriginal(packet.Rotation);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.Error("Error while processing remote player movement: " + ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
