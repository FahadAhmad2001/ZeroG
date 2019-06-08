using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroG.Logger;
using UnityEngine;
using Harmony;
using System.Reflection;
using ZeroG.MultiplayerClient;


namespace ZeroG.Patches
{
    public class PlayerDetailsCanvasAwakePatch
    {
        public static void Postfix(PlayerDetailsCanvas __instance)
        {
            try
            {
                FieldInfo accountMenuInfo = __instance.GetType().GetField("_accountMenu", BindingFlags.NonPublic | BindingFlags.Instance);
                AccountMenu acMenu = (AccountMenu)accountMenuInfo.GetValue(__instance);
                InstanceKeeper.SetAccountMenu(acMenu);
                WriteLog.Verbose("Successfully got AccountMenu class");
            }
            catch(Exception ex)
            {
                WriteLog.Error("Error while obtaining AccountMenu class: " + ex.ToString());
            }
        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            MethodBase orignalMethod = typeof(PlayerDetailsCanvas).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo postfixMethod = GetType().GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, null, new HarmonyMethod(postfixMethod));
        }
    }
}
