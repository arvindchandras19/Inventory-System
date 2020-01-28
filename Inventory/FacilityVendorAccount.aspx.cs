#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Net;
using System.Configuration;
using Inventory.Inventoryserref;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using System.IO;
using Inventory.Class;
using System.Text;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   Facility Vendor Account
'' Type      :   C# File
'' Description  :To add,update the Facility Vendor Account Details
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				   Sairam.P		                  New
 *  08/11/2017         V2.0                Sairam.P                       Change a Save method for Facility Vendor Mapping
 *                                                                             for existing Vendor Checking
 *  10/22/2017         V3.0                Sairam.P                       Validation Check for Mandatory and Dropdown check 
 *                                                                             for active and inactive.
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class FacilityVendorAccount : System.Web.UI.Page
    {
        #region Declartions
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALFacilityVendorAccount listFacilityVendorAcc = new BALFacilityVendorAccount();
        StringBuilder SB = new StringBuilder();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.FacilityVendorAccountSaveMessage.Replace("ShowPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.buttonPrint);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    //LoadLookups("Add");
                    BindFacility();
                    BindVendor();
                    BindGrid();
                    if (defaultPage != null)
                    {
                        if (defaultPage.VendorFacilityActPage_Edit == false && defaultPage.VendorFacilityActPage_View == true)
                        {
                            //SaveCancel.Visible = false;
                            btnsave.Visible = false;

                        }

                        if (defaultPage.VendorFacilityActPage_Edit == false && defaultPage.VendorFacilityActPage_View == false)
                        {
                            div_ADDContent.Visible = false;
                            DivSearch.Visible = false;
                            gridFacilityVendorAcc.Visible = false;
                            btnsave.Visible = false;
                            div_SearchDiv.Visible = false;
                            savecancel.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            ReqfielddrdFacilityName.ErrorMessage = req;
            ReqfielddrdVendorName.ErrorMessage = req;
            reqfieldtxtShipAccount.ErrorMessage = req;
            reqfieldtxtBillAccount.ErrorMessage = req;
        }

        public void BindGrid()
        {

            try
            {                
                //List<GetFacilityVendorAccount> list = new List<GetFacilityVendorAccount>();
                
                List<SearchFacilityVendorAcct> list = new List<SearchFacilityVendorAcct>();                

                foreach (ListItem lst in drpVendorSearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }

                if (SB.ToString() != "")
                    listFacilityVendorAcc.ListVendorID = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpVendorSearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                SB.Clear();

                foreach (ListItem lst in drpFacilitySearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }

                if (SB.ToString() != "")
                    listFacilityVendorAcc.ListFacilityID = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpItemCategorySearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                SB.Clear();

                listFacilityVendorAcc.IsStrActive = rdbstatus.SelectedValue;
                listFacilityVendorAcc.LoggedIN = defaultPage.UserId;
                listFacilityVendorAcc.Filter = "";
                list = lclsservice.GetFacilityVendorAcct(listFacilityVendorAcc).ToList();
                //lblrowcount.Text = "No of records : " + list.Count.ToString();
                gridFacilityVendorAcc.DataSource = list;
                gridFacilityVendorAcc.DataBind();
                //if (drpVendorSearch.SelectedIndex == 0 && drpFacilitySearch.SelectedIndex == 0)
                //{
                //    list = service.GetFacilityVendorAccount(SearchItem).ToList();
                //    gridFacilityVendorAcc.DataSource = list;
                //    gridFacilityVendorAcc.DataBind();
                //}
                //else if (drpVendorSearch.SelectedIndex == 0 || drpFacilitySearch.SelectedIndex == 0)
                //{
                //    list = service.GetFacilityVendorAccount(SearchItem).Where(a => a.FacilityDescription == drpFacilitySearch.SelectedItem.Text || a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();
                //    gridFacilityVendorAcc.DataSource = list;
                //    gridFacilityVendorAcc.DataBind();
                //}
                //else
                //{
                //    list = service.GetFacilityVendorAccount(SearchItem).Where(a => a.FacilityDescription == drpFacilitySearch.SelectedItem.Text && a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();
                //    gridFacilityVendorAcc.DataSource = list;
                //    gridFacilityVendorAcc.DataBind();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        public void BindVendor()
        {
            try
            {
                string SearchItem = string.Empty;
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetVendor> listVendor = new List<GetVendor>();
                listVendor = service.GetVendor().ToList();
                drpVendorSearch.DataSource = listVendor;
                drpVendorSearch.DataTextField = "VendorDescription";
                drpVendorSearch.DataValueField = "VendorID";
                drpVendorSearch.DataBind();
                //drpVendorSearch.Items.Insert(0, lst);

                foreach (ListItem lst1 in drpVendorSearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }


        public void BindVendorAdd(string mode)
        {
            try
            {
                string SearchItem = string.Empty;
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetVendor> listVendor = new List<GetVendor>();
                if (mode == "Add")
                {
                    listVendor = service.GetVendor().Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    listVendor = service.GetVendor().ToList();
                }
                drdVendorName.DataSource = listVendor;
                drdVendorName.DataTextField = "VendorDescription";
                drdVendorName.DataValueField = "VendorID";
                drdVendorName.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdVendorName.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        public void BindFacility()
        {
            try
            {                
                List<GetFacility> lstcategory = new List<GetFacility>();
                lstcategory = lclsservice.GetFacility().ToList();
                drpFacilitySearch.DataSource = lstcategory;
                drpFacilitySearch.DataValueField = "FacilityID";
                drpFacilitySearch.DataTextField = "FacilityDescription";
                drpFacilitySearch.DataBind();
                //drpFacilitySearch.Items.Insert(0, lst);

                foreach (ListItem lst1 in drpFacilitySearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        public void BindFacilityAdd(string mode)
        {
            try
            {
                List<GetFacility> lstcategory = new List<GetFacility>();
                if (mode == "Add")
                {
                    lstcategory = lclsservice.GetFacility().Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    lstcategory = lclsservice.GetFacility().ToList();
                }
                drdFacilityName.DataSource = lstcategory;
                drdFacilityName.DataValueField = "FacilityID";
                drdFacilityName.DataTextField = "FacilityDescription";
                drdFacilityName.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdFacilityName.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                BindFacilityAdd("Add");
                BindVendorAdd("Add");
                ToggleAdd("Add");
                HideAndShowControls("Edit");
                DivCheckBox.Style.Add("display","none");
                drdFacilityName.SelectedIndex = -1;
                drdVendorName.SelectedIndex = -1;
                txtBillAccount.Text = "";
                txtShipAccount.Text = "";
                hdnFVAccountID.Value = "0";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        public void ToggleAdd(string Mode)
        {
            try
            {
                if (Mode == "Add")
                {
                    btnCancel.Visible = false;
                    btnclose.Visible = true;
                    btnSearchItems.Visible = false;
                    btnAdd.Visible = false;
                    buttonPrint.Visible = false;
                    btnsave.Visible = true;
                    DivSearchControl.Style.Add("display", "none");
                    DivAdd.Style.Add("display", "block");
                    lblseroutHeader.Visible = false;
                    DivSearch.Style.Add("display", "none");
                }
                else
                {
                    btnCancel.Visible = true;
                    btnclose.Visible = false;
                    btnSearchItems.Visible = true;
                    btnAdd.Visible = true;
                    buttonPrint.Visible = true;
                    btnsave.Visible = false;
                    DivSearchControl.Style.Add("display", "block");
                    DivAdd.Style.Add("display", "none");
                    lblseroutHeader.Visible = true;
                    DivSearch.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }

        }

        public void HideAndShowControls(string Mode)
        {
            try
            {
                if (Mode == "Edit")
                {
                    drdFacilityName.Enabled = true;
                    drdVendorName.Enabled = true;
                    txtShipAccount.Enabled = true;
                    txtBillAccount.Enabled = true;
                    btnsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = false;
                    drdFacilityName.Enabled = false;
                    drdVendorName.Enabled = false;
                    txtShipAccount.Enabled = false;
                    txtBillAccount.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                string FacilityVendorAcct = string.Empty;
                string lstrMessage = string.Empty;
                InventoryServiceClient service = new InventoryServiceClient();
                BALFacilityVendorAccount obj = new BALFacilityVendorAccount();
                obj.FacilityVendorAccID = Convert.ToInt64(hdnFVAccountID.Value);
                obj.FacilityID = Convert.ToInt64(drdFacilityName.SelectedValue);
                obj.VendorID = Convert.ToInt64(drdVendorName.SelectedValue);
                obj.ShipAccount = txtShipAccount.Text;
                obj.BillAccount = txtBillAccount.Text;
                obj.CreatedBy = defaultPage.UserId;
                obj.CreatedOn = DateTime.Now;
                if (chkactive.Checked == true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                if (obj.FacilityVendorAccID == 0)
                {

                    List<GetFacilityVendorAccount> lstcount = service.GetFacilityVendorAccount(drdFacilityName.SelectedItem.Text).Where(o => o.VendorDescription == drdVendorName.SelectedItem.Text.ToString()).ToList();
                    if (lstcount.Count <= 0)
                    {
                        lstrMessage = service.InsertUpdateFacilityVendorAccount(obj);
                        FacilityVendorAcct = drdFacilityName.SelectedItem.Text + " " + drdVendorName.SelectedItem.Text;
                        log.LogInformation(msgwrn.Replace("<<FacilityDescription>>", FacilityVendorAcct.ToString()));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountSaveMessage.Replace("<<FacilityDescription>>", FacilityVendorAcct.ToString()), true);

                    }
                    else
                    {
                        //Functions objfun = new Functions();
                        //objfun.MessageDialog(this, "Record Exist");
                        string msg = Constant.WarningFacilityVendorAccountMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<FacilityDescription>>", "Record Exists").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilityVendorAccountMessage.Replace("<<FacilityDescription>>", "Record Exists"), true);
                    }
                }
                else
                {
                    if (!ValidateLoookups(service)) return;

                    List<GetFacilityVendorAccount> lstcount = service.GetFacilityVendorAccount(drdFacilityName.SelectedItem.Text).Where(o => o.VendorDescription == drdVendorName.SelectedItem.Text.ToString() && o.FacilityVendorAccID != obj.FacilityVendorAccID).ToList();
                    if (lstcount.Count == 0)
                    {
                        lstrMessage = service.InsertUpdateFacilityVendorAccount(obj);
                        FacilityVendorAcct = drdFacilityName.SelectedItem.Text + " " + drdVendorName.SelectedItem.Text;
                        log.LogInformation(msgwrn.Replace("<<FacilityDescription>>", FacilityVendorAcct.ToString()));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountSaveMessage.Replace("<<FacilityDescription>>", FacilityVendorAcct.ToString()), true);
                    }
                    else
                    {
                        //Functions objfun = new Functions();
                        //objfun.MessageDialog(this, "The Record Entered is alredy Exist");
                        string msg = Constant.FacilityVendorAccountErrorMessage.Replace("ShowdelPopup('", "").Replace("<<FacilityDescription>>", "The record entered is already exist").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", "The record entered is already exist"), true);
                    }
                }

                if (lstrMessage == "Saved Successfully")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    //txtSearchFacility.Text = "";
                    BindGrid();
                    drdFacilityName.SelectedIndex = 0;
                    drdVendorName.SelectedIndex = 0;
                    txtBillAccount.Text = "";
                    txtShipAccount.Text = "";
                    hdnFVAccountID.Value = "0";
                    ToggleAdd("Clear");
                }

            }

            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        private void clear()
        {
            throw new NotImplementedException();
        }

        protected void lbedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ToggleAdd("Add");
                HideAndShowControls("Edit");
                //div_AddContent.Visible = true;
                InventoryServiceClient service = new InventoryServiceClient();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnFVAccountID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                BindFacilityAdd("Edit");
                drdFacilityName.ClearSelection();
                drdFacilityName.Items.FindByText(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                BindVendorAdd("Edit");
                drdVendorName.ClearSelection();
                drdVendorName.Items.FindByText(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                txtShipAccount.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                txtBillAccount.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                    DivCheckBox.Style.Add("display", "none");
                }
                else
                {
                    chkactive.Visible = true;
                    DivCheckBox.Style.Add("display","block");
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showContent", "editfacility();", true);

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        //private void LoadLookups(string mode)
        //{
        //    BindVendor(mode);
        //    BindFacility(mode);
        //}
        private bool ValidateLoookups(InventoryServiceClient service)
        {
            bool result = true;
            string errmessage = "";
            //Facility Lookup
            List<GetFacility> lstfac = service.GetFacility().Where(a => a.IsActive == true && a.FacilityID == Convert.ToInt64(drdFacilityName.SelectedValue)).ToList();
            if (lstfac.Count == 0)
            {
                errmessage += "Facility (" + drdFacilityName.SelectedItem.Text + ") , ";
                result = false;
            }

            //Vendor Lookup
            List<GetvendorDetails> lstvendor = service.GetvendorDetails("").Where(a => a.IsActive == true && a.VendorID == Convert.ToInt64(drdVendorName.SelectedValue)).ToList();
            if (lstvendor.Count == 0)
            {
                errmessage += "Vendor (" + drdVendorName.SelectedItem.Text + ") ";
                result = false;
            }


            if (!result)
            {
                EventLogger log = new EventLogger(config);
                string msg = Constant.WarningLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<values>>", errmessage).Replace("');", "");
                log.LogWarning(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningLookupMessage.Replace("<<values>>", errmessage), true);
            }

            return result;
        }

        protected void btndeleteimg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndelete = sender as ImageButton;
                GridViewRow row = (GridViewRow)btndelete.NamingContainer;
                hdnFVAccountID.Value = row.Cells[1].Text.Trim();
                HddFacilityVendor.Value = row.Cells[2].Text.Trim() + "," + row.Cells[3].Text.Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }

        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();

                lclsService.DeleteFacilityVendorAccount(Convert.ToInt64(hdnFVAccountID.Value), Convert.ToInt64(defaultPage.UserId), false);
                EventLogger log = new EventLogger(config);
                string msg = Constant.FacilityVendorAccountDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<FacilityDescription>>", HddFacilityVendor.Value.ToString()).Replace("');", "");
                log.LogInformation(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountDeleteMessage.Replace("<<FacilityDescription>>", HddFacilityVendor.Value.ToString()), true);
                BindGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
           
        }



        //protected void chkActive_CheckedChanged(object sender, EventArgs e)
        //{

        //    GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //    CheckBox chkactive = (CheckBox)row.FindControl("chkActive");

        //    InventoryServiceClient lclsService = new InventoryServiceClient();

        //    if (chkactive.Checked == true)
        //    {
        //        lclsService.DeleteFacilityVendorAccount(Convert.ToInt64(row.Cells[1].Text.Trim()), Convert.ToInt64(defaultPage.UserId), true);
        //    }
        //    else
        //    {
        //        lclsService.DeleteFacilityVendorAccount(Convert.ToInt64(row.Cells[1].Text.Trim()), Convert.ToInt64(defaultPage.UserId), false);



        //    }

        //    BindGrid();
        //}


        protected void btnSearchFacility_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                //if (drpVendorSearch.SelectedIndex == 0 && drpFacilitySearch.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorItemMapMessage.Replace("<<VendorItemMap>>", "Select any one of the search fields"), true);
                //}
                //else
                //{
                //    BindGrid();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                drdFacilityName.SelectedIndex = -1;
                drdVendorName.SelectedIndex = -1;
                txtBillAccount.Text = "";
                txtShipAccount.Text = "";
                rdbstatus.SelectedValue = "1";
                //LoadLookups("Add");
                BindFacility();
                BindVendor();
                ToggleAdd("Clear");
                BindGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }
        #endregion

        protected void buttonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = PrintFacility();
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Facility Vendor Account " + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
         
        }

        private void ShowPDFFile(string path)
        {
            try
            {
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
                        System.IO.File.Delete(path);
                        Response.End();
                    }
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintFacility()
        {
            List<object> llstarg = new List<object>();
            List<BindFacilityVendorAccountReport> llstreview = null;

            BindGrid();          

            foreach (ListItem lst in drpVendorSearch.Items)
            {
                if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                {
                    SB.Append(lst.Value + ',');
                }
            }

            if (SB.ToString() != "")
                listFacilityVendorAcc.ListVendorID = SB.ToString().Substring(0, (SB.Length - 1));            
            SB.Clear();

            foreach (ListItem lst in drpFacilitySearch.Items)
            {
                if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                {
                    SB.Append(lst.Value + ',');
                }
            }

            if (SB.ToString() != "")
                listFacilityVendorAcc.ListFacilityID = SB.ToString().Substring(0, (SB.Length - 1));            
            SB.Clear();

            listFacilityVendorAcc.IsStrActive = rdbstatus.SelectedValue;
            listFacilityVendorAcc.LoggedIN = defaultPage.UserId;
            listFacilityVendorAcc.Filter = "";
            llstreview = lclsservice.BindFacilityVendorAccountReport(listFacilityVendorAcc).ToList();

            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvFacilityVendorAccreport.ProcessingMode = ProcessingMode.Local;
            rvFacilityVendorAccreport.LocalReport.ReportPath = Server.MapPath("~/Reports/FacilityVendorAccountReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("FacilityVendorAccountReportDS", llstreview);
            rvFacilityVendorAccreport.LocalReport.DataSources.Clear();
            rvFacilityVendorAccreport.LocalReport.DataSources.Add(datasource);
            rvFacilityVendorAccreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvFacilityVendorAccreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        protected void gridFacilityVendorAcc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lbldelete = (ImageButton)e.Row.FindControl("btndeleteimg");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    if (lblActive.Text == "Yes")
                    {
                        lbldelete.Enabled = true;
                        chkactive.Visible = false;
                        DivCheckBox.Style.Add("display", "none");
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkactive.Visible = true;
                        DivCheckBox.Style.Add("display", "block");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleAdd("Clear");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
           
        }
    }
}