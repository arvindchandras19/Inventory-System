using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Inventory.NotificationScheduler
{
    public partial class NotificationScheduler : Form
    {
        public NotificationScheduler()
        {
            InitializeComponent();
        }

        private void NotificationScheduler_Load(object sender, EventArgs e)
        {
            try
            {                
                ReadConfigXML();               
            }
            catch(Exception ce)
            {
                MessageBox.Show(ce.Message );
            }

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            /* llstxml.DBServer = txtdbserver.Text;
             llstxml.DBName = txtdbname.Text;
             llstxml.DBUser = txtdbuser.Text;
             llstxml.DBPassword = txtdbpassword.Text;
             llstxml.AdminEmailAddress = txtadminemailadd.Text;
             llstxml.ErrEmailAddress = txterroremailadd.Text;

             string tempStr = SerializeObject<DBServiceConfigConfig>(llstxml);
             DBServiceConfigConfig tempProjects = DeserializeObject<DBServiceConfigConfig>(tempStr);*/

            try
            {   
                SaveConfigXML();
                MessageBox.Show("Saved Successfully");

            }
            catch(Exception ce)
            {
                MessageBox.Show(ce.Message);
            }


        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //ReadConfigXML();
            Application.Exit(); 
           /* string Appserver = string.Empty;

            using (FileStream xmlStream = new FileStream("D:\\TFS\\Inventory System\\Inventory.NotificationScheduler\\Inventory.NotificationScheduler.xml", FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DBServiceConfigConfig));
                    DBServiceConfigConfig deserializedStudents = serializer.Deserialize(xmlReader) as DBServiceConfigConfig;
                    Appserver = "DBServer" + deserializedStudents.DBServer + "DBName" + deserializedStudents.DBName + "DBUser" + deserializedStudents.DBUser + "DBPassword" +
                            deserializedStudents.DBPassword + "AdminEmailAddress" + deserializedStudents.AdminEmailAddress;
                }
            }

            MessageBox.Show(Appserver.ToString());*/

            //DBServiceConfig llstxml = new DBServiceConfig();
            //string tempStr = SerializeObject<DBServiceConfig>(llstxml);
            //DBServiceConfig tempProjects = DeserializeObject<DBServiceConfig>(tempStr);
            //NotificationScheduler notifysche = new NotificationScheduler();
            //notifysche.Hide();
        }
        private void ReadConfigXML()
        {
            DBServiceConfig objXmlConfig = new DBServiceConfig();
            Exception ce = new Exception();
            DBServiceConfig.LoadFromFile(ConstantStrings.ConfigXMLPath, out objXmlConfig, out ce);
            if (objXmlConfig != null)
            {
                txtdbserver.Text = objXmlConfig.DBServer;
                txtdbname.Text = objXmlConfig.DBName;
                txtdbuser.Text = objXmlConfig.DBUser;
                txtdbpassword.Text = objXmlConfig.DBPassword;
                txtadminemailadd.Text  = objXmlConfig.AdminEmailAddress;
                txterroremailadd.Text = objXmlConfig.ErrEmailAddress;                
                LastAccessed = objXmlConfig.LastAccessed;
                txtSMTPServer.Text = objXmlConfig.SMTPServer;
                txtSMTPSSL.Text = objXmlConfig.SMTPSSL;
                txtSMTPPort.Text = objXmlConfig.SMTPPort;
                txtSMTPSenderEmail.Text = objXmlConfig.SMTPSenderEmail;
                txtSMTPSenderPwd.Text = objXmlConfig.SMTPSenderPwd;
            }
            objXmlConfig = null;
            ce = null;
        }
        private void SaveConfigXML()
        {
            DBServiceConfig objXmlConfig = new DBServiceConfig();
            Exception ce = new Exception();
            objXmlConfig.DBServer = txtdbserver.Text  ;
            objXmlConfig.DBName = txtdbname.Text  ;
            objXmlConfig.DBUser = txtdbuser.Text ;
            objXmlConfig.DBPassword = txtdbpassword.Text;
            objXmlConfig.AdminEmailAddress = txtadminemailadd.Text ;
            objXmlConfig.ErrEmailAddress = txterroremailadd.Text;
            //Server Mail Info
            objXmlConfig.SMTPServer = txtSMTPServer.Text;
            objXmlConfig.SMTPSSL = txtSMTPSSL.Text;
            objXmlConfig.SMTPPort = txtSMTPPort.Text;
            objXmlConfig.SMTPSenderEmail = txtSMTPSenderEmail.Text;
            objXmlConfig.SMTPSenderPwd = txtSMTPSenderPwd.Text;
                        
            if (LastAccessed == null || LastAccessed == "")
                objXmlConfig.LastAccessed = "";
            else
                objXmlConfig.LastAccessed = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); //MB:29Dec2009
            objXmlConfig.SaveToFile(ConstantStrings.ConfigXMLPath, out ce);
            objXmlConfig = null;
            ce = null;
        }
        #region Properties

       
        string _LastAccessed;
        private string LastAccessed
        {
            get { return _LastAccessed; }
            set { _LastAccessed = value; }
        }

        #endregion
       
    }
}
