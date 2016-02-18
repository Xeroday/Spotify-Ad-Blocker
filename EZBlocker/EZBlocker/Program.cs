using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EZBlocker
{
    static class Program
    {
        public static string appGuid =
            ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string mutexId = string.Format("Local\\{{{0}}}", appGuid); // unique id for local mutex

            using (var mutex = new Mutex(false, mutexId))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMain());
                    mutex.ReleaseMutex();
                }
                else // another instance is already running
                {
                    WindowUtilities.ShowFirstInstance();
                    return;
                }
            }
        }
    }
}
