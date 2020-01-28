using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using System.Linq;
using System.Data;

namespace Inventory
{
    public partial class Default : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
                DoAutoLogin();
        }
        private void DoAutoLogin()
        {
            InventoryServiceClient lclsService = new InventoryServiceClient();
            BALUser lcluser = new BALUser();
            //bool IsUserExist = lclsService.IsUserExist(txtUsername.Text, txtPassword.Text);
            lcluser.UserName = Context.User.Identity.Name.ToString();
            DataSet ds = new DataSet();
            ds = lclsService.GetUserCredentials(lcluser);
            if (ds.Tables.Count > 0)
            {             
                Response.Redirect("Login.aspx");
            }
        
            else
            {
                Response.Redirect(@"Account\SignOut.aspx");
            }

        }
    }
}