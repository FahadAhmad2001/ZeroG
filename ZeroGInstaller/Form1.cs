using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZeroGInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string gamePath = "";
        bool isAlreadyPatched = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            Logger.StartLogging();
            Logger.useRTB = true;
            Logger.appLog = richTextBox1;
            WriteToLog("ZeroG Installer developed by EmperorProdigy");
            WriteToLog("Scanning for ZeroG client assembly and server files (for installation)");
            bool filesPresent = FilesVerifierPatcher.VerifyFilesPath(new string[42] { @"0Harmony.dll", @"FrameworkZeroG.dll", @"FrameworkZeroG.pdb", @"LiteNetLib.dll", @"ZeroG.dll", @"ZeroG.pdb", @"Server\Normal\FrameworkZeroG.dll", @"Server\Normal\FrameworkZeroG.pdb", @"Server\Normal\LiteNetLib.dll", @"Server\Normal\UnityEngine.CoreModule.dll", @"Server\Normal\UnityEngine.CoreModule.xml", @"Server\Normal\UnityEngine.SharedInternalsModule.dll", @"Server\Normal\UnityEngine.SharedInternalsModule.xml", @"Server\Normal\ZeroGServer.exe", @"Server\Normal\ZeroGServer.pdb", @"Server\GUI\Blue Marble (Space).jpg", @"Server\GUI\Cryon Terminal (Space).png", @"Server\GUI\Drifters Paradise (Urban).jpg", @"Server\GUI\FrameworkZeroG.dll", @"Server\GUI\FrameworkZeroG.pdb", @"Server\GUI\LiteNetLib.dll", @"Server\GUI\Michael's Bay (Island).png", @"Server\GUI\Nevelmale Pass (Arctic).png", @"Server\GUI\Nexus Wastes (Desert).png", @"Server\GUI\Northern Ring (Arctic).png", @"Server\GUI\Nososs Grill (Island).png", @"Server\GUI\Polar Cavern (Arctic).jpg", @"Server\GUI\Scorching Testinggrounds (Practice).png", @"Server\GUI\Serpentine Valley (Desert).png", @"Server\GUI\Skydome City (Urban).png", @"Server\GUI\Spiral Drop (Urban).jpg", @"Server\GUI\Square Creek (Island).png", @"Server\GUI\Trappist Station (Space).png", @"Server\GUI\UnityEngine.CoreModule.dll", @"Server\GUI\UnityEngine.CoreModule.xml", @"Server\GUI\UnityEngine.SharedInternalsModule.dll", @"Server\GUI\UnityEngine.SharedInternalsModule.xml", @"Server\GUI\Vengo canyon (Desert).png", @"Server\GUI\ZeroGGuiServer.exe", @"Server\GUI\ZeroGGuiServer.pdb", @"Server\GUI\ZeroGServer.exe", @"Server\GUI\ZeroGServer.pdb" });
            if (!filesPresent)
            {
                MessageBox.Show("Some files were not present so installation cannot continue");
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
        public void WriteToLog(string text)
        {
            Logger.LogText(DateTime.Now + "   " + text + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = folderBrowserDialog1.SelectedPath;
                button2.Enabled = true;
                gamePath = folderBrowserDialog1.SelectedPath;
                if (FilesVerifierPatcher.VerifyFilePath(folderBrowserDialog1.SelectedPath + @"\\Antigraviator_Data\\Managed\\Assembly-CSharp.dll"))
                {
                    isAlreadyPatched = FilesVerifierPatcher.CheckACShrpPatched(folderBrowserDialog1.SelectedPath + @"\\Antigraviator_Data\\Managed\\Assembly-CSharp.dll");
                    if (isAlreadyPatched)
                    {
                        button3.Enabled = true;
                    }
                    else
                    {
                        button3.Enabled = false;
                    }
                }
                else
                {
                    WriteToLog("Installation might crash as selected folder does not have correct/complete Antigraviator files");
                    MessageBox.Show("Installation might crash as selected folder does not have correct/complete Antigraviator files");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            string managed = gamePath + "\\Antigraviator_Data\\Managed";
            List<string> sourcePaths = new List<string>();
            List<string> destPaths = new List<string>();
            string[] assembly = new string[6] { "ZeroG.pdb", "ZeroG.dll", "LiteNetLib.dll", "FrameworkZeroG.dll", "FrameworkZeroG.pdb", "0Harmony.dll" };
            foreach (string file in assembly)
            {
                if (!FilesCopier.DeleteFile(managed + file))
                {
                    WriteToLog(managed + "\\" + file + " failed to delete");
                    MessageBox.Show("Some old mod files could not be deleted, this may cause problems with the installation. " + Environment.NewLine + "Please check the log for more details and manually replace the respective files");

                }
                sourcePaths.Add(Application.StartupPath + "\\" + file);
                destPaths.Add(managed + "\\" + file);
            }
            if (!FilesCopier.CopyMultiple(sourcePaths, destPaths,true))
            {
                WriteToLog("Some new mod files could not be copied to " + managed);
                MessageBox.Show("Some new mod files could not be copied to " + managed + Environment.NewLine + "Please copy them manually (refer to support for how)");
            }
            if (isAlreadyPatched)
            {
                WriteToLog("No need to patch Assembly-CSharp.dll as it is already patched");
                //MessageBox.Show("Successfully installed!");
            }
            else
            {
                if (!FilesCopier.CreateBackup())
                {
                    WriteToLog("A backup could not be created. It is recommended to manually create a backup of " + managed + "\\Assembly-CSharp.dll");
                    MessageBox.Show("An automatic backup could not be created." + Environment.NewLine + "Please manually create a backup of " + managed + "\\Assembly-CSharp.dll , then press OK");

                }
                else
                {
                    WriteToLog("Created automatic backup at My Documents");
                }
                if (!FilesCopier.AddToBackup(managed + "\\Assembly-CSharp.dll", "Backup-Assembly-CSharp.dll"))
                {
                    WriteToLog("A backup of Assembly-CSharp.dll could not be created!");
                    MessageBox.Show("An automatic backup of Assembly-CSharp.dll could not be created. Please manually backup" + Environment.NewLine + managed + "\\Assembly-CSharp.dll , then press OK");
                }
                else
                {
                    WriteToLog("Created backup of Assembly-CSharp.dll");
                }
                if (!FilesVerifierPatcher.PatchACShrp(managed + "\\Assembly-CSharp.dll", managed + "\\ZeroG.dll","TempACShrp.dll",managed))
                {
                    WriteToLog("Assembly-CSharp.dll could not be patched!");
                    MessageBox.Show(managed + "\\Assembly-CSharp.dll could not be patched! Please do it manually");
                    if (FilesCopier.DeleteFile(managed + "\\Assembly-CSharp.dll"))
                    {
                        if (FilesCopier.CopyFile(Environment.SpecialFolder.MyDocuments + "\\ZeroG Mod Temp files\\Backup-Assembly-CSharp.dll", managed + "\\Assembly-CSharp.dll",true))
                        {
                            WriteToLog("Assembly-CSharp.dll successfully restored.");
                        }
                        else
                        {
                            WriteToLog("Assembly-CSharp.dll could not be automatically restored. Please do it manually");
                            MessageBox.Show(@"Assembly-CSharp.dll could not be restored. Please restore it from My Documents\ZeroG Mod Temp Files\Backup-Assembly-CSharp.dll" + Environment.NewLine + " (or from wherever you backed it up manually) and paste it in " + Environment.NewLine + managed + " and rename it Assembly-CSharp");
                        }
                    }
                    else
                    {
                        WriteToLog("Assembly-CSharp.dll could not be deleted!");
                        MessageBox.Show(managed + @"\Assembly-CSharp.dll could not be restored. Please restore it from My Documents\ZeroG Mod Temp Files\Backup-Assembly-CSharp.dll" + Environment.NewLine + " (or from wherever you backed it up manually) and paste it in " + Environment.NewLine + managed + " and rename it Assembly-CSharp");
                    }
                }
                else
                {
                    if (!FilesCopier.ClearBackup())
                    {
                        WriteToLog("Failed to delete temporary backup");
                        MessageBox.Show("The automatic temporary backup that was created was not deleted. Please do it manually");
                    }
                    WriteToLog("Assembly-CSharp.dll successfully patched!");
                }
                
            }
            if (FilesCopier.CopyFolder(Application.StartupPath + "\\Server\\GUI", System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Antigraviator LAN Multiplayer Server", true))
            {
                WriteToLog("Successfully copied server files to desktop");
            }
            else
            {
                WriteToLog("Failed to copy server files!");
                MessageBox.Show("You have to copy the server from the zip manually");
            }
            MessageBox.Show("Installation complete!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string managed = gamePath + "\\Antigraviator_Data\\Managed";
            if (isAlreadyPatched)
            {
                if (!FilesCopier.CreateBackup())
                {
                    WriteToLog("A backup could not be created. It is recommended to manually create a backup of " + managed + "\\Assembly-CSharp.dll");
                    MessageBox.Show("An automatic backup could not be created." + Environment.NewLine + "Please manually create a backup of " + managed + "\\Assembly-CSharp.dll , then press OK");

                }
                else
                {
                    WriteToLog("Created automatic backup at My Documents");
                }
                if (!FilesCopier.AddToBackup(managed + "\\Assembly-CSharp.dll", "Backup-Assembly-CSharp.dll"))
                {
                    WriteToLog("A backup of Assembly-CSharp.dll could not be created!");
                    MessageBox.Show("An automatic backup of Assembly-CSharp.dll could not be created. Please manually backup" + Environment.NewLine + managed + "\\Assembly-CSharp.dll , then press OK");
                }
                else
                {
                    WriteToLog("Created backup of Assembly-CSharp.dll");
                }
                if (!FilesVerifierPatcher.RestoreACShrp(managed + "\\Assembly-CSharp.dll","TmpACShrp.dll",managed))
                {
                    WriteToLog("Assembly-CSharp.dll could not be unpatched!");
                    MessageBox.Show(managed + "\\Assembly-CSharp.dll could not be unpatched! Please do it manually");
                    if (FilesCopier.DeleteFile(managed + "\\Assembly-CSharp.dll"))
                    {
                        if (FilesCopier.CopyFile(Environment.SpecialFolder.MyDocuments + "\\ZeroG Mod Temp files\\Backup-Assembly-CSharp.dll", managed + "\\Assembly-CSharp.dll",true))
                        {
                            WriteToLog("Assembly-CSharp.dll successfully restored.");
                        }
                        else
                        {
                            WriteToLog("Assembly-CSharp.dll could not be automatically restored. Please do it manually");
                            MessageBox.Show(@"Assembly-CSharp.dll could not be restored. Please restore it from My Documents\ZeroG Mod Temp Files\Backup-Assembly-CSharp.dll" + Environment.NewLine + " (or from wherever you backed it up manually) and paste it in " + Environment.NewLine + managed + " and rename it Assembly-CSharp");
                        }
                    }
                    else
                    {
                        WriteToLog("Assembly-CSharp.dll could not be deleted!");
                        MessageBox.Show(managed + @"\Assembly-CSharp.dll could not be restored. Please restore it from My Documents\ZeroG Mod Temp Files\Backup-Assembly-CSharp.dll" + Environment.NewLine + " (or from wherever you backed it up manually) and paste it in " + Environment.NewLine + managed + " and rename it Assembly-CSharp");
                    }
                }
                else
                {
                    if (!FilesCopier.ClearBackup())
                    {
                        WriteToLog("Failed to delete temporary backup");
                        MessageBox.Show("The automatic temporary backup that was created was not deleted. Please do it manually");
                    }
                    WriteToLog("Assembly-CSharp.dll successfully unpatched!");
                }
            }
            string[] assembly = new string[6] { "ZeroG.pdb", "ZeroG.dll", "LiteNetLib.dll", "FrameworkZeroG.dll", "FrameworkZeroG.pdb", "0Harmony.dll" };
            foreach (string file in assembly)
            {
                if (!FilesCopier.DeleteFile(managed + file))
                {
                    WriteToLog(managed + "\\" + file + " failed to delete");
                    MessageBox.Show("Some old mod files could not be deleted. " + Environment.NewLine + "Please check the log for more details and manually delete the respective files");

                }
            }
            button3.Enabled = false;
            MessageBox.Show("Successfully uninstalled ZeroG");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To manually copy mod files: " +
                "\nCopy ZeroG.dll, ZeroG.pdb, FrameworkZeroG.dll, FrameworkZeroG.pdb, 0Harmony.dll, and LiteNetLib.dll from" +
                "\n the ZIP to Antigraviator folder" +
                @"\Antigraviator_Data\Managed" +
                "\n For other support please contact me on Discord EmperorProdigy#0500");
        }
    }
}
