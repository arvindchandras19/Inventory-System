#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Net;
using System.Configuration;
using Inventory.Inventoryserref;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using Inventory.Class;
using System.Threading;
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
'' 	09/25/2017		   V1.0				   Vivekanand.S		                  New
'' 10/25/2017           V.01              Vivekanand.S                  Locked the record.
 ''--------------------------------------------------------------------------------
'*/
#endregion



namespace Inventory
{
    public partial class ServiceRequest : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        #endregion
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALServiceRequest llstServiceRequest = new BALServiceRequest();
        string a = string.Empty;
        string b = string.Empty;
        string ErrorList = string.Empty;
        string PendingApproval = Constant.PendingApprovalforreq;
        private string _sessionPDFFileName;
        DataTable Attachment = new DataTable();
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.grdSRMaster);
                scriptManager.RegisterPostBackControl(this.GvTempEdit);
                scriptManager.RegisterPostBackControl(this.GrdUploadFile);
                if (!IsPostBack)
                {
                    //BindCorporate();                    
                    BindLookUp("Add");
                    //BindServiceList("Add");                   
                    if (defaultPage != null)
                    {
                        if (Request.QueryString["SRPOID"] != null)
                        {
                            QueryStringPageLoad();
                        }
                        else
                        {
                            BindCorporate(1, "Add");
                            //drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                            BindFacility(1, "Add");
                            //drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                            BindServiceCategory(Convert.ToInt64(defaultPage.CorporateID), "Add");
                            BindServiceList("Add");
                            BindEquipcategory(Convert.ToInt64(defaultPage.CorporateID), "Add");
                            GetEquipementSubCategory("Add");
                            GetEquipementList("Add");
                            SearchGrid();
                            if (defaultPage.Req_WorkOrServicePage_Edit == false && defaultPage.Req_WorkOrServicePage_View == true)
                            {
                                btnAdd.Visible = false;
                                btnSave.Visible = false;
                            }
                            if (defaultPage.Req_WorkOrServicePage_Edit == false && defaultPage.Req_WorkOrServicePage_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                            }                            
                        }



                        //else
                        //{

                        //BindVendor(1);

                        //if (defaultPage.RoleID == 1)
                        //{
                        //    drpcor.Enabled = true;
                        //    drpfacility.Enabled = true;
                        //    ddlCorporate.Enabled = true;
                        //    ddlFacility.Enabled = true;
                        //    BindGrid(0);
                        //}
                        //else
                        //{
                        //    drpcor.Enabled = false;
                        //    drpfacility.Enabled = false;
                        //    ddlCorporate.Enabled = false;
                        //    ddlFacility.Enabled = false;
                        //    List<GetServiceRequestMaster> lstMPRMaster = lclsservice.GetServiceRequestMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                        //    grdSRMaster.DataSource = lstMPRMaster;
                        //    grdSRMaster.DataBind();

                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Facility details from Facility marter table to dropdown control 
        /// </summary>
        #region Bind Facility Values
        private void BindFacility(int Search, string Mode)
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
                        foreach (ListItem lst in drpfacility.Items)
                        {
                            lst.Attributes.Add("class", "selected");
                            lst.Selected = true;
                        }
                    }
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Facility--";
                    // Insert Drop Down
                    if (Mode == "Add")
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

                    else if (Mode == "Edit")
                    {
                        ddlFacility.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(ddlCorporate.SelectedValue)).ToList();
                        ddlFacility.DataTextField = "FacilityDescription";
                        ddlFacility.DataValueField = "FacilityID";
                        ddlFacility.DataBind();
                        ddlFacility.Items.Insert(0, lst);
                        ddlFacility.SelectedIndex = 0;
                    }

                }
               
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Vendor details from vendor master table to dropdown control 
        /// </summary>
        #region Bind Vendor Values
        public void BindFooterVendor()
        {
            List<GetFacilityVendorAccount> lstvendordetails = null;
            try
            {
                DropDownList drpFooterVendor = gvSearchSRDetails.FooterRow.FindControl("ddlFootVendor") as DropDownList;

                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Vendor--";
                lstvendordetails = new List<GetFacilityVendorAccount>();


                // Insert Drop Down                
                lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.ServiceOrder == true).ToList();
                drpFooterVendor.DataSource = lstvendordetails;
                drpFooterVendor.DataTextField = "VendorDescription";
                drpFooterVendor.DataValueField = "VendorID";
                drpFooterVendor.DataBind();
                drpFooterVendor.Items.Insert(0, lst);
                drpFooterVendor.SelectedIndex = 0;

            }
            catch (Exception ex)
            {

                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);

            }
            // return lstvendordetails;

        }
        #endregion


        /// <summary>
        /// Bind the Service Category details to dropdown control 
        /// </summary>
        #region Bind Service Category
        public void BindServiceCategory(Int64 CorporateID, string Mode)
        {
            try
            {
                List<GetServiceCategory> lstServiceCategory = new List<GetServiceCategory>();
                lstServiceCategory = lclsservice.GetServiceCategory(CorporateID, Mode).ToList();

                // Search Drop Down

                ddlServiceCategory.DataSource = lstServiceCategory;
                ddlServiceCategory.DataTextField = "ServiceCatDescription";
                ddlServiceCategory.DataValueField = "ServiceCategoryID";
                ddlServiceCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Service Category--";
                ddlServiceCategory.Items.Insert(0, lst);
                ddlServiceCategory.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Status LookUp details from Status master table to dropdown control 
        /// </summary>
        #region Bind Status LookUp Values
        public void BindLookUp(string Mode)
        {
            try
            {
                // Search Status drop down
                List<GetList> lstLookUp = new List<GetList>();
                List<GetList> lstLookUpship = new List<GetList>();
                string SearchText = string.Empty;
                lstLookUp = lclsservice.GetList("ServiceRequest", "Status", Mode).ToList();

                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";


                // Search Status Drop Down
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                //drpStatus.Items.Insert(0, lst);
                drpStatus.Items.FindByText(PendingApproval).Selected = true;

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
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
                    if (drpcor.SelectedValue == "All")
                    {
                        llstServiceRequest.CorporateName = "ALL";
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

                        llstServiceRequest.CorporateName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpfacility.SelectedValue == "All")
                    {
                        llstServiceRequest.FacilityName = "ALL";
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
                        llstServiceRequest.FacilityName = FinalString;

                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpStatus.SelectedValue == "All")
                    {
                        llstServiceRequest.Status = "ALL";
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
                        llstServiceRequest.Status = FinalString;
                    }
                    SB.Clear();


                    //llstServiceRequest.VendorID = Convert.ToInt64(drpvendor.SelectedValue);
                    //if (txtDateFrom.Text != "") llstServiceRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    //if (txtDateTo.Text != "") llstServiceRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    if (txtDateFrom.Text == "")
                    {
                        txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstServiceRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    }
                    if (txtDateTo.Text == "")
                    {
                        txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstServiceRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    }

                    llstServiceRequest.LoggedinBy = defaultPage.UserId;
                    List<SearchServiceRequestMaster> lstMPRMaster = lclsservice.SearchServiceRequestMaster(llstServiceRequest).ToList();
                    GvTempEdit.DataSource = lstMPRMaster;
                    GvTempEdit.DataBind();

                }
                else
                {
                    SearchGrid();
                    //grdSRMaster.DataSource = lstMPRMaster;
                    //grdSRMaster.DataBind();
                }

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


        protected void grdSRMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string b ="<img src= \"Images/Readmore.png\" />";
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Label lblCreatedBy = (Label)e.Row.FindControl("lblCreatedBy");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                ImageButton imgbtnEdit = (e.Row.FindControl("imgbtnEdit") as ImageButton);

                string Status = e.Row.Cells[6].Text;
                //if (lblRemarks.Text.Length > 100)
                //{
                //    lblRemarks.Text = lblRemarks.Text.Substring(0, 100) + "....";
                //    imgreadmore.Visible = true;
                //}
                //else
                //{
                //    imgreadmore.Visible = false;
                //}
                //if (lblCreatedBy.Text.Length > 100)
                //{
                //    lblCreatedBy.Text = lblCreatedBy.Text.Substring(0, 100) + "....";
                //    imgreadmore1.Visible = true;
                //}
                //else
                //{
                //    imgreadmore1.Visible = false;
                //}

                if (Status == "Ordered and Pending Receive")
                {
                    imgbtnEdit.Visible = false;
                }
                else
                {
                    imgbtnEdit.Visible = true;
                }

            }

        }





        /// <summary>
        /// Bind the Service Category details to dropdown control 
        /// </summary>
        #region Bind Service List
        public void BindServiceList(string Mode)
        {
            try
            {
                List<GetServiceList> lstServiceList = new List<GetServiceList>();
                lstServiceList = lclsservice.GetServiceList(Convert.ToInt64(ddlServiceCategory.SelectedValue), Mode).ToList();

                // Search Drop Down

                ddlServiceList.DataSource = lstServiceList;
                ddlServiceList.DataTextField = "ServiceListDescription";
                ddlServiceList.DataValueField = "ServiceListID";
                ddlServiceList.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Service List--";
                ddlServiceList.Items.Insert(0, lst);
                ddlServiceList.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


        protected void ddlServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindServiceList("Add");
        }

        public void BindEquipcategory(Int64 CorporateID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipmentCategory> lstequipcat = lclsservice.GetEquipmentCategory(CorporateID, Mode).ToList();
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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        protected void ddlEquipmentCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipementSubCategory("Add");
        }
        public void GetEquipementList(string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementList> lstequiplist = lclsservice.GetEquipementList(Convert.ToInt64(ddlEquipmentSubCat.SelectedValue), Mode).ToList();
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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        public void GetEquipementSubCategory(string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementSubCategory> lstequipSubCat = lclsservice.GetEquipementSubCategory(Convert.ToInt64(ddlEquipmentCategory.SelectedValue), Mode).ToList();
                ddlEquipmentSubCat.DataSource = lstequipSubCat;
                ddlEquipmentSubCat.DataValueField = "EquipementSubCategoryID";
                ddlEquipmentSubCat.DataTextField = "EquipmentSubCategoryDescription";
                ddlEquipmentSubCat.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "-- Select Equipment Sub Category --";
                ddlEquipmentSubCat.Items.Insert(0, lst);
                ddlEquipmentSubCat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        private void SetAddScreen(string Add, int i)
        {
            try
            {
                if (i == 1)
                {

                    if (Add == "Add")
                    {
                        btnSave.Enabled = true;
                    }

                    if (defaultPage.RoleID == 1)
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        //ddlCorporate.Enabled = false;
                        //ddlFacility.Enabled = false;
                        if (defaultPage.Req_WorkOrServicePage_Edit == true && defaultPage.Req_WorkOrServicePage_View == true)
                        {
                            if (HddMasterID.Value != "" && HddFacilityID.Value != "")
                            {
                                List<GetServiceRequestMaster> lstMPRMaster = lclsservice.GetServiceRequestMaster().Where(a => a.FacilityID == Convert.ToInt64(HddFacilityID.Value) && a.ServiceRequestMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                                if (lstMPRMaster[0].Status == "Pending Order" || lstMPRMaster[0].Status == "Pending Approval" || lstMPRMaster[0].Status == "Hold")
                                {
                                    btnSave.Visible = true;
                                    btnAdd.Visible = false;
                                }
                                else
                                {
                                    gvSearchSRDetails.Enabled = false;
                                }
                            }
                            else
                            {
                                btnSave.Visible = true;
                                btnAdd.Visible = false;
                            }

                        }
                        else
                        {
                            gvSearchSRDetails.Enabled = false;
                        }
                    }


                    //btnPrint.Visible = false;
                    btnClose.Visible = true;
                    btnSearch.Visible = false;
                    btnSave.Text = "Review";
                    //btnSave.Style.Add("display", "block");
                    //btnPrint.Style.Add("display", "none");
                    //btnSearch.Style.Add("display", "none");
                    //btnAdd.Style.Add("display", "none");
                    //btnClose.Style.Add("display", "block");                    
                    divAddMachine.Style.Add("display", "block");
                    lblUpdateHeader.Visible = true;
                    lblMasterHeader.Visible = false;
                    divSRDetails.Style.Add("display", "block");
                    divSearchMachine.Style.Add("display", "none");
                    lblseroutHeader.Visible = false;
                    divSRMaster.Style.Add("display", "none");
                    divEdit.Style.Add("display", "none");
                    rdbServiceType.ClearSelection();
                    DivServiceCategory.Style.Add("display", "none");
                    DivServiceList.Style.Add("display", "none");
                    DivEquipmentCategory.Style.Add("display", "none");
                    DivEquipmentSubCat.Style.Add("display", "none");
                    DivEquipmentList.Style.Add("display", "none");
                    ReqdrdddlServiceCategory.Visible = false;
                    ReqdrdddlServiceList.Visible = false;
                    ReqdrdddlEquipmentCategory.Visible = false;
                    ReqdrdddlEquipmentList.Visible = false;
                    rdbServiceType.Enabled = true;
                    ddlServiceCategory.Enabled = true;
                    ddlEquipmentCategory.Enabled = true;
                    ddlEquipmentSubCat.Enabled = true;
                    ddlServiceList.Enabled = true;
                    ddlEquipmentList.Enabled = true;
                    imgeservicecatadd.Enabled = true;
                    imgeservicecatedit.Enabled = true;
                    imgeservicecatdelete.Enabled = true;
                    imgeservicelistadd.Enabled = true;
                    imgeservicelistedit.Enabled = true;
                    imgeservicelistdelete.Enabled = true;
                }
                else
                {
                    if (defaultPage.RoleID == 1)
                    {
                        ddlCorporate.Enabled = true;
                        ddlFacility.Enabled = true;
                        btnAdd.Visible = true;
                        btnSave.Visible = false;
                        BindGrid(0);
                    }
                    else
                    {
                        BindGrid(1);
                        //ddlCorporate.Enabled = false;
                        //ddlFacility.Enabled = false;
                        if (defaultPage.Req_WorkOrServicePage_Edit == true && defaultPage.Req_WorkOrServicePage_View == true)
                        {
                            btnSave.Visible = false;
                            btnAdd.Visible = true;
                        }
                        if (defaultPage.RoleID == 1)
                        {
                            BindGrid(0);
                        }
                        else
                        {
                            BindGrid(1);
                            //List<GetServiceRequestMaster> lstMPRMaster = lclsservice.GetServiceRequestMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                            //grdSRMaster.DataSource = lstMPRMaster;
                            //grdSRMaster.DataBind();
                        }
                    }

                    //btnPrint.Visible = true;
                    btnClose.Visible = true;
                    btnSearch.Visible = true;

                    BindCorporate(1, "Add");
                    drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                    BindFacility(1, "Add");
                    drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                    //BindVendor(1);
                    //btnAdd.Style.Add("display", "block");
                    //btnSave.Style.Add("display", "none");
                    //btnPrint.Style.Add("display", "block");
                    //btnSearch.Style.Add("display", "block");                
                    //btnClose.Style.Add("display", "block");
                    lblUpdateHeader.Visible = false;
                    lblMasterHeader.Visible = true;
                    lblEditHeader.Visible = false;
                    lblServiceMasterNo.Visible = false;
                    DivServiceMasterNo.Style.Add("display", "none");
                    divSRDetails.Style.Add("display", "none");
                    divUploadFile.Style.Add("display", "none");
                    lblseroutHeader.Visible = true;
                    divSRMaster.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        private void ClearMaster()
        {
            //drpcor.ClearSelection();
            //drpfacility.ClearSelection();
            //drpvendor.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatus.ClearSelection();
            BindCorporate(1, "Add");
            BindFacility(1, "Add");
            BindLookUp("Add");
            HddListCorpID.Value = "";
        }

        private void ClearDetails()
        {
            //ddlCorporate.ClearSelection();
            //ddlFacility.ClearSelection();
            //ddlVendor.ClearSelection();
            ddlServiceCategory.ClearSelection();
            ddlServiceList.ClearSelection();
            ddlEquipmentCategory.ClearSelection();
            ddlEquipmentSubCat.ClearSelection();
            ddlEquipmentList.ClearSelection();
            //ddlstatus.ClearSelection();
            lblServiceCategory.Text = "";
            lblServiceList.Text = "";
            lblServiceType.Text = "";
            lblEquipmentCategory.Text = "";
            lblEquipementEquipSubCat.Text = "";
            lblEquipementList.Text = "";
            gvAddSRDetails.DataSource = null;
            gvAddSRDetails.DataBind();
            gvSearchSRDetails.DataSource = null;
            gvSearchSRDetails.DataBind();
            GvTempEdit.DataSource = null;
            GvTempEdit.DataBind();
            btnPrint.Visible = false;
            lbpopprint.Visible = false;
            ViewState["ServiceRequestMasterID"] = "";
            HddListCorpID.Value = "";
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

                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dt.Rows.Add(dr);

                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;

                //Bind the DataTable to the Grid
                gvAddSRDetails.DataSource = dt;
                gvAddSRDetails.DataBind();

                DropDownList ddl1 = (DropDownList)gvAddSRDetails.Rows[0].Cells[1].FindControl("ddlVendor");
                TextBox box1 = (TextBox)gvAddSRDetails.Rows[0].Cells[2].FindControl("Service");
                TextBox box2 = (TextBox)gvAddSRDetails.Rows[0].Cells[3].FindControl("Unit");
                TextBox box3 = (TextBox)gvAddSRDetails.Rows[0].Cells[4].FindControl("Price");
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Vendor--";

                List<GetFacilityVendorAccount> lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.ServiceOrder == true).ToList();
                ddl1.DataSource = lstvendordetails;
                ddl1.DataTextField = "VendorDescription";
                ddl1.DataValueField = "VendorID";
                ddl1.DataBind();
                ddl1.Items.Insert(0, lst);
                ddl1.SelectedIndex = 0;
                dt.Rows[0]["Column1"] = ddl1.SelectedItem.Text;
                dt.Rows[0]["Column2"] = box1.Text;
                dt.Rows[0]["Column3"] = box2.Text;
                dt.Rows[0]["Column4"] = box3.Text;
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }


        protected void btn_New_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lclsservice.SyncServiceReceivingorder();
                SetAddScreen("Add", 1);
                ClearDetails();
                ddlCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                BindFacility(0, "Add");
                ddlFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);

                SetInitialRow();
                lclsservice.TruncateSRTempAttch();
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }

        }

        private void AddNewRowToGrid()
        {
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    //Set Previous Data on Postbacks   
                    SetPreviousData();
                    //SetInitialRow();
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

                        gvAddSRDetails.DataSource = dtCurrentTable;
                        gvAddSRDetails.DataBind();

                    }
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        //extract the TextBox values   

                        DropDownList ddl1 = (DropDownList)gvAddSRDetails.Rows[i].Cells[1].FindControl("ddlVendor");
                        TextBox box1 = (TextBox)gvAddSRDetails.Rows[i].Cells[2].FindControl("Service");
                        TextBox box2 = (TextBox)gvAddSRDetails.Rows[i].Cells[3].FindControl("Unit");
                        TextBox box3 = (TextBox)gvAddSRDetails.Rows[i].Cells[4].FindControl("Price");

                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select Vendor--";

                        List<GetFacilityVendorAccount> lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.ServiceOrder == true).ToList();
                        ddl1.DataSource = lstvendordetails;
                        ddl1.DataTextField = "VendorDescription";
                        ddl1.DataValueField = "VendorID";
                        ddl1.DataBind();
                        ddl1.Items.Insert(0, lst);
                        ddl1.SelectedIndex = 0;
                        if (i == dtCurrentTable.Rows.Count - 1)
                        {
                            dtCurrentTable.Rows[i]["Column1"] = ddl1.SelectedItem.Text;
                            dtCurrentTable.Rows[i]["Column2"] = box1.Text;
                            dtCurrentTable.Rows[i]["Column3"] = box2.Text;
                            dtCurrentTable.Rows[i]["Column4"] = box3.Text;
                        }
                        ddl1.ClearSelection();
                        ddl1.Items.FindByText(dtCurrentTable.Rows[i]["Column1"].ToString()).Selected = true;
                        //ddl1.SelectedItem.Text = dtCurrentTable.Rows[i]["Column1"].ToString();
                        box1.Text = dtCurrentTable.Rows[i]["Column2"].ToString();
                        box2.Text = dtCurrentTable.Rows[i]["Column3"].ToString();
                        box3.Text = dtCurrentTable.Rows[i]["Column4"].ToString();

                        dtCurrentTable.Rows[i]["Column1"] = ddl1.SelectedItem.Text;
                        dtCurrentTable.Rows[i]["Column2"] = box1.Text;
                        dtCurrentTable.Rows[i]["Column3"] = box2.Text;
                        dtCurrentTable.Rows[i]["Column4"] = box3.Text;

                    }

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
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
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
                            //Label box1 = (Label)gvAddSRDetails.Rows[i].Cells[1].FindControl("SINo");
                            DropDownList ddl1 = (DropDownList)gvAddSRDetails.Rows[i].Cells[1].FindControl("ddlVendor");
                            TextBox box1 = (TextBox)gvAddSRDetails.Rows[i].Cells[1].FindControl("Service");
                            TextBox box2 = (TextBox)gvAddSRDetails.Rows[i].Cells[2].FindControl("Unit");
                            TextBox box3 = (TextBox)gvAddSRDetails.Rows[i].Cells[3].FindControl("Price");


                            dt.Rows[i]["Column1"] = ddl1.SelectedItem.Text;
                            dt.Rows[i]["Column2"] = box1.Text;
                            dt.Rows[i]["Column3"] = box2.Text;
                            dt.Rows[i]["Column4"] = box3.Text;

                            ddl1.ClearSelection();
                            ddl1.Items.FindByText(dt.Rows[i]["Column1"].ToString()).Selected = true;
                            //ddl1.SelectedItem.Text = dt.Rows[i]["Column1"].ToString();
                            box1.Text = dt.Rows[i]["Column2"].ToString();
                            box2.Text = dt.Rows[i]["Column3"].ToString();
                            box3.Text = dt.Rows[i]["Column4"].ToString();

                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        public void SetInitialAttachment()
        {
            try
            {
                DataRow dr = null;


                Attachment.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                Attachment.Columns.Add(new DataColumn("LocationOfTheFile", typeof(string)));
                Attachment.Columns.Add(new DataColumn("UploadedBy", typeof(string)));
                Attachment.Columns.Add(new DataColumn("UploadedDateTime", typeof(string)));
                Attachment.Columns.Add(new DataColumn("Description", typeof(string)));
                Attachment.Columns.Add(new DataColumn("FileName", typeof(string)));

                dr = Attachment.NewRow();
                Attachment.Rows.Add(dr);

                //Store the DataTable in ViewState
                ViewState["CurrentAttachmentTable"] = Attachment;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        public void SetAttachmentData(int rowindex)
        {
            try
            {
                try
                {

                    DataRow dr = null;
                    if (ViewState["CurrentAttachmentTable"] != null)
                    {
                        Attachment = (DataTable)ViewState["CurrentAttachmentTable"];
                        Guid id = Guid.NewGuid();

                    }
                    else
                    {
                        SetInitialAttachment();
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void SetPreviousAttachmentData()
        {
            try
            {

                DataRow dr = null;
                if (ViewState["CurrentTable"] != null)
                {
                    Attachment = (DataTable)ViewState["CurrentAttachmentTable"];
                    foreach (GridViewRow row in gvAddSRDetails.Rows)
                    {
                        Label lblrowindex = (Label)row.FindControl("lblSINo");
                        dr["RowNumber"] = lblrowindex.Text;
                    }
                }
                else
                {
                    SetInitialAttachment();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        public void DeleteAttachment(int rowIndex)
        {
            try
            {
                Attachment = (DataTable)ViewState["CurrentAttachmentTable"];
                if (Attachment.Rows.Count > 1)
                {
                    Attachment.Rows.Remove(Attachment.Rows[rowIndex]);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }



        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPreviousData();
        }

        protected void Service_TextChanged(object sender, EventArgs e)
        {
            SetPreviousData();
        }

        protected void btnAddDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton DelRow = (ImageButton)sender;
                GridViewRow gvRow = (GridViewRow)DelRow.NamingContainer;
                int rowID = gvRow.RowIndex;

                SetPreviousData();
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
                        gvAddSRDetails.DataSource = dt;
                        gvAddSRDetails.DataBind();

                        //DeleteAttachment(rowIndex);

                        for (int i = 0; i < gvAddSRDetails.Rows.Count; i++)
                        {
                            DropDownList ddl1 = (DropDownList)gvAddSRDetails.Rows[i].Cells[1].FindControl("ddlVendor");
                            TextBox box1 = (TextBox)gvAddSRDetails.Rows[i].Cells[2].FindControl("Service");
                            TextBox box2 = (TextBox)gvAddSRDetails.Rows[i].Cells[3].FindControl("Unit");
                            TextBox box3 = (TextBox)gvAddSRDetails.Rows[i].Cells[4].FindControl("Price");
                            //TextBox box5 = (TextBox)gvAddSRDetails.Rows[i].Cells[5].FindControl("Quote");
                            //TextBox box6 = (TextBox)gvAddSRDetails.Rows[i].Cells[6].FindControl("TotalPrice");
                            ListItem lst = new ListItem();
                            lst.Value = "0";
                            lst.Text = "--Select Vendor--";
                            //box1.Text = (dtCurrentTable.Rows.Count + 1).ToString();
                            List<GetFacilityVendorAccount> lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.ServiceOrder == true).ToList();
                            ddl1.DataSource = lstvendordetails;
                            ddl1.DataTextField = "VendorDescription";
                            ddl1.DataValueField = "VendorID";
                            ddl1.DataBind();
                            ddl1.Items.Insert(0, lst);
                            ddl1.SelectedIndex = 0;

                            ddl1.ClearSelection();
                            ddl1.Items.FindByText(dt.Rows[i]["Column1"].ToString()).Selected = true;
                            box1.Text = dt.Rows[i]["Column2"].ToString();
                            box2.Text = dt.Rows[i]["Column3"].ToString();
                            box3.Text = dt.Rows[i]["Column4"].ToString();

                            dt.Rows[i]["Column1"] = ddl1.SelectedItem.Text;
                            dt.Rows[i]["Column2"] = box1.Text;
                            dt.Rows[i]["Column3"] = box2.Text;
                            dt.Rows[i]["Column4"] = box3.Text;
                        }
                        SetPreviousData();
                        Label lblSINo = (Label)gvRow.FindControl("lblSINo");
                        string lstmessage = lclsservice.DeleteSRTempAttch(Convert.ToInt32(lblSINo.Text), "Row");
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (HddUpdateLockinEdit.Value == "Edit")
            {
                a = lclsservice.AutoUpdateLockedOut(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId), "Service");
                HddUpdateLockinEdit.Value = "";
            }
            if (Request.QueryString["SRPOID"] != null)
            {
                Response.Redirect("ServiceRequestPO.aspx");
            }
            else
            {
                SetAddScreen("", 0);
                ClearMaster();
                ClearDetails();
                HddMasterID.Value = "";
                btnPrint.Visible = true;
                //btnPrint.Visible = false;
                lbpopprint.Visible = false;
                //SearchGrid();
            }
            SearchGrid();
        }

        public void InsertServiceRequest()
        {
            List<object> lstIDwithmessage = new List<object>();

            // Insert Machine Parts Request Master 
            lstIDwithmessage = lclsservice.InsertServiceRequestMaster(llstServiceRequest).ToList();
            a = lstIDwithmessage[0].ToString();
            llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(lstIDwithmessage[1]);


            //Get Grid Details Value
            foreach (GridViewRow grdfs in gvAddSRDetails.Rows)
            {
                //Label SINo = (Label)grdfs.FindControl("SINo");
                DropDownList DrpVendor = (DropDownList)grdfs.FindControl("ddlVendor");
                TextBox Service = (TextBox)grdfs.FindControl("Service");
                TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                TextBox Price = (TextBox)grdfs.FindControl("Price");
                Label lblSINo = (Label)grdfs.FindControl("lblSINo");

                //llstServiceRequest.SNo = Convert.ToInt32(SINo.Text);
                llstServiceRequest.VendorID = Convert.ToInt64(DrpVendor.SelectedValue);
                llstServiceRequest.Service = Convert.ToString(Service.Text);
                llstServiceRequest.Unit = Convert.ToString(Unit.Text);
                llstServiceRequest.Price = Convert.ToDecimal(Price.Text);
                llstServiceRequest.SNo = Convert.ToInt32(lblSINo.Text);

                // Insert Machine Parts Request Details
                b = lclsservice.InsertServcieRequestDetails(llstServiceRequest);
            }
            if (a == b && b == "Saved Successfully")
            {
                //Functions objfun = new Functions();
                //objfun.MessageDialog(this, "Saved Successfully");

                ClearDetails();
                btnPrint.Visible = true;
                SetAddScreen("", 0);
                List<GetServiceRequestMaster> lstSRMaster = lclsservice.GetServiceRequestMaster().ToList();
                lbpopprint.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('" + lstSRMaster[0].SRNo + " is Successfully Created,Click here to Print');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSaveMessage.Replace("<<ServiceRequestDescription>>", lstSRMaster[0].SRNo.ToString()), true);
                ViewState["ServiceRequestMasterID"] = llstServiceRequest.ServiceRequestMasterID;
                SearchGrid();
                //btnPrint.Visible = true;
            }
        }

        public void UpdateServiceRequest()
        {

            if (gvSearchSRDetails.PageCount == 0)
            {
                //Get Grid Details Value
                foreach (GridViewRow grdfs in gvAddSRDetails.Rows)
                {
                    //Label SINo = (Label)grdfs.FindControl("SINo");
                    DropDownList DrpVendor = (DropDownList)grdfs.FindControl("ddlVendor");
                    TextBox Service = (TextBox)grdfs.FindControl("Service");
                    TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                    TextBox Price = (TextBox)grdfs.FindControl("Price");

                    //llstServiceRequest.SNo = Convert.ToInt32(SINo.Text);
                    llstServiceRequest.VendorID = Convert.ToInt64(DrpVendor.SelectedValue);
                    llstServiceRequest.Service = Convert.ToString(Service.Text);
                    llstServiceRequest.Unit = Convert.ToString(Unit.Text);
                    llstServiceRequest.Price = Convert.ToDecimal(Price.Text);
                    llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(HddMasterID.Value);


                    // Insert Machine Parts Request Details
                    a = lclsservice.InsertServcieRequestDetails(llstServiceRequest);
                }
            }
            else
            {
                //Get Grid Details Value
                foreach (GridViewRow grdfs in gvSearchSRDetails.Rows)
                {
                    //Label SINo = (Label)grdfs.FindControl("SINo");

                    TextBox Service = (TextBox)grdfs.FindControl("Service");
                    TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                    TextBox Price = (TextBox)grdfs.FindControl("Price");

                    Label ServiceRequestMasterID = (Label)grdfs.FindControl("lbServiceRequestMasterID");
                    Label ServiceRequestDetailsID = (Label)grdfs.FindControl("lbServiceRequestDetailsID");


                    //llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(grdfs.Cells[5].Text);
                    //llstServiceRequest.ServiceRequestDetailID = Convert.ToInt64(grdfs.Cells[6].Text);
                    //llstServiceRequest.SNo = Convert.ToInt32(SINo.Text);

                    llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(ServiceRequestMasterID.Text);
                    llstServiceRequest.ServiceRequestDetailID = Convert.ToInt64(ServiceRequestDetailsID.Text);
                    llstServiceRequest.LastModifiedBy = Convert.ToInt64(defaultPage.UserId);

                    llstServiceRequest.Service = Convert.ToString(Service.Text);
                    llstServiceRequest.Unit = Convert.ToString(Unit.Text);
                    llstServiceRequest.Price = Convert.ToDecimal(Price.Text);
                    //ViewState["ServiceRequestMasterID"] = llstServiceRequest.ServiceRequestMasterID;
                    // Update Machine Parts Request Details
                    b = lclsservice.UpdateServiceRequestDetails(llstServiceRequest);
                }
                // Update Machine Parts Request Master
                //a = lclsservice.UpdateServiceRequestMaster(llstServiceRequest);

                if (gvSearchSRDetails.FooterRow.Visible == true)
                {
                    FooterRowSave();
                }
            }

            if (a == "Saved Successfully")
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);
                lbpopprint.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSaveMessage.Replace("<<ServiceRequestDescription>>", HddMasterID.Value), true);
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));

            }
            else if (b == "Updated Successfully")
            {
                //Functions objfun = new Functions();
                //objfun.MessageDialog(this, "Update Successfully");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Update Successfully');", true);
                //ViewState["ServiceRequestMasterID"] = llstServiceRequest.ServiceRequestMasterID + ",";
                // ViewState["SerachFilters"] = llstServiceRequest.CorporateID + "," + llstServiceRequest.FacilityID + "," + llstServiceRequest.DateFrom + "," + llstServiceRequest.DateTo + "," + llstServiceRequest.Status;
                lbpopprint.Visible = true;
                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestUpdateMessage.Replace("<<ServiceRequestDescription>>", HddAutoServiceMasterNo.Value ), true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestUpdateMessage.Replace("<<ServiceRequestDescription>>", lblhead.Text), true);
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                //btnPrint.Visible = true;
            }
            btnPrint.Visible = true;
        }

        protected void btnSearchNewRow_Click(object sender, EventArgs e)
        {
            if (gvSearchSRDetails.PageCount != 0)
            {
                gvSearchSRDetails.FooterRow.Visible = true;
                BindFooterVendor();
                lclsservice.TruncateSRTempAttch();
            }
            else
            {
                divAddMachine.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "none");
                SetInitialRow();
            }


        }

        protected void btnSaveRow_Click(object sender, ImageClickEventArgs e)
        {
            string CheckQuotes = string.Empty;
            if (gvSearchSRDetails.FooterRow.Visible == true)
            {
                DropDownList drpFooterVendor = gvSearchSRDetails.FooterRow.FindControl("ddlFootVendor") as DropDownList;
                TextBox txtFootService = gvSearchSRDetails.FooterRow.FindControl("FootService") as TextBox;
                TextBox txtFootUnit = gvSearchSRDetails.FooterRow.FindControl("FootUnit") as TextBox;
                TextBox txtFootPrice = gvSearchSRDetails.FooterRow.FindControl("FootPrice") as TextBox;
                if (drpFooterVendor.SelectedValue == "0" || txtFootService.Text == "" || txtFootUnit.Text == "" || txtFootPrice.Text == "")
                {
                    ErrorList = "The Grid feilds should not be Empty";
                }
                Int32 FootSINo = 0;
                List<GetServiceTempAttachment> llstCheckQuotes = lclsservice.GetServiceTempAttachment(FootSINo).ToList();
                if (llstCheckQuotes.Count == 0)
                {
                    CheckQuotes += txtFootService.Text + ",";
                }
            }
            if (ErrorList == "")
            {
                if (CheckQuotes == "")
                {
                    FooterRowSave();
                }
                else
                {
                    CheckQuotes = CheckQuotes.Substring(0, CheckQuotes.Length - 1);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", " Quotes not attched to (" + CheckQuotes + ") respected services"), true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ErrorList + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ErrorList), true);
            }
        }

        /// <summary>
        /// Bind the Machine Parts Request details from MPRDetails table to Grid control 
        /// </summary>
        #region Bind Machine Parts Request details Values
        public void BindDetailGrid(Int64 ServiceMasterID, Int64 UserID)
        {
            try
            {
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetServiceRequestetailsbyServiceRequestMasterID> llstSRDetails = lclsservice.GetServiceRequestetailsbyServiceRequestMasterID(ServiceMasterID, UserID, Convert.ToInt64(LockTimeOut)).ToList();

                if (llstSRDetails[0].IsReadOnly == 0)
                {
                    gvSearchSRDetails.Enabled = true;
                    btnSave.Enabled = true;
                }
                else if (llstSRDetails[0].IsReadOnly == 1)
                {
                    gvSearchSRDetails.Enabled = false;
                    btnSave.Enabled = false;
                    List<GetUserDetails> llstuserdetails = lclsservice.GetUserDetails(Convert.ToInt64(llstSRDetails[0].Lockedby)).ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestLockINMessage.Replace("<<ServiceRequestDescription>>", llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName), true);
                }
                gvSearchSRDetails.DataSource = llstSRDetails;
                gvSearchSRDetails.DataBind(); 
                

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }
        #endregion

        public void FooterRowSave()
        {
            try
            {
                Control control = null;
                if (gvSearchSRDetails.FooterRow != null)
                {
                    control = gvSearchSRDetails.FooterRow;
                }
                else
                {
                    control = gvSearchSRDetails.Controls[0].Controls[0];
                }


                llstServiceRequest.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                //llstServiceRequest.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                //llstServiceRequest.ServiceCategoryID = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                //llstServiceRequest.ServiceListID = Convert.ToInt64(ddlServiceList.SelectedValue);
                llstServiceRequest.CreatedBy = defaultPage.UserId;

                Int64 Vendor = Convert.ToInt64((control.FindControl("ddlFootVendor") as DropDownList).SelectedValue);
                string Service = (control.FindControl("FootService") as TextBox).Text;
                string Unit = (control.FindControl("FootUnit") as TextBox).Text;
                string Price = (control.FindControl("FootPrice") as TextBox).Text;

                llstServiceRequest.VendorID = Vendor;
                llstServiceRequest.Service = Service.ToString();
                llstServiceRequest.Unit = Unit.ToString();
                llstServiceRequest.Price = Convert.ToDecimal(Price);
                llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(HddMasterID.Value);

                // Insert Machine Parts Request Details

                b = lclsservice.InsertServcieRequestDetails(llstServiceRequest);


                if (b == "Saved Successfully")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    lclsservice.TruncateSRTempAttch();
                }
            }
            catch (Exception ex)
            {

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //set Previous Data in the Grid View 
                SetPreviousData();
                DataTable ReviewPopupData = new DataTable();
                DataRow dr = null;
                int j = 0;
                string CheckQuotes = string.Empty;


                // Validate the Mandatory fields in Grid            
                foreach (GridViewRow grdfs in gvAddSRDetails.Rows)
                {
                    //Label SINo = (Label)grdfs.FindControl("SINo");
                    DropDownList DrpVendor = (DropDownList)grdfs.Cells[1].FindControl("ddlVendor");
                    TextBox Service = (TextBox)grdfs.FindControl("Service");
                    TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                    TextBox Price = (TextBox)grdfs.FindControl("Price");
                    Label lblSINo = (Label)grdfs.FindControl("lblSINo");
                    if (DrpVendor.SelectedValue == "0" || Service.Text == "" || Unit.Text == "" || Price.Text == "")
                    {
                        ErrorList = "The Grid feilds should not be Empty";
                    }
                    List<GetServiceTempAttachment> llstCheckQuotes = lclsservice.GetServiceTempAttachment(Convert.ToInt32(lblSINo.Text)).ToList();
                    if (llstCheckQuotes.Count == 0)
                    {
                        CheckQuotes += Service.Text + ",";
                    }

                }
                // Validate the Mandatory fields in Grid
                foreach (GridViewRow grdfs in gvSearchSRDetails.Rows)
                {
                    //Label SINo = (Label)grdfs.FindControl("SINo");

                    TextBox Service = (TextBox)grdfs.FindControl("Service");
                    TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                    TextBox Price = (TextBox)grdfs.FindControl("Price");
                    Label lbServiceRequestDetailsID = (Label)grdfs.FindControl("lbServiceRequestDetailsID");
                    if (Service.Text == "" || Unit.Text == "" || Price.Text == "")
                    {
                        ErrorList = "The Grid feilds should not be Empty";
                    }
                    List<GetServiceAttachment> llstServiceAttachment = lclsservice.GetServiceAttachment(Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    if (llstServiceAttachment.Count == 0)
                    {
                        CheckQuotes += Service.Text + ",";
                    }
                }


                lblreviewcorporate.Text = ddlCorporate.SelectedItem.Text;
                lblreviewfacility.Text = ddlFacility.SelectedItem.Text;
                if (rdbServiceType.SelectedValue == "1")
                {
                    lblServiceType.Text = "Building";
                    lblServiceCategory.Text = ddlServiceCategory.SelectedItem.Text;
                    lblServiceList.Text = ddlServiceList.SelectedItem.Text;
                    DivreviewEC.Style.Add("display", "none");
                    DivreviewESC.Style.Add("display", "none");
                    DivreviewEL.Style.Add("display", "none");
                    DivreviewLEC.Style.Add("display", "none");
                    DivreviewLESC.Style.Add("display", "none");
                    DivreviewLEL.Style.Add("display", "none");
                    DivreviewSC.Style.Add("display", "block");
                    DivreviewLSC.Style.Add("display", "block");
                    DivreviewSL.Style.Add("display", "block");
                    DivreviewLSL.Style.Add("display", "block");
                }
                else if (rdbServiceType.SelectedValue == "2")
                {
                    lblServiceType.Text = "Equipment";
                    lblEquipmentCategory.Text = ddlEquipmentCategory.SelectedItem.Text;
                    lblEquipementEquipSubCat.Text = ddlEquipmentSubCat.SelectedItem.Text;
                    lblEquipementList.Text = ddlEquipmentList.SelectedItem.Text;
                    DivreviewEC.Style.Add("display", "block");
                    DivreviewESC.Style.Add("display", "block");
                    DivreviewEL.Style.Add("display", "block");
                    DivreviewLEC.Style.Add("display", "block");
                    DivreviewLESC.Style.Add("display", "block");
                    DivreviewLEL.Style.Add("display", "block");
                    DivreviewSC.Style.Add("display", "none");
                    DivreviewLSC.Style.Add("display", "none");
                    DivreviewSL.Style.Add("display", "none");
                    DivreviewLSL.Style.Add("display", "none");
                }
                if (HddMasterID.Value == "")
                {
                    DivPopupSRNo.Style.Add("display", "none");
                    //lblpopupheader.Visible = false;
                    ReviewPopupData = (DataTable)ViewState["CurrentTable"];
                }
                else
                {
                    if (gvSearchSRDetails.PageCount == 0)
                    {
                        ReviewPopupData = (DataTable)ViewState["CurrentTable"];
                    }
                    else
                    {
                        DivPopupSRNo.Style.Add("display", "block");
                        //lblpopupheader.Visible = true;
                        ReviewPopupData.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                        ReviewPopupData.Columns.Add(new DataColumn("Column1", typeof(string)));
                        ReviewPopupData.Columns.Add(new DataColumn("Column2", typeof(string)));
                        ReviewPopupData.Columns.Add(new DataColumn("Column3", typeof(string)));
                        ReviewPopupData.Columns.Add(new DataColumn("Column4", typeof(string)));

                        foreach (GridViewRow grdfs in gvSearchSRDetails.Rows)
                        {
                            //Label SINo = (Label)grdfs.FindControl("SINo");
                            Label lblvendor = (Label)grdfs.FindControl("lblVendor");
                            TextBox Service = (TextBox)grdfs.FindControl("Service");
                            TextBox Unit = (TextBox)grdfs.FindControl("Unit");
                            TextBox Price = (TextBox)grdfs.FindControl("Price");
                            if (Service.Text == "" || Unit.Text == "" || Price.Text == "")
                            {
                                ErrorList = "The Grid feilds should not be Empty";
                            }

                            dr = ReviewPopupData.NewRow();
                            j = j + 1;
                            dr["RowNumber"] = j;
                            dr["Column1"] = lblvendor.Text;
                            dr["Column2"] = Service.Text;
                            dr["Column3"] = Unit.Text;
                            dr["Column4"] = Price.Text;
                            ReviewPopupData.Rows.Add(dr);
                        }
                        if (gvSearchSRDetails.FooterRow.Visible == true)
                        {
                            dr = ReviewPopupData.NewRow();
                            DropDownList drpFooterVendor = gvSearchSRDetails.FooterRow.FindControl("ddlFootVendor") as DropDownList;
                            TextBox txtFootService = gvSearchSRDetails.FooterRow.FindControl("FootService") as TextBox;
                            TextBox txtFootUnit = gvSearchSRDetails.FooterRow.FindControl("FootUnit") as TextBox;
                            TextBox txtFootPrice = gvSearchSRDetails.FooterRow.FindControl("FootPrice") as TextBox;
                            if (drpFooterVendor.SelectedValue == "0" || txtFootService.Text == "" || txtFootUnit.Text == "" || txtFootPrice.Text == "")
                            {
                                ErrorList = "The Grid feilds should not be Empty";
                            }
                            Int32 FootSINo = 0;
                            List<GetServiceTempAttachment> llstCheckQuotes = lclsservice.GetServiceTempAttachment(FootSINo).ToList();
                            if (llstCheckQuotes.Count == 0)
                            {
                                CheckQuotes += txtFootService.Text + ",";
                            }
                        }
                        if (gvSearchSRDetails.FooterRow.Visible == true)
                        {
                            dr = ReviewPopupData.NewRow();
                            DropDownList drpFooterVendor = gvSearchSRDetails.FooterRow.FindControl("ddlFootVendor") as DropDownList;
                            TextBox txtFootService = gvSearchSRDetails.FooterRow.FindControl("FootService") as TextBox;
                            TextBox txtFootUnit = gvSearchSRDetails.FooterRow.FindControl("FootUnit") as TextBox;
                            TextBox txtFootPrice = gvSearchSRDetails.FooterRow.FindControl("FootPrice") as TextBox;
                            j = j + 1;
                            dr["RowNumber"] = j;
                            dr["Column1"] = drpFooterVendor.SelectedItem.Text;
                            dr["Column2"] = txtFootService.Text;
                            dr["Column3"] = txtFootUnit.Text;
                            dr["Column4"] = txtFootPrice.Text;
                            ReviewPopupData.Rows.Add(dr);
                        }
                    }
                }
                if (ErrorList == "")
                {
                    if (CheckQuotes == "")
                    {
                        mpeSerRequReview.Show();
                        lblhead.Text = HddAutoServiceMasterNo.Value;
                        GvServiceRequestReview.DataSource = ReviewPopupData;
                        GvServiceRequestReview.DataBind();
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ErrorList + "');", true);
                        CheckQuotes = CheckQuotes.Substring(0, CheckQuotes.Length - 1);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", " Quotes not attched to (" + CheckQuotes + ") respected services"), true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ErrorList), true);
                }

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        private void EditDisplayControls()
        {
            try
            {
                SetAddScreen("", 1);
                BindGrid(1);
                //if (defaultPage.RoleID == 1)
                //{
                //    BindGrid(1);
                //}
                //else
                //{
                //    List<GetServiceRequestMaster> lstMPRMaster = lclsservice.GetServiceRequestMaster().Where(a => a.FacilityID == Convert.ToInt64(drpfacility.SelectedValue)).ToList();
                //    GvTempEdit.DataSource = lstMPRMaster;
                //    GvTempEdit.DataBind();
                //}

                DivServiceMasterNo.Style.Add("display", "block");
                if (Request.QueryString["SRPOID"] != null)
                {
                    divEdit.Style.Add("display", "none");
                }
                else
                {
                    divEdit.Style.Add("display", "block");
                }

                divSearchMachine.Style.Add("display", "block");
                divAddMachine.Style.Add("display", "none");
                lblServiceMasterNo.Visible = true;
                ddlCorporate.Enabled = false;
                ddlFacility.Enabled = false;
                rdbServiceType.Enabled = false;
                ddlServiceCategory.Enabled = false;
                ddlEquipmentCategory.Enabled = false;
                ddlEquipmentSubCat.Enabled = false;
                ddlServiceList.Enabled = false;
                ddlEquipmentList.Enabled = false;
                imgeservicecatadd.Enabled = false;
                imgeservicecatedit.Enabled = false;
                imgeservicecatdelete.Enabled = false;
                imgeservicelistadd.Enabled = false;
                imgeservicelistedit.Enabled = false;
                imgeservicelistdelete.Enabled = false;
                //  btnPrint.Visible = true;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        public void QueryStringPageLoad()
        {
            try
            {
                HddUpdateLockinEdit.Value = "Edit";
                HddMasterID.Value = Request.QueryString["SRPOID"];
                List<GetServiceRequestMaster> lstMPRMaster = lclsservice.GetServiceRequestMaster().Where(a => a.ServiceRequestMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                HddFacilityID.Value = lstMPRMaster[0].FacilityID.ToString();
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["ServiceRequestMasterID"] = HddMasterID.Value;

                EditDisplayControls();
                if (Request.QueryString["SRPOID"] != null)
                {
                    lblEditHeader.Visible = false;
                    lblUpdateHeader.Visible = true;
                    lblMasterHeader.Visible = false;
                }
                else
                {
                    lblEditHeader.Visible = true;
                    lblUpdateHeader.Visible = false;
                    lblMasterHeader.Visible = true;
                }
                lblseroutHeader.Visible = false;


                lblServiceMasterNo.Text = lstMPRMaster[0].SRNo;
                HddAutoServiceMasterNo.Value = lblServiceMasterNo.Text;
                BindCorporate(0, "Edit");
                ddlCorporate.ClearSelection();
                ddlCorporate.SelectedValue = lstMPRMaster[0].CorporateID.ToString();

                BindFacility(0, "Edit");
                ddlFacility.ClearSelection();
                ddlFacility.SelectedValue = lstMPRMaster[0].FacilityID.ToString();

                rdbServiceType.ClearSelection();
                if (lstMPRMaster[0].ServiceType.ToString() == "" && lstMPRMaster[0].ServiceType.ToString() == null)
                {
                    rdbServiceType.ClearSelection();
                }
                else
                {
                    if (lstMPRMaster[0].ServiceType == true)
                    {
                        rdbServiceType.SelectedValue = "1";
                        RadioCheckServiceType();
                        ddlServiceCategory.ClearSelection();
                        BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Edit");
                        ddlServiceCategory.SelectedValue = lstMPRMaster[0].ServiceCategoryID.ToString();
                        BindServiceList("Edit");
                        ddlServiceList.ClearSelection();
                        ddlServiceList.SelectedValue = lstMPRMaster[0].ServiceListID.ToString();
                    }
                    else
                    {
                        rdbServiceType.SelectedValue = "2";
                        RadioCheckServiceType();
                        ddlEquipmentCategory.ClearSelection();
                        BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Edit");
                        ddlEquipmentCategory.SelectedValue = lstMPRMaster[0].EquipmentCategoryID.ToString();

                        ddlEquipmentSubCat.ClearSelection();
                        GetEquipementSubCategory("Edit");
                        ddlEquipmentSubCat.SelectedValue = lstMPRMaster[0].EquipementSubCategoryID.ToString();

                        ddlEquipmentList.ClearSelection();
                        GetEquipementList("Edit");
                        ddlEquipmentList.SelectedValue = lstMPRMaster[0].EquipementListID.ToString();

                    }
                }
                SREditQuerygrid.Attributes["class"] = "SRAddgrid";
                btnAdd.Visible = false;
                if (defaultPage.Req_WorkOrServicePage_Edit == true && defaultPage.Req_WorkOrServicePage_View == true)
                {
                    btnSave.Visible = true;
                }
                if (defaultPage.Req_WorkOrServicePage_Edit == false && defaultPage.Req_WorkOrServicePage_View == true)
                {
                    btnSave.Visible = false;
                }
                if (defaultPage.Req_WorkOrServicePage_Edit == false && defaultPage.Req_WorkOrServicePage_View == false)
                {
                    updmain.Visible = false;
                    User_Permission_Message.Visible = true;
                }
                ddlCorporate.Enabled = false;
                ddlFacility.Enabled = false;
                btnPrint.Visible = false;
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }


        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                hdncheckfield.Value = "1";
                //  btnPrint.ValidationGroup = "";                
                HddUpdateLockinEdit.Value = "Edit";
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                HddMasterID.Value = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                HddFacilityID.Value = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["ServiceRequestMasterID"] = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");


                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                EditDisplayControls();
                lblEditHeader.Visible = true;
                lblseroutHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblMasterHeader.Visible = true;
                lblServiceMasterNo.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                HddAutoServiceMasterNo.Value = lblServiceMasterNo.Text;
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
                //BindVendor(0);
                //ddlVendor.ClearSelection();
                //if (gvrow.Cells[10].Text == "&nbsp;")
                //{
                //    ddlVendor.Items.FindByText("--Select Vendor--").Selected = true;
                //}
                //else
                //{
                //    ddlVendor.SelectedValue = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                //}
                rdbServiceType.ClearSelection();
                if (gvrow.Cells[12].Text == "&nbsp;")
                {
                    rdbServiceType.ClearSelection();
                }
                else
                {
                    if (gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "") == "True")
                    {
                        rdbServiceType.SelectedValue = "1";
                        RadioCheckServiceType();
                        ddlServiceCategory.ClearSelection();
                        BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Edit");
                        if (gvrow.Cells[9].Text == "&nbsp;")
                        {
                            ddlServiceCategory.Items.FindByText("--Select Service Category--").Selected = true;
                        }
                        else
                        {
                            ddlServiceCategory.SelectedValue = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                        }
                        BindServiceList("Edit");
                        ddlServiceList.ClearSelection();
                        if (gvrow.Cells[10].Text == "&nbsp;")
                        {
                            ddlServiceList.Items.FindByText("--Select Service List--").Selected = true;
                        }
                        else
                        {
                            ddlServiceList.SelectedValue = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                        }
                    }
                    else
                    {
                        rdbServiceType.SelectedValue = "2";
                        RadioCheckServiceType();
                        ddlEquipmentCategory.ClearSelection();
                        BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Edit");
                        if (gvrow.Cells[13].Text == "&nbsp;")
                        {
                            ddlEquipmentCategory.Items.FindByText("--Select Equipment Category--").Selected = true;
                        }
                        else
                        {
                            ddlEquipmentCategory.SelectedValue = gvrow.Cells[13].Text.Trim().Replace("&nbsp;", "");
                        }
                        ddlEquipmentSubCat.ClearSelection();
                        GetEquipementSubCategory("Edit");
                        if (gvrow.Cells[14].Text == "&nbsp;")
                        {
                            ddlEquipmentSubCat.Items.FindByText("-- Select Equipment Sub Category --").Selected = true;
                        }
                        else
                        {
                            ddlEquipmentSubCat.SelectedValue = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                        }
                        ddlEquipmentList.ClearSelection();
                        GetEquipementList("Edit");
                        if (gvrow.Cells[15].Text == "&nbsp;")
                        {
                            ddlEquipmentList.Items.FindByText("--Select Equipment List--").Selected = true;
                        }
                        else
                        {
                            ddlEquipmentList.SelectedValue = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        protected void btnRemoveRow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gvSearchSRDetails.FooterRow.Visible = false;
                Control control = null;
                if (gvSearchSRDetails.FooterRow != null)
                {
                    control = gvSearchSRDetails.FooterRow;
                }
                else
                {
                    control = gvSearchSRDetails.Controls[0].Controls[0];
                }
                TextBox Service = control.FindControl("FootService") as TextBox;
                TextBox Unit = control.FindControl("FootUnit") as TextBox;
                TextBox Price = control.FindControl("FootPrice") as TextBox;

                Service.Text = "";
                Unit.Text = "";
                Price.Text = "";
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //Functions objfun = new Functions();
            //objfun.MessageDialog(this, "Check Hai");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowSuccess('Print Link is not given');", true);
            string sservicemasterIds = string.Empty;
            List<BindServiceRequestReport> llstreview = new List<BindServiceRequestReport>();
            if ((ViewState["ServiceRequestMasterID"] == null) || (Convert.ToString(ViewState["ServiceRequestMasterID"]) == ""))
            {
                //SearchGrid();
                foreach (GridViewRow row in grdSRMaster.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (sservicemasterIds == string.Empty)
                            sservicemasterIds = row.Cells[11].Text;
                        else
                            sservicemasterIds = sservicemasterIds + "," + row.Cells[11].Text;
                    }
                }
                llstreview = lclsservice.BindServiceRequestReport(null, sservicemasterIds, defaultPage.UserId,defaultPage.UserId).ToList();
            }
            else
            {
                sservicemasterIds = ViewState["ServiceRequestMasterID"].ToString();
                sservicemasterIds = sservicemasterIds.Replace(",", "");
                llstreview = lclsservice.BindServiceRequestReport(sservicemasterIds, null, defaultPage.UserId,defaultPage.UserId).ToList();
            }

            rvservicerequestreport.ProcessingMode = ProcessingMode.Local;
            rvservicerequestreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceReqestReview.rdlc");
            string s = Convert.ToString(ViewState["ServiceRequestMasterID"]);
            //s = s.Substring(0, s.Length - 1);
            string q = Convert.ToString(ViewState["SerachFilters"]);
            Int64 r = defaultPage.UserId;

            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("ServiceRequestMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));
            this.rvservicerequestreport.LocalReport.SetParameters(p1);

            ReportDataSource datasource = new ReportDataSource("ServiceRequestReviewDS", llstreview);
            rvservicerequestreport.LocalReport.DataSources.Clear();
            rvservicerequestreport.LocalReport.DataSources.Add(datasource);
            rvservicerequestreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvservicerequestreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "ServiceRequest" + guid + ".pdf";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, _sessionPDFFileName);
            using (StreamWriter sw = new StreamWriter(File.Create(path)))
            {
                sw.Write("");
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            // byte[] data = new byte[fs.Length];
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            ShowPDFFile(path, "");
            ViewState["ServiceRequestMasterID"] = "";
        }

        private void ShowPDFFile(string path, string serviceattach)
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
                            Response.Flush();

                        }
                        //if (_sessionPDFFileName.Contains("ICD10"))
                        //{

                        //}
                        //else
                        //{
                        //    System.IO.File.Delete(path);
                        //}
                        //Response.End();
                    }
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    if (serviceattach == "")
                    {
                        Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                    }
                    else
                    {
                        Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path) + ",SR"));
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }


        protected void btnImgUploadQuote_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Label ServiceRequestDetailsID = (Label)gvrow.FindControl("lbServiceRequestDetailsID");
            divSearchMachine.Style.Add("display", "none");
            divUploadFile.Style.Add("display", "block");
            UploadOpacity.Attributes["class"] = "Upopacity";
            Label lblSINo = (Label)gvrow.FindControl("lblSINo");

            if (ServiceRequestDetailsID != null)
            {
                HddDetailsID.Value = ServiceRequestDetailsID.Text;
            }
            else if (lblSINo != null)
            {
                HddDetailsID.Value = "0";
                HddRowIndex.Value = lblSINo.Text;
            }
            else
            {
                HddDetailsID.Value = "0";
                HddRowIndex.Value = "0";
            }
            txtDescrip.Text = "";
            BindServiceAttachment();
        }

        protected void btnupclose_Click(object sender, EventArgs e)
        {
            if (HddMasterID.Value != "")
            {
                divSearchMachine.Style.Add("display", "block");
            }
            divUploadFile.Style.Add("display", "none");
            UploadOpacity.Attributes["class"] = "";
        }

        protected void btnuploadfile_Click(object sender, EventArgs e)
        {
            if (Fileuploadfile.HasFile)
            {
                try
                {
                    List<GetServiceAttachment> llstServiceAttachment = lclsservice.GetServiceAttachment(Convert.ToInt64(HddDetailsID.Value)).ToList();
                    string path = string.Empty;
                    string llstmessage = string.Empty;
                    path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();

                    string extension = System.IO.Path.GetExtension(Fileuploadfile.FileName);
                    if (extension == ".doc" || extension == ".docx" || extension == ".pdf")
                    {
                        string UploadFileName = Fileuploadfile.FileName;
                        string FilePath = string.Empty;
                        int img = Fileuploadfile.PostedFile.ContentLength;
                        byte[] msdata = new byte[img];
                        HttpPostedFile Posted_Image = Fileuploadfile.PostedFile;
                        ViewState["EmpImg"] = Posted_Image.InputStream.Read(msdata, 0, img);
                        //string AutoFileName = HddDetailsID.Value + "-" + DateTime.Now.ToString("yyyyMMdd") + "" + DateTime.Now.ToString("hhmmss") + "." + Convert.ToString(DateTime.Now.Ticks) + "" + extension;
                        Guid AutoFileName = Guid.NewGuid();
                        if (extension == ".pdf")
                        {
                            path = path + "Attachment\\PDF\\";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            path = path + AutoFileName + ".pdf";
                            //System.IO.Directory.CreateDirectory(path);
                            //path = path + AutoFileName;

                        }
                        else if (extension == ".docx")
                        {
                            path = path + "Attachment\\WORD\\";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            path = path + AutoFileName + ".docx";
                            //System.IO.Directory.CreateDirectory(path);
                            //path = path + AutoFileName;
                        }
                        else
                        {
                            path = path + AutoFileName + ".doc";
                        }
                        //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        llstServiceRequest.ServiceRequestDetailID = Convert.ToInt64(HddDetailsID.Value);
                        llstServiceRequest.Description = txtDescrip.Text;
                        llstServiceRequest.FileName = UploadFileName;
                        llstServiceRequest.CreatedBy = defaultPage.UserId;
                        Fileuploadfile.SaveAs(path);
                        //ViewState["Fileurl"] = "~/" + FilePath;
                        //string AutoFileName = HddDetailsID.Value + "-" + DateMY + "" + extension;
                        llstServiceRequest.LocationOfTheFile = path;
                        if (HddDetailsID.Value == "0")
                        {
                            llstServiceRequest.SNo = Convert.ToInt32(HddRowIndex.Value);
                            llstServiceRequest.UploadedBy = defaultPage.UserId;
                            List<GetServiceTempAttachment> llstcheckdoc = lclsservice.GetServiceTempAttachment(llstServiceRequest.SNo).ToList();
                            if (llstcheckdoc.Count != 0)
                            {
                                lclsservice.DeleteSRTempAttch(llstServiceRequest.SNo, "");
                            }
                            llstmessage = lclsservice.InsertSRTempAttch(llstServiceRequest);
                        }
                        else
                        {
                            if (llstServiceAttachment.Count != 0)
                            {
                                lclsservice.DeleteServiceAttachment(Convert.ToInt64(HddDetailsID.Value), defaultPage.UserId);
                            }
                            llstmessage = lclsservice.InsertServiceAttachment(llstServiceRequest);
                        }

                        System.Threading.Thread.Sleep(5000);
                        if (llstmessage == "Saved Successfully")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Uploaded Successfully');", true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestMessage.Replace("<<ServiceRequestDescription>>", "File Uploaded Successfully"), true);
                            txtDescrip.Text = "";
                            BindServiceAttachment();
                        }
                        else
                        {
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + llstmessage + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", llstmessage), true);
                        }

                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('Only Word or PDF formats documents can upload');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestOnlyDocMessage, true);
                    }
                }


                catch (Exception ex)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message.ToString() + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('File is not selected');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestNoFileMessage, true);
            }
        }


        public void BindServiceAttachment()
        {
            if (HddDetailsID.Value == "0")
            {
                List<GetServiceTempAttachment> llstserviceattach = lclsservice.GetServiceTempAttachment(Convert.ToInt32(HddRowIndex.Value)).ToList();
                GrdUploadFile.DataSource = llstserviceattach;
                GrdUploadFile.DataBind();
            }
            else
            {
                List<GetServiceAttachment> llstServiceAttachment = lclsservice.GetServiceAttachment(Convert.ToInt64(HddDetailsID.Value)).ToList();
                GrdUploadFile.DataSource = llstServiceAttachment;
                GrdUploadFile.DataBind();
            }

        }

        protected void btnImgView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkbtn = sender as ImageButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvrow.Cells[2].Text;
                //string filepdfpath = Server.MapPath(filePath);
                //string output = Regex.Replace(filepdfpath, "[\\]", "/");
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string extension = System.IO.Path.GetExtension(filePath);
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                path = Path.Combine(path, filePath);
                //response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath.ToString() + "\"");
                byte[] data = req.DownloadData(path);

                if (extension == ".pdf")
                {
                    ShowPDFFile(path, "SR");
                }
                else
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.WriteFile(file.FullName);
                        Response.Flush();
                    }
                    else
                    {
                        Response.Write("This file does not exist.");
                    }

                }

                //if (extension == ".pdf")
                //{
                //    // Open PDF File in Web Browser 
                //    mpeimgview.Show();
                //    frame1.Attributes["src"] = Path.GetFullPath(filePath.ToString());
                //    //byte[] buffer = req.DownloadData(Server.MapPath(filePath));
                //    //Response.ContentType = "application/pdf";
                //    //Response.AddHeader("content-length", buffer.Length.ToString());
                //    //Response.BinaryWrite(buffer);                
                //}
                //else
                //{

                //    // Download Word Document in Web Browser
                //    string path = string.Empty;
                //    //string localuploadpath = System.Configuration.ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //    if (file_name.Contains(filePath))
                //    {
                //        path = file_name;
                //    }
                //    if (File.Exists(file_name))
                //    {
                //        System.Net.WebClient client = new System.Net.WebClient();
                //        Byte[] buffer = client.DownloadData(file_name);
                //        if (buffer != null)
                //        {
                //            Response.ContentType = "application/docx";
                //            Response.AddHeader("content-length", buffer.Length.ToString());
                //            Response.BinaryWrite(buffer);
                //        }
                //        System.IO.File.Delete(path);
                //        Response.End();
                //    }
                //response.BinaryWrite(data);
                //Response.Redirect(filePath.ToString(), false);

                //mpeimgview.Show();
                //frame1.Attributes["src"] = "http://docs.google.com/viewer?embedded=true&url=" + filePath;

                //frame1.Src = "";
                //frame1.Attributes.Clear();
                //frame1.Dispose();
                //frame1.Attributes["src"] = filePath;                
            }


            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            }
            //response.End();
        }


        protected void btnpopclose_Click(object sender, EventArgs e)
        {
            mpeimgview.Hide();
        }

        /// <summary>
        /// Search Service Request Master details gridview
        /// </summary>
        #region Bind Search Values
        public void SearchGrid()
        {
            try
            {
                if (drpcor.SelectedValue == "All")
                {
                    llstServiceRequest.CorporateName = "ALL";
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

                    llstServiceRequest.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacility.SelectedValue == "All")
                {
                    llstServiceRequest.FacilityName = "ALL";
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
                    llstServiceRequest.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    llstServiceRequest.Status = "ALL";
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
                    llstServiceRequest.Status = FinalString;
                }
                SB.Clear();


                //llstServiceRequest.VendorID = Convert.ToInt64(drpvendor.SelectedValue);
                //if (txtDateFrom.Text != "") llstServiceRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                //if (txtDateTo.Text != "") llstServiceRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstServiceRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstServiceRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                // --- Arjun ---- Start //

                llstServiceRequest.LoggedinBy = defaultPage.UserId;
                List<SearchServiceRequestMaster> llstSearchMaster = lclsservice.SearchServiceRequestMaster(llstServiceRequest).ToList();
                grdSRMaster.DataSource = llstSearchMaster;
                grdSRMaster.DataBind();

                ///--- Arjun -- END

                //if (llstServiceRequest.DateFrom <= llstServiceRequest.DateTo)
                //{
                //    List<SearchServiceRequestMaster> llstSearchMaster = lclsservice.SearchServiceRequestMaster(llstServiceRequest).ToList();
                //    grdSRMaster.DataSource = llstSearchMaster;
                //    grdSRMaster.DataBind();
                //}
                //else
                //{
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('Date from should be less than date to');", true);
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMessage.Replace("<<ServiceRequestDescription>>", "Date from should be less than date to"), true);
                //}
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void btnSearchDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label ServiceRequestDetailsID = (Label)gvrow.FindControl("lbSRDetailsID");
                HddDetailsID.Value = ServiceRequestDetailsID.Text;
                // Label2.Text = "Are you sure you want to delete this Record?";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                Label2.Text = ex.Message;
            }
        }

        protected void btnImgDeleteQuote_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnattachment.Value = gvrow.Cells[0].Text;
                // Label2.Text = "Are you sure you want to delete this Record?";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                ViewState["Deletevalue"] = "servcatattachdelete";

                if (HddDetailsID.Value == "0")
                {

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Label2.Text = ex.Message;
            }
        }

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

        protected void drpfacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindVendor(1);
        }

        protected void ddlCorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
            BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
        }

        protected void ddlFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindVendor(0);
            SetInitialRow();
        }

        protected void imgeservicecatadd_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlFacility.SelectedIndex != 0)
            {
                mpeServiceCat.Show();
                ddlServiceCategory.SelectedIndex = 0;
                txtServiceCategory.Text = "";
                btnaddservicecategory.Text = "Save";
                txtServiceCategory.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectFacilityMessage.Replace("<<Action>>", "adding"), true);
            }

        }

        protected void imgeservicecatedit_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlServiceCategory.SelectedIndex == 0)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Should select a Service Category');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectCatMessage, true);
            }
            else if (ddlFacility.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectFacilityMessage.Replace("<<Action>>", "editing"), true);
            }
            else
                mpeServiceCat.Show();
            txtServiceCategory.Text = ddlServiceCategory.SelectedItem.Text;
            ViewState["ServiceCatID"] = txtServiceCategory.Text;
            btnaddservicecategory.Text = "Update";
            txtServiceCategory.Enabled = true;
        }

        protected void imgeservicecatdelete_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlServiceCategory.SelectedIndex == 0)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Should select a Service Category');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectCatMessage, true);
            }
            else if (ddlFacility.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectFacilityMessage.Replace("<<Action>>", "deleting"), true);
            }
            else
            {
                mpeServiceCat.Show();
                txtServiceCategory.Text = ddlServiceCategory.SelectedItem.Text;
                btnaddservicecategory.Text = "Delete";
                txtServiceCategory.Enabled = false;
            }


        }

        protected void btnaddservicecategory_Click(object sender, EventArgs e)
        {
            try
            {
                BALServiceRequest objbalServiceCat = new BALServiceRequest();
                if (txtServiceCategory.Text == "")
                {
                    txtServiceCategory.Style.Add("border", "Solid 1px red");
                    mpeServiceCat.Show();
                }
                else
                {
                    objbalServiceCat.ServiceCategoryID = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                    ViewState["ServiceCatID"] = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                    objbalServiceCat.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                    objbalServiceCat.ServiceCatDesc = txtServiceCategory.Text;
                    objbalServiceCat.CreatedBy = defaultPage.UserId;
                    objbalServiceCat.LastModifiedBy = defaultPage.UserId;
                    if (objbalServiceCat.ServiceCategoryID == 0 && btnaddservicecategory.Text != "Update" && btnaddservicecategory.Text != "Delete")
                    {
                        List<GetServiceCategory> lstcount = lclsservice.GetServiceCategory(Convert.ToInt64(ddlFacility.SelectedValue), "Add").Where(o => o.ServiceCatDescription == txtServiceCategory.Text.ToString()).ToList();
                        if (lstcount.Count <= 0)
                        {
                            string Servcat = lclsservice.InsertServiceCategoryMaster(objbalServiceCat);
                            if (Servcat == "Saved Successfully")
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestNormalMessage.Replace("<<ServiceRequestDescription>>", "Saved Successfully"), true);
                            }

                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowWarning('Record Exists');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestRecordExistMessage, true);
                        }


                    }
                    else if (btnaddservicecategory.Text == "Delete")
                    {
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                        ViewState["Deletevalue"] = "Servcatdelete";
                        ViewState["ddlServiceCategory"] = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                    }
                    else
                    {
                        List<GetServiceCategory> lstcount = lclsservice.GetServiceCategory(Convert.ToInt64(ddlFacility.SelectedValue), "Add").Where(o => o.ServiceCatDescription == txtServiceCategory.Text.ToString()).ToList();
                        if (lstcount.Count <= 0)
                        {
                            string Servcar = lclsservice.UpdateServiceCategoryMaster(objbalServiceCat);
                            if (Servcar == "Updated Successfully")
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Updated Successfully');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestNormalMessage.Replace("<<ServiceRequestDescription>>", "Updated Successfully"), true);
                                btnaddservicecategory.Text = "Save";
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowWarning('Record Exists');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestRecordExistMessage, true);
                        }
                    }
                    BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
                    List<SavedServiceCategory> lstsave = lclsservice.SavedServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), Convert.ToInt64(ViewState["ServiceCatID"])).ToList();
                    ddlServiceCategory.SelectedValue = Convert.ToString(lstsave[0].ServiceCategoryID);
                    ddlServiceList.ClearSelection();
                }
            }


            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
            }
        }

        protected void btnservicecategoryclose_Click(object sender, EventArgs e)
        {
            mpeServiceCat.Hide();
            txtServiceCategory.Style.Remove("border");
        }
        protected void imgeservicelistadd_Click(object sender, ImageClickEventArgs e)
        {

            if (ddlServiceCategory.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectListMessage, true);
            }
            else
            {
                ddlServiceList.SelectedIndex = 0;
                mpeServiceList.Show();
                txtServiceList.Text = "";
                btnaddserviceList.Text = "Save";
                txtServiceList.Enabled = true;
            }
        }

        protected void imgeservicelistedit_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlServiceList.SelectedIndex == 0 && ddlServiceCategory.SelectedIndex == 0)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Should select a Service Category with Service List Value');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestSelectListMessage, true);
            }
            else if (ddlServiceCategory.SelectedIndex > 0 && ddlServiceList.SelectedIndex == 0)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Should select a Service List');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestListMessage, true);
            }

            else
                mpeServiceList.Show();
            txtServiceList.Text = ddlServiceList.SelectedItem.Text;
            ViewState["ServiceListID"] = txtServiceList.Text;
            btnaddserviceList.Text = "Update";
            txtServiceList.Enabled = true;
        }

        protected void imgeservicelistdelete_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlServiceList.SelectedIndex == 0)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Should select a Service List');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestListMessage, true);
            }
            else
                mpeServiceList.Show();
            txtServiceList.Text = ddlServiceList.SelectedItem.Text;
            btnaddserviceList.Text = "Delete";
            txtServiceList.Enabled = false;
        }
        protected void btnaddserviceList_Click(object sender, EventArgs e)
        {
            try
            {
                BALServiceRequest objbalServiceList = new BALServiceRequest();
                Functions objfun = new Functions();
                if (txtServiceList.Text == "")
                {
                    txtServiceList.Style.Add("border", "Solid 1px red");
                    mpeServiceList.Show();
                }
                else
                {
                    ViewState["ServiceListID"] = Convert.ToInt64(ddlServiceList.SelectedValue);
                    objbalServiceList.ServiceCategoryID = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                    objbalServiceList.ServiceListID = Convert.ToInt64(ddlServiceList.SelectedValue);
                    objbalServiceList.ServiceListDesc = txtServiceList.Text;
                    objbalServiceList.CreatedBy = defaultPage.UserId;
                    objbalServiceList.LastModifiedBy = defaultPage.UserId;
                    if (objbalServiceList.ServiceListID == 0 && btnaddserviceList.Text != "Update" && btnaddserviceList.Text != "Delete")
                    {
                        List<GetServiceList> lstcount = lclsservice.GetServiceList(ddlServiceCategory.SelectedIndex, "Add").Where(o => o.ServiceListDescription == txtServiceList.Text.ToString()).ToList();
                        if (lstcount.Count <= 0)
                        {
                            string Servlist = lclsservice.InsertServiceListMaster(objbalServiceList);
                            if (Servlist == "Saved Successfully")
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestNormalMessage.Replace("<<ServiceRequestDescription>>", "Saved Successfully"), true);
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowWarning('Record Exists');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestRecordExistMessage, true);
                        }
                    }
                    else
                    {
                        if (btnaddserviceList.Text == "Delete")
                        {
                            List<GetActiveServiceListvalue> lstactiveServlst = new List<GetActiveServiceListvalue>();
                            lstactiveServlst = lclsservice.GetActiveServiceListvalue(Convert.ToInt64(ddlServiceList.SelectedValue)).ToList();
                            if (lstactiveServlst.Count == 0)
                            {
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                                ViewState["Deletevalue"] = "servlstdelete";
                                ViewState["ddlServiceList"] = Convert.ToInt64(ddlServiceList.SelectedValue);
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('ServiceList Should not allow to delete for Active ServiceRequest');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestListDelActMessage, true);
                            }
                        }
                        else
                        {
                            List<GetServiceList> lstcount = lclsservice.GetServiceList(ddlServiceCategory.SelectedIndex, "Add").Where(o => o.ServiceListDescription == txtServiceList.Text.ToString()).ToList();
                            if (lstcount.Count <= 0)
                            {
                                string Servlistup = lclsservice.UpdateServiceListMaster(objbalServiceList);
                                if (Servlistup == "Updated Successfully")
                                {
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Saved Successfully');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestNormalMessage.Replace("<<ServiceRequestDescription>>", "updated Successfully"), true);
                                    btnaddserviceList.Text = "Save";
                                }

                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowWarning('Record Exists');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestRecordExistMessage, true);
                            }
                        }
                    }
                    BindServiceList("Add");
                    List<SavedServiceList> lstservlst = lclsservice.SavedServiceList(Convert.ToInt64(ViewState["ServiceListID"])).ToList();
                    ddlServiceList.SelectedValue = Convert.ToString(lstservlst[0].ServiceListID);
                }
            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            }

        }

        protected void btnservicelistclose_Click(object sender, EventArgs e)
        {
            mpeServiceList.Hide();
            txtServiceList.Style.Remove("border");
        }

        protected void btnYes_Click(object sender, ImageClickEventArgs e)
        {
            //string b = string.Empty;
            //BALServiceRequest objbalServiceCat = new BALServiceRequest();
            //string deletevalues = Convert.ToString(ViewState["Deletevalue"]);
            //List<CheckServicelist> lstservlist = new List<CheckServicelist>();
            //if (deletevalues == "Servcatdelete")
            //{
            //    Int64 servcatvalue = Convert.ToInt64(ViewState["ddlServiceCategory"]);
            //    lstservlist = lclsservice.GetCheckServicelist(servcatvalue).ToList();
            //    if (lstservlist.Count > 0)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Selected category have Servicelist values');", true);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestListValMessage, true);
            //    }
            //    else
            //    {

            //        b = lclsservice.DeleteServiceCategoryMaster(Convert.ToInt64(ViewState["ddlServiceCategory"]), defaultPage.UserId);
            //        if (b == "Deleted Successfully")
            //        {
            //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
            //            hdnServicecat.Value = "0";
            //            ViewState["Deletevalue"] = "";
            //            ViewState["ddlServiceCategory"] = 0;
            //            BindServiceCategory(Convert.ToInt64(ddlFacility.SelectedValue),"Add");
            //            ddlServiceList.Items.Clear();
            //            ddlServiceList.DataSource = null;
            //            ddlServiceList.DataBind();
            //        }
            //    }
            //}
            //else if (deletevalues == "servlstdelete")
            //{
            //    b = lclsservice.DeleteServiceListMaster(Convert.ToInt64(ViewState["ddlServiceList"]), defaultPage.UserId);
            //    if (b == "Deleted Successfully")
            //    {
            //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
            //        //hdnServicelist.Value = "0";
            //        //imgeservicelistdelete.Style.Add("display", "none");
            //        ViewState["Deletevalue"] = "";
            //        ViewState["ddlServiceList"] = "";
            //        BindServiceList("Add");
            //    }
            //}
            //else if (deletevalues == "servcatattachdelete")
            //{
            //    try
            //    {

            //        InventoryServiceClient lclsService = new InventoryServiceClient();
            //        string lstrMessage = lclsService.DeleteServiceAttachment(Convert.ToInt64(hdnattachment.Value), defaultPage.UserId);
            //        if (lstrMessage == "Deleted Successfully")
            //        {
            //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
            //            BindServiceAttachment(); ;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            //    }

            //}
            //else
            //{
            //    try
            //    {

            //        InventoryServiceClient lclsService = new InventoryServiceClient();
            //        string lstrMessage = lclsService.DeleteServiceRequestDetails(Convert.ToInt64(HddDetailsID.Value), false, defaultPage.UserId);
            //        if (lstrMessage == "Deleted Successfully")
            //        {
            //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
            //            BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            //    }
            //}
        }

        public void RadioCheckServiceType()
        {
            if (rdbServiceType.SelectedValue == "1")
            {
                DivServiceCategory.Style.Add("display", "block");
                DivServiceList.Style.Add("display", "block");
                DivEquipmentCategory.Style.Add("display", "none");
                DivEquipmentSubCat.Style.Add("display", "none");
                DivEquipmentList.Style.Add("display", "none");
                ReqdrdddlServiceCategory.Visible = true;
                ReqdrdddlServiceList.Visible = true;
                ReqdrdddlEquipmentCategory.Visible = false;
                ReqdrdddlEquipmentList.Visible = false;
                ddlEquipmentCategory.ClearSelection();
                ddlEquipmentSubCat.ClearSelection();
                ddlEquipmentList.ClearSelection();
                BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            }
            else if (rdbServiceType.SelectedValue == "2")
            {
                DivServiceCategory.Style.Add("display", "none");
                DivServiceList.Style.Add("display", "none");
                DivEquipmentCategory.Style.Add("display", "block");
                DivEquipmentSubCat.Style.Add("display", "block");
                DivEquipmentList.Style.Add("display", "block");
                ReqdrdddlEquipmentCategory.Visible = true;
                ReqdrdddlEquipmentList.Visible = true;
                ReqdrdddlServiceCategory.Visible = false;
                ReqdrdddlServiceList.Visible = false;
                ddlServiceCategory.ClearSelection();
                ddlServiceList.ClearSelection();
                BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            }
        }

        protected void rdbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioCheckServiceType();
        }

        protected void btnreviewcancel_Click(object sender, EventArgs e)
        {
            mpeSerRequReview.Hide();
        }

        protected void btnSaveReview_Click(object sender, EventArgs e)
        {
            try
            {
                llstServiceRequest.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                llstServiceRequest.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                //llstServiceRequest.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                if (rdbServiceType.SelectedValue == "1")
                {
                    llstServiceRequest.ServiceType = true;
                    llstServiceRequest.ServiceCategoryID = Convert.ToInt64(ddlServiceCategory.SelectedValue);
                    llstServiceRequest.ServiceListID = Convert.ToInt64(ddlServiceList.SelectedValue);
                }
                else if (rdbServiceType.SelectedValue == "2")
                {
                    llstServiceRequest.ServiceType = false;
                    llstServiceRequest.EquipmentCategoryID = Convert.ToInt64(ddlEquipmentCategory.SelectedValue);
                    llstServiceRequest.EquipementSubCategoryID = Convert.ToInt64(ddlEquipmentSubCat.SelectedValue);
                    llstServiceRequest.EquipmentListID = Convert.ToInt64(ddlEquipmentList.SelectedValue);
                }

                llstServiceRequest.CreatedBy = defaultPage.UserId;

                if (HddMasterID.Value == "")
                {
                    InsertServiceRequest();
                }
                else
                {
                    UpdateServiceRequest();
                }

            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            string smachinePartID = string.Empty;
            smachinePartID = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
            // ViewState["ServiceRequestMasterID"] = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "") + ",";
            // ViewState["SerachFilters"] = gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "") + "," + gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "") + "," + gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "") + "," + gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "") + "," + gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "");
            List<BindServiceRequestReport> llstreview = lclsservice.BindServiceRequestReport(smachinePartID, null, defaultPage.UserId,defaultPage.UserId).ToList();
            rvservicerequestreport.ProcessingMode = ProcessingMode.Local;
            rvservicerequestreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceReqestReview.rdlc");

            //s = s.Substring(0, s.Length - 1);
            // string s = Convert.ToString(ViewState["ServiceRequestMasterID"]);
            //s = s.Substring(0, s.Length - 1);
            // string q = Convert.ToString(ViewState["SerachFilters"]);
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("ServiceRequestMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));
            this.rvservicerequestreport.LocalReport.SetParameters(p1);

            ReportDataSource datasource = new ReportDataSource("ServiceRequestReviewDS", llstreview);
            rvservicerequestreport.LocalReport.DataSources.Clear();
            rvservicerequestreport.LocalReport.DataSources.Add(datasource);
            rvservicerequestreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvservicerequestreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "ServiceRequest" + guid + ".pdf";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, _sessionPDFFileName);
            using (StreamWriter sw = new StreamWriter(File.Create(path)))
            {
                sw.Write("");
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            // byte[] data = new byte[fs.Length];
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            ShowPDFFile(path, "");
        }

        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            string b = string.Empty;
            BALServiceRequest objbalServiceCat = new BALServiceRequest();
            string deletevalues = Convert.ToString(ViewState["Deletevalue"]);
            List<CheckServicelist> lstservlist = new List<CheckServicelist>();
            if (deletevalues == "Servcatdelete")
            {
                Int64 servcatvalue = Convert.ToInt64(ViewState["ddlServiceCategory"]);
                lstservlist = lclsservice.GetCheckServicelist(servcatvalue).ToList();
                if (lstservlist.Count > 0)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('Selected category have Servicelist values');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestListValMessage, true);
                }
                else
                {

                    b = lclsservice.DeleteServiceCategoryMaster(Convert.ToInt64(ViewState["ddlServiceCategory"]), defaultPage.UserId);
                    if (b == "Deleted Successfully")
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
                        hdnServicecat.Value = "0";
                        ViewState["Deletevalue"] = "";
                        ViewState["ddlServiceCategory"] = 0;
                        BindServiceCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
                        ddlServiceList.Items.Clear();
                        ddlServiceList.DataSource = null;
                        ddlServiceList.DataBind();
                    }
                }
            }
            else if (deletevalues == "servlstdelete")
            {
                b = lclsservice.DeleteServiceListMaster(Convert.ToInt64(ViewState["ddlServiceList"]), defaultPage.UserId);
                if (b == "Deleted Successfully")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
                    //hdnServicelist.Value = "0";
                    //imgeservicelistdelete.Style.Add("display", "none");
                    ViewState["Deletevalue"] = "";
                    ViewState["ddlServiceList"] = "";
                    BindServiceList("Add");
                }
            }
            else if (deletevalues == "servcatattachdelete")
            {
                try
                {
                    string lstrMessage = string.Empty;
                    InventoryServiceClient lclsService = new InventoryServiceClient();
                    if (HddDetailsID.Value == "0")
                    {
                        lstrMessage = lclsService.DeleteSRTempAttch(Convert.ToInt32(HddRowIndex.Value), "");
                    }
                    else
                    {
                        lstrMessage = lclsService.DeleteServiceAttachment(Convert.ToInt64(HddDetailsID.Value), defaultPage.UserId);
                    }
                    if (lstrMessage == "Deleted Successfully")
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
                        BindServiceAttachment(); ;
                    }
                }
                catch (Exception ex)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
                }

            }
            else
            {
                try
                {

                    InventoryServiceClient lclsService = new InventoryServiceClient();
                    string lstrMessage = lclsService.DeleteServiceRequestDetails(Convert.ToInt64(HddDetailsID.Value), false, defaultPage.UserId);
                    lstrMessage = lclsService.DeleteServiceAttachment(Convert.ToInt64(HddDetailsID.Value), defaultPage.UserId);
                    if (lstrMessage == "Deleted Successfully")
                    {
                        if (lstrMessage == "Deleted Successfully")
                        {
                            BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                        }
                        else
                        {
                            SetPreviousData();
                            DataTable dt = (DataTable)ViewState["CurrentTable"];
                            dt.Rows.RemoveAt(Convert.ToInt32(HddDetailsID.Value));
                            gvSearchSRDetails.DataSource = dt;
                            gvSearchSRDetails.DataBind();                            
                        }
                        
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Deleted Successfully');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", "Deleted Successfully"), true);
                        //BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    }

                }
                catch (Exception ex)
                {
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
                }
            }
        }

        protected void ddlEquipmentSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipementList("Add");
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
                UploadOpacity.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            UploadOpacity.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                UploadOpacity.Attributes["class"] = "Upopacity";
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
            HddListCorpID.Value = "";
            
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

            HddListCorpID.Value = "";        
        }

       
    }
}