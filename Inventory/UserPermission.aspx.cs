#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using Inventory.Class;
using System.Data;
using System.Text;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   User Permission
'' Type      :   C# File
'' Description  :To add,update User Permission Details
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Dhanasekran.C		                  New
''  08/16/2017         V1.0                Vivekanand.S                 Check the dupicates and existing record in DataBase while Save the Fields
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class UserPermission : System.Web.UI.Page
    {
        InventoryServiceClient lclsService = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        string NotChecked = string.Empty;
        string ErrorMessage = string.Empty;
        string ErrorList = string.Empty;
        string MandatoryErrorMessage = string.Empty;
        string ApprovalPermission = string.Empty;
        private string _sessionPDFFileName;
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msg = Constant.UserPerMandatoryWarningMsg.Replace("ShowwarningPopup('", "").Replace("');", "");
        string logmsg = Constant.UserPerRepeatWarningMsg.Replace("ShowwarningPopup('", "").Replace("');", "");
        string lgmsg = Constant.UseractioncheckboxWarningMsg.Replace("ShowwarningPopup('", "").Replace("');", "");
        string lmsg = Constant.UserPermissionSaveMessage.Replace("ShowPopup('", "").Replace("');", ""); 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    BindMainMenu();
                    BindPermissionRole();

                    //if (defaultPage.VendorPage_Edit == false && defaultPage.VendorPage_View == true)
                    //{
                    //    //SaveCancel.Visible = false;
                    //    //btnsave.Visible = false;

                    //}
                    if (defaultPage != null)
                    {
                        if (defaultPage.UserPermission_Edit == false && defaultPage.UserPermission_View == false)
                        {

                            permission.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            try
            {
                string req = Constant.RequiredFieldValidator;
                ReqfielddrpMainmenu.ErrorMessage = req;
                Reqfielddrpsubmenu.ErrorMessage = req;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        public void BindPermissionRole()
        {
            try
            {
                List<BindPermission> lstrolefacility = lclsService.BindPermission().ToList();
                GrdUserPermissionMaster.DataSource = lstrolefacility;
                GrdUserPermissionMaster.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        public void BindUserApprovePermission()
        {
            try
            {
                List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                GrdUserPermissionMaster.DataSource = llstUserApprovePermission;
                GrdUserPermissionMaster.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        public void GETPermissionRole()
        {
            try
            {
                int i = 0;
                Int64 MainMenu = Convert.ToInt64(drpMainmenu.SelectedValue);
                Int64 SubMenu = Convert.ToInt64(drpsubmenu.SelectedValue);
                List<GetPermission> lstrolefacility = lclsService.GetPermission(MainMenu, SubMenu).ToList();
                hdnfield.Value = "";
                if (lstrolefacility.Count > 0)
                {
                    foreach (GridViewRow row in GrdUserPermissionMaster.Rows)
                    {
                        try
                        {
                            if (lstrolefacility[i].IsEdit == false)
                            {
                                CheckBox IsEdit = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsEdit") as CheckBox;
                                IsEdit.Checked = false;
                            }
                            else
                            {
                                CheckBox IsEdit = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsEdit") as CheckBox;
                                IsEdit.Checked = true;
                            }
                            if (lstrolefacility[i].IsViewOnly == false)
                            {
                                CheckBox IsView = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsViewOnly") as CheckBox;
                                IsView.Checked = false;
                            }
                            else
                            {
                                CheckBox IsView = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsViewOnly") as CheckBox;
                                IsView.Checked = true;
                            }
                            if (lstrolefacility[i].IsEmailNotification==false)
                            {
                                CheckBox IsEmailNotification = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsEmailNotification") as CheckBox;
                                IsEmailNotification.Checked = false;
                            }
                            else
                            {
                                CheckBox IsEmailNotification = GrdUserPermissionMaster.Rows[row.RowIndex].FindControl("IsEmailNotification") as CheckBox;
                                IsEmailNotification.Checked = true;
                            }
                            //GrdUserPermissionMaster.DataSource = lstrolefacility[0].IsEdit;
                            //GrdUserPermissionMaster.DataBind();
                            i++;
                        }
                        catch (Exception ex)
                        {
                            EventLogger log = new EventLogger(config);
                            log.LogException(ex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
                        }

                    }
                }
                else
                {
                    BindPermissionRole();
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        private void BindMainMenu()
        {
            try
            {
                List<GetdrpMainMenu> lstmainmenu = lclsService.GetdrpMainMenu().ToList();
                drpMainmenu.DataSource = lstmainmenu;
                drpMainmenu.DataTextField = "PageMainMenu";
                drpMainmenu.DataValueField = "PageMainMenuID";
                drpMainmenu.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpMainmenu.Items.Insert(0, lst);
                drpMainmenu.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }

        }
        private void BindSubMenu()
        {
            try
            {
                drpsubmenu.DataSource = lclsService.GetdrpSubMenu(Convert.ToInt64(drpMainmenu.SelectedValue)).ToList();
                drpsubmenu.DataTextField = "PageSubMenu";
                drpsubmenu.DataValueField = "PageSubMenuID";
                drpsubmenu.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpsubmenu.Items.Insert(0, lst);
                drpsubmenu.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        //public void BindChoosenUserRoleName(int i, string llstUserID, Int64 PermissionID)
        //{
        //    try
        //    {
        //        //ListItem lst = new ListItem();
        //        //lst.Value = "0";
        //        //lst.Text = "--Select User--";
        //        List<BindMultiRolesPermission> lstUserRoleName = new List<BindMultiRolesPermission>();

        //        if (GrdUserRoleApprove.FooterRow.Visible == true && i == 0)
        //        {
        //            //ListBox drpFooterChoosenUser = GrdUserRoleApprove.FooterRow.FindControl("footdrpchoosenUserRole") as ListBox;
        //            if (drpMainmenu.SelectedItem.Text == "Generate/Approve order" && drpsubmenu.SelectedItem.Text == "Service")
        //            {
        //                //DropDownList drpFooterChoosenUser = GrdUserRoleApprove.FooterRow.FindControl("footdrpchoosenUserRole") as DropDownList;
        //                // Insert Drop Down                
        //                //lstUserRoleName = lclsService.BindMultiRolesPermission(llstUserID, PermissionID).ToList();
        //                //drpFooterChoosenUser.DataSource = lstUserRoleName;
        //                //drpFooterChoosenUser.DataTextField = "UserRole";
        //                //drpFooterChoosenUser.DataValueField = "UserRoleID";
        //                //drpFooterChoosenUser.DataBind();
        //                //dropChoosenUser.Items.Insert(0, lst);
        //                //dropChoosenUser.SelectedIndex = 0;
        //            }

        //        }
        //        else
        //        {
        //            //ListBox dropChoosenUser = (ListBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("drpchoosenUserRole");

        //            //DropDownList dropChoosenUser = (DropDownList)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("drpchoosenUserRole");
        //            if (drpMainmenu.SelectedItem.Text == "Generate/Approve order" && drpsubmenu.SelectedItem.Text == "Service")
        //            {
        //                // Insert Drop Down                
        //                lstUserRoleName = lclsService.BindMultiRolesPermission(llstUserID, PermissionID).ToList();
        //                dropChoosenUser.DataSource = lstUserRoleName;
        //                dropChoosenUser.DataTextField = "UserRole";
        //                dropChoosenUser.DataValueField = "UserRoleID";
        //                dropChoosenUser.DataBind();
        //                //dropChoosenUser.Items.Insert(0, lst);
        //                //dropChoosenUser.SelectedIndex = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public void BindApprovePermissionGrid(List<GetUserApprovePermission> argApprovePermission)
        {
            try
            {
                int k = 0;
                string ShowMultiSelect = string.Empty;
                string llstUserID = string.Empty;
                GrdUserRoleApprove.DataSource = argApprovePermission;
                GrdUserRoleApprove.DataBind();
                List<GetUserApprovePermission> llstUseCount = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                for (int i = 0; i < argApprovePermission.Count; i++)
                {
                    Label Box1 = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblUserRoleName");
                    Label Box2 = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblUserRoleId");
                    CheckBox IsEdit = (CheckBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("IsEdit");
                    CheckBox IsView = (CheckBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("IsView");
                    CheckBox IsApprove = (CheckBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("IsApprove");
                    CheckBox IsOrder = (CheckBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("IsOrder");
                    CheckBox IsDeny = (CheckBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("IsDeny");
                    TextBox txtApproveRangeFrom = (TextBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("txtApprovalFrom");
                    TextBox txtApproveRangeTo = (TextBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("txtApprovalTo");
                    LinkButton lkbmultiapprove = (LinkButton)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lkbmultiapprove");
                    Label lblPermissionID = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblPermissionID");
                    Label lblhddListUserID = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblhddListUserID");
                    //ListBox drpchoosenUserRole = (ListBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("drpchoosenUserRole");

                    Box1.Text = argApprovePermission[i].UserRole;
                    Box2.Text = argApprovePermission[i].UserRoleID.ToString();
                    lblPermissionID.Text = argApprovePermission[i].PermissionID.ToString();
                    if (argApprovePermission[i].IsEdit == true)
                    {
                        IsEdit.Checked = true;
                    }
                    else
                    {
                        IsEdit.Checked = false;
                    }
                    ///////////////
                    if (argApprovePermission[i].IsViewOnly == true)
                    {
                        IsView.Checked = true;
                    }
                    else
                    {
                        IsView.Checked = false;
                    }
                    /////////
                    if (argApprovePermission[i].IsApprove == true)
                    {
                        IsApprove.Checked = true;
                    }
                    else
                    {
                        IsApprove.Checked = false;
                    }
                    /////////
                    if (argApprovePermission[i].IsOrder == true)
                    {
                        IsOrder.Checked = true;
                    }
                    else
                    {
                        IsOrder.Checked = false;
                    }
                    /////////
                    if (argApprovePermission[i].IsDeny == true)
                    {
                        IsDeny.Checked = true;
                    }
                    else
                    {
                        IsDeny.Checked = false;
                    }
                    txtApproveRangeFrom.Text = Convert.ToString(string.Format("{0:F2}", argApprovePermission[i].ApproveRangeFrom));
                    txtApproveRangeTo.Text = Convert.ToString(string.Format("{0:F2}", argApprovePermission[i].ApproveRangeTo));


                    //ListBox drpchoosenUserRole = (ListBox)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("drpchoosenUserRole");



                    for (int j = 0; j < llstUseCount.Count; j++)
                    {
                        if (j != k)
                        {
                            //TextBox ApproveRangeFrom = (TextBox)GrdUserRoleApprove.Rows[j].Cells[1].FindControl("txtApprovalFrom");
                            //TextBox ApproveRangeTo = (TextBox)GrdUserRoleApprove.Rows[j].Cells[1].FindControl("txtApprovalTo");
                            TextBox KApproveRangeFrom = (TextBox)GrdUserRoleApprove.Rows[k].Cells[1].FindControl("txtApprovalFrom");
                            TextBox KApproveRangeTo = (TextBox)GrdUserRoleApprove.Rows[k].Cells[1].FindControl("txtApprovalTo");
                            if (llstUseCount[j].ApproveRangeFrom == Convert.ToDecimal(KApproveRangeFrom.Text) && llstUseCount[j].ApproveRangeTo == Convert.ToDecimal(KApproveRangeTo.Text))
                            {
                                if (Convert.ToDecimal(KApproveRangeFrom.Text) != 0 && Convert.ToDecimal(KApproveRangeTo.Text) != 0)
                                {
                                    ShowMultiSelect = "Show";
                                    llstUserID += llstUseCount[j].UserRoleID.ToString() + ",";
                                }
                            }
                        }
                    }

                    if (llstUserID != "")
                        llstUserID = Box2.Text + "," + llstUserID.ToString().Substring(0, (llstUserID.Length - 1));

                    lblhddListUserID.Text = llstUserID;


                    List<GetMultiUserApprove> llstmultiuser = new List<GetMultiUserApprove>();
                    llstmultiuser = lclsService.GetMultiUserApprove(Convert.ToInt64(lblPermissionID.Text)).ToList();

                    if (ShowMultiSelect != "Show")
                    {
                        lkbmultiapprove.Visible = false;

                    }


                    k++;
                    ShowMultiSelect = "";
                    llstUserID = "";
                }
                hdnfield.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnMultiroleApproveClose_Click(object sender, EventArgs e)
        {
            try
            {
                DivMultiroleApprove.Style.Add("display", "none");
                permission.Attributes["class"] = "mypanel-body";
                List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                BindApprovePermissionGrid(llstUserApprovePermission);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnMultiroleApproveAssign_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALUser lllstuserinfo = new BALUser();
                string InsertMsg = string.Empty;
                string ErrorMsg = string.Empty;
                foreach (GridViewRow row in GrdMultiroleApprove.Rows)
                {
                    TextBox txtactionorder = (TextBox)row.FindControl("txtactionorder");

                    foreach (GridViewRow row1 in GrdMultiroleApprove.Rows)
                    {
                        TextBox txtactionorder1 = (TextBox)row1.FindControl("txtactionorder");
                        CheckBox chkmultirole = (CheckBox)row1.FindControl("chkmultirole");
                        if (txtactionorder1.Text != "")
                            chkmultirole.Checked = true;
                        else
                            chkmultirole.Checked = false;
                        if (row.RowIndex != row1.RowIndex)
                        {
                            if (txtactionorder.Text != "" && txtactionorder1.Text != "")
                            {
                                if (Convert.ToInt32(txtactionorder.Text) == Convert.ToInt32(txtactionorder1.Text))
                                {
                                    ErrorMsg = "Not Unique";
                                }
                            }
                        }
                    }

                }
                if (ErrorMsg == "")
                {
                    foreach (GridViewRow row in GrdMultiroleApprove.Rows)
                    {
                        CheckBox chkmultirole = (CheckBox)row.FindControl("chkmultirole");
                        Label lblroleid = (Label)row.FindControl("lblroleid");
                        Label lblrole = (Label)row.FindControl("lblrole");
                        TextBox txtactionorder = (TextBox)row.FindControl("txtactionorder");
                        Label lblMultiPermissionID = (Label)row.FindControl("lblMultiPermissionID");
                        Label lblPageMasterPermissionMultiRoleID = (Label)row.FindControl("lblPageMasterPermissionMultiRoleID");
                        Label lblApprovefrom = (Label)row.FindControl("lblApprovefrom");
                        Label lblApproveto = (Label)row.FindControl("lblApproveto");
                        //Label lblrole = (Label)row.FindControl("lblrole");
                        if (txtactionorder.Text != "")
                            chkmultirole.Checked = true;
                        else
                            chkmultirole.Checked = false;
                        if (chkmultirole.Checked == true)
                        {
                            lllstuserinfo.IsActive = true;
                        }
                        else
                        {
                            lllstuserinfo.IsActive = false;
                        }

                        lllstuserinfo.UserRoleID = Convert.ToInt64(lblroleid.Text);
                        if (txtactionorder.Text != "")
                            lllstuserinfo.Approveorder = Convert.ToInt32(txtactionorder.Text);
                        else
                            lllstuserinfo.Approveorder = 0;

                        lllstuserinfo.PermissionID = Convert.ToInt64(lblMultiPermissionID.Text);
                        if (lblPageMasterPermissionMultiRoleID.Text == "0")
                        {
                            lllstuserinfo.CreatedBy = defaultPage.UserId;
                            lllstuserinfo.ApproveRangeFrom = Convert.ToDecimal(lblApprovefrom.Text);
                            lllstuserinfo.ApproveRangeTo = Convert.ToDecimal(lblApproveto.Text);

                            InsertMsg = lclsService.InsertMultiPermissionApprove(lllstuserinfo);
                        }
                        else
                        {
                            lllstuserinfo.LastModifiedBy = defaultPage.UserId;
                            lllstuserinfo.PageMasterPermissionMultiRoleID = Convert.ToInt64(lblPageMasterPermissionMultiRoleID.Text);
                            InsertMsg = lclsService.UpdateMultiPermissionApprove(lllstuserinfo);
                        }
                    }

                    if (InsertMsg == "Saved Successfully" || InsertMsg == "Updated Successfully")
                    {
                        string msg = Constant.UserPermissionMultiRoelSaveMessage.Replace("ShowPopup('", "").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionMultiRoelSaveMessage, true);
                        if (drpMainmenu.SelectedValue == "7")
                        {
                            List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                            BindApprovePermissionGrid(llstUserApprovePermission);
                        }
                        DivMultiroleApprove.Style.Add("display", "none");
                        permission.Attributes["class"] = "mypanel-body";
                    }
                }
                else
                {
                    string msg = Constant.UserPerActionOrderWarningMsg.Replace("ShowwarningPopup('", "").Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerActionOrderWarningMsg, true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void lkbmultiapprove_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                DivMultiroleApprove.Style.Add("display", "block");
                permission.Attributes["class"] = "Upopacity";

                Label lblPermissionID = (Label)gvrow.FindControl("lblPermissionID");
                Label lblhddListUserID = (Label)gvrow.FindControl("lblhddListUserID");

                TextBox txtApprovalFrom = (TextBox)gvrow.FindControl("txtApprovalFrom");
                TextBox txtApprovalTo = (TextBox)gvrow.FindControl("txtApprovalTo");
                List<GetMultiUserApprove> llstmultiuser = new List<GetMultiUserApprove>();
                llstmultiuser = lclsService.GetMultiUserApprove(Convert.ToInt64(lblPermissionID.Text)).ToList();

                if (llstmultiuser.Count > 0)
                {
                    GrdMultiroleApprove.DataSource = llstmultiuser;
                    GrdMultiroleApprove.DataBind();
                    foreach (GridViewRow row in GrdMultiroleApprove.Rows)
                    {
                        CheckBox chkmultirole = (CheckBox)row.FindControl("chkmultirole");
                        Label lblrole = (Label)row.FindControl("lblrole");
                        Label lblroleid = (Label)row.FindControl("lblroleid");
                        TextBox txtactionorder = (TextBox)row.FindControl("txtactionorder");
                        Label lblMultiPermissionID = (Label)row.FindControl("lblMultiPermissionID");
                        Label lblPageMasterPermissionMultiRoleID = (Label)row.FindControl("lblPageMasterPermissionMultiRoleID");
                        if (txtactionorder.Text != "")
                            chkmultirole.Checked = true;
                        if (llstmultiuser[row.RowIndex].IsActive == true)
                        {
                            chkmultirole.Checked = true;
                        }
                        lblroleid.Text = llstmultiuser[row.RowIndex].UserRoleID.ToString();
                        lblrole.Text = llstmultiuser[row.RowIndex].UserRole.ToString();
                        if (llstmultiuser[row.RowIndex].ApproverOrder == 0)
                            txtactionorder.Text = "";
                        else
                            txtactionorder.Text = llstmultiuser[row.RowIndex].ApproverOrder.ToString();
                        lblMultiPermissionID.Text = llstmultiuser[row.RowIndex].PermissionID.ToString();
                        lblPageMasterPermissionMultiRoleID.Text = llstmultiuser[row.RowIndex].PageMasterPermissionMultiRoleID.ToString();
                    }
                }
                else
                {
                    List<BindMultiRolesPermission> llstmultirole = lclsService.BindMultiRolesPermission(lblhddListUserID.Text, Convert.ToInt64(lblPermissionID.Text)).ToList();
                    GrdMultiroleApprove.DataSource = llstmultirole;
                    GrdMultiroleApprove.DataBind();
                    foreach (GridViewRow row in GrdMultiroleApprove.Rows)
                    {
                        CheckBox chkmultirole = (CheckBox)row.FindControl("chkmultirole");
                        Label lblroleid = (Label)row.FindControl("lblroleid");
                        Label lblrole = (Label)row.FindControl("lblrole");
                        TextBox txtactionorder = (TextBox)row.FindControl("txtactionorder");
                        Label lblMultiPermissionID = (Label)row.FindControl("lblMultiPermissionID");
                        Label lblPageMasterPermissionMultiRoleID = (Label)row.FindControl("lblPageMasterPermissionMultiRoleID");
                        Label lblApprovefrom = (Label)row.FindControl("lblApprovefrom");
                        Label lblApproveto = (Label)row.FindControl("lblApproveto");
                        if (txtactionorder.Text != "")
                            chkmultirole.Checked = true;

                        lblroleid.Text = llstmultirole[row.RowIndex].UserRoleID.ToString();
                        lblrole.Text = llstmultirole[row.RowIndex].UserRole;
                        lblMultiPermissionID.Text = lblPermissionID.Text;
                        lblApprovefrom.Text = txtApprovalFrom.Text;
                        lblApproveto.Text = txtApprovalTo.Text;
                        lblPageMasterPermissionMultiRoleID.Text = "0";
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void drpMainmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSubMenu();
                if (drpMainmenu.SelectedValue == "7")
                {
                    DivUserPermission.Style.Add("display", "none");
                    DivUserPermissionApprove.Style.Add("display", "block");
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                    if (llstUserApprovePermission.Count > 0)
                    {
                        BindApprovePermissionGrid(llstUserApprovePermission);
                    }
                    else
                    {
                        List<BindPermission> lstrolefacility = lclsService.BindPermission().ToList();
                        GrdUserRoleApprove.DataSource = lstrolefacility;
                        GrdUserRoleApprove.DataBind();
                        for (int i = 0; i < lstrolefacility.Count; i++)
                        {
                            Label Box1 = (Label)GrdUserRoleApprove.Rows[i].FindControl("lblUserRoleName");
                            Label Box2 = (Label)GrdUserRoleApprove.Rows[i].FindControl("lblUserRoleId");
                            //ListBox drp = (ListBox)GrdUserRoleApprove.Rows[i].FindControl("drpchoosenUserRole");
                            Box1.Text = lstrolefacility[i].UserRole;
                            Box2.Text = lstrolefacility[i].UserRoleID.ToString();
                            //BindChoosenUserRoleName(i, lstrolefacility[i].UserRoleID);
                            //drp.Style.Add("Enabled", "false");
                        }
                        //dt = lstrolefacility;
                        hdnfield.Value = "0";
                    }
                    //BindUserApprovePermission();
                }
                else
                {
                    DivUserPermission.Style.Add("display", "block");
                    DivUserPermissionApprove.Style.Add("display", "none");
                    DivAddApproveLink.Style.Add("display", "none");
                    BindPermissionRole();
                    hdnfield.Value = "0";
                }

                if (drpMainmenu.SelectedValue == "7" && drpsubmenu.SelectedValue == "3")
                {
                    DivAddApproveLink.Style.Add("display", "block");
                }
                else
                {
                    DivAddApproveLink.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void lkbtnAddAcitve_Click(object sender, EventArgs e)
        {
            try
            {
                GrdUserRoleApprove.FooterRow.Visible = true;
                BindFooterDropDown();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
            //BindChoosenUserRoleName(0, 1,"");
        }

        public void BindFooterDropDown()
        {
            try
            {
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select User--";
                List<BindPermission> lstvendordetails = new List<BindPermission>();
                DropDownList drpFooterUser = GrdUserRoleApprove.FooterRow.FindControl("footdrpUserRole") as DropDownList;


                // Insert Drop Down                
                lstvendordetails = lclsService.BindPermission().Where(a => a.UserRoleID != 1).ToList();
                drpFooterUser.DataSource = lstvendordetails;
                drpFooterUser.DataTextField = "UserRole";
                drpFooterUser.DataValueField = "UserRoleID";
                drpFooterUser.DataBind();
                drpFooterUser.Items.Insert(0, lst);
                drpFooterUser.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                drpMainmenu.ClearSelection();
                drpsubmenu.ClearSelection();
                DivUserPermission.Style.Add("display", "block");
                DivUserPermissionApprove.Style.Add("display", "none");
                DivAddApproveLink.Style.Add("display", "none");
                BindPermissionRole();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void drpsubmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (drpMainmenu.SelectedItem.Text == "Transfers" && drpsubmenu.SelectedItem.Text == "Transfer Out")
                {
                     BindPermissionRole(); 
                }
                if (drpMainmenu.SelectedItem.Text == "Transfers" && drpsubmenu.SelectedItem.Text == "Transfer In")
                {
                    BindPermissionRole();
                }
                if (drpMainmenu.SelectedItem.Text == "Transfers" && drpsubmenu.SelectedItem.Text == "Transfer History")
                {
                    BindPermissionRole();
                } 
                if (drpMainmenu.SelectedItem.Text != "--Select--")
                {
                    List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                    if (drpMainmenu.SelectedValue == "7")
                    {
                        if (llstUserApprovePermission.Count > 0)
                        {
                            BindApprovePermissionGrid(llstUserApprovePermission);
                        }
                        else
                        {
                            List<BindPermission> lstrolefacility = lclsService.BindPermission().ToList();
                            GrdUserRoleApprove.DataSource = lstrolefacility;
                            GrdUserRoleApprove.DataBind();
                            for (int i = 0; i < lstrolefacility.Count; i++)
                            {
                                Label Box1 = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblUserRoleName");
                                Label Box2 = (Label)GrdUserRoleApprove.Rows[i].Cells[1].FindControl("lblUserRoleId");
                                Box1.Text = lstrolefacility[i].UserRole;
                                Box2.Text = lstrolefacility[i].UserRoleID.ToString();
                                //BindChoosenUserRoleName(i, lstrolefacility[i].UserRoleID);
                            }
                            //dt = lstrolefacility;

                        }
                    }
                    else
                    {
                        List<GetPermission> lstrolefacility = lclsService.GetPermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                        if (lstrolefacility.Count > 0)
                        {                            
                            GETPermissionRole();
                        }
                        else
                        {
                            BindPermissionRole();
                        }
                    }

                }
                else
                    GETPermissionRole();

                if (drpMainmenu.SelectedValue == "7" && drpsubmenu.SelectedValue == "3")
                {
                    DivAddApproveLink.Style.Add("display", "block");
                }
                else
                {
                    DivAddApproveLink.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        public void FooterRowSave(string lstuserper)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALUser lclsUser = new BALUser();
                string lstrMessage = string.Empty;
                //string MandatoryErrorMessage = string.Empty;
                Control control = null;
               // string NotChecked = string.Empty;

                if (GrdUserRoleApprove.FooterRow != null)
                {
                    control = GrdUserRoleApprove.FooterRow;
                }
                else
                {
                    control = GrdUserRoleApprove.Controls[0].Controls[0];
                }


                if (((CheckBox)control.FindControl("footIsEdit")).Checked)
                {
                    lclsUser.IsEdit = true;
                }
                else
                {
                    lclsUser.IsEdit = false;
                }
                if (((CheckBox)control.FindControl("footIsView")).Checked)
                {
                    lclsUser.IsView = true;
                }
                else
                {
                    lclsUser.IsView = false;
                }
                if (((CheckBox)control.FindControl("footIsApprove")).Checked)
                {
                    lclsUser.IsApprove = true;
                    TextBox txtApproveRangeFrom = (TextBox)control.FindControl("foottxtApprovalFrom");
                    TextBox txtApproveRangeTo = (TextBox)control.FindControl("foottxtApprovalTo");
                    //ListBox drpchoosenUserRole = (ListBox)control.FindControl("footdrpchoosenUserRole");

                    if (txtApproveRangeTo.Text == "" || txtApproveRangeTo.Text == "")
                    {
                        lclsUser.ApproveRangeFrom = 0;
                        lclsUser.ApproveRangeTo = 0;
                    }
                    else
                    {
                        lclsUser.ApproveRangeFrom = Convert.ToDecimal(txtApproveRangeFrom.Text);
                        lclsUser.ApproveRangeTo = Convert.ToDecimal(txtApproveRangeTo.Text);
                    }


                    //var FinalString = "";
                    //var SB = new StringBuilder();
                    //foreach (ListItem lst in drpchoosenUserRole.Items)
                    //{
                    //    if (lst.Selected)
                    //    {
                    //        SB.Append(lst.Value + ',');
                    //    }
                    //}
                    //if (SB.Length > 0)
                    //    FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    //lclsUser.MultipleRoles = FinalString;
                    lclsUser.MultipleRoles = "";

                }
                else
                {
                    lclsUser.IsApprove = false;
                    lclsUser.ApproveRangeFrom = 0;
                    lclsUser.ApproveRangeTo = 0;
                    lclsUser.MultipleRoles = "";
                }
                DropDownList footdrpUserRole = (DropDownList)control.FindControl("footdrpUserRole");

                if (((CheckBox)control.FindControl("footIsApprove")).Checked && Convert.ToDecimal(lclsUser.ApproveRangeFrom) == 0 && Convert.ToDecimal(lclsUser.ApproveRangeTo) == 0)
                {
                    MandatoryErrorMessage += footdrpUserRole.SelectedItem.Text + ",";
                }
                if (((CheckBox)control.FindControl("footIsOrder")).Checked)
                {
                    lclsUser.IsOrder = true;
                }
                else
                {
                    lclsUser.IsOrder = false;
                }
                if (((CheckBox)control.FindControl("footIsDeny")).Checked)
                {
                    lclsUser.IsDeny = true;
                }
                else
                {
                    lclsUser.IsDeny = false;
                }




                lclsUser.UserRoleID = Convert.ToInt64(footdrpUserRole.SelectedValue);
                lclsUser.MenuID = Convert.ToInt64(drpMainmenu.SelectedValue);
                lclsUser.SubMenuId = Convert.ToInt64(drpsubmenu.SelectedValue);
                lclsUser.PageName = drpsubmenu.SelectedItem.Text;
                lclsUser.CreatedBy = Convert.ToInt64(defaultPage.UserId);

                lclsUser.PermissionID = 0;
                


                foreach (GridViewRow row in GrdUserRoleApprove.Rows)
                {
                    Label lblUserRoleID = (Label)row.FindControl("lblUserRoleId");
                    Label lblUserRoleName = (Label)row.FindControl("lblUserRoleName");
                    TextBox txtApproveRangeFrom = (TextBox)row.FindControl("txtApprovalFrom");
                    TextBox txtApproveRangeTo = (TextBox)row.FindControl("txtApprovalTo");



                    if (lclsUser.UserRoleID == Convert.ToInt64(lblUserRoleID.Text))
                    {
                        if (Convert.ToDecimal(txtApproveRangeFrom.Text) <= lclsUser.ApproveRangeFrom && Convert.ToDecimal(txtApproveRangeTo.Text) >= lclsUser.ApproveRangeFrom ||
                        Convert.ToDecimal(txtApproveRangeFrom.Text) <= lclsUser.ApproveRangeTo && Convert.ToDecimal(txtApproveRangeTo.Text) >= lclsUser.ApproveRangeTo)
                        {
                            ErrorList += lblUserRoleName.Text + ",";
                        }
                    }
                    if (((CheckBox)control.FindControl("footIsApprove")).Checked == false && txtApproveRangeFrom.Text != "" && txtApproveRangeTo.Text != "")
                    {
                        NotChecked += lblUserRoleName.Text + ",";
                    }
                }
                if (ErrorList == "" && MandatoryErrorMessage == "" && footdrpUserRole.SelectedValue != "0" && NotChecked == "")
                {
                    lstrMessage = lclsService.InsertUserApprovePermission(lclsUser);
                }
                else
                {
                    if (MandatoryErrorMessage != "")
                    {
                        MandatoryErrorMessage = MandatoryErrorMessage.ToString().Substring(0, (MandatoryErrorMessage.Length - 1));
                        log.LogWarning(msg.Replace("<<UserPermission>>", MandatoryErrorMessage));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerMandatoryWarningMsg.Replace("<<UserPermission>>", MandatoryErrorMessage), true);

                    }
                    else if (ErrorList != "")
                    {
                        ErrorList = ErrorList.ToString().Substring(0, (ErrorList.Length - 1));
                        log.LogWarning(logmsg.Replace("<<UserPermission>>", ErrorList));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerRepeatWarningMsg.Replace("<<UserPermission>>", ErrorList), true);
                    }
                    else if (footdrpUserRole.SelectedValue == "0")
                    {
                        string msg = Constant.UserPerEmptyWarningMsg.Replace("ShowwarningPopup('", "").Replace("<<UserPermission>>", "User not selected").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerEmptyWarningMsg.Replace("<<UserPermission>>", "User not selected"), true);
                    }
                    else if (NotChecked != "")
                    {
                        NotChecked = NotChecked.ToString().Substring(0, (NotChecked.Length - 1));
                        log.LogWarning(lgmsg.Replace("<<UserPermission>>", NotChecked));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UseractioncheckboxWarningMsg.Replace("<<UserPermission>>", NotChecked), true);
                    }
                    BindFooterDropDown();
                    GrdUserRoleApprove.FooterRow.Visible = true;
                }

                //if (MandatoryErrorMessage != "")
                //{
                //    MandatoryErrorMessage = MandatoryErrorMessage.ToString().Substring(0, (MandatoryErrorMessage.Length - 1));
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerMandatoryWarningMsg.Replace("<<UserPermission>>", MandatoryErrorMessage), true);
                //}
                //else if (ErrorMessage != "")
                //{
                //    ErrorMessage = ErrorMessage.ToString().Substring(0, (ErrorMessage.Length - 1));
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerRepeatWarningMsg.Replace("<<UserPermission>>", ErrorMessage), true);
                //}


                if (lstrMessage == "Saved Successfully" && NotChecked == "")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);
                    log.LogInformation(lmsg.Replace("<<UserPermission>>", lstuserper.ToString()));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionSaveMessage.Replace("<<UserPermission>>", lstuserper.ToString()), true);
                    List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                    BindApprovePermissionGrid(llstUserApprovePermission);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                string lstuserper = string.Empty;
                lstuserper = drpMainmenu.SelectedItem.Text + " " + drpsubmenu.SelectedItem.Text;
                string lstrMessage = string.Empty;
               
                
                int i = 0;
                int k = 0;
                BALUser lclsUser = new BALUser();
                List<object> lstuserrolename = new List<object>();

                if (drpMainmenu.SelectedValue == "7")
                {
                    if (GrdUserRoleApprove.FooterRow.Visible == true)
                    {
                        FooterRowSave(lstuserper.ToString());
                    }
                    else
                    {
                        List<BindPermission> lstrole = lclsService.BindPermission().ToList();
                        List<GetPermission> lstrolefacility = lclsService.GetPermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();

                        foreach (GridViewRow row in GrdUserRoleApprove.Rows)
                        {
                            CheckBox IsApprove = (CheckBox)row.FindControl("IsApprove");
                            Label lblUserRoleName = (Label)row.FindControl("lblUserRoleName");
                            TextBox txtApproveRangeFrom = (TextBox)row.FindControl("txtApprovalFrom");
                            TextBox txtApproveRangeTo = (TextBox)row.FindControl("txtApprovalTo");
                            if (txtApproveRangeFrom.Text != "" && txtApproveRangeTo.Text != "")
                            {
                                if (Convert.ToDecimal(txtApproveRangeFrom.Text) > Convert.ToDecimal(txtApproveRangeTo.Text))
                                {
                                    ApprovalPermission += lblUserRoleName.Text + ",";
                                }
                                if (IsApprove.Checked == false && Convert.ToDecimal(txtApproveRangeFrom.Text) != 0 && Convert.ToDecimal(txtApproveRangeTo.Text) != 0)
                                {
                                    NotChecked += lblUserRoleName.Text + ","; 
                                }  
                            }
                            if (txtApproveRangeFrom.Text != "" || txtApproveRangeTo.Text != "")
                            {
                                if (IsApprove.Checked == false && Convert.ToDecimal(txtApproveRangeFrom.Text) != 0 && Convert.ToDecimal(txtApproveRangeTo.Text) != 0)
                                {
                                    NotChecked += lblUserRoleName.Text + ",";
                                }
                                
                            }

                            lstuserrolename.Insert(k, lblUserRoleName.Text);
                            if (((CheckBox)row.FindControl("IsApprove")).Checked && Convert.ToDecimal(txtApproveRangeFrom.Text) == 0 && Convert.ToDecimal(txtApproveRangeTo.Text) == 0)
                            {
                                MandatoryErrorMessage += lblUserRoleName.Text + ",";
                            }
                            for (int o = 0; o < lstuserrolename.Count; o++)
                            {
                                if (o != k && lstuserrolename[o].ToString() == lblUserRoleName.Text)
                                {
                                    TextBox ApproveRangeFrom = (TextBox)GrdUserRoleApprove.Rows[o].Cells[1].FindControl("txtApprovalFrom");
                                    TextBox ApproveRangeTo = (TextBox)GrdUserRoleApprove.Rows[o].Cells[1].FindControl("txtApprovalTo");
                                    TextBox KApproveRangeFrom = (TextBox)GrdUserRoleApprove.Rows[k].Cells[1].FindControl("txtApprovalFrom");
                                    TextBox KApproveRangeTo = (TextBox)GrdUserRoleApprove.Rows[k].Cells[1].FindControl("txtApprovalTo");
                                    if ((Convert.ToDecimal(ApproveRangeFrom.Text) <= Convert.ToDecimal(KApproveRangeFrom.Text) && Convert.ToDecimal(ApproveRangeTo.Text) >= Convert.ToDecimal(KApproveRangeFrom.Text))
                                        || (Convert.ToDecimal(ApproveRangeFrom.Text) <= Convert.ToDecimal(KApproveRangeTo.Text) && Convert.ToDecimal(ApproveRangeTo.Text) >= Convert.ToDecimal(KApproveRangeTo.Text)))
                                    {
                                        ErrorMessage += lblUserRoleName.Text + ",";
                                    }
                                }
                            }
                            k++;
                        }

                        if (ErrorMessage == "" && MandatoryErrorMessage == "" && ApprovalPermission == "" && NotChecked == "")
                        {
                            foreach (GridViewRow row in GrdUserRoleApprove.Rows)
                            {
                                if (((CheckBox)row.FindControl("IsEdit")).Checked)
                                {
                                    lclsUser.IsEdit = true;
                                }
                                else
                                {
                                    lclsUser.IsEdit = false;
                                }
                                if (((CheckBox)row.FindControl("IsView")).Checked)
                                {
                                    lclsUser.IsView = true;
                                }
                                else
                                {
                                    lclsUser.IsView = false;
                                }
                                if (((CheckBox)row.FindControl("IsApprove")).Checked)
                                {
                                    lclsUser.IsApprove = true;
                                    Label lblUserRoleId = (Label)row.FindControl("lblUserRoleId");
                                    TextBox txtApproveRangeFrom = (TextBox)row.FindControl("txtApprovalFrom");
                                    TextBox txtApproveRangeTo = (TextBox)row.FindControl("txtApprovalTo");
                                    //ListBox drpchoosenUserRole = (ListBox)row.FindControl("drpchoosenUserRole");
                                    lclsUser.ApproveRangeFrom = Convert.ToDecimal(txtApproveRangeFrom.Text);
                                    lclsUser.ApproveRangeTo = Convert.ToDecimal(txtApproveRangeTo.Text);

                                    //var FinalString = "";
                                    //var SB = new StringBuilder();
                                    //foreach (ListItem lst in drpchoosenUserRole.Items)
                                    //{
                                    //    if (lst.Selected)
                                    //    {
                                    //        SB.Append(lst.Value + ',');
                                    //    }
                                    //}
                                    //if (SB.Length > 0)
                                    //    FinalString = lblUserRoleId.Text + ',' + SB.ToString().Substring(0, (SB.Length - 1));
                                    //lclsUser.MultipleRoles = FinalString;

                                }
                                else
                                {
                                    lclsUser.IsApprove = false;
                                    lclsUser.ApproveRangeFrom = 0;
                                    lclsUser.ApproveRangeTo = 0;
                                    lclsUser.MultipleRoles = "";
                                }

                                if (((CheckBox)row.FindControl("IsOrder")).Checked)
                                {
                                    lclsUser.IsOrder = true;
                                }
                                else
                                {
                                    lclsUser.IsOrder = false;
                                }
                                if (((CheckBox)row.FindControl("IsDeny")).Checked)
                                {
                                    lclsUser.IsDeny = true;
                                }
                                else
                                {
                                    lclsUser.IsDeny = false;
                                }
                                Label lblUserRoleID = (Label)row.FindControl("lblUserRoleId");

                                lclsUser.UserRoleID = Convert.ToInt64(lblUserRoleID.Text);
                                lclsUser.MenuID = Convert.ToInt64(drpMainmenu.SelectedValue);
                                lclsUser.SubMenuId = Convert.ToInt64(drpsubmenu.SelectedValue);
                                lclsUser.PageName = drpsubmenu.SelectedItem.Text;
                                lclsUser.CreatedBy = Convert.ToInt64(defaultPage.UserId);


                                for (int j = 0; j < lstrole.Count; j++)
                                {
                                    if (lstrolefacility.Count > i)
                                    {
                                        if (lstrolefacility.Count != 0 && lstrole[j].UserRoleID == lstrolefacility[i].UserRoleID)
                                        {
                                            lclsUser.PermissionID = Convert.ToInt64(lstrolefacility[i].PermissionID);
                                            lstrMessage = lclsService.UpdateUserApprovePermission(lclsUser);
                                        }
                                        else if (lstrole[j].UserRoleID == lstrolefacility[i].UserRoleID)
                                        {
                                            lclsUser.PermissionID = 0;
                                            lstrMessage = lclsService.InsertUserApprovePermission(lclsUser);
                                        }
                                    }
                                    else
                                    {
                                        lclsUser.PermissionID = 0;
                                        lstrMessage = lclsService.InsertUserApprovePermission(lclsUser);
                                        j = j + lstrole.Count;
                                    }
                                }
                                i++;
                            }
                        }
                        else
                        {
                            if (MandatoryErrorMessage != "")
                            {   
                                log.LogWarning(msg.Replace("<<UserPermission>>", MandatoryErrorMessage));
                                MandatoryErrorMessage = MandatoryErrorMessage.ToString().Substring(0, (MandatoryErrorMessage.Length - 1));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerMandatoryWarningMsg.Replace("<<UserPermission>>", MandatoryErrorMessage), true);
                            }
                            else if (ErrorMessage != "")
                            {
                                ErrorMessage = ErrorMessage.ToString().Substring(0, (ErrorMessage.Length - 1));
                                log.LogWarning(logmsg.Replace("<<UserPermission>>", ErrorMessage));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerRepeatWarningMsg.Replace("<<UserPermission>>", ErrorMessage), true);
                            }
                            else if (ApprovalPermission != "")
                            {
                                ApprovalPermission = ApprovalPermission.ToString().Substring(0, (ApprovalPermission.Length - 1));
                                string msg = Constant.UserPerApprovalWarningMsg.Replace("ShowwarningPopup('", "").Replace("<<UserPermission>>", ApprovalPermission).Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerApprovalWarningMsg.Replace("<<UserPermission>>", ApprovalPermission), true);
                            }
                            else if(NotChecked != "")
                            {
                                NotChecked = NotChecked.ToString().Substring(0, (NotChecked.Length - 1));
                                log.LogWarning(lgmsg.Replace("<<UserPermission>>", NotChecked));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UseractioncheckboxWarningMsg.Replace("<<UserPermission>>", NotChecked), true);
                            }
                        }
                    }
                }
                else
                {
                    List<BindPermission> lstrole = lclsService.BindPermission().ToList();
                    List<GetPermission> lstrolefacility = lclsService.GetPermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                    foreach (GridViewRow row in GrdUserPermissionMaster.Rows)
                    {
                        if (((CheckBox)row.FindControl("IsEdit")).Checked)
                        {
                            lclsUser.IsEdit = true;
                        }
                        else
                        {
                            lclsUser.IsEdit = false;
                        }
                        if (((CheckBox)row.FindControl("IsViewOnly")).Checked)
                        {
                            lclsUser.IsView = true;
                        }
                        else
                        {
                            lclsUser.IsView = false;
                        }
                        if (drpMainmenu.SelectedValue=="5" && drpsubmenu.SelectedValue=="27")
                        {
                            if (((CheckBox)row.FindControl("IsEmailNotification")).Checked)
                            {
                                lclsUser.IsEmailNotification = true;
                            }
                            else
                            {
                                lclsUser.IsEmailNotification = false;
                            }
                        }
                        lclsUser.UserRoleID = Convert.ToInt64(row.Cells[0].Text.Trim().Replace("&nbsp;", ""));
                        lclsUser.MenuID = Convert.ToInt64(drpMainmenu.SelectedValue);
                        lclsUser.SubMenuId = Convert.ToInt64(drpsubmenu.SelectedValue);
                        lclsUser.PageName = drpsubmenu.SelectedItem.Text;
                        lclsUser.CreatedBy = Convert.ToInt64(defaultPage.UserId);
                        for (int j = 0; j < lstrole.Count; j++)
                        {
                            if (lstrolefacility.Count > i)
                            {
                                if (lstrolefacility.Count != 0 && lstrole[j].UserRoleID == lstrolefacility[i].UserRoleID)
                                {
                                    lclsUser.PermissionID = Convert.ToInt64(lstrolefacility[i].PermissionID);
                                    lstrMessage = lclsService.InsertUpdatePermission(lclsUser);
                                }
                                else if (lstrole[j].UserRoleID == lstrolefacility[i].UserRoleID)
                                {
                                    lclsUser.PermissionID = 0;
                                    lstrMessage = lclsService.InsertUpdatePermission(lclsUser);
                                }
                            }
                            else
                            {
                                lclsUser.PermissionID = 0;
                                lstrMessage = lclsService.InsertUpdatePermission(lclsUser);
                                j = j + lstrole.Count;
                            }
                        }
                        i++;
                    }
                }


                if (lstrMessage == "Saved Successfully")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    log.LogInformation(lmsg.Replace("<<UserPermission>>", lstuserper.ToString()));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionSaveMessage.Replace("<<UserPermission>>", lstuserper.ToString()), true);
                    if (drpMainmenu.SelectedValue == "7" && NotChecked == "")
                    {
                        List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                        BindApprovePermissionGrid(llstUserApprovePermission);
                    }
                }
                //else
                //{
                //    //Functions objfun = new Functions();
                //    //objfun.MessageDialog(this, "Updated Successfully");
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionSaveMessage.Replace("<<UserPermission>>", lstuserper.ToString()), true);
                //    if (drpMainmenu.SelectedValue == "7" && NotChecked == "" && ErrorMessage == "" && MandatoryErrorMessage == "" && ApprovalPermission == "")
                //    {
                //        List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                //        BindApprovePermissionGrid(llstUserApprovePermission);
                //    }
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void lkbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtRole.Style.Add("border", "Solid 1px #d2d6de");
                txtRole.Text = "";
                DivErrorList.Style.Add("display", "none");
                mpeRole.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRole.Text) == true)
                {
                    txtRole.Style.Add("border", "Solid 1px red");
                    mpeRole.Show();
                }
                else
                {
                    string lstrMessage = string.Empty;
                    BALUser lclsUser = new BALUser();
                    lclsUser.RoleName = txtRole.Text;
                    if (drpMainmenu.SelectedValue != "0")
                    {
                        lclsUser.MenuID = Convert.ToInt64(drpMainmenu.SelectedValue);
                        lclsUser.SubMenuId = Convert.ToInt64(drpsubmenu.SelectedValue);
                    }
                    else
                    {
                        lclsUser.MenuID = 0;
                        lclsUser.SubMenuId = 0;
                    }
                    lclsUser.CreatedBy = defaultPage.UserId;
                    List<BindPermission> llstUserRole = lclsService.BindPermission().Where(a => a.UserRole == lclsUser.RoleName).ToList();
                    if(llstUserRole.Count <= 0)
                    {
                        DivErrorList.Style.Add("display", "none");
                        lstrMessage = lclsService.InsertRole(lclsUser);

                        if (lstrMessage == "Saved Successfully")
                        {
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "Saved Successfully");
                            EventLogger log = new EventLogger(config);
                            log.LogInformation(lmsg.Replace("<<UserPermission>>", txtRole.Text.ToString()));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionSaveMessage.Replace("<<UserPermission>>", txtRole.Text.ToString()), true);
                            if (drpMainmenu.SelectedValue == "7")
                            {
                                List<GetUserApprovePermission> llstUserApprovePermission = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                                BindApprovePermissionGrid(llstUserApprovePermission);
                            }
                            else
                            {
                                List<GetPermission> lstrolefacility = null;
                                if (drpsubmenu.SelectedValue == "")
                                {
                                    lstrolefacility = lclsService.GetPermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(0)).ToList();
                                }
                                else
                                {
                                    lstrolefacility = lclsService.GetPermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                                }
                                
                                if (lstrolefacility.Count > 0)
                                {
                                    BindPermissionRole();
                                    GETPermissionRole();
                                }
                                else
                                {
                                    BindPermissionRole();
                                }
                            }
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPerUniqueRoleNameWarningMsg, true);
                        DivErrorList.Style.Add("display","block");
                        lblErrorList.Text = Constant.UserPerUniqueRoleNameWarningMsg;
                        mpeRole.Show();
                    }
                
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }

        }

        protected void GrdUserRoleApprove_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "";
                    HeaderCell.ColumnSpan = 6;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Style.Add("text-align", "center");
                    HeaderCell.Text = "Approval Range";
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    if (drpMainmenu.SelectedItem.Text == "Generate/Approve order" && drpsubmenu.SelectedItem.Text == "Service")
                    {
                        List<GetUserApprovePermission> llstpermissioncheck = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                        if (llstpermissioncheck.Count != 0)
                        {
                            HeaderCell = new TableCell();
                            HeaderCell.Text = "";
                            HeaderCell.ColumnSpan = 1;
                            HeaderGridRow.Cells.Add(HeaderCell);
                        }
                    }
                    GrdUserRoleApprove.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void GrdUserRoleApprove_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (drpMainmenu.SelectedItem.Text == "Generate/Approve order" && drpsubmenu.SelectedItem.Text != "Service")
                {
                    e.Row.Cells[8].Visible = false;
                }
                else
                {
                    List<GetUserApprovePermission> llstpermissioncheck = lclsService.GetUserApprovePermission(Convert.ToInt64(drpMainmenu.SelectedValue), Convert.ToInt64(drpsubmenu.SelectedValue)).ToList();
                    if (llstpermissioncheck.Count == 0)
                    {
                        e.Row.Cells[8].Visible = false;
                    }
                }
                if (drpMainmenu.SelectedValue != "0" && drpsubmenu.SelectedValue != "0")
                {
                    CheckBox IsOrder = (CheckBox)e.Row.FindControl("IsOrder");
                    if (IsOrder != null)
                    {
                        if (e.Row.RowIndex > 0)
                        {
                            IsOrder.Enabled = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void GrdUserPermissionMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (drpMainmenu.SelectedItem.Text == "Transfers" && drpsubmenu.SelectedItem.Text == "Transfer Out")
                {
                    e.Row.Cells[4].Visible = true;
                }
                else
                {
                    e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string MainMenuId = null;
                string SubMenuId = null;
                string SearchFilter = null;

                if (drpMainmenu.SelectedIndex > 0)
                    MainMenuId = drpMainmenu.SelectedValue;

                if (drpsubmenu.SelectedIndex > 0)
                    SubMenuId = drpsubmenu.SelectedValue;

                List<GetUserPermissionReport> llstreview = new List<GetUserPermissionReport>();                


                llstreview = lclsService.GetUserPermissionReport(MainMenuId, SubMenuId, SearchFilter, defaultPage.UserId).ToList();
                rvUserPermissionreport.ProcessingMode = ProcessingMode.Local;
                rvUserPermissionreport.LocalReport.ReportPath = Server.MapPath("~/Reports/UserPermissionReport.rdlc");

                Int64 r = defaultPage.UserId;

                ReportDataSource datasource = new ReportDataSource("DSUserPermissionReport", llstreview);
                rvUserPermissionreport.LocalReport.DataSources.Clear();
                rvUserPermissionreport.LocalReport.DataSources.Add(datasource);
                rvUserPermissionreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvUserPermissionreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);


                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "SessionFile" + Session.SessionID + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }
        }

        private void ShowPDFFile(string path)
        {
            try
            {
                // Open PDF File in Web Browser 
                if (Request.Browser.Type.ToUpper().Contains("IE"))
                {
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

                        //System.IO.File.Delete(path);
                        Response.End();
                    }
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UserPermissionErrorMessage.Replace("<<UserPermission>>", ex.Message), true);
            }

        }

    }
}