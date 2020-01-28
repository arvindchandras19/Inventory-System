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

namespace Inventory
{
    public partial class RequestIT : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALRequestIT argRequestIT = new BALRequestIT();
        string a = string.Empty;
        string b = string.Empty;
        string PendingApproval = Constant.PendingApprovalforreq;
        private string _sessionPDFFileName;
        string ErrorList = string.Empty;
        string ITRNO = string.Empty;
        string FinalString = "";
        string loadshipping = Constant.loadshipping;
        StringBuilder SB = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.grdITMastersearch);
                scriptManager.RegisterPostBackControl(this.GvTempEdit);                
                if (!IsPostBack)
                {
                  
                    if (defaultPage != null)
                    {
                        BindStatus("Add");
                        BindCorporate(1, "Add");                      
                        BindFacility(1, "Add");                      
                        BindVendor(1, "Add");
                        string ITRNO = Request.QueryString["ITRNo"];
                        if (ITRNO != null)
                        {
                            divMPRDetails.Style.Add("display", "block");
                            divSearchMachine.Style.Add("display", "block");
                            divMPRMaster.Style.Add("diplay", "none");
                            divContentDetails.Style.Add("display", "block");
                            divContent.Style.Add("display", "none");
                            btnSave.Visible = true;
                            btnSave.ValidationGroup = "";                                        
                            DivMPRMasterNo.Style.Add("display", "block");
                            lblMasterNo.Text = ITRNO;                          
                            btnReview.Visible = true;
                            btnReview.ValidationGroup = "";
                            btnAdd.Visible = false;
                            btnSearch.Visible = false;
                            BindDetailsgridfromPO(ITRNO);                          
                            btnSearchHeader.Visible = false;
                            lblseroutHeader.Visible = false;
                            lblMasterHeader.Visible = false;
                            btnPrint.Visible = false;
                            lblcount.Visible = false;
                            lblUpdateHeader.Visible = true;
                        }
                        else
                        {
                            btnPrint.Visible = true;
                            divMPRDetails.Style.Add("display", "none");
                            divSearchMachine.Style.Add("display", "none");
                            divMPRMaster.Style.Add("diplay", "block");                           
                            divContent.Style.Add("display", "block");
                            btnSave.Visible = false;
                            btnSave.ValidationGroup = "EmptyFieldSave";
                            btnReview.Visible = false;
                            btnReview.ValidationGroup = "EmptyFieldSave";
                            btnAdd.Visible = true;
                            btnSearch.Visible = true;
                            SearchGrid();
                            if (defaultPage.Req_RequestITPage_Edit == false && defaultPage.Req_RequestITPage_View == true)
                            {
                                btnAdd.Visible = false;
                                btnSave.Visible = false;
                                btnReview.Visible = false;
                                btnImgDeletePopUp.Visible = false;
                                btnSearchNewRow.Enabled = false;
                            }
                            if (defaultPage.Req_RequestITPage_Edit == false && defaultPage.Req_RequestITPage_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                            }                            
                        }
                    }

                    rdorequesttypesearch.SelectedItem.Text = "All";                   
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void BindDetailsgridfromPO(string ITRNO)
        {
            try
            {
                List<BindRequestITDetailsfromPO> lstpo = new List<BindRequestITDetailsfromPO>();
                lstpo = lclsservice.BindRequestITDetailsfromPO(ITRNO).ToList();
                ViewState["Requesttype"] = lstpo[0].Reequestype;
                if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                    rdorequesttype.SelectedValue = "0";
                else
                    rdorequesttype.SelectedValue = "1";
                HddMasterID.Value = Convert.ToString(lstpo[0].ITRequestMasterID);
                ViewState["CorporateID"] = lstpo[0].CorporateID;
                ddlCorporate.SelectedValue = Convert.ToString(lstpo[0].CorporateID);
                BindFacility(0, "Edit");
                ViewState["FacilityID"] = Convert.ToInt64(lstpo[0].Facility);
                ddlFacility.SelectedValue = Convert.ToString(lstpo[0].Facility);
                BindVendor(0, "Edit");
                ViewState["VendorID"] = Convert.ToInt64(lstpo[0].VendorID);
                ddlVendor.SelectedValue = Convert.ToString(ViewState["VendorID"]);
                ViewState["Facility"] = lstpo[0].FacilityDescription;
                ViewState["Vendor"] = lstpo[0].VendorDescription;
                ViewState["Corporate"] = lstpo[0].CorporateName;
                ViewState["Shipping"] = lstpo[0].Shipping;
                BindShipping("Edit");
                ddlShipping.SelectedValue = Convert.ToString(lstpo[0].Shipping);
                ViewState["ReportITRequestID"] = lstpo[0].ITRequestMasterID;
                rdorequesttype.Enabled = false;
                ddlFacility.Enabled = false;
                ddlVendor.Enabled = false;
                ddlShipping.Enabled = false;
                ddlCorporate.Enabled = false;
                gvSearchMRPDetails.DataSource = lstpo;
                gvSearchMRPDetails.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                    if (defaultPage.RoleID == 1)
                    {
                        // Search Drop Down   
                        lstfacility = lclsservice.GetCorporateMaster().ToList();
                        drpcorsearch.DataSource = lstfacility;
                        drpcorsearch.DataTextField = "CorporateName";
                        drpcorsearch.DataValueField = "CorporateID";
                        drpcorsearch.DataBind();                        
                    }
                    else
                    {
                        lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                        drpcorsearch.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                        drpcorsearch.DataTextField = "CorporateName";
                        drpcorsearch.DataValueField = "CorporateID";
                        drpcorsearch.DataBind();                       
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                BindVendor(1, "Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                        lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a=>a.IT == true).ToList();
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
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => (a.IT == true) && (a.IsActive == true)).Distinct().ToList();
                        ddlVendor.DataSource = lstvendordetails;
                        ddlVendor.DataTextField = "VendorDescription";
                        ddlVendor.DataValueField = "VendorID";
                        ddlVendor.DataBind();
                        ddlVendor.Items.Insert(0, lst);
                        ddlVendor.SelectedIndex = 0;
                    }
                    else if (mode == "Edit")
                    {
                        lstvendordetails = lclsservice.GetFacilityVendorAccount(ddlFacility.SelectedItem.Text).Where(a => a.IT == true).Distinct().ToList();
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);

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
                lstLookUp = lclsservice.GetList("ITRequest", "Status", Mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                drpStatussearch.Items.FindByText(PendingApproval).Selected = true;
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
                lstLookUpship = lclsservice.GetList("ITRequest", "Shipping", Mode).ToList();
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lclsservice.SynITReceivingorder();
                SetAddScreen(1);
                ClearDetails();
                SetInitialRow();
                rdorequesttype.Enabled = true;
                rdorequesttype.SelectedValue = "0";
                ddlCorporate.Enabled = true;
                ddlFacility.Enabled = true;
                BindShipping("Add");       

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        private void SetAddScreen(int i)
        {
            try
            {
                if (i == 1)
                {
                    if (defaultPage.Req_RequestITPage_Edit == true && defaultPage.Req_RequestITPage_View == true)
                    {
                        if (defaultPage.RoleID == 1)
                        {
                            btnSave.Visible = true;
                            btnReview.Visible = true;                          
                            btnAdd.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = true;
                            btnReview.Visible = true;                          
                            btnAdd.Visible = false;                           
                        }

                    }
                    if (defaultPage.Req_RequestITPage_Edit == false && defaultPage.Req_RequestITPage_View == true)
                    {
                        btnSave.Visible = false;
                        btnReview.Visible = false;
                        btnSearchNewRow.Visible = false;
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
                    if (defaultPage.Req_RequestITPage_Edit == true && defaultPage.Req_RequestITPage_View == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        #region Bind IT Parts Request Master Values

        public void BindGrid(int TestEdit)
        {
            try
            {
                if (TestEdit == 1)
                {
                    BALRequestIT llstITSearch = new BALRequestIT();
                   
                    if (drpcorsearch.SelectedValue == "All")
                    {
                        llstITSearch.CorporateName = "ALL";
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

                        llstITSearch.CorporateName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpfacilitysearch.SelectedValue == "All")
                    {
                        llstITSearch.FacilityName = "ALL";
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
                        llstITSearch.FacilityName = FinalString;

                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpvendorsearch.SelectedValue == "All")
                    {
                        llstITSearch.VendorName = "ALL";
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
                        llstITSearch.VendorName = FinalString;
                    }
                    FinalString = "";
                    SB.Clear();
                    if (drpStatussearch.SelectedValue == "All")
                    {
                        llstITSearch.Status = "ALL";
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
                        llstITSearch.Status = FinalString;
                    }
                    SB.Clear();
                    if (txtDateFrom.Text != "") llstITSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    if (txtDateTo.Text != "") llstITSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);                    
                    llstITSearch.loggedinBy = defaultPage.UserId;
                    if (rdorequesttypesearch.SelectedValue == "0")
                        llstITSearch.RequestType = false;
                    else if (rdorequesttypesearch.SelectedValue == "1")
                        llstITSearch.RequestType = true;
                    List<SearchITRequest> lstMSRMaster = lclsservice.SearchITRequest(llstITSearch).ToList();
                    GvTempEdit.DataSource = lstMSRMaster;
                    GvTempEdit.DataBind();
                }
                else
                {
                    SearchGrid();
                }
                ViewState["ReportITRequestID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }




        public void BindGridNonSuperAdmin(Int64 CorporateID, Int64 FacilityID)
        {
            try
            {
                List<GetNonsuperAdminRequestITMaster> lstMSRMaster = lclsservice.GetNonsuperAdminRequestITMaster(CorporateID, FacilityID).ToList();
                grdITMastersearch.DataSource = lstMSRMaster;
                grdITMastersearch.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }
        public void SearchGrid()
        {
            try
            {
                BALRequestIT llstITSearch = new BALRequestIT();
               
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstITSearch.CorporateName = "ALL";
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

                    llstITSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstITSearch.FacilityName = "ALL";
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
                    llstITSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstITSearch.VendorName = "ALL";
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
                    llstITSearch.VendorName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstITSearch.Status = "ALL";
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
                    llstITSearch.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstITSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstITSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                llstITSearch.loggedinBy = defaultPage.UserId;
                if (rdorequesttypesearch.SelectedValue == "0")
                    llstITSearch.RequestType = false;
                else if (rdorequesttypesearch.SelectedValue == "1")
                    llstITSearch.RequestType = true;
                List<SearchITRequest> lstMSRMaster = lclsservice.SearchITRequest(llstITSearch).ToList();
                grdITMastersearch.DataSource = lstMSRMaster;
                grdITMastersearch.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }       
        #endregion
        private void ClearDetails()
        {
            ddlCorporate.ClearSelection();
            ddlFacility.ClearSelection();
            ddlVendor.ClearSelection();
            ddlVendor.ClearSelection();           
            ddlShipping.ClearSelection();         
            gvAddITRDetails.DataSource = null;
            gvAddITRDetails.DataBind();
            gvSearchMRPDetails.DataSource = null;
            gvSearchMRPDetails.DataBind();
            GvTempEdit.DataSource = null;
            GvTempEdit.DataBind();            
            rdorequesttype.SelectedIndex = 0;
            ddlCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
            BindFacility(0, "Add");
            ddlFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
            BindVendor(0, "Add");
            btnPrint.Visible = false;
            ViewState["ReportITRequestID"] = "";
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
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
                dt.Columns.Add(new DataColumn("Column8", typeof(string)));
                dt.Columns.Add(new DataColumn("SINo", typeof(string)));
                dt.Columns.Add(new DataColumn("SerialNo", typeof(string)));
                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;
                //Bind the DataTable to the Grid
                gvAddITRDetails.DataSource = dt;
                gvAddITRDetails.DataBind();               
                DropDownList box1 = (DropDownList)gvAddITRDetails.Rows[0].Cells[3].FindControl("drpequipsubcat");
                DropDownList box2 = (DropDownList)gvAddITRDetails.Rows[0].Cells[4].FindControl("drpequiplst");
                TextBox box7 = (TextBox)gvAddITRDetails.Rows[0].Cells[6].FindControl("txtuser");
                TextBox box3 = (TextBox)gvAddITRDetails.Rows[0].Cells[7].FindControl("txtqty");
                TextBox box4 = (TextBox)gvAddITRDetails.Rows[0].Cells[8].FindControl("txtppq");
                TextBox box5 = (TextBox)gvAddITRDetails.Rows[0].Cells[9].FindControl("txttotprice");
                TextBox box6 = (TextBox)gvAddITRDetails.Rows[0].Cells[10].FindControl("txtreason");
                Label box8 = (Label)gvAddITRDetails.Rows[0].Cells[10].FindControl("lblserialNo");
                BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");           

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        public void BindEquipcategory(Int64 CorporateID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList drpequipsubcat = (DropDownList)gvAddITRDetails.Rows[0].Cells[3].FindControl("drpequipsubcat");
                List<BindEquipementsubcategoryFORIT> lstequipcat = lclsservice.BindEquipementsubcategory(Convert.ToInt64(ddlCorporate.SelectedValue), Mode).ToList();
                drpequipsubcat.DataSource = lstequipcat;
                drpequipsubcat.DataValueField = "EquipementSubCategoryID";
                drpequipsubcat.DataTextField = "EquipmentSubCategoryDescription";              
                drpequipsubcat.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpequipsubcat.Items.Insert(0, lst);
                drpequipsubcat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        public void GetEquipementList(long EquipmentsubcatID, string Mode, Int32 rowindex)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                DropDownList drpequiplst = (DropDownList)gvAddITRDetails.Rows[rowindex].Cells[4].FindControl("drpequiplst");
                List<BindEquipementListFORIT> lstequiplist = lclsservice.BindEquipementListFORIT(EquipmentsubcatID, Mode).ToList();
                if (rdorequesttype.SelectedValue != "0")
                {
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        public void BindSerialNo(Int64 EquipmentSubcatID, Int64 EquipmentListID, int rowindex)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Label lblserialNo = (Label)gvAddITRDetails.Rows[rowindex].Cells[5].FindControl("lblserialNo");
                if (rdorequesttype.SelectedValue == "1")
                {
                    lblserialNo.Visible = true;
                    List<GetSerialNo> lstSerialNo = lclsservice.GetSerialNo(EquipmentSubcatID, EquipmentListID).ToList();
                    if (lstSerialNo.Count > 0)
                        lblserialNo.Text = lstSerialNo[0].SerialNo;
                }
                else
                {
                    lblserialNo.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }
        protected void btnReview_Click(object sender, EventArgs e)
        {
            try
            {
                bool isqtycgd = false;
                string equip = string.Empty;
                Int64 revEquipcat = 0;
                Int64 revEquiplist = 0;
                bool isrvalid = false;
                Int64 revfaclityid = Convert.ToInt64(ddlFacility.SelectedValue);

                if (HddMasterID.Value == "" || gvSearchMRPDetails.PageCount == 0)
                {
                    foreach (GridViewRow grdfs in gvAddITRDetails.Rows)
                    {
                        DropDownList drpequipsubcat = (DropDownList)grdfs.FindControl("drpequipsubcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                        Label lbleserialNo = (Label)grdfs.FindControl("lbleserialNo");
                        if (rdorequesttype.SelectedValue != "0")
                        {
                            if (drpequipsubcat.SelectedValue == "0" || drpequlst.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }
                        else
                        {
                            if (drpequipsubcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }

                        revEquipcat = Convert.ToInt64(drpequipsubcat.SelectedValue);
                        revEquiplist = Convert.ToInt64(drpequlst.SelectedValue);
                        List<ValidITEquipment> lstMaster = lclsservice.ValidITEquipment(revEquipcat, revEquiplist, revfaclityid).ToList();
                        if (lstMaster[0].Edit == 1)
                        {
                            isrvalid = true;
                        }
                        else
                        {
                            equip += drpequipsubcat.SelectedItem.Text + ",";
                            isrvalid = false;
                        }

                    }
                    if (ErrorList == "" || Convert.ToString(ViewState["ErrorList"]) == null)
                    {
                        if (isrvalid == true && equip == "")
                        {
                            mpereview.Show();
                            bindreview();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITValidMessage.Replace("<<ITRequestDescription>>", "" + equip), true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", ErrorList), true);
                    }
                }
                else
                {
                    Int64 FootEquipcat = 0;
                    Int64 FootEquiplist = 0;
                    Int64 Footfaclityid = 0;
                    bool isevalid = false;
                    string equipme = string.Empty;

                    foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                    {

                        Label lbleequipcat = (Label)grdfs.FindControl("lbleequipcat");
                        Label lbleequiplst = (Label)grdfs.FindControl("lbleequiplst");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txteqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtePricePerUnit");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txteTotalPrice");
                        Label lbleserialNo = (Label)grdfs.FindControl("lbleserialNo");
                        TextBox txtereason = (TextBox)grdfs.FindControl("txtereason");
                        Label lblqty = (Label)grdfs.FindControl("lblqty");

                        if (txtqty.Text != lblqty.Text)
                        {
                            if (txtereason.Text == "")
                            {
                                isqtycgd = true;
                                txtereason.Style.Add("border", "Solid 1px red");
                            }
                        }
                        if (rdorequesttype.SelectedValue == "")
                        {
                            if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                            {
                                rdorequesttype.SelectedValue = "0";
                            }
                            else
                            {
                                rdorequesttype.SelectedValue = "1";
                            }
                        }
                        if (rdorequesttype.SelectedValue != "0")
                        {

                            if (lbleequipcat.Text == "" || lbleequiplst.Text == "" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }
                        else
                        {
                            if (isqtycgd == true)
                            {
                                ErrorList = "Please Enter Reason";
                            }
                            if (lbleequipcat.Text == "" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }

                    }
                    if (isqtycgd == true)
                    {
                        ErrorList = "Please Enter Reason";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITValidMessage.Replace("<<ITRequestDescription>>", "Please Enter Reason"), true);
                    }
                    
                    if (ErrorList == "" && gvSearchMRPDetails.FooterRow.Visible == true)
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
                        DropDownList drpequcat = control.FindControl("ddlFooteequipcat") as DropDownList;
                        DropDownList drpequlst = control.FindControl("ddlFooteequiplst") as DropDownList;
                        TextBox txtqty = control.FindControl("FooteOrderQuantity") as TextBox;
                        TextBox txtppq = control.FindControl("FootePricePerUnit") as TextBox;
                        TextBox txttotprice = control.FindControl("FooteTotalPrice") as TextBox;
                        TextBox txtfootreason = control.FindControl("txtfootreason") as TextBox;
                        Label lblfootserialno = control.FindControl("lblfootserialno") as Label;
                        TextBox txtfootuser = control.FindControl("txtfootuser") as TextBox;
                        if (rdorequesttype.SelectedValue == "")
                        {
                            if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                            {
                                rdorequesttype.SelectedValue = "0";
                            }
                            else
                            {
                                rdorequesttype.SelectedValue = "1";
                            }
                        }
                        if (rdorequesttype.SelectedValue != "0")
                        {
                            if (drpequcat.SelectedValue == "0" || drpequlst.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                            else
                            {
                                ErrorList = "";
                            }
                        }
                        else
                        {
                            if (drpequcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                            else
                            {
                                ErrorList = "";
                            }

                        }

                        FootEquipcat = Convert.ToInt64(drpequcat.SelectedValue);
                        FootEquiplist = Convert.ToInt64(drpequlst.SelectedValue);
                        Footfaclityid = Convert.ToInt64(ddlFacility.SelectedValue);
                        List<ValidITEquipment> lstMaster = lclsservice.ValidITEquipment(FootEquipcat, FootEquiplist, Footfaclityid).ToList();
                        if (lstMaster[0].Edit == 1)
                        {
                            isevalid = true;
                        }
                        else
                        {
                            equipme += drpequcat.SelectedItem.Text + ",";
                            isevalid = false;
                        }

                        if (ErrorList == "")
                        {
                            if (isevalid == true)
                            {
                                bindreview();
                                mpereview.Show();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITValidMessage.Replace("<<ITRequestDescription>>", "" + equipme), true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", ErrorList), true);
                        }
                    }
                    else
                    {
                        if (ErrorList == "" && isqtycgd == false)
                        {
                           
                            bindreview();
                            mpereview.Show();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", ErrorList), true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        public void bindreview()
        {
            try
            {
                if (ddlCorporate.SelectedItem.Text == "--Select Corporate--")
                    lblCorp.Text = Convert.ToString(ViewState["Corporate"]);
                else
                    lblCorp.Text = ddlCorporate.SelectedItem.Text;

                if (ddlFacility.SelectedValue == "")
                    lblFac.Text = Convert.ToString(ViewState["Facility"]);
                else
                    lblFac.Text = ddlFacility.SelectedItem.Text;
                if (ddlVendor.SelectedValue == "")
                    lblVen.Text = Convert.ToString(ViewState["Vendor"]);
                else
                    lblVen.Text = ddlVendor.SelectedItem.Text;
                if (ddlShipping.SelectedItem.Text == "--Select Shipping--")
                    lblship.Text = Convert.ToString(ViewState["Shipping"]);
                else
                    lblship.Text = ddlShipping.SelectedItem.Text;
                if (rdorequesttype.SelectedItem.Text == "")
                {
                    if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                        lblreqtype.Text = "New";
                    else
                        lblreqtype.Text = "Replacement";
                }
                else
                    lblreqtype.Text = rdorequesttype.SelectedItem.Text;
                lblreqdate.Text = DateTime.Now.ToString("MM/dd/yyyy");              
                lblmprreview.Text = lblMasterNo.Text;
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                dr = null;
                dt.Columns.Add("RowNumber");
                dt.Columns.Add("SINo");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("Equipcat");
                dt.Columns.Add("Equiplst");
                dt.Columns.Add("User");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Priceperqty");
                dt.Columns.Add("TotalPrice");
                dt.Columns.Add("Reason");
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
                    foreach (GridViewRow row in gvAddITRDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = 1;
                        dr["SINo"] = 1;
                        dr["SerialNo"] = (row.FindControl("lblserialNo") as Label).Text;
                        dr["Equipcat"] = (row.FindControl("drpequipsubcat") as DropDownList).SelectedItem.Text;
                        DropDownList drpequiplst = (row.FindControl("drpequiplst") as DropDownList);
                        if (drpequiplst.SelectedValue == "0")
                        {
                            dr["Equiplst"] = "";
                        }
                        else
                        {
                            dr["Equiplst"] = (row.FindControl("drpequiplst") as DropDownList).SelectedItem.Text;
                        }
                        dr["User"] = (row.FindControl("txtuser") as TextBox).Text;
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
                //Edit or Update the Exisiting Machine Parts Request//
                else
                {
                    DivMPRMasterNoreview.Style.Add("display", "block");
                    lblmprreview.Visible = true;
                    List<GetRequestITMaster> llstmpr = lclsservice.GetRequestITMaster().Where(a => a.RequestITMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                    foreach (GridViewRow row in gvSearchMRPDetails.Rows)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = 1;
                        dr["SINo"] = 1;
                        dr["SerialNo"] = (row.FindControl("lbleserialNo") as Label).Text;
                        dr["Equipcat"] = (row.FindControl("lbleequipcat") as Label).Text;
                        dr["Equiplst"] = (row.FindControl("lbleequiplst") as Label).Text;
                        dr["User"] = (row.FindControl("txtsearuser") as Label).Text;
                        dr["Qty"] = (row.FindControl("txteqty") as TextBox).Text;
                        dr["Priceperqty"] = (row.FindControl("txtePricePerUnit") as TextBox).Text;
                        dr["TotalPrice"] = (row.FindControl("txteTotalPrice") as TextBox).Text;
                        dr["Reason"] = (row.FindControl("txtereason") as TextBox).Text;
                        string val = (row.FindControl("txteTotalPrice") as TextBox).Text;
                        dt.Rows.Add(dr);

                    }
                    if (gvSearchMRPDetails.FooterRow.Visible == true)
                    {
                        dr = dt.NewRow();                       
                        Control control = null;
                        if (gvSearchMRPDetails.FooterRow != null)
                        {
                            control = gvSearchMRPDetails.FooterRow;
                        }
                        else
                        {
                            control = gvSearchMRPDetails.Controls[0].Controls[0];
                        }
                        TextBox FooteOrderQuantity = control.FindControl("FooteOrderQuantity") as TextBox;
                        TextBox FootePricePerUnit = control.FindControl("FootePricePerUnit") as TextBox;
                        TextBox txtfootuser = control.FindControl("txtfootuser") as TextBox;
                        TextBox FooteTotalPrice = control.FindControl("FooteTotalPrice") as TextBox;
                        TextBox txtfootreason = control.FindControl("txtfootreason") as TextBox;
                        DropDownList lbleequipcat = control.FindControl("ddlFooteequipcat") as DropDownList;
                        DropDownList lbleequiplst = control.FindControl("ddlFooteequiplst") as DropDownList;
                        Label lblfootserialno = control.FindControl("lblfootserialno") as Label;
                        dr["RowNumber"] = 1;
                        dr["SINo"] = 1;
                        if (lblfootserialno.Text != "")
                            dr["SerialNo"] = lblfootserialno.Text;
                        dr["User"] = txtfootuser.Text;
                        dr["Qty"] = FooteOrderQuantity.Text;
                        dr["Priceperqty"] = FootePricePerUnit.Text;
                        dr["TotalPrice"] = FooteTotalPrice.Text;
                        dr["Reason"] = txtfootreason.Text;
                        dr["Equipcat"] = lbleequipcat.SelectedItem.Text;
                        dr["Equiplst"] = lbleequiplst.SelectedItem.Text;
                        dt.Rows.Add(dr);
                    }
                    grdreview.DataSource = dt;
                    grdreview.DataBind();
                    TextBox txtShippingCost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                    TextBox txtTax = grdreview.FooterRow.FindControl("txttax") as TextBox;
                    TextBox txtTotalCost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                    txtShippingCost.Text = llstmpr[0].Shippingcost;
                    txtTax.Text = llstmpr[0].Tax;
                    txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", llstmpr[0].TotalCost));
                    decimal sum = 0;
                    for (int i = 0; i < grdreview.Rows.Count; ++i)
                    {
                        Label lblTotalPrice = grdreview.Rows[i].FindControl("lblTotalPrice") as Label;
                        sum += Convert.ToDecimal(lblTotalPrice.Text);
                    }
                    (grdreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
                    if (llstmpr[0].Shippingcost != "" && llstmpr[0].Tax != "")
                    {
                        sum += Convert.ToDecimal(llstmpr[0].Shippingcost) + Convert.ToDecimal(llstmpr[0].Tax);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (HddUpdateLockinEdit.Value == "Edit")
                {
                    a = lclsservice.AutoUpdateLockedOut(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId), "ITRequest");
                    HddUpdateLockinEdit.Value = "";
                }
                if (Request.QueryString["ITRNo"] != null)
                {
                    Response.Redirect("RequestITPO.aspx");
                }
                divMPRDetails.Style.Add("display", "none");
                divSearchMachine.Style.Add("display", "none");
                divMPRMaster.Style.Add("diplay", "block");
                divContentDetails.Style.Add("display", "block");
                divContent.Style.Add("display", "block");
                SetAddScreen(0);
                ClearDetails();
                ClearMaster();
                HddMasterID.Value = "";                
                btnPrint.Visible = true;
                btnReview.Enabled = true;              
                if (defaultPage.RoleID != 1)
                {
                    btnReview.Visible = false;                    
                    SearchGrid();
                    if (defaultPage.Req_MachinePartsPage_Edit == false && defaultPage.Req_MachinePartsPage_View == true)
                    {
                        btnAdd.Visible = false;
                    }
                }
                else
                {
                    SearchGrid();
                }
                ViewState["ReportITRequestID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        private void ClearMaster()
        {
            drpvendorsearch.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatussearch.ClearSelection();
            rdorequesttype.SelectedIndex = -1;
            BindStatus("Add");
            BindCorporate(1,"Add");
            BindFacility(1, "Add");
            BindVendor(1, "Add");
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<BindRequestITPartsReport> llstreview = null;
                string RequestITIds = string.Empty;
                if ((ViewState["ReportITRequestID"] == null) || (Convert.ToString(ViewState["ReportITRequestID"]) == ""))
                {
                    foreach (GridViewRow row in grdITMastersearch.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (RequestITIds == string.Empty)
                                RequestITIds = row.Cells[1].Text;
                            else
                                RequestITIds = RequestITIds + "," + row.Cells[1].Text;
                        }
                    }
                    llstreview = lclsservice.BindRequestITPartsReport(null, RequestITIds, defaultPage.UserId,defaultPage.UserId).ToList();
                }
                else
                {
                    RequestITIds = ViewState["ReportITRequestID"].ToString();
                    RequestITIds = RequestITIds.Replace(",", "");
                    llstreview = lclsservice.BindRequestITPartsReport(RequestITIds, null, defaultPage.UserId,defaultPage.UserId).ToList();
                }
                rvITRequestreport.ProcessingMode = ProcessingMode.Local;
                rvITRequestreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITRequest.rdlc");
                string s = Convert.ToString(ViewState["ReportITRequestID"]);
                string q = "1";
                Int64 r = defaultPage.UserId;               
                ReportDataSource datasource = new ReportDataSource("RequestITPartsReportDS", llstreview);
                rvITRequestreport.LocalReport.DataSources.Clear();
                rvITRequestreport.LocalReport.DataSources.Add(datasource);
                rvITRequestreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvITRequestreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITRequset" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlVendor.SelectedValue == "")
                    argRequestIT.VendorID = Convert.ToInt64(ViewState["VendorID"]);
                else
                    argRequestIT.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
                if (ddlCorporate.SelectedValue == "0")
                    argRequestIT.CorporateID = Convert.ToInt64(ViewState["CorporateID"]);
                else
                    argRequestIT.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                if (rdorequesttype.SelectedValue == "")
                {
                    argRequestIT.RequestType = Convert.ToBoolean(ViewState["Requesttype"]);
                }
                else
                {
                    if (rdorequesttype.SelectedValue == "1")
                        argRequestIT.RequestType = true;
                    else
                        argRequestIT.RequestType = false;
                }
                if (ddlShipping.SelectedValue == "0")
                    argRequestIT.Shipping = Convert.ToString(ViewState["Shipping"]);
                else
                    argRequestIT.Shipping = ddlShipping.SelectedValue;
                if (ddlFacility.SelectedValue == "")
                    argRequestIT.FacilityID = Convert.ToInt64(ViewState["FacilityID"]);
                else
                    argRequestIT.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);               
                argRequestIT.DateFrom = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                argRequestIT.Audittrial = "";
                argRequestIT.Remarks = "";
                argRequestIT.CreatedBy = defaultPage.UserId;
                argRequestIT.ShippingCost = (grdreview.FooterRow.FindControl("txtsipcost") as TextBox).Text;
                argRequestIT.Tax = (grdreview.FooterRow.FindControl("txttax") as TextBox).Text;
                argRequestIT.TotalCost = Convert.ToDecimal((grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text);
                if (HddMasterID.Value == "")
                {
                    InsertRequestIT();
                }
                else
                {
                    UpdateRequestIT();

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        public void InsertRequestIT()
        {
            try
            {
                List<object> lstIDwithmessage = new List<object>();
                // string ErrorList = string.Empty;
                foreach (GridViewRow grdfs in gvAddITRDetails.Rows)
                {
                    DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipsubcat");
                    DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                    TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                    TextBox txtuser = (TextBox)grdfs.FindControl("txtuser");
                    TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                    TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                    TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                    Label lblserialNo = (Label)grdfs.FindControl("lblserialNo");
                    if (rdorequesttype.SelectedValue != "0")
                    {
                        if (drpequcat.SelectedValue == "0" || drpequlst.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }
                    }
                    else
                    {
                        if (drpequcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }
                    }

                }
                if (ErrorList == "")
                {
                    // Insert Machine Parts Request Master
                    lstIDwithmessage = lclsservice.InsertITRequestMaster(argRequestIT).ToList();
                    a = lstIDwithmessage[0].ToString();
                    argRequestIT.RequestITMasterID = Convert.ToInt64(lstIDwithmessage[1]);

                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddITRDetails.Rows)
                    {
                        DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipsubcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtuser = (TextBox)grdfs.FindControl("txtuser");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                        Label lblserialNo = (Label)grdfs.FindControl("lblserialNo");
                        argRequestIT.EquipmentCategoryID = Convert.ToInt64(drpequcat.SelectedValue);
                        if (drpequlst.SelectedValue != "Select")
                            argRequestIT.EquipementListID = Convert.ToInt64(drpequlst.SelectedValue);
                        else
                            argRequestIT.EquipementListID = 0;
                        argRequestIT.Qty = Convert.ToInt32(txtqty.Text);
                        argRequestIT.PricePerQty = Convert.ToDecimal(txtppq.Text);
                        argRequestIT.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                        argRequestIT.Reason = txtreason.Text;
                        argRequestIT.User = txtuser.Text;
                        argRequestIT.SerialNo = lblserialNo.Text;

                        // Insert Machine Parts Request Details
                        b = lclsservice.InsertITRequestDetails(argRequestIT);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", "Item Grid fields are should not be Empty"), true);
                }
                if (b == "Saved Successfully")
                {
                    ClearDetails();
                    btnPrint.Visible = true;
                    SetAddScreen(0);                   
                    //if (defaultPage.RoleID != 1)
                    //{
                    //    BindGridNonSuperAdmin(Convert.ToInt64(ddlCorporate.SelectedValue), Convert.ToInt64(ddlFacility.SelectedValue));
                    //}
                    //else
                    //{
                   
                   // }
                   // SearchGrid();
                    List<GetRequestITMaster> lstMSRMaster = new List<GetRequestITMaster>();
                    lstMSRMaster = lclsservice.GetRequestITMaster().ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestSaveMessage.Replace("<<ITRequestDescription>>", lstMSRMaster[0].ITRNo.ToString()), true);
                    ViewState["ReportITRequestID"] = argRequestIT.RequestITMasterID;
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }


        }

        public void UpdateRequestIT()
        {
            try
            {
                // string ErrorList = string.Empty;
                if (gvSearchMRPDetails.PageCount == 0)
                {
                    //Get Grid Details Value
                    foreach (GridViewRow grdfs in gvAddITRDetails.Rows)
                    {
                        DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipsubcat");
                        DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                        TextBox txtuser = (TextBox)grdfs.FindControl("txtuser");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                        Label lblserialNo = (Label)grdfs.FindControl("lblserialNo");
                        argRequestIT.EquipmentCategoryID = Convert.ToInt64(drpequcat.SelectedValue);
                        argRequestIT.EquipementListID = Convert.ToInt64(drpequlst.SelectedValue);
                        argRequestIT.Qty = Convert.ToInt32(txtqty.Text);
                        argRequestIT.PricePerQty = Convert.ToDecimal(txtppq.Text);
                        argRequestIT.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                        argRequestIT.Remarks = txtreason.Text;
                        argRequestIT.SerialNo = lblserialNo.Text;
                        argRequestIT.User = txtuser.Text;
                        argRequestIT.Reason = txtreason.Text;
                        // Insert Machine Parts Request Details
                        a = lclsservice.InsertITRequestDetails(argRequestIT);
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                    {
                        Label drpequcat = (Label)grdfs.FindControl("lbleequipcat");
                        Label drpequlst = (Label)grdfs.FindControl("lbleequiplst");
                        TextBox txtqty = (TextBox)grdfs.FindControl("txteqty");
                        TextBox txtppq = (TextBox)grdfs.FindControl("txtePricePerUnit");
                        TextBox txttotprice = (TextBox)grdfs.FindControl("txteTotalPrice");
                        Label lbleserialNo = (Label)grdfs.FindControl("lbleserialNo");                       
                        if (rdorequesttype.SelectedValue != "0")
                        {
                            if (drpequcat.Text == "" || drpequlst.Text == "" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }
                        else
                        {
                            if (drpequcat.Text == "" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                            {
                                ErrorList = "Item Grid fields are should not be Empty";
                            }
                        }

                    }
                    if (ErrorList == "")
                    {
                        //Get Grid Details Value
                        foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                        {
                            Label drpequcat = (Label)grdfs.FindControl("lbleequipcat");
                            Label drpequlst = (Label)grdfs.FindControl("lbleequiplst");
                            Label drpequcatID = (Label)grdfs.FindControl("lbleequipsubcatID");
                            Label drpequlstID = (Label)grdfs.FindControl("lbleequiplistID");
                            TextBox txtqty = (TextBox)grdfs.FindControl("txteqty");
                            TextBox txtppq = (TextBox)grdfs.FindControl("txtePricePerUnit");
                            TextBox txttotprice = (TextBox)grdfs.FindControl("txteTotalPrice");
                            Label ITRMasterID = (Label)grdfs.FindControl("lbITRMasterID");
                            Label ITRDetailsID = (Label)grdfs.FindControl("lbITRDetailsID");
                            Label lbleserialNo = (Label)grdfs.FindControl("lbleserialNo");
                            Label txtsearuser = (Label)grdfs.FindControl("txtsearuser");
                            TextBox txtereason = (TextBox)grdfs.FindControl("txtereason");
                            argRequestIT.RequestITMasterID = Convert.ToInt64(ITRMasterID.Text);
                            argRequestIT.RequestITDetailID = Convert.ToInt64(ITRDetailsID.Text);
                            argRequestIT.EquipmentCategoryID = Convert.ToInt64(drpequcatID.Text);
                            if (drpequlstID.Text != "")
                                argRequestIT.EquipementListID = Convert.ToInt64(drpequlstID.Text);
                            argRequestIT.Qty = Convert.ToInt32(txtqty.Text);
                            argRequestIT.PricePerQty = Convert.ToDecimal(txtppq.Text);
                            argRequestIT.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                            argRequestIT.LastModifiedBy = defaultPage.UserId;
                            argRequestIT.SerialNo = lbleserialNo.Text;
                            argRequestIT.User = txtsearuser.Text;
                            argRequestIT.Reason = txtereason.Text;
                            // Update Machine Parts Request Details
                            b = lclsservice.UpdateITRequestDetails(argRequestIT);                            
                        }
                        // Update Machine Parts Request Master
                        a = lclsservice.UpdateITRequestMaster(argRequestIT);                       
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", "Item Grid fields are should not be Empty"), true);
                    }
                    if (gvSearchMRPDetails.FooterRow.Visible == true)
                    {
                        FooterRowSave();

                    }

                }

                if (a == "Saved Successfully")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestSaveMessage.Replace("<<ITRequestDescription>>", lblmprreview.Text), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    btnClose.Visible = true;
                }
                else if (b == "Updated Successfully")
                {
                    List<GetMPRMaster> lstMPRMaster = lclsservice.GetMPRMaster().ToList();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestMessage.Replace("<<ITRequestDescription>>", lblmprreview.Text), true);
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    btnClose.Visible = true;

                }              


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }

        }
        protected void btnSearchNewRow_Click(object sender, EventArgs e)
        {
            if (gvSearchMRPDetails.PageCount != 0)
            {                
                gvSearchMRPDetails.FooterRow.Visible = true;
                BindFooterEquipSubCategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            }
            else
            {
                divAddMachine.Style.Add("display", "block");
                divSearchMachine.Style.Add("display", "none");
                SetInitialRow();
            }

        }
        protected void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow grdfs in gvAddITRDetails.Rows)
                {
                    DropDownList drpequcat = (DropDownList)grdfs.FindControl("drpequipsubcat");
                    DropDownList drpequlst = (DropDownList)grdfs.FindControl("drpequiplst");
                    TextBox txtqty = (TextBox)grdfs.FindControl("txtqty");
                    TextBox txtuser = (TextBox)grdfs.FindControl("txtuser");
                    TextBox txtppq = (TextBox)grdfs.FindControl("txtppq");
                    TextBox txttotprice = (TextBox)grdfs.FindControl("txttotprice");
                    // TextBox txtreason = (TextBox)grdfs.FindControl("txtreason");
                    if (rdorequesttype.SelectedValue != "0")
                    {
                        if (drpequcat.SelectedValue == "0" || drpequlst.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
                        {
                            ErrorList = "Item Grid fields are should not be Empty";
                        }
                    }
                    else
                    {
                        if (drpequcat.SelectedValue == "0" || txtqty.Text == "" || txtppq.Text == "" || txttotprice.Text == "")
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", "Item Grid fields are should not be Empty"), true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }

        }
        public void AddNewRowToGrid()
        {
            try
            {

                if (ViewState["CurrentTable"] != null)
                {
                    //SetPreviousData();
                    DataTable Dummytable = new DataTable();
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;

                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;

                        //add new row to DataTable   
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["CurrentTable"] = dtCurrentTable;                       

                    }
                    for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                    {
                        //extract the TextBox values   

                        DropDownList box1 = (DropDownList)gvAddITRDetails.Rows[i].Cells[3].FindControl("drpequipsubcat");
                        DropDownList box2 = (DropDownList)gvAddITRDetails.Rows[i].Cells[4].FindControl("drpequiplst");
                        TextBox box7 = (TextBox)gvAddITRDetails.Rows[i].Cells[6].FindControl("txtuser");
                        TextBox box3 = (TextBox)gvAddITRDetails.Rows[i].Cells[7].FindControl("txtqty");
                        TextBox box4 = (TextBox)gvAddITRDetails.Rows[i].Cells[8].FindControl("txtppq");
                        TextBox box5 = (TextBox)gvAddITRDetails.Rows[i].Cells[9].FindControl("txttotprice");
                        TextBox box6 = (TextBox)gvAddITRDetails.Rows[i].Cells[10].FindControl("txtreason");
                        Label box8 = (Label)gvAddITRDetails.Rows[i].Cells[5].FindControl("lblserialNo");                       
                        if (i == dtCurrentTable.Rows.Count - 1)
                        {
                            dtCurrentTable.Rows[i]["Column1"] = box1.SelectedItem.Text;
                            dtCurrentTable.Rows[i]["Column2"] = box2.SelectedItem.Text;
                            dtCurrentTable.Rows[i]["Column3"] = box3.Text;
                            dtCurrentTable.Rows[i]["Column4"] = box4.Text;
                            dtCurrentTable.Rows[i]["Column5"] = box5.Text;
                            dtCurrentTable.Rows[i]["Column6"] = box6.Text;
                            dtCurrentTable.Rows[i]["Column7"] = box7.Text;
                            dtCurrentTable.Rows[i]["Column8"] = box8.Text;
                        }                       
                        dtCurrentTable.Rows[i]["Column1"] = box1.SelectedItem.Text;                       
                        dtCurrentTable.Rows[i]["Column2"] = box2.SelectedItem.Text;                     
                        dtCurrentTable.Rows[i]["Column3"] = box3.Text;
                        dtCurrentTable.Rows[i]["Column4"] = box4.Text;
                        dtCurrentTable.Rows[i]["Column5"] = box5.Text;
                        dtCurrentTable.Rows[i]["Column6"] = box6.Text;
                        dtCurrentTable.Rows[i]["Column7"] = box7.Text;
                        dtCurrentTable.Rows[i]["Column8"] = box8.Text;
                    }

                    //Rebind the Grid with the current data to reflect changes   
                    gvAddITRDetails.DataSource = dtCurrentTable;
                    gvAddITRDetails.DataBind();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                            DropDownList box1 = (DropDownList)gvAddITRDetails.Rows[rowIndex].Cells[3].FindControl("drpequipsubcat");
                            DropDownList box2 = (DropDownList)gvAddITRDetails.Rows[rowIndex].Cells[4].FindControl("drpequiplst");
                            TextBox box3 = (TextBox)gvAddITRDetails.Rows[rowIndex].Cells[7].FindControl("txtqty");
                            TextBox box4 = (TextBox)gvAddITRDetails.Rows[rowIndex].Cells[8].FindControl("txtppq");
                            TextBox box5 = (TextBox)gvAddITRDetails.Rows[rowIndex].Cells[9].FindControl("txttotprice");
                            TextBox box6 = (TextBox)gvAddITRDetails.Rows[rowIndex].Cells[10].FindControl("txtreason");
                            TextBox box7 = (TextBox)gvAddITRDetails.Rows[rowIndex].Cells[6].FindControl("txtuser");
                            Label box8 = (Label)gvAddITRDetails.Rows[rowIndex].Cells[5].FindControl("lblserialNo");
                            

                            if (dt.Rows[i]["Column1"].ToString() != "")
                            {
                                box1.Items.FindByText(dt.Rows[i]["Column1"].ToString()).Selected = true;
                                if (rdorequesttype.SelectedValue != "0")
                                {
                                    box2.DataSource = BindEquipementListFORIT(Convert.ToInt32(box1.SelectedValue), "Add");
                                    box2.DataValueField = "EquipementListID";
                                    box2.DataTextField = "EquipmentListDescription";
                                    box2.DataBind();                                  
                                    box2.Items.FindByText(dt.Rows[i]["Column2"].ToString()).Selected = true;
                                }
                                else
                                {
                                    ListItem lst = new ListItem();
                                    lst.Value = "0";
                                    lst.Text = "Select";
                                    box2.Items.Insert(0, lst);
                                }

                            }
                            box3.Text = dt.Rows[i]["Column3"].ToString();
                            box4.Text = dt.Rows[i]["Column4"].ToString();
                            box5.Text = dt.Rows[i]["Column5"].ToString();
                            box6.Text = dt.Rows[i]["Column6"].ToString();
                            box7.Text = dt.Rows[i]["Column7"].ToString();
                            box8.Text = dt.Rows[i]["Column8"].ToString();
                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }


        public List<BindEquipementListFORIT> BindEquipementListFORIT(int EquimentSubCategoryID, string Mode)
        {

            InventoryServiceClient lclsservice = new InventoryServiceClient();
            List<BindEquipementListFORIT> lstequiplist = lclsservice.BindEquipementListFORIT(EquimentSubCategoryID, Mode).ToList();
            return lstequiplist;
        }

        protected void drpequipsubcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {               
                DropDownList ddlSpin2 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
                int RowIndex = gridrow.RowIndex;
                DropDownList drpequipsubcat = (DropDownList)gvAddITRDetails.Rows[RowIndex].Cells[3].FindControl("drpequipsubcat");
                DropDownList drpequiplst = (DropDownList)gvAddITRDetails.Rows[RowIndex].Cells[4].FindControl("drpequiplst");
                if (rdorequesttype.SelectedValue != "0")
                {
                    GetEquipementList(Convert.ToInt64(drpequipsubcat.SelectedValue), "Add", RowIndex);
                    drpequiplst.Enabled = true;

                }
                else
                {
                    drpequiplst.Enabled = false;
                }

                //SetPreviousData();
                RebindGrid();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string status = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                EditDisplayControls();                
                hdncheckfield.Value = "1";
                lblEditHeader.Visible = true;
                lblseroutHeader.Visible = false;
                lblUpdateHeader.Visible = false;
                lblMasterHeader.Visible = true;
                HddUpdateLockinEdit.Value = "Edit";
                HddMasterID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                HddUserID.Value = defaultPage.UserId.ToString();
                ViewState["ReportITRequestID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                lblMasterNo.Text = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                ViewState["ITRNo"] = lblMasterNo.Text;
                ddlCorporate.ClearSelection();
                if (gvrow.Cells[2].Text == "&nbsp;")
                {
                    ddlCorporate.Items.FindByText("--Select Corporate--").Selected = true;
                }
                else
                {
                    ddlCorporate.SelectedValue = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                }
                BindFacility(0, "Edit");
                ddlFacility.ClearSelection();
                if (gvrow.Cells[3].Text == "&nbsp;")
                {
                    ddlFacility.Items.FindByText("--Select Facility--").Selected = true;
                }
                else
                {
                    ddlFacility.SelectedValue = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                }
                BindVendor(0, "Edit");
                ddlVendor.ClearSelection();
                if (gvrow.Cells[4].Text == "&nbsp;")
                {
                    ddlVendor.Items.FindByText("--Select Vendor--").Selected = true;
                }
                else
                {
                    ddlVendor.SelectedValue = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                }
                BindShipping("Edit");
                ddlShipping.ClearSelection();
                if (gvrow.Cells[15].Text == "&nbsp;")
                {
                    ddlShipping.Items.FindByText("--Select Shipping--").Selected = true;
                }
                else
                {
                    ddlShipping.SelectedValue = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                }
                if (gvrow.Cells[10].Text.Trim() == "New")
                {
                    rdorequesttype.SelectedValue = "0";
                }
                else
                {
                    rdorequesttype.SelectedValue = "1";
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                rdorequesttype.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        public void BindDetailGrid(Int64 RequestITMasterID, Int64 UserId)
        {
            try
            {
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetITRequestDetailsbyMasterID> llstMPRMaster = lclsservice.GetITRequestDetailsbyMasterID(RequestITMasterID, UserId, Convert.ToInt64(LockTimeOut)).ToList();
                if (llstMPRMaster.Count > 0)
                {
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", "Another user " + llstuserdetails[0].LastName + "," + llstuserdetails[0].FirstName + " is updating this record , Please try after some time."), true);
                    }
                }
                gvSearchMRPDetails.DataSource = llstMPRMaster;
                gvSearchMRPDetails.DataBind();               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        #region Bind Footer Equimentsubcategory Values
        public void BindFooterEquipSubCategory(Int64 CorporateID, string Mode)
        {
            List<GetFacilityVendorAccount> lstvendordetails = null;
            try
            {
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                lstvendordetails = new List<GetFacilityVendorAccount>();
                DropDownList drpFooterequcat = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequipcat") as DropDownList;
                // Insert Drop Down                
                List<BindEquipementsubcategoryFORIT> lstequipcat = lclsservice.BindEquipementsubcategory(Convert.ToInt64(ddlCorporate.SelectedValue), Mode).ToList();
                drpFooterequcat.DataSource = lstequipcat;
                drpFooterequcat.DataTextField = "EquipmentSubCategoryDescription";
                drpFooterequcat.DataValueField = "EquipementSubCategoryID";
                drpFooterequcat.DataBind();
                drpFooterequcat.Items.Insert(0, lst);
                drpFooterequcat.SelectedIndex = 0;

            }
            catch (Exception ex)
            {               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);

            }

        }
        #endregion
        #region Bind Footer EquimentList Values
        public void BindFooterEquipList(Int64 EquipmentsubcatID, string Mode)
        {
            List<GetFacilityVendorAccount> lstvendordetails = null;
            try
            {
                lstvendordetails = new List<GetFacilityVendorAccount>();
                DropDownList ddlFooteequiplst = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequiplst") as DropDownList;
                // Insert Drop Down                
                List<BindEquipementListFORIT> lstequiplist = lclsservice.BindEquipementListFORIT(EquipmentsubcatID, Mode).ToList();
                ddlFooteequiplst.DataSource = lstequiplist;
                ddlFooteequiplst.DataTextField = "EquipmentListDescription";
                ddlFooteequiplst.DataValueField = "EquipementListID";
                ddlFooteequiplst.DataBind(); 
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);

            }           

        }
        #endregion

        protected void ddlFooteequipcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //SetPreviousData();
                DropDownList drpFooterequcat = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequipcat") as DropDownList;
                DropDownList ddlFooteequiplst = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequiplst") as DropDownList;
                if (rdorequesttype.SelectedValue == "")
                {
                    if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                    {
                        rdorequesttype.SelectedValue = "0";
                    }
                    else
                    {
                        rdorequesttype.SelectedValue = "1";
                    }
                }

                if (rdorequesttype.SelectedValue != "0")
                {
                    BindFooterEquipList(Convert.ToInt64(drpFooterequcat.SelectedValue), "Add");
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    ddlFooteequiplst.Items.Insert(0, lst);
                    ddlFooteequiplst.Enabled = true;
                }
                else
                {
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    ddlFooteequiplst.Items.Insert(0, lst);
                    ddlFooteequiplst.Enabled = false;
                }
                RebindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }

        }

        protected void drpequiplst_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSpin2 = (DropDownList)sender;
            GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
            int RowIndex = gridrow.RowIndex;
            DropDownList drpequipsubcat = (DropDownList)gvAddITRDetails.Rows[RowIndex].Cells[3].FindControl("drpequipsubcat");
            DropDownList drpequiplst = (DropDownList)gvAddITRDetails.Rows[RowIndex].Cells[4].FindControl("drpequiplst");
            BindSerialNo(Convert.ToInt64(drpequipsubcat.SelectedValue), Convert.ToInt64(drpequiplst.SelectedValue), RowIndex);
            RebindGrid();
            //SetPreviousData();
        }

        protected void ddlFooteequiplst_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlFooteequipcat = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequipcat") as DropDownList;
            DropDownList ddlFooteequiplst = gvSearchMRPDetails.FooterRow.FindControl("ddlFooteequiplst") as DropDownList;
            Label lbleserialNo = (Label)gvSearchMRPDetails.Rows[0].Cells[4].FindControl("lbleserialNo");
            List<GetSerialNo> lstSerialNo = lclsservice.GetSerialNo(Convert.ToInt64(ddlFooteequipcat.SelectedValue), Convert.ToInt64(ddlFooteequiplst.SelectedValue)).ToList();
            if (lstSerialNo.Count > 0)
                lbleserialNo.Text = lstSerialNo[0].SerialNo;
            RebindGrid();
        }
        protected void btnSearchDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label MPRDetailsID = (Label)gvrow.FindControl("lbITRDetailsID");
                Label lbITRMasterID = (Label)gvrow.FindControl("lbITRMasterID");
                HddDetailsID.Value = MPRDetailsID.Text;
                HddMasterID.Value = lbITRMasterID.Text;              
                HddDetailRowID.Value = gvrow.RowIndex.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnClose.Visible = false;
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteITRDetails(Convert.ToInt64(HddDetailsID.Value), false, defaultPage.UserId);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindDetailGrid(Convert.ToInt64(HddMasterID.Value), Convert.ToInt64(defaultPage.UserId));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.DeleteITRDetailsDeleteMessage.Replace("<<DeleteITRDetails>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                        gvAddITRDetails.DataSource = dt;
                        gvAddITRDetails.DataBind();

                        for (int i = 0; i < gvAddITRDetails.Rows.Count - 1; i++)
                        {
                            gvAddITRDetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        }
                        SetPreviousData();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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

                            DropDownList box1 = (DropDownList)gvAddITRDetails.Rows[i - 1].Cells[3].FindControl("drpequipsubcat");
                            DropDownList box2 = (DropDownList)gvAddITRDetails.Rows[i - 1].Cells[4].FindControl("drpequiplst");
                            TextBox box3 = (TextBox)gvAddITRDetails.Rows[i - 1].Cells[7].FindControl("txtqty");
                            TextBox box4 = (TextBox)gvAddITRDetails.Rows[i - 1].Cells[8].FindControl("txtppq");
                            TextBox box5 = (TextBox)gvAddITRDetails.Rows[i - 1].Cells[9].FindControl("txttotprice");
                            TextBox box6 = (TextBox)gvAddITRDetails.Rows[i - 1].Cells[10].FindControl("txtreason");
                            TextBox box7 = (TextBox)gvAddITRDetails.Rows[i - 1].Cells[6].FindControl("txtuser");
                            Label box8 = (Label)gvAddITRDetails.Rows[i - 1].Cells[5].FindControl("lblserialNo");

                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["RowNumber"] = i + 1;
                            if (box1.SelectedItem.Text != "Select")
                            {
                                dtCurrentTable.Rows[i - 1]["Column1"] = box1.SelectedItem.Text;

                                dtCurrentTable.Rows[i - 1]["Column2"] = box2.SelectedItem.Text;
                            }
                            dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                            dtCurrentTable.Rows[i - 1]["Column4"] = box4.Text;
                            dtCurrentTable.Rows[i - 1]["Column5"] = box5.Text;
                            dtCurrentTable.Rows[i - 1]["Column6"] = box6.Text;
                            dtCurrentTable.Rows[i - 1]["Column7"] = box7.Text;
                            dtCurrentTable.Rows[i - 1]["Column8"] = box8.Text;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string ITRequestID = string.Empty;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ITRequestID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<BindRequestITPartsReport> llstreview = lclsservice.BindRequestITPartsReport(ITRequestID, null, defaultPage.UserId,defaultPage.UserId).ToList();
                rvITRequestreport.ProcessingMode = ProcessingMode.Local;
                rvITRequestreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITRequest.rdlc");
                string s = ITRequestID;
                string q = "1";
                Int64 r = defaultPage.UserId;
                ReportDataSource datasource = new ReportDataSource("RequestITPartsReportDS", llstreview);
                rvITRequestreport.LocalReport.DataSources.Clear();
                rvITRequestreport.LocalReport.DataSources.Add(datasource);
                rvITRequestreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvITRequestreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITRequset" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void gvAddITRDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Int64 CorporateID;
                    CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                    string Mode = "Add";
                    //Find the DropDownList in the Row
                    List<BindEquipementsubcategoryFORIT> lstequipcat = lclsservice.BindEquipementsubcategory(CorporateID, Mode).ToList();
                    DropDownList drpequipsubcat = (e.Row.FindControl("drpequipsubcat") as DropDownList);
                    DropDownList drpequiplst = (e.Row.FindControl("drpequiplst") as DropDownList);
                    drpequipsubcat.DataSource = lstequipcat;
                    drpequipsubcat.DataValueField = "EquipementSubCategoryID";
                    drpequipsubcat.DataTextField = "EquipmentSubCategoryDescription";
                    drpequipsubcat.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    drpequipsubcat.Items.Insert(0, lst);
                    drpequiplst.Items.Insert(0, lst);                                 
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
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
                TextBox txtPricePerUnit = (TextBox)gvAddITRDetails.Rows[index].FindControl("txtppq");
                TextBox txtOrderQuantity = (TextBox)gvAddITRDetails.Rows[index].FindControl("txtqty");
                TextBox txtTotalPrice = (TextBox)gvAddITRDetails.Rows[index].FindControl("txttotprice");
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
                TextBox txtPricePerUnit = (TextBox)gvr.Cells[index].FindControl("txtePricePerUnit");
                TextBox txtOrderQuantity = (TextBox)gvr.Cells[index].FindControl("txteqty");
                TextBox txtTotalPrice = (TextBox)gvr.Cells[index].FindControl("txteTotalPrice");
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
        public void RebindGrid()
        {
            try
            {
                if (HddMasterID.Value == "")
                {
                    for (int i = 0; i <= gvAddITRDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvAddITRDetails.Rows[Convert.ToInt32(i)];
                        DropDownList ddl1 = (DropDownList)grdfs.FindControl("drpequipsubcat");
                        DropDownList ddl2 = (DropDownList)grdfs.FindControl("drpequiplst");
                        Label box1 = (Label)grdfs.FindControl("lblserialNo");
                        TextBox box2 = (TextBox)grdfs.FindControl("txtqty");
                        TextBox box3 = (TextBox)grdfs.FindControl("txtppq");
                        TextBox box4 = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox box5 = (TextBox)grdfs.FindControl("txttotprice");
                        TextBox box6 = (TextBox)grdfs.FindControl("txtuser");
                        TextBox box7 = (TextBox)grdfs.FindControl("txtreason");

                        if (box4.Text == "")
                        {
                            CalTotalPrice(box4);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= gvSearchMRPDetails.Rows.Count - 1; i++)
                    {
                        GridViewRow grdfs = gvSearchMRPDetails.Rows[Convert.ToInt32(i)];
                        Label ddl1 = (Label)grdfs.FindControl("lbleequipcat");
                        Label ddl2 = (Label)grdfs.FindControl("lbleequiplst");
                        Label box1 = (Label)grdfs.FindControl("lbleserialNo");
                        Label box6 = (Label)grdfs.FindControl("txtsearuser");
                        TextBox box2 = (TextBox)grdfs.FindControl("txteqty");
                        TextBox box3 = (TextBox)grdfs.FindControl("txtePricePerUnit");
                        TextBox box4 = (TextBox)grdfs.FindControl("txteTotalPrice");
                        TextBox box5 = (TextBox)grdfs.FindControl("txtereason");
                        if (box4.Text == "")
                        {
                            CalTotalPriceUpdate(box4);
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
                        DropDownList ItemID = control.FindControl("ddlFooteequipcat") as DropDownList;
                        DropDownList ItemDescription = control.FindControl("ddlFooteequiplst") as DropDownList;
                        TextBox FooteOrderQuantity = control.FindControl("FooteOrderQuantity") as TextBox;
                        TextBox FootePricePerUnit = control.FindControl("FootePricePerUnit") as TextBox;
                        TextBox FooteTotalPrice = control.FindControl("FooteTotalPrice") as TextBox;
                        TextBox txtfootreason = control.FindControl("txtfootreason") as TextBox;
                        if (FooteTotalPrice.Text == "")
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

        protected void txteqty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtqty = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)txtqty.NamingContainer;
                int index = Convert.ToInt16(gvr.RowIndex);
                TextBox txtereason = (TextBox)gvSearchMRPDetails.Rows[index].FindControl("txtereason");

                if (txtereason.Text == "")
                {
                    ErrorList = "Reason should be Required for Modified Qty in Items Grid";
                    ViewState["ErrorList"] = ErrorList;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqMessage.Replace("<<ITRequestDescription>>", Convert.ToString(ViewState["ErrorList"])), true);
                }
                else
                {
                    ViewState["ErrorList"] = "";
                }



                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void gvSearchMRPDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int64 CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                Control control = null;
                string Mode = "Add";
                control = gvSearchMRPDetails.FooterRow;
                //Find the DropDownList in the Row
                List<BindEquipementsubcategoryFORIT> lstequipcat = lclsservice.BindEquipementsubcategory(CorporateID, Mode).ToList();
                DropDownList drpequipsubcat = control.FindControl("ddlFooteequipcat") as DropDownList;
                DropDownList drpequiplst = control.FindControl("ddlFooteequiplst") as DropDownList;
                drpequipsubcat.DataSource = lstequipcat;
                drpequipsubcat.DataValueField = "EquipementSubCategoryID";
                drpequipsubcat.DataTextField = "EquipmentSubCategoryDescription";
                drpequipsubcat.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpequipsubcat.Items.Insert(0, lst);
                drpequiplst.Items.Insert(0, lst);                            
            }
        }

        public void CalFootRow()
        {
            try
            {
                Control control = null;
                gvSearchMRPDetails.FooterRow.Visible = true;
                if (gvSearchMRPDetails.FooterRow != null)
                {
                    control = gvSearchMRPDetails.FooterRow;
                }
                else
                {
                    control = gvSearchMRPDetails.Controls[0].Controls[0];
                }

                string PricePerUnit = (control.FindControl("FootePricePerUnit") as TextBox).Text;
                string OrderQuantity = (control.FindControl("FooteOrderQuantity") as TextBox).Text;
                TextBox TotalPrice = (control.FindControl("FooteTotalPrice") as TextBox);
                if (PricePerUnit != "" && OrderQuantity != "")
                {
                    decimal Footde = (Convert.ToDecimal(PricePerUnit) * Convert.ToInt32(OrderQuantity));
                    TotalPrice.Text = string.Format("{0:F2}", Footde).ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true); ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsRequestErrorMessage.Replace("<<MachinePartsRequestDescription>>", ex.Message.ToString()), true);
            }
        }

        protected void FooteOrderQuantity_TextChanged(object sender, EventArgs e)
        {
            CalFootRow();
        }
        public void FooterRowSave()
        {
            try
            {
                Int64 footEquipcat = 0;
                Int64 footEquiplist = 0;
                Int64 footfaclityid = 0;
                bool isvalid = false;
                string validequ = string.Empty;
                BALMPRMaster llstMPRMaster = new BALMPRMaster();
                string b = string.Empty;
                foreach (GridViewRow grdfs in gvSearchMRPDetails.Rows)
                {
                    Label drpequcat = (Label)grdfs.FindControl("lbleequipcat");
                    Label drpequlst = (Label)grdfs.FindControl("lbleequiplst");
                    Label drpequcatID = (Label)grdfs.FindControl("lbleequipsubcatID");
                    Label drpequlstID = (Label)grdfs.FindControl("lbleequiplistID");
                    TextBox txtqty = (TextBox)grdfs.FindControl("txteqty");
                    TextBox txtppq = (TextBox)grdfs.FindControl("txtePricePerUnit");
                    TextBox txttotprice = (TextBox)grdfs.FindControl("txteTotalPrice");
                    Label ITRMasterID = (Label)grdfs.FindControl("lbITRMasterID");
                    Label ITRDetailsID = (Label)grdfs.FindControl("lbITRDetailsID");
                    Label lbleserialNo = (Label)grdfs.FindControl("lbleserialNo");
                    Label txtsearuser = (Label)grdfs.FindControl("txtsearuser");
                    TextBox txtereason = (TextBox)grdfs.FindControl("txtereason");
                    argRequestIT.RequestITMasterID = Convert.ToInt64(ITRMasterID.Text);
                    argRequestIT.RequestITDetailID = Convert.ToInt64(ITRDetailsID.Text);
                    argRequestIT.EquipmentCategoryID = Convert.ToInt64(drpequcatID.Text);
                    if (drpequlstID.Text != "")
                        argRequestIT.EquipementListID = Convert.ToInt64(drpequlstID.Text);
                    argRequestIT.Qty = Convert.ToInt32(txtqty.Text);
                    argRequestIT.PricePerQty = Convert.ToDecimal(txtppq.Text);
                    argRequestIT.TotalPrice = Convert.ToDecimal(txttotprice.Text);
                    argRequestIT.LastModifiedBy = defaultPage.UserId;
                    argRequestIT.SerialNo = lbleserialNo.Text;
                    argRequestIT.User = txtsearuser.Text;
                    argRequestIT.Reason = txtereason.Text;
                    // Update Machine Parts Request Details
                    b = lclsservice.UpdateITRequestDetails(argRequestIT);
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



                string ErrorList = string.Empty;
                llstMPRMaster.MPRMasterID = Convert.ToInt64(HddMasterID.Value);
                if (ddlFacility.SelectedValue == "")
                    llstMPRMaster.FacilityID = Convert.ToInt64(ViewState["FacilityID"]);
                else
                    llstMPRMaster.FacilityID = Convert.ToInt64(ddlFacility.SelectedValue);
                if (ddlVendor.SelectedValue == "")
                    llstMPRMaster.VendorID = Convert.ToInt64(ViewState["VendorID"]);
                else
                    llstMPRMaster.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);              
                llstMPRMaster.CreatedBy = defaultPage.UserId;
                DropDownList ddlFooteequipcat = control.FindControl("ddlFooteequipcat") as DropDownList;
                DropDownList ddlFooteequiplst = control.FindControl("ddlFooteequiplst") as DropDownList;
                TextBox FooteOrderQuantity = control.FindControl("FooteOrderQuantity") as TextBox;
                TextBox FootePricePerUnit = control.FindControl("FootePricePerUnit") as TextBox;
                TextBox FooteTotalPrice = control.FindControl("FooteTotalPrice") as TextBox;
                TextBox txtfootreason = control.FindControl("txtfootreason") as TextBox;
                Label lblfootserialno = control.FindControl("lblfootserialno") as Label;
                TextBox txtfootuser = control.FindControl("txtfootuser") as TextBox;
                if (rdorequesttype.SelectedValue == "")
                {
                    if (Convert.ToBoolean(ViewState["Requesttype"]) == false)
                    {
                        rdorequesttype.SelectedValue = "0";
                    }
                    else
                    {
                        rdorequesttype.SelectedValue = "1";
                    }
                }
                if (rdorequesttype.SelectedValue != "0")
                {
                    if (ddlFooteequipcat.SelectedValue == "0" || ddlFooteequiplst.SelectedValue == "0" || FooteOrderQuantity.Text == "" || FootePricePerUnit.Text == "" || FooteTotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }
                else
                {
                    if (ddlFooteequipcat.SelectedValue == "0" || FooteOrderQuantity.Text == "" || FootePricePerUnit.Text == "" || FooteTotalPrice.Text == "")
                    {
                        ErrorList = "Item Grid fields are should not be Empty";
                    }
                }
                if (ErrorList == "")
                {                   
                    argRequestIT.EquipmentCategoryID = Convert.ToInt64(ddlFooteequipcat.SelectedValue);
                    if (ddlFooteequiplst.SelectedValue != "0")
                        argRequestIT.EquipementListID = Convert.ToInt64(ddlFooteequiplst.SelectedValue);
                    argRequestIT.Qty = Convert.ToInt32(FooteOrderQuantity.Text);
                    argRequestIT.PricePerQty = Convert.ToDecimal(FootePricePerUnit.Text);
                    argRequestIT.TotalPrice = Convert.ToDecimal(FooteTotalPrice.Text);
                    argRequestIT.LastModifiedBy = defaultPage.UserId;
                    argRequestIT.SerialNo = lblfootserialno.Text;
                    argRequestIT.User = txtfootuser.Text;
                    argRequestIT.Reason = txtfootreason.Text;
                    // Insert Machine Parts Request Details


                    footEquipcat = argRequestIT.EquipmentCategoryID;
                    footEquiplist = argRequestIT.EquipementListID;
                    footfaclityid = llstMPRMaster.FacilityID;

                    List<ValidITEquipment> lstMaster = lclsservice.ValidITEquipment(footEquipcat, footEquiplist, footfaclityid).ToList();
                    if (lstMaster[0].Edit == 1)
                    {
                        isvalid = true;
                    }
                    else
                    {
                        validequ += ddlFooteequipcat.SelectedItem.Text + ",";
                        isvalid = false;
                    }

                    if( isvalid == true)
                    {
                        b = lclsservice.InsertITRequestDetails(argRequestIT);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITValidMessage.Replace("<<ITRequestDescription>>", "" + validequ), true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachinePartsReqMessage.Replace("<<MachinePartsRequestDescription>>", ErrorList), true);
                }


                if (b == "Saved Successfully")
                {                  
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
                DropDownList ddlFooteequipcat = control.FindControl("ddlFooteequipcat") as DropDownList;
                DropDownList ddlFooteequiplst = control.FindControl("ddlFooteequiplst") as DropDownList;
                Label lblfootserialno = control.FindControl("lblfootserialno") as Label;
                TextBox txtfootuser = control.FindControl("txtfootuser") as TextBox;
                TextBox FooteOrderQuantity = control.FindControl("FooteOrderQuantity") as TextBox;
                TextBox FootePricePerUnit = control.FindControl("FootePricePerUnit") as TextBox;
                TextBox FooteTotalPrice = control.FindControl("FooteTotalPrice") as TextBox;
                ddlFooteequipcat.SelectedIndex = -1;
                ddlFooteequiplst.SelectedIndex = -1;
                lblfootserialno.Text = "";
                txtfootuser.Text = "";
                FooteOrderQuantity.Text = "";
                FootePricePerUnit.Text = "";
                FooteTotalPrice.Text = "";
            }
            catch (Exception ex)
            {               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message), true);
            }
        }

        protected void grdITMastersearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                    //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                    //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                    string status = e.Row.Cells[12].Text;
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

                    if (defaultPage.UserId == 1)
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
                    if (defaultPage.UserId != 1)
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestErrorMessage.Replace("<<ITRequestDescription>>", ex.Message.ToString()), true);
            }
        }

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

        protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void ddlCorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFacility(0, "Add");
            BindEquipcategory(Convert.ToInt64(ddlCorporate.SelectedValue), "Add");
            RebindGrid();
        }

        protected void ddlFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVendor(0, "Add");
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

        protected void GvTempEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                string status = e.Row.Cells[12].Text;
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
                divITrequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divITrequest.Attributes["class"] = "mypanel-body";
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
                divITrequest.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divITrequest.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divITrequest.Attributes["class"] = "Upopacity";
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
                    divITrequest.Attributes["class"] = "Upopacity";
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