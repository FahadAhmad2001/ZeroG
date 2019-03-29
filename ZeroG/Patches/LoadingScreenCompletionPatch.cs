using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroG.Logger;
using ZeroG.MultiplayerClient;
using UnityEngine.UI;
using System.Reflection;
using I2.Loc;
using Harmony;
using ZeroG.Logger;

namespace ZeroG.Patches
{
    public class LoadingScreenCompletionPatch
    {
        public static bool Prefix(LoadingScreenManager __instance)
        {
            if (Main.IsConnected)
            {
                try
                {
                    WriteLog.General("Game scene loading completed");
                    FieldInfo progressBarInfo = __instance.GetType().GetField("_progressBar", BindingFlags.NonPublic | BindingFlags.Instance);
                    Image progressBar = (Image)progressBarInfo.GetValue(__instance);
                    FieldInfo loadingTextInfo = __instance.GetType().GetField("_loadingLocalize", BindingFlags.NonPublic | BindingFlags.Instance);
                    Localize loadingText = (Localize)loadingTextInfo.GetValue(__instance);
                    progressBar.fillAmount = 1f;
                    loadingText.SetTerm("LOADING COMPLETE: Waiting for other players (ZeroG)");
                }
                catch (Exception ex)
                {

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
            MethodBase orignalMethod = typeof(LoadingScreenManager).GetMethod("ShowCompletionVisuals", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo prefixMethod = GetType().GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, new HarmonyMethod(prefixMethod));
        }
    }
}
