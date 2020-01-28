#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using System.Text;
using Inventory.Class;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Drawing;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      : FacilitySuppliesMap
'' Type      :   C# File
'' Description  :<<To add,update the Facility Medical Supply Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Vivekanand.S		                  New
'' 	08/16/2017		   V1.0				   Vivekanand.S		                  Validation Check in Clear function
'' 	08/19/2017		   V1.0				   Vivekanand.S		                  Multi Comment Total Census,Duplicate DropDown and Grid bind
 *  10/24/2017         V2.0                Sairam.P                           Validate mandatory fields and Check the active and inactive records in dropdown
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class FacilitySuppliesMap : System.Web.UI.Page
    {
        // Class declaration for User Detail with permission
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsService = new InventoryServiceClient();
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.WarningFacilitySuppliesMapMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnprint);
                // If Not PostBack
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    BindCorporate();
                    if (defaultPage != null)
                    {
                        if (defaultPage.FacilitySuppliesMap_Edit == false && defaultPage.FacilitySuppliesMap_View == true)
                        {
                            btnSave.Visible = false;

                        }
                        if (defaultPage.FacilitySuppliesMap_Edit == false && defaultPage.FacilitySuppliesMap_View == false)
                        {
                            updmain.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
         
        }
        #endregion

        /// <summary>
        /// Required field Valudation function.
        /// </summary>
        #region Required field Validator
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;

            //Search Control Validation
            //ReqfielddrpcopSearch.ErrorMessage = req;
            //ReqfielddrpfacilitySearch.ErrorMessage = req;
            //ReqfielddrpvendorSearch.ErrorMessage = req;
            //ReqfielddrpItemcategorySearch.ErrorMessage = req;


            //Add Control Validation
            Reqfielddrpcorp.ErrorMessage = req;
            Reqfielddrpfacility.ErrorMessage = req;
            Reqfielddrpvendor.ErrorMessage = req;
            ReqfielddrpItemcategory.ErrorMessage = req;

            //reqfieldtxtFactor.ErrorMessage = req;
            RequiredrbCensus.ErrorMessage = req;

            //Requireddrporderdate.ErrorMessage = req;
        }

        #endregion


        /// <summary>
        /// Bind the Facility details from Facility marter table to dropdown control 
        /// </summary>
        #region Bind Facility Values
        private void BindFacility()
        {
            try
            {
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Facility--";
                if (HddCuurentStatus.Value == "Search")
                {
                    drpfacilitySearch.DataSource = lclsService.GetCorporateFacility(Convert.ToInt64(drpcopSearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    drpfacilitySearch.DataTextField = "FacilityDescription";
                    drpfacilitySearch.DataValueField = "FacilityID";
                    drpfacilitySearch.DataBind();
                    drpfacilitySearch.Items.Insert(0, lst);
                    drpfacilitySearch.SelectedIndex = 0;
                    grdFacilitySupplySearch.DataSource = null;
                    grdFacilitySupplySearch.DataBind();
                }
                else
                {
                    drpfacility.DataSource = lclsService.GetCorporateFacility(Convert.ToInt64(drpcor.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    drpfacility.DataTextField = "FacilityDescription";
                    drpfacility.DataValueField = "FacilityID";
                    drpfacility.DataBind();
                    drpfacility.Items.Insert(0, lst);
                    drpfacility.SelectedIndex = 0;
                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();
                }
                //txtFactor.Text = "";
                //rbCensus.ClearSelection();
                //divvendor.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
            
        }
        #endregion


        /// <summary>
        /// Bind the Corporate details to dropdown control 
        /// </summary>
        #region Bind Corporate Values
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstfacility = new List<BALUser>();
                lstfacility = lclsService.GetCorporateMaster().ToList();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Corporate--";
                if (HddCuurentStatus.Value == "Search")
                {
                    drpcopSearch.DataSource = lstfacility;
                    drpcopSearch.DataTextField = "CorporateName";
                    drpcopSearch.DataValueField = "CorporateID";
                    drpcopSearch.DataBind();
                    drpcopSearch.Items.Insert(0, lst);
                    drpcopSearch.SelectedIndex = 0;
                    grdFacilitySupplySearch.DataSource = null;
                    grdFacilitySupplySearch.DataBind();
                }
                else
                {
                    drpcor.DataSource = lstfacility;
                    drpcor.DataTextField = "CorporateName";
                    drpcor.DataValueField = "CorporateID";
                    drpcor.DataBind();
                    drpcor.Items.Insert(0, lst);
                    drpcor.SelectedIndex = 0;
                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();
                }
                //txtFactor.Text = "";
                //rbCensus.ClearSelection();
                //divvendor.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }
        #endregion

        #region Multi Select Dropdown

        protected void lnkMultiVendor_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (drpfacilitySearch.SelectedValue != "")
                {
                    List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                    lstvendordetails = lclsService.GetFacilityVendorAccount(drpfacilitySearch.SelectedItem.Text).Where(A => (A.RegularSupplies == true) && (A.IsActive == true)).ToList();

                    GrdMultiVendor.DataSource = lstvendordetails;
                    GrdMultiVendor.DataBind();

                    foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                    {
                        if (lst1.Selected == true)
                        {

                            foreach (GridViewRow row in GrdMultiVendor.Rows)
                            {
                                Label lblVendorID = (Label)row.FindControl("lblVendorID");
                                CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");
                                CheckBox ChkAllVendor = GrdMultiVendor.HeaderRow.FindControl("ChkAllVendor") as CheckBox;

                                if (lst1.Value == lblVendorID.Text)
                                {
                                    chkmultiVendor.Checked = true;
                                    i++;
                                }

                                if (i == drpvendorcodeSearch.Items.Count)
                                {
                                    ChkAllVendor.Checked = true;
                                }

                            }
                        }
                    }
                    DivMultiVendor.Style.Add("display", "block");
                    UploadOpacity.Attributes["class"] = "Upopacity";
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        protected void lnkClearVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpfacilitySearch.SelectedValue != "")
                {
                    BindVendor();
                }
                HddListVendorID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }

        }

        protected void lnkClearAllVendor_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst in drpvendorcodeSearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                }
                foreach (ListItem lst in drpItemcategorySearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                }
                HddListVendorID.Value = "";
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        protected void ChkAllVendor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ChkAllVendor = (CheckBox)GrdMultiVendor.HeaderRow.FindControl("ChkAllVendor");

                foreach (GridViewRow row in GrdMultiVendor.Rows)

                {

                    CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");

                    if (ChkAllVendor.Checked == true)

                    {
                        chkmultiVendor.Checked = true;
                    }
                    else

                    {
                        chkmultiVendor.Checked = false;
                    }

                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        protected void btnMultiVendorselect_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                {
                    lst1.Attributes.Add("class", "");
                    lst1.Selected = false;
                }
                foreach (GridViewRow row in GrdMultiVendor.Rows)
                {
                    CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");
                    Label lblVendorID = (Label)row.FindControl("lblVendorID");

                    if (chkmultiVendor.Checked == true)
                    {
                        foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                        {
                            if (lst1.Value == lblVendorID.Text)
                            {
                                lst1.Attributes.Add("class", "selected");
                                lst1.Selected = true;
                            }
                        }
                    }
                }
                BindCategory();
                DivMultiVendor.Style.Add("display", "none");
                UploadOpacity.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }

        protected void btnMultiVendorClose_Click(object sender, EventArgs e)
        {
            try
            {
                DivMultiVendor.Style.Add("display", "none");
                UploadOpacity.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           
        }
        #endregion


        /// <summary>
        /// Vendor On Selected changes event 
        /// </summary>
        #region Vendor Selected Change Event
        protected void drpvendorcodeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                foreach (ListItem lst in drpvendorcodeSearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        i++;
                    }
                }
                if (i == 1)
                {
                    BindCategory();
                    foreach (ListItem lst in drpvendorcodeSearch.Items)
                    {
                        if (lst.Selected == true)
                        {
                            HddListVendorID.Value = lst.Value;
                        }
                    }
                }
                else if (i == 2)
                {
                    foreach (ListItem lst in drpvendorcodeSearch.Items)
                    {
                        lst.Attributes.Add("class", "");
                        lst.Selected = false;
                        if (HddListVendorID.Value == lst.Value)
                        {
                            lst.Attributes.Add("class", "selected");
                            lst.Selected = true;
                        }

                    }
                }
                else
                {
                    foreach (ListItem lst in drpvendorcodeSearch.Items)
                    {
                        lst.Attributes.Add("class", "");
                        lst.Selected = false;
                        HddListVendorID.Value = "";
                    }
                    BindCategory();
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }
        #endregion


        /// <summary>
        /// Bind the Vendor details from vendor master table to dropdown control 
        /// </summary>
        #region Bind Vendor Values
        public void BindVendor()
        {
            try
            {
                List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                string SearchText = string.Empty;

                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Vendor--";

                if (HddCuurentStatus.Value == "Search")
                {
                    lstvendordetails = lclsService.GetFacilityVendorAccount(drpfacilitySearch.SelectedItem.Text).Where(a => a.IsActive == true).ToList();
                    drpvendorcodeSearch.DataSource = lstvendordetails;
                    drpvendorcodeSearch.DataTextField = "VendorDescription";
                    drpvendorcodeSearch.DataValueField = "VendorID";
                    drpvendorcodeSearch.DataBind();
                    //drpvendorSearch.Items.Insert(0, lst);
                    //drpvendorSearch.SelectedIndex = 0;
                    grdFacilitySupplySearch.DataSource = null;
                    grdFacilitySupplySearch.DataBind();

                    foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                    {
                        lst1.Attributes.Add("class", "selected");
                        lst1.Selected = true;
                    }

                }
                else
                {
                    lstvendordetails = lclsService.GetFacilityVendorAccount(drpfacility.SelectedItem.Text).Where(a => a.IsActive == true).ToList();
                    drpvendor.DataSource = lstvendordetails;
                    drpvendor.DataTextField = "VendorDescription";
                    drpvendor.DataValueField = "VendorID";
                    drpvendor.DataBind();
                    drpvendor.Items.Insert(0, lst);
                    drpvendor.SelectedIndex = 0;
                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();
                }
                //txtFactor.Text = "";
                //rbCensus.ClearSelection();
                //divvendor.Style.Add("display", "none");
                BindCategory();
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Item Category from master table to dropdown control
        /// </summary>
        #region Bind Item Category Values
        public void BindCategory()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Category--";
                if (HddCuurentStatus.Value == "Search")
                {
                    foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                    {
                        if (lst1.Selected && drpvendorcodeSearch.SelectedValue != "All")
                        {
                            SB.Append(lst1.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    List<GetCategoryByListVendorID> lstcategory = lclsservice.GetCategoryByListVendorID(FinalString).ToList();
                    drpItemcategorySearch.DataSource = lstcategory.Select(a => new { a.CategoryID, a.CategoryName }).Distinct();
                    drpItemcategorySearch.DataValueField = "CategoryID";
                    drpItemcategorySearch.DataTextField = "CategoryName";
                    drpItemcategorySearch.DataBind();
                    //drpItemcategorySearch.Items.Insert(0, lst);
                    grdFacilitySupplySearch.DataSource = null;
                    grdFacilitySupplySearch.DataBind();

                    foreach (ListItem lst1 in drpItemcategorySearch.Items)
                    {
                        lst1.Attributes.Add("class", "selected");
                        lst1.Selected = true;
                    }

                }
                else
                {
                    List<GetItemMap> lstcategory = lclsservice.GetItemMap().Where(a => a.VendorName == drpvendor.SelectedItem.Text).Where(a => a.IsActive == true).ToList();
                    drpItemcategory.DataSource = lstcategory.Select(a => new { a.CategoryID, a.CategoryName }).Distinct();
                    drpItemcategory.DataValueField = "CategoryID";
                    drpItemcategory.DataTextField = "CategoryName";
                    drpItemcategory.DataBind();
                    drpItemcategory.Items.Insert(0, lst);
                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();


                }

                //divvendor.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        //public void BindParlevel()
        //{
        //    drpParlevel.Items.Add();
          
            
        //}
        #endregion

        /// <summary>
        /// Bind the Order date with respect to given search (Facility and Vendor Drop Down) 
        /// </summary>
        #region Bind Order date Values
        public void BindOrderdate()
        {
            //try
            //{
            //    BALFacilitySupply objbalFacilitySupply = new BALFacilitySupply();
            //    InventoryServiceClient lclsservice = new InventoryServiceClient();
            //    objbalFacilitySupply.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
            //    objbalFacilitySupply.VendorID = Convert.ToInt64(drpvendor.SelectedValue);
            //    List<GetFacilitySupply> lstFacilitySupply = lclsService.GetFacilitySupply(objbalFacilitySupply).ToList();
            //    drporderdate.DataTextFormatString = "{0:MM/dd/yyyy}";
            //    drporderdate.DataSource = lstFacilitySupply.Select(a => new { a.VendorOrderDate }).Distinct();
            //    //drporderdate.DataValueField = "CategoryID";
            //    drporderdate.DataTextField = "VendorOrderDate";
            //    drporderdate.DataBind();

            //    //ListItem lst = new ListItem();
            //    //lst.Value = "0";
            //    //lst.Text = "New";
            //    drporderdate.Items.Insert(0, "---Select---");
            //    drporderdate.Items.Insert(1, "New");

            //}
            //catch (Exception ex)
            //{

            //}
        }
        #endregion

        /// <summary>
        /// Save and Update all the details to the database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Add event

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                HddCuurentStatus.Value = "Add";
                DivAdd.Style.Add("display", "block");
                DivSearch.Style.Add("display", "none");
                btnAdd.Visible = false;
                btnSave.Visible = true;
                btnsearch.Visible = false;
                btnprint.Visible = false;
                btnSave.Enabled = true;
                rbCensus.ClearSelection();
                txtotherCensus.Text = "";
                txtotherCensus.Enabled = false;
                BindCorporate();
                drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                BindFacility();
                drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                BindVendor();
                BindCategory();
                drpcor.Enabled = true;
                drpfacility.Enabled = true;
                drpvendor.Enabled = true;
                drpItemcategory.Enabled = true;
                rbCensus.Enabled = true;
                hdnEdit.Value = "0";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           
        }

        #endregion

        /// <summary>
        /// Calculation and Review Popup Screen open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Review event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                string valcheck = string.Empty;
                valcheck = HdnLoad.Value;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "loadcensushide();", true);
                if ((Hdncheckcensus.Value == rbCensus.SelectedValue) && (valcheck == "0"))
                {
                    review();
                }
                else if(valcheck == "1")
                {
                    review();
                }
                else
                {
                    if(rbCensus.SelectedValue == "4")
                    {
                        txtotherCensus.Enabled = true;
                    }
                    else
                    {
                        txtotherCensus.Enabled = false;
                    }
                    string msg = Constant.FacilitySuppliesVendorloadMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesVendorloadMessage, true);
                }
                
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }
        #endregion


        public void review()
        {
            try
            {
                string ErrorList = string.Empty;
                BALFacilitySupply objbalFacilitySupply = new BALFacilitySupply();
                List<GetFacilitySupplyGird> llstFacilitySupply = new List<GetFacilitySupplyGird>();
                Int64 QyPerPack = 0;

                if (rbCensus.SelectedValue == "4")
                {
                    txtotherCensus.Enabled = true;
                }
                else
                {
                    txtotherCensus.Enabled = false;
                }
                lblreviewcorporate.Text = drpcor.SelectedItem.Text;
                lblreviewfacility.Text = drpfacility.SelectedItem.Text;
                lblvendor.Text = drpvendor.SelectedItem.Text;
                lblItemCat.Text = drpItemcategory.SelectedItem.Text;

                foreach (GridViewRow grd in grdFacilitySupply.Rows)
                {
                    TextBox txtCen = (TextBox)grd.FindControl("txtCensus");
                    if (txtCen.Text == "")
                    {
                        ErrorList = "censusError";
                    }

                }

                if (btnSave.Text == "Review")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                    foreach (GridViewRow grdfs in grdFacilitySupply.Rows)
                    {
                        objbalFacilitySupply.FacilitySupplyID = Convert.ToInt64(grdfs.Cells[0].Text);
                        objbalFacilitySupply.CorporateID = Convert.ToInt64(grdfs.Cells[1].Text);
                        objbalFacilitySupply.FacilityID = Convert.ToInt64(grdfs.Cells[2].Text);
                        objbalFacilitySupply.VendorID = Convert.ToInt64(grdfs.Cells[3].Text);
                        objbalFacilitySupply.ItemCategory = Convert.ToInt64(grdfs.Cells[4].Text);

                        string Vendorcode = HttpUtility.HtmlDecode(grdfs.Cells[6].Text);
                        string ItemGroup = HttpUtility.HtmlDecode(grdfs.Cells[7].Text);
                        string VendorItemCode = HttpUtility.HtmlDecode(grdfs.Cells[8].Text);
                        objbalFacilitySupply.ItemID = Convert.ToInt64(grdfs.Cells[9].Text);
                        string ItemDescription = HttpUtility.HtmlDecode(grdfs.Cells[10].Text);
                        decimal UnitePrice = Convert.ToDecimal(grdfs.Cells[11].Text);
                        string UOM = grdfs.Cells[12].Text;
                        Int64 QtyPer = Convert.ToInt64(grdfs.Cells[13].Text);

                        decimal ParLevelDecimal = 0;
                        var RowDetails = new StringBuilder();
                        List<BindVendorOrderDue> lstOrdertype = lclsService.BindVendorOrderDue(objbalFacilitySupply).ToList();

                        TextBox txtCen = (TextBox)grdfs.FindControl("txtCensus");
                        TextBox txtFactor = (TextBox)grdfs.FindControl("txtFactor");

                        if (txtFactor.Text != "")
                            objbalFacilitySupply.Factor = Convert.ToDecimal(txtFactor.Text);
                        else
                            objbalFacilitySupply.Factor = Convert.ToDecimal(1);


                        if (txtCen.Text != "")
                            objbalFacilitySupply.Census = Convert.ToInt64(txtCen.Text);
                        //objbalFacilitySupply.VendorOrderDate = Convert.ToDateTime(grdfs.Cells[12].Text);
                        objbalFacilitySupply.VendorOrderDate = null;

                        List<BindItem> llstItemperpack = lclsService.binditem("").Where(b => b.ItemID == objbalFacilitySupply.ItemID).ToList();

                        int WeekSchedule = 0;
                        if (lstOrdertype.Count > 0)
                        {
                            if (lstOrdertype[0].OrderType == "Weekly")
                            {
                                WeekSchedule = 1;
                            }
                            else if (lstOrdertype[0].OrderType == "Bi-Weekly")
                            {
                                WeekSchedule = 2;
                            }
                            else if (lstOrdertype[0].OrderType == "Monthly")
                            {
                                WeekSchedule = 4;
                            }
                            else if (lstOrdertype[0].OrderType == "Ad-hoc")
                            {
                                WeekSchedule = 1;
                            }
                            if (llstItemperpack.Count > 0)
                                QyPerPack = Convert.ToInt64(llstItemperpack[0].QtyPack);
                            if (QyPerPack <= 0) QyPerPack = 1;
                            BALFacility lclfacility = new BALFacility();
                            lclfacility.SearchText = "";
                            lclfacility.Active = "";
                            lclfacility.LogginBy = defaultPage.UserId;
                            lclfacility.Filter = "";

                            List<BindFacility> lstfacility = lclsService.BindFacility(lclfacility).Where(c => c.FacilityID == objbalFacilitySupply.FacilityID).ToList();
                            Int64 Txtweek = 0;

                            Txtweek = Convert.ToInt64(lstfacility[0].TxWeek);
                            if (Txtweek == 0)
                            {
                                ParLevelDecimal = objbalFacilitySupply.Factor * objbalFacilitySupply.Census * WeekSchedule / QyPerPack;
                            }
                            else
                            {
                                ParLevelDecimal = objbalFacilitySupply.Factor * objbalFacilitySupply.Census * WeekSchedule * Txtweek / QyPerPack;
                            }
                            mpeFacilSupplyReview.Show();
                            objbalFacilitySupply.Parlevel = Convert.ToInt64(Math.Ceiling(ParLevelDecimal));
                            //if (objbalFacilitySupply.Parlevel > 0)
                            //{
                            llstFacilitySupply.Add(new GetFacilitySupplyGird
                            {
                                FacilitySupplyID = objbalFacilitySupply.FacilitySupplyID,
                                CorporateID = objbalFacilitySupply.CorporateID,
                                FacilityID = objbalFacilitySupply.FacilityID,
                                VendorID = objbalFacilitySupply.VendorID,
                                CategoryID = objbalFacilitySupply.ItemCategory,
                                VendorShortName = Vendorcode,
                                CategoryName = ItemGroup,
                                VendorItemCode = VendorItemCode,
                                ItemID = objbalFacilitySupply.ItemID,
                                ItemDescription = ItemDescription,
                                QtyPack = QtyPer,
                                UnitPrice = UnitePrice,
                                UOM = UOM,
                                Factor = objbalFacilitySupply.Factor,
                                Census = objbalFacilitySupply.Census,
                                Parlevel = objbalFacilitySupply.Parlevel
                            });
                            //}
                        }
                        else
                        {
                            EventLogger log = new EventLogger(config);
                            string msg = Constant.FacilitySuppliesVendorOrderMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
                            log.LogWarning(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesVendorOrderMessage, true);

                        }
                    }

                    GrdFacilSupplReview.DataSource = llstFacilitySupply;
                    GrdFacilSupplReview.DataBind();

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        /// <summary>
        /// Row Databound to check factor above 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Review Grid Row databound
        protected void GrdFacilSupplReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    decimal Factor = Convert.ToDecimal(e.Row.Cells[13].Text);
                    Int64 Census = Convert.ToInt64(e.Row.Cells[14].Text);
                    if (Factor > 1)
                    {
                        //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#0097e1'");
                        //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#b4c6e7'");
                        e.Row.BackColor = Color.FromName("#b4c6e7");
                    }
                    if (Census == 0)
                    {
                        e.Row.Attributes.CssStyle.Add("display", "none");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }
        #endregion


        /// <summary>
        /// Save and Update all the details to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Save Event
        protected void btnSaveReview_Click(object sender, EventArgs e)
        {          
            Save();

        }
        #endregion

        /// <summary>
        /// Save and Update all the details in the review Popup screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Save Function
        public void Save()
        {
            try
            {
                BALFacilitySupply objbalFacilitySupply = new BALFacilitySupply();
                string a = string.Empty;
                string ErrorList = string.Empty;
                Int64 QyPerPack = 0;


                foreach (GridViewRow grdfs in GrdFacilSupplReview.Rows)
                {
                    objbalFacilitySupply.FacilitySupplyID = Convert.ToInt64(grdfs.Cells[0].Text);
                    objbalFacilitySupply.CorporateID = Convert.ToInt64(grdfs.Cells[1].Text);
                    objbalFacilitySupply.FacilityID = Convert.ToInt64(grdfs.Cells[2].Text);
                    objbalFacilitySupply.VendorID = Convert.ToInt64(grdfs.Cells[3].Text);
                    objbalFacilitySupply.ItemCategory = Convert.ToInt64(grdfs.Cells[4].Text);
                    objbalFacilitySupply.ItemID = Convert.ToInt64(grdfs.Cells[8].Text);

                    if (Convert.ToInt64(rbCensus.SelectedValue) == 1)
                    {
                        objbalFacilitySupply.IsEmp = true;
                        objbalFacilitySupply.IsPatient = false;
                        objbalFacilitySupply.Isboth = false;
                        objbalFacilitySupply.Isothers = false;
                    }
                    else if (Convert.ToInt64(rbCensus.SelectedValue) == 2)
                    {
                        objbalFacilitySupply.IsEmp = false;
                        objbalFacilitySupply.IsPatient = true;
                        objbalFacilitySupply.Isboth = false;
                        objbalFacilitySupply.Isothers = false;
                    }
                    else if (Convert.ToInt64(rbCensus.SelectedValue) == 3)
                    {
                        objbalFacilitySupply.IsEmp = false;
                        objbalFacilitySupply.IsPatient = false;
                        objbalFacilitySupply.Isboth = true;
                        objbalFacilitySupply.Isothers = false;
                    }
                    else if (Convert.ToInt64(rbCensus.SelectedValue) == 4)
                    {
                        objbalFacilitySupply.IsEmp = false;
                        objbalFacilitySupply.IsPatient = false;
                        objbalFacilitySupply.Isboth = false;
                        objbalFacilitySupply.Isothers = true;
                    }

                    Int64 txtCen = Convert.ToInt64(grdfs.Cells[14].Text);
                    decimal txtFactor = Convert.ToDecimal(grdfs.Cells[13].Text);


                    objbalFacilitySupply.Factor = Convert.ToDecimal(txtFactor);
                    objbalFacilitySupply.Census = Convert.ToInt64(txtCen);
                    decimal ParLevelDecimal = Convert.ToDecimal(grdfs.Cells[15].Text);

                    objbalFacilitySupply.Parlevel = Convert.ToInt64(Math.Ceiling(ParLevelDecimal));

                    objbalFacilitySupply.LastModifiedBy = defaultPage.UserId;
                    objbalFacilitySupply.CreatedBy = defaultPage.UserId;

                    // Each an every time hits the server Multi time Insert                      
                    a = lclsService.InsertUpdateFacilitySupply(objbalFacilitySupply);

                }



                if (a == "Saved Successfully")
                {
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.FacilitySuppliesMapSaveMessage.Replace("ShowPopup('", "").Replace("<<FacilitySuppliesMap>>", drpfacility.SelectedItem.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapSaveMessage.Replace("<<FacilitySuppliesMap>>", drpfacility.SelectedItem.Text), true);
                    List<GetFacilitySupplyGird> lstFacilitySupply = lclsService.GetFacilitySupplyGird(objbalFacilitySupply).ToList();
                    //divvendor.Style.Add("display", "block");
                    //grdFacilitySupply.DataSource = lstFacilitySupply;
                    //grdFacilitySupply.DataBind();
                    //BindOrderdate();
                    clearall(1);

                    HddCuurentStatus.Value = "Search";
                    DivAdd.Style.Add("display", "none");
                    DivSearch.Style.Add("display", "block");
                    btnAdd.Visible = true;
                    btnSave.Visible = false;
                    btnsearch.Visible = true;
                    btnprint.Visible = true;
                    btnSave.Enabled = false;

                    BindCorporate();
                    drpcopSearch.ClearSelection();
                    drpcopSearch.SelectedValue = objbalFacilitySupply.CorporateID.ToString();
                    BindFacility();
                    drpfacilitySearch.ClearSelection();
                    drpfacilitySearch.SelectedValue = objbalFacilitySupply.FacilityID.ToString();
                    BindVendor();
                    drpvendorcodeSearch.ClearSelection();
                    drpvendorcodeSearch.SelectedValue = objbalFacilitySupply.VendorID.ToString();
                    BindCategory();
                    drpItemcategorySearch.ClearSelection();
                    drpItemcategorySearch.SelectedValue = objbalFacilitySupply.ItemCategory.ToString();

                    SearchRecords();

                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }
        #endregion 



        /// <summary>
        /// Clear function 
        /// </summary>
        /// <param name="Test"></param>
        #region Clear function
        public void clearall(int Test)
        {
            try
            {
                if (Test == 1)
                    hdnFacilitySupplyID.Value = "0";
                else
                {
                    hdnFacilitySupplyID.Value = "";
                    // txtFactor.Text = "";
                    rbCensus.ClearSelection();
                    if (drpcor.SelectedIndex != -1)
                    {
                        drpcor.SelectedIndex = 0;
                    }
                    if (drpfacility.SelectedIndex != -1)
                    {
                        drpfacility.SelectedIndex = 0;
                    }
                    if (drpvendor.SelectedIndex != -1)
                    {
                        drpvendor.SelectedIndex = 0;
                    }
                    if (drpItemcategory.SelectedIndex != -1)
                    {
                        drpItemcategory.SelectedIndex = 0;
                    }
                    //if (drporderdate.SelectedIndex != -1)
                    //{
                    //    drporderdate.SelectedIndex = 0;
                    //}               

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           

        }
        #endregion

        /// <summary>
        /// Clear button to clear the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        #region Clear event
        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                clearall(0);

                HdnLoad.Value = "0";
                Hdncheckcensus.Value = "";

                if (HddCuurentStatus.Value == "Add")
                {
                    HddCuurentStatus.Value = "Search";
                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();
                    btnAdd.Visible = true;
                    btnSave.Visible = false;
                    btnsearch.Visible = true;
                    btnprint.Visible = true;
                    //divvendor.Style.Add("display", "none");
                    DivAdd.Style.Add("display", "none");
                    DivSearch.Style.Add("display", "block");
                    drpcor.Enabled = true;
                    drpfacility.Enabled = true;
                    drpvendor.Enabled = true;
                    drpItemcategory.Enabled = true;
                    rbCensus.Enabled = true;
                }
                else
                {
                    HddCuurentStatus.Value = "Search";
                    Response.Redirect("FacilitySuppliesMap.aspx");
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }
        #endregion


        /// <summary>
        /// Facility dropdown is binded according to the corporate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region corporate dropdown SelectedIndexChanged event
        protected void drpcor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility();
        }
        #endregion



        #region Search button Click event
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            SearchRecords();
        }
        #endregion



        /// <summary>
        /// Search functionality to bind the gird
        /// </summary>
        #region Search function
        public void SearchRecords()
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALFacilitySupply objFacilitySupply = new BALFacilitySupply();
                if (HddCuurentStatus.Value == "Search")
                {
                    string ListDrpVedorItem = string.Empty;
                    string ListDrpItemCategory = string.Empty;

                    foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                    {
                        if (lst1.Selected && drpvendorcodeSearch.SelectedValue != "All")
                        {
                            SB.Append(lst1.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        ListDrpVedorItem = SB.ToString().Substring(0, (SB.Length - 1));

                    SB.Clear();

                    foreach (ListItem lst1 in drpItemcategorySearch.Items)
                    {
                        if (lst1.Selected && drpItemcategorySearch.SelectedValue != "All")
                        {
                            SB.Append(lst1.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        ListDrpItemCategory = SB.ToString().Substring(0, (SB.Length - 1));

                    SB.Clear();

                    if (drpcopSearch.SelectedIndex != 0 && drpfacilitySearch.SelectedIndex != 0 && ListDrpVedorItem != "" && ListDrpItemCategory != "")
                    {
                        objFacilitySupply.CorporateID = Convert.ToInt64(drpcopSearch.SelectedValue);
                        objFacilitySupply.FacilityID = Convert.ToInt64(drpfacilitySearch.SelectedValue);
                        objFacilitySupply.ListVendorID = ListDrpVedorItem;
                        //objFacilitySupply.VendorOrderDate = Convert.ToDateTime(drporderdate.SelectedItem.Text);
                        objFacilitySupply.ListItemCategory = ListDrpItemCategory;
                        ViewState["SearchDetail"] = objFacilitySupply;

                        BALFacility lclfacility = new BALFacility();
                        lclfacility.SearchText = "";
                        lclfacility.Active = "";
                        lclfacility.LogginBy = defaultPage.UserId;
                        lclfacility.Filter = "";

                        List<BindFacility> lstfacility = lclsService.BindFacility(lclfacility).Where(c => c.FacilityID == objFacilitySupply.FacilityID).ToList();
                        if (lstfacility.Count > 0)
                        {
                            if (lstfacility[0].EmployeeCensus != null)
                                HddEmployeeCensus.Value = lstfacility[0].EmployeeCensus.ToString();
                            else
                                HddEmployeeCensus.Value = "0";

                            if (lstfacility[0].PatientCensus != null)
                                HddPatientCensus.Value = lstfacility[0].PatientCensus.ToString();
                            else
                                HddPatientCensus.Value = "0";

                            if (lstfacility[0].EmployeeCensus != null && lstfacility[0].PatientCensus != null)
                                HddBothCensus.Value = (lstfacility[0].EmployeeCensus + lstfacility[0].PatientCensus).ToString();
                            else
                                HddBothCensus.Value = "0";

                        }


                        List<GetFacilitySupplyGird> lstFacilitySupply = lclsService.GetFacilitySupplyGird(objFacilitySupply).ToList();
                        if (lstFacilitySupply.Count > 0)
                        {
                            if (drpParlevel.SelectedValue == "1")
                            {
                                grdFacilitySupplySearch.DataSource = lstFacilitySupply.Where(a => a.FacilitySupplyID != 0 && a.Parlevel>0).ToList();
                                grdFacilitySupplySearch.DataBind();
                                DivSearchGrid.Style.Add("display", "block");
                                hdnFacilitySupplyID.Value = "0";
                            }
                            else if (drpParlevel.SelectedValue == "2")
                            {
                                grdFacilitySupplySearch.DataSource = lstFacilitySupply.Where(a => a.FacilitySupplyID != 0 && a.Parlevel <= 0).ToList();
                                grdFacilitySupplySearch.DataBind();
                                DivSearchGrid.Style.Add("display", "block");
                                hdnFacilitySupplyID.Value = "0";
                            }
                            else
                            {
                                grdFacilitySupplySearch.DataSource = lstFacilitySupply.Where(a => a.FacilitySupplyID != 0).ToList();
                                grdFacilitySupplySearch.DataBind();
                                DivSearchGrid.Style.Add("display", "block");
                                hdnFacilitySupplyID.Value = "0";
                            }
                          

                        }
                        else
                        {
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "NO Records Found");
                            log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "NO Records Found"));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "NO Records Found"), true);

                            grdFacilitySupplySearch.DataSource = null;
                            grdFacilitySupplySearch.DataBind();
                            //divvendor.Style.Add("display", "block");
                            hdnFacilitySupplyID.Value = "";
                            rbCensus.ClearSelection();
                            //txtFactor.Text = "";                        
                        }
                    }
                    else
                    {
                        //Functions objfun = new Functions();
                        //objfun.MessageDialog(this, "Select the Mandatory Fields for Search");
                        log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "Select the Mandatory Fields for Search"));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "Select the Mandatory Fields for Search"), true);
                    }
                }
                else
                {
                    if (drpcor.SelectedIndex != 0 && drpfacility.SelectedIndex != 0 && drpvendor.SelectedIndex != 0 && drpItemcategory.SelectedIndex != 0)
                    {
                        objFacilitySupply.CorporateID = Convert.ToInt64(drpcor.SelectedValue);
                        objFacilitySupply.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
                        objFacilitySupply.ListVendorID = drpvendor.SelectedValue;
                        //objFacilitySupply.VendorOrderDate = Convert.ToDateTime(drporderdate.SelectedItem.Text);
                        objFacilitySupply.ListItemCategory = drpItemcategory.SelectedValue;
                        ViewState["SearchDetail"] = objFacilitySupply;

                        BALFacility lclfacility = new BALFacility();
                        lclfacility.SearchText = "";
                        lclfacility.Active = "";
                        lclfacility.LogginBy = defaultPage.UserId;
                        lclfacility.Filter = "";

                        List<BindFacility> lstfacility = lclsService.BindFacility(lclfacility).Where(c => c.FacilityID == objFacilitySupply.FacilityID).ToList();
                        if (lstfacility.Count > 0)
                        {
                            if (lstfacility[0].EmployeeCensus != null)
                                HddEmployeeCensus.Value = lstfacility[0].EmployeeCensus.ToString();
                            else
                                HddEmployeeCensus.Value = "0";

                            if (lstfacility[0].PatientCensus != null)
                                HddPatientCensus.Value = lstfacility[0].PatientCensus.ToString();
                            else
                                HddPatientCensus.Value = "0";

                            HddBothCensus.Value = (Convert.ToInt64(HddEmployeeCensus.Value) + Convert.ToInt64(HddPatientCensus.Value)).ToString(); 

                            

                        }

                        List<GetFacilitySupplyGird> lstFacilitySupply = lclsService.GetFacilitySupplyGird(objFacilitySupply).Where(a => a.CategoryID == Convert.ToInt64(drpItemcategory.SelectedValue)).ToList();
                        if (lstFacilitySupply.Count > 0)
                        {
                            drpItemcategory.ClearSelection();
                            drpItemcategory.Items.FindByText(lstFacilitySupply[0].CategoryName).Selected = true;
                            //txtFactor.Text = Convert.ToString(lstFacilitySupply[0].Factor);
                            if (lstFacilitySupply[0].IsEmp == true)
                            {
                                rbCensus.SelectedValue = "1";
                                txtotherCensus.Enabled = false;
                                txtotherCensus.Text = "";
                            }
                            else if (lstFacilitySupply[0].IsPatient == true)
                            {
                                rbCensus.SelectedValue = "2";
                                txtotherCensus.Enabled = false;
                                txtotherCensus.Text = "";
                            }
                            else if (lstFacilitySupply[0].IsEmp == true && lstFacilitySupply[0].IsPatient == true)
                            {
                                rbCensus.SelectedValue = "3";
                                txtotherCensus.Enabled = false;
                                txtotherCensus.Text = "";
                            }
                            else if (lstFacilitySupply[0].IsEmp == false && lstFacilitySupply[0].IsPatient == false)
                            {
                                if (lstFacilitySupply[0].FacilitySupplyID > 0)
                                {
                                    rbCensus.SelectedValue = "4";
                                    txtotherCensus.Text = lstFacilitySupply[0].Census.ToString();
                                    txtotherCensus.Enabled = true;
                                }
                                else
                                {
                                    rbCensus.ClearSelection();
                                    txtotherCensus.Enabled = false;
                                    txtotherCensus.Text = "";
                                }
                            }
                            else
                            {
                                rbCensus.ClearSelection();
                            }
                            lstFacilitySupply = lstFacilitySupply.Where(a => a.CategoryID == Convert.ToInt64(drpItemcategory.SelectedValue)).ToList();

                            grdFacilitySupply.DataSource = lstFacilitySupply;
                            grdFacilitySupply.DataBind();
                            //divvendor.Style.Add("display", "block");
                            lstFacilitySupply = lstFacilitySupply.Where(b => b.FacilitySupplyID == 0).ToList();
                            if (lstFacilitySupply.Count > 0)
                            {
                                hdnFacilitySupplyID.Value = "1";
                            }
                            else
                            {
                                hdnFacilitySupplyID.Value = "0";
                            }

                        }
                        else
                        {
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "NO Records Found");
                            log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "NO Records Found"));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "NO Records Found"), true);

                            grdFacilitySupply.DataSource = null;
                            grdFacilitySupply.DataBind();
                            //divvendor.Style.Add("display", "block");
                            hdnFacilitySupplyID.Value = "";
                            rbCensus.ClearSelection();
                            //txtFactor.Text = "";                        
                        }
                    }
                    else
                    {
                        //Functions objfun = new Functions();
                        //objfun.MessageDialog(this, "Select the Mandatory Fields for Search");
                        log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "Select the Mandatory Fields for Search"));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "Select the Mandatory Fields for Search"), true);
                    }
                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
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
            BindVendor();
        }
        #endregion

        /// <summary>
        /// Drop down event to bind Item Catgory and Order Date details based on selected Vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Vendor dropdown SelectedIndexChanged event
        protected void drpvendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCategory();
                BindOrderdate();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           
        }
        #endregion

        /// <summary>
        /// Bind the Grid With Items mapped with the Item Catogory based on the Order date selected
        /// </summary>
        #region Bind the Grid function
        //public void GetNewGrid()
        //{
        //    grdFacilitySupply.DataSource = null;
        //    BALFacilitySupply objbalFacilitySupply = new BALFacilitySupply();
        //    objbalFacilitySupply.CorporateID = Convert.ToInt64(drpcor.SelectedValue);
        //    objbalFacilitySupply.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
        //    objbalFacilitySupply.VendorID = Convert.ToInt64(drpvendor.SelectedValue);
        //    objbalFacilitySupply.ItemCategory = Convert.ToInt64(drpItemcategory.SelectedValue);
        //    List<BindVendorOrderDue> lstVendorOrderDate = lclsService.BindVendorOrderDue(objbalFacilitySupply).ToList();

        //    List<GetFacilitySupplyGird> lstFacilitySupply1 = lclsService.GetFacilitySupplyGird(objbalFacilitySupply).Where(b => b.CategoryID == Convert.ToInt64(drpItemcategory.SelectedValue)).ToList();
        //    // if (lstFacilitySupply1.Count == 0)
        //    // {
        //    if (lstVendorOrderDate.Count > 0)
        //    {
        //        objbalFacilitySupply.VendorOrderDate = Convert.ToDateTime(lstVendorOrderDate[0].OrderdueDate);
        //        List<GetFacilitySupplyGird> lstFacilitySupply = lclsService.GetFacilitySupplyGird(objbalFacilitySupply).ToList();
        //        grdFacilitySupply.DataSource = lstFacilitySupply;
        //        grdFacilitySupply.DataBind();
        //        divvendor.Style.Add("display", "block");
        //        //txtFactor.Text = "";
        //        //rbCensus.ClearSelection();
        //        //hdnFacilitySupplyID.Value = "";
        //        btnSave.Text = "Save";
        //    }
        //    else
        //    {
        //        //Functions objfun = new Functions();
        //        //objfun.MessageDialog(this, "Vendor Order Due is not available");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "Vendor Order Due is not available"), true);
        //    }
        //    //  }
        //    //else
        //    //{
        //    //    //Functions objfun = new Functions();
        //    //    //objfun.MessageDialog(this, "Record already Exits");
        //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "Record already Exits"), true);
        //    //}
        //}
        #endregion

        /// <summary>
        /// Drop down event to bind the Existing record in the grid or create new based on the order date selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region order date dropdown SelectedIndexChanged event
        //protected void drporderdate_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //if (drporderdate.SelectedItem.Text == "New" && drpItemcategory.SelectedIndex != 0)
        //{
        //    GetNewGrid();
        //}
        //else if (drporderdate.SelectedItem.Text != "---Select---" && drpItemcategory.SelectedIndex != 0)
        //{
        //    SearchRecords();
        //}
        //else
        //{
        //    txtFactor.Text = "";
        //    rbCensus.ClearSelection();
        //    hdnFacilitySupplyID.Value = "";
        //    btnSave.Text = "New";
        //    grdFacilitySupply.DataSource = null;
        //    grdFacilitySupply.DataBind();
        //    divvendor.Style.Add("display", "none");
        //}
        //}
        #endregion


        /// <summary>
        /// Drop down event to bind the Existing record in the grid or create new based on the order date and Item category selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Item category dropdown SelectedIndexChanged event
        protected void drpItemcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SearchRecords();
                if (hdnFacilitySupplyID.Value != "1")
                {
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "Record already exists"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "Record already exists"), true);
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
                //if (drporderdate.SelectedItem.Text == "New")
                //{
                //    GetNewGrid();
                //}
                //else if (drporderdate.SelectedItem.Text != "---Select---")
                //{
                //    SearchRecords();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           
        }
        #endregion


        /// <summary>
        /// Edit Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Edit Function
        public void EditbyID(GridViewRow gvrow)
        {
            try
            {
                BALFacilitySupply objFacilitySupply = new BALFacilitySupply();
                divvendor.Style.Add("display", "block");
                objFacilitySupply.CorporateID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
                objFacilitySupply.FacilityID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                objFacilitySupply.ListVendorID = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                //objFacilitySupply.VendorOrderDate = Convert.ToDateTime(drporderdate.SelectedItem.Text);
                objFacilitySupply.ListItemCategory = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                Int64 FaciSuppID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                ViewState["SearchDetail"] = objFacilitySupply;
                List<GetFacilitySupplyGird> FirstlstFacilitySupply = new List<GetFacilitySupplyGird>();
                List<GetFacilitySupplyGird> FinallstFacilitySupply = new List<GetFacilitySupplyGird>();

                //get cencus value//
                BALFacility lclfacility = new BALFacility();
                lclfacility.SearchText = "";
                lclfacility.Active = "";
                lclfacility.LogginBy = defaultPage.UserId;
                lclfacility.Filter = "";

                List<BindFacility> lstfacility = lclsService.BindFacility(lclfacility).Where(c => c.FacilityID == objFacilitySupply.FacilityID).ToList();
                if (lstfacility.Count > 0)
                {
                    if (lstfacility[0].EmployeeCensus != null)
                        HddEmployeeCensus.Value = lstfacility[0].EmployeeCensus.ToString();
                    else
                        HddEmployeeCensus.Value = "0";

                    if (lstfacility[0].PatientCensus != null)
                        HddPatientCensus.Value = lstfacility[0].PatientCensus.ToString();
                    else
                        HddPatientCensus.Value = "0";

                    HddBothCensus.Value = (Convert.ToInt64(HddEmployeeCensus.Value) + Convert.ToInt64(HddPatientCensus.Value)).ToString();
                }
                //end//

                List<GetFacilitySupplyGird> lstFacilitySupply = lclsService.GetFacilitySupplyGird(objFacilitySupply).ToList();
                if (lstFacilitySupply.Count > 0)
                {
                    divvendor.Style.Add("display", "block");
                    HddCuurentStatus.Value = "Add";
                    DivAdd.Style.Add("display", "block");
                    DivSearch.Style.Add("display", "none");
                    btnAdd.Visible = false;
                    btnSave.Visible = true;
                    btnsearch.Visible = false;
                    btnprint.Visible = false;
                    btnSave.Enabled = true;
                    drpItemcategory.ClearSelection();
                    drpItemcategorySearch.Items.FindByText(lstFacilitySupply[0].CategoryName).Selected = true;
                    BindCorporate();
                    //hdnFacilitySupplyID.Value = "0";
                    rbCensus.ClearSelection();
                    if (lstFacilitySupply[0].IsEmp == true)
                    {
                        rbCensus.SelectedValue = "1";
                        txtotherCensus.Text = "";
                        txtotherCensus.Enabled = false;
                    }
                    else if (lstFacilitySupply[0].IsPatient == true)
                    {
                        rbCensus.SelectedValue = "2";
                        txtotherCensus.Text = "";
                        txtotherCensus.Enabled = false;
                    }
                    else if (lstFacilitySupply[0].Isboth == true)
                    {
                        rbCensus.SelectedValue = "3";
                        txtotherCensus.Text = "";
                        txtotherCensus.Enabled = false;
                    }
                    else if (lstFacilitySupply[0].Isother == true)
                    {
                        rbCensus.SelectedValue = "4";
                        txtotherCensus.Text = lstFacilitySupply[0].Census.ToString();
                        txtotherCensus.Enabled = false;
                    }
                    else
                    {
                        rbCensus.ClearSelection();
                    }
                    Hdncheckcensus.Value = rbCensus.SelectedValue;
                    rbCensus.Enabled = true;
                    drpcor.ClearSelection();
                    drpcor.SelectedValue = lstFacilitySupply[0].CorporateID.ToString();
                    drpcor.Enabled = false;
                    BindFacility();
                    drpfacility.ClearSelection();
                    drpfacility.SelectedValue = lstFacilitySupply[0].FacilityID.ToString();
                    drpfacility.Enabled = false;
                    BindVendor();
                    drpvendor.ClearSelection();
                    drpvendor.SelectedValue = lstFacilitySupply[0].VendorID.ToString();
                    drpvendor.Enabled = false;
                    BindCategory();
                    drpItemcategory.ClearSelection();
                    drpItemcategory.SelectedValue = lstFacilitySupply[0].CategoryID.ToString();
                    drpItemcategory.Enabled = false;
                    divvendor.Style.Add("display", "block");

                    lstFacilitySupply = lstFacilitySupply.Where(a => a.CategoryID == Convert.ToInt64(drpItemcategory.SelectedValue)).ToList();

                    FirstlstFacilitySupply = lstFacilitySupply.Where(b => b.FacilitySupplyID == FaciSuppID).ToList();
                    FinallstFacilitySupply = lstFacilitySupply.Where(c => c.FacilitySupplyID != FaciSuppID).ToList();

                    FinallstFacilitySupply = FirstlstFacilitySupply.Concat(FinallstFacilitySupply).ToList();

                    //grdFacilitySupply.DataSource = FinallstFacilitySupply;
                    //grdFacilitySupply.DataBind();


                    grdFacilitySupply.DataSource = FinallstFacilitySupply;
                    grdFacilitySupply.DataBind();
                }
                else
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "NO Records Found");
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msgwrn.Replace("<<FacilitySuppliesMap>>", "NO Records Found"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningFacilitySuppliesMapMessage.Replace("<<FacilitySuppliesMap>>", "NO Records Found"), true);

                    grdFacilitySupply.DataSource = null;
                    grdFacilitySupply.DataBind();
                    hdnFacilitySupplyID.Value = "";
                    rbCensus.ClearSelection();
                    //txtFactor.Text = "";                        
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }

        #endregion


        /// <summary>
        /// Grid Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        #region Grid Edit Button Click
        protected void ImgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                EditbyID(gvrow);
                hdnEdit.Value = "1";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
         
        }

        #endregion


        /// <summary>
        /// Print Summary and Details Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Print Button Click
        protected void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 CorporateId = 0;
                Int64 FacilityId = 0;
                Int64 ParlevelId = 0;
                string SearchFilter = null;
                CorporateId = Convert.ToInt64(drpcopSearch.SelectedValue);
                FacilityId = Convert.ToInt64(drpfacilitySearch.SelectedValue);
                ParlevelId = Convert.ToInt64(drpParlevel.SelectedValue);
            
                string ListDrpVedorItem = string.Empty;
                string ListDrpItemCategory = string.Empty;

                foreach (ListItem lst1 in drpvendorcodeSearch.Items)
                {
                    if (lst1.Selected && drpvendorcodeSearch.SelectedValue != "All")
                    {
                        SB.Append(lst1.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    ListDrpVedorItem = SB.ToString().Substring(0, (SB.Length - 1));

                SB.Clear();

                foreach (ListItem lst1 in drpItemcategorySearch.Items)
                {
                    if (lst1.Selected && drpItemcategorySearch.SelectedValue != "All")
                    {
                        SB.Append(lst1.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    ListDrpItemCategory = SB.ToString().Substring(0, (SB.Length - 1));

                SB.Clear();


                List<GetFacilitySuppliesMapReport> llstreview = new List<GetFacilitySuppliesMapReport>();


                llstreview = lclsService.GetFacilitySuppliesMapReport(CorporateId, FacilityId, ListDrpVedorItem, ListDrpItemCategory,ParlevelId,SearchFilter, defaultPage.UserId).ToList();
                rvFacilitySuppliesMapreport.ProcessingMode = ProcessingMode.Local;
                rvFacilitySuppliesMapreport.LocalReport.ReportPath = Server.MapPath("~/Reports/FacilitySupplyMapSummary.rdlc");

                Int64 r = defaultPage.UserId;

                ReportDataSource datasource = new ReportDataSource("DSFacilitySuppliesMap", llstreview);
                rvFacilitySuppliesMapreport.LocalReport.DataSources.Clear();
                rvFacilitySuppliesMapreport.LocalReport.DataSources.Add(datasource);
                rvFacilitySuppliesMapreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvFacilitySuppliesMapreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);


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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Show Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Show PDF File
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
        }





        #endregion

        protected void btnreviewcancel_Click(object sender, EventArgs e)
        {
            try
            {
                mpeFacilSupplyReview.Hide();
                if (hdnEdit.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "addfacility();", true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
          
        }

        protected void btnrevclose_Click(object sender, EventArgs e)
        {
            try
            {
                mpeFacilSupplyReview.Hide();
                if (hdnEdit.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "addfacility();", true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySuppliesMapErrorMessage.Replace("<<FacilitySuppliesMap>>", ex.Message), true);
            }
           
        }
    }
}