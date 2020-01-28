using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.NotificationScheduler
{
    class Program
    {
        public static Log log;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                if (args.Length > 0 && ((args[0].Trim().ToLower() == "/configure") || (args[0].Trim().ToLower() == "configure")))
                 {
                    log = new Log("configure");
                    log.Mode = "configure";
                    log.Write("Inventory Notification Scheduler Application started");
                    Program.log.Write("Application running in configuration mode");
                    NotificationScheduler ConfigurationForm = new NotificationScheduler();
                    Application.Run(ConfigurationForm);
                }
                else
                {
                    log = new Log();
                    log.Mode = "schedule";
                    log.Write("Inventory Notification Scheduler Application started");
                    Program.log.Write("Syncronization  - started");
                    Scheduler oScheduler = new Scheduler();
                    oScheduler.Start();
                    Program.log.Write("Syncronization - completed");                                                     
                    
                }
                if(Program.log == null) log = new Log();
                Program.log.Write("Application completed");
                Program.log.Close();               

            }
            catch (Exception ex)
            {
                if (Program.log == null) log = new Log();
                Program.log.Write(ex);
                Program.log.Write("Application aborted");
                Program.log.Close();                
            }
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Program.log.Write(e.Exception);
            Environment.Exit(-2);
        }      
    }
}
