using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZeroG.Logger;
using System.Reflection;
using UnityEngine;
using I2.Loc;
using Harmony;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace ZeroG.Patches
{
    public class AccountMenuShowLoginAccountPanelPatch
    {
        public static void Postfix(AccountMenu __instance)
        {
            WriteLog.General("Searching for serverlogin.tmp ...");
            if (File.Exists("serverlogin.tmp"))
            {
                WriteLog.Verbose("Successfully found serverlogin.tmp, verifying timestamp...");
                try
                {
                    
                    string text = File.ReadAllText("serverlogin.tmp");
                    File.Delete("serverlogin.tmp");
                    string[] splitDateTime = text.Split(':');
                    if(splitDateTime[0]==DateTime.Now.Year.ToString() && splitDateTime[1] == DateTime.Now.Month.ToString() && splitDateTime[2] == DateTime.Now.Day.ToString() && (splitDateTime[3] == DateTime.Now.Hour.ToString() || splitDateTime[3] == (DateTime.Now.Hour -1).ToString()|| splitDateTime[3] == (24).ToString()) && (splitDateTime[4] == DateTime.Now.Minute.ToString() || splitDateTime[4] == (DateTime.Now.Minute -1).ToString() || splitDateTime[4] == (59).ToString()))     //dunno if this is necessary, but did this in case
                    {
                        WriteLog.Verbose("Successfully verified serverlogin.tmp");
                        foreach (GameObject gObject in SceneManager.GetActiveScene().GetRootGameObjects())
                        {
                            //WriteLog.General("FOR DEVELOPMENT: Found a GameObject in scene called " + gObject.name);
                            foreach(TextMeshProUGUI TMP in gObject.GetComponentsInChildren<TextMeshProUGUI>())
                            {
                                //WriteLog.General("Found a text called " + TMP.text + " in GameObject " + TMP.gameObject);
                                if(TMP.text=="Login E-mail:" && TMP.gameObject.name== "AccountEmailText")
                                {
                                    TMP.text = "Server IP:";
                                }
                                else if(TMP.text=="Password:" && TMP.gameObject.name == "AccountPasswordText")
                                {
                                    TMP.text = "Username:";
                                }
                                else if (TMP.text == "Login" && TMP.gameObject.name == "Text")
                                {
                                    TMP.text = "Connect";
                                }
                                else if (TMP.text == "Login" && TMP.gameObject.name == "TopAccountPanelText")
                                {
                                    TMP.text = "Please enter server IP and your username";
                                }
                                else if(TMP.text=="Forgot password" && TMP.gameObject.name == "Text")
                                {
                                    TMP.text = "";
                                }
                            }
                            foreach(Text NormalText in gObject.GetComponentsInChildren<Text>())
                            {
                               // WriteLog.General("Found a Unity text called " + NormalText.text + " in GameObject " + NormalText.gameObject);
                                if(NormalText.text== "Enter e-mail address..." && NormalText.gameObject.name== "Placeholder")
                                {
                                    NormalText.text = "";
                                }
                                else if(NormalText.text== "Enter password..." && NormalText.gameObject.name == "Placeholder")
                                {
                                    NormalText.text = "";
                                }
                            }
                        }
                        FieldInfo topPanelHeaderInfo = __instance.GetType().GetField("_topPanelHeaderLocalize", BindingFlags.NonPublic | BindingFlags.Instance);
                        Localize topPanelHeader = (Localize)topPanelHeaderInfo.GetValue(__instance);
                        topPanelHeader.SetTerm("Please enter the server IP and your username");
                        __instance.CancelDelegate = new AccountMenu.CancelButtonDelegate(__instance.HideAccountPanel);
                    }
                }
                catch(Exception ex)
                {
                    WriteLog.Error("An error ocurred while using ShowLoginAccountPanel to get server IP and username: " + ex.ToString());
                }
                

            }
        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            MethodBase orignalMethod = typeof(AccountMenu).GetMethod("ShowLoginAccountPanel", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo postfixMethod = GetType().GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, null, new HarmonyMethod(postfixMethod));
        }
    }
}
