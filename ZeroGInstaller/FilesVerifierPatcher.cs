using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ZeroGInstaller
{
    public class FilesVerifierPatcher
    {
        public static bool VerifyFilePath(string filepath)
        {
            WriteToLog("Attempting to find file: " + filepath);
            if (File.Exists(filepath))
            {
                WriteToLog("File found successfully");
                return true;
            }
            else
            {
                WriteToLog("File not found");
                return false;
            }
        }
        private static void WriteToLog(string text)
        {
            Logger.LogText(DateTime.Now + "   " + text + Environment.NewLine);
        }
        public static bool VerifyFilesPath(string[] filesPath)
        {
            bool status = true;
            foreach (string eachPath in filesPath)
            {
                if (!VerifyFilePath(eachPath))
                {
                    WriteToLog("File: " + eachPath + " was not found!");
                    status = false;
                }
            }
            return status;
        }
        public static bool RestoreACShrp(string path, string tempname, string managedDir)
        {
            bool found = false;
            using(ModuleDefMD module = ModuleDefMD.Load(path))
            {
                foreach (TypeDef type in module.Types)
                {
                    if (type.Name.Contains("SplashScreen"))
                    {
                        WriteToLog("Successfully found SplashScreen class, getting Awake method");
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Name.Contains("Awake"))
                            {
                                WriteToLog("Successfully found Awake method");
                                foreach (Instruction instruction in method.Body.Instructions)
                                {
                                    if (instruction.Operand != null)
                                    {
                                        if (instruction.Operand.ToString() == "System.Void ZeroG.Init::Load()")
                                        {
                                            found = true;
                                            WriteToLog("Found initialization instruction, removing");
                                            method.Body.Instructions.Remove(instruction);
                                            module.Write(managedDir + "\\" + tempname);
                                            //module.Dispose();
                                            goto EndRestoreACSharp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            EndRestoreACSharp:;
            }

            if (!found)
            {
                File.SetAttributes(path, System.IO.FileAttributes.Normal);
                WriteToLog("Failed to unpatch Assembly-CSharp.dll");
                return false;
            }
            File.SetAttributes(path, System.IO.FileAttributes.Normal);
            File.Delete(path);
            File.Copy(managedDir + "\\" + tempname, path);
            File.Delete(managedDir + "\\" + tempname);
            return true;
            
            //ModuleDefMD module = ModuleDefMD.Load(path);

        }
        public static bool PatchACShrp(string assemblypath, string modpath, string tempName, string managedDir)
        {
            bool installed = false;
            WriteToLog("Attempting to patch Assembly-CSharp.dll");
            using(ModuleDefMD mod = ModuleDefMD.Load(modpath))
            {
                foreach (TypeDef type in mod.GetTypes())
                {
                    if (type.Name == "Init")
                    {
                        WriteToLog("Got mod initialization class");
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Name == "Load")
                            {
                                WriteToLog("Got mod initialization method");
                                using (ModuleDefMD module = ModuleDefMD.Load(assemblypath))
                                {
                                    MemberRef reference = module.Import(method);
                                    foreach (TypeDef type2 in module.GetTypes())
                                    {
                                        if (type2.Name == "SplashScreen")
                                        {
                                            WriteToLog("Successfully found SplashScreen class, getting Awake method");
                                            foreach (MethodDef method2 in type2.Methods)
                                            {
                                                if (method2.Name == "Awake")
                                                {
                                                    WriteToLog("Successfully found Awake method");
                                                    Instruction modLoader = OpCodes.Call.ToInstruction(reference);
                                                    method2.Body.Instructions.Insert(0, modLoader);
                                                    WriteToLog("Successfully added ZeroG loading instruction");
                                                    module.Write(managedDir + "\\" + tempName);
                                                    installed = true;
                                                    goto EndPatchACSharp;
                                                }
                                            }
                                        }
                                    }
                                }
                                //ModuleDefMD module = ModuleDefMD.Load(assemblypath);
                                
                            }
                        }
                    }
                }
            EndPatchACSharp:;
            }
            // ModuleDefMD mod = ModuleDefMD.Load(modpath);
            if (!installed)
            {
                File.SetAttributes(assemblypath, System.IO.FileAttributes.Normal);
                WriteToLog("Unable to patch Assembly-CSharp.dll. You have to do it manually");
                return false;
            }
            File.SetAttributes(assemblypath, System.IO.FileAttributes.Normal);
            File.Delete(assemblypath);
            File.Copy(managedDir + "\\" + tempName, assemblypath);
            File.Delete(managedDir + "\\" + tempName);
            WriteToLog("Successfully patched Assembly-CSharp.dll");
            return true;
        }
        public static bool CheckACShrpPatched(string filePath)
        {
            WriteToLog("Attemting to load Assembly-CSharp.dll");
            bool patched = false;
            using(ModuleDefMD module = ModuleDefMD.Load(filePath))
            {
                WriteToLog("Attempting to get types");
                foreach (TypeDef type in module.Types)
                {
                    if (type.Name.Contains("SplashScreen"))
                    {
                        WriteToLog("Successfully found SplashScreen class, getting Awake method");
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Name.Contains("Awake"))
                            {
                                WriteToLog("Successfully found Awake method");
                                foreach (Instruction instruction in method.Body.Instructions)
                                {
                                    if (instruction.Operand != null)
                                    {
                                        if (instruction.Operand.ToString() == "System.Void ZeroG.Init::Load()")
                                        {
                                            WriteToLog("Assembly-CSharp.dll is already patched");
                                            patched = true;
                                            goto EndCheckPatch;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            EndCheckPatch:;
                module.Dispose();
            }

            if (patched)
            {
                File.SetAttributes(filePath, System.IO.FileAttributes.Normal);
                return true;
            }
            WriteToLog("Assembly-CSharp.dll is not patched");
            File.SetAttributes(filePath, System.IO.FileAttributes.Normal);
            return false;
            //ModuleDefMD module = ModuleDefMD.Load(filePath);

        }
    }
}
