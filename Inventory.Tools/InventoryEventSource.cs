using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.Tools
{
    public partial class InventoryEventSource : Form
    {
        public InventoryEventSource()
        {
            InitializeComponent();
        }

        private void btneventsource_Click(object sender, EventArgs e)
        {
            try
            {
                if (!EventLog.SourceExists("Inventory"))
                {
                    EventLog.CreateEventSource("Inventory", "Inventory");
                    MessageBox.Show("Created new log \"Inventory\"");

                }
                else
                {
                    EventLog eventLog = new EventLog();
                    eventLog.Source = "Inventory";
                    eventLog.WriteEntry("Event source[Inventory] is exists."); 
                    MessageBox.Show("Event source[Inventory] is exists.");

                }

            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString());
            }
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit (); 
        }
    }
}
