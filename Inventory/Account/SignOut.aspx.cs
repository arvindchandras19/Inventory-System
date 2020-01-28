using System;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Inventory.Inventoryserref;
using Inventory.Class;

namespace Inventory.Account
{
    public partial class SignOut : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsService = new InventoryServiceClient();
        BALActivityTracking LstActivityTrack = new BALActivityTracking();

        protected void Page_Load(object sender, EventArgs e)
        {

            defaultPage = (Page_Controls)Session["Permission"];
            if (Context.User.Identity.IsAuthenticated)
            {

                HttpContext.Current.GetOwinContext().Authentication.SignOut(
     OpenIdConnectAuthenticationDefaults.AuthenticationType,
     CookieAuthenticationDefaults.AuthenticationType);
            }

            if (Session["Permission"] != null)
            {
                
                string lstmsg = string.Empty;

                LstActivityTrack.AppFeature = "Logout";
                LstActivityTrack.UserID = defaultPage.UserId;
                LstActivityTrack.FacilityID = defaultPage.FacilityID;
                LstActivityTrack.MachineID = GetIPAddress.GetMachineAddress();
                LstActivityTrack.IPAddress = GetIPAddress.GetDeviceAddress();


                lstmsg = lclsService.ActivityTracker(LstActivityTrack);
                Session["Permission"] = null;

            }           

        }
    }
}