using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Inventory.NotificationScheduler
{
    public static  class ConstantStrings
    {
        public static string ConfigXSDPath = Application.StartupPath.ToString() + @"\" + "Inventory.NotificationScheduler.xsd";
        public static string ConfigXMLPath = Application.StartupPath.ToString() + @"\" + "Inventory.NotificationScheduler.xml";
        public static string FileUploadPath = Application.StartupPath.ToString() + @"\" + "Upload";
        public static string CompletedFolder = "Completed";
        public static string OtherFolder = "Others";
    }
}
