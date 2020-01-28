using Inventory.Class;
using Inventory.Inventoryserref;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<Facility Vendo Account>>
'' Type      :   C# File
'' Description  :<<To add,update the Facility Vendor Account Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				   Dhanasekaran.c	                  New
'' 	08/09/2017		   V1.0				   Dhanasekaran.c	                  New Menu FacilitySupplierMap is added.
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class Inven : System.Web.UI.MasterPage
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsService = new InventoryServiceClient();
        BALActivityTracking LstActivityTrack = new BALActivityTracking();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                defaultPage = (Page_Controls)Session["Permission"];

                if (Session["Permission"] != null)
                {
                    lblUserN1.Text = "Welcome : " + defaultPage.UserName;
                    lbldate.Text = "Login Time : " + DateTime.Now;
                    lblUserN2.Text = defaultPage.UserName + " - " + defaultPage.UserRoleName;


                    string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                    System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
                    string sRet = oInfo.Name;

                    string[] Filename = sRet.Split('.');

                    if (Filename.Length > 0)
                    {
                        string PageName = Filename[0].ToString();
                        LstActivityTrack.AppFeature = PageName;
                    }

                    string lstmsg = string.Empty;

                    //LstActivityTrack.UserID = defaultPage.UserId;
                    //LstActivityTrack.FacilityID = defaultPage.FacilityID;
                    //LstActivityTrack.MachineID = GetIPAddress.GetMachineAddress();
                    //LstActivityTrack.IPAddress = GetIPAddress.GetDeviceAddress();
                    //lstmsg = lclsService.ActivityTracker(LstActivityTrack);

                }
                else
                {
                    Server.Transfer("Login.aspx");
                }
            }
        }


        protected void btnsignout_Click(object sender, EventArgs e)
        {
            Server.Transfer("Login.aspx");
        }


    }
}