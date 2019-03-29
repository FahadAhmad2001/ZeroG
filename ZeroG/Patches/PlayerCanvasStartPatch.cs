using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroG.Logger;
using ZeroG.MultiplayerClient;
using Harmony;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeroG.MultiplayerClient.CustomComponents;
using System.IO;

namespace ZeroG.Patches
{
    public class PlayerCanvasStartPatch
    {
        public static void Postfix(PlayerCanvas __instance)
        {
            if (Main.IsConnected)
            {
                try
                {
                    WriteLog.General("Da parent GameObject is " + __instance.gameObject.gameObject.transform.parent.gameObject.name + " in scene " + __instance.gameObject.scene.name);
                    foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        WriteLog.General("Current GameObject is " + gameObject.name + " in scene " + gameObject.scene.name);
                        if (gameObject.name == "AITargets")
                        {
                            UnityEngine.Object.Destroy(gameObject);
                        }
                        if (gameObject.name == "Players")
                        {
                            WriteLog.General("Found players gameobject");

                            List<string> playersList = new List<string>();
                            Main clientInst = InstanceKeeper.GetMainClient();
                            playersList = clientInst.opponentsNames;
                            List<string> addedNames = new List<string>();
                            int count = gameObject.transform.GetChildCount();
                            WriteLog.General("Found " + count + " children");
                            for (int counter = 0; counter < count; counter++)
                            {
                                WriteLog.General("Current player : " + counter);
                                foreach (Component component in gameObject.transform.GetChild(counter).gameObject.GetComponentsInChildren<Component>())
                                {
                                    WriteLog.General("TEMP, for testing: found sub-component called " + component.name + " of type " + component.GetType().Name + " and pos " + component.transform.position);
                                }
                                OnShipUI playerUI = gameObject.transform.GetChild(counter).gameObject.GetComponentInChildren<OnShipUI>();
                                if (playerUI == null/* && gameObject.transform.GetChild(counter).gameObject.name=="Player (Clone)"*/)
                                {
                                    WriteLog.General("Found an AI player");
                                    gameObject.transform.GetChild(counter).gameObject.GetComponentInChildren<VehicleBehaviour>().SetAI(false);
                                    foreach (string name in playersList)
                                    {
                                        if (!addedNames.Contains(name) && (File.ReadAllText("username.txt") != name))
                                        {
                                            WriteLog.General("Adding player: " + name);
                                            gameObject.transform.GetChild(counter).gameObject.AddComponent<ZeroGPlayer>();
                                            ZeroGPlayer playerLocalName = gameObject.transform.GetChild(counter).gameObject.GetComponent<ZeroGPlayer>();
                                            playerLocalName.PlayerName = name;
                                            addedNames.Add(name);
                                            goto Endforeach;
                                        }
                                    }
                                Endforeach:;
                                    clientInst.playerShips.Add(gameObject.transform.GetChild(counter).gameObject);
                                }
                            }
                            InstanceKeeper.SetMainClient(clientInst);
                        }
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.Error("Error while trying to get GameObject, : " + ex.Message + ex.StackTrace);
                }
            }
            else
            {

            }
        }
        public void PatchGame(HarmonyInstance harmonyInst)
        {
            MethodBase orignalMethod = typeof(PlayerCanvas).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo postfixMethod = GetType().GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
            harmonyInst.Patch(orignalMethod, null, new HarmonyMethod(postfixMethod));
        }
    }
}
