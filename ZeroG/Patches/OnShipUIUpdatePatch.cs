using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using System.Reflection;
using ZeroG.MultiplayerClient;
using ZeroG.Logger;
using UnityEngine.SceneManagement;

namespace ZeroG.Patches
{
    public class OnShipUIUpdatePatch
    {
        public static void Postfix(OnShipUI __instance)
        {
            if (Main.IsConnected && (SceneManager.GetActiveScene().name != "MenuScene"))
            {
                try
                {
                    FieldInfo shipInfo = __instance.GetType().GetField("_ship", BindingFlags.NonPublic | BindingFlags.Instance);
                    GameObject playerInfo = (GameObject)shipInfo.GetValue(__instance);
                    Main clientInstance = InstanceKeeper.GetMainClient();
                    clientInstance.SendPlayerUpdate(playerInfo.transform.position, playerInfo.transform.rotation);
                }
                catch (Exception ex)
                {
                    WriteLog.Error("Error while processing local player update: " + ex.Message + ex.StackTrace);
                }
            }

        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            MethodBase orignalMethod = typeof(OnShipUI).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo postfixMethod = GetType().GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, null, new HarmonyMethod(postfixMethod));
        }
    }
}
