using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroG.MultiplayerClient;
using Harmony;
using System.Reflection;

namespace ZeroG.Patches
{
    public class GameManagerStartCountDownPatch
    {
        public static bool Prefix(GameManager __instance)
        {
            if (Main.IsConnected)
            {
                Main clientInst = InstanceKeeper.GetMainClient();
                clientInst.SendLoadingComplete();
                while (!Main.AllPlayersLoaded)
                {

                }
                return true;
            }
            else
            {
                return true;
            }
        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            MethodBase orignalMethod = typeof(GameManager).GetMethod("StartCountDown", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            MethodInfo prefixMethod = GetType().GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, new HarmonyMethod(prefixMethod));
        }
    }
}
