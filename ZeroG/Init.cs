using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Harmony;
using System.Reflection;
using ZeroG.Patches;
using ZeroG.Logger;

namespace ZeroG
{
    public class Init
    {
        public static void Load()
        {
            try
            {
                try
                {
                    File.WriteAllText("temp.txt", "testing for logging");
                    File.Delete("temp.txt");
                }
                catch (Exception ex)
                {
                    //So that it wont crash trying to write log if the folder needs admin permissions
                    WriteLog.EnableDisable(false);
                }
                WriteLog.SetLogLevel(LogSeverity.Verbose, true);
                WriteLog.General("Creating harmony instance");
                HarmonyInstance harmony = HarmonyInstance.Create("com.zerog.mpmod");
                WriteLog.General("Attempting to get all types");
                Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
                WriteLog.General("Found " + allTypes.Length + " types");
                List<Type> patches = new List<Type>();
                foreach (Type type in allTypes)
                {
                    if (type.Namespace == "ZeroG.Patches" && !type.IsAbstract)
                    {
                        WriteLog.Verbose("Found a patch: " + type.Name);
                        patches.Add(type);
                    }
                }
                foreach (Type type in patches)
                {
                    var instance = Activator.CreateInstance(type);
                    MethodInfo patchMethod = type.GetMethod("PatchGame", BindingFlags.Instance | BindingFlags.Public);
                    WriteLog.General("Attempting to invoke PatchGame method on " + type.Name);
                    patchMethod.Invoke(instance, new object[] { harmony });
                    WriteLog.Debug("Successfully patched " + type.Name);
                }
                WriteLog.Debug("Successfully patched methods");
            }
            catch (Exception ex)
            {
                WriteLog.Error(ex.Message + ex.StackTrace + ex.TargetSite + ex.InnerException);
            }
        }
    }
}