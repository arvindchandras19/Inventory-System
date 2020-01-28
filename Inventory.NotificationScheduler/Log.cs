
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Inventory.NotificationScheduler
{
    public class Log
    {
        StreamWriter strmWtr;
        #region properties
        private string _mode;
        public string Mode {
             get { return _mode; }
            set { _mode = value; }
        }
        #endregion properties
        #region Constructor
        public Log()
        {
            CreatLogDirectory();
            Open("");
        }
        public Log(string mode)
        {
            CreatLogDirectory();
            Open(mode);
        }
        #endregion 

        #region private methods
        private void CreatLogDirectory()
        {
            if (Directory.Exists(Application.StartupPath + @"\Activity Log") == false)
                Directory.CreateDirectory(Application.StartupPath + @"\Activity Log");
            if (Directory.Exists(Application.StartupPath + @"\Activity Log\Configure") == false)
                Directory.CreateDirectory(Application.StartupPath + @"\\Activity Log\Configure");
        }


        private void Open(string mode)
        {
            string fileName = string.Empty;
            fileName = Application.StartupPath + "\\Activity Log\\Log_" + DateTime.Now.ToString("dd") + "_" + DateTime.Now.ToString("MMMM") + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("HH") + "_" + DateTime.Now.ToString("mm") + "_" + DateTime.Now.ToString("ss") + ".log";
            if (mode== "configure")
             fileName = Application.StartupPath + "\\Activity Log\\Configure\\Log_" + DateTime.Now.ToString("dd") + "_" + DateTime.Now.ToString("MMMM") + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("HH") + "_" + DateTime.Now.ToString("mm") + "_" + DateTime.Now.ToString("ss") + ".log";
            if (File.Exists(fileName) == false)
                strmWtr = File.CreateText(fileName);
            else
                strmWtr = File.AppendText(fileName);
        }
        
        #endregion 

        #region Public methods
        public void Write(string message)
        {
            strmWtr.WriteLine(DateTime.Today.Day.ToString() + "-" + DateTime.Today.ToString("MMMM") + "-" + DateTime.Today.Year.ToString() + " " + DateTime.Now.ToLongTimeString() + " - " + message); //KK:05-Mar-2008 change the format
        }

        public void Write(Exception ex)
        {
            Write(ex, string.Empty);
        }

        public void Write(Exception ex, string description)
        {
            StringBuilder sb = new StringBuilder();

            if (description != string.Empty)
                sb.Append("Reason for the Error : " + description);  
            sb.Append(Environment.NewLine);
            sb.Append("Error : " + ex.Message);
            sb.Append(Environment.NewLine);
            sb.Append("Detailed Error Message : " + ex.ToString());
            sb.Append(Environment.NewLine);

            Write(sb.ToString());
        }
        public void Close()
        {
            strmWtr.Close();
        }

        #endregion 
    }
}
