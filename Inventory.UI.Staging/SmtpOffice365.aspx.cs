using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Inventory.UI.Staging
{
    public partial class SmtpOffice365 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsend_Click(object sender, EventArgs e)
        {
            try
            {
                EmailController obj = new EmailController();
            obj.SendEmailTransferOut("procurement@ppghealthcare.com", "dhanasekar@kalmango.com", "Test Content", "test Subject");
                lblError.Text = "Mail Sent";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.ToString();
            }
        }
        protected void btnsendAttach_Click(object sender, EventArgs e)
        {
            try
            {
                EmailController obj = new EmailController();
                obj.SendEmail("procurement@ppghealthcare.com", "dhanasekar@kalmango.com", "Test Content", "test Subject",@"D:\workfolder\testdata.pdf");
                lblError.Text = "Mail Sent";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.ToString();
            }
        }
    }
}