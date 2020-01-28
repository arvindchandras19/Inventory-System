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


/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<MajorItem>>
'' Type      :   C# File
'' Description  :<<To add,update,delete the CR Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	12/12/2017		   V1.0				   Murali M		                     New
 * 06/Mar/2018         V1.0             Vivekanand.S                    Multi Search
''--------------------------------------------------------------------------------
'*/


namespace Inventory
{
    public partial class CapitalItem : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALCapital llstCR = new BALCapital();
        string a = string.Empty;
        string b = string.Empty;
        string ErrorList = string.Empty;
        string PendingApproval = Constant.PendingApprovalforreq;
        string loadshipping = Constant.loadshipping;
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.grdCRsearch);
                scriptManager.RegisterPostBackControl(this.GvTempEdit);
                HddQueryStringID.Value = Request.QueryString["CapitalItemMasterID"];
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    //BindGrid(0);

                    if (defaultPage != null)
                    {
                        if (HddQueryStringID.Value != "")
                        {
                            GetOrderDetails();
                        }
                        else
                        {
                            BindCorporate(1, "Add");
                            //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                            BindFacility(1, "Add");
                            //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                            BindVendor(1, "Add");
                            BindStatus("Add");
                            rdorequesttypesearch.SelectedValue = "1";
                            SearchGrid();
                            if (defaultPage.CapitalItemRequest_Edit == false && defaultPage.CapitalItemRequest_View == true)
                            {
                                btnAdd.Visible = false;
                                btnSave.Visible = false;
                                btnReview.Visible = false;
                                btnImgDeletePopUp.Visible = false;
                                btnnewrowup.Visible = false;
                            }
                            if (defaultPage.CapitalItemRequest_Edit == false && defaultPage.CapitalItemRequest_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                            }
                            //BindCorporate(1, "Add");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }
        public void BindQueryStringvalues()
        {
            List<GetCapitalItemMaster> lstMPRMaster = lclsservice.GetCapitalItemMaster().Where(a => a.CapitalItemMasterID == Convert.ToInt64(HddQueryStringID.Value)).ToList();
            BindCorporate(0, "Edit");
            ddlCorporate.ClearSelection();
            ddlCorporate.SelectedValue = Convert.ToString(lstMPRMaster[0].CorporateID);
            BindFacility(0, "Edit");
            ddlFacility.ClearSelection();
            ddlFacility.SelectedValue = Convert.ToString(lstMPRMaster[0].FacilityID);
            BindVendor(0, "Edit");
            ddlVendor.ClearSelection();
            ddlVendor.SelectedValue = Convert.ToString(lstMPRMaster[0].VendorID);
            BindShipping("Add");
            ddlShipping.ClearSelection();
            ddlShipping.SelectedValue = Convert.ToString(lstMPRMaster[0].Shipping);

            if (lstMPRMaster[0].Type == "New")
            {
                rdorequesttype.SelectedValue = "0";
            }
            if (lstMPRMaster[0].Type == "Replacement")
            {
                rdorequesttype.SelectedValue = "1";
            }
            lblMasterNo.Text = lstMPRMaster[0].CRNo;

            ddlCorporate.Enabled = false;
            ddlFacility.Enabled = false;
            ddlVendor.Enabled = false;
            ddlShipping.Enabled = false;
            rdorequesttype.Enabled = false;
            //BindCorporate(0, "Edit");
            //BindFacility(0, "Edit");
            //BindVendor(0, "Edit");
            //BindLookUp("Add");
        }

        public void GetOrderDetails()
        {
            btnAdd.Visible = false;
            btnSearch.Visible = false;
            btnPrint.Visible = false;
            btnReview.Visible = true;
            btnClose.Visible = true;
            btnSave.Visible = true;
            lblrcount4.Visible = false;
            divMPRMaster.Style.Add("display", "none");
            DivMPRMasterNo.Style.Add("display", "block");
            lblMasterNo.Visible = true;
            lblseroutHeader.Visible = false;
            divContentDetails.Style.Add("display", "block");
            divSearchMachine.Style.Add("display", "block");
            lblAddItemHeader.Style.Add("display", "none");
            lblAddItemHeader.Visible = false;
            lblEditHeader.Style.Add("display", "block");
            lblEditHeader.Visible = false;
            lblMasterHeader.Visible = true;
            divsearch.Style.Add("display", "none");
            divAddMachine.Style.Add("display", "block");
            divMPRDetails.Style.Add("display", "block");
            lblseroutHeader.Visible = false;
            lblseroutHeader.Style.Add("display", "none");
            lblUpdateHeader.Style.Add("display", "block");
            lblMasterHeader.Style.Add("display", "none");
            lblMasterHeader.Visible = true;
            lblUpdateHeader.Visible = true;
            btnNew.Visible = false;
            btnPrint.Visible = false;
            HddMasterID.Value = HddQueryStringID.Value;
            BindQueryStringvalues();
            string LockTimeOut = "";
            LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
            List<GetCapitalItemDetails> lstMPRMasterDetails = lclsservice.GetCapitalItemDetails(Convert.ToInt64(HddMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();
            gvCREditDetails.DataSource = lstMPRMasterDetails;
            gvCREditDetails.DataBind();
        }

        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            ReqDateFrom.ErrorMessage = req;
            ReqdrdddlFacility.ErrorMessage = req;
            ReqdrdddlShipping.ErrorMessage = req;
            ReqdrdddlVendor.ErrorMessage = req;
            Reqdrpcorsearch.ErrorMessage = req;
            ReqdrpStatus.ErrorMessage = req;
            reqrdorequesttype.ErrorMessage = req;
            Reqrdorequesttypesearch.ErrorMessage = req;
            Reqdrpvendorsearch.ErrorMessage = req;
            Reqdrpfacilitysearch.ErrorMessage = req;
            ReqtxtDateTo.ErrorMessage = req;
            ReqdrdddlCorp.ErrorMessage = req;

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
                        drpcorsearch.DataSource = lstfacility;
                        drpcorsearch.DataTextField = "CorporateName";
                        drpcorsearch.DataValueField = "CorporateID";
                        drpcorsearch.DataBind();
                        //drpcorsearch.Items.Insert(0, lst);
                        //drpcorsearch.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        drpcorsearch.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        drpcorsearch.DataTextField = "CorporateName";
                        drpcorsearch.DataValueField = "CorporateID";
                        drpcorsearch.DataBind();
                        //drpcorsearch.Items.Insert(0, lst);
                        //drpcorsearch.SelectedIndex = 0;
                    }
                    foreach (ListItem lst in drpcorsearch.Items)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                    if (drpcorsearch.SelectedValue != "")
                    {
                        foreach (ListItem lst in drpcorsearch.Items)
                        {
                            if (lst.Selected && drpcorsearch.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        // Search Drop Down
                        drpfacilitysearch.DataSource = lclsservice.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                        drpfacilitysearch.DataTextField = "FacilityDescription";
                        drpfacilitysearch.DataValueField = "FacilityID";
                        drpfacilitysearch.DataBind();

                        //if (defaultPage.RoleID == 1)
                        //{
                        //    if (drpcorsearch.SelectedValue != "All")
                        //    {
                        //        // Search Drop Down
                        //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                        //        drpfacilitysearch.DataTextField = "FacilityDescription";
                        //        drpfacilitysearch.DataValueField = "FacilityID";
                        //        drpfacilitysearch.DataBind();
                        //        //ListItem lst = new ListItem();
                        //        //lst.Value = "All";
                        //        //lst.Text = "All";
                        //        //drpfacilitysearch.Items.Insert(0, lst);
                        //        //drpfacilitysearch.SelectedIndex = 0;
                        //    }
                        //    else
                        //    {
                        //        drpfacilitysearch.SelectedIndex = 0;
                        //    }
                        //}
                        //else
                        //{
                        //    if (drpcorsearch.SelectedValue != "All")
                        //    {
                        //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == drpcorsearch.SelectedItem.Text).ToList();
                        //        drpfacilitysearch.DataTextField = "FacilityName";
                        //        drpfacilitysearch.DataValueField = "FacilityID";
                        //        drpfacilitysearch.DataBind();
                        //        //ListItem lst = new ListItem();
                        //        //lst.Value = "All";
                        //        //lst.Text = "All";
                        //        //drpfacilitysearch.Items.Insert(0, lst);
                        //        //drpfacilitysearch.SelectedIndex = 0;
                        //    }
                        //    else
                        //    {
                        //        drpfacilitysearch.SelectedIndex = 0;
                        //    }
                        //}
                    }
                    foreach (ListItem lst in drpfacilitysearch.Items)
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
                BindVendor(1,"Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                    if (drpfacilitysearch.SelectedValue != "")
                    {
                        foreach (ListItem lst in drpfacilitysearch.Items)
                        {
                            if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        string SearchText = string.Empty;
                        List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
                        lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a=>a.MachineParts == true).ToList();
                        drpvendorsearch.DataSource = lstvendor;
                        drpvendorsearch.DataTextField = "VendorDescription";
                        drpvendorsearch.DataValueField = "VendorID";
                        drpvendorsearch.DataBind();
                        //ListItem lst = new ListItem();
                        //lst.Value = "All";
                        //lst.Text = "All";
                        //drpvendorsearch.Items.Insert(0, lst);
                        //drpvendorsearch.SelectedIndex = 0;                        
                    }
                    foreach (ListItem lst in drpvendorsearch.Items)
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);

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
                lstLookUp = lclsservice.GetList("CapitalItemRequest", "Status", Mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                drpStatussearch.Items.FindByText(PendingApproval).Selected = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void BindShipping(string Mode)
        {
            try
            {
                List<GetList> lstLookUpship = new List<GetList>();
                lstLookUpship = lclsservice.GetList("CapitalItemRequest", "Shipping", Mode).ToList();
                ddlShipping.DataSource = lstLookUpship;
                ddlShipping.DataTextField = "InvenValue";
                ddlShipping.DataValueField = "InvenValue";
                ddlShipping.DataBind();
                ddlShipping.Items.FindByText(loadshipping).Selected = true;

            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }
        #endregion
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lclsservice.SyncCapitalReceivingorder();
                SetAddScreen(1);
                ClearDetails();
                SetInitialRow();
                BindShipping("Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        #region corporate dropdown SelectedIndexChanged event
        protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            foreach (ListItem lst in drpcorsearch.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }


            if (i == 1)
            {
                BindFacility(1, "Add");
                foreach (ListItem lst in drpcorsearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListCorpID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (ListItem lst in drpcorsearch.Items)
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
                foreach (ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListCorpID.Value = "";
                }
                BindFacility(1, "Add");
            }

        }
        #endregion


        #region facility dropdown SelectedIndexChanged event
        protected void drpfacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ListItem lst in drpfacilitysearch.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }

            if (i == 1)
            {
                BindVendor(1, "Add");
                foreach (ListItem lst in drpfacilitysearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListFacID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (ListItem lst in drpfacilitysearch.Items)
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
                foreach (ListItem lst in drpfacilitysearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListFacID.Value = "";
                }
                BindVendor(1, "Add");
            }
        }
        #endregion


        protected void ddlFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendor(0, "Add");
            RebindGrid();
        }

        protected void ddlCorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
            BindEquipSubcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            RebindGrid();
        }



        private void SetAddScreen(int i)
        {
            try
            {
                if (i == 1)
                {
                    if (defaultPage.CapitalItemRequest_Edit == true && defaultPage.CapitalItemRequest_View == true)
                    {

                        btnSave.Visible = true;
                        btnReview.Visible = true;
                        btnAdd.Visible = false;
                    }
                    if (defaultPage.CapitalItemRequest_Edit == false && defaultPage.CapitalItemRequest_View == true)
                    {
                        btnSave.Visible = false;
                        btnReview.Visible = false;
                        btnAdd.Visible = false;
                        ddlCorporate.Enabled = false;
                        ddlFacility.Enabled = false;
                    }
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
                }
                else
                {
                    if (defaultPage.CapitalItemRequest_Edit == true && defaultPage.CapitalItemRequest_View == true)
                    {
                        btnSave.Visible = false;
                        btnReview.Visible = false;
                        btnAdd.Visible = true;
                    }
                    btnClose.Visible = true;
                    btnSearch.Visible = true;
                    BindCorporate(1, "Add");
                    //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                    BindFacility(1, "Add");
                    //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void SearchGrid()
        {
            try
            {
                BALCapital llstCRSearch = new BALCapital();
               
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstCRSearch.CorporateName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpcorsearch.Items)
                    {
                        if (lst.Selected && drpcorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    llstCRSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstCRSearch.FacilityName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpfacilitysearch.Items)
                    {
                        if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstCRSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstCRSearch.VendorName = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpvendorsearch.Items)
                    {
                        if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstCRSearch.VendorName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstCRSearch.Status = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpStatussearch.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstCRSearch.Status = FinalString;
                }
                SB.Clear();
                //if (txtDateFrom.Text != "") llstCRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                //if (txtDateTo.Text != "") llstCRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstCRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstCRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                llstCRSearch.loggedinBy = defaultPage.UserId;
                if (txtDateFrom.Text != "" && txtDateTo.Text != "")
                {
                    if (rdorequesttypesearch.SelectedValue == "1")
                    {
                        List<SearchCapitalItemRequestMaster> lstCRMaster = lclsservice.SearchCapitalItemRequestMaster(llstCRSearch).ToList();
                        grdCRsearch.DataSource = lstCRMaster;
                        grdCRsearch.DataBind();
                    }
                    else
                    {
                        List<SearchCapitalItemRequestMaster> lstCRMaster = lclsservice.SearchCapitalItemRequestMaster(llstCRSearch).Where(o => o.Type == Convert.ToString(rdorequesttypesearch.SelectedItem.Text)).ToList();
                        grdCRsearch.DataSource = lstCRMaster;
                        grdCRsearch.DataBind();
                    }
                }
                else
                {
                    List<SearchCapitalItemRequestMaster> lstCRMaster = lclsservice.SearchCapitalItemRequestMaster(llstCRSearch).ToList();
                    grdCRsearch.DataSource = lstCRMaster;
                    grdCRsearch.DataBind();
                }



                ViewState["ReportCapitalItemID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void BindGrid(int TestEdit)
        {
            try
            {

                if (TestEdit == 1)
                {
                    BALCapital llstCRSearch = new BALCapital();
                    
                    if (drpcorsearch.SelectedValue == "All")
                    {
                        llstCRSearch.CorporateName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpcorsearch.Items)
                        {
                            if (lst.Selected && drpcorsearch.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        llstCRSearch.CorporateName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpfacilitysearch.SelectedValue == "All")
                    {
                        llstCRSearch.FacilityName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpfacilitysearch.Items)
                        {
                            if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstCRSearch.FacilityName = FinalString;

                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpvendorsearch.SelectedValue == "All")
                    {
                        llstCRSearch.VendorName = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpvendorsearch.Items)
                        {
                            if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstCRSearch.VendorName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpStatussearch.SelectedValue == "All")
                    {
                        llstCRSearch.Status = "ALL";
                    }
                    else
                    {
                        foreach (ListItem lst in drpStatussearch.Items)
                        {
                            if (lst.Selected)
                            {
                                SB.Append(lst.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                        llstCRSearch.Status = FinalString;
                    }
                    SB.Clear();
                    //if (txtDateFrom.Text != "") llstCRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    //if (txtDateTo.Text != "") llstCRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    if (txtDateFrom.Text == "")
                    {
                        txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstCRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    }
                    if (txtDateTo.Text == "")
                    {
                        txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        llstCRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                    }                    
                    llstCRSearch.loggedinBy = defaultPage.UserId;
                    List<SearchCapitalItemRequestMaster> lstCRMaster = lclsservice.SearchCapitalItemRequestMaster(llstCRSearch).ToList();
                    GvTempEdit.DataSource = lstCRMaster;
                    GvTempEdit.DataBind();
                }
                else
                {
                    SearchGrid();

                }
                ViewState["ReportCapitalItemID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        private void ClearDetails()
        {
            ddlCorporate.ClearSelection();
            ddlFacility.ClearSelection();
            ddlVendor.ClearSelection();
            ddlShipping.ClearSelection();
            ddlCorporate.Enabled = true;
            ddlFacility.Enabled = true;
            rdorequesttype.Enabled = true;
            gvAddCRDetails.DataSource = null;
            gvAddCRDetails.DataBind();
            gvCREditDetails.DataSource = null;
            gvCREditDetails.DataBind();
            GvTempEdit.DataSource = null;
            GvTempEdit.DataBind();
            btnPrint.Visible = false;
            rdorequesttype.SelectedIndex = 0;
            ddlCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
            BindFacility(0, "Add");
            ddlFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
            BindVendor(0, "Add");
            ViewState["ReportCapitalItemID"] = "";
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        public void BindEquipSubcategory(Int64 CorporateID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList drpequipcat = (DropDownList)gvAddCRDetails.Rows[0].Cells[1].FindControl("drpequipcat");
                List<GetEquipmentSubCategoryforCapital> lstequipcat = lclsservice.GetEquipmentSubCategoryforCapital(Convert.ToInt64(ddlCorporate.SelectedValue), Mode).ToList();
                drpequipcat.DataSource = lstequipcat;
                drpequipcat.DataValueField = "EquipementSubCategoryID";
                drpequipcat.DataTextField = "EquipmentSubCategoryDescription";
                drpequipcat.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpequipcat.Items.Insert(0, lst);
                drpequipcat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void GetEquipementList(Int64 EquimentSubCategoryID, string Mode, int rowIndex)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList drpequiplst = (DropDownList)gvAddCRDetails.Rows[rowIndex].Cells[2].FindControl("drpequiplst");
                List<GetEquipementListforCapital> lstequiplist = lclsservice.GetEquipementListforCapital(EquimentSubCategoryID, Mode).ToList();
                if (lstequiplist.Count > 0)
                {
                    drpequiplst.DataSource = lstequiplist;
                    drpequiplst.DataValueField = "EquipementListID";
                    drpequiplst.DataTextField = "EquipmentListDescription";
                    drpequiplst.DataBind();
                }
                else
                {
                    drpequiplst.Items.Clear();
                }
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpequiplst.Items.Insert(0, lst);
                drpequiplst.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void FootBindEquipSubcategory(Int64 CorporateID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList FootddlEquip = (DropDownList)gvCREditDetails.FooterRow.FindControl("FootddlEquip");
                List<GetEquipmentSubCategoryforCapital> lstfootequipcat = lclsservice.GetEquipmentSubCategoryforCapital(Convert.ToInt64(ddlCorporate.SelectedValue), Mode).ToList();
                FootddlEquip.DataSource = lstfootequipcat;
                FootddlEquip.DataValueField = "EquipementSubCategoryID";
                FootddlEquip.DataTextField = "EquipmentSubCategoryDescription";
                FootddlEquip.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                FootddlEquip.Items.Insert(0, lst);
                FootddlEquip.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void FootGetEquipementList(Int64 EquimentSubCategoryID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList Footdrpequiplst = (DropDownList)gvCREditDetails.FooterRow.FindControl("Footddlequiplst");
                List<GetEquipementListforCapital> lstequiplist = lclsservice.GetEquipementListforCapital(EquimentSubCategoryID, Mode).ToList();
                if (lstequiplist.Count > 0)
                {
                    Footdrpequiplst.DataSource = lstequiplist;
                    Footdrpequiplst.DataValueField = "EquipementListID";
                    Footdrpequiplst.DataTextField = "EquipmentListDescription";
                    Footdrpequiplst.DataBind();
                }
                else
                {
                    Footdrpequiplst.Items.Clear();
                }
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                Footdrpequiplst.Items.Insert(0, lst);
                Footdrpequiplst.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (HddUpdateLockinEdit.Value == "Edit")
            {
                a = lclsservice.AutoUpdateLockedOut(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId), "CapitalItemRequest");
                HddUpdateLockinEdit.Value = "";
            }

            if (HddQueryStringID.Value != "")
            {
                Response.Redirect("CapitalOrder.aspx");
            }
            SetAddScreen(0);
            ClearDetails();
            ClearMaster();
            HddMasterID.Value = "";
            btnPrint.Visible = true;
            btnReview.Enabled = true;
            rdorequesttypesearch.SelectedValue = "1";
            if (defaultPage.RoleID != 1)
            {
                btnReview.Visible = false;
                btnAdd.Visible = true;
                SearchGrid();
                if (defaultPage.CapitalItemRequest_Edit == false && defaultPage.CapitalItemRequest_View == true)
                {
                    btnAdd.Visible = false;
                }
            }
            else
            {
                SearchGrid();
            }
        }

        private void ClearMaster()
        {
            drpvendorsearch.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatussearch.ClearSelection();
            BindStatus("Add");
            BindCorporate(1,"Add");
            BindFacility(1, "Add");
            BindVendor(1, "Add");
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string sCapitalItmeRequestIds = string.Empty;
            List<BindCapitalItemRequestReport> llstreview = new List<BindCapitalItemRequestReport>();
            if ((ViewState["ReportCapitalItemID"] == null) || (Convert.ToString(ViewState["ReportCapitalItemID"]) == ""))
            {
                //SearchGrid();
                foreach (GridViewRow row in grdCRsearch.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (sCapitalItmeRequestIds == string.Empty)
                            sCapitalItmeRequestIds = row.Cells[15].Text;
                        else
                            sCapitalItmeRequestIds = sCapitalItmeRequestIds + "," + row.Cells[15].Text;
                    }
                }
                llstreview = lclsservice.BindCapitalItemRequestReport(null, sCapitalItmeRequestIds, defaultPage.UserId, defaultPage.UserId).ToList();
            }
            else
            {

                sCapitalItmeRequestIds = ViewState["ReportCapitalItemID"].ToString();
                sCapitalItmeRequestIds = sCapitalItmeRequestIds.Replace(",", "");
                llstreview = lclsservice.BindCapitalItemRequestReport(sCapitalItmeRequestIds, null, defaultPage.UserId, defaultPage.UserId).ToList();
            }
            rvCapitalItemsreport.ProcessingMode = ProcessingMode.Local;
            rvCapitalItemsreport.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalItemRequestReview.rdlc");
            string s = Convert.ToString(ViewState["ReportCapitalItemID"]);
            string q = Convert.ToString(ViewState["SerachFilters"]);
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("CapitalItemMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));
            this.rvCapitalItemsreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("CapitalItemRequestReviewDS", llstreview);
            rvCapitalItemsreport.LocalReport.DataSources.Clear();
            rvCapitalItemsreport.LocalReport.DataSources.Add(datasource);
            rvCapitalItemsreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvCapitalItemsreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

            //Random rnd = new Random();
            //int month = rnd.Next(1, 13); // creates a number between 1 and 12
            //int dice = rnd.Next(1, 7); // creates a number between 1 and 6
            //int card = rnd.Next(9); // creates a number between 0 and 51
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "MajorItemRequest" + guid + ".pdf";
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
            ShowPDFFile(path);
            ViewState["ReportCapitalItemID"] = "";
            //string path = Server.MapPath(_sessionPDFFileName);
            // Open PDF File in Web Browser 
            RebindGrid();
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
                        if (_sessionPDFFileName.Contains("ICD10"))
                        {

                        }
                        else
                        {
                            System.IO.File.Delete(path);
                        }
                        Response.End();
                    }
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }
            }
            catch
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                llstCR.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                llstCR.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                llstCR.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                llstCR.Shipping = ddlShipping.SelectedValue;
                if (rdorequesttype.SelectedValue == "1")
                    llstCR.RequestType = true;
                else
                    llstCR.RequestType = false;
                llstCR.CreatedBy = defaultPage.UserId;
                llstCR.ShippingCost = (grdreview.FooterRow.FindControl("txtsipcost") as TextBox).Text;
                llstCR.Tax = (grdreview.FooterRow.FindControl("txttax") as TextBox).Text;
                llstCR.TotalCost = Convert.ToDecimal((grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text);
                if (HddMasterID.Value == "")
                {
                    InsertCR();
                }
                else
                {
                    UpdateCR();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void InsertCR()
        {
            try
            {
                List<object> lstIDwithmessage = new List<object>();
                string ErrorList = string.Empty;
                foreach (GridViewRow grdfs in gvAddCRDetails.Rows)
                {
                    DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipcat");
                    DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                    Label lblser = (Label)grdfs.FindControl("lblser");
                    TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                    TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                    TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                    TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                    if (drpequcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }
                if (ErrorList == "")
                {
                    // Insert Capital Items Master
                    lstIDwithmessage = lclsservice.InsertCapitalItemMaster(llstCR).ToList();
                    a = lstIDwithmessage[0].ToString();
                    llstCR.CapitalItemMasterID = Convert.ToInt64(lstIDwithmessage[1]);

                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddCRDetails.Rows)
                    {
                        DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        Label lblser = (Label)grdfs.FindControl("lblser");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");

                        llstCR.EquipmentSubCategoryID = Convert.ToInt64(drpequcat.SelectedValue);
                        llstCR.EquipementListID = Convert.ToInt64(drpequlst.SelectedValue);
                        llstCR.SerialNo = lblser.Text;
                        llstCR.OrderQuantity = Convert.ToInt32(txtqty.Text);
                        llstCR.PricePerUnit = Convert.ToDecimal(txtppq.Text);
                        llstCR.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                        llstCR.Reason = txtreason.Text;


                        // InsertCapital Items Details
                        b = lclsservice.InsertCapitalItemDetails(llstCR);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReqMessage.Replace("<<MajorItem>>", ""), true);
                }
                if (a == b && b == "Saved Successfully")
                {
                    ClearDetails();
                    btnPrint.Visible = true;
                    SetAddScreen(0);
                    if (defaultPage.RoleID != 1)
                    {
                        List<GetCapitalItemMaster> lstCRMaster = lclsservice.GetCapitalItemMaster().ToList();
                        //List<GetCapitalItemMaster> lstCRMaster = lclsservice.GetCapitalItemMaster().Where(c => c.FacilityID == Convert.ToInt64(ddlFacility.SelectedValue)).ToList();
                        //grdCRsearch.DataSource = lstCRMaster;
                        //grdCRsearch.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemSaveMessage.Replace("<<MajorItem>>", lstCRMaster[0].CRNo.ToString()), true);
                        ViewState["ReportCapitalItemID"] = llstCR.CapitalItemMasterID;
                    }
                    else
                    {
                        List<GetCapitalItemMaster> lstCRMaster = lclsservice.GetCapitalItemMaster().ToList();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemSaveMessage.Replace("<<MajorItem>>", lstCRMaster[0].CRNo.ToString()), true);
                        ViewState["ReportCapitalItemID"] = llstCR.CapitalItemMasterID;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }

        }

        public void UpdateCR()
        {
            try
            {
                if (gvCREditDetails.PageCount == 0)
                {
                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddCRDetails.Rows)
                    {
                        DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        Label lblser = (Label)grdfs.FindControl("lblser");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");

                        llstCR.EquipmentSubCategoryID = Convert.ToInt64(drpequcat.SelectedValue);
                        llstCR.EquipementListID = Convert.ToInt64(drpequlst.SelectedValue);
                        llstCR.SerialNo = lblser.Text;
                        llstCR.OrderQuantity = Convert.ToInt32(txtqty.Text);
                        llstCR.PricePerUnit = Convert.ToDecimal(txtppq.Text);
                        llstCR.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                        llstCR.Reason = txtreason.Text;
                        llstCR.CapitalItemMasterID = Convert.ToInt64(HddMasterID.Value);

                        a = lclsservice.InsertCapitalItemDetails(llstCR);
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in gvCREditDetails.Rows)
                    {
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        if (txtqty.Text == "")
                        {
                            ErrorList = "Qty field should not be Empty";
                        }
                    }

                    if (ErrorList == "")
                    {
                        foreach (GridViewRow grdfs in gvCREditDetails.Rows)
                        {
                            Label lblequ = (Label)grdfs.FindControl("lblEquipCategory");
                            Label lblqulst = (Label)grdfs.FindControl("lblEquipList");
                            Label lblSerialNo = (Label)grdfs.FindControl("lblSerialNo");
                            TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                            TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                            TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                            TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                            Label MPRMasterID = (Label)grdfs.FindControl("lbCRMasterID");
                            Label MPRDetailsID = (Label)grdfs.FindControl("lbCRDetailsID");

                            llstCR.CapitalItemMasterID = Convert.ToInt64(MPRMasterID.Text);
                            llstCR.CapitalItemDetailsID = Convert.ToInt64(MPRDetailsID.Text);
                            llstCR.OrderQuantity = Convert.ToInt32(txtqty.Text);
                            llstCR.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                            llstCR.Reason = txtreason.Text;
                            llstCR.LastModifiedBy = Convert.ToInt64(defaultPage.UserId);

                            b = lclsservice.UpdateCapitalItemDetails(llstCR);
                        }

                        a = lclsservice.UpdateCapitalIemMaster(llstCR);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemQtyMessage.Replace("<<MajorItem>>", ""), true);
                    }

                    if (gvCREditDetails.FooterRow.Visible == true)
                    {
                        FooterRowSave();
                    }
                }

                if (a == "Saved Successfully")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemSaveMessage.Replace("<<MajorItem>>", lblmprreview.Text), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                }

                else if (b == "Updated Successfully")
                {
                    List<GetCapitalItemMaster> lstCRMaster = lclsservice.GetCapitalItemMaster().ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemUpdateMessage.Replace("<<MajorItem>>", lblmprreview.Text), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                dt.Columns.Add(new DataColumn("Column7", typeof(string)));

                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dt.Rows.Add(dr);
                ViewState["CurrentTable"] = dt;

                //Bind the DataTable to the Grid

                gvAddCRDetails.DataSource = dt;
                gvAddCRDetails.DataBind();

                DropDownList ddl1 = (DropDownList)gvAddCRDetails.Rows[0].Cells[1].FindControl("drpequipcat");
                DropDownList ddl2 = (DropDownList)gvAddCRDetails.Rows[0].Cells[2].FindControl("drpequiplst");
                Label box1 = (Label)gvAddCRDetails.Rows[0].Cells[3].FindControl("lblser");
                TextBox box2 = (TextBox)gvAddCRDetails.Rows[0].Cells[4].FindControl("txtqty");
                TextBox box3 = (TextBox)gvAddCRDetails.Rows[0].Cells[5].FindControl("txtppq");
                TextBox box4 = (TextBox)gvAddCRDetails.Rows[0].Cells[6].FindControl("txttotprice");
                TextBox box5 = (TextBox)gvAddCRDetails.Rows[0].Cells[7].FindControl("txtreason");

                BindEquipSubcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow grdfs in gvAddCRDetails.Rows)
                {
                    DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipcat");
                    DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                    Label lblser = (Label)grdfs.FindControl("lblser");
                    TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                    TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                    TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                    TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");

                    if (rdorequesttype.SelectedValue == "0")
                    {
                        if (drpequcat.SelectedValue == "0"|| txtqty.Text == "" || txtppq.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }
                    }
                    else
                    {
                        if (drpequcat.SelectedValue == "0" ||  drpequlst.SelectedValue == "0" ||txtqty.Text == "" || txtppq.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }

                    }
               }
                if (ErrorList == "")
                {
                   AddNewRowToGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReqMessage.Replace("<<MajorItem>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void gvAddCRDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Int64 CorpID;
                    CorpID = Convert.ToInt64(ddlCorporate.SelectedValue);
                    string Mode = "Add";
                    //Find the DropDownList in the Row
                    List<GetEquipmentSubCategoryforCapital> lstequipcat = lclsservice.GetEquipmentSubCategoryforCapital(CorpID, Mode).ToList();
                    DropDownList drpequipcat = (e.Row.FindControl("drpequipcat") as DropDownList);
                    drpequipcat.DataSource = lstequipcat;
                    drpequipcat.DataValueField = "EquipementSubCategoryID";
                    drpequipcat.DataTextField = "EquipmentSubCategoryDescription";
                    drpequipcat.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    drpequipcat.Items.Insert(0, lst);
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList drpequipcat = e.Row.FindControl("drpequipcat") as DropDownList;
                    DropDownList drpequiplst = e.Row.FindControl("drpequiplst") as DropDownList;
                    Label lserame = e.Row.FindControl("lblser") as Label;
                    TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");
                    TextBox txtppq = (TextBox)e.Row.FindControl("txtppq");
                    TextBox txttotprice = (TextBox)e.Row.FindControl("txttotprice");
                    TextBox txtreason = (TextBox)e.Row.FindControl("txtreason");

                    int i = e.Row.RowIndex * 8;

                    drpequipcat.Attributes.Add("tabindex", (i + 1).ToString());
                    drpequiplst.Attributes.Add("tabindex", (i + 2).ToString());
                    lserame.Attributes.Add("tabindex", (i + 3).ToString());
                    txtqty.Attributes.Add("tabindex", (i + 4).ToString());
                    txtppq.Attributes.Add("tabindex", (i + 5).ToString());
                    txttotprice.Attributes.Add("tabindex", (i + 6).ToString());
                    txtreason.Attributes.Add("tabindex", (i + 7).ToString());
                    btnNew.Attributes.Add("tabindex", (i + 8).ToString());

                    //if (e.Row.RowIndex == 0)
                    //    drpequipcat.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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

                        DropDownList ddl1 = (DropDownList)gvAddCRDetails.Rows[i].FindControl("drpequipcat");
                        DropDownList ddl2 = (DropDownList)gvAddCRDetails.Rows[i].FindControl("drpequiplst");
                        Label box1 = (Label)gvAddCRDetails.Rows[i].FindControl("lblser");
                        TextBox box2 = (TextBox)gvAddCRDetails.Rows[i].FindControl("txtqty");
                        TextBox box3 = (TextBox)gvAddCRDetails.Rows[i].FindControl("txtppq");
                        TextBox box4 = (TextBox)gvAddCRDetails.Rows[i].FindControl("txttotprice");
                        TextBox box5 = (TextBox)gvAddCRDetails.Rows[i].FindControl("txtreason");

                        dtCurrentTable.Rows[i]["Column1"] = ddl1.SelectedItem.Text;
                        dtCurrentTable.Rows[i]["Column2"] = ddl2.SelectedItem.Text;
                        dtCurrentTable.Rows[i]["Column3"] = box1.Text;
                        dtCurrentTable.Rows[i]["Column4"] = box2.Text;
                        dtCurrentTable.Rows[i]["Column5"] = box3.Text;
                        dtCurrentTable.Rows[i]["Column6"] = box4.Text;
                        dtCurrentTable.Rows[i]["Column7"] = box5.Text;

                    }
                    gvAddCRDetails.DataSource = dtCurrentTable;
                    gvAddCRDetails.DataBind();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                            DropDownList ddl1 = (DropDownList)gvAddCRDetails.Rows[rowIndex].Cells[1].FindControl("drpequipcat");
                            DropDownList ddl2 = (DropDownList)gvAddCRDetails.Rows[rowIndex].Cells[2].FindControl("drpequiplst");
                            Label box1 = (Label)gvAddCRDetails.Rows[rowIndex].Cells[3].FindControl("lblser");
                            TextBox box2 = (TextBox)gvAddCRDetails.Rows[rowIndex].Cells[4].FindControl("txtqty");
                            TextBox box3 = (TextBox)gvAddCRDetails.Rows[rowIndex].Cells[5].FindControl("txtppq");
                            TextBox box4 = (TextBox)gvAddCRDetails.Rows[rowIndex].Cells[6].FindControl("txttotprice");
                            TextBox box5 = (TextBox)gvAddCRDetails.Rows[rowIndex].Cells[7].FindControl("txtreason");

                            if (dt.Rows[i]["Column1"].ToString() != "")
                            {
                                ddl1.Items.FindByText(dt.Rows[i]["Column1"].ToString()).Selected = true;
                                if (rdorequesttype.SelectedValue != "0")
                                {
                                    ddl2.DataSource = GetEquipementListforCapital(Convert.ToInt32(ddl1.SelectedValue), "Add");
                                    ddl2.DataValueField = "EquipementListID";
                                    ddl2.DataTextField = "EquipmentListDescription";
                                    ddl2.DataBind();
                                    ddl2.Enabled = true;
                                    ddl2.Items.FindByText(dt.Rows[i]["Column2"].ToString()).Selected = true;
                                }
                                else
                                {
                                    ListItem lst = new ListItem();
                                    lst.Value = "0";
                                    lst.Text = "Select";
                                    ddl2.Items.Insert(0, lst);
                                    ddl2.Enabled = false;
                                }
                            }

                            box1.Text = dt.Rows[i]["Column3"].ToString();
                            box2.Text = dt.Rows[i]["Column4"].ToString();
                            box3.Text = dt.Rows[i]["Column5"].ToString();
                            box4.Text = dt.Rows[i]["Column6"].ToString();
                            box5.Text = dt.Rows[i]["Column7"].ToString();


                            ddl1.Focus();
                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                            DropDownList ddl1 = (DropDownList)gvAddCRDetails.Rows[i - 1].FindControl("drpequipcat");
                            DropDownList ddl2 = (DropDownList)gvAddCRDetails.Rows[i - 1].FindControl("drpequiplst");
                            Label box1 = (Label)gvAddCRDetails.Rows[i - 1].FindControl("lblser");
                            TextBox box2 = (TextBox)gvAddCRDetails.Rows[i - 1].FindControl("txtqty");
                            TextBox box3 = (TextBox)gvAddCRDetails.Rows[i - 1].FindControl("txtppq");
                            TextBox box4 = (TextBox)gvAddCRDetails.Rows[i - 1].FindControl("txttotprice");
                            TextBox box5 = (TextBox)gvAddCRDetails.Rows[i - 1].FindControl("txtreason");

                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["RowNumber"] = i + 1;
                            if (ddl1.SelectedIndex != 0)
                            {
                                dtCurrentTable.Rows[i - 1]["Column1"] = ddl1.SelectedItem.Text;
                                dtCurrentTable.Rows[i - 1]["Column2"] = ddl2.SelectedItem.Text;
                            }
                            else
                            {
                                dtCurrentTable.Rows[i - 1]["Column1"] = "0";
                            }
                            dtCurrentTable.Rows[i - 1]["Column3"] = box1.Text;
                            dtCurrentTable.Rows[i - 1]["Column4"] = box2.Text;
                            dtCurrentTable.Rows[i - 1]["Column5"] = box3.Text;
                            dtCurrentTable.Rows[i - 1]["Column6"] = box4.Text;
                            dtCurrentTable.Rows[i - 1]["Column7"] = box5.Text;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public List<GetEquipementListforCapital> GetEquipementListforCapital(int EquimentSubCategoryID, string Mode)
        {

            InventoryServiceClient lclsservice = new InventoryServiceClient();

            List<GetEquipementListforCapital> lstequiplist = lclsservice.GetEquipementListforCapital(EquimentSubCategoryID, Mode).ToList();

            return lstequiplist;

        }

        protected void drpequipcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSpin2 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
                int RowIndex = gridrow.RowIndex;
                DropDownList drpequipcat = (DropDownList)gvAddCRDetails.Rows[RowIndex].Cells[1].FindControl("drpequipcat");
                DropDownList drpequiplst = (DropDownList)gvAddCRDetails.Rows[RowIndex].Cells[2].FindControl("drpequiplst");

                if (rdorequesttype.SelectedValue == "1")
                {
                    GetEquipementList(Convert.ToInt64(drpequipcat.SelectedValue), "Add", RowIndex);
                    drpequiplst.Enabled = true;
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    drpequiplst.Items.Insert(0, lst);
                    drpequiplst.Enabled = false;
                }
                RebindGrid();
                drpequipcat.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void drpequiplst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList ddlSpin3 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin3.NamingContainer;
                int RowIndex = gridrow.RowIndex;
                DropDownList drpequipcat = (DropDownList)gvAddCRDetails.Rows[RowIndex].Cells[1].FindControl("drpequipcat");
                DropDownList drpequiplst = (DropDownList)gvAddCRDetails.Rows[RowIndex].Cells[2].FindControl("drpequiplst");
                Label lblser = (Label)gvAddCRDetails.Rows[RowIndex].Cells[3].FindControl("lblser");
                if (rdorequesttype.SelectedValue == "1")
                {
                    List<GetSerialNo> lstserial = lclsservice.GetSerialNo(Convert.ToInt64(drpequipcat.SelectedValue), Convert.ToInt64(drpequiplst.SelectedValue)).ToList();
                    if (lstserial.Count > 0)
                    {
                        lblser.Text = lstserial[0].SerialNo;
                    }
                    else
                    {
                        lblser.Text = "";
                    }
                }
                else
                {
                    lblser.Text = "";
                }

                RebindGrid();
                drpequiplst.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }

        }
        private void EditDisplayControls()
        {
            try
            {
                SetAddScreen(1);
                BindGrid(1);
                DivMPRMasterNo.Style.Add("display", "block");
                divEdit.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "block");
                divAddMachine.Style.Add("display", "none");
                lblMasterNo.Visible = true;
                ddlCorporate.Enabled = false;
                ddlFacility.Enabled = false;
                ddlVendor.Enabled = false;
                ddlShipping.Enabled = false;
                btnPrint.Visible = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                EditDisplayControls();
                hdncheckfield.Value = "1";
                lblEditHeader.Visible = true;
                lblseroutHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblMasterHeader.Visible = true;
                HddUpdateLockinEdit.Value = "Edit";
                HddMasterID.Value = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["ReportCapitalItemID"] = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                lblMasterNo.Text = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                ddlCorporate.ClearSelection();
                if (gvrow.Cells[1].Text == "&nbsp;")
                {

                    ddlCorporate.Items.FindByText("--Select Corporate--").Selected = true;
                }
                else
                {
                    ddlCorporate.SelectedValue = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                }
                BindFacility(0, "Edit");
                ddlFacility.ClearSelection();
                if (gvrow.Cells[2].Text == "&nbsp;")
                {
                    ddlFacility.Items.FindByText("--Select Facility--").Selected = true;
                }
                else
                {
                    ddlFacility.SelectedValue = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                }
                BindVendor(0, "Edit");
                ddlVendor.ClearSelection();
                if (gvrow.Cells[3].Text == "&nbsp;")
                {
                    ddlVendor.Items.FindByText("--Select Vendor--").Selected = true;
                }
                else
                {
                    ddlVendor.SelectedValue = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                }
                BindShipping("Edit");
                ddlShipping.ClearSelection();
                if (gvrow.Cells[12].Text == "&nbsp;")
                {
                    ddlShipping.Items.FindByText("--Select Shipping--").Selected = true;
                }
                else
                {
                    ddlShipping.SelectedValue = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                }
                rdorequesttype.ClearSelection();
                if (gvrow.Cells[9].Text == "&nbsp;")
                {
                    rdorequesttype.ClearSelection();
                }
                else
                {

                    if (gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "") == "Replacement")
                    {
                        rdorequesttype.SelectedValue = "1";
                    }
                    if (gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "") == "New")
                    {
                        rdorequesttype.SelectedValue = "0";
                    }
                }

                rdorequesttype.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void BindDetailGrid(Int64 CapitalItemMasterID, Int64 UserId)
        {
            try
            {
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetCapitalItemDetails> llstCRMaster = lclsservice.GetCapitalItemDetails(CapitalItemMasterID, UserId, Convert.ToInt64(LockTimeOut)).ToList();
                if (llstCRMaster.Count == 0)
                {
                    gvCREditDetails.DataSource = llstCRMaster;
                    gvCREditDetails.DataBind();
                }
                if (llstCRMaster[0].IsReadOnly == 0)
                {
                    gvCREditDetails.Enabled = true;
                    btnReview.Enabled = true;
                }
                else if (llstCRMaster[0].IsReadOnly == 1)
                {
                    gvCREditDetails.Enabled = false;
                    btnReview.Enabled = false;
                    List<GetUserDetails> llstuserdetails = lclsservice.GetUserDetails(Convert.ToInt64(llstCRMaster[0].Lockedby)).ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemMessage.Replace("<<MajorItem>>", "Another user " + llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName + " is updating this record , Please try after some time."), true);
                }
                gvCREditDetails.DataSource = llstCRMaster;
                gvCREditDetails.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                    if (dt.Rows.Count == 1)
                    {
                        DropDownList ddl1 = (DropDownList)gvAddCRDetails.Rows[0].Cells[1].FindControl("drpequipcat");
                        DropDownList ddl2 = (DropDownList)gvAddCRDetails.Rows[0].Cells[2].FindControl("drpequiplst");
                        Label box1 = (Label)gvAddCRDetails.Rows[0].Cells[3].FindControl("lblser");
                        TextBox box2 = (TextBox)gvAddCRDetails.Rows[0].Cells[4].FindControl("txtqty");
                        TextBox box3 = (TextBox)gvAddCRDetails.Rows[0].Cells[5].FindControl("txtppq");
                        TextBox box4 = (TextBox)gvAddCRDetails.Rows[0].Cells[6].FindControl("txttotprice");
                        TextBox box5 = (TextBox)gvAddCRDetails.Rows[0].Cells[7].FindControl("txtreason");

                        ddl1.SelectedValue = "0";
                        ddl2.SelectedValue = "0";
                        box1.Text = "";
                        box2.Text = "";
                        box3.Text = "";
                        box4.Text = "";
                        box5.Text = "";
                    }

                    if (dt.Rows.Count > 1)
                    {
                        dt.Rows.Remove(dt.Rows[rowIndex]);
                        drCurrentRow = dt.NewRow();
                        ViewState["CurrentTable"] = dt;
                        gvAddCRDetails.DataSource = dt;
                        gvAddCRDetails.DataBind();

                        for (int i = 0; i < gvAddCRDetails.Rows.Count - 1; i++)
                        {
                            gvAddCRDetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        }

                        SetPreviousData();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSearchDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label lbCRDetailsID = (Label)gvrow.FindControl("lbCRDetailsID");
                HddDetailsID.Value = lbCRDetailsID.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnClose.Visible = false;
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteCapitalItemDetails(Convert.ToInt64(HddDetailsID.Value), false, defaultPage.UserId);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemDeleteMessage.Replace("<<MajorItem>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnReview_Click(object sender, EventArgs e)
        { 
            try
            {
                bool isrvalid = false;
                string Equcat = string.Empty;
                Int64 revEquipcat = 0;
                Int64 revEquiplist = 0;
                Int64 revfaclityid = Convert.ToInt64(ddlFacility.SelectedValue);

                if (HddMasterID.Value == "" || gvCREditDetails.PageCount == 0)
                {
                    foreach (GridViewRow grdfs in gvAddCRDetails.Rows)
                    {
                        DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        Label lblser = (Label)grdfs.FindControl("lblser");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");

                        if (rdorequesttype.SelectedValue == "0")
                        {
                            if (drpequcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }
                        else
                        {
                            if (drpequcat.SelectedValue == "0" || drpequlst.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }

                        }

                           revEquipcat = Convert.ToInt64(drpequcat.SelectedValue);
                           revEquiplist = Convert.ToInt64(drpequlst.SelectedValue);
                           List<ValidCapitalEquipment> lstMaster = lclsservice.ValidCapitalEquipment(revEquipcat, revEquiplist, revfaclityid).ToList();
                            if (lstMaster[0].Edit == 1)
                            {
                                isrvalid = true;
                            }
                            else
                            {
                                Equcat += drpequcat.SelectedItem.Text + ",";
                                isrvalid = false;
                            }
                        
                    }
                    if (ErrorList == "")
                    {
                        if (isrvalid == true && Equcat == "")
                        {
                            mpereview.Show();
                            bindreview();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemValidMessage.Replace("<<MajorItem>>", "" +Equcat), true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReqMessage.Replace("<<MajorItem>>", ""), true);
                    }

                }
                else
                {
                    bool isqtycgd = false;
                    bool isevalid = false;
                    Int64 FootEquipcat = 0;
                    Int64 FootEquiplist = 0;
                    Int64 Footfaclityid = 0;
                    string FootEqucat = string.Empty;
                    foreach (GridViewRow grdfs in gvCREditDetails.Rows)
                    {
                        Label lblEquipCategory = (Label)grdfs.FindControl("drpequipcat");
                        Label lblEquipList = (Label)grdfs.FindControl("drpequiplst");
                        Label lblser = (Label)grdfs.FindControl("lblser");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");

                        //string tt = txttotprice.Text;
                        if (txtqty.Text == "")
                        {
                            ErrorList = "Qty field should not be empty";
                        }

                        Label lblqty = (Label)grdfs.FindControl("lblqty");
                        if (txtqty.Text != lblqty.Text)
                        {
                            if (txtreason.Text == "")
                            {
                                isqtycgd = true;
                                txtreason.Style.Add("border", "Solid 1px red");
                            }

                        }
                    }

                    if (isqtycgd == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReasonMessage.Replace("<<MajorItem>>", ""), true);
                    }


                    if (ErrorList == "" && gvCREditDetails.FooterRow.Visible == true)
                    {
                        Control control = null;
                        if (gvCREditDetails.FooterRow != null)
                        {
                            control = gvCREditDetails.FooterRow;
                        }
                        else
                        {
                            control = gvCREditDetails.Controls[0].Controls[0];
                        }

                        DropDownList FootddlEquip = control.FindControl("FootddlEquip") as DropDownList;
                        DropDownList Footddlequiplst = control.FindControl("Footddlequiplst") as DropDownList;
                        Label Footser = control.FindControl("Footser") as Label;
                        TextBox Foottxtqty = control.FindControl("Foottxtqty") as TextBox;
                        TextBox Foottxtppq = control.FindControl("Foottxtppq") as TextBox;
                        TextBox Foottxttotprice = control.FindControl("Foottxttotprice") as TextBox;
                        TextBox Foottxtreason = control.FindControl("Foottxtreason") as TextBox;

                        if (rdorequesttype.SelectedValue == "0")
                        {
                            if (FootddlEquip.SelectedValue == "0" || Foottxtqty.Text == "" || Foottxtppq.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }
                        else
                        {
                            if (FootddlEquip.SelectedValue == "0" || Footddlequiplst.SelectedValue == "0" || Foottxtqty.Text == "" || Foottxtppq.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }

                        }

                            FootEquipcat = Convert.ToInt64(FootddlEquip.SelectedValue);
                            FootEquiplist = Convert.ToInt64(Footddlequiplst.SelectedValue);
                            Footfaclityid = Convert.ToInt64(ddlFacility.SelectedValue);
                            List<ValidCapitalEquipment> lstMaster = lclsservice.ValidCapitalEquipment(FootEquipcat, FootEquiplist, Footfaclityid).ToList();
                            if (lstMaster[0].Edit == 1)
                            {
                                isevalid = true;
                            }
                            else
                            {
                                FootEqucat += FootddlEquip.SelectedItem.Text + ",";
                                isevalid = false;
                            }
                        

                        if (ErrorList == "")
                        {
                            if (isevalid == true && FootEqucat == "")
                            {
                                mpereview.Show();
                                bindreview();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemValidMessage.Replace("<<MajorItem>>", "" +FootEqucat), true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReqMessage.Replace("<<MajorItem>>", ""), true);
                        }
                    }
                    else
                    {
                        if (ErrorList == "" && isqtycgd == false)
                        {
                            mpereview.Show();
                            bindreview();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemQtyMessage.Replace("<<MajorItem>>", ""), true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void bindreview()
        {
            try
            {
                lblCorp.Text = ddlCorporate.SelectedItem.Text;
                lblFac.Text = ddlFacility.SelectedItem.Text;
                lblVen.Text = ddlVendor.SelectedItem.Text;
                lblship.Text = ddlShipping.SelectedItem.Text;
                lblreqtype.Text = rdorequesttype.SelectedItem.Text;
                lblreqdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lblmprreview.Text = lblMasterNo.Text;

                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                dr = null;
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("Shipping");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("Equipcat");
                dt.Columns.Add("Equiplst");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Priceperqty");
                dt.Columns.Add("TotalPrice");
                dt.Columns.Add("Reason");
                dt.Columns.Add("txtsipcost");
                dt.Columns.Add("txttax");
                dt.Columns.Add("txtTotalcost");

                //Add New Capital Request //
                if (HddMasterID.Value == "" || gvCREditDetails.PageCount == 0)
                {
                    DivMPRMasterNoreview.Style.Add("display", "none");
                    lblmprreview.Visible = false;
                    if (HddMasterID.Value != "")
                    {
                        DivMPRMasterNoreview.Style.Add("display", "block");
                        lblmprreview.Visible = true;
                    }
                    foreach (GridViewRow row in gvAddCRDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = HddMasterID.Value;
                        DropDownList drpequlst = (row.FindControl("drpequiplst") as DropDownList);
                        dr["Equipcat"] = (row.FindControl("drpequipcat") as DropDownList).SelectedItem.Text;
                        if (rdorequesttype.SelectedValue == "0" || drpequlst.SelectedValue == "0")
                        {
                            dr["Equiplst"] = "";
                        }
                        else
                        {
                            dr["Equiplst"] = (row.FindControl("drpequiplst") as DropDownList).SelectedItem.Text;
                        }
                        dr["SerialNo"] = (row.FindControl("lblser") as Label).Text;
                        dr["Qty"] = (row.FindControl("txtqty") as TextBox).Text;
                        dr["Priceperqty"] = (row.FindControl("txtppq") as TextBox).Text;
                        dr["TotalPrice"] = (row.FindControl("txttotprice") as TextBox).Text;
                        dr["Reason"] = (row.FindControl("txtreason") as TextBox).Text;

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
                    (grdreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
                    (grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                }
                //Edit or Update the Exisiting Capital Items Request//
                else
                {
                    DivMPRMasterNoreview.Style.Add("display", "block");
                    lblmprreview.Visible = true;
                    List<GetCapitalItemMaster> llstmpr = lclsservice.GetCapitalItemMaster().Where(a => a.CapitalItemMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                    foreach (GridViewRow row in gvCREditDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = HddMasterID.Value;
                        dr["Equipcat"] = (row.FindControl("lblEquipCategory") as Label).Text;
                        dr["Equiplst"] = (row.FindControl("lblEquipList") as Label).Text;
                        dr["SerialNo"] = (row.FindControl("lblSerialNo") as Label).Text;
                        dr["Qty"] = (row.FindControl("txtqty") as TextBox).Text;
                        dr["Priceperqty"] = (row.FindControl("txtppq") as TextBox).Text;
                        dr["TotalPrice"] = (row.FindControl("txttotprice") as TextBox).Text;
                        dr["Reason"] = (row.FindControl("txtreason") as TextBox).Text;
                        dt.Rows.Add(dr);
                    }

                    if (gvCREditDetails.FooterRow.Visible == true)
                    {
                        dr = dt.NewRow();
                        //gvSearchMRPDetails.FooterRow.Visible = false;
                        Control control = null;
                        if (gvCREditDetails.FooterRow != null)
                        {
                            control = gvCREditDetails.FooterRow;
                        }
                        else
                        {
                            control = gvCREditDetails.Controls[0].Controls[0];
                        }

                        DropDownList FootddlEquip = control.FindControl("FootddlEquip") as DropDownList;
                        DropDownList Footddlequiplst = control.FindControl("Footddlequiplst") as DropDownList;
                        Label Footser = control.FindControl("Footser") as Label;
                        TextBox Foottxtqty = control.FindControl("Foottxtqty") as TextBox;
                        TextBox Foottxtppq = control.FindControl("Foottxtppq") as TextBox;
                        TextBox Foottxttotprice = control.FindControl("Foottxttotprice") as TextBox;
                        TextBox Foottxtreason = control.FindControl("Foottxtreason") as TextBox;

                        dr["Equipcat"] = FootddlEquip.SelectedItem.Text;
                        if (rdorequesttype.SelectedValue == "0" || Footddlequiplst.SelectedValue == "0")
                        {
                            dr["Equiplst"] = "";
                        }
                        else
                        {
                            dr["Equiplst"] = Footddlequiplst.SelectedItem.Text;
                        }
                        dr["SerialNo"] = Footser.Text;
                        dr["Qty"] = Foottxtqty.Text;
                        dr["Priceperqty"] = Foottxtppq.Text;
                        dr["TotalPrice"] = Foottxttotprice.Text;
                        dr["Reason"] = Foottxtreason.Text;

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void RebindGrid()
        {
            try
            {
                if (HddMasterID.Value == "" || gvCREditDetails.PageCount == 0)
                {
                    for (int i = 0; i <= gvAddCRDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvAddCRDetails.Rows[Convert.ToInt32(i)];
                        DropDownList ddl1 = (DropDownList)grdfs.FindControl("drpequipcat");
                        DropDownList ddl2 = (DropDownList)grdfs.FindControl("drpequiplst");
                        Label box1 = (Label)grdfs.FindControl("lblser");
                        TextBox box2 = (TextBox)grdfs.FindControl("txtqty");
                        TextBox box3 = (TextBox)grdfs.FindControl("txtppq");
                        TextBox box4 = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox box5 = (TextBox)grdfs.FindControl("txtreason");

                        if (box4.Text == "")
                        {
                            CalTotalPrice(box4);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= gvCREditDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvCREditDetails.Rows[Convert.ToInt32(i)];
                        Label ddl1 = (Label)grdfs.FindControl("lblEquipCategory");
                        Label ddl2 = (Label)grdfs.FindControl("lblEquipList");
                        Label box1 = (Label)grdfs.FindControl("lblSerialNo");
                        TextBox box2 = (TextBox)grdfs.FindControl("txtqty");
                        TextBox box3 = (TextBox)grdfs.FindControl("txtppq");
                        TextBox box4 = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox box5 = (TextBox)grdfs.FindControl("txtreason");

                        if (box4.Text == "")
                        {
                            CalTotalPriceUpdate(box4);
                        }
                    }

                    if (gvCREditDetails.FooterRow.Visible == true)
                    {

                        Control control = null;
                        if (gvCREditDetails.FooterRow != null)
                        {
                            control = gvCREditDetails.FooterRow;
                        }
                        else
                        {
                            control = gvCREditDetails.Controls[0].Controls[0];
                        }

                        DropDownList FootddlEquip = control.FindControl("FootddlEquip") as DropDownList;
                        DropDownList Footddlequiplst = control.FindControl("Footddlequiplst") as DropDownList;
                        Label Footser = control.FindControl("Footser") as Label;
                        TextBox Foottxtqty = control.FindControl("Foottxtqty") as TextBox;
                        TextBox Foottxtppq = control.FindControl("Foottxtppq") as TextBox;
                        TextBox Foottxttotprice = control.FindControl("Foottxttotprice") as TextBox;
                        TextBox Foottxtreason = control.FindControl("Foottxtreason") as TextBox;

                        if (Foottxttotprice.Text == "")
                        {
                            CalFootRow(Foottxttotprice);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
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
                TextBox txtPricePerUnit = (TextBox)gvAddCRDetails.Rows[index].FindControl("txtppq");
                TextBox txtOrderQuantity = (TextBox)gvAddCRDetails.Rows[index].FindControl("txtqty");
                TextBox txtTotalPrice = (TextBox)gvAddCRDetails.Rows[index].FindControl("txttotprice");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void CalTotalPriceUpdate(TextBox CalupdateTotal)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)CalupdateTotal.NamingContainer;
                int index = Convert.ToInt16(gvr.RowIndex);
                Int32 OrderQuantity = 0;
                decimal PricePerUnit = 0;
                TextBox txtPricePerUnit = (TextBox)gvCREditDetails.Rows[index].FindControl("txtppq");
                TextBox txtOrderQuantity = (TextBox)gvCREditDetails.Rows[index].FindControl("txtqty");
                TextBox txtTotalPrice = (TextBox)gvCREditDetails.Rows[index].FindControl("txttotprice");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }


        public void CalFootRow(TextBox CalfootTotal)
        {
            try
            {

                GridViewRow gvr = (GridViewRow)CalfootTotal.NamingContainer;
                //int index = Convert.ToInt16(gvr.RowIndex);
                Int32 OrderQuantity = 0;
                decimal PricePerUnit = 0;
                TextBox txtPricePerUnit = (TextBox)gvCREditDetails.FooterRow.FindControl("Foottxtppq");
                TextBox txtOrderQuantity = (TextBox)gvCREditDetails.FooterRow.FindControl("Foottxtqty");
                TextBox txtTotalPrice = (TextBox)gvCREditDetails.FooterRow.FindControl("Foottxttotprice");

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            string sCapitalItemsID = string.Empty;
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            sCapitalItemsID = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
            List<BindCapitalItemRequestReport> llstreview = lclsservice.BindCapitalItemRequestReport(sCapitalItemsID, null, defaultPage.UserId, defaultPage.UserId).ToList();
            rvCapitalItemsreport.ProcessingMode = ProcessingMode.Local;
            rvCapitalItemsreport.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalItemRequestReview.rdlc");
            string s = sCapitalItemsID;
            string q = "1";
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("CapitalItemMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "testvalue");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));
            this.rvCapitalItemsreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("CapitalItemRequestReviewDS", llstreview);
            rvCapitalItemsreport.LocalReport.DataSources.Clear();
            rvCapitalItemsreport.LocalReport.DataSources.Add(datasource);
            rvCapitalItemsreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvCapitalItemsreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

            //Random rnd = new Random();
            //int month = rnd.Next(1, 13); // creates a number between 1 and 12
            //int dice = rnd.Next(1, 7); // creates a number between 1 and 6
            //int card = rnd.Next(9); // creates a number between 0 and 51
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "MajorItemRequest" + guid + ".pdf";
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
            ShowPDFFile(path);
        }

        protected void grdCRsearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = string.Empty;
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                if (e.Row.Cells.Count > 0)
                {
                    status = e.Row.Cells[11].Text;
                }
                Image imgbtnEdit = (Image)e.Row.FindControl("imgbtnEdit");
                //if (lblRemarks.Text.Length > 150)
                //{
                //    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                //    imgreadmore.Visible = true;
                //}
                //else
                //{
                //    imgreadmore.Visible = false;
                //}

                //if (lblaudit.Text.Length > 150)
                //{
                //    lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                //    imgreadmore1.Visible = true;
                //}
                //else
                //{
                //    imgreadmore1.Visible = false;
                //}

                if (defaultPage.RoleID == 1)
                {
                    if (status == "Pending Approval" || status == "Hold" || status == "Pending Order")
                    {
                        imgbtnEdit.Visible = true;
                    }
                    else
                    {
                        imgbtnEdit.Visible = false;
                    }
                }
                if (defaultPage.RoleID != 1)
                {
                    if (status == "Pending Approval")
                    {
                        imgbtnEdit.Visible = true;
                    }
                    else
                    {
                        imgbtnEdit.Visible = false;
                    }
                }
            }
        }

        protected void btnnewrowup_Click(object sender, EventArgs e)
        {
            if (gvCREditDetails.PageCount != 0)
            {
                gvCREditDetails.FooterRow.Visible = true;
                FootBindEquipSubcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            }
            else
            {
                divAddMachine.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "none");
                SetInitialRow();
            }
        }

        protected void FootddlEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSpin2 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
                //int RowIndex = gridrow.RowIndex;
                DropDownList Footdrpequipcat = (DropDownList)gvCREditDetails.FooterRow.FindControl("FootddlEquip");
                DropDownList Foordrpequiplst = (DropDownList)gvCREditDetails.FooterRow.FindControl("Footddlequiplst");

                if (rdorequesttype.SelectedValue == "1")
                {
                    FootGetEquipementList(Convert.ToInt64(Footdrpequipcat.SelectedValue), "Add");
                    Foordrpequiplst.Enabled = true;
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    Foordrpequiplst.Items.Insert(0, lst);
                    Foordrpequiplst.Enabled = false;
                }
                RebindGrid();
                Footdrpequipcat.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSaveRow_Click(object sender, ImageClickEventArgs e)
        {
            FooterRowSave();
        }

        protected void btnRemoveRow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gvCREditDetails.FooterRow.Visible = false;
                Control control = null;
                if (gvCREditDetails.FooterRow != null)
                {
                    control = gvCREditDetails.FooterRow;
                }
                else
                {
                    control = gvCREditDetails.Controls[0].Controls[0];
                }
                DropDownList FootddlEquip = control.FindControl("FootddlEquip") as DropDownList;
                DropDownList Footddlequiplst = control.FindControl("Footddlequiplst") as DropDownList;
                Label Footser = control.FindControl("Footser") as Label;
                TextBox Foottxtqty = control.FindControl("Foottxtqty") as TextBox;
                TextBox Foottxtppq = control.FindControl("Foottxtppq") as TextBox;
                TextBox Foottxttotprice = control.FindControl("Foottxttotprice") as TextBox;
                TextBox Foottxtreason = control.FindControl("Foottxtreason") as TextBox;
                FootddlEquip.SelectedIndex = 0;
                Footddlequiplst.SelectedIndex = 0;
                Footser.Text = "";
                Foottxtqty.Text = "";
                Foottxtppq.Text = "";
                Foottxttotprice.Text = "";
                Foottxtreason.Text = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        public void FooterRowSave()
        {
            try
            {
                Int64 footEquipcat = 0;
                Int64 footEquiplist = 0;
                Int64 footfaclityid = 0;
                bool isvequp = false;
                string equipment = string.Empty;

                Control control = null;
                if (gvCREditDetails.FooterRow != null)
                {
                    control = gvCREditDetails.FooterRow;
                }
                else
                {
                    control = gvCREditDetails.Controls[0].Controls[0];
                }

                string b = string.Empty;
                string ErrorList = string.Empty;
                BALCapital llstCRMaster = new BALCapital();
                llstCRMaster.CapitalItemMasterID = Convert.ToInt64(HddMasterID.Value);
                llstCRMaster.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                llstCRMaster.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                llstCRMaster.CreatedBy = defaultPage.UserId;


                DropDownList FootddlEquip = control.FindControl("FootddlEquip") as DropDownList;
                DropDownList Footddlequiplst = control.FindControl("Footddlequiplst") as DropDownList;
                Label Footser = control.FindControl("Footser") as Label;
                TextBox Foottxtqty = control.FindControl("Foottxtqty") as TextBox;
                TextBox Foottxtppq = control.FindControl("Foottxtppq") as TextBox;
                TextBox Foottxttotprice = control.FindControl("Foottxttotprice") as TextBox;
                TextBox Foottxtreason = control.FindControl("Foottxtreason") as TextBox;

                if (rdorequesttype.SelectedValue == "0")
                {
                    if (FootddlEquip.SelectedValue == "0" || Foottxtqty.Text == "" || Foottxtppq.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }
                else
                {
                    if (FootddlEquip.SelectedValue == "0" || Footddlequiplst.SelectedValue == "0" || Foottxtqty.Text == "" || Foottxtppq.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }

                }

                if (ErrorList == "")
                {
                    llstCRMaster.EquipmentSubCategoryID = Convert.ToInt64(FootddlEquip.SelectedValue);
                    llstCRMaster.EquipementListID = Convert.ToInt64(Footddlequiplst.SelectedValue);
                    llstCRMaster.SerialNo = Footser.Text;
                    llstCRMaster.OrderQuantity = Convert.ToInt32(Foottxtqty.Text);
                    llstCRMaster.PricePerUnit = Convert.ToDecimal(Foottxtppq.Text);
                    llstCRMaster.TotalPrice = Convert.ToDecimal(Foottxttotprice.Text);
                    llstCRMaster.Reason = Foottxtreason.Text;

                    footEquipcat = llstCRMaster.EquipmentSubCategoryID;
                    footEquiplist = llstCRMaster.EquipementListID;
                    footfaclityid = llstCRMaster.FacilityID;
                    List<ValidCapitalEquipment> lstMaster = lclsservice.ValidCapitalEquipment(footEquipcat, footEquiplist, footfaclityid).ToList();
                    if (lstMaster[0].Edit == 1)
                    {
                        isvequp = true;
                    }
                    else
                    {
                        equipment += FootddlEquip.SelectedItem.Text + ",";
                        isvequp = false;
                    }
                    if(isvequp == true)
                    {
                        b = lclsservice.InsertCapitalItemDetails(llstCRMaster);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemValidMessage.Replace("<<MajorItem>>", "" +equipment), true);
                    }

                    if (b == "Saved Successfully")
                    {
                        BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalItemReqMessage.Replace("<<MajorItem>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message), true);

            }
        }

        protected void Footddlequiplst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList ddlSpin3 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin3.NamingContainer;
                //int RowIndex = gridrow.RowIndex;
                DropDownList FootddlEquip = (DropDownList)gvCREditDetails.FooterRow.FindControl("FootddlEquip");
                DropDownList Footddlequiplst = (DropDownList)gvCREditDetails.FooterRow.FindControl("Footddlequiplst");
                Label Footser = (Label)gvCREditDetails.FooterRow.FindControl("Footser");
                if (rdorequesttype.SelectedValue == "1")
                {
                    List<GetSerialNo> lstserial = lclsservice.GetSerialNo(Convert.ToInt64(FootddlEquip.SelectedValue), Convert.ToInt64(Footddlequiplst.SelectedValue)).ToList();
                    if (lstserial.Count > 0)
                    {
                        Footser.Text = lstserial[0].SerialNo;
                    }
                    else
                    {
                        Footser.Text = "";
                    }
                }
                else
                {
                    Footser.Text = "";
                }

                RebindGrid();
                Footddlequiplst.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }

        }

        protected void GvTempEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                string status = e.Row.Cells[11].Text;
                Image TempbtnEdit = (Image)e.Row.FindControl("TempbtnEdit");
                //if (lblRemarks.Text.Length > 150)
                //{
                //    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                //    imgreadmore.Visible = true;
                //}
                //else
                //{
                //    imgreadmore.Visible = false;
                //}
                //if (lblaudit.Text.Length > 150)
                //{
                //    lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                //    imgreadmore1.Visible = true;
                //}
                //else
                //{
                //    imgreadmore1.Visible = false;
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

        //Multi Select for dropdowns
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
                foreach (ListItem lst1 in drpcorsearch.Items)
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
                        foreach (ListItem lst1 in drpcorsearch.Items)
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
                divCapitalItemrequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divCapitalItemrequest.Attributes["class"] = "mypanel-body";
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
                foreach (ListItem lst1 in drpfacilitysearch.Items)
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
                        foreach (ListItem lst1 in drpfacilitysearch.Items)
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
                divCapitalItemrequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divCapitalItemrequest.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divCapitalItemrequest.Attributes["class"] = "Upopacity";
                int i = 0;
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    GrdMultiCorp.DataSource = lstcrop;
                    GrdMultiCorp.DataBind();
                    foreach (ListItem lst1 in drpcorsearch.Items)
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

                                if (i == drpcorsearch.Items.Count)
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
                    drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    GrdMultiCorp.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    GrdMultiCorp.DataBind();
                    foreach (ListItem lst1 in drpcorsearch.Items)
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

                                if (i == drpcorsearch.Items.Count)
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
            foreach (ListItem lst in drpcorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
            foreach (ListItem lst in drpfacilitysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (ListItem lst in drpvendorsearch.Items)
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
                if (drpcorsearch.SelectedValue != "")
                {
                    foreach (ListItem lst in drpcorsearch.Items)
                    {
                        if (lst.Selected && drpcorsearch.SelectedValue != "All")
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
                    divCapitalItemrequest.Attributes["class"] = "Upopacity";
                    foreach (ListItem lst1 in drpfacilitysearch.Items)
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

                                if (j == drpfacilitysearch.Items.Count)
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
            foreach (ListItem lst in drpfacilitysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (ListItem lst in drpvendorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }
    }
}

