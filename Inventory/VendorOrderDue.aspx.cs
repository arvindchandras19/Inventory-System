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

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   VendorOrderDue
'' Type      :   C# File
'' Description  :To add,update the VendorOrderDue Details
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				  Dhanasekaran.C	                  New
 *  10/24/2017         V2.0               Sairam.P                           Validate mandatory fields and Check the active and 
 *                                                                           inactive records in dropdown
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class VendorOverDue : System.Web.UI.Page
    {
        InventoryServiceClient lclsService = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnprint);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    if (defaultPage != null)
                    {
                        BindCorporate(1, "Add");
                        drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility(1, "Add");
                        BindVendor(1, "Add");
                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select--";
                        drpdeliwin.Items.Insert(0, lst);
                        drpdeliwin.SelectedIndex = 0;
                        txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        txtDateTo.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString("MM/dd/yyyy");
                        SearchRecords();

                        if (defaultPage.VendorOrderDue_Edit == false && defaultPage.VendorOrderDue_View == true)
                        {
                            btnSave.Visible = false;
                        }

                        if (defaultPage.VendorOrderDue_Edit == false && defaultPage.VendorOrderDue_View == false)
                        {
                            //grdvendororderue.Visible = false;
                            //btnSave.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
          
        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            Reqfielddrpcor.ErrorMessage = req;
            Reqdrpfacility.ErrorMessage = req;
            Reqdrpvendorsearch.ErrorMessage = req;
            reqcroporate.ErrorMessage = req;
            reqfacility.ErrorMessage = req;
            reqvendoradd.ErrorMessage = req;
            radRfv.ErrorMessage = req;
            reqfieldorderduedate.ErrorMessage = req;
            RequiredFielddrpdeliwin.ErrorMessage = req;
            reqfieldtxtdaysnotify.ErrorMessage = req;
        }
        private void BindFacility(int search, string mode)
        {
            try
            {
                if (search == 1)
                {
                    if (drpcor.SelectedValue != "")
                    {
                        foreach (ListItem lst1 in drpcor.Items)
                        {
                            if (lst1.Selected && drpcor.SelectedValue != "All")
                            {
                                SB.Append(lst1.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        if (drpcor.SelectedValue != "")
                        {
                            drpfacilitysearch.DataSource = lclsService.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                            drpfacilitysearch.DataTextField = "FacilityDescription";
                            drpfacilitysearch.DataValueField = "FacilityID";
                            drpfacilitysearch.DataBind();

                            foreach (ListItem lst in drpfacilitysearch.Items)
                            {
                                lst.Attributes.Add("class", "selected");
                                lst.Selected = true;
                            }
                        }
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
                            ddrpfacility.DataSource = lclsService.GetCorporateFacility(Convert.ToInt64(ddrpcorporate.SelectedValue)).Where(a => a.IsActive == true).ToList();
                            ddrpfacility.DataTextField = "FacilityDescription";
                            ddrpfacility.DataValueField = "FacilityID";
                            ddrpfacility.DataBind();
                            ddrpfacility.Items.Insert(0, lst);
                            ddrpfacility.SelectedIndex = 0;
                        }
                        else
                        {
                            ddrpfacility.DataSource = lclsService.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == ddrpcorporate.SelectedItem.Text).ToList();
                            ddrpfacility.DataTextField = "FacilityName";
                            ddrpfacility.DataValueField = "FacilityID";
                            ddrpfacility.DataBind();
                            ddrpfacility.Items.Insert(0, lst);
                            ddrpfacility.SelectedIndex = 0;
                        }
                    }

                    else if (mode == "Edit")
                    {
                        ddrpfacility.DataSource = lclsService.GetCorporateFacility(Convert.ToInt64(ddrpcorporate.SelectedValue)).ToList();
                        ddrpfacility.DataTextField = "FacilityDescription";
                        ddrpfacility.DataValueField = "FacilityID";
                        ddrpfacility.DataBind();
                        ddrpfacility.Items.Insert(0, lst);
                        ddrpfacility.SelectedIndex = 0;
                    }

                }
                BindVendor(1, "Add");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }
        public void BindCorporate(int search, string mode)
        {
            try
            {
                List<BALUser> lstfacility = new List<BALUser>();
                if (search == 1)
                {

                    if (defaultPage.RoleID == 1)
                    {
                        // Search Drop Down   
                        lstfacility = lclsService.GetCorporateMaster().ToList();
                        drpcor.DataSource = lstfacility;
                        drpcor.DataTextField = "CorporateName";
                        drpcor.DataValueField = "CorporateID";
                        drpcor.DataBind();
                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select Corporate--";
                        drpcor.Items.Insert(0, lst);
                        drpcor.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsService.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        drpcor.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        drpcor.DataTextField = "CorporateName";
                        drpcor.DataValueField = "CorporateID";
                        drpcor.DataBind();
                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select Corporate--";
                        drpcor.Items.Insert(0, lst);
                        drpcor.SelectedIndex = 0;
                    }
                }

                ListItem lstDDl = new ListItem();
                lstDDl.Value = "0";
                lstDDl.Text = "--Select Corporate--";
                if (mode == "Add")
                {
                    if (defaultPage.RoleID == 1)
                    {
                        lstfacility = lclsService.GetCorporateMaster().ToList();
                        ddrpcorporate.DataSource = lstfacility;
                        ddrpcorporate.DataTextField = "CorporateName";
                        ddrpcorporate.DataValueField = "CorporateID";
                        ddrpcorporate.DataBind();
                        ddrpcorporate.Items.Insert(0, lstDDl);
                        ddrpcorporate.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsService.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        ddrpcorporate.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        ddrpcorporate.DataTextField = "CorporateName";
                        ddrpcorporate.DataValueField = "CorporateID";
                        ddrpcorporate.DataBind();
                        ddrpcorporate.Items.Insert(0, lstDDl);
                        ddrpcorporate.SelectedIndex = 0;
                    }
                }
                if (mode == "Edit")
                {
                    ddrpcorporate.DataSource = lclsService.GetCorporateMaster().ToList();
                    ddrpcorporate.DataTextField = "CorporateName";
                    ddrpcorporate.DataValueField = "CorporateID";
                    ddrpcorporate.DataBind();
                    ddrpcorporate.Items.Insert(0, lstDDl);
                    ddrpcorporate.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        //public void BindVendor()
        //{
        //    List<GetvendorDetails> lstvendordetails = new List<GetvendorDetails>();
        //    string SearchText = string.Empty;
        //    lstvendordetails = lclsService.GetvendorDetails(SearchText).ToList();
        //    drpvendor.DataSource = lstvendordetails;
        //    drpvendor.DataTextField = "VendorDescription";
        //    drpvendor.DataValueField = "VendorID";
        //    drpvendor.DataBind();
        //    ListItem lst = new ListItem();
        //    lst.Value = "0";
        //    lst.Text = "--Select Vendor--";
        //    drpvendor.Items.Insert(0, lst);
        //    drpvendor.SelectedIndex = 0;
        //}
        public void BindVendor(int search, string mode)
        {
            try
            {
                List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                if (search == 1)
                {
                    if (drpfacilitysearch.SelectedValue != "")
                    {
                        foreach (ListItem lstitem in drpfacilitysearch.Items)
                        {
                            if (lstitem.Selected && drpfacilitysearch.SelectedValue != "All")
                            {
                                SB.Append(lstitem.Value + ',');
                            }
                        }
                        if (SB.Length > 0)
                            FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                        string SearchText = string.Empty;
                        List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
                        lstvendor = lclsService.GetVendorByFacilityID(FinalString, defaultPage.UserId).ToList();
                        drpVendorsearch.DataSource = lstvendor;
                        drpVendorsearch.DataTextField = "VendorDescription";
                        drpVendorsearch.DataValueField = "VendorID";
                        drpVendorsearch.DataBind();
                        //ListItem lst = new ListItem();
                        //lst.Value = "0";
                        //lst.Text = "--Select Vendor--";
                        //drpvendor.Items.Insert(0, lst);
                        //drpvendor.SelectedIndex = 0;

                        foreach (ListItem lst in drpVendorsearch.Items)
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
                    lst.Text = "--Select Vendor--";
                    if (mode == "Add")
                    {
                        lstvendordetails = lclsService.GetFacilityVendorAccount(ddrpfacility.SelectedItem.Text).Where(a => a.IsActive == true).Distinct().ToList();
                        ddrpvendor.DataSource = lstvendordetails;
                        ddrpvendor.DataTextField = "VendorDescription";
                        ddrpvendor.DataValueField = "VendorID";
                        ddrpvendor.DataBind();
                        ddrpvendor.Items.Insert(0, lst);
                        ddrpvendor.SelectedIndex = 0;
                    }
                    else if (mode == "Edit")
                    {
                        lstvendordetails = lclsService.GetFacilityVendorAccount(ddrpfacility.SelectedItem.Text).ToList();
                        ddrpvendor.DataSource = lstvendordetails;
                        ddrpvendor.DataTextField = "VendorDescription";
                        ddrpvendor.DataValueField = "VendorID";
                        ddrpvendor.DataBind();
                        ddrpvendor.Items.Insert(0, lst);
                        ddrpvendor.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        public void SearchRecords()
        {
            try
            {
                divresult.Style.Add("display", "block");
                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbalvendorOrderDue.ListFacilityID = "ALL";
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
                    objbalvendorOrderDue.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpVendorsearch.SelectedValue == "All")
                {
                    objbalvendorOrderDue.ListVendorID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpVendorsearch.Items)
                    {
                        if (lst.Selected && drpVendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbalvendorOrderDue.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalvendorOrderDue.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {

                    txtDateTo.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString();
                }
                else
                {
                    objbalvendorOrderDue.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                objbalvendorOrderDue.Filter = "";
                objbalvendorOrderDue.LoggedInBy = defaultPage.UserId;
                List<SearchVendorOrderType> lstvendor = lclsService.SearchVendorOrderType(objbalvendorOrderDue).ToList();
                grdvendororderue.DataSource = lstvendor;
                grdvendororderue.DataBind();
                hdnvendororderID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        public void BindVendorOderDueGrid()
        {
            try
            {
                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                objbalvendorOrderDue.OrderType = Convert.ToInt32(rbordertype.SelectedValue);
                if (rbordertype.SelectedValue == "4")
                {
                    objbalvendorOrderDue.OrderDueDate = System.DateTime.Now;
                }
                else
                {
                    if (txtorderduedate.Text != "")
                        objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                }
                List<ValidateVendorOrderType> lstvendor = lclsService.ValidateVendorOrderType(objbalvendorOrderDue).ToList();
                grdaddvendor.DataSource = lstvendor;
                grdaddvendor.DataBind();
                btnreview.Visible = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        protected void ChkAllFac_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
         
        }

        protected void lnkMultiFac_Click(object sender, EventArgs e)
        {

            try
            {
                int j = 0;
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
                    GrdMultiFac.DataSource = lclsService.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                    GrdMultiFac.DataBind();
                    DivFacCorp.Style.Add("display", "block");
                    divvendordue.Attributes["class"] = "Upopacity";
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
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
                divvendordue.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            try
            {
                DivFacCorp.Style.Add("display", "none");
                divvendordue.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
           
        }



        protected void lnkClearFac_Click(object sender, EventArgs e)
        {
            try
            {
                BindFacility(1, "Add");
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        protected void lnkClearAllFac_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst in drpfacilitysearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                }

                foreach (ListItem lst in drpVendorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                }
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        protected void drpcor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(1, "Add");
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            SearchRecords();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                string a = string.Empty;

                if (hdndeleteid.Value == "1")
                {
                    objbalvendorOrderDue.CorporateID = Convert.ToInt64(ddrpcorporate.SelectedValue);
                    objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                    objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                    if (rbordertype.SelectedValue == "4")
                    {
                        objbalvendorOrderDue.OrderDueDate = System.DateTime.Now;
                    }
                    else
                    {
                        if (txtorderduedate.Text != "")
                            objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                    }
                    objbalvendorOrderDue.IsActive = false;
                    objbalvendorOrderDue.LastModifitedBy = defaultPage.UserId;
                    string lstrMessage = lclsService.DeleteVendorOrderDue(objbalvendorOrderDue);
                    if (lstrMessage == "Deleted Successfully")
                    {
                        //SearchRecords();
                        //BindVendorOderDueGrid();
                        hdnvendororderID.Value = "";
                        hdndeleteid.Value = "";
                    }
                    else
                    {
                        string msg = Constant.WarningVendorOrderDueMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<VendorOrderDue>>", "Overwritten is not performed").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueMessage.Replace("<<VendorOrderDue>>", "Overwritten is not performed"), true);
                    }
                }
                
              if (hdnvendororderID.Value == "")
                {
                    foreach (GridViewRow grdfs in grdaddvendor.Rows)
                    {
                        Label lbladdFacility = (Label)grdfs.FindControl("lbladdFacility");
                        Label lbladdVendor = (Label)grdfs.FindControl("lbladdVendor");
                        TextBox txtoderDuedate = (TextBox)grdfs.FindControl("txtoderDuedate");
                        TextBox txtDeliveryDate = (TextBox)grdfs.FindControl("txtDeliveryDate");
                        Label lbladdOrderType = (Label)grdfs.FindControl("lbladdOrderType");
                        TextBox txtToNotify = (TextBox)grdfs.FindControl("txtToNotify");

                        if (lbladdOrderType.Text == "Ad-hoc")
                        {
                            txtorderduedate.Text = Convert.ToString(System.DateTime.Now);
                            txtDeliveryDate.Text = Convert.ToString(System.DateTime.Now);
                            txtdaysnotify.Text = "1";
                            drpdeliwin.SelectedValue = "1";
                        }

                        if (hdnvendororderID.Value != "")
                            objbalvendorOrderDue.VendorOrderdueID = Convert.ToInt64(hdnvendororderID.Value);
                        objbalvendorOrderDue.CorporateID = Convert.ToInt64(ddrpcorporate.SelectedValue);
                        objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                        objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                        if (rbordertype.SelectedValue != "")
                            objbalvendorOrderDue.OrderType = Convert.ToInt32(rbordertype.SelectedValue);
                        if (txtorderduedate.Text != "")
                            objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                        if (txtDeliveryDate.Text != "")
                            objbalvendorOrderDue.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                        if (drpdeliwin.SelectedValue != "")
                            objbalvendorOrderDue.DeliveryWindow = Convert.ToInt64(drpdeliwin.SelectedValue);
                        if (txtdaysnotify.Text != "")
                            objbalvendorOrderDue.DaytoToNotify = Convert.ToInt64(txtdaysnotify.Text);
                        objbalvendorOrderDue.CreatedBy = defaultPage.UserId;

                        if (Convert.ToInt64(grdfs.Cells[0].Text) == 0)
                        {
                            a = lclsService.InsertUpdateVendorOrderDue(objbalvendorOrderDue);
                        }
                        else
                        {
                            a = "Saved Successfully";
                        }
                    }

                    hdnvendororderID.Value = "";
                    SearchRecords();
                }
                else
                {
                    string s = hdnvendororderID.Value;
                    s = s.Substring(0, s.Length - 1);
                    string[] values = s.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        GridViewRow row = grdaddvendor.Rows[Convert.ToInt32(values[i])];
                        objbalvendorOrderDue.VendorOrderdueID = Convert.ToInt64(row.Cells[0].Text);
                        TextBox txtoderDuedate = (TextBox)row.FindControl("txtoderDuedate");
                        TextBox txtDeliveryDate = (TextBox)row.FindControl("txtDeliveryDate");
                        TextBox txtToNotify = (TextBox)row.FindControl("txtToNotify");
                        if (txtoderDuedate.Text != "")
                            objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtoderDuedate.Text);
                        if (txtDeliveryDate.Text != "")
                            objbalvendorOrderDue.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                        if (txtToNotify.Text != "")
                            objbalvendorOrderDue.DaytoToNotify = Convert.ToInt64(txtToNotify.Text);
                        objbalvendorOrderDue.LastModifitedBy = defaultPage.UserId;
                        a = lclsService.InsertUpdateVendorOrderDue(objbalvendorOrderDue);
                    }
                    hdnvendororderID.Value = "";
                }
                if (a == "Saved Successfully")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    string msg = Constant.VendorOrderDueSaveMessage.Replace("ShowPopup('", "").Replace("<<VendorOrderDue>>", ddrpvendor.SelectedItem.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueSaveMessage.Replace("<<VendorOrderDue>>", ddrpvendor.SelectedItem.Text), true);
                    SearchRecords();
                    closediv();
                    //BindVendorOderDueGrid();
                    hdneditororder.Value = "";
                    hdnvendororderID.Value = "";
                    clearall();
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }

        }
        public void clearall()
        {
            hdnvendororderID.Value = "";
            ddrpcorporate.SelectedIndex = 0;
            ddrpfacility.ClearSelection();
            ddrpfacility.SelectedIndex = -1;
            ddrpvendor.ClearSelection();
            ddrpvendor.SelectedIndex = -1;
            rbordertype.SelectedIndex = -1;
            txtorderduedate.Text = "";
            drpdeliwin.SelectedIndex = 0;
            txtdaysnotify.Text = "";
        }


        //protected void txtoderDuedate_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox tb = (TextBox)sender;
        //    GridViewRow gvr = (GridViewRow)tb.NamingContainer;
        //    hdnvendororderID.Value += gvr.RowIndex + ",";
        //}

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                BindCorporate(1, "Add");
                drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                BindFacility(1, "Add");
                BindVendor(1, "Add");
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpdeliwin.Items.Insert(0, lst);
                drpdeliwin.SelectedIndex = 0;
                txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtDateTo.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString("MM/dd/yyyy");
                SearchRecords();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        protected void drpfacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }
        protected class DeliveryWindow
        {
            public string Text { get; set; }
            public Int32 Value { get; set; }
        }
        protected static List<DeliveryWindow> students = new List<DeliveryWindow>
        {
        new DeliveryWindow {Text = "1", Value = 1},
        new DeliveryWindow {Text = "2", Value = 2},
        new DeliveryWindow {Text = "3", Value = 3},
        new DeliveryWindow {Text = "4", Value = 4},
        new DeliveryWindow {Text = "5", Value = 5},
        new DeliveryWindow {Text = "6", Value = 6},
        new DeliveryWindow {Text = "7", Value = 7},
        new DeliveryWindow {Text = "8", Value = 8},
        new DeliveryWindow {Text = "9", Value = 9},
        new DeliveryWindow {Text = "10", Value = 10},
        new DeliveryWindow {Text = "11", Value = 11},
        new DeliveryWindow {Text = "12", Value = 12},
        new DeliveryWindow {Text = "13", Value = 13},
        new DeliveryWindow {Text = "14", Value = 14},
        new DeliveryWindow {Text = "15", Value = 15},
        new DeliveryWindow {Text = "16", Value = 16},
        new DeliveryWindow {Text = "17", Value = 17},
        new DeliveryWindow {Text = "18", Value = 18},
        new DeliveryWindow {Text = "19", Value = 19},
        new DeliveryWindow {Text = "20", Value = 20},
        new DeliveryWindow {Text = "21", Value = 21},
        new DeliveryWindow {Text = "22", Value = 22},
        new DeliveryWindow {Text = "23", Value = 23},
        new DeliveryWindow {Text = "24", Value = 24},
        new DeliveryWindow {Text = "25", Value = 25},
        new DeliveryWindow {Text = "26", Value = 26},
        new DeliveryWindow {Text = "27", Value = 27},
        new DeliveryWindow {Text = "28", Value = 28}
        };

        protected void rbordertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rbotype();
                txtdaysnotify.Text = "";
                grdaddvendor.DataSource = null;
                grdaddvendor.DataBind();
                btnreview.Visible = false;
                if (rbordertype.SelectedValue == "4")
                {
                    txtorderduedate.Text = "";
                    txtdaysnotify.Text = "";
                    drpdeliwin.SelectedValue = "0";
                    divadhoc.Style.Add("display", "none");
                    reqfieldorderduedate.Enabled = false;
                    RequiredFielddrpdeliwin.Enabled = false;
                    reqfieldtxtdaysnotify.Enabled = false;
                }
                else
                {
                    divadhoc.Style.Add("display", "block");
                    reqfieldorderduedate.Enabled = true;
                    RequiredFielddrpdeliwin.Enabled = true;
                    reqfieldtxtdaysnotify.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        public void rbotype()
        {
            ListItem lst = new ListItem();
            lst.Value = "0";
            lst.Text = "--Select--";
            VendorOverDue obj = new VendorOverDue();

            List<DeliveryWindow> lstdel = students;
            List<DeliveryWindow> lststr = new List<DeliveryWindow>();
            if (rbordertype.SelectedValue == "1")
            {
                lststr = lstdel.Take(7).ToList();
            }
            else if (rbordertype.SelectedValue == "2")
            {
                lststr = lstdel.Take(14).ToList();
            }
            else if (rbordertype.SelectedValue == "3")
            {
                lststr = lstdel.Take(28).ToList();
            }
            else if(rbordertype.SelectedValue == "4")
            {
                lststr = lstdel.Take(1).ToList();
            }

            if (lststr.Count > 0)
            {
                drpdeliwin.DataSource = lststr;
                drpdeliwin.DataTextField = "Text";
                drpdeliwin.DataValueField = "Value";
                drpdeliwin.DataBind();
                drpdeliwin.Items.Insert(0, lst);
                drpdeliwin.SelectedIndex = 0;
            }
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                //string CorporateId = null;
                //string FacilityId = null;
                //string VendorId = null;
                //string OrderType = null;
                //string SearchFilter = null;

                //if (drpcor.SelectedIndex > 0)
                //    CorporateId = drpcor.SelectedValue;

                //if (drpfacilitysearch.SelectedIndex > 0)
                //    FacilityId = drpfacilitysearch.SelectedValue;

                //if (drpfacilityven.SelectedIndex > 0)
                //    VendorId = drpfacilityven.SelectedValue;
                //if (rbordertype.SelectedIndex > 0)
                //    OrderType = rbordertype.SelectedValue;
                //txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                //txtDateTo.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString("MM/dd/yyyy");
                //List<GetVendorOrderDueReport> llstreview = new List<GetVendorOrderDueReport>();
                //llstreview = lclsService.GetVendorOrderDueReport(CorporateId, FacilityId, VendorId, Convert.ToDateTime(txtDateFrom.Text), Convert.ToDateTime(txtDateTo.Text), OrderType, SearchFilter, defaultPage.UserId).ToList();

                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();

                objbalvendorOrderDue.ListCorporateID = drpcor.SelectedValue;
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbalvendorOrderDue.ListFacilityID = "ALL";
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
                    objbalvendorOrderDue.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpVendorsearch.SelectedValue == "All")
                {
                    objbalvendorOrderDue.ListVendorID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpVendorsearch.Items)
                    {
                        if (lst.Selected && drpVendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbalvendorOrderDue.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalvendorOrderDue.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {

                    txtDateTo.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString();
                }
                else
                {
                    objbalvendorOrderDue.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                objbalvendorOrderDue.OrderTypestr = rbordertype.SelectedValue;
                objbalvendorOrderDue.Filter = "";
                objbalvendorOrderDue.LoggedInBy = defaultPage.UserId;
                List<GetVendorOrderDueReport> lstvendor = lclsService.GetVendorOrderDueReport(objbalvendorOrderDue).ToList();
                rvVendorOrderDuereport.ProcessingMode = ProcessingMode.Local;
                rvVendorOrderDuereport.LocalReport.ReportPath = Server.MapPath("~/Reports/VendorOrderDueReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("DSVendorOrderDueReport", lstvendor);
                rvVendorOrderDuereport.LocalReport.DataSources.Clear();
                rvVendorOrderDuereport.LocalReport.DataSources.Add(datasource);
                rvVendorOrderDuereport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvVendorOrderDuereport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);


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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                btnaddnew.Visible = true;
                ddrpcorporate.Enabled = true;
                ddrpfacility.Enabled = true;
                ddrpvendor.Enabled = true;
                rbordertype.Enabled = true;
                txtorderduedate.Enabled = true;
                txtdaysnotify.Enabled = true;
                drpdeliwin.Enabled = true;
                divadhoc.Style.Add("display", "block");
                divsearchbtn.Style.Add("display", "none");
                divorderdue.Style.Add("display", "none");
                divvendor.Style.Add("display", "none");
                adddiv.Style.Add("display", "block");
                divaddgrid.Style.Add("display", "none");
                btnreview.Visible = false;
                clearall();
                hdndeleteid.Value = "";
                hdnvendororderID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                closediv();
                hdnvendororderID.Value = "";
                hdneditororder.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
        }

        public void closediv()
        {
            try
            {
                divsearchbtn.Style.Add("display", "block");
                divorderdue.Style.Add("display", "block");
                divvendor.Style.Add("display", "block");
                divaddgrid.Style.Add("display", "none");
                adddiv.Style.Add("display", "none");
                clearall();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message), true);
            }
          
        }

        protected void ddrpcorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
        }

        protected void ddrpfacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendor(0, "Add");
        }

        protected void btnreview_Click(object sender, EventArgs e)
        {
            try
            {
                bindreview();
                mpereview.Show();

                //if (hdneditororder.Value == "")
                //{
                //    string Ordertype = string.Empty;
                //    string OrderNo = string.Empty;
                //    string errorlist = string.Empty;
                //    if (ddrpcorporate.SelectedValue == "0" || ddrpfacility.SelectedValue == "0" || ddrpfacility.SelectedValue == "" || ddrpvendor.SelectedValue == "0" || ddrpvendor.SelectedValue == "" || rbordertype.SelectedValue == "" )
                //    {
                //        errorlist = "Mandatory fields are should not be empty";
                //    }
                //    if (errorlist == "")
                //    {
                //        BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                //        objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                //        objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                //        if (rbordertype.SelectedValue == "4")
                //        {
                //            objbalvendorOrderDue.OrderDueDate = System.DateTime.Now;
                //        }
                //        else
                //        {
                //            if (txtorderduedate.Text != "")
                //                objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                //        }
                //        List<ValidateVendorOrderType> lstvendor = lclsService.ValidateVendorOrderType(objbalvendorOrderDue).ToList();
                //        if (lstvendor.Count != 0)
                //        {
                //            if (lstvendor[0].Ordernumber == Convert.ToInt32(rbordertype.SelectedValue))
                //            {
                //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorrecordMessage.Replace("<<VendorOrderDue>>", ""), true);
                //            }
                //            else if (lstvendor[0].Ordernumber != Convert.ToInt32(rbordertype.SelectedValue))
                //            {
                //                Ordertype = lstvendor[0].OrderType;
                //                hdndeleteid.Value = "1";
                //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderMessage.Replace("<<VendorOrderDue>>", Ordertype), true);
                //            }
                //        }
                //        else
                //        {
                //            hdndeleteid.Value = "";
                //            bindreview();
                //            mpereview.Show();
                //        }
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorrecordexistMessage.Replace("<<VendorOrderDue>>", errorlist), true);
                //    }
                //}
                //else
                //{
                //    hdndeleteid.Value = "";
                //    bindreview();
                //    mpereview.Show();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        public void bindreview()
        {
            try
            {
                lblCorp.Text = ddrpcorporate.SelectedItem.Text;
                lblFac.Text = ddrpfacility.SelectedItem.Text;
                lblVen.Text = ddrpvendor.SelectedItem.Text;
                lblordertype.Text = rbordertype.SelectedItem.Text;
                if(lblordertype.Text == "Ad-hoc")
                {
                    lblduedate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    lbldelivery.Text = lblduedate.Text;
                    lblnotifydate.Text = "1";
                }
                else
                {
                    lblduedate.Text = txtorderduedate.Text;
                    lbldelivery.Text = drpdeliwin.SelectedItem.Text;
                    lblnotifydate.Text = txtdaysnotify.Text;
                }
               

                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                dr = null;
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("FacilityShortName");
                dt.Columns.Add("VendorShortName");
                dt.Columns.Add("OrderdueDate");
                dt.Columns.Add("DeliveryDate");
                dt.Columns.Add("OrderType");
                dt.Columns.Add("DaysToNotify");

                foreach (GridViewRow row in grdaddvendor.Rows)
                {
                    dr = dt.NewRow();
                    dr["RowNumber"] = hdnvendororderID.Value;
                    dr["FacilityShortName"] = (row.FindControl("lbladdFacility") as Label).Text;
                    dr["VendorShortName"] = (row.FindControl("lbladdVendor") as Label).Text;
                    dr["OrderdueDate"] = (row.FindControl("txtoderDuedate") as TextBox).Text;
                    dr["DeliveryDate"] = (row.FindControl("txtDeliveryDate") as TextBox).Text;
                    dr["OrderType"] = (row.FindControl("lbladdOrderType") as Label).Text;
                    dr["DaysToNotify"] = (row.FindControl("txtToNotify") as TextBox).Text;
                    dt.Rows.Add(dr);
                }
                grdreview.DataSource = dt;
                grdreview.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        protected void btnaddnew_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                string Ordertype = string.Empty;
                string OrderNo = string.Empty;
                divaddgrid.Style.Add("display", "block");
                string errorlist = string.Empty;
                if(rbordertype.SelectedValue == "4")
                {
                    if (ddrpcorporate.SelectedValue == "0" || ddrpfacility.SelectedValue == "0" || ddrpfacility.SelectedValue == "" || ddrpvendor.SelectedValue == "0" || ddrpvendor.SelectedValue == "" || rbordertype.SelectedValue == "")
                    {
                        errorlist = "Mandatory fields are should not be empty";
                    }
                }
                else
                {
                    if (ddrpcorporate.SelectedValue == "0" || ddrpfacility.SelectedValue == "0" || ddrpfacility.SelectedValue == "" || ddrpvendor.SelectedValue == "0" || ddrpvendor.SelectedValue == "" || rbordertype.SelectedValue == "" || drpdeliwin.SelectedValue =="0"|| txtorderduedate.Text == "" || txtdaysnotify.Text=="")
                    {
                        errorlist = "Mandatory fields are should not be empty";
                    }
                }
                if (errorlist == "")
                {
                    BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                    objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                    objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                    if (txtorderduedate.Text != "")
                    {
                        objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                    }
                    else
                    {
                        objbalvendorOrderDue.OrderDueDate = System.DateTime.Now;
                    }
                    List<ValidateVendorOrderType> lstvendor = lclsService.ValidateVendorOrderType(objbalvendorOrderDue).ToList();

                    if (lstvendor.Count != 0)
                    {
                        if (lstvendor[0].Ordernumber == Convert.ToInt32(rbordertype.SelectedValue))
                        {
                            btnreview.Visible = false;
                            string msg = Constant.WarningVendorrecordMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<VendorOrderDue>>", "").Replace("');", "");
                            log.LogWarning(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorrecordMessage.Replace("<<VendorOrderDue>>", ""), true);
                        }
                        if(rbordertype.SelectedValue == "4")
                        {
                            if (lstvendor[0].Ordernumber != Convert.ToInt32(rbordertype.SelectedValue))
                            {
                                Ordertype = lstvendor[0].OrderType;
                                string msg = Constant.WarningVendoradhocMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<VendorOrderDue>>", "").Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendoradhocMessage.Replace("<<VendorOrderDue>>", Ordertype), true);
                            }
                        }
                        else if (lstvendor[0].Ordernumber != Convert.ToInt32(rbordertype.SelectedValue))
                        {
                            Ordertype = lstvendor[0].OrderType;
                            hdndeleteid.Value = "1";
                            string msg = Constant.WarningVendorOrderMessage.Replace("ShowwarningOrderLookupPopup('", "").Replace("<<VendorOrderDue>>", "").Replace("');", "");
                            log.LogWarning(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderMessage.Replace("<<VendorOrderDue>>", Ordertype), true);
                        }

                    }
                    else
                    {
                        bindaddgrid();
                    }
                }
                
                hdnvendororderID.Value = "";
                hdneditororder.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }

        }

        public void bindaddgrid()
        {
            try
            {
                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                if (hdnvendororderID.Value != "")
                    objbalvendorOrderDue.VendorOrderdueID = Convert.ToInt64(hdnvendororderID.Value);
                objbalvendorOrderDue.CorporateID = Convert.ToInt64(ddrpcorporate.SelectedValue);
                objbalvendorOrderDue.FacilityID = Convert.ToInt64(ddrpfacility.SelectedValue);
                objbalvendorOrderDue.VendorID = Convert.ToInt64(ddrpvendor.SelectedValue);
                objbalvendorOrderDue.OrderType = Convert.ToInt32(rbordertype.SelectedValue);
                if (rbordertype.SelectedValue == "4")
                {
                    objbalvendorOrderDue.OrderDueDate = System.DateTime.Now;
                }
                else
                {
                    if (txtorderduedate.Text != "")
                        objbalvendorOrderDue.OrderDueDate = Convert.ToDateTime(txtorderduedate.Text);
                }
                if (drpdeliwin.SelectedValue != "0")
                    objbalvendorOrderDue.DeliveryWindow = Convert.ToInt64(drpdeliwin.SelectedValue);
                if (txtdaysnotify.Text != "")
                    objbalvendorOrderDue.DaytoToNotify = Convert.ToInt32(txtdaysnotify.Text);
                List<GetVendorOrderDue> lstvendoradd = lclsService.GetVendorOrderDue(objbalvendorOrderDue).ToList();
                grdaddvendor.DataSource = lstvendoradd;
                grdaddvendor.DataBind();
                btnreview.Visible = true;

                foreach (GridViewRow row in grdaddvendor.Rows)
                {
                    TextBox txtDeliveryDate = (TextBox)row.FindControl("txtDeliveryDate");
                    TextBox txtToNotify = (TextBox)row.FindControl("txtToNotify");
                    if (rbordertype.SelectedValue == "4")
                    {
                        txtDeliveryDate.Enabled = false;
                        txtToNotify.Enabled = false;
                    }
                    else
                    {
                        txtDeliveryDate.Enabled = true;
                        txtToNotify.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
        }

        protected void btnclosepop_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BALVendorOrderDue objbalvendorOrderDue = new BALVendorOrderDue();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnvendororderID.Value = "";
                hdneditororder.Value= gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                ddrpcorporate.ClearSelection();
                if (gvrow.Cells[8].Text == "&nbsp;")
                {
                    ddrpcorporate.Items.FindByText("--Select Corporate--").Selected = true;
                }
                else
                {
                    ddrpcorporate.SelectedValue = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                }
                BindFacility(0, "Edit");
                ddrpfacility.ClearSelection();
                if (gvrow.Cells[9].Text == "&nbsp;")
                {
                    ddrpfacility.Items.FindByText("--Select Facility--").Selected = true;
                }
                else
                {
                    ddrpfacility.SelectedValue = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                }
                BindVendor(0, "Edit");
                ddrpvendor.ClearSelection();
                if (gvrow.Cells[10].Text == "&nbsp;")
                {
                    ddrpvendor.Items.FindByText("--Select Vendor--").Selected = true;
                }
                else
                {
                    ddrpvendor.SelectedValue = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                }

                if (gvrow.Cells[6].Text == "&nbsp;")
                {
                    rbordertype.ClearSelection();
                }
                else
                {
                    divadhoc.Style.Add("display", "block");
                    if (gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "") == "Weekly")
                    {
                        rbordertype.SelectedValue = "1";
                    }
                    if (gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "") == "Bi-Weekly")
                    {
                        rbordertype.SelectedValue = "2";
                    }
                    if (gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "") == "Monthly")
                    {
                        rbordertype.SelectedValue = "3";
                    }
                    if (gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "") == "Ad-hoc")
                    {
                        rbordertype.SelectedValue = "4";
                        divadhoc.Style.Add("display", "none");
                    }
                    
                }

                txtorderduedate.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                txtdaysnotify.Text = gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "");
                rbotype();
                drpdeliwin.ClearSelection();
                if (gvrow.Cells[11].Text == "&nbsp;")
                {
                    drpdeliwin.Items.FindByText("--Select--").Selected = true;
                }
                else
                {
                    drpdeliwin.SelectedValue = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                }

                objbalvendorOrderDue.ListFacilityID = ddrpfacility.SelectedValue;
                objbalvendorOrderDue.ListVendorID = ddrpvendor.SelectedValue;
                objbalvendorOrderDue.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                objbalvendorOrderDue.DateTo = Convert.ToDateTime(txtDateTo.Text);
                objbalvendorOrderDue.Filter = "";
                objbalvendorOrderDue.LoggedInBy = defaultPage.UserId;

                List<SearchVendorOrderType> firstlstvendor = new List<SearchVendorOrderType>();
                List<SearchVendorOrderType> finallstvendor = new List<SearchVendorOrderType>();

                List<SearchVendorOrderType> lstvendor = lclsService.SearchVendorOrderType(objbalvendorOrderDue).ToList();

                firstlstvendor = lclsService.SearchVendorOrderType(objbalvendorOrderDue).Where(b => b.VenOrderDueID == Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""))).ToList();

                finallstvendor = lclsService.SearchVendorOrderType(objbalvendorOrderDue).Where(b => b.VenOrderDueID != Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""))).ToList();

                finallstvendor = firstlstvendor.Concat(finallstvendor).ToList();

                grdaddvendor.DataSource = finallstvendor;
                grdaddvendor.DataBind();



                divsearchbtn.Style.Add("display", "none");
                divorderdue.Style.Add("display", "none");
                divvendor.Style.Add("display", "none");
                adddiv.Style.Add("display", "block");
                divaddgrid.Style.Add("display", "block");
                btnaddnew.Visible = false;
                ddrpcorporate.Enabled = false;
                ddrpfacility.Enabled = false;
                ddrpvendor.Enabled = false;
                rbordertype.Enabled = false;
                txtorderduedate.Enabled = false;
                txtdaysnotify.Enabled = false;
                drpdeliwin.Enabled = false;
                btnreview.Visible = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }

        }
        protected void btnorderreview_Click(object sender, EventArgs e)
        {
            try
            {
                divaddgrid.Style.Add("display", "block");
                bindaddgrid();

                //bindreview();
                //mpereview.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueErrorMessage.Replace("<<VendorOrderDue>>", ex.Message.ToString()), true);
            }
          
        }
    }
}