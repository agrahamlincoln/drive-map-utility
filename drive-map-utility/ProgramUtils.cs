using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace drive_map_utility
{

    /** The methods in this class are intended to be utility methods that can be used on any program
     * Please do not add any program-specific methods here.
     */
    class ProgramUtils //rename to Utilities
    {
        const string TIMESTAMP_FORMAT = "MM/dd HH:mm:ss ffff";
        const string LOGFILE_NAME = "DriveMaps_log.txt";

        public static bool matchString_IgnoreCase(string str1, string str2)
        {
            bool isMatch = false;
            if (str1.Equals(str2, StringComparison.OrdinalIgnoreCase))
            {
                isMatch = true;
            }
            return isMatch;
        }

        public static void writeLog(string error)
        {
            string logLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + LOGFILE_NAME;

            if (!System.IO.File.Exists(logLocation))
            {
                using (StreamWriter sw = System.IO.File.CreateText(logLocation))
                {
                    sw.WriteLine(getTimestamp(DateTime.Now) + error);
                }
            }
            else
            {
                StreamWriter sWriter = new StreamWriter(logLocation, true);
                sWriter.WriteLine(getTimestamp(DateTime.Now) + " " + error);

                sWriter.Close();
            }
        }

        private static string getTimestamp(DateTime value)
        {
            return value.ToString(TIMESTAMP_FORMAT);
        }

        //verifies whether machine has .net3.5 or greater
        public static bool HasNet35()
        {
            bool returnValue = false;
            List<string> versions = GetVersionFromRegistry(true);
            Regex netVersion = new Regex("(v3\\.5|v4.0|v4)+");

            foreach (string ver in versions)
            {
                if (netVersion.IsMatch(ver))
                {
                    writeLog(".NET version " + ver + " is installed.");
                    returnValue = true;
                    break;
                }
            }
            if (returnValue == false)
                writeLog(".NET 3.5 must be installed to run this.");
            return returnValue;
        }

        /* Code came from MSDN to search registry for currently installed .net versions
         * Has been modified to return the results in a List<string> format for verification
         */
        public static List<string> GetVersionFromRegistry(bool isSilent)
        {
            List<string> versions = new List<string>();

            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5  
                // or later, you can use: 
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,  
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "")
                        { //no install info, must be later.
                            if (!isSilent)
                                writeLog(versionKeyName + "  " + name);
                            versions.Add(versionKeyName);
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                if (!isSilent)
                                    writeLog(versionKeyName + "  " + name + "  SP" + sp);
                                versions.Add(versionKeyName);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "")
                            { //no install info, must be later.
                                if (!isSilent)
                                    writeLog(versionKeyName + "  " + name);
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    if (!isSilent)
                                        writeLog("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    if (!isSilent)
                                        writeLog("  " + subKeyName + "  " + name);
                                }
                            }
                        }
                    }
                }
            }
            return versions;
        }
    }
}
