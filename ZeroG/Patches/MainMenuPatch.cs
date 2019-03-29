using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using ZeroG.Logger;
using System.Reflection;
using UnityEngine.UI;

namespace ZeroG.Patches
{
    //[HarmonyPatch(typeof(MenuScreenCamera),"StartRotation")]
    public class MainMenuPatch
    {
        //[HarmonyPostfix]
        public static void Postfix(MenuScreenCamera __instance)
        {
            try
            {
                WriteLog.General("trying to get all GameObjects in scene");
                foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    WriteLog.General("Current GameObject is " + gameObject.name + " in scene " + gameObject.scene.name);
                    if (gameObject.name == "Main Canvas")
                    {
                        WriteLog.Verbose("Got Maincanvas GameObject, trying to get text...");
                        foreach (TextMeshProUGUI creditsText in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
                        {
                            WriteLog.General("Current TextMeshProUGUI is " + creditsText.name + " with text " + creditsText.text);
                            if (creditsText.name == "TutorialButton")
                            {
                                WriteLog.Verbose("Found TutorialButton text, renaming to LAN Multi");
                                creditsText.text = "LAN Multi";
                                WriteLog.Debug("Successfully renamed tutorial button to LAN Multi");
                                MultiplayerClient.Main.IsPatched = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Error(ex.Message + ex.StackTrace);
            }
        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            //throw new NotImplementedException();
            MethodBase orignalMethod = typeof(MenuScreenCamera).GetMethod("StartRotation");
            MethodInfo postfixMethod = GetType().GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, null, new HarmonyMethod(postfixMethod));
        }
    }
}
