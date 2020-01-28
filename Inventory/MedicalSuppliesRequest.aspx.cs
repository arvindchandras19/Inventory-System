using Inventory.Class;
using Inventory.Inventoryserref;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<MachinePartsRequestOrder>>
'' Type      :   C# File
'' Description  :<<To add,update the Medicalsupplies Request Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
 *  09/14/2018         V1.0                Dhanasekaran.C                         New 
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class MedicalSuppliesRequest : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALMedicalSuppliesRequest llstMedicalSupplyRequest = new BALMedicalSuppliesRequest();
        private string _sessionPDFFileName;
        string actionOrder = Constant.actionOrder;
        string actionApprove = Constant.actionApprove;
        string actionDeny = Constant.actionDeny;
        string actionHold = Constant.actionHold;
        string StatusOrder = Constant.OrderStatus;
        string StatusApprove = Constant.PendingOrderStatus;
        string StatusDeny = Constant.DeniedStatus;
        string StatusHold = Constant.HoldStatus;
        string FinalString = "";
        string loadshipping = Constant.loadshipping;
        string ErrorList = string.Empty;
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgstr = Constant.WarningMedsupplyreqNoRecordsItemGrid.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msg1 = Constant.WarningMedsupplyreqNoRecordsReview.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msg2 = Constant.WarningMedsupplyreqItemNotMapped.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msg3 = Constant.WarningMedsupplyAlreadyExitsTime.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msg4 = Constant.WarningMedsupplyReqTime.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        StringBuilder SB = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnreviewprint);
                scriptManager.RegisterPostBackControl(this.grdMedReqSearch);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CorpDrop();", true);


                HdnQueryStringID.Value = Request.QueryString["MedicalsupplyID"];
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        BindCorporate(1, "Add");
                        //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility(1, "Add");
                        //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindVendor(1, "Add");
                        BindStatus("Add");
                        if (HdnQueryStringID.Value != "")
                        {
                            PageloadFunction();
                            PurchaseOrderEditFunction(Convert.ToInt64(HdnQueryStringID.Value));
                            divgrdMSRSearch.Style.Add("display", "None");
                            //lblseroutHeader.Style.Add("display", "None");  
                            //divpr.Style.Add("display", "None");
                        }
                        else
                        {
                            PageloadFunction();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void POFunction()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void PageloadFunction()
        {
            try
            {
                SearchGrid();

                if (defaultPage.Req_MedicalSuppliesPage_Edit == false && defaultPage.Req_MedicalSuppliesPage_View == true)
                {
                    btnSave.Visible = false;
                }
                if (defaultPage.Req_MedicalSuppliesPage_Edit == false && defaultPage.Req_MedicalSuppliesPage_View == false)
                {
                    updmain.Visible = false;
                    User_Permission_Message.Visible = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }

        }
        #region Bind MedicalSupplies Request Master Values
        public void BindGrid()
        {
            try
            {
                SearchGrid();
                //List<GetMedicalSupplyRequestMaster> lstMSRMaster = lclsservice.GetMedicalSupplyRequestMaster().ToList();
                //grdMedReqSearch.DataSource = lstMSRMaster;
                //grdMedReqSearch.DataBind();
                //ViewState["ReportMedSupplyID"] = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void BindGridNonSuperAdminMSR(Int64 CorporateID, Int64 FacilityID)
        {
            try
            {
                List<GetNonsuperAdminMedicalSupplyMaster> lstMSRMaster = lclsservice.GetNonsuperAdminMedicalSupplyMaster(CorporateID, FacilityID).ToList();
                grdMedReqSearch.DataSource = lstMSRMaster;
                grdMedReqSearch.DataBind();
                ViewState["ReportMedSupplyID"] = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        #endregion

        #region Bind Search MedicalSuppliesRequest Master Values
        public void SearchGrid()
        {
            try
            {
                BALMedicalSuppliesRequest llstMachineSearch = new BALMedicalSuppliesRequest();

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstMachineSearch.CorporateName = "ALL";
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

                    llstMachineSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstMachineSearch.FacilityName = "ALL";
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
                    llstMachineSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstMachineSearch.VendorName = "ALL";
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
                List<SearchMedicalSupplyRequest> lstMSRMaster = lclsservice.SearchMedicalSupplyRequest(llstMachineSearch).ToList();
                grdMedReqSearch.DataSource = lstMSRMaster;
                grdMedReqSearch.DataBind();
                ViewState["ReportMedSupplyID"] = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        #endregion

        protected void grdMedReqSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblAudit = (Label)e.Row.FindControl("lblaudit");
                    Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                    Image imgreadmoreaudit = (Image)e.Row.FindControl("imgreadmoreaudit");
                    ImageButton imgbtnEdit = (e.Row.FindControl("imgbtnEdit") as ImageButton);
                    //if (lblRemarks.Text.Length > 150)
                    //{
                    //    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                    //    imgreadmore.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmore.Visible = false;
                    //}
                    //if (lblAudit.Text.Length > 150)
                    //{
                    //    lblAudit.Text = lblAudit.Text.Substring(0, 150) + "....";
                    //    imgreadmoreaudit.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmoreaudit.Visible = false;
                    //}
                    string Status = e.Row.Cells[7].Text;
                    if (Status == StatusOrder)
                    {
                        imgbtnEdit.Visible = false;
                    }
                    else if (Status == StatusDeny)
                    {
                        imgbtnEdit.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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
                DivMedicalRequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            DivMedicalRequest.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                DivMedicalRequest.Attributes["class"] = "Upopacity";
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
                           
                            foreach(GridViewRow row in GrdMultiCorp.Rows)
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }


        protected void lnkClearCorp_Click(object sender, EventArgs e)
        {
            try
            {
                BindCorporate(1, "Add");
                BindFacility(1, "Add");
                BindVendor(1, "Add");
                HddListCorpID.Value = "";
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          
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
                        ddlCorporateadd.DataSource = lstfacility;
                        ddlCorporateadd.DataTextField = "CorporateName";
                        ddlCorporateadd.DataValueField = "CorporateID";
                        ddlCorporateadd.DataBind();
                        ddlCorporateadd.Items.Insert(0, lstDDl);
                        ddlCorporateadd.SelectedIndex = 0;
                    }
                    else
                    {
                        lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        ddlCorporateadd.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        ddlCorporateadd.DataTextField = "CorporateName";
                        ddlCorporateadd.DataValueField = "CorporateID";
                        ddlCorporateadd.DataBind();
                        ddlCorporateadd.Items.Insert(0, lstDDl);
                        ddlCorporateadd.SelectedIndex = 0;
                    }
                }
                if (mode == "Edit")
                {
                    ddlCorporateadd.DataSource = lclsservice.GetCorporateMaster().ToList();
                    ddlCorporateadd.DataTextField = "CorporateName";
                    ddlCorporateadd.DataValueField = "CorporateID";
                    ddlCorporateadd.DataBind();
                    ddlCorporateadd.Items.Insert(0, lstDDl);
                    ddlCorporateadd.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        #endregion



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
                DivMedicalRequest.Attributes["class"] = "mypanel-body";
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
            DivFacCorp.Style.Add("display", "none");
            DivMedicalRequest.Attributes["class"] = "mypanel-body";
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
                    DivMedicalRequest.Attributes["class"] = "Upopacity";
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


        protected void lnkClearFac_Click(object sender, EventArgs e)
        {
            BindFacility(1, "Add");
        }



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
                            ddlFacilityadd.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(ddlCorporateadd.SelectedValue)).Where(a => a.IsActive == true).ToList();
                            ddlFacilityadd.DataTextField = "FacilityDescription";
                            ddlFacilityadd.DataValueField = "FacilityID";
                            ddlFacilityadd.DataBind();
                            ddlFacilityadd.Items.Insert(0, lst);
                            ddlFacilityadd.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlFacilityadd.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == ddlCorporateadd.SelectedItem.Text).ToList();
                            ddlFacilityadd.DataTextField = "FacilityName";
                            ddlFacilityadd.DataValueField = "FacilityID";
                            ddlFacilityadd.DataBind();
                            ddlFacilityadd.Items.Insert(0, lst);
                            ddlFacilityadd.SelectedIndex = 0;
                        }
                    }

                    else if (mode == "Edit")
                    {
                        ddlFacilityadd.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(ddlCorporateadd.SelectedValue)).ToList();
                        ddlFacilityadd.DataTextField = "FacilityDescription";
                        ddlFacilityadd.DataValueField = "FacilityID";
                        ddlFacilityadd.DataBind();
                        ddlFacilityadd.Items.Insert(0, lst);
                        ddlFacilityadd.SelectedIndex = 0;
                    }

                }
                BindVendor(1, "Add");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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
                        lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => (a.RegularSupplies == true)).Distinct().ToList();

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
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacilityadd.SelectedItem.Text).Where(A => (A.RegularSupplies == true) && (A.IsActive == true)).ToList();
                        ddlVendoradd.DataSource = lstvendordetails;
                        ddlVendoradd.DataTextField = "VendorDescription";
                        ddlVendoradd.DataValueField = "VendorID";
                        ddlVendoradd.DataBind();
                        ddlVendoradd.Items.Insert(0, lst);
                        ddlVendoradd.SelectedIndex = 0;
                    }
                    else if (mode == "Edit")
                    {
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacilityadd.SelectedItem.Text).Where(A => A.RegularSupplies == true).ToList();
                        ddlVendoradd.DataSource = lstvendordetails;
                        ddlVendoradd.DataTextField = "VendorDescription";
                        ddlVendoradd.DataValueField = "VendorID";
                        ddlVendoradd.DataBind();
                        ddlVendoradd.Items.Insert(0, lst);
                        ddlVendoradd.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void BindOrderPeriod()
        {
            try
            {
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                List<GetOrderPeriod> lstvendordetails = new List<GetOrderPeriod>();
                Int64 Vendor = 0;
                if (ddlVendoradd.SelectedValue == "")
                {
                    Vendor = 0;
                }
                else
                {
                    Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                }
                lstvendordetails = lclsservice.GetOrderPeriod(Convert.ToInt64(ddlCorporateadd.SelectedValue), Convert.ToInt64(ddlFacilityadd.SelectedValue), Vendor, Convert.ToInt32(rdovendortype.SelectedValue)).ToList();
                if (lstvendordetails.Count > 0)
                {
                    drporderperiod.DataSource = lstvendordetails;
                    drporderperiod.DataTextField = "OrderdueDate";
                    drporderperiod.DataValueField = "VenOrderDueID";
                    drporderperiod.DataBind();
                }
                else
                {
                    drporderperiod.Items.Clear();
                    ListItem lstorder = new ListItem();
                    lstorder.Value = "0";
                    lstorder.Text = "No OrderPeriod For Selected OrderType";
                    drporderperiod.Items.Insert(0, lstorder);
                    drporderperiod.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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
                List<GetList> lstLookUp = new List<GetList>();
                string SearchText = string.Empty;
                // Search Status Drop Down
                lstLookUp = lclsservice.GetList("MedicalSuppliesRequest", "Status", Mode).ToList();
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatus.Items.Insert(0, lst);
                drpStatus.Items.FindByText(StatusApprove).Selected = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }

        }

        public void BindShipping(string Mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                ListItem lstShip = new ListItem();
                lstShip.Value = "0";
                lstShip.Text = "--Select Shipping--";
                // Insert Shipping Drop Down
                lstLookUp = lclsservice.GetList("MedicalSuppliesRequest", "Shipping", Mode).ToList();
                drpshipping.DataSource = lstLookUp;
                drpshipping.DataTextField = "InvenValue";
                drpshipping.DataValueField = "InvenValue";
                drpshipping.DataBind();
                drpshipping.Items.FindByText(loadshipping).Selected = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void BindTimeDelivery(string Mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                ListItem lstShip = new ListItem();
                lstShip.Value = "0";
                lstShip.Text = "--Select TimeDelivery--";
                lstLookUp = lclsservice.GetList("MedicalSuppliesRequest", "TimeDelivery", Mode).ToList();
                drptimedelivery.DataSource = lstLookUp;
                drptimedelivery.DataTextField = "InvenValue";
                drptimedelivery.DataValueField = "InvenValue";
                drptimedelivery.DataBind();
                drptimedelivery.Items.Insert(0, lstShip);
                drptimedelivery.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        public void BindVendorOrderType()
        {
            try
            {
                List<GetLookUpList> lstLookUp = new List<GetLookUpList>();
                lstLookUp = lclsservice.GetLookUpList(6).ToList();
                rdovendortype.DataSource = lstLookUp;
                rdovendortype.DataTextField = "InvenValue";
                rdovendortype.DataValueField = "InvenValue";
                rdovendortype.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void BindAddMedicalsupplies()
        {
            try
            {
                List<GetMedicalsuppliesItem> lstmedgrid = new List<GetMedicalsuppliesItem>();
                List<CheckVendorOrderDue> lstvendororder = new List<CheckVendorOrderDue>();
                Int64 Vendor = 0;
                if (ddlVendoradd.SelectedValue == "")
                {
                    Vendor = 0;
                }
                else
                {
                    Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                }

                lstmedgrid = lclsservice.GetMedicalsuppliesItem(Convert.ToInt64(ddlCorporateadd.SelectedValue), Convert.ToInt64(ddlFacilityadd.SelectedValue), Vendor).ToList();
                lstvendororder = lclsservice.CheckVendorOrderDue(Convert.ToInt64(ddlCorporateadd.SelectedValue), Convert.ToInt64(ddlFacilityadd.SelectedValue), Vendor).ToList();

                if(lstmedgrid.Count > 0 && lstvendororder.Count > 0)
                {
                    if (lstmedgrid.Count > 0 && lstvendororder[0].OrderType != "Ad-hoc")
                    {
                        txtadhocorder.Text = "";
                        rdovendortype.Enabled = true;
                        txtadhocorder.Enabled = true;
                        grdMedReqadd.DataSource = lstmedgrid;
                        grdMedReqadd.DataBind();
                    }
                    else if(lstvendororder[0].OrderType == "Ad-hoc")
                    {
                        rdovendortype.ClearSelection();
                        rdovendortype.SelectedValue = "4";
                        divorderperiod.Style.Add("display", "none");
                        divOrderAdhoc.Style.Add("display", "block");
                        ReqdrporderperiodAdhoc.Visible = true;
                        ReqdrporderperiodAdhoc.ValidationGroup = "EmptyFieldAdd";
                        Reqdrporderperiod.Visible = false;
                        Reqdrporderperiod.ValidationGroup = "";
                        txtadhocorder.Text = "";
                        txtadhocorder.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                        txtadhocorder.Enabled = false;
                        rdovendortype.Enabled = false;
                        grdMedReqadd.DataSource = lstmedgrid;
                        grdMedReqadd.DataBind();
                    }
                    else
                    {
                        txtadhocorder.Text = "";
                        rdovendortype.Enabled = true;
                        txtadhocorder.Enabled = true;
                        grdMedReqadd.DataSource = lstmedgrid;
                        grdMedReqadd.DataBind();
                    }

                }
                else
                {
                    txtadhocorder.Text = "";
                    rdovendortype.Enabled = true;
                    txtadhocorder.Enabled = true;
                    grdMedReqadd.DataSource = lstmedgrid;
                    grdMedReqadd.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        #endregion


        protected void ddlFacilityadd_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendor(0, "Add");
        }

        protected void ddlCorporateadd_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
        }
        //protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindVendor(1, "Add");
        //}

        //protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindFacility(1, "Add");
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lclsservice.SyncMedicalSuppliesReceivingorder();
                ViewState["ReportMedSupplyID"] = "";
                lblUpdateHeader.Visible = true;
                lblEditHeader.Visible = false;
                lblseroutHeader.Visible = false;
                divSearchContent.Style.Add("display", "none");
                divgrdMSRSearch.Style.Add("display", "none");
                divcount.Style.Add("display", "none");
                divMSRContentAddHeader.Style.Add("display", "block");
                divContentgrdAdd.Style.Add("display", "block");
                divContentgrdEdit.Style.Add("display", "None");
                btnAdd.Visible = false;
                btnSearch.Visible = false;
                EnableEditControls();
                if (defaultPage.Req_MedicalSuppliesPage_Edit == false && defaultPage.Req_MedicalSuppliesPage_View == true)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                }
                btnClose.Visible = true;
                // btnPrint.Visible = false;
                btnreviewprint.Visible = false;
                btnReview.Visible = true;
                if (drpcorsearch.SelectedValue == "All")
                {
                    ddlCorporateadd.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                }
                else
                {
                    ddlCorporateadd.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                }
                BindFacility(0, "Add");
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    ddlFacilityadd.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                }
                else
                {
                    ddlFacilityadd.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                }
                BindVendor(0, "Add");
                ddlVendoradd.ClearSelection();
                //if (Convert.ToString(drpvendorsearch.SelectedValue) != "All")
                //    ddlVendoradd.SelectedValue = Convert.ToString(drpvendorsearch.SelectedValue);
                BindAddMedicalsupplies();
                BindStatus("Add");
                BindShipping("Add");
                BindTimeDelivery("Add");
                ClearAdd();
                divorderperiod.Style.Add("display", "block");
                divshipping.Style.Add("display", "block");
                divtimedelivery.Style.Add("display", "none");
                divOrderAdhoc.Style.Add("display", "none");
                ViewState["MedicalsupplyMasterID"] = 0;
                Reqdrporderperiod.Visible = true;
                Reqdrporderperiod.ValidationGroup = "EmptyFieldAdd";
                ReqdrporderperiodAdhoc.Visible = false;
                ReqdrporderperiodAdhoc.ValidationGroup = "";
                lbladdfoottotal.Text = "0";
                lbleditfoottotal.Text = "0";
                btnreviewprint.Visible = false;
                lbpopprint.Visible = false;
                grdmedsupitemedit.DataSource = null;
                grdmedsupitemedit.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void imgeshipadd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // drpshipping.SelectedIndex = 0;
                mpeshipping.Show();
                txtshipping.Text = "";
                txtshipping.Enabled = true;
                txtshipping.Focus();
                btnaddshipping.Text = "Save";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          

        }

        protected void imgeshipedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mpeshipping.Show();
                EventLogger log = new EventLogger(config);
                if (drpshipping.SelectedValue == "0")
                {
                    string msg = Constant.WarningMedsupplyReqShipping.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyReqShipping.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    mpeshipping.Hide();
                }
                else
                {
                    txtshipping.Text = drpshipping.SelectedValue;
                    ViewState["InvenValue"] = txtshipping.Text;
                    btnaddshipping.Text = "Update";
                    txtshipping.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
           

        }

        protected void imgeshipdelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mpeshipping.Show();
                EventLogger log = new EventLogger(config);
                if (drpshipping.SelectedValue == "0")
                {
                    string msg = Constant.WarningMedsupplyReqShipping.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyReqShipping.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    mpeshipping.Hide();
                }
                else
                {
                    txtshipping.Text = drpshipping.SelectedValue;
                    btnaddshipping.Text = "Delete";
                    txtshipping.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          
        }
        protected void btnaddshipping_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                BALMedicalSuppliesRequest objmedsupplyrequest = new BALMedicalSuppliesRequest();
                Functions objfun = new Functions();
                string Medsupply = string.Empty;
                objmedsupplyrequest.InvenValue = txtshipping.Text;
                objmedsupplyrequest.Updatekeyvalue = Convert.ToString(ViewState["InvenValue"]);
                objmedsupplyrequest.CreatedBy = defaultPage.UserId;
                objmedsupplyrequest.DeletedBy = defaultPage.UserId;
                if (Convert.ToString(ViewState["InvenValue"]) == "" && btnaddshipping.Text != "Update" && btnaddshipping.Text != "Delete")
                {
                    List<FindDuplicateShippingValue> lstshipping = new List<FindDuplicateShippingValue>();
                    lstshipping = lclsservice.FindDuplicateShippingValue(txtshipping.Text).ToList();
                    if (lstshipping[0].Shipping == 0)
                    {
                        Medsupply = lclsservice.InsertShipping(objmedsupplyrequest);
                        if (Medsupply == "Saved Successfully")
                        {
                            string msg = Constant.MedicalSuppliesRequestShippingSaveMessage.Replace("ShowPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestShippingSaveMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        }
                    }
                    else
                    {
                        string msg = Constant.WarningMedsupplyAlreadyExitsShipping.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyAlreadyExitsShipping.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        Medsupply = "Deleted Successfully";
                    }
                }
                else if (btnaddshipping.Text == "Delete")
                {
                    btnaddshipping.Text = "Save";
                    Medsupply = lclsservice.DeleteShipping(objmedsupplyrequest);
                    if (Medsupply == "Deleted Successfully")
                    {
                        string msg = Constant.MedicalSuppliesReqShiptDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReqShiptDeleteMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    }
                }
                else
                {
                    List<FindDuplicateShippingValue> lstshipping = new List<FindDuplicateShippingValue>();
                    lstshipping = lclsservice.FindDuplicateShippingValue(txtshipping.Text).ToList();
                    if (lstshipping[0].Shipping == 0)
                    {
                        Medsupply = lclsservice.UpdateShipping(objmedsupplyrequest);
                        if (Medsupply == "Saved Successfully")
                        {
                            string msg = Constant.MedicalSuppliesRequestShippingUpdateMessage.Replace("ShowPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestShippingUpdateMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                            ViewState["InvenValue"] = "";
                            btnaddshipping.Text = "Save";
                        }
                    }
                    else
                    {
                        string msg = Constant.WarningMedsupplyAlreadyExitsShipping.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyAlreadyExitsShipping.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        Medsupply = "Deleted Successfully";
                    }
                }
                BindShipping("Add");
                List<SavedShippingValue> lstship = new List<SavedShippingValue>();
                lstship = lclsservice.SavedShippingValue().ToList();
                if (lstship.Count > 0 && Medsupply != "Deleted Successfully")
                {
                    drpshipping.SelectedValue = lstship[0].InvenValue;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void drpshipping_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                if (drpshipping.SelectedValue == "Time Delivery")
                {
                    divtimedelivery.Style.Add("display", "block");
                    Reqdrptimedelivery.Visible = true;
                    Reqdrptimedelivery.ValidationGroup = "EmptyFieldAdd";
                    rdovendortype.Enabled = true;
                }
                else
                {
                    divtimedelivery.Style.Add("display", "none");
                }
                AddGrandTotalcalculation();
                EditGrandTotalcalculation();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
           

        }

        protected void btnshipclose_Click(object sender, EventArgs e)
        {
            mpeshipping.Hide();
        }

        protected void imgtimedeladd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //drptimedelivery.SelectedIndex = 0;
                mpetimedel.Show();
                txttimedel.Text = "";
                txttimedel.Enabled = true;
                txttimedel.Focus();
                btntimedelsave.Text = "Save";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
         
        }

        protected void imgtimedeledit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mpetimedel.Show();
                if (drptimedelivery.SelectedValue == "0")
                {
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msg4.Replace("<<MedicalSupplyRequestDescription>>", ""));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyReqTime.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    mpetimedel.Hide();
                }
                else
                {
                    txttimedel.Text = drptimedelivery.SelectedValue;
                    ViewState["InventimeValue"] = txttimedel.Text;
                    btntimedelsave.Text = "Update";
                    txttimedel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          
        }

        protected void imgtimedldel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mpetimedel.Show();
                if (drptimedelivery.SelectedValue == "0")
                {
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msg4.Replace("<<MedicalSupplyRequestDescription>>", ""));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyReqTime.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    mpetimedel.Hide();
                }
                else
                {
                    txttimedel.Text = drptimedelivery.SelectedValue;
                    btntimedelsave.Text = "Delete";
                    txttimedel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          
        }

        protected void btntimedelsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                BALMedicalSuppliesRequest objmedsupplyrequest = new BALMedicalSuppliesRequest();
                Functions objfun = new Functions();
                string Medtimedel = string.Empty;
                objmedsupplyrequest.InvenValue = txttimedel.Text;
                objmedsupplyrequest.Updatekeyvalue = Convert.ToString(ViewState["InventimeValue"]);
                objmedsupplyrequest.CreatedBy = defaultPage.UserId;
                objmedsupplyrequest.DeletedBy = defaultPage.UserId;
                if (Convert.ToString(ViewState["InventimeValue"]) == "" && btntimedelsave.Text != "Update" && btntimedelsave.Text != "Delete")
                {
                    List<FindDuplicateTimeDeliveryValue> lstTimeDelivery = new List<FindDuplicateTimeDeliveryValue>();
                    lstTimeDelivery = lclsservice.FindDuplicateTimeDeliveryValue(txttimedel.Text).ToList();
                    if (lstTimeDelivery[0].Shipping == 0)
                    {
                        Medtimedel = lclsservice.InsertTimeDelivery(objmedsupplyrequest);
                        if (Medtimedel == "Saved Successfully")
                        {
                            log.LogWarning(msg3.Replace("<<MedicalSupplyRequestDescription>>", ""));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestTimeSaveMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        }
                    }
                    else
                    {
                      
                        log.LogWarning(msg3.Replace("<<MedicalSupplyRequestDescription>>", ""));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyAlreadyExitsTime.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        Medtimedel = "Deleted Successfully";
                    }
                }
                else if (btntimedelsave.Text == "Delete")
                {
                    btntimedelsave.Text = "Save";
                    Medtimedel = lclsservice.DeleteTimeDelivery(objmedsupplyrequest);
                    if (Medtimedel == "Deleted Successfully")
                    {
                        string msg = Constant.MedicalSuppliesReqTimetDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReqTimetDeleteMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    }
                }
                else
                {
                    List<FindDuplicateTimeDeliveryValue> lstTimeDelivery = new List<FindDuplicateTimeDeliveryValue>();
                    lstTimeDelivery = lclsservice.FindDuplicateTimeDeliveryValue(txttimedel.Text).ToList();
                    if (lstTimeDelivery[0].Shipping == 0)
                    {
                        Medtimedel = lclsservice.UpdateTimeDelivery(objmedsupplyrequest);
                        if (Medtimedel == "Saved Successfully")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestTimeUpdateMessage.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                            ViewState["InventimeValue"] = "";
                            btntimedelsave.Text = "Save";
                        }
                    }
                    else
                    {
                        string msg = Constant.WarningMedsupplyAlreadyExitsTime.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyAlreadyExitsTime.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        Medtimedel = "Deleted Successfully";
                    }
                }
                BindTimeDelivery("Add");
                List<SavedTimeDeliveryValue> lsttimedel = new List<SavedTimeDeliveryValue>();
                lsttimedel = lclsservice.SavedTimeDeliveryValue().ToList();
                if (lsttimedel.Count > 0 && Medtimedel != "Deleted Successfully")
                {
                    drptimedelivery.SelectedValue = lsttimedel[0].InvenValue;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void rdovendortype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                if (rdovendortype.SelectedValue == "4")
                {
                    divorderperiod.Style.Add("display", "none");
                    divOrderAdhoc.Style.Add("display", "block");
                    ReqdrporderperiodAdhoc.Visible = true;
                    ReqdrporderperiodAdhoc.ValidationGroup = "EmptyFieldAdd";
                    Reqdrporderperiod.Visible = false;
                    Reqdrporderperiod.ValidationGroup = "";
                    txtadhocorder.Text = "";
                    BindOrderPeriod();
                }
                else
                {
                    divorderperiod.Style.Add("display", "block");
                    divOrderAdhoc.Style.Add("display", "none");
                    ReqdrporderperiodAdhoc.Visible = false;
                    ReqdrporderperiodAdhoc.ValidationGroup = "";
                    Reqdrporderperiod.Visible = true;
                    Reqdrporderperiod.ValidationGroup = "EmptyFieldAdd";
                }
                BindOrderPeriod();
                AddGrandTotalcalculation();
                EditGrandTotalcalculation();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
         
        }

        protected void txtadhocorder_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                AddGrandTotalcalculation();
                EditGrandTotalcalculation();
                List<GetOrderPeriod> lstvendordetails = new List<GetOrderPeriod>();
                lstvendordetails = lclsservice.GetOrderPeriod(Convert.ToInt64(ddlCorporateadd.SelectedValue), Convert.ToInt64(ddlFacilityadd.SelectedValue), Convert.ToInt64(ddlVendoradd.SelectedValue), Convert.ToInt16(rdovendortype.SelectedValue)).ToList();
                string AdhocOrderPeriod = string.Empty;
                string UniqueString = string.Empty;
                foreach (var i in lstvendordetails)
                {
                    AdhocOrderPeriod += Convert.ToString(i.OrderType) + ',';
                    List<string> uniqueValues = AdhocOrderPeriod.ToLower().Split(',').Distinct().ToList();
                    UniqueString = string.Join(",", uniqueValues);
                }
                UniqueString = UniqueString.Remove(UniqueString.Length - 1);
                DateTime today = DateTime.Today;
                string NextDate = string.Empty;
                switch (UniqueString)
                {
                    case "1":
                        NextDate = DateTime.Now.AddDays(7).ToShortDateString();
                        break;
                    case "2":
                        NextDate = DateTime.Now.AddDays(14).ToShortDateString();
                        break;
                    case "3":
                        NextDate = DateTime.Now.AddDays(28).ToShortDateString();
                        break;
                    case "1,2,3":
                        NextDate = DateTime.Now.AddDays(28).ToShortDateString();
                        break;
                    case "1,2":
                        NextDate = DateTime.Now.AddDays(14).ToShortDateString();
                        break;
                    case "1,3":
                        NextDate = DateTime.Now.AddDays(28).ToShortDateString();
                        break;
                    case "2,3":
                        NextDate = DateTime.Now.AddDays(28).ToShortDateString();
                        break;
                    default:
                        today = DateTime.Now;
                        break;
                }
                DateTime Nextadhocdate = Convert.ToDateTime(NextDate);
                DateTime txtadhocdate = Convert.ToDateTime(txtadhocorder.Text);
                if (txtadhocdate <= Nextadhocdate)
                {
                    txtadhocorder.Text = NextDate.ToString();
                }
                else
                {
                    string msg = Constant.WarningMedsupplyreqOrderPeriodExists.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqOrderPeriodExists.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALMedicalSuppliesRequest objmedsupplyrequest = new BALMedicalSuppliesRequest();
                Functions objfun = new Functions();
                string meditem = string.Empty;
                Int64 medsave = 0;
                int rowcount = grdMedReqadd.Rows.Count;
                int ReviewCount = gvmedreview.Rows.Count;
                if (Convert.ToInt64(ViewState["MedicalsupplyMasterID"]) == 0)
                {
                    if (rowcount != 0)
                    {
                        if (ReviewCount != 0)
                        {
                            objmedsupplyrequest.CorporateID = Convert.ToInt64(ddlCorporateadd.SelectedValue);
                            objmedsupplyrequest.FacilityID = Convert.ToInt64(ddlFacilityadd.SelectedValue);
                            objmedsupplyrequest.Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                            objmedsupplyrequest.OrderType = Convert.ToInt32(rdovendortype.SelectedValue);
                            if (rdovendortype.SelectedValue == "4")
                            {
                                objmedsupplyrequest.OrderPeriod = Convert.ToDateTime(txtadhocorder.Text);
                            }
                            else
                            {
                                if (drporderperiod.SelectedItem.Text == "No OrderPeriod For Selected OrderType")
                                {
                                    drporderperiod.SelectedItem.Text = "";
                                }
                                else
                                {
                                    // Date time to change Check
                                    objmedsupplyrequest.OrderPeriod = Convert.ToDateTime(drporderperiod.SelectedItem.Text);
                                }
                            }
                            objmedsupplyrequest.Shipping = Convert.ToString(drpshipping.SelectedValue);
                            objmedsupplyrequest.TimeDelivery = Convert.ToString(drptimedelivery.SelectedValue);
                            //objmedsupplyrequest.Status = "Pending";
                            objmedsupplyrequest.Remarks = "";
                            objmedsupplyrequest.CreatedBy = defaultPage.UserId;
                            objmedsupplyrequest.LastModifiedBy = 0;
                            // string s = Convert.ToString(ViewState["MSRDetailID"]);                            
                            string s = Convert.ToString(HdnMSRDetailID.Value);
                            if (s != "")
                            {
                                medsave = lclsservice.InsertMedicalsupplyMaster(objmedsupplyrequest);

                                s = s.Substring(0, s.Length - 1);
                                string[] values = s.Split(',');
                                string[] c = values.Distinct().ToArray();
                                for (int i = 0; i < c.Length; i++)
                                {
                                    GridViewRow row = grdMedReqadd.Rows[Convert.ToInt32(c[i])];
                                    TextBox txtqihand = (TextBox)row.FindControl("txtqihand");
                                    Label lblItemId = (Label)row.FindControl("lblItemId");
                                    Label lblCategoryName = (Label)row.FindControl("lblCategoryName");
                                    Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                                    Label lblUOM = (Label)row.FindControl("lblUOM");
                                    Label lbluomvalue = (Label)row.FindControl("lbluomvalue");
                                    Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                                    Label lblParlevel = (Label)row.FindControl("lblParlevel");
                                    Label lbloqty = (Label)row.FindControl("lbloqty");
                                    Label lblPrice = (Label)row.FindControl("lblPrice");
                                    Label lbltotprice = (Label)row.FindControl("lbltotprice");
                                    if ((txtqihand.Text != "") && (lbloqty.Text != "") && (lbltotprice.Text != ""))
                                    {
                                        objmedsupplyrequest.SNGItemID = Convert.ToInt64(lblItemId.Text);
                                        objmedsupplyrequest.Itemcatgroup = lblCategoryName.Text;
                                        objmedsupplyrequest.Itemdescription = lblItemDescription.Text;
                                        objmedsupplyrequest.UOM = Convert.ToInt64(lbluomvalue.Text);
                                        objmedsupplyrequest.QuantityPack = Convert.ToInt32(lblQtyPack.Text);
                                        objmedsupplyrequest.Parlevel = lblParlevel.Text;
                                        objmedsupplyrequest.Price = Convert.ToDecimal(lblPrice.Text);
                                        objmedsupplyrequest.TotalPrice = Convert.ToDecimal(lbltotprice.Text);
                                        objmedsupplyrequest.MedicalSupplyMasterID = medsave;
                                        if (lblPrice.Text != "")
                                            objmedsupplyrequest.Price = Convert.ToDecimal(lblPrice.Text);
                                        if (lbltotprice.Text != "")
                                            objmedsupplyrequest.TotalPrice = Convert.ToDecimal(lbltotprice.Text);
                                        if (txtqihand.Text != "")
                                        {
                                            objmedsupplyrequest.QuantityinHand = Convert.ToInt32(txtqihand.Text);
                                            objmedsupplyrequest.OrderQuantity = Convert.ToInt32(lbloqty.Text);
                                        }
                                        else
                                        {
                                            objmedsupplyrequest.QuantityinHand = Convert.ToInt32(0);
                                            objmedsupplyrequest.OrderQuantity = Convert.ToInt32(0);
                                        }
                                        if (objmedsupplyrequest.OrderQuantity != 0)
                                            meditem = lclsservice.InsertMedicalSuppliesDetail(objmedsupplyrequest);
                                    }
                                }
                                if (medsave != 0 && meditem == "Saved Successfully")
                                {
                                    List<GetMedicalSupplyRequestMaster> lstMSRMaster = lclsservice.GetMedicalSupplyRequestMaster().ToList();
                                    string msg = Constant.MedicalSuppliesRequestSaveMessage.Replace("ShowPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", lstMSRMaster[0].PRNo.ToString()).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestSaveMessage.Replace("<<MedicalSupplyRequestDescription>>", lstMSRMaster[0].PRNo.ToString()), true);
                                    Reqdrporderperiod.Visible = false;
                                    Reqdrporderperiod.ValidationGroup = "";
                                    ReqdrporderperiodAdhoc.Visible = false;
                                    ReqdrporderperiodAdhoc.ValidationGroup = "";
                                    Reqdrptimedelivery.Visible = false;
                                    Reqdrptimedelivery.ValidationGroup = "";
                                    CloseFunction();
                                    ViewState["ReportMedSupplyID"] = medsave;
                                    lbpopprint.Visible = true;
                                    divcount.Style.Add("display", "block");
                                    btnClose.Visible = true;
                                }
                            }
                            else
                            {
                                string msg = Constant.WarningMedsupplyreqQtyinhandRequired.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqQtyinhandRequired.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                            }
                            HdnMSRDetailID.Value = "";
                        }
                        else
                        {
                            log.LogWarning(msgstr.Replace("<<MedicalSupplyRequestDescription>>", ""));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqNoRecordsReview.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        }
                    }
                    else
                    {
                        mpemedsupplyReview.Show();
                        log.LogWarning(msg2.Replace("<<MedicalSupplyRequestDescription>>", ""));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqItemNotMapped.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    }
                }
                else
                {
                    int rowcount1 = gvmedreview.Rows.Count;
                    if (rowcount1 != 0)
                    {
                        if (ReviewCount != 0)
                        {
                            //string s = Convert.ToString(ViewState["MSRDetailID"]);
                            string s = Convert.ToString(HdnMSRDetailID.Value);
                            if (s != "")
                            {
                                s = s.Substring(0, s.Length - 1);
                                string[] values = s.Split(',');
                                string[] c = values.Distinct().ToArray();
                                for (int i = 0; i < c.Length; i++)
                                {
                                    GridViewRow row = grdmedsupitemedit.Rows[Convert.ToInt32(c[i])];
                                    objmedsupplyrequest.MedicalSupplyDetailID = Convert.ToInt64(row.Cells[1].Text);
                                    Label lblItemId = (Label)row.FindControl("lbleItemId");
                                    //Label lblevendorItemId = (Label)row.FindControl("lblevendorItemId");
                                    Label lblCategoryName = (Label)row.FindControl("lbleCategoryName");
                                    Label lblItemDescription = (Label)row.FindControl("lbleItemDescription");
                                    Label lbluomvalue = (Label)row.FindControl("lbluomvalue");
                                    Label lblUOM = (Label)row.FindControl("lbleUOM");
                                    Label lblQtyPack = (Label)row.FindControl("lbleQtyPack");
                                    Label lblParlevel = (Label)row.FindControl("lbleParlevel");
                                    TextBox txtqihand = (TextBox)row.FindControl("txteqihand");
                                    Label lbloqty = (Label)row.FindControl("lbleoqty");
                                    Label lblPrice = (Label)row.FindControl("lblPrice");
                                    Label lbltotprice = (Label)row.FindControl("lbltotprice");
                                    objmedsupplyrequest.SNGItemID = Convert.ToInt64(lblItemId.Text);
                                    objmedsupplyrequest.Itemcatgroup = lblCategoryName.Text;
                                    objmedsupplyrequest.Itemdescription = lblItemDescription.Text;
                                    objmedsupplyrequest.UOM = Convert.ToInt64(lbluomvalue.Text);
                                    objmedsupplyrequest.QuantityPack = Convert.ToInt32(lblQtyPack.Text);
                                    objmedsupplyrequest.Parlevel = lblParlevel.Text;
                                    objmedsupplyrequest.Price = Convert.ToDecimal(lblPrice.Text);
                                    objmedsupplyrequest.TotalPrice = Convert.ToDecimal(lbltotprice.Text);
                                    objmedsupplyrequest.PRNo = lblprno.Text;
                                    objmedsupplyrequest.MedicalSupplyMasterID = Convert.ToInt64(ViewState["MedicalsupplyMasterID"]);
                                    if (txtqihand.Text != "")
                                    {
                                        objmedsupplyrequest.QuantityinHand = Convert.ToInt32(txtqihand.Text);
                                        objmedsupplyrequest.OrderQuantity = Convert.ToInt32(lbloqty.Text);
                                    }
                                    else
                                    {
                                        objmedsupplyrequest.QuantityinHand = Convert.ToInt32(0);
                                        objmedsupplyrequest.OrderQuantity = Convert.ToInt32(0);
                                    }
                                    if (objmedsupplyrequest.MedicalSupplyDetailID == 0)
                                    {
                                        objmedsupplyrequest.CreatedBy = defaultPage.UserId;
                                        meditem = lclsservice.InsertMedicalSuppliesDetail(objmedsupplyrequest);
                                    }
                                    else
                                    {
                                        objmedsupplyrequest.LastModifiedBy = defaultPage.UserId;
                                        if (objmedsupplyrequest.OrderQuantity != 0)
                                            meditem = lclsservice.UpdateMedicalSupplyDetails(objmedsupplyrequest);
                                        // ViewState["ReportMedSupplyID"] = objmedsupplyrequest.MedicalSupplyMasterID + ",";
                                        // ViewState["SerachFilters"] = objmedsupplyrequest.CorporateID + "," + objmedsupplyrequest.FacilityID + "," + objmedsupplyrequest.Vendor + "," + objmedsupplyrequest.DateFrom + "," + objmedsupplyrequest.DateTo + "," + objmedsupplyrequest.Status;
                                    }
                                }
                            }
                            else
                            {
                                objmedsupplyrequest.MedicalSupplyMasterID = Convert.ToInt64(ViewState["MedicalsupplyMasterID"]);
                                objmedsupplyrequest.CorporateID = Convert.ToInt64(ddlCorporateadd.SelectedValue);
                                objmedsupplyrequest.FacilityID = Convert.ToInt64(ddlFacilityadd.SelectedValue);
                                objmedsupplyrequest.Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                                objmedsupplyrequest.OrderType = Convert.ToInt32(rdovendortype.SelectedValue);
                                if (rdovendortype.SelectedValue == "4")
                                {
                                    objmedsupplyrequest.OrderPeriod = Convert.ToDateTime(txtadhocorder.Text);
                                }
                                else
                                {
                                    objmedsupplyrequest.OrderPeriod = Convert.ToDateTime(drporderperiod.SelectedItem.Text);
                                }
                                objmedsupplyrequest.Shipping = Convert.ToString(drpshipping.SelectedValue);
                                objmedsupplyrequest.TimeDelivery = Convert.ToString(drptimedelivery.SelectedValue);
                                //
                                //objmedsupplyrequest.Status = "Pending";
                                objmedsupplyrequest.Remarks = "";
                                objmedsupplyrequest.LastModifiedBy = defaultPage.UserId;
                                meditem = lclsservice.UpdateMedicalsupplyMaster(objmedsupplyrequest);
                            }
                            if (meditem == "Saved Successfully")
                            {
                                string msg = Constant.MedicalSuppliesRequestMessage.Replace("ShowPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", lblrwprno.Text).Replace("');", "");
                                log.LogInformation(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestMessage.Replace("<<MedicalSupplyRequestDescription>>", lblrwprno.Text), true);
                                //CloseFunction();
                                ViewState["ReportMedSupplyID"] = objmedsupplyrequest.MedicalSupplyMasterID;
                                lbpopprint.Visible = true;
                                btnreviewprint.Visible = true;
                                ViewState["SelectedRecords"] = null;
                                btnClose.Visible = true;
                            }
                            //ViewState["MSRDetailID"] = "";
                            HdnMSRDetailID.Value = "";
                        }
                        else
                        {
                           
                            log.LogWarning(msgstr.Replace("<<MedicalSupplyRequestDescription>>", ""));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqNoRecordsReview.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        }

                    }
                    else
                    {
                        log.LogWarning(msg2.Replace("<<MedicalSupplyRequestDescription>>", ""));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqItemNotMapped.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        mpemedsupplyReview.Show();
                    }
                }
                btnreviewprint.Visible = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }


        public void AddCalculateOrderQtyandTotalprice()
        {
            try
            {
                for (int i = 0; i <= grdMedReqadd.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = grdMedReqadd.Rows[Convert.ToInt32(i)];
                    Label lblUOM = (Label)gvrow.FindControl("lblUOM");
                    Label lblQtyPack = (Label)gvrow.FindControl("lblQtyPack");
                    Label lblPrice = (Label)gvrow.FindControl("lblPrice");
                    Label lblParlevel = (Label)gvrow.FindControl("lblParlevel");
                    TextBox txtqihand = (TextBox)gvrow.FindControl("txtqihand");
                    Label lbloqty = (Label)gvrow.FindControl("lbloqty");
                    Label lbltotprice = (Label)gvrow.FindControl("lbltotprice");
                    if (txtqihand.Text != "")
                    {
                        if (lblUOM.Text == "Each")
                        {
                            Decimal Orderquantity = Convert.ToInt64(lblParlevel.Text) - Convert.ToInt64(txtqihand.Text);
                            Int64 roundoffqtyeach = Convert.ToInt64(Math.Round(Orderquantity, 0));
                            lbloqty.Text = Convert.ToString(roundoffqtyeach);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbloqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                        else
                        {
                            Decimal qty = Convert.ToDecimal(txtqihand.Text) / Convert.ToDecimal(lblQtyPack.Text);
                            Decimal OrderquantityDecimal = Convert.ToInt64(lblParlevel.Text) - qty;
                            Int64 roundoffqty = Convert.ToInt64(Math.Round(OrderquantityDecimal, 0));
                            lbloqty.Text = Convert.ToString(roundoffqty);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbloqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            // lbltotprice.Text = Convert.ToString(Total);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void EditCalculateOrderQtyandTotalprice()
        {
            try
            {
                for (int i = 0; i <= grdmedsupitemedit.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = grdmedsupitemedit.Rows[Convert.ToInt32(i)];
                    Label lbleUOM = (Label)gvrow.FindControl("lbleUOM");
                    Label lbleQtyPack = (Label)gvrow.FindControl("lbleQtyPack");
                    Label lblPrice = (Label)gvrow.FindControl("lblPrice");
                    Label lbleParlevel = (Label)gvrow.FindControl("lbleParlevel");
                    TextBox txteqihand = (TextBox)gvrow.FindControl("txteqihand");
                    txteqihand.Text = txteqihand.Text.Trim();
                    Label lbleoqty = (Label)gvrow.FindControl("lbleoqty");
                    Label lbltotprice = (Label)gvrow.FindControl("lbltotprice");
                    if (txteqihand.Text != "")
                    {
                        if (lbleUOM.Text == "Each")
                        {
                            Decimal Orderquantity = Convert.ToDecimal(lbleParlevel.Text) - Convert.ToDecimal(txteqihand.Text);
                            Int64 roundoffqtyeach = Convert.ToInt64(Math.Round(Orderquantity, 0));
                            lbleoqty.Text = Convert.ToString(roundoffqtyeach);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbleoqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                        else
                        {
                            Decimal qty = Convert.ToDecimal(txteqihand.Text) / Convert.ToDecimal(lbleQtyPack.Text);
                            Decimal OrderquantityDecimal = Convert.ToInt64(lbleParlevel.Text) - qty;
                            Int64 roundoffqty = Convert.ToInt64(Math.Round(OrderquantityDecimal, 0));
                            lbleoqty.Text = Convert.ToString(roundoffqty);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbleoqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            //lbltotprice.Text = Convert.ToString(Total);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        public void AddGrandTotalcalculation()
        {
            try
            {
                Decimal FootTotal = 0;
                for (int i = 0; i <= grdMedReqadd.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = grdMedReqadd.Rows[Convert.ToInt32(i)];
                    Label lbltotprice = (Label)gvrow.FindControl("lbltotprice");
                    string lbltotal = lbltotprice.Text.TrimStart('$');
                    string totprice = lbltotal.Trim();
                    if (totprice != "")
                        FootTotal += Convert.ToDecimal(totprice);
                }
                lbladdfoottotal.Text = Convert.ToString(FootTotal);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void EditGrandTotalcalculation()
        {
            try
            {
                Decimal FootTotal = 0;
                for (int i = 0; i <= grdmedsupitemedit.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = grdmedsupitemedit.Rows[Convert.ToInt32(i)];
                    Label lbltotprice = (Label)gvrow.FindControl("lbltotprice");
                    string lbltotal = lbltotprice.Text.TrimStart('$');
                    string totprice = lbltotal.Trim();
                    if (totprice != "")
                        FootTotal += Convert.ToDecimal(totprice);
                }
                lbleditfoottotal.Text = Convert.ToString(FootTotal);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void ReviewGrandTotalcalculation()
        {
            try
            {
                Decimal FootTotal = 0;
                for (int i = 0; i <= gvmedreview.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = gvmedreview.Rows[Convert.ToInt32(i)];
                    string lbltotprice = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                    string lblreviewtotal = lbltotprice.TrimStart('$');
                    string totprice = lblreviewtotal.Trim();
                    if (totprice != "")
                        FootTotal += Convert.ToDecimal(totprice);
                }
                //  lblrwgrandtotal.Text = Convert.ToString(FootTotal);
                lblrwgrandtotal.Text = string.Format("{0:F2}", FootTotal);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void PurchaseOrderEditFunction(Int64 MedicalsuppliesID)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                divSearchContent.Style.Add("display", "none");
                divgrdMSRSearch.Style.Add("display", "block");
                btnEdititemAdd.Enabled = true;
                lbpopprint.Visible = true;
                lblUpdateHeader.Visible = true;
                lblrowcount.Visible = false;
                btnreviewprint.Visible = false;
                lblseroutHeader.Visible = false;
                lblEditHeader.Visible = false;
                btnSearchHeader.Visible = false;
                divMSRContentAddHeader.Style.Add("display", "block");
                divContentgrdAdd.Style.Add("display", "none");
                divContentgrdEdit.Style.Add("display", "block");
                btnAdd.Visible = false;
                btnSearch.Visible = false;
                if (defaultPage.Req_MedicalSuppliesPage_Edit == false && defaultPage.Req_MedicalSuppliesPage_View == true)
                {
                    btnSave.Visible = false;
                    removeyes.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    removeyes.Visible = true;
                }
                btnClose.Visible = true;
                divgrdMSRSearch.Attributes["MSRSearchgrid"] = "";
                divgrdMSRSearch.Attributes["class"] = "MSRSearchEditgrid";
                btnReview.Visible = true;
                HddMasterID.Value = Convert.ToString(MedicalsuppliesID);
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["MedicalsupplyMasterID"] = Convert.ToString(MedicalsuppliesID);
                List<BindMedicalsupplymaster> lstmstrdetail = new List<BindMedicalsupplymaster>();
                lstmstrdetail = lclsservice.BindMedicalsupplymaster(MedicalsuppliesID).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("MedicalsuppliesItemID");
                dt.Columns.Add("ItemID");
                dt.Columns.Add("VendorItemID");
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("ItemDescription");
                dt.Columns.Add("UomID");
                dt.Columns.Add("UOM");
                dt.Columns.Add("QtyPack");
                dt.Columns.Add("Price");
                dt.Columns.Add("Parlevel");
                dt.Columns.Add("QtyInHand");
                dt.Columns.Add("OrderQty");
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("TotalPrice");
                dt.AcceptChanges();
                ViewState["UpdateItemRecords"] = dt;
                ViewState["DeleteItemRecords"] = dt;
                if (lstmstrdetail.Count > 0)
                {
                    string LockTimeOut = "";
                    LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                    //List<BindMedicalsupplyDetail> llstMedicalSupplyRequest = lclsservice.BindMedicalsupplyDetail(Convert.ToInt64(ViewState["MedicalsupplyMasterID"]), Convert.ToInt64(defaultPage.UserId), Convert.ToInt64(LockTimeOut)).ToList();
                    List<BindMedicalsupplyDetail> llstMedicalSupplyRequest = lclsservice.BindMedicalsupplyDetail(MedicalsuppliesID, Convert.ToInt64(defaultPage.UserId), Convert.ToInt64(LockTimeOut)).ToList();
                    foreach (var item in llstMedicalSupplyRequest)
                    {
                        dt.Rows.Add(item.MedicalsuppliesItemID, item.ItemID, item.VendorItemID, item.CategoryName, item.ItemDescription, item.UomID, item.UOM, item.QtyPack, item.Price, item.Parlevel, item.QtyInHand, item.OrderQty, item.RowNumber, item.TotalPrice);
                    }
                    ViewState["UpdateItemRecords"] = dt;
                    ViewState["DeleteItemRecords"] = dt;
                    HddUpdateLockinEdit.Value = "Edit";
                    if (llstMedicalSupplyRequest.Count > 0)
                    {
                        if (llstMedicalSupplyRequest[0].MedicalsuppliesItemID != 0)
                        {
                            if (llstMedicalSupplyRequest[0].IsReadOnly == 0)
                            {
                                grdmedsupitemedit.Enabled = true;
                                btnReview.Enabled = true;
                            }
                            else if (llstMedicalSupplyRequest[0].IsReadOnly == 1)
                            {
                                grdmedsupitemedit.Enabled = false;
                                btnReview.Enabled = false;
                                List<GetUserDetails> llstuserdetails = lclsservice.GetUserDetails(Convert.ToInt64(llstMedicalSupplyRequest[0].Lockedby)).ToList();
                                string msg = Constant.WarningMedsupplyreqMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "Another user " + llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName + " is updating this record , Please try after some time.").Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqMessage.Replace("<<MedicalSupplyRequestDescription>>", "Another user " + llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName + " is updating this record , Please try after some time."), true);
                                btnEdititemAdd.Enabled = false;
                            }
                            grdmedsupitemedit.DataSource = llstMedicalSupplyRequest;
                            grdmedsupitemedit.DataBind();
                        }
                        else
                        {
                            grdmedsupitemedit.DataSource = null;
                            grdmedsupitemedit.DataBind();
                        }
                    }
                    else
                    {
                        grdmedsupitemedit.DataSource = null;
                        grdmedsupitemedit.DataBind();
                    }
                    //BindCorporate(0, "Edit");
                    ddlCorporateadd.ClearSelection();
                    ddlCorporateadd.SelectedValue = Convert.ToString(lstmstrdetail[0].Corporate);
                    ddlCorporateadd.Enabled = false;
                    BindFacility(0, "Edit");
                    ddlFacilityadd.ClearSelection();
                    ddlFacilityadd.SelectedValue = Convert.ToString(lstmstrdetail[0].Facility);
                    ddlFacilityadd.Enabled = false;
                    BindVendor(0, "Edit");
                    ddlVendoradd.ClearSelection();
                    ddlVendoradd.SelectedValue = Convert.ToString(lstmstrdetail[0].Vendor);
                    ddlVendoradd.Enabled = false;
                    BindStatus("");
                    BindShipping("");
                    BindTimeDelivery("");
                    rdovendortype.SelectedValue = Convert.ToString(lstmstrdetail[0].OrderType);
                    BindOrderPeriod();
                    if (rdovendortype.SelectedValue == "4")
                    {
                        divorderperiod.Style.Add("display", "none");
                        divOrderAdhoc.Style.Add("display", "block");
                        txtadhocorder.Text = Convert.ToDateTime(lstmstrdetail[0].OrderPeriod).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        divorderperiod.Style.Add("display", "block");
                        divOrderAdhoc.Style.Add("display", "none");
                        drporderperiod.SelectedItem.Text = Convert.ToDateTime(lstmstrdetail[0].OrderPeriod).ToString("MM/dd/yyyy");
                    }
                    rdovendortype.Enabled = false;
                    txtadhocorder.Enabled = false;
                    drporderperiod.Enabled = false;
                    drpshipping.SelectedValue = Convert.ToString(lstmstrdetail[0].Shipping);
                    drpshipping.Enabled = false;
                    if (drpshipping.SelectedValue == "Time Delivery")
                    {
                        divtimedelivery.Style.Add("display", "block");
                        drptimedelivery.SelectedValue = Convert.ToString(lstmstrdetail[0].TimeDelivery);
                        drptimedelivery.Enabled = false;
                    }
                    else
                    {
                        divtimedelivery.Style.Add("display", "none");
                    }
                    imgeshipadd.Enabled = false;
                    imgeshipedit.Enabled = false;
                    imgeshipdelete.Enabled = false;
                    imgtimedeladd.Enabled = false;
                    imgtimedeledit.Enabled = false;
                    imgtimedldel.Enabled = false;
                    //divpr.Style.Add("display", "block");
                    divPRNo.Style.Add("display", "block");
                    lblprno.Text = lstmstrdetail[0].PRNo;
                }
                EditGrandTotalcalculation();
                ViewState["SelectedRecords"] = null;

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                hdncheckfield.Value = "1";
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["MedicalsupplyMasterID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                Int64 MedicalsupplyID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                ViewState["ReportMedSupplyID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                PurchaseOrderEditFunction(MedicalsupplyID);
                btnreviewprint.Visible = true;
                lblseroutHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblEditHeader.Visible = true;
                btnSearchHeader.Visible = false;
                lblrowcount.Visible = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            string a = string.Empty;
            if (HddUpdateLockinEdit.Value == "Edit")
            {
                a = lclsservice.AutoUpdateLockedOut(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId), "MedicalSupply");
                HddUpdateLockinEdit.Value = "";
            }
            if (Request.QueryString["MedicalsupplyID"] != null)
            {
                Response.Redirect("MedicalSuppliesRequestOrder.aspx");
            }
            else
            {
                CloseFunction();
                btnReview.Enabled = true;
                ViewState["SelectedRecords"] = null;
                btnreviewprint.Visible = true;
            }
            btnSearchHeader.Visible = true;
            divcount.Style.Add("display", "block");
        }
        public void CloseFunction()
        {
            try
            {
                // btnreviewprint.ValidationGroup = "EmptyFieldSearch";
                divSearchContent.Style.Add("display", "block");
                divgrdMSRSearch.Style.Add("display", "block");
                divMSRContentAddHeader.Style.Add("display", "none");
                divContentgrdAdd.Style.Add("display", "none");
                divContentgrdEdit.Style.Add("display", "none");
                //divpr.Style.Add("display", "none");
                btnAdd.Visible = true;
                btnSearch.Visible = true;
                btnSave.Visible = false;
                btnClose.Visible = true;
                // btnreviewprint.Visible = false;
                lbpopprint.Visible = false;
                //btnPrint.Visible = true;
                btnReview.Visible = false;
                divPRNo.Style.Add("display", "none");
                divgrdMSRSearch.Attributes["MSRSearchgrid"] = "MSRSearchEditgrid";
                divgrdMSRSearch.Attributes["class"] = "MSRSearchgrid";
                BindStatus("Add");
                ClearSearch();
                SearchGrid();
                ViewState["UpdateItemRecords"] = "";
                ViewState["DeleteItemRecords"] = "";
                ViewState["MSRDetailItemKey"] = "";
                ViewState["ReportMedSupplyID"] = "";
                lbladdfoottotal.Text = "";
                lbleditfoottotal.Text = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void ClearSearch()
        {
            try
            {
                lblEditHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblseroutHeader.Visible = true;
                drpcorsearch.ClearSelection();
                drpfacilitysearch.ClearSelection();
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                BindCorporate(1, "Add");
                // drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                BindFacility(1, "Add");
                //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                BindVendor(1, "Add");
                HddListCorpID.Value = "";
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void ClearAdd()
        {
            drpStatus.SelectedIndex = 0;
            drporderperiod.Items.Clear();
            rdovendortype.SelectedIndex = -1;
            txtadhocorder.Text = "";
        }
        protected void lbdelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["MedicalsuppliesItemID"] = Convert.ToInt64(gvrow.Cells[1].Text);
                ViewState["RemainItemRowNumber"] = gvrow.Cells[13].Text;
                ViewState["MSRDetailItemKey"] = "";
                //peconfirm.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }

        }

        protected void removeyes_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                EditCalculateOrderQtyandTotalprice();
                BindItemGrid();
                btnClose.Visible = false;
                // EditCalTotalprice();
                string delete = string.Empty;
                InventoryServiceClient lclsService = new InventoryServiceClient();
                if (Convert.ToInt64(ViewState["MedicalsuppliesItemID"]) == 0)
                {
                    DataTable dt = (DataTable)ViewState["MedicalsupplyItem"];
                    DataRow[] dr = dt.Select("ItemID = '" + ViewState["RemainItemRowNumber"] + "'");
                    if (dr.Length > 0)
                    {
                        dt.Rows.Remove(dr[0]);
                        dt.AcceptChanges();
                    }
                    grdmedsupitemedit.DataSource = dt;
                    grdmedsupitemedit.DataBind();            
                }
                else
                {
                    delete = lclsService.DeleteMedicalSuppliesDetails(Convert.ToInt64(ViewState["MedicalsuppliesItemID"]), defaultPage.UserId);
                    DataTable dt = (DataTable)ViewState["MedicalsupplyItem"];
                    DataTable dtmaindelete = (DataTable)ViewState["DeleteItemRecords"];
                    DataRow[] dr = dt.Select("MedicalsuppliesItemID = '" + ViewState["MedicalsuppliesItemID"] + "'");
                    if (dr.Length > 0)
                    {
                        dt.Rows.Remove(dr[0]);
                        dt.AcceptChanges();
                    }
                    grdmedsupitemedit.DataSource = dt;
                    grdmedsupitemedit.DataBind();
                    ViewState["MedicalsupplyItem"] = dt;
                    if (delete == "Deleted Successfully")
                    {
                        string msg = Constant.WarningMedsupplyItemNotReceived.Replace("ShowdelPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "Deleted Successfully").Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReqShiptDeleteMessage.Replace("<<MedicalSupplyRequestDescription>>", "Deleted Successfully"), true);         
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void removeno_Click(object sender, EventArgs e)
        {
            EditCalculateOrderQtyandTotalprice();
        }

        protected void ddlVendoradd_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdovendortype.ClearSelection();
            drporderperiod.Items.Clear();
            BindAddMedicalsupplies();
        }

        protected void btnEdititemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblerrormsgselectitem.Style.Add("display", "none");
                BindItemGrid();
                if (grdmedsupitemedit.Rows.Count != 0)
                {
                    for (int i = 0; i < grdmedsupitemedit.Rows.Count; i++)
                    {
                        string index = grdmedsupitemedit.Rows[i].Cells[13].Text;
                        ViewState["MSRDetailItemKey"] += Convert.ToString(index) + ',';
                    }
                }
                else
                {
                    ViewState["MSRDetailItemKey"] = "";
                    ViewState["UpdateItemRecords"] = "";
                }
                BindRemainingItemGrid();
                mpeeditnewitem.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        public void BindRemainingItemGrid()
        {
            try
            {
                BALMedicalSuppliesRequest Objmsr = new BALMedicalSuppliesRequest();
                Objmsr.CorporateID = Convert.ToInt64(ddlCorporateadd.SelectedValue);
                Objmsr.FacilityID = Convert.ToInt64(ddlFacilityadd.SelectedValue);
                Objmsr.Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                Objmsr.CombineKey = Convert.ToString(ViewState["MSRDetailItemKey"]);
                List<AddMedicalsupplyitem> lstMSRMaster = lclsservice.AddMedicalsupplyitem(Objmsr).ToList();
                grdeditnewitemadd.DataSource = lstMSRMaster;
                grdeditnewitemadd.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void ckboxSelectAllNewItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ChkBoxHeader = (CheckBox)grdeditnewitemadd.HeaderRow.FindControl("ckboxSelectAllNewItem");
                foreach (GridViewRow row in grdeditnewitemadd.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("ckboxselectitem");
                    if (ChkBoxHeader.Checked == true)
                    {
                        ChkBoxRows.Checked = true;
                    }
                    else
                    {
                        ChkBoxRows.Checked = false;
                    }
                }
                mpeeditnewitem.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void ckboxselectitem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkselectall = (CheckBox)grdeditnewitemadd.HeaderRow.FindControl("ckboxSelectAllNewItem");
                int SelectedCount = 0;
                foreach (GridViewRow row in grdeditnewitemadd.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("ckboxselectitem");
                    if (ChkBoxRows.Checked == false)
                    {
                        chkselectall.Checked = false;
                    }
                    if (ChkBoxRows.Checked == true)
                    {
                        SelectedCount++;
                    }
                    if (grdeditnewitemadd.Rows.Count == SelectedCount)
                    {
                        chkselectall.Checked = true;
                    }
                }
                SelectedCount = 0;
                CheckBox cb = (CheckBox)sender;
                GridViewRow rowdata = (GridViewRow)cb.NamingContainer;
                int index = Convert.ToInt16(rowdata.RowIndex);
                HdnMSRDetailID.Value += Convert.ToString(index) + ',';
                mpeeditnewitem.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void grdeditnewitemadd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (grdeditnewitemadd.HeaderRow != null)
                    {
                        CheckBox chkselectall = (CheckBox)grdeditnewitemadd.HeaderRow.FindControl("ckboxSelectAllNewItem");
                        CheckBox ChkBoxRows = (CheckBox)e.Row.FindControl("ckboxselectitem");

                        if (chkselectall.Checked)
                        {
                            ChkBoxRows.Checked = true;
                        }

                        else
                        {
                            ChkBoxRows.Checked = false;
                        }
                    }
                }
                mpeeditnewitem.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void addnewItem_Click(object sender, EventArgs e)
        {
            try
            {               
                ViewState["MSRDetailItemKey"] = "";
                EditCalculateOrderQtyandTotalprice();
                BindItemGrid();
                // EditCalTotalprice();
                GetData();
                SetData();
                // BindSecondaryGrid();
                BindItemRecords();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        private void GetData()
        {
            try
            {
                lblerrormsgselectitem.Style.Add("display", "none");
                DataTable dt;
                dt = CreateDataTable();
                CheckBox chkAll = (CheckBox)grdeditnewitemadd.HeaderRow.Cells[0].FindControl("ckboxSelectAllNewItem");
                for (int i = 0; i < grdeditnewitemadd.Rows.Count; i++)
                {
                    if (chkAll.Checked)
                    {
                        dt = AddRow(grdeditnewitemadd.Rows[i], dt);
                    }
                    else
                    {
                        CheckBox chk = (CheckBox)grdeditnewitemadd.Rows[i]
                                        .Cells[0].FindControl("ckboxselectitem");
                        if (chk.Checked)
                        {
                            dt = AddRow(grdeditnewitemadd.Rows[i], dt);
                        }
                        else
                        {
                            dt = RemoveRow(grdeditnewitemadd.Rows[i], dt);
                        }
                    }
                }
                ViewState["SelectedItemRecords"] = dt;
                if (dt.Rows.Count == 0)
                {
                    if (grdeditnewitemadd.Rows.Count == 0)
                    {
                        lblerrormsgselectitem.Style.Add("display", "none");
                    }
                    else
                    {
                        lblerrormsgselectitem.Style.Add("display", "block");
                    }
                    mpeeditnewitem.Show();
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        private void SetData()
        {
            try
            {
                CheckBox chkAll = (CheckBox)grdeditnewitemadd.HeaderRow.Cells[0].FindControl("ckboxSelectAllNewItem");
                chkAll.Checked = true;
                if (ViewState["SelectedItemRecords"] != null)
                {
                    DataTable dt = (DataTable)ViewState["SelectedItemRecords"];
                    for (int i = 0; i < grdeditnewitemadd.Rows.Count; i++)
                    {
                        CheckBox chk = (CheckBox)grdeditnewitemadd.Rows[i].Cells[0].FindControl("ckboxselectitem");
                        if (chk != null)
                        {
                            DataRow[] dr = dt.Select("MedicalsuppliesItemID = '" + grdeditnewitemadd.Rows[i].Cells[1].Text + "'");
                            chk.Checked = dr.Length > 0;
                            if (!chk.Checked)
                            {
                                chkAll.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void EnableEditControls()
        {
            try
            {
                ddlCorporateadd.Enabled = true;
                ddlFacilityadd.Enabled = true;
                ddlVendoradd.Enabled = true;
                rdovendortype.Enabled = true;
                drporderperiod.Enabled = true;
                drpshipping.Enabled = true;
                if (drpshipping.SelectedValue == "Time Delivery")
                {
                    divtimedelivery.Style.Add("display", "block");
                    drptimedelivery.Enabled = true;
                }
                else
                {
                    divtimedelivery.Style.Add("display", "none");
                    drptimedelivery.Enabled = true;
                }
                txtadhocorder.Enabled = true;
                imgeshipadd.Enabled = true;
                imgeshipedit.Enabled = true;
                imgeshipdelete.Enabled = true;
                imgtimedeladd.Enabled = true;
                imgtimedeledit.Enabled = true;
                imgtimedldel.Enabled = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }

        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MedicalsuppliesItemID");
            dt.Columns.Add("ItemID");
            dt.Columns.Add("VendorItemID");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("ItemDescription");
            dt.Columns.Add("UomID");
            dt.Columns.Add("UOM");
            dt.Columns.Add("QtyPack");
            dt.Columns.Add("Price");
            dt.Columns.Add("Parlevel");
            dt.Columns.Add("QtyInHand");
            dt.Columns.Add("OrderQty");
            dt.Columns.Add("RowNumber");
            dt.Columns.Add("TotalPrice");
            dt.AcceptChanges();
            return dt;

        }
        private DataTable AddRow(GridViewRow gvRow, DataTable dt)
        {
            DataRow[] dr = dt.Select("ItemID = '" + gvRow.Cells[2].Text + "'");
            if (dr.Length <= 0)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1]["MedicalsuppliesItemID"] = gvRow.Cells[1].Text;
                dt.Rows[dt.Rows.Count - 1]["ItemID"] = gvRow.Cells[2].Text;
                dt.Rows[dt.Rows.Count - 1]["VendorItemID"] = gvRow.Cells[3].Text;
                dt.Rows[dt.Rows.Count - 1]["CategoryName"] = gvRow.Cells[4].Text;
                dt.Rows[dt.Rows.Count - 1]["ItemDescription"] = gvRow.Cells[5].Text;
                dt.Rows[dt.Rows.Count - 1]["UomID"] = gvRow.Cells[6].Text;
                dt.Rows[dt.Rows.Count - 1]["UOM"] = gvRow.Cells[7].Text;
                dt.Rows[dt.Rows.Count - 1]["QtyPack"] = gvRow.Cells[8].Text;
                dt.Rows[dt.Rows.Count - 1]["Price"] = gvRow.Cells[9].Text;
                dt.Rows[dt.Rows.Count - 1]["Parlevel"] = gvRow.Cells[10].Text;
                dt.Rows[dt.Rows.Count - 1]["QtyInHand"] = gvRow.Cells[11].Text;
                dt.Rows[dt.Rows.Count - 1]["OrderQty"] = gvRow.Cells[12].Text;
                dt.Rows[dt.Rows.Count - 1]["RowNumber"] = gvRow.Cells[13].Text;
                dt.AcceptChanges();
            }
            return dt;

        }
        private DataTable RemoveRow(GridViewRow gvRow, DataTable dt)
        {

            DataRow[] dr = dt.Select("ItemID = '" + gvRow.Cells[2].Text + "'");
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
                dt.AcceptChanges();
            }
            return dt;
        }
        private void BindItemGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("MedicalsuppliesItemID");
                dt.Columns.Add("ItemID");
                dt.Columns.Add("VendorItemID");
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("ItemDescription");
                dt.Columns.Add("UomID");
                dt.Columns.Add("UOM");
                dt.Columns.Add("QtyPack");
                dt.Columns.Add("Price");
                dt.Columns.Add("Parlevel");
                dt.Columns.Add("QtyInHand");
                dt.Columns.Add("OrderQty");
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("TotalPrice");
                dt.AcceptChanges();
                foreach (GridViewRow row in grdmedsupitemedit.Rows)
                {
                    //Label lbleItemId = (Label)row.FindControl("lbleItemId");
                    Label lblevendorItemId = (Label)row.FindControl("lblevendorItemId");
                    Label lbleCategoryName = (Label)row.FindControl("lbleCategoryName");
                    Label lbleItemDescription = (Label)row.FindControl("lbleItemDescription");
                    Label lbluomvalue = (Label)row.FindControl("lbluomvalue");
                    Label lbleUOM = (Label)row.FindControl("lbleUOM");
                    Label lbleQtyPack = (Label)row.FindControl("lbleQtyPack");
                    Label lblPrice = (Label)row.FindControl("lblPrice");
                    Label lbleParlevel = (Label)row.FindControl("lbleParlevel");
                    TextBox txteqihand = (TextBox)row.FindControl("txteqihand");
                    Label lbleoqty = (Label)row.FindControl("lbleoqty");
                    Label lbltotprice = (Label)row.FindControl("lbltotprice");

                    dr = dt.NewRow();

                    dr["MedicalsuppliesItemID"] = row.Cells[1].Text;
                    dr["ItemID"] = row.Cells[13].Text;
                    dr["VendorItemID"] = lblevendorItemId.Text;
                    dr["CategoryName"] = lbleCategoryName.Text;
                    dr["ItemDescription"] = lbleItemDescription.Text;
                    dr["UomID"] = lbluomvalue.Text;
                    dr["UOM"] = lbleUOM.Text;
                    dr["QtyPack"] = lbleQtyPack.Text;
                    dr["Price"] = lblPrice.Text;
                    dr["Parlevel"] = lbleParlevel.Text;
                    dr["QtyInHand"] = txteqihand.Text;
                    dr["OrderQty"] = lbleoqty.Text;
                    dr["RowNumber"] = row.Cells[12].Text;
                    dr["TotalPrice"] = lbltotprice.Text;

                    dt.Rows.Add(dr);
                }
                ViewState["MedicalsupplyItem"] = dt;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        private void BindItemRecords()
        {
            try
            {
                BindItemGrid();
                DataTable data = new DataTable();
                DataTable dtSelectedRecords = (DataTable)ViewState["SelectedItemRecords"];
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["MedicalsupplyItem"];
                if (dt.Rows.Count > 0)
                {
                    data.Merge((DataTable)ViewState["MedicalsupplyItem"]);
                    data.Merge(dtSelectedRecords);
                    data.AcceptChanges();
                    grdmedsupitemedit.DataSource = data;
                    grdmedsupitemedit.DataBind();
                    ViewState["MedicalsupplyItem"] = "";
                    ViewState["MedicalsupplyItem"] = data;
                }
                else
                {
                    grdmedsupitemedit.DataSource = dt;
                    grdmedsupitemedit.DataBind();
                    ViewState["MedicalsupplyItem"] = dt;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void EditCalTotalprice()
        {
            try
            {
                for (int i = 0; i <= grdmedsupitemedit.Rows.Count - 1; i++)
                {
                    GridViewRow gvrow = grdmedsupitemedit.Rows[Convert.ToInt32(i)];
                    Label lbleUOM = (Label)gvrow.FindControl("lbleUOM");
                    Label lbleQtyPack = (Label)gvrow.FindControl("lbleQtyPack");
                    Label lblPrice = (Label)gvrow.FindControl("lblPrice");
                    Label lbleParlevel = (Label)gvrow.FindControl("lbleParlevel");
                    TextBox txteqihand = (TextBox)gvrow.FindControl("txteqihand");
                    txteqihand.Text = txteqihand.Text.Trim();
                    Label lbleoqty = (Label)gvrow.FindControl("lbleoqty");
                    Label lbltotprice = (Label)gvrow.FindControl("lbltotprice");
                    if (txteqihand.Text != "")
                    {
                        if (lbleUOM.Text == "Each")
                        {
                            Decimal Orderquantity = Convert.ToDecimal(lbleParlevel.Text) - Convert.ToDecimal(txteqihand.Text);
                            Int64 roundoffqtyeach = Convert.ToInt64(Math.Round(Orderquantity, 0));
                            lbleoqty.Text = Convert.ToString(roundoffqtyeach);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbleoqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                        else
                        {
                            Decimal qty = Convert.ToDecimal(txteqihand.Text) / Convert.ToDecimal(lbleQtyPack.Text);
                            Decimal OrderquantityDecimal = Convert.ToInt64(lbleParlevel.Text) - qty;
                            Int64 roundoffqty = Convert.ToInt64(Math.Round(OrderquantityDecimal, 0));
                            lbleoqty.Text = Convert.ToString(roundoffqty);
                            Decimal Total = 0;
                            Total = Convert.ToDecimal(lbleoqty.Text) * Convert.ToDecimal(lblPrice.Text);
                            lbltotprice.Text = string.Format("{0:F2}", Total);
                        }
                    }
                }
                BindItemGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                //bool isvaliditem = false;
                string isvaliditem = string.Empty;
                string validitem = string.Empty;
                foreach (GridViewRow row in grdMedReqadd.Rows)
                {
                    if (ddlFacilityadd.SelectedItem.Text != "--Select Facility--")
                        llstMedicalSupplyRequest.FacilityID = Convert.ToInt64(ddlFacilityadd.SelectedValue);
                    if (ddlVendoradd.SelectedItem.Text != "--Select Vendor--")
                        llstMedicalSupplyRequest.Vendor = Convert.ToInt64(ddlVendoradd.SelectedValue);
                    Label lblItemId = (Label)row.FindControl("lblItemId");
                    TextBox txtqihand = (TextBox)row.FindControl("txtqihand");
                    Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                    llstMedicalSupplyRequest.CreatedBy = defaultPage.UserId;
                    if (lblItemId.Text != "")
                    {
                        llstMedicalSupplyRequest.SNGItemID = Convert.ToInt64(lblItemId.Text);
                    }
                     if (txtqihand.Text != "")
                    {
                        List<ValidateMedicalSuppliesItem> valMedicalSupp = lclsservice.ValidateMedicalSuppliesItem(llstMedicalSupplyRequest).ToList();
                        if (valMedicalSupp[0].Edit == 0 && rdovendortype.SelectedValue != "4")
                        {
                            //validitem += lblItemDescription.Text + ",";
                            if(validitem == "")
                            {
                                validitem = validitem + valMedicalSupp[0].ItemDescrip + ", " + valMedicalSupp[0].ItemTrack;
                            }
                            else
                            {
                                validitem = validitem + ", " + valMedicalSupp[0].ItemDescrip + ", " + valMedicalSupp[0].ItemTrack;
                            }
                            
                            isvaliditem = "Error";
                        }
                        
                    }
                }

                ////////if (isvaliditem == true)
                ////////{
                AddCalculateOrderQtyandTotalprice();
                EditCalculateOrderQtyandTotalprice();
                AddGrandTotalcalculation();
                EditGrandTotalcalculation();
                lblrwprno.Text = lblprno.Text;
                if (ddlCorporateadd.SelectedItem.Text != "--Select Corporate--")
                    lblreviewcorporate.Text = ddlCorporateadd.SelectedItem.Text;
                if (ddlFacilityadd.SelectedItem.Text != "--Select Facility--")
                    lblreviewfacility.Text = ddlFacilityadd.SelectedItem.Text;
                if (ddlVendoradd.SelectedItem.Text != "--Select Vendor--")
                    lblvendor.Text = ddlVendoradd.SelectedItem.Text;
                if (rdovendortype.SelectedValue != "")
                    lblordertype.Text = rdovendortype.SelectedItem.Text;
                if (rdovendortype.SelectedValue == "4")
                {
                    lblorderperiod.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    if (drporderperiod.SelectedValue != "")
                        lblorderperiod.Text = drporderperiod.SelectedItem.Text;
                }
                if (drpshipping.SelectedItem.Text != "--Select Shipping--")
                    lblshipping.Text = drpshipping.SelectedItem.Text;
                if (drptimedelivery.SelectedItem.Text != "--Select TimeDelivery--")
                    lbltimedelivery.Text = drptimedelivery.SelectedItem.Text;
                if (Convert.ToInt64(ViewState["MedicalsupplyMasterID"]) == 0)
                {
                    Int32 DetailGridRow = grdMedReqadd.Rows.Count;

                    if (DetailGridRow != 0)
                    {
                        mpemedsupplyReview.Show();
                        diveviewprno.Style.Add("display", "none");
                        //string s = Convert.ToString(ViewState["MSRDetailID"]);
                        string s = Convert.ToString(HdnMSRDetailID.Value);
                        DataTable dt = CreateDataTable();
                        if (s != "")
                        {
                            s = s.Substring(0, s.Length - 1);
                            string[] values = s.Split(',');
                            string[] c = values.Distinct().ToArray();
                            for (int i = 0; i < c.Length; i++)
                            {
                                GridViewRow row = grdMedReqadd.Rows[Convert.ToInt32(c[i])];
                                Label lblItemId = (Label)row.FindControl("lblItemId");
                                Label lblvendorItemId = (Label)row.FindControl("lblvendorItemId");
                                Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                                Label lblCategoryName = (Label)row.FindControl("lblCategoryName");
                                Label lblUOM = (Label)row.FindControl("lblUOM");
                                Label lbluomvalue = (Label)row.FindControl("lbluomvalue");
                                Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                                Label lblParlevel = (Label)row.FindControl("lblParlevel");
                                TextBox txtqihand = (TextBox)row.FindControl("txtqihand");
                                Label lbloqty = (Label)row.FindControl("lbloqty");
                                Label lblPrice = (Label)row.FindControl("lblPrice");
                                Label lbltotprice = (Label)row.FindControl("lbltotprice");
                                if ((txtqihand.Text != "") && (lbloqty.Text != "") && (lbltotprice.Text != ""))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["MedicalsuppliesItemID"] = 0;
                                    dr["ItemID"] = lblItemId.Text;
                                    dr["VendorItemID"] = lblvendorItemId.Text;
                                    dr["CategoryName"] = lblCategoryName.Text;
                                    dr["ItemDescription"] = lblItemDescription.Text;
                                    dr["UomID"] = lbluomvalue.Text;
                                    dr["UOM"] = lblUOM.Text;
                                    dr["QtyPack"] = lblQtyPack.Text;
                                    dr["Price"] = lblPrice.Text;
                                    dr["Parlevel"] = lblParlevel.Text;
                                    dr["QtyInHand"] = txtqihand.Text;
                                    dr["OrderQty"] = lbloqty.Text;
                                    dr["TotalPrice"] = lbltotprice.Text;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        if (dt.Rows.Count == 0)
                        {
                            mpemedsupplyReview.Hide();
                            log.LogWarning(msgstr.Replace("<<MedicalSupplyRequestDescription>>", ""));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqNoRecordsItemGrid.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                        }
                        else
                        {
                            if (isvaliditem == "Error")
                            {
                                mpemedsupplyReview.Hide();
                                string msg = Constant.WarningMedsupplyItemNotReceived.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "" + validitem).Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyItemNotReceived.Replace("<<MedicalSupplyRequestDescription>>", "" + validitem), true);
                            }
                            else
                            {                                
                                gvmedreview.DataSource = dt;
                                gvmedreview.DataBind();
                            }
                        }
                    }
                    else
                    {
                        log.LogWarning(msgstr.Replace("<<MedicalSupplyRequestDescription>>", ""));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqNoRecordsItemGrid.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    }
                }
                else
                {

                    int rowcount1 = grdmedsupitemedit.Rows.Count;
                    if (rowcount1 != 0)
                    {
                        mpemedsupplyReview.Show();
                        diveviewprno.Style.Add("display", "block");
                        DataTable dt = CreateDataTable();
                        foreach (GridViewRow row in grdmedsupitemedit.Rows)
                        {
                            //GridViewRow row = grdmedsupitemedit.Rows[Convert.ToInt32(row)];                               
                            Label lblItemId = (Label)row.FindControl("lbleItemId");
                            Label lblvendorItemId = (Label)row.FindControl("lblevendorItemId");
                            Label lblCategoryName = (Label)row.FindControl("lbleCategoryName");
                            Label lblItemDescription = (Label)row.FindControl("lbleItemDescription");
                            Label lbluomvalue = (Label)row.FindControl("lbluomvalue");
                            Label lblUOM = (Label)row.FindControl("lbleUOM");
                            Label lblQtyPack = (Label)row.FindControl("lbleQtyPack");
                            Label lblParlevel = (Label)row.FindControl("lbleParlevel");
                            TextBox txtqihand = (TextBox)row.FindControl("txteqihand");
                            Label lbloqty = (Label)row.FindControl("lbleoqty");
                            Label lblPrice = (Label)row.FindControl("lblPrice");
                            Label lbltotprice = (Label)row.FindControl("lbltotprice");
                            if ((txtqihand.Text != "") && (lbloqty.Text != "") && (lbltotprice.Text != ""))
                            {
                                DataRow dr = dt.NewRow();
                                dr["MedicalsuppliesItemID"] = 0;
                                dr["ItemID"] = lblItemId.Text;
                                dr["VendorItemID"] = lblvendorItemId.Text;
                                dr["CategoryName"] = lblCategoryName.Text;
                                dr["ItemDescription"] = lblItemDescription.Text;
                                dr["UomID"] = lbluomvalue.Text;
                                dr["UOM"] = lblUOM.Text;
                                dr["QtyPack"] = lblQtyPack.Text;
                                dr["Price"] = lblPrice.Text;
                                dr["Parlevel"] = lblParlevel.Text;
                                dr["QtyInHand"] = txtqihand.Text;
                                dr["OrderQty"] = lbloqty.Text;
                                dr["TotalPrice"] = lbltotprice.Text;
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                string msg = Constant.WarningMedsupplyreqQtyEditMode.Replace("ShowwarningLookupPopup('", "").Replace("<<MedicalSupplyRequestDescription>>", "").Replace("');", "");
                                log.LogWarning(msg);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqQtyEditMode.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                                mpemedsupplyReview.Hide();
                            }
                        }
                        //for (int i = 0; i < grdmedsupitemedit.Rows.Count; i++)
                        //{
                        //    GridViewRow row = grdmedsupitemedit.Rows[Convert.ToInt32(i)];

                        //}
                        gvmedreview.DataSource = dt;
                        gvmedreview.DataBind();
                    }
                    else
                    {
                        log.LogWarning(msgstr.Replace("<<MedicalSupplyRequestDescription>>", ""));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyreqNoRecordsItemGrid.Replace("<<MedicalSupplyRequestDescription>>", ""), true);
                    }

                }
                ReviewGrandTotalcalculation();
                //////}
                //////else
                //////{
                //////    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyItemNotReceived.Replace("<<MedicalSupplyRequestDescription>>", "" + validitem), true);
                //////}

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void CopyToDataTable(object sender, EventArgs e)
        {

        }

        protected void btnreviewcancel_Click(object sender, EventArgs e)
        {
            mpemedsupplyReview.Hide();
        }

        protected void btnreviewprint_Click(object sender, EventArgs e)
        {
            try
            {
                string smedmasterIds = string.Empty;
                List<GetmedicalsupplyReviewReport> llstreview = new List<GetmedicalsupplyReviewReport>();
                if ((ViewState["ReportMedSupplyID"] == null) || (Convert.ToString(ViewState["ReportMedSupplyID"]) == ""))
                {
                    foreach (GridViewRow row in grdMedReqSearch.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (smedmasterIds == string.Empty)
                                smedmasterIds = row.Cells[1].Text;
                            else
                                smedmasterIds = smedmasterIds + "," + row.Cells[1].Text;
                        }
                    }
                    llstreview = lclsservice.GetmedicalsupplyReviewReport(null, smedmasterIds, defaultPage.UserId, defaultPage.UserId).ToList();
                }
                else
                {

                    smedmasterIds = ViewState["ReportMedSupplyID"].ToString();
                    smedmasterIds = smedmasterIds.Replace(",", "");
                    llstreview = lclsservice.GetmedicalsupplyReviewReport(smedmasterIds, null, defaultPage.UserId, defaultPage.UserId).ToList();
                }

                rvmedicalsupplyreport.ProcessingMode = ProcessingMode.Local;
                rvmedicalsupplyreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalSuppliesReview.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[3];
                p1[0] = new ReportParameter("MedicalSupplyID", "0");
                p1[1] = new ReportParameter("SearchFilters", "test");
                p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));


                this.rvmedicalsupplyreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("MedicalSuppliesReviewDS", llstreview);
                rvmedicalsupplyreport.LocalReport.DataSources.Clear();
                rvmedicalsupplyreport.LocalReport.DataSources.Add(datasource);
                rvmedicalsupplyreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvmedicalsupplyreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MedicalSupplies" + guid + ".pdf";
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
                ViewState["ReportMedSupplyID"] = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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
                        System.IO.File.Delete(path);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string MedreqID = string.Empty;
                MedreqID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetmedicalsupplyReviewReport> llstreview = lclsservice.GetmedicalsupplyReviewReport(MedreqID, null, defaultPage.UserId, defaultPage.UserId).ToList();
                rvmedicalsupplyreport.ProcessingMode = ProcessingMode.Local;
                rvmedicalsupplyreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalSuppliesReview.rdlc");

                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[3];
                p1[0] = new ReportParameter("MedicalSupplyID", "0");
                p1[1] = new ReportParameter("SearchFilters", "test");
                p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

                this.rvmedicalsupplyreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("MedicalSuppliesReviewDS", llstreview);
                rvmedicalsupplyreport.LocalReport.DataSources.Clear();
                rvmedicalsupplyreport.LocalReport.DataSources.Add(datasource);
                rvmedicalsupplyreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvmedicalsupplyreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MedicalSupplies" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }

        }
        protected void btneditnewitemclose_Click(object sender, EventArgs e)
        {
            try
            {
                EditCalculateOrderQtyandTotalprice();
                mpeeditnewitem.Hide();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
         
        }



        protected void ChkAllCorp_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
          
        }

        protected void lnkClearAllCorp_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
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

                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
           
        }

        protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
          
      
    }
}


