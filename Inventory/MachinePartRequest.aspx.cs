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
'' Name      :   <<MachinePartsRequest>>
'' Type      :   C# File
'' Description  :<<To add,update the Machine Parts Request Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	09/06/2017		   V1.0				   Vivekanand.S		                   New
 *  09/19/2017         V2.0                Sairam.P                       Exception Handling and Notification
 *  10/21/2017         V3.0                Murali M                       Added Review Screen & CR Updates
 *  10/25/2017         V.01              Vivekanand.S                     Locked the record.
 ''--------------------------------------------------------------------------------
'*/
#endregion


namespace Inventory
{
    public partial class MachinePartRequest : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        #endregion
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALMPRMaster llstMPRMaster = new BALMPRMaster();
        string a = string.Empty;
        string b = string.Empty;
        string ErrorList = string.Empty;
        string PendingApproval = Constant.PendingApprovalforreq;
        private string _sessionPDFFileName;
        string FinalString = "";
        string loadshipping = Constant.loadshipping;
        StringBuilder SB = new StringBuilder();
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.grdMPRMaster);
                scriptManager.RegisterPostBackControl(this.GvTempEdit);
                HddQueryStringID.Value = Request.QueryString["MPRID"];
                if (!IsPostBack)
                {
                    //BindCorporate();
                    BindStatus("Add");
                    if (defaultPage != null)
                    {

                        if (HddQueryStringID.Value != "")
                        {
                            BindValuesOfQueryString();
                        }
                        else
                        {
                            BindCorporate(1, "Add");
                            //drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                            BindFacility(1, "Add");
                            //drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                            BindVendor(1, "Add");
                            BindGrid(0);
                            BindEquipcategory(Convert.ToInt64(defaultPage.CorporateID));
                            SearchGrid();
                            //if (defaultPage.RoleID == 1)
                            //{
                            //    drpcor.Enabled = true;
                            //    drpfacility.Enabled = true;
                            //}

                            //else
                            //{
                            //    drpcor.Enabled = false;
                            //    drpfacility.Enabled = false;
                            //    ddlCorporate.Enabled = false;
                            //    ddlFacility.Enabled = false;
                            //    List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                            //    grdMPRMaster.DataSource = lstMPRMaster;
                            //    grdMPRMaster.DataBind();
                            //}
                            if (defaultPage.Req_MachinePartsPage_Edit == false && defaultPage.Req_MachinePartsPage_View == true)
                            {
                                btnAdd.Visible = false;
                                btnSave.Visible = false;
                                btnReview.Visible = false;
                                btnImgDeletePopUp.Visible = false;
                                btnSearchNewRow.Enabled = false;
                            }
                            if (defaultPage.Req_MachinePartsPage_Edit == false && defaultPage.Req_MachinePartsPage_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                            }
                            //BindCorporate(0, "Edit");
                            //BindFacility(1, "Add");
                            //BindVendor(1, "Add");
                            //BindLookUp("Add");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Values for Lookup for Machine Parts Order 
        /// </summary>
        public void BindQueryStringLookupvalues()
        {
            List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().Where(a => a.MPRMasterID == Convert.ToInt64(HddQueryStringID.Value)).ToList();
            BindCorporate(0, "Edit");
            ddlCorporate.ClearSelection();
            ddlCorporate.SelectedValue = Convert.ToString(lstMPRMaster[0].CorporateID);
            BindFacility(0, "Edit");
            ddlFacility.ClearSelection();
            ddlFacility.SelectedValue = Convert.ToString(lstMPRMaster[0].FacilityID);

            BindVendor(0, "Edit");
            ddlVendor.ClearSelection();
            ddlVendor.SelectedValue = Convert.ToString(lstMPRMaster[0].VendorID);

            ddlEquipmentCategory.ClearSelection();
            ddlEquipmentCategory.Items.FindByText(lstMPRMaster[0].EquipmentCategory).Selected = true;

            GetEquipmentSubCategory();
            ddlEquipmentSubCat.ClearSelection();
            ddlEquipmentSubCat.Items.FindByText(lstMPRMaster[0].EquipmentSubCategory).Selected = true;

            GetEquipementList();
            ddlEquipmentList.ClearSelection();
            ddlEquipmentList.Items.FindByText(lstMPRMaster[0].EquipementList).Selected = true;
            BindShipping("Add");
            ddlShipping.ClearSelection();
            ddlShipping.SelectedValue = Convert.ToString(lstMPRMaster[0].Shipping);
            txtSerialNo.Text = lstMPRMaster[0].SerialNo;
            txtHoursonmachine.Text = lstMPRMaster[0].Hoursonmachine;
            lblMasterNo.Text = lstMPRMaster[0].MPRNo;
            //BindCorporate(0, "Edit");
            //BindFacility(1, "Edit");
            //BindVendor(1, "Edit");
            //BindLookUp("Edit");
        }
        /// <summary>
        /// Bind the Values for Machine Parts Order 
        /// </summary>
        public void BindValuesOfQueryString()
        {
            BindCorporate(0, "Add");
            ddlCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
            BindFacility(0, "Add");
            ddlFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
            BindVendor(0, "Add");
            BindEquipcategory(Convert.ToInt64(defaultPage.CorporateID));
            //GetEquipmentSubCategory();
            //GetEquipementList();
            btnAdd.Visible = false;
            btnSearch.Visible = false;
            btnPrint.Visible = false;
            btnReview.Visible = true;
            btnClose.Visible = true;
            btnSave.Visible = true;
            divMPRMaster.Style.Add("display", "none");
            divsearch.Style.Add("display", "none");
            divContentDetails.Style.Add("display", "block");
            divSearchMachine.Style.Add("display", "block");
            divAddMachine.Style.Add("display", "block");
            DivMPRMasterNo.Style.Add("display", "block");
            divMPRDetails.Style.Add("display", "block");
            lblseroutHeader.Style.Add("display", "none");
            lblAddItemHeader.Style.Add("display", "none");
            //lblUpdateHeader.Style.Add("display", "block");
            //lblMasterHeader.Style.Add("display", "block");
            lblMasterHeader.Visible = false;
            lblUpdateHeader.Visible = true;
            lblAddItemHeader.Visible = false;
            lblrcount3.Visible = false;
            btn_New.Visible = false;
            //foreach (GridViewRow row in grdMPRMaster.Rows)
            //{
            //    if (HddQueryStringID.Value == row.Cells[16].Text)
            //    {
            //        lblMasterNo.Text = row.Cells[4].Text;
            //    }
            //}
            HddMasterID.Value = HddQueryStringID.Value;
            BindQueryStringLookupvalues();
            string LockTimeOut = "";
            LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
            List<GetMPRDetailsbyMPRMasterID> lstMPRMasterDetails = lclsservice.GetMPRDetailsbyMPRMasterID(Convert.ToInt64(HddMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();
            gvSearchMRPDetails.DataSource = lstMPRMasterDetails;
            gvSearchMRPDetails.DataBind();
            ddlCorporate.Enabled = false;
            ddlFacility.Enabled = false;
            ddlVendor.Enabled = false;
            ddlEquipmentCategory.Enabled = false;
            ddlEquipmentSubCat.Enabled = false;
            ddlEquipmentList.Enabled = false;
            ddlShipping.Enabled = false;
        }
        /// <summary>
        /// Bind the Corporate details to dropdown control 
        /// </summary>
        #region Bind Corporate Values
        public void BindCorporate(int search, string mode)
        {
            try
            {
                List<BALUser> lstfacility = new List<BALUser>();
                if (search == 1)
                {
                    //ListItem lst = new ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
                    if (defaultPage.RoleID == 1)
                    {
                        // Search Drop Down   
                        lstfacility = lclsservice.GetCorporateMaster().ToList();
                        drpcor.DataSource = lstfacility;
                        drpcor.DataTextField = "CorporateName";
                        drpcor.DataValueField = "CorporateID";
                        drpcor.DataBind();
                        //drpcor.Items.Insert(0, lst);
                        //drpcor.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        drpcor.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        drpcor.DataTextField = "CorporateName";
                        drpcor.DataValueField = "CorporateID";
                        drpcor.DataBind();
                        //drpcor.Items.Insert(0, lst);
                        //drpcor.SelectedIndex = 0;
                    }
                    foreach (ListItem lst in drpcor.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }

                ListItem lstDDl = new ListItem();
                lstDDl.Value = "0";
                lstDDl.Text = "--Select Corporate--";
                if (mode == "Add")
                {
                    if (defaultPage.RoleID == 1)
                    {
                        lstfacility = lclsservice.GetCorporateMaster().ToList();
                        ddlCorporate.DataSource = lstfacility;
                        ddlCorporate.DataTextField = "CorporateName";
                        ddlCorporate.DataValueField = "CorporateID";
                        ddlCorporate.DataBind();
                        ddlCorporate.Items.Insert(0, lstDDl);
                        ddlCorporate.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        ddlCorporate.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        ddlCorporate.DataTextField = "CorporateName";
                        ddlCorporate.DataValueField = "CorporateID";
                        ddlCorporate.DataBind();
                        ddlCorporate.Items.Insert(0, lstDDl);
                        ddlCorporate.SelectedIndex = 0;
                    }
                }
                if (mode == "Edit")
                {
                    ddlCorporate.DataSource = lclsservice.GetCorporateMaster().ToList();
                    ddlCorporate.DataTextField = "CorporateName";
                    ddlCorporate.DataValueField = "CorporateID";
                    ddlCorporate.DataBind();
                    ddlCorporate.Items.Insert(0, lstDDl);
                    ddlCorporate.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Facility details from Facility marter table to dropdown control 
        /// </summary>
        #region Bind Facility Values
        private void BindFacility(int Search, string mode)
        {
            try
            {
                if (Search == 1)
                {
                    if (drpcor.SelectedValue != "")
                    {
                        foreach (ListItem lst in drpcor.Items)
                        {
                            if (lst.Selected && drpcor.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        // Search Drop Down
                        drpfacility.DataSource = lclsservice.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                        drpfacility.DataTextField = "FacilityDescription";
                        drpfacility.DataValueField = "FacilityID";
                        drpfacility.DataBind();
                        //if (defaultPage.RoleID == 1)
                        //{
                        //    if (drpcor.SelectedValue != "All")
                        //    {
                        //        // Search Drop Down
                        //        drpfacility.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcor.SelectedValue)).Where(a => a.IsActive == true).ToList();
                        //        drpfacility.DataTextField = "FacilityDescription";
                        //        drpfacility.DataValueField = "FacilityID";
                        //        drpfacility.DataBind();
                        //        //ListItem lst = new ListItem();
                        //        //lst.Value = "All";
                        //        //lst.Text = "All";
                        //        //drpfacility.Items.Insert(0, lst);
                        //        //drpfacility.SelectedIndex = 0;
                        //    }
                        //    else
                        //    {
                        //        drpfacility.SelectedIndex = 0;
                        //    }

                        //}
                        //else
                        //{
                        //    if (drpcor.SelectedValue != "All")
                        //    {
                        //        drpfacility.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == drpcor.SelectedItem.Text).ToList();
                        //        drpfacility.DataTextField = "FacilityName";
                        //        drpfacility.DataValueField = "FacilityID";
                        //        drpfacility.DataBind();
                        //        //ListItem lst = new ListItem();
                        //        //lst.Value = "All";
                        //        //lst.Text = "All";
                        //        //drpfacility.Items.Insert(0, lst);
                        //        //drpfacility.SelectedIndex = 0;
                        //    }
                        //    else
                        //    {
                        //        drpfacility.SelectedIndex = 0;
                        //    }
                        //}
                    }
                    foreach (ListItem lst in drpfacility.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Facility--";
                    // Insert Drop Down
                    if (mode == "Add")
                    {
                        if (defaultPage.RoleID == 1)
                        {
                            ddlFacility.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(ddlCorporate.SelectedValue)).Where(a => a.IsActive == true).ToList();
                            ddlFacility.DataTextField = "FacilityDescription";
                            ddlFacility.DataValueField = "FacilityID";
                            ddlFacility.DataBind();
                            ddlFacility.Items.Insert(0, lst);
                            ddlFacility.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlFacility.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == ddlCorporate.SelectedItem.Text).ToList();
                            ddlFacility.DataTextField = "FacilityName";
                            ddlFacility.DataValueField = "FacilityID";
                            ddlFacility.DataBind();
                            ddlFacility.Items.Insert(0, lst);
                            ddlFacility.SelectedIndex = 0;
                        }
                    }

                    else if (mode == "Edit")
                    {
                        ddlFacility.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(ddlCorporate.SelectedValue)).ToList();
                        ddlFacility.DataTextField = "FacilityDescription";
                        ddlFacility.DataValueField = "FacilityID";
                        ddlFacility.DataBind();
                        ddlFacility.Items.Insert(0, lst);
                        ddlFacility.SelectedIndex = 0;
                    }

                }
                BindVendor(1, "Add");
            }     
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Vendor details from vendor master table to dropdown control 
        /// </summary>
        #region Bind Vendor Values
        public void BindVendor(int Search, string mode)
        {
            try
            {
                List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                if (Search == 1)
                {
                    // Search Drop Down 
                    if (drpfacility.SelectedValue != "")
                    {
                        foreach (ListItem lst in drpfacility.Items)
                        {
                            if (lst.Selected && drpfacility.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        string SearchText = string.Empty;
                        List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
                        lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => a.MachineParts == true).ToList();

                        drpvendor.DataSource = lstvendor;
                        drpvendor.DataTextField = "VendorDescription";
                        drpvendor.DataValueField = "VendorID";
                        drpvendor.DataBind();
                        //ListItem lstven = new ListItem();
                        //lstven.Value = "All";
                        //lstven.Text = "All";
                        //drpvendor.Items.Insert(0, lstven);
                        //drpvendor.SelectedIndex = 0;
                    }
                    foreach (ListItem lst in drpvendor.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Vendor--";
                    if (mode == "Add")
                    {
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => (a.MachineParts == true) && (a.IsActive == true)).Distinct().ToList();
                        ddlVendor.DataSource = lstvendordetails;
                        //drpvendor.DataSource = lstvendordetails;
                        ddlVendor.DataTextField = "VendorDescription";
                        ddlVendor.DataValueField = "VendorID";
                        ddlVendor.DataBind();
                        ddlVendor.Items.Insert(0, lst);
                        ddlVendor.SelectedIndex = 0;
                    }
                    else if (mode == "Edit")
                    {
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.MachineParts == true).Distinct().ToList();
                        ddlVendor.DataSource = lstvendordetails;
                        ddlVendor.DataTextField = "VendorDescription";
                        ddlVendor.DataValueField = "VendorID";
                        ddlVendor.DataBind();
                        ddlVendor.Items.Insert(0, lst);
                        ddlVendor.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);

            }

        }
        #endregion


        /// <summary>
        /// Bind the Status LookUp details from Status master table to dropdown control 
        /// </summary>
        #region Bind Status LookUp Values
        public void BindStatus(string Mode)
        {
            try
            {
                // Search Status drop down
                List<GetList> lstLookUp = new List<GetList>();
                string SearchText = string.Empty;
                lstLookUp = lclsservice.GetList("MachinePartsRequest", "Status", Mode).ToList();
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                drpStatus.Items.FindByText(PendingApproval).Selected = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void BindShipping(string Mode)
        {
            try
            {
                List<GetList> lstLookUpship = new List<GetList>();
                lstLookUpship = lclsservice.GetList("MachinePartsRequest", "Shipping", Mode).ToList();
                ddlShipping.DataSource = lstLookUpship;
                ddlShipping.DataTextField = "InvenValue";
                ddlShipping.DataValueField = "InvenValue";
                ddlShipping.DataBind();
                ddlShipping.Items.FindByText(loadshipping).Selected = true;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Machine Parts Request Master details from MPRMaster table to Grid control 
        /// </summary>
        #region Bind Machine Parts Request Master Values
        public void BindGrid(int TestEdit)
        {
            try
            {
                if (TestEdit == 1)
                {
                    BALMPRMaster llstMachineSearch = new BALMPRMaster();
                    
                    if (drpcor.SelectedValue == "All")
                    {
                        llstMachineSearch.CorporateName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpcor.Items)
                        {
                            if (lst.Selected && drpcor.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        llstMachineSearch.CorporateName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpfacility.SelectedValue == "All")
                    {
                        llstMachineSearch.FacilityName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpfacility.Items)
                        {
                            if (lst.Selected && drpfacility.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstMachineSearch.FacilityName = FinalString;

                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpvendor.SelectedValue == "All")
                    {
                        llstMachineSearch.VendorName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpvendor.Items)
                        {
                            if (lst.Selected && drpvendor.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstMachineSearch.VendorName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpStatus.SelectedValue == "All")
                    {
                        llstMachineSearch.Status = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpStatus.Items)
                        {
                            if (lst.Selected)
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstMachineSearch.Status = FinalString;
                    }
                    SB.Clear();
                    //if (txtDateFrom.Text != "") llstMachineSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    //if (txtDateTo.Text != "") llstMachineSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    if (txtDateFrom.Text == "")
                    {
                        txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstMachineSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    }
                    if (txtDateTo.Text == "")
                    {
                        txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstMachineSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    }

                    llstMachineSearch.loggedinBy = defaultPage.UserId;
                    List<SearchMPRMaster> lstMPRMaster = lclsservice.SearchMPRMaster(llstMachineSearch).ToList();
                    GvTempEdit.DataSource = lstMPRMaster;
                    GvTempEdit.DataBind();

                }
                else
                {
                    SearchGrid();
                    //grdMPRMaster.DataSource = lstMPRMaster;
                    //grdMPRMaster.DataBind();
                }
                ViewState["ReportMachinePartsID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Machine Parts Request details from MPRDetails table to Grid control 
        /// </summary>
        #region Bind Machine Parts Request details Values
        public void BindDetailGrid(Int64 MPRMasterID, Int64 UserId)
        {
            try
            {
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetMPRDetailsbyMPRMasterID> llstMPRMaster = lclsservice.GetMPRDetailsbyMPRMasterID(MPRMasterID, UserId, Convert.ToInt64(LockTimeOut)).ToList();

                if (llstMPRMaster[0].IsReadOnly == 0)
                {
                    gvSearchMRPDetails.Enabled = true;
                    btnReview.Enabled = true;
                }
                else if (llstMPRMaster[0].IsReadOnly == 1)
                {
                    gvSearchMRPDetails.Enabled = false;
                    btnReview.Enabled = false;
                    List<GetUserDetails> llstuserdetails = lclsservice.GetUserDetails(Convert.ToInt64(llstMPRMaster[0].Lockedby)).ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsreqMessage.Replace("<<MachinePartsRequestDescription>>", "Another user " + llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName + " is updating this record , Please try after some time."), true);
                }
                gvSearchMRPDetails.DataSource = llstMPRMaster;
                gvSearchMRPDetails.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


        /// <summary>
        /// Search the Machine Parts Request Master details from vendor master table to dropdown control 
        /// </summary>
        #region Bind Search Values
        public void SearchGrid()
        {
            try
            {
                BALMPRMaster llstMachineSearch = new BALMPRMaster();
                                
                if (drpcor.SelectedValue == "All")
                {
                    llstMachineSearch.CorporateName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpcor.Items)
                    {
                        if (lst.Selected && drpcor.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    llstMachineSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacility.SelectedValue == "All")
                {
                    llstMachineSearch.FacilityName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpfacility.Items)
                    {
                        if (lst.Selected && drpfacility.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMachineSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendor.SelectedValue == "All")
                {
                    llstMachineSearch.VendorName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpvendor.Items)
                    {
                        if (lst.Selected && drpvendor.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMachineSearch.VendorName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    llstMachineSearch.Status = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpStatus.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMachineSearch.Status = FinalString;
                }
                SB.Clear();

                //if (txtDateFrom.Text != "") llstMachineSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                //if (txtDateTo.Text != "") llstMachineSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMachineSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMachineSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                llstMachineSearch.loggedinBy = defaultPage.UserId;
                List<SearchMPRMaster> lstMSRMaster = lclsservice.SearchMPRMaster(llstMachineSearch).ToList();
                grdMPRMaster.DataSource = lstMSRMaster;
                grdMPRMaster.DataBind();




                //llstMachineSearch.CorporateID = Convert.ToInt64(drpcor.SelectedValue);
                //llstMachineSearch.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
                //llstMachineSearch.VendorID = Convert.ToInt64(drpvendor.SelectedValue);
                //llstMachineSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                //llstMachineSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                //llstMachineSearch.Status = drpStatus.SelectedValue;
                //if (llstMachineSearch.DateFrom <= llstMachineSearch.DateTo)
                //{
                //    List<SearchMPRMaster> lstMPRMaster = lclsservice.SearchMPRMaster(llstMachineSearch).ToList();
                //    grdMPRMaster.DataSource = lstMPRMaster;
                //    grdMPRMaster.DataBind();
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessagedate.Replace("<<MachinePartsRequestDescription>>", ""), true);
                //}
                ViewState["ReportMachinePartsID"] = "";

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion



        protected void grdMPRMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string status = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                status = e.Row.Cells[6].Text;
                //string b ="<img src= \"Images/Readmore.png\" />";
                //Label lblAudit = (Label)e.Row.FindControl("lblAudit");
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                ImageButton imgbtnedit = (ImageButton)e.Row.FindControl("imgbtnEdit");

                if (status == "Ordered and Pending Receive" || status == "Denied")
                {
                    imgbtnedit.Visible = false;
                }
                else
                {
                    imgbtnedit.Visible = true;
                }
                //if (lblRemarks.Text != "")
                //{
                //    if (lblRemarks.Text.Length > 150)
                //    {
                //        lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                //        imgreadmore.Visible = true;
                //    }
                //    else
                //    {
                //        imgreadmore.Visible = false;
                //    }
                //}
                //if (lblAudit.Text != "")
                //{
                //    if (lblAudit.Text.Length > 150)
                //    {
                //        lblAudit.Text = lblAudit.Text.Substring(0, 150) + "....";
                //        imgreadmore1.Visible = true;
                //    }
                //    else
                //    {
                //        imgreadmore1.Visible = false;
                //    }
                //}

                if (defaultPage.RoleID == 1)
                {
                    if (status == "Pending Approval" || status == "Hold" || status == "Pending Order")
                    {
                        imgbtnedit.Visible = true;
                    }
                    else
                    {
                        imgbtnedit.Visible = false;
                    }
                }
                if (defaultPage.RoleID != 1)
                {
                    if (status == "Pending Approval")
                    {
                        imgbtnedit.Visible = true;
                    }
                    else
                    {
                        imgbtnedit.Visible = false;
                    }
                }

            }
        }
        protected void GvTempEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string status = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                status = e.Row.Cells[6].Text;
                //string b ="<img src= \"Images/Readmore.png\" />";
                //Label lblAudit = (Label)e.Row.FindControl("lblAudit");
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                ImageButton TempbtnEdit = (ImageButton)e.Row.FindControl("TempbtnEdit");

                if (status == "Ordered and Pending Receive" || status == "Denied")
                {
                    TempbtnEdit.Visible = false;
                }
                else
                {
                    TempbtnEdit.Visible = true;
                }
                //if (lblRemarks.Text != "")
                //{
                //    if (lblRemarks.Text.Length > 150)
                //    {
                //        lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                //        imgreadmore.Visible = true;
                //    }
                //    else
                //    {
                //        imgreadmore.Visible = false;
                //    }
                //}
                //if (lblAudit.Text != "")
                //{
                //    if (lblAudit.Text.Length > 150)
                //    {
                //        lblAudit.Text = lblAudit.Text.Substring(0, 150) + "....";
                //        imgreadmore1.Visible = true;
                //    }
                //    else
                //    {
                //        imgreadmore1.Visible = false;
                //    }
                //}
                if (defaultPage.RoleID == 1)
                {
                    if (status == "Pending Approval" || status == "Hold" || status == "Pending Order")
                    {
                        TempbtnEdit.Visible = true;
                    }
                    else
                    {
                        TempbtnEdit.Visible = false;
                    }
                }
                if (defaultPage.RoleID != 1)
                {
                    if (status == "Pending Approval")
                    {
                        TempbtnEdit.Visible = true;
                    }
                    else
                    {
                        TempbtnEdit.Visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// Facility dropdown is binded according to the corporate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region corporate dropdown SelectedIndexChanged event
        protected void drpcor_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i = 0;

            foreach (ListItem lst in drpcor.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }


            if (i == 1)
            {
                BindFacility(1, "Add");
                foreach (ListItem lst in drpcor.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListCorpID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (ListItem lst in drpcor.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    if (HddListCorpID.Value == lst.Value)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }

                }
            }
            else
            {
                foreach (ListItem lst in drpcor.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListCorpID.Value = "";
                }
                BindFacility(1, "Add");
            }
        }
        #endregion


        /// <summary>
        /// Drop down event to bind vendor details based on selected facility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region facility dropdown SelectedIndexChanged event
        protected void drpfacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ListItem lst in drpfacility.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }

            if (i == 1)
            {
                BindVendor(1, "Add");
                foreach (ListItem lst in drpfacility.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListFacID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (ListItem lst in drpfacility.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    if (HddListFacID.Value == lst.Value)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }

                }
            }
            else
            {
                foreach (ListItem lst in drpfacility.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListFacID.Value = "";
                }
                BindVendor(1, "Add");
            }
        }
        #endregion


        public void BindEquipcategory(Int64 CorporateID)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipmentCategory> lstequipcat = lclsservice.GetEquipmentCategory(CorporateID, "Add").ToList();
                ddlEquipmentCategory.DataSource = lstequipcat;
                ddlEquipmentCategory.DataValueField = "EquipmentCategoryID";
                ddlEquipmentCategory.DataTextField = "EquipmentCatDescription";
                ddlEquipmentCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Equipment Category--";
                ddlEquipmentCategory.Items.Insert(0, lst);
                ddlEquipmentCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void GetEquipmentSubCategory()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementSubCategory> lstequipsub = lclsservice.GetEquipementSubCategory(Convert.ToInt64(ddlEquipmentCategory.SelectedValue), "Add").ToList();
                ddlEquipmentSubCat.DataSource = lstequipsub;
                ddlEquipmentSubCat.DataValueField = "EquipementSubCategoryID";
                ddlEquipmentSubCat.DataTextField = "EquipmentSubCategoryDescription";
                ddlEquipmentSubCat.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Equipment Sub Category--";
                ddlEquipmentSubCat.Items.Insert(0, lst);
                ddlEquipmentSubCat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void GetEquipementList()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementList> lstequiplist = lclsservice.GetEquipementList(Convert.ToInt64(ddlEquipmentSubCat.SelectedValue), "Add").ToList();
                ddlEquipmentList.DataSource = lstequiplist;
                ddlEquipmentList.DataValueField = "EquipementListID";
                ddlEquipmentList.DataTextField = "EquipmentListDescription";
                ddlEquipmentList.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Equipment List--";
                ddlEquipmentList.Items.Insert(0, lst);
                ddlEquipmentList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void GetSerialhrsfromMachineMaster()
        {
            try
            {
                if (ddlCorporate.SelectedValue != "0" && ddlEquipmentCategory.SelectedValue != "0" && ddlEquipmentSubCat.SelectedValue != "0" && ddlEquipmentList.SelectedValue != "0")
                {
                    List<GetMachinemasterDetails> lstequiplist = lclsservice.GetMachinemasterDetails().Where(a => a.FacilityID == Convert.ToInt64(ddlFacility.SelectedValue) && a.EquipmentCategoryID == Convert.ToInt64(ddlEquipmentCategory.SelectedValue) && a.EquipementSubCategoryID == Convert.ToInt64(ddlEquipmentSubCat.SelectedValue) && a.EquipementListID == Convert.ToInt64(ddlEquipmentList.SelectedValue) && a.IsActive == true).ToList();
                    if (lstequiplist.Count > 0)
                    {
                        txtHoursonmachine.Text = "";
                        if (ddlEquipmentCategory.SelectedValue == "1")
                        {
                            txtHoursonmachine.ReadOnly = true;
                        }
                        else
                        {
                            txtHoursonmachine.ReadOnly = true;
                        }
                        txtHoursonmachine.Text = lstequiplist[0].Hoursonthemachine.ToString();
                        txtSerialNo.Text = lstequiplist[0].SerialNo;
                    }
                    else
                    {
                        txtHoursonmachine.ReadOnly = true;
                        txtHoursonmachine.Text = "";
                        txtSerialNo.Text = "";
                    }
                }
                else
                {
                    txtHoursonmachine.ReadOnly = true;
                    txtHoursonmachine.Text = "";
                    txtSerialNo.Text = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void ddlEquipmentCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipmentSubCategory();
            GetSerialhrsfromMachineMaster();
            RebindGrid();
        }

        protected void ddlEquipmentSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipementList();
            GetSerialhrsfromMachineMaster();
            RebindGrid();
        }

        protected void ddlEquipmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSerialhrsfromMachineMaster();
            RebindGrid();
        }

        protected void ddlFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendor(0, "Add");
            GetSerialhrsfromMachineMaster();
            RebindGrid();
        }

        protected void ddlCorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
            RebindGrid();
            BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue));
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
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

                    }

                    System.IO.File.Delete(path);
                    Response.End();
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalOrderMessage.Replace("<<CapitalOrder>>", ex.Message), true);
            }

        }


        public List<object> SearchOrderReport(string MPRMasterID)
        {
            string smedmasterIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<BindMachinePartsReport> llstreview = new List<BindMachinePartsReport>();
            if (MPRMasterID == "")
            {
                foreach (GridViewRow row in grdMPRMaster.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (smedmasterIds == string.Empty)
                            smedmasterIds = row.Cells[16].Text;
                        else
                            smedmasterIds = smedmasterIds + "," + row.Cells[16].Text;
                    }
                }

                llstreview = lclsservice.BindMachinePartsReport(null, smedmasterIds, defaultPage.UserId, defaultPage.UserId).ToList();
            }
            else
            {
                llstreview = lclsservice.BindMachinePartsReport(MPRMasterID, null, defaultPage.UserId, defaultPage.UserId).ToList();
            }
            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvmachinepartsreport.ProcessingMode = ProcessingMode.Local;
            rvmachinepartsreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsReview.rdlc");
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("MPRMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

            this.rvmachinepartsreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MachinePartsReviewDS", llstreview);
            rvmachinepartsreport.LocalReport.DataSources.Clear();
            rvmachinepartsreport.LocalReport.DataSources.Add(datasource);
            rvmachinepartsreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvmachinepartsreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = SearchOrderReport("");
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                _sessionPDFFileName = "SessionFile" + guid + ".pdf";
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
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
                RebindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        private void ClearMaster()
        {
            drpvendor.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatus.ClearSelection();
            BindStatus("Add");
            BindCorporate(1,"Add");
            BindFacility(1,"Add");
            BindVendor(1, "Add");
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }


        private void ClearDetails()
        {
            ddlVendor.ClearSelection();
            ddlEquipmentCategory.ClearSelection();
            ddlEquipmentSubCat.ClearSelection();
            ddlEquipmentList.ClearSelection();
            ddlShipping.ClearSelection();
            ddlCorporate.ClearSelection();
            ddlFacility.ClearSelection();
            //ddlstatus.ClearSelection();
            txtSerialNo.Text = "";
            txtHoursonmachine.Text = "";
            gvAddMRPDetails.DataSource = null;
            gvAddMRPDetails.DataBind();
            gvSearchMRPDetails.DataSource = null;
            gvSearchMRPDetails.DataBind();
            GvTempEdit.DataSource = null;
            GvTempEdit.DataBind();
            txtHoursonmachine.ReadOnly = true;
            btnPrint.Visible = false;
            ddlCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
            BindFacility(0, "Add");
            ddlFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
            BindVendor(0, "Add");
            ViewState["ReportMachinePartsID"] = "";
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        private void SetAddScreen(int i)
        {
            try
            {
                if (i == 1)
                {
                    if (defaultPage.Req_MachinePartsPage_Edit == true && defaultPage.Req_MachinePartsPage_View == true)
                    {
                        //if (defaultPage.RoleID == 1)
                        //{
                        btnSave.Visible = true;
                        btnReview.Visible = true;
                        btnSearchNewRow.Visible = true;
                        btnAdd.Visible = false;
                        //ddlCorporate.Enabled = true;
                        //ddlFacility.Enabled = true;
                        //}
                        //else
                        //{
                        //    btnSave.Visible = true;
                        //    btnReview.Visible = true;
                        //    btnSearchNewRow.Visible = true;
                        //    btnAdd.Visible = false;
                        //ddlCorporate.Enabled = false;
                        //ddlFacility.Enabled = false;

                        //}

                    }
                    if (defaultPage.Req_MachinePartsPage_Edit == false && defaultPage.Req_MachinePartsPage_View == true)
                    {
                        btnSave.Visible = false;
                        btnReview.Visible = false;
                        btnSearchNewRow.Visible = true;
                        btnAdd.Visible = false;
                        //ddlCorporate.Enabled = false;
                        //ddlFacility.Enabled = false;
                    }

                    // btnPrint.Visible = false;
                    btnClose.Visible = true;
                    btnSearch.Visible = false;
                    divAddMachine.Style.Add("display", "block");
                    lblUpdateHeader.Visible = true;
                    lblMasterHeader.Visible = false;
                    divMPRDetails.Style.Add("display", "block");
                    divSearchMachine.Style.Add("display", "none");
                    lblseroutHeader.Visible = false;
                    divMPRMaster.Style.Add("display", "none");
                    divEdit.Style.Add("display", "none");
                    ddlVendor.Enabled = true;
                    ddlShipping.Enabled = true;
                    ddlEquipmentCategory.Enabled = true;
                    ddlEquipmentSubCat.Enabled = true;
                    ddlEquipmentList.Enabled = true;
                }
                else
                {
                    if (defaultPage.Req_MachinePartsPage_Edit == true && defaultPage.Req_MachinePartsPage_View == true)
                    {
                        btnSave.Visible = false;
                        btnReview.Visible = false;
                        btnAdd.Visible = true;
                        btnSearchNewRow.Visible = false;
                    }
                    // btnPrint.Visible = true;
                    btnClose.Visible = true;
                    btnSearch.Visible = true;
                    BindCorporate(1, "Add");
                    drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                    BindFacility(1, "Add");
                    drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                    BindVendor(1, "Add");
                    lblUpdateHeader.Visible = false;
                    lblMasterHeader.Visible = true;
                    lblEditHeader.Visible = false;
                    lblMasterNo.Visible = false;
                    DivMPRMasterNo.Style.Add("display", "none");
                    divMPRDetails.Style.Add("display", "none");
                    lblseroutHeader.Visible = true;
                    divMPRMaster.Style.Add("display", "block");
                    BindGrid(0);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lclsservice.SyncMachinePartsReceivingorder();
                SetAddScreen(1);
                ClearDetails();
                SetInitialRow();
                BindShipping("Add");
                //BindCorporate(0, "Add");
                //BindFacility(0, "Add");
                //if (drpcor.SelectedValue != "0")
                //{
                //    ddlCorporate.SelectedValue = drpcor.SelectedValue;
                //    BindFacility(0, "Add");
                //    if (drpfacility.SelectedValue != "0")
                //    {
                //        ddlFacility.SelectedValue = drpfacility.SelectedValue;
                //        BindVendor(0, "Add");
                //        if (drpvendor.SelectedValue != "0")
                //        {
                //            ddlVendor.SelectedValue = drpvendor.SelectedValue;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (HddQueryStringID.Value != "")
            {
                Response.Redirect("MachinePartsOrder.aspx");
            }
            if (HddUpdateLockinEdit.Value == "Edit")
            {
                a = lclsservice.AutoUpdateLockedOut(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId), "MachineParts");
                HddUpdateLockinEdit.Value = "";
            }
            SetAddScreen(0);
            ClearDetails();
            ClearMaster();
            HddMasterID.Value = "";
            // btnPrint.Visible = false;
            btnPrint.Visible = true;
            btnReview.Enabled = true;
            if (defaultPage.RoleID != 1)
            {
                btnReview.Visible = false;
                btnAdd.Visible = true;
                SearchGrid();
                //List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                //grdMPRMaster.DataSource = lstMPRMaster;
                //grdMPRMaster.DataBind();
                if (defaultPage.Req_MachinePartsPage_Edit == false && defaultPage.Req_MachinePartsPage_View == true)
                {
                    btnAdd.Visible = false;
                }
            }
            else
            {
                SearchGrid();
            }

        }
        private void SetInitialRow()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));
                dt.Columns.Add(new DataColumn("Column3", typeof(string)));
                dt.Columns.Add(new DataColumn("Column4", typeof(string)));
                dt.Columns.Add(new DataColumn("Column5", typeof(string)));
                dt.Columns.Add(new DataColumn("Column6", typeof(string)));
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;

                //Bind the DataTable to the Grid
                gvAddMRPDetails.DataSource = dt;
                gvAddMRPDetails.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void RebindGrid()
        {
            try
            {
                //DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                //Rebind the Grid with the current data to reflect changes  
                if (HddMasterID.Value == "")
                {
                    for (int i = 0; i <= gvAddMRPDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvAddMRPDetails.Rows[Convert.ToInt32(i)];
                        TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                        TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                        TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                        TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                        TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                        TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");
                        if (TotalPrice.Text == "")
                        {
                            CalTotalPrice(TotalPrice);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= gvSearchMRPDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvSearchMRPDetails.Rows[Convert.ToInt32(i)];
                        TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                        TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                        TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                        TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                        TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                        TextBox TotalPrice = (TextBox)grdfs.FindControl("TotalPrice");
                        if (TotalPrice.Text == "")
                        {
                            CalTotalPriceUpdate(TotalPrice);
                        }
                    }

                    if (gvSearchMRPDetails.FooterRow.Visible == true)
                    {

                        Control control = null;
                        if (gvSearchMRPDetails.FooterRow != null)
                        {
                            control = gvSearchMRPDetails.FooterRow;
                        }
                        else
                        {
                            control = gvSearchMRPDetails.Controls[0].Controls[0];
                        }
                        TextBox ItemID = control.FindControl("FootItemID") as TextBox;
                        TextBox ItemDescription = control.FindControl("FootItemDescription") as TextBox;
                        TextBox UOM = control.FindControl("FootUOM") as TextBox;
                        TextBox PricePerUnit = control.FindControl("FootPricePerUnit") as TextBox;
                        TextBox OrderQuantity = control.FindControl("FootOrderQuantity") as TextBox;
                        TextBox TotalPrice = control.FindControl("FootTotalPrice") as TextBox;
                        if (TotalPrice.Text == "")
                        {
                            CalFootRow();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void AddNewRowToGrid()
        {
            try
            {
                if (ViewState["CurrentTable"] != null)
                {

                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;

                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;

                        //add new row to DataTable   
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        //Store the current data to ViewState for future reference   

                        ViewState["CurrentTable"] = dtCurrentTable;
                    }
                    for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                    {
                        //extract the TextBox values   

                        TextBox box1 = (TextBox)gvAddMRPDetails.Rows[i].Cells[1].FindControl("ItemID");
                        TextBox box2 = (TextBox)gvAddMRPDetails.Rows[i].Cells[2].FindControl("ItemDescription");
                        TextBox box3 = (TextBox)gvAddMRPDetails.Rows[i].Cells[3].FindControl("UOM");
                        TextBox box4 = (TextBox)gvAddMRPDetails.Rows[i].Cells[4].FindControl("PricePerUnit");
                        TextBox box5 = (TextBox)gvAddMRPDetails.Rows[i].Cells[5].FindControl("OrderQuantity");
                        TextBox box6 = (TextBox)gvAddMRPDetails.Rows[i].Cells[6].FindControl("ATotalPriceValue");

                        dtCurrentTable.Rows[i]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i]["Column2"] = box2.Text;
                        dtCurrentTable.Rows[i]["Column3"] = box3.Text;
                        dtCurrentTable.Rows[i]["Column4"] = box4.Text;
                        dtCurrentTable.Rows[i]["Column5"] = box5.Text;
                        dtCurrentTable.Rows[i]["Column6"] = box6.Text;
                    }

                    //Rebind the Grid with the current data to reflect changes   
                    gvAddMRPDetails.DataSource = dtCurrentTable;
                    gvAddMRPDetails.DataBind();
                }
                else
                {
                    Response.Write("ViewState is null");

                }
                //Set Previous Data on Postbacks   
                SetPreviousData();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        private void SetPreviousData()
        {
            try
            {
                int rowIndex = 0;
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox box1 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[1].FindControl("ItemID");
                            TextBox box2 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[2].FindControl("ItemDescription");
                            TextBox box3 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[3].FindControl("UOM");
                            TextBox box4 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[4].FindControl("PricePerUnit");
                            TextBox box5 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[5].FindControl("OrderQuantity");
                            TextBox box6 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[6].FindControl("ATotalPriceValue");

                            box1.Text = dt.Rows[i]["Column1"].ToString();
                            box2.Text = dt.Rows[i]["Column2"].ToString();
                            box3.Text = dt.Rows[i]["Column3"].ToString();
                            box4.Text = dt.Rows[i]["Column4"].ToString();
                            box5.Text = dt.Rows[i]["Column5"].ToString();
                            box6.Text = dt.Rows[i]["Column6"].ToString();
                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnAddDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton DelRow = (ImageButton)sender;
                GridViewRow gvRow = (GridViewRow)DelRow.NamingContainer;
                int rowID = gvRow.RowIndex;

                SetRowData();
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    int rowIndex = Convert.ToInt32(gvRow.RowIndex);
                    if (dt.Rows.Count > 1)
                    {
                        dt.Rows.Remove(dt.Rows[rowIndex]);
                        drCurrentRow = dt.NewRow();
                        ViewState["CurrentTable"] = dt;
                        gvAddMRPDetails.DataSource = dt;
                        gvAddMRPDetails.DataBind();

                        for (int i = 0; i < gvAddMRPDetails.Rows.Count - 1; i++)
                        {
                            gvAddMRPDetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        }
                        SetPreviousData();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        private void SetRowData()
        {
            try
            {
                int rowIndex = 0;

                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {

                            TextBox box1 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[1].FindControl("ItemID");
                            TextBox box2 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[2].FindControl("ItemDescription");
                            TextBox box3 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[3].FindControl("UOM");
                            TextBox box4 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[4].FindControl("PricePerUnit");
                            TextBox box5 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[5].FindControl("OrderQuantity");
                            TextBox box6 = (TextBox)gvAddMRPDetails.Rows[rowIndex].Cells[6].FindControl("ATotalPriceValue");

                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["RowNumber"] = i + 1;
                            dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                            dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                            dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                            dtCurrentTable.Rows[i - 1]["Column4"] = box4.Text;
                            dtCurrentTable.Rows[i - 1]["Column5"] = box5.Text;
                            dtCurrentTable.Rows[i - 1]["Column6"] = box6.Text;
                            rowIndex++;
                        }

                        ViewState["CurrentTable"] = dtCurrentTable;
                    }
                }
                else
                {
                    Response.Write("ViewState is null");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow grdfs in gvAddMRPDetails.Rows)
                {
                    TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                    TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                    TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                    TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                    TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                    TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");
                    if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }

                }
                if (ErrorList == "")
                {
                    AddNewRowToGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }

        }
        public void UpdateMPR()
        {
            try
            {
                if (gvSearchMRPDetails.PageCount == 0)
                {
                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddMRPDetails.Rows)
                    {
                        TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                        TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                        TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                        TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                        TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                        TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");

                        llstMPRMaster.ItemID = Convert.ToString(ItemID.Text);
                        llstMPRMaster.ItemDescription = Convert.ToString(ItemDescription.Text);
                        llstMPRMaster.UOM = Convert.ToString(UOM.Text);
                        llstMPRMaster.PricePerUnit = Convert.ToDecimal(PricePerUnit.Text);
                        llstMPRMaster.OrderQuantity = Convert.ToInt32(OrderQuantity.Text);
                        llstMPRMaster.TotalPrice = Convert.ToDecimal(TotalPrice.Text);
                        llstMPRMaster.MPRMasterID = Convert.ToInt64(HddMasterID.Value);
                        // Insert Machine Parts Request Details
                        a = lclsservice.InsertMPRDetails(llstMPRMaster);
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                    {
                        TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                        TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                        TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                        TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                        TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                        TextBox TotalPrice = (TextBox)grdfs.FindControl("TotalPrice");
                        if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }
                    }
                    if (ErrorList == "")
                    {
                        //Get Grid Details Value
                        foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                        {
                            TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                            TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                            TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                            TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                            TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                            TextBox TotalPrice = (TextBox)grdfs.FindControl("TotalPrice");
                            Label MPRMasterID = (Label)grdfs.FindControl("lbMPRMasterID");
                            Label MPRDetailsID = (Label)grdfs.FindControl("lbMPRDetailsID");
                            llstMPRMaster.MPRMasterID = Convert.ToInt64(MPRMasterID.Text);
                            llstMPRMaster.MPRDetailID = Convert.ToInt64(MPRDetailsID.Text);
                            llstMPRMaster.ItemID = Convert.ToString(ItemID.Text);
                            llstMPRMaster.ItemDescription = Convert.ToString(ItemDescription.Text);
                            llstMPRMaster.UOM = Convert.ToString(UOM.Text);
                            llstMPRMaster.PricePerUnit = Convert.ToDecimal(PricePerUnit.Text);
                            llstMPRMaster.OrderQuantity = Convert.ToInt32(OrderQuantity.Text);
                            llstMPRMaster.TotalPrice = Convert.ToDecimal(TotalPrice.Text);
                            // Update Machine Parts Request Details
                            b = lclsservice.UpdateMPRDetails(llstMPRMaster);
                            // ViewState["ReportMachinePartsID"] = llstMPRMaster.MPRMasterID + ",";
                            //  ViewState["SerachFilters"] = llstMPRMaster.CorporateID + "," + llstMPRMaster.FacilityID + "," + llstMPRMaster.VendorID + "," + llstMPRMaster.DateFrom + "," + llstMPRMaster.DateTo + "," + llstMPRMaster.Status;
                        }
                        // Update Machine Parts Request Master
                        a = lclsservice.UpdateMPRMaster(llstMPRMaster);
                        // ViewState["ReportMachinePartsID"] = llstMPRMaster.MPRMasterID;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                    }
                    if (gvSearchMRPDetails.FooterRow.Visible == true)
                    {
                        FooterRowSave();

                    }
                }

                if (a == "Saved Successfully")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestSaveMessage.Replace("<<MachinePartsRequestDescription>>", lblmprreview.Text), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    //btnPrint.Visible = true;
                }
                else if (b == "Updated Successfully")
                {
                    List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestMessage.Replace("<<MachinePartsRequestDescription>>", lblmprreview.Text), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    btnClose.Visible = true;
                }
                if (HddQueryStringID.Value != "")
                {
                    btnPrint.Visible = false;
                }
                else
                {
                    btnPrint.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }

        }

        public void InsertMPR()
        {
            try
            {
                List<object> lstIDwithmessage = new List<object>();
                foreach (GridViewRow grdfs in gvAddMRPDetails.Rows)
                {
                    TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                    TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                    TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                    TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                    TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                    TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");
                    if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }
                if (ErrorList == "")
                {
                    // Insert Machine Parts Request Master
                    lstIDwithmessage = lclsservice.InsertMPRMaster(llstMPRMaster).ToList();
                    a = lstIDwithmessage[0].ToString();
                    llstMPRMaster.MPRMasterID = Convert.ToInt64(lstIDwithmessage[1]);

                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddMRPDetails.Rows)
                    {
                        TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                        TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                        TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                        TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                        TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                        TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");

                        llstMPRMaster.ItemID = Convert.ToString(ItemID.Text);
                        llstMPRMaster.ItemDescription = Convert.ToString(ItemDescription.Text);
                        llstMPRMaster.UOM = Convert.ToString(UOM.Text);
                        llstMPRMaster.PricePerUnit = Convert.ToDecimal(PricePerUnit.Text);
                        llstMPRMaster.OrderQuantity = Convert.ToInt32(OrderQuantity.Text);
                        llstMPRMaster.TotalPrice = Convert.ToDecimal(TotalPrice.Text);

                        // Insert Machine Parts Request Details
                        b = lclsservice.InsertMPRDetails(llstMPRMaster);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                }
                if (a == b && b == "Saved Successfully")
                {
                    ClearDetails();
                    btnPrint.Visible = true;
                    SetAddScreen(0);
                    List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestSaveMessage.Replace("<<MachinePartsRequestDescription>>", lstMPRMaster[0].MPRNo.ToString()), true);
                    ViewState["ReportMachinePartsID"] = llstMPRMaster.MPRMasterID;
                    //btnPrint.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                llstMPRMaster.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                llstMPRMaster.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                llstMPRMaster.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                llstMPRMaster.EquipmentCategoryID = Convert.ToInt64(ddlEquipmentCategory.SelectedValue);
                llstMPRMaster.EquipementSubCategoryID = Convert.ToInt64(ddlEquipmentCategory.SelectedValue);
                llstMPRMaster.EquipementSubCategoryID = Convert.ToInt64(ddlEquipmentSubCat.SelectedValue);
                llstMPRMaster.EquipementListID = Convert.ToInt64(ddlEquipmentList.SelectedValue);
                llstMPRMaster.SerialNo = txtSerialNo.Text;
                llstMPRMaster.Shipping = ddlShipping.SelectedValue;
                //llstMPRMaster.StatusID = Convert.ToInt64(ddlstatus.SelectedValue);
                llstMPRMaster.Hoursonthemachine = txtHoursonmachine.Text;
                llstMPRMaster.CreatedBy = defaultPage.UserId;
                llstMPRMaster.ShippingCost = (grdreview.FooterRow.FindControl("txtsipcost") as TextBox).Text;
                llstMPRMaster.Tax = (grdreview.FooterRow.FindControl("txttax") as TextBox).Text;
                llstMPRMaster.TotalCost = Convert.ToDecimal((grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text);

                if (HddMasterID.Value == "")
                {
                    InsertMPR();
                }
                else
                {
                    UpdateMPR();

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        private void EditDisplayControls()
        {
            try
            {
                SetAddScreen(1);
                BindGrid(1);
                //if (defaultPage.RoleID == 1)
                //{
                //    BindGrid(1);
                //}
                //else
                //{
                //    List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                //    GvTempEdit.DataSource = lstMPRMaster;
                //    GvTempEdit.DataBind();

                //}

                DivMPRMasterNo.Style.Add("display", "block");
                divEdit.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "block");
                divAddMachine.Style.Add("display", "none");
                txtSerialNo.ReadOnly = true;
                txtHoursonmachine.ReadOnly = true;
                lblMasterNo.Visible = true;
                ddlCorporate.Enabled = false;
                ddlFacility.Enabled = false;
                ddlVendor.Enabled = false;
                ddlShipping.Enabled = false;
                ddlEquipmentCategory.Enabled = false;
                ddlEquipmentSubCat.Enabled = false;
                ddlEquipmentList.Enabled = false;
                txtHoursonmachine.Enabled = false;
                btnPrint.Visible = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EditDisplayControls();
                hdncheckfield.Value = "1";
                // btnPrint.ValidationGroup = "";
                lblEditHeader.Visible = true;
                lblseroutHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblMasterHeader.Visible = true;
                HddUpdateLockinEdit.Value = "Edit";
                //btnPrint.Visible = true;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                HddMasterID.Value = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["ReportMachinePartsID"] = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                lblMasterNo.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                ddlCorporate.ClearSelection();
                if (gvrow.Cells[7].Text == "&nbsp;")
                {
                    ddlCorporate.Items.FindByText("--Select Corporate--").Selected = true;
                }
                else
                {
                    ddlCorporate.SelectedValue = gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "");
                }
                BindFacility(0, "Edit");
                ddlFacility.ClearSelection();
                if (gvrow.Cells[8].Text == "&nbsp;")
                {
                    ddlFacility.Items.FindByText("--Select Facility--").Selected = true;
                }
                else
                {
                    ddlFacility.SelectedValue = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                }
                BindVendor(0, "Edit");
                ddlVendor.ClearSelection();
                if (gvrow.Cells[9].Text == "&nbsp;")
                {
                    ddlVendor.Items.FindByText("--Select Vendor--").Selected = true;
                }
                else
                {
                    ddlVendor.SelectedValue = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                }
                BindShipping("Edit");
                ddlShipping.ClearSelection();
                BindShipping("Edit");
                if (gvrow.Cells[14].Text == "&nbsp;")
                {
                    ddlShipping.Items.FindByText("--Select Shipping--").Selected = true;
                }
                else
                {
                    ddlShipping.SelectedValue = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                }
                ddlEquipmentCategory.ClearSelection();
                if (gvrow.Cells[10].Text == "&nbsp;")
                {
                    ddlEquipmentCategory.Items.FindByText("--Select Equipment Category--").Selected = true;
                }
                else
                {
                    ddlEquipmentCategory.SelectedValue = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                    if (ddlEquipmentCategory.SelectedValue == "1")
                    {
                        txtHoursonmachine.Text = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                        txtHoursonmachine.ReadOnly = false;
                    }
                    else
                    {
                        txtHoursonmachine.Text = "";
                        txtHoursonmachine.ReadOnly = true;
                    }
                }
                GetEquipmentSubCategory();
                ddlEquipmentSubCat.ClearSelection();
                if (gvrow.Cells[20].Text == "&nbsp;")
                {
                    ddlEquipmentSubCat.Items.FindByText("--Select Equipment Sub Category--").Selected = true;
                }
                else
                {
                    ddlEquipmentSubCat.SelectedValue = gvrow.Cells[20].Text.Trim().Replace("&nbsp;", "");
                }
                GetEquipementList();
                ddlEquipmentList.ClearSelection();
                if (gvrow.Cells[11].Text == "&nbsp;")
                {
                    ddlEquipmentList.Items.FindByText("--Select Equipment List--").Selected = true;
                }
                else
                {
                    ddlEquipmentList.SelectedValue = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                }
                txtSerialNo.Text = gvrow.Cells[13].Text.Trim().Replace("&nbsp;", "");
                txtHoursonmachine.Text = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void CalTotalPrice(TextBox CalTotal)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)CalTotal.NamingContainer;
                int index = Convert.ToInt16(gvr.RowIndex);
                Int32 OrderQuantity = 0;
                decimal PricePerUnit = 0;
                TextBox txtPricePerUnit = (TextBox)gvAddMRPDetails.Rows[index].FindControl("PricePerUnit");
                TextBox txtOrderQuantity = (TextBox)gvAddMRPDetails.Rows[index].FindControl("OrderQuantity");
                TextBox txtTotalPrice = (TextBox)gvAddMRPDetails.Rows[index].FindControl("ATotalPriceValue");
                if (txtPricePerUnit.Text != "")
                {
                    PricePerUnit = Convert.ToDecimal(txtPricePerUnit.Text);
                }
                if (txtOrderQuantity.Text != "")
                {
                    OrderQuantity = Convert.ToInt32(txtOrderQuantity.Text);
                }
                decimal de = (PricePerUnit * OrderQuantity);
                txtTotalPrice.Text = string.Format("{0:F2}", de).ToString();

                if (txtPricePerUnit.Text == "" || txtOrderQuantity.Text == "")
                {
                    txtTotalPrice.Text = "";
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }


        public void CalTotalPriceUpdate(TextBox CalTotal1)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)CalTotal1.NamingContainer;
                int index = Convert.ToInt16(gvr.RowIndex);
                decimal PricePerUnit = 0;
                Int32 OrderQuantity = 0;
                TextBox txtPricePerUnit = (TextBox)gvr.Cells[index].FindControl("PricePerUnit");
                TextBox txtOrderQuantity = (TextBox)gvr.Cells[index].FindControl("OrderQuantity");
                TextBox txtTotalPrice = (TextBox)gvr.Cells[index].FindControl("TotalPrice");
                if (txtPricePerUnit.Text != "")
                {
                    PricePerUnit = Convert.ToDecimal(txtPricePerUnit.Text);
                }
                if (txtOrderQuantity.Text != "")
                {
                    OrderQuantity = Convert.ToInt32(txtOrderQuantity.Text);
                }

                decimal de = (PricePerUnit * OrderQuantity);
                txtTotalPrice.Text = string.Format("{0:F2}", de).ToString();
                //txtOrderQuantity.Focus();

                if (txtPricePerUnit.Text == "" || txtOrderQuantity.Text == "")
                {
                    txtTotalPrice.Text = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        public void CalFootRow()
        {
            try
            {
                Control control = null;
                if (gvSearchMRPDetails.FooterRow != null)
                {
                    control = gvSearchMRPDetails.FooterRow;
                }
                else
                {
                    control = gvSearchMRPDetails.Controls[0].Controls[0];
                }

                string PricePerUnit = (control.FindControl("FootPricePerUnit") as TextBox).Text;
                string OrderQuantity = (control.FindControl("FootOrderQuantity") as TextBox).Text;
                TextBox TotalPrice = (control.FindControl("FootTotalPrice") as TextBox);

                decimal Footde = (Convert.ToDecimal(PricePerUnit) * Convert.ToInt32(OrderQuantity));
                TotalPrice.Text = string.Format("{0:F2}", Footde).ToString();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true); ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void PricePerUnit_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            CalTotalPrice(tb);
        }

        protected void btnSearchDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label MPRDetailsID = (Label)gvrow.FindControl("lbMPRDetailsID");
                HddDetailsID.Value = MPRDetailsID.Text;
                // Label2.Text = "Are you sure you want to delete this Record?";
                //ModalPopupExtender2.Show();
                HddDetailRowID.Value = gvrow.RowIndex.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnYes_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void FootOrderQuantity_TextChanged(object sender, EventArgs e)
        {

            CalFootRow();
        }

        public void FooterRowSave()
        {
            try
            {
                BALMPRMaster llstMPRMaster = new BALMPRMaster();
                foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                {
                    TextBox Item = (TextBox)grdfs.FindControl("ItemID");
                    TextBox ItemDesc = (TextBox)grdfs.FindControl("ItemDescription");
                    TextBox Uom = (TextBox)grdfs.FindControl("UOM");
                    TextBox Price = (TextBox)grdfs.FindControl("PricePerUnit");
                    TextBox Order = (TextBox)grdfs.FindControl("OrderQuantity");
                    TextBox Total = (TextBox)grdfs.FindControl("TotalPrice");
                    Label MPRMasterID = (Label)grdfs.FindControl("lbMPRMasterID");
                    Label MPRDetailsID = (Label)grdfs.FindControl("lbMPRDetailsID");

                    llstMPRMaster.ItemID = Convert.ToString(Item.Text);
                    llstMPRMaster.ItemDescription = Convert.ToString(ItemDesc.Text);
                    llstMPRMaster.UOM = Convert.ToString(Uom.Text);
                    llstMPRMaster.PricePerUnit = Convert.ToDecimal(Price.Text);
                    llstMPRMaster.OrderQuantity = Convert.ToInt32(Order.Text);
                    llstMPRMaster.TotalPrice = Convert.ToDecimal(Total.Text);
                    llstMPRMaster.MPRMasterID = Convert.ToInt64(MPRMasterID.Text);
                    llstMPRMaster.MPRDetailID = Convert.ToInt64(MPRDetailsID.Text);
                    // Insert Machine Parts Request Details
                    a = lclsservice.UpdateMPRDetails(llstMPRMaster);
                }
                Control control = null;
                if (gvSearchMRPDetails.FooterRow != null)
                {
                    control = gvSearchMRPDetails.FooterRow;
                }
                else
                {
                    control = gvSearchMRPDetails.Controls[0].Controls[0];
                }


                string b = string.Empty;
                string ErrorList = string.Empty;
                llstMPRMaster.MPRMasterID = Convert.ToInt64(HddMasterID.Value);
                llstMPRMaster.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                llstMPRMaster.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                llstMPRMaster.EquipmentCategoryID = Convert.ToInt64(ddlEquipmentCategory.SelectedValue);
                llstMPRMaster.EquipementListID = Convert.ToInt64(ddlEquipmentList.SelectedValue);
                llstMPRMaster.CreatedBy = defaultPage.UserId;
                string ItemID = (control.FindControl("FootItemID") as TextBox).Text;
                string ItemDescription = (control.FindControl("FootItemDescription") as TextBox).Text;
                string UOM = (control.FindControl("FootUOM") as TextBox).Text;
                string PricePerUnit = (control.FindControl("FootPricePerUnit") as TextBox).Text;
                string OrderQuantity = (control.FindControl("FootOrderQuantity") as TextBox).Text;
                string TotalPrice = (control.FindControl("FootTotalPrice") as TextBox).Text;
                if (ItemID == "" || ItemDescription == "" || UOM == "" || PricePerUnit == "" || OrderQuantity == "" || TotalPrice == "")
                {
                    ErrorList = "Item Grid fields are should not be Empty";
                }

                if (ErrorList == "")
                {
                    //Get Grid Details Value
                    llstMPRMaster.ItemID = ItemID.ToString();
                    llstMPRMaster.ItemDescription = ItemDescription.ToString();
                    llstMPRMaster.UOM = UOM.ToString();
                    llstMPRMaster.PricePerUnit = Convert.ToDecimal(PricePerUnit);
                    llstMPRMaster.OrderQuantity = Convert.ToInt32(OrderQuantity);
                    llstMPRMaster.TotalPrice = Convert.ToDecimal(TotalPrice);
                    // Insert Machine Parts Request Details
                    b = lclsservice.InsertMPRDetails(llstMPRMaster);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                }


                if (b == "Saved Successfully")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestSaveMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                }

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);

            }
        }

        protected void btnSaveRow_Click(object sender, ImageClickEventArgs e)
        {
            FooterRowSave();

            //UpdateMPR();
        }

        protected void btnSearchNewRow_Click(object sender, EventArgs e)
        {
            if (gvSearchMRPDetails.PageCount != 0)
            {
                gvSearchMRPDetails.FooterRow.Visible = true;
            }
            else
            {
                divAddMachine.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "none");
                SetInitialRow();
            }

        }

        protected void btnRemoveRow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gvSearchMRPDetails.FooterRow.Visible = false;
                Control control = null;
                if (gvSearchMRPDetails.FooterRow != null)
                {
                    control = gvSearchMRPDetails.FooterRow;
                }
                else
                {
                    control = gvSearchMRPDetails.Controls[0].Controls[0];
                }
                TextBox ItemID = control.FindControl("FootItemID") as TextBox;
                TextBox ItemDescription = control.FindControl("FootItemDescription") as TextBox;
                TextBox UOM = control.FindControl("FootUOM") as TextBox;
                TextBox PricePerUnit = control.FindControl("FootPricePerUnit") as TextBox;
                TextBox OrderQuantity = control.FindControl("FootOrderQuantity") as TextBox;
                TextBox TotalPrice = control.FindControl("FootTotalPrice") as TextBox;
                ItemID.Text = "";
                ItemDescription.Text = "";
                UOM.Text = "";
                PricePerUnit.Text = "";
                OrderQuantity.Text = "";
                TotalPrice.Text = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            if (HddMasterID.Value == "" || gvSearchMRPDetails.PageCount == 0)
            {
                foreach (GridViewRow grdfs in gvAddMRPDetails.Rows)
                {
                    TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                    TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                    TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                    TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                    TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                    TextBox TotalPrice = (TextBox)grdfs.FindControl("ATotalPriceValue");
                    if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }

                }
                if (ErrorList == "")
                {
                   checkisvalid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                }
            }
            else
            {

                foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                {
                    TextBox ItemID = (TextBox)grdfs.FindControl("ItemID");
                    TextBox ItemDescription = (TextBox)grdfs.FindControl("ItemDescription");
                    TextBox UOM = (TextBox)grdfs.FindControl("UOM");
                    TextBox PricePerUnit = (TextBox)grdfs.FindControl("PricePerUnit");
                    TextBox OrderQuantity = (TextBox)grdfs.FindControl("OrderQuantity");
                    TextBox TotalPrice = (TextBox)grdfs.FindControl("TotalPrice");
                    if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }

                if (gvSearchMRPDetails.FooterRow.Visible == true)
                {

                    Control control = null;
                    if (gvSearchMRPDetails.FooterRow != null)
                    {
                        control = gvSearchMRPDetails.FooterRow;
                    }
                    else
                    {
                        control = gvSearchMRPDetails.Controls[0].Controls[0];
                    }
                    TextBox ItemID = control.FindControl("FootItemID") as TextBox;
                    TextBox ItemDescription = control.FindControl("FootItemDescription") as TextBox;
                    TextBox UOM = control.FindControl("FootUOM") as TextBox;
                    TextBox PricePerUnit = control.FindControl("FootPricePerUnit") as TextBox;
                    TextBox OrderQuantity = control.FindControl("FootOrderQuantity") as TextBox;
                    TextBox TotalPrice = control.FindControl("FootTotalPrice") as TextBox;
                    if (ItemID.Text == "" || ItemDescription.Text == "" || UOM.Text == "" || PricePerUnit.Text == "" || OrderQuantity.Text == "" || TotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }

                if (ErrorList == "")
                {
                    checkisvalid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ""), true);
                }

            }
        }

        public void checkisvalid()
        {
            try
            {
                Int64 Faclityid = 0;
                Int64 revEquipcat = 0;
                Int64 revEquiplist = 0;
                Int64 revEquipsublist = 0;
                bool isvalid = false;
                string equip = string.Empty;

                Faclityid = Convert.ToInt64(ddlFacility.SelectedValue);
                revEquipcat = Convert.ToInt64(ddlEquipmentCategory.SelectedValue);
                revEquipsublist = Convert.ToInt64(ddlEquipmentSubCat.SelectedValue);
                revEquiplist = Convert.ToInt64(ddlEquipmentList.SelectedValue);

                List<ValidMachineEquipment> lstMaster = lclsservice.ValidMachineEquipment(revEquipcat, revEquipsublist, revEquiplist, Faclityid).ToList();
                if (lstMaster[0].Edit == 1)
                {
                    isvalid = true;
                }
                else
                {
                     equip += ddlEquipmentCategory.SelectedItem.Text+",";
                     isvalid = false;
                }

                if(isvalid == true)
                {
                    mpereview.Show();
                    bindreview();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsValidMessage.Replace("<<MachinePartsRequestDescription>>", "" +equip), true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
          
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
        }
        public void bindreview()
        {
            try
            {
                lblCorp.Text = ddlCorporate.SelectedItem.Text;
                lblFac.Text = ddlFacility.SelectedItem.Text;
                lblVen.Text = ddlVendor.SelectedItem.Text;
                lblship.Text = ddlShipping.SelectedItem.Text;
                lblEquip.Text = ddlEquipmentCategory.SelectedItem.Text;
                lblEquipSubCat.Text = ddlEquipmentSubCat.SelectedItem.Text;
                lblEquiplist.Text = ddlEquipmentList.SelectedItem.Text;
                lblSer.Text = txtSerialNo.Text;
                lblhours.Text = txtHoursonmachine.Text;
                lblmprreview.Text = lblMasterNo.Text;
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                dr = null;
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("ItemID");
                dt.Columns.Add("ItemDescription");
                dt.Columns.Add("UOM");
                dt.Columns.Add("PricePerUnit");
                dt.Columns.Add("OrderQuantity");
                dt.Columns.Add("TotalPrice");
                dt.Columns.Add("txtsipcost");
                dt.Columns.Add("txttax");
                dt.Columns.Add("txtTotalcost");

                //Add New Machine parts Requset//
                if (HddMasterID.Value == "" || gvSearchMRPDetails.PageCount == 0)
                {
                    DivMPRMasterNoreview.Style.Add("display", "none");
                    lblmprreview.Visible = false;
                    if (HddMasterID.Value != "")
                    {
                        DivMPRMasterNoreview.Style.Add("display", "block");
                        lblmprreview.Visible = true;
                    }
                    foreach (GridViewRow row in gvAddMRPDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = row.Cells[0].Text;
                        dr["ItemID"] = (row.FindControl("ItemID") as TextBox).Text;
                        dr["ItemDescription"] = (row.FindControl("ItemDescription") as TextBox).Text;
                        dr["UOM"] = (row.FindControl("UOM") as TextBox).Text;
                        dr["PricePerUnit"] = (row.FindControl("PricePerUnit") as TextBox).Text;
                        dr["OrderQuantity"] = (row.FindControl("OrderQuantity") as TextBox).Text;
                        dr["TotalPrice"] = (row.FindControl("ATotalPriceValue") as TextBox).Text;
                        string val = (row.FindControl("ATotalPriceValue") as TextBox).Text;
                        dt.Rows.Add(dr);
                    }
                    grdreview.DataSource = dt;
                    grdreview.DataBind();
                    TextBox txtShippingCost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                    TextBox txtTax = grdreview.FooterRow.FindControl("txttax") as TextBox;
                    TextBox txtTotalCost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                    decimal sum = 0;
                    for (int i = 0; i < grdreview.Rows.Count; ++i)
                    {
                        Label lblTotalPrice = grdreview.Rows[i].FindControl("lblTotalPrice") as Label;
                        sum += Convert.ToDecimal(lblTotalPrice.Text);
                    }
                    //sum += Convert.ToDecimal(llstmpr[0].ShippingCost) + Convert.ToDecimal(llstmpr[0].Tax);
                    (grdreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
                    (grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                }
                //Edit or Update the Exisiting Machine Parts Request//
                else
                {
                    DivMPRMasterNoreview.Style.Add("display", "block");
                    lblmprreview.Visible = true;
                    List<GetMPRMaster> llstmpr = lclsservice.GetMPRMaster().Where(a => a.MPRMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                    foreach (GridViewRow row in gvSearchMRPDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = HddMasterID.Value;
                        dr["ItemID"] = (row.FindControl("ItemID") as TextBox).Text;
                        dr["ItemDescription"] = (row.FindControl("ItemDescription") as TextBox).Text;
                        dr["UOM"] = (row.FindControl("UOM") as TextBox).Text;
                        dr["PricePerUnit"] = (row.FindControl("PricePerUnit") as TextBox).Text;
                        dr["OrderQuantity"] = (row.FindControl("OrderQuantity") as TextBox).Text;
                        dr["TotalPrice"] = (row.FindControl("TotalPrice") as TextBox).Text;
                        string val = (row.FindControl("TotalPrice") as TextBox).Text;
                        dt.Rows.Add(dr);

                    }
                    if (gvSearchMRPDetails.FooterRow.Visible == true)
                    {
                        dr = dt.NewRow();
                        //gvSearchMRPDetails.FooterRow.Visible = false;
                        Control control = null;
                        if (gvSearchMRPDetails.FooterRow != null)
                        {
                            control = gvSearchMRPDetails.FooterRow;
                        }
                        else
                        {
                            control = gvSearchMRPDetails.Controls[0].Controls[0];
                        }
                        TextBox ItemID = control.FindControl("FootItemID") as TextBox;
                        TextBox ItemDescription = control.FindControl("FootItemDescription") as TextBox;
                        TextBox UOM = control.FindControl("FootUOM") as TextBox;
                        TextBox PricePerUnit = control.FindControl("FootPricePerUnit") as TextBox;
                        TextBox OrderQuantity = control.FindControl("FootOrderQuantity") as TextBox;
                        TextBox TotalPrice = control.FindControl("FootTotalPrice") as TextBox;

                        dr["ItemID"] = ItemID.Text;
                        dr["ItemDescription"] = ItemDescription.Text;
                        dr["UOM"] = UOM.Text;
                        dr["PricePerUnit"] = PricePerUnit.Text;
                        dr["OrderQuantity"] = OrderQuantity.Text;
                        dr["TotalPrice"] = TotalPrice.Text;
                        dt.Rows.Add(dr);
                    }
                    grdreview.DataSource = dt;
                    grdreview.DataBind();
                    TextBox txtShippingCost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                    TextBox txtTax = grdreview.FooterRow.FindControl("txttax") as TextBox;
                    TextBox txtTotalCost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                    txtShippingCost.Text = llstmpr[0].ShippingCost;
                    txtTax.Text = llstmpr[0].Tax;
                    txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", llstmpr[0].TotalCost));
                    decimal sum = 0;
                    for (int i = 0; i < grdreview.Rows.Count; ++i)
                    {
                        Label lblTotalPrice = grdreview.Rows[i].FindControl("lblTotalPrice") as Label;
                        sum += Convert.ToDecimal(lblTotalPrice.Text);
                    }
                    (grdreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
                    if (llstmpr[0].ShippingCost != "" && llstmpr[0].Tax != "")
                    {
                        sum += Convert.ToDecimal(llstmpr[0].ShippingCost) + Convert.ToDecimal(llstmpr[0].Tax);
                        (grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                    }
                    else
                    {
                        sum += Convert.ToDecimal(0) + Convert.ToDecimal(0);
                        (grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string MPRMasterID = string.Empty;
                MPRMasterID = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");
                List<object> llstresult = SearchOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "SessionFile" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnClose.Visible = false;
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteMPRDetails(Convert.ToInt64(HddDetailsID.Value),false, defaultPage.UserId);             
                if (lstrMessage == "Deleted Successfully")
                {                  
                    SetSearchGridRowData();
                    DataTable dt = ViewState["CurrentSearchTable"] as DataTable;
                    dt.Rows.RemoveAt(Convert.ToInt32(HddDetailRowID.Value));
                    gvSearchMRPDetails.DataSource = dt;
                    gvSearchMRPDetails.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.DeleteMPRDetailsDeleteMessage.Replace("<<DeleteMPRDetails>>", ""), true);       
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }

        }


        private void SetSearchGridRowData()
        {
            try
            {
                int rowIndex = 0;
                DataTable dtCurrentTable = new DataTable();
                DataRow drCurrentRow = null;

                dtCurrentTable.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("ItemID", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("UOM", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("PricePerUnit", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("OrderQuantity", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("MPRMasterID", typeof(string)));
                dtCurrentTable.Columns.Add(new DataColumn("MPRDetailsID", typeof(string)));
                if (gvSearchMRPDetails.Rows.Count > 0)
                {
                    for (int i = 1; i <= gvSearchMRPDetails.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[1].FindControl("ItemID");
                        TextBox box2 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[2].FindControl("ItemDescription");
                        TextBox box3 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[3].FindControl("UOM");
                        TextBox box4 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[4].FindControl("PricePerUnit");
                        TextBox box5 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[5].FindControl("OrderQuantity");
                        TextBox box6 = (TextBox)gvSearchMRPDetails.Rows[rowIndex].Cells[6].FindControl("TotalPrice");
                        Label lblMstrID = (Label)gvSearchMRPDetails.Rows[rowIndex].Cells[0].FindControl("lbMPRMasterID");
                        Label lblMPRDetailsID = (Label)gvSearchMRPDetails.Rows[rowIndex].Cells[0].FindControl("lbMPRDetailsID");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        drCurrentRow["ItemID"] = box1.Text;
                        drCurrentRow["ItemDescription"] = box2.Text;
                        drCurrentRow["UOM"] = box3.Text;
                        drCurrentRow["PricePerUnit"] = box4.Text;
                        drCurrentRow["OrderQuantity"] = box5.Text;
                        drCurrentRow["TotalPrice"] = box6.Text;
                        drCurrentRow["MPRMasterID"] = lblMstrID.Text;
                        drCurrentRow["MPRDetailsID"] = lblMPRDetailsID.Text;
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        rowIndex++;
                    }

                    ViewState["CurrentSearchTable"] = dtCurrentTable;

                }
                else
                {
                    Response.Write("ViewState is null");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void ChkAllCorp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkAllCorp = (CheckBox)GrdMultiCorp.HeaderRow.FindControl("ChkAllCorp");

            foreach (GridViewRow row in GrdMultiCorp.Rows)

            {

                CheckBox chkmultiCorp = (CheckBox)row.FindControl("chkmultiCorp");

                if (ChkAllCorp.Checked == true)

                {
                    chkmultiCorp.Checked = true;
                }
                else

                {
                    chkmultiCorp.Checked = false;
                }

            }
        }

        protected void btnMultiCorpselect_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst1 in drpcor.Items)
                {
                    lst1.Attributes.Add("class", "");
                    lst1.Selected = false;
                }
                foreach (GridViewRow row in GrdMultiCorp.Rows)
                {
                    CheckBox chkmultiCorp = (CheckBox)row.FindControl("chkmultiCorp");
                    Label lblCorpID = (Label)row.FindControl("lblCorpID");

                    if (chkmultiCorp.Checked == true)
                    {
                        foreach (ListItem lst1 in drpcor.Items)
                        {
                            if (lst1.Value == lblCorpID.Text)
                            {
                                lst1.Attributes.Add("class", "selected");
                                lst1.Selected = true;
                            }
                        }
                    }
                }
                BindFacility(1, "Add");
                DivMultiCorp.Style.Add("display", "none");
                divMPRRequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divMPRRequest.Attributes["class"] = "mypanel-body";
        }

        protected void ChkAllFac_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkAllFac = (CheckBox)GrdMultiFac.HeaderRow.FindControl("ChkAllFac");

            foreach (GridViewRow row in GrdMultiFac.Rows)

            {

                CheckBox chkmultiFac = (CheckBox)row.FindControl("chkmultiFac");

                if (ChkAllFac.Checked == true)

                {
                    chkmultiFac.Checked = true;
                }
                else

                {
                    chkmultiFac.Checked = false;
                }

            }
        }

        protected void btnMultiFacselect_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst1 in drpfacility.Items)
                {
                    lst1.Attributes.Add("class", "");
                    lst1.Selected = false;
                }
                foreach (GridViewRow row in GrdMultiFac.Rows)
                {
                    CheckBox chkmultiFac = (CheckBox)row.FindControl("chkmultiFac");
                    Label lblFacID = (Label)row.FindControl("lblFacID");

                    if (chkmultiFac.Checked == true)
                    {
                        foreach (ListItem lst1 in drpfacility.Items)
                        {
                            if (lst1.Value == lblFacID.Text)
                            {
                                lst1.Attributes.Add("class", "selected");
                                lst1.Selected = true;
                            }
                        }
                    }
                }
                BindVendor(1, "Add");
                DivFacCorp.Style.Add("display", "none");
                divMPRRequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divMPRRequest.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divMPRRequest.Attributes["class"] = "Upopacity";
                int i = 0;
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    GrdMultiCorp.DataSource = lstcrop;
                    GrdMultiCorp.DataBind();
                    foreach (ListItem lst1 in drpcor.Items)
                    {
                        if (lst1.Selected == true)
                        {

                            foreach (GridViewRow row in GrdMultiCorp.Rows)
                            {
                                Label lblCorpID = (Label)row.FindControl("lblCorpID");
                                CheckBox chkmultiCorp = (CheckBox)row.FindControl("chkmultiCorp");
                                CheckBox ChkAllCorp = GrdMultiCorp.HeaderRow.FindControl("ChkAllCorp") as CheckBox;

                                if (lst1.Value == lblCorpID.Text)
                                {
                                    chkmultiCorp.Checked = true;
                                    i++;
                                }

                                if (i == drpcor.Items.Count)
                                {
                                    ChkAllCorp.Checked = true;
                                }

                            }
                        }
                    }

                }

                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcor.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    GrdMultiCorp.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    GrdMultiCorp.DataBind();
                    foreach (ListItem lst1 in drpcor.Items)
                    {
                        if (lst1.Selected == true)
                        {

                            foreach (GridViewRow row in GrdMultiCorp.Rows)
                            {
                                Label lblCorpID = (Label)row.FindControl("lblCorpID");
                                CheckBox chkmultiCorp = (CheckBox)row.FindControl("chkmultiCorp");
                                CheckBox ChkAllCorp = GrdMultiCorp.HeaderRow.FindControl("ChkAllCorp") as CheckBox;

                                if (lst1.Value == lblCorpID.Text)
                                {
                                    chkmultiCorp.Checked = true;
                                    i++;
                                }

                                if (i == drpcor.Items.Count)
                                {
                                    ChkAllCorp.Checked = true;
                                }

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void lnkClearCorp_Click(object sender, EventArgs e)
        {
            BindCorporate(1, "Add");
            BindFacility(1, "Add");
            BindVendor(1, "Add");
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        protected void lnkClearAllCorp_Click(object sender, EventArgs e)
        {
            foreach (ListItem lst in drpcor.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
            foreach (ListItem lst in drpfacility.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (ListItem lst in drpvendor.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        protected void lnkMultiFac_Click(object sender, EventArgs e)
        {
            int j = 0;
            try
            {
                if (drpcor.SelectedValue != "")
                {
                    foreach (ListItem lst in drpcor.Items)
                    {
                        if (lst.Selected && drpcor.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    // Search Drop Down
                    GrdMultiFac.DataSource = lclsservice.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                    GrdMultiFac.DataBind();
                    DivFacCorp.Style.Add("display", "block");
                    divMPRRequest.Attributes["class"] = "Upopacity";
                    foreach (ListItem lst1 in drpfacility.Items)
                    {
                        if (lst1.Selected == true)
                        {

                            foreach (GridViewRow row in GrdMultiFac.Rows)
                            {
                                Label lblFacID = (Label)row.FindControl("lblFacID");
                                CheckBox chkmultiFac = (CheckBox)row.FindControl("chkmultiFac");
                                CheckBox ChkAllFac = GrdMultiFac.HeaderRow.FindControl("ChkAllFac") as CheckBox;

                                if (lst1.Value == lblFacID.Text)
                                {
                                    chkmultiFac.Checked = true;
                                    j++;
                                }

                                if (j == drpfacility.Items.Count)
                                {
                                    ChkAllFac.Checked = true;
                                }

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void lnkClearFac_Click(object sender, EventArgs e)
        {
            BindFacility(1, "Add");
        }

        protected void lnkClearAllFac_Click(object sender, EventArgs e)
        {
            foreach (ListItem lst in drpfacility.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (ListItem lst in drpvendor.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }

    }
}