using System;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using Anotar.NLog;

namespace SpotifyMuter
{
    static class Program
    {
        private static readonly string AppGuid =
            ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NlogConfiguration.Configure();

            LogTo.Debug("Starting SpotifyMuter.");

            string mutexId = $"Local\\{{{AppGuid}}}"; // unique id for local mutex
            using (var mutex = new Mutex(false, mutexId))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Main());
                    mutex.ReleaseMutex();
                }
                else
                {
                    LogTo.Debug("SpotifyMuter is already running. Exiting.");
                }
            }

            LogTo.Debug("SpotifyMuter was closed. Exiting");
        }
    }
}