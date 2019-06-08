using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using ZeroG.Logger;
using ZeroG.MultiplayerClient;
using Harmony;
using UnityEngine.UI;

namespace ZeroG.Patches
{
    public class AccountMenuLoginClickPatch
    {
        public static bool Prefix(AccountMenu __instance)
        {
            if(Main.IsPatched && Main.AwaitingServerAndName)
            {
                try
                {
                    FieldInfo emailInputInfo = __instance.GetType().GetField("_loginEmailInputField", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo passInputInfo = __instance.GetType().GetField("_loginPasswordInputField", BindingFlags.NonPublic | BindingFlags.Instance);
                    InputField serverIP = (InputField)emailInputInfo.GetValue(__instance);
                    InputField username = (InputField)passInputInfo.GetValue(__instance);
                    InstanceKeeper.mainClient.SetServerIpUsername(serverIP.text, username.text);
                }
                catch(Exception ex)
                {
                    WriteLog.Error("Error while determining server ip and username: " + ex.ToString());
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
            MethodBase originalMethod = typeof(AccountMenu).GetMethod("LoginClick");
            MethodInfo prefixMethod = GetType().GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(originalMethod, new HarmonyMethod(prefixMethod));
        }
    }
}
