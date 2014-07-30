using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace drive_map_utility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        public static string readFromAppConfig(string customKey)
        {
            AppSettingsReader appConfig = new AppSettingsReader();
            return appConfig.GetValue(customKey, typeof(string)).ToString();
        }
    }
}
