using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ZeroGInstaller
{
    public class FilesCopier
    {
        public static bool CreateBackup()
        {
            if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files"))
            {
                Directory.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files");
            }
            Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files");
            if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files"))
            {
                return true;
            }
            return false;
        }
        public static bool AddToBackup(string filepath, string newname)
        {
            WriteToLog("Attempting to add " + filepath + " with name " + newname + " to backup");
            if (File.Exists(filepath) && !File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files\\" + newname))
            {
                WriteToLog("Can copy file, not currently in backup");
                File.Copy(filepath, System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files\\" + newname);
                WriteToLog("Attempt to copy backup file");
                if (File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files\\" + newname))
                {
                    WriteToLog("Successfully added to backup");
                    return true;
                }
            }
            else
            {
                if (!File.Exists(filepath))
                {
                    WriteToLog("Source file does not exist!");
                }
            }
            WriteToLog("Could not add file to backup because it does not exist or is already in backup!");
            return false;
        }
        private static void WriteToLog(string text)
        {
            Logger.LogText(DateTime.Now + "   " + text + Environment.NewLine);
        }
        public static bool ClearBackup()
        {
            if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files"))
            {
                Directory.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files",true);
            }
            if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZeroG Mod Temp Files"))
            {
                return false;
            }
            return true;
        }
        
        public static bool CopyFile(string origPath, string destPath, bool canOverwrite)
        {
            WriteToLog("Attempting to copy " + origPath + " to " + destPath);
            string[] parts = Regex.Split(origPath, @"\\");
            string fileName = parts[parts.Length - 1];
            WriteToLog("Name of file is " + fileName);
            if (File.Exists(origPath))
            {
                if (File.Exists(destPath))
                {
                    if (canOverwrite)
                    {
                        WriteToLog("Overwriting file...");
                        File.Delete(destPath);
                        File.Copy(origPath, destPath);
                        if (File.Exists(destPath))
                        {
                            WriteToLog("successfully overwrote file");
                            return true;
                        }
                        WriteToLog("Failed to overwrite file");
                        return false;
                    }
                    else
                    {
                        WriteToLog("File already exists in destination folder and should not be overwritten");
                    }
                }
                else
                {
                    File.Copy(origPath, destPath);
                    if (File.Exists(destPath))
                    {
                        WriteToLog("Successfully copied file");
                        return true;
                    }
                }
                
            }
            else
            {
                WriteToLog("File does not exist!");
            }
            WriteToLog("File could not be copied as it already exists in the destination folder and should not be overwritten, or it does not exist!");
            return false;
        }
        public static bool CopyMultiple(List<string> origPath, List<string> destPath,bool canOverWrite)
        {
            if (origPath.Count == destPath.Count)
            {
                bool status = true;
                for (int count = 0; count < origPath.Count; count++)
                {
                    bool temp = CopyFile(origPath[count], destPath[count],canOverWrite);
                    if (!temp)
                    {
                        status = false;
                    }
                }
                return status;
            }
            return false;
        }
        public static bool DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Logger.LogText(DateTime.Now + "   Deleting: " + filePath + Environment.NewLine);
                File.Delete(filePath);
                if (File.Exists(filePath))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool DeleteMultiple(string[] filePaths)
        {
            bool status = true;
            foreach (string path in filePaths)
            {
                if (!DeleteFile(path))
                {
                    status = false;
                }
            }
            return status;
        }
        public static bool CopyFolder(string origPath, string destPath, bool canOverwrite)
        {
            WriteToLog("Attempting to recursively copy a directory: " + origPath + " to " + destPath);
            if (Directory.Exists(origPath))
            {
                if (Directory.Exists(destPath))
                {
                    if (canOverwrite)
                    {
                        Directory.Delete(destPath, true); //Maybe add a check here to verify its deleted in the future
                        Copy(origPath, destPath);
                        if (Directory.Exists(destPath))
                        {
                            WriteToLog("Successfully copied directory");
                            return true;
                        }
                    }
                    else
                    {
                        WriteToLog("Folder already exists and should not be overwritten");
                    }
                }
                else
                {
                    Copy(origPath, destPath);
                    if (Directory.Exists(destPath))
                    {
                        WriteToLog("Successfully copied directory");
                        return true;
                    }
                }
                
            }
            else
            {
                WriteToLog("Source folder does not exist");
            }
            WriteToLog("Failed to copy folder");
            return false;
        }

        //The following code was obtained from https://code.4noobz.net/c-copy-a-folder-its-content-and-the-subfolders/
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
