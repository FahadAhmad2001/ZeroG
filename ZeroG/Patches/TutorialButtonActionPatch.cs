using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using ZeroG.Logger;
using System.Reflection;
using ZeroG.MultiplayerClient;
using LiteNetLib;
using System.IO;

namespace ZeroG.Patches
{
    //[HarmonyPatch(typeof(MainMenuScreenBehaviour),"Credits")]
    public class TutorialButtonActionPatch
    {
        //[HarmonyPrefix]
        public static bool Prefix(MainMenuScreenBehaviour __instance)
        {
            if (Main.IsPatched)
            {
                try
                {
                    WriteLog.General("Attempting to connect to server");
                    Main mpManager = new Main();
                    WriteLog.General("Created instance of MultiplayerClient.Main");
                    mpManager.Start();
                    InstanceKeeper.SetMainClient(mpManager);
                }
                catch (Exception ex)
                {
                    WriteLog.Error("An error ocurred while trying to instantiate multiplayer client. " + ex.Message + ex.StackTrace + ex.TargetSite + ex.InnerException + ex.Source);
                }
                return false;
            }
            else
            {
                return true;
            }

        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            //throw new NotImplementedException();
            MethodBase originalMethod = typeof(MainMenuScreenBehaviour).GetMethod("Tutorial");
            MethodInfo prefixMethod = GetType().GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(originalMethod, new HarmonyMethod(prefixMethod));
        }
    }
}
