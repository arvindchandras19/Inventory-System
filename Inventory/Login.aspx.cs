#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   Login.aspx.cs
'' Type      :   C# File
'' Description  :Entered user to login
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Dhanasekran.C		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class Login : System.Web.UI.Page
    {
        InventoryServiceClient lclsService = new InventoryServiceClient();
        Page_Controls defaultPageControls = new Page_Controls();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCorporateMaster();
                BindFacility();
            }
        }
        private void BindCorporateMaster()
        {
            drpcorp.DataSource = lclsService.GetCorporateMaster().ToList();
            drpcorp.DataTextField = "CorporateName";
            drpcorp.DataValueField = "CorporateID";
            drpcorp.DataBind();
            ListItem lst = new ListItem();
            lst.Value = "0";
            lst.Text = "--Select Corporate--";
            drpcorp.Items.Insert(0, lst);
            drpcorp.SelectedIndex = 0;
        }
        private void BindFacility()
        {
            drpfacility.DataSource = lclsService.GetCorporateFacility(Convert.ToInt64(drpcorp.SelectedValue)).Where(a=>a.IsActive==true).ToList();
            drpfacility.DataTextField = "FacilityDescription";
            drpfacility.DataValueField = "FacilityID";
            drpfacility.DataBind();
            ListItem lst = new ListItem();
            lst.Value = "0";
            lst.Text = "--Select Facility--";
            drpfacility.Items.Insert(0, lst);
            drpfacility.SelectedIndex = 0;
        }
        //private void BindFacility()
        //{
        //    drpfacility.DataSource = lclsService.GetFacility().ToList();
        //    drpfacility.DataTextField = "FacilityName";
        //    drpfacility.DataValueField = "FacilityID";
        //    drpfacility.DataBind();
        //    ListItem lst = new ListItem();
        //    lst.Value = "0";
        //    lst.Text = "--Select Facility--";
        //    drpfacility.Items.Insert(0, lst);
        //    drpfacility.SelectedIndex = 0;
        //}

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            InventoryServiceClient lclsService = new InventoryServiceClient();
            BALUser lclsUser = new BALUser();
            lclsUser.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
            lclsUser.UserName = txtemail.Text;
            lclsUser.Password = txtpwd.Text;
            lclsUser.CorporateID = Convert.ToInt64(drpcorp.SelectedValue);
            List<GetLoginDetails> lstloginuser = lclsService.GetLoginDetails(lclsUser).ToList();

            if (lstloginuser.Count != 0)
            {

                defaultPageControls.UserId = lstloginuser[0].UserID;
                defaultPageControls.UserName = lstloginuser[0].UserName;
                defaultPageControls.FacilityID = Convert.ToInt64(lstloginuser[0].FacilityID);
                defaultPageControls.RoleID = Convert.ToInt64(lstloginuser[0].RoleID);
                defaultPageControls.UserRoleName = lstloginuser[0].UserRoleName;
                defaultPageControls.CorporateID = Convert.ToInt64(lstloginuser[0].CorporateID);
                List<GetUserPagePermission> lstPermission = lclsService.GetLoginPermissionDetails(defaultPageControls.RoleID).ToList();


                for (var i = 0; i < lstPermission.Count; i++)
                {
                    switch (Convert.ToInt32(lstPermission[i].MainMenuID))
                    {
                        case 1:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 1:
                                    defaultPageControls.Req_MedicalSuppliesPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_MedicalSuppliesPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 2:
                                    defaultPageControls.Req_WorkOrServicePage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_WorkOrServicePage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 3:
                                    defaultPageControls.Req_MachinePartsPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_MachinePartsPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 4:
                                    defaultPageControls.Req_RequestITPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_RequestITPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 5:
                                    defaultPageControls.CapitalItemRequest_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CapitalItemRequest_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;

                        case 2:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 5:
                                    defaultPageControls.Rec_MedicalSuppliesPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Rec_MedicalSuppliesPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 6:
                                    defaultPageControls.Rec_WorkOrServicePage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Rec_WorkOrServicePage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 7:
                                    defaultPageControls.Rec_MachinePartsPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Rec_MachinePartsPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 8:
                                    defaultPageControls.CapitalReceiving_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CapitalReceiving_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 30:
                                    defaultPageControls.ITReceiving_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.ITReceiving_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;

                        case 3:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 9:
                                    defaultPageControls.StocksOrInventory_Ending_Page_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.StocksOrInventory_Ending_Page_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 10:
                                    defaultPageControls.StocksOrInventory_Daily_Page_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.StocksOrInventory_Daily_Page_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;

                        case 4:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 11:
                                    defaultPageControls.CurrencyPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CurrencyPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 12:
                                    defaultPageControls.CorporatePage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CorporatePage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 13:
                                    defaultPageControls.FacilityPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.FacilityPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 14:
                                    defaultPageControls.FacilityReligionPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.FacilityReligionPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 15:
                                    defaultPageControls.ItemCategoryPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.ItemCategoryPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 16:
                                    defaultPageControls.MedicalSuppliesPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.MedicalSuppliesPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 17:
                                    defaultPageControls.UomMaster_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.UomMaster_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 18:
                                    defaultPageControls.AddUserPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.AddUserPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 19:
                                    defaultPageControls.UserPermission_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.UserPermission_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 20:
                                    defaultPageControls.VendorPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.VendorPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                //case 21:
                                //    defaultPageControls.VendorFacilityActPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                //    defaultPageControls.VendorFacilityActPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                //    break;
                                case 22:
                                    defaultPageControls.VendorItemMap_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.VendorItemMap_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 23:
                                    defaultPageControls.FacilityItemMap_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.FacilityItemMap_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 24:
                                    defaultPageControls.VendorFacilityActPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.VendorFacilityActPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 25:
                                    defaultPageControls.VendorOrderDue_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.VendorOrderDue_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 26:
                                    defaultPageControls.FacilitySuppliesMap_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.FacilitySuppliesMap_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 27:
                                    defaultPageControls.MachineMaster_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.MachineMaster_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 28:
                                    defaultPageControls.CorpEquipmentMap_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CorpEquipmentMap_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;

                            }
                            break;

                        case 5:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 26:

                                    defaultPageControls.TransferInPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.TransferInPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 27:
                                    defaultPageControls.TransferOutPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.TransferOutPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    defaultPageControls.TransferOutPage_Email = Convert.ToBoolean(lstPermission[i].IsEmailNotification);
                                    break;
                                case 28:
                                    defaultPageControls.TransferHistoryPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.TransferHistoryPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;

                        case 6:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 28:
                                    defaultPageControls.VendorOrderDueRemainder_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.VendorOrderDueRemainder_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;

                        case 7:
                            switch (Convert.ToInt32(lstPermission[i].SubMenuID))
                            {
                                case 1:
                                    defaultPageControls.CapitalOrder_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.CapitalOrder_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 2:
                                    defaultPageControls.Req_RequestITPOPage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_RequestITPOPage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                                case 3:
                                    defaultPageControls.Req_POServicePage_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.Req_POServicePage_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;                               
                                case 4:
                                    defaultPageControls.MachinePartsOrder_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.MachinePartsOrder_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;

                                case 5:
                                    defaultPageControls.MedicalSuppliesOrder_Edit = Convert.ToBoolean(lstPermission[i].IsEdit);
                                    defaultPageControls.MedicalSuppliesOrder_View = Convert.ToBoolean(lstPermission[i].IsViewOnly);
                                    break;
                            }
                            break;
                      
                    }

                }

                Session["Permission"] = defaultPageControls;
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid UserName and Password";
            }
        }

        protected void drpcorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility();
        }
    }
}