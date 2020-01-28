using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Inventory
{
    public partial class PrintPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string file_name = Request.QueryString["file"];
            string[] file_name_Check = file_name.Split(',');
            string chkserviceattach = string.Empty;
            if(file_name_Check.Length == 2)
            {
                chkserviceattach = file_name_Check[1].ToString();
            }
            string path = string.Empty;
            string localuploadpath = System.Configuration.ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            path = file_name_Check[0].ToString();
            if (path.Contains(localuploadpath))
            {
                path = file_name_Check[0].ToString(); 
            }
            
                       
            // Open PDF File in Web Browser 
            if (File.Exists(path))
            {
                System.Net.WebClient client = new System.Net.WebClient();
                Byte[] buffer = client.DownloadData(path);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", buffer.Length.ToString());
                    Response.BinaryWrite(buffer);

                }
                if(chkserviceattach == "")
                {
                    System.IO.File.Delete(path);
                }
                
                    
                
                Response.End();
            }
        }
    }
}