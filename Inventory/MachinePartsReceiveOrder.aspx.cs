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
'' Name      :   <<MachinePartsReceiveOrder>>
'' Type      :   C# File
'' Description  :<<To add,update,delete the MachinePartsReceiveOrder Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	02/13/2018		   V1.0				   Sairam P	                     New
''--------------------------------------------------------------------------------
'*/

namespace Inventory
{
    public partial class MachinePartsReceiveOrder : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALMachinePartsReceiveOrder lstMPR = new BALMachinePartsReceiveOrder();
        Page_Controls defaultPage = new Page_Controls();
        EmailController objemail = new EmailController();
        string StatusReceivingOrder = Constant.ReceivingStatus;
        string StatusOrder = Constant.OrderStatus;
        string StatusBackOrder = Constant.BackOrderStatus;
        string PartialOrderStatus = Constant.PartialOrderStatus;
        string VoidOrderStatus = Constant.VoidOrderStatus;
        string CloseOrderStatus = Constant.CloseOrderStatus;
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        Int32 POhdnrowcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.grdMPRMaster);
                scriptManager.RegisterPostBackControl(this.btnPrintAll);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();  
                    BindCorporate();
                    if (defaultPage != null)
                    {
                        //drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility(1, "Add");
                        //drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindVendor(1, "Add");
                        BindLookUp("Add");
                        BindReason("Add");
                        BindReceivingAction("Add","");
                        lclsservice.SyncMachinePartsReceivingorder();
                        BindMachinePartsReceiveGrid();                       
                        Hdnrole.Value = Convert.ToString(defaultPage.RoleID);
                        if (defaultPage.RoleID == 1)
                        {                        
                            foreach (GridViewRow row in grdMPRMaster.Rows)
                            {
                                LinkButton lblMPONo = (LinkButton)row.FindControl("lbMPONo");
                                lblMPONo.Enabled = false;
                            }
                        }
                        if (defaultPage.Rec_MachinePartsPage_Edit == false && defaultPage.Rec_MachinePartsPage_Edit == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                        if (defaultPage.Rec_MachinePartsPage_Edit == false && defaultPage.Rec_MachinePartsPage_View == true)
                        {
                            //btnSave.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

     
        /// <summary>
        /// Bind the Corporate details to dropdown control 
        ///   // Search Drop Down
        /// </summary>
        #region Bind Corporate Values
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    drpcor.DataSource = lstcrop;
                    drpcor.DataTextField = "CorporateName";
                    drpcor.DataValueField = "CorporateID";
                    drpcor.DataBind();
                    //ListItem lst = new ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
                    //drpcor.Items.Insert(0, lst);
                    //drpcor.SelectedIndex = 0;
                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcor.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    drpcor.DataTextField = "CorporateName";
                    drpcor.DataValueField = "CorporateID";
                    drpcor.DataBind();
                    //ListItem lst = new ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
                    //drpcor.Items.Insert(0, lst);
                    //drpcor.SelectedIndex = 0;
                }
                foreach (ListItem lst in drpcor.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
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
                    
                }
                foreach (ListItem lst in drpfacility.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
                BindVendor(1, "Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
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
                if (Search == 1)
                {
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
                        //ListItem lst = new ListItem();
                        //lst.Value = "All";
                        //lst.Text = "All";
                        //drpvendor.Items.Insert(0, lst);
                        //drpvendor.SelectedIndex = 0;
                    }
                    foreach (ListItem lst in drpvendor.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }


            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);

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
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("MachinePartsReceive", "Status", Mode).ToList();
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatus.Items.Insert(0, lst);
                if (defaultPage.RoleID == 1)
                {
                    drpStatus.Items.FindByText(StatusReceivingOrder).Selected = true;
                }
                else
                {
                    drpStatus.Items.FindByText(StatusOrder).Selected = true;
                    drpStatus.Items.FindByText(StatusBackOrder).Selected = true;
                }
            }

                 // Search Status Drop Down



            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }
        #endregion
        /// <summary>
        /// Bind the Reason LookUp details from Status master table to dropdown control 
        /// </summary>
        #region Bind Reason LookUp Values
        public void BindReason(string Mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("ReceivingOrders", "Reason", Mode).ToList();
                ddlReason.DataSource = lstLookUp;
                ddlReason.DataTextField = "InvenValue";
                ddlReason.DataValueField = "InvenValue";
                ddlReason.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Reason--";
                ddlReason.Items.Insert(0, lst);
                ddlReason.SelectedIndex = 0;
            }

                 // Search Status Drop Down

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Receiving Action LookUp details from Status master table to dropdown control 
        /// </summary>
        #region Bind Receiving Action LookUp Values
        public void BindReceivingAction(string Mode, string type)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("MachinePartsReceive", "Action", Mode).ToList();
                if (type == "TYPE1")
                {
                    ddlReceivingAction.DataSource = lstLookUp.Where(a => a.InvenValue == CloseOrderStatus);
                }
                else if (type == "TYPE2")
                {
                    ddlReceivingAction.DataSource = lstLookUp.Where(a => a.InvenValue == PartialOrderStatus);
                }
                else if (type == "TYPE3")
                {
                    ddlReceivingAction.DataSource = lstLookUp.Where(a => a.InvenValue == VoidOrderStatus);
                }
                //ddlReceivingAction.DataSource = lstLookUp;
                ddlReceivingAction.DataTextField = "InvenValue";
                ddlReceivingAction.DataValueField = "InvenValue";
                ddlReceivingAction.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Action--";
                ddlReceivingAction.Items.Insert(0, lst);
                ddlReceivingAction.SelectedIndex = 0;
            }

                 // Search Status Drop Down

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }
        #endregion
        /// <summary>
        /// Bind the Machine Parts Request Master details from MPRMaster table to Grid control 
        /// </summary>

        #region Bind Machine Parts Request Master Values
        public void BindMachinePartsReceiveGrid()
        {
            try
            {
                BALMachinePartsReceiveOrder lstMp = new BALMachinePartsReceiveOrder();
              
                if (drpcor.SelectedValue == "All")
                {
                    lstMp.ListCorporateID = "ALL";
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

                    lstMp.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacility.SelectedValue == "All")
                {
                    lstMp.ListFacilityID = "ALL";
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
                    lstMp.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendor.SelectedValue == "All")
                {
                    lstMp.ListVendorID = "ALL";
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
                    lstMp.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    lstMp.FinalStatus = "ALL";
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
                    lstMp.FinalStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                }
                else
                {
                    lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchMachinePartsReceive> lstMSRMaster = lclsservice.SearchMachinePartsReceive(lstMp).ToList();
                grdMPRMaster.DataSource = lstMSRMaster;
                grdMPRMaster.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
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
                BALMachinePartsReceiveOrder lstMp = new BALMachinePartsReceiveOrder();
                
                if (drpcor.SelectedValue == "All")
                {
                    lstMp.ListCorporateID = "ALL";
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

                    lstMp.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacility.SelectedValue == "All")
                {
                    lstMp.ListFacilityID = "ALL";
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
                    lstMp.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendor.SelectedValue == "All")
                {
                    lstMp.ListVendorID = "ALL";
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
                    lstMp.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    lstMp.FinalStatus = "ALL";
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
                    lstMp.FinalStatus = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text != "") lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                if (txtDateTo.Text != "") lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);                
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchMachinePartsReceive> lstMSRMaster = lclsservice.SearchMachinePartsReceive(lstMp).ToList();
                grdMPRMaster.DataSource = lstMSRMaster;
                grdMPRMaster.DataBind();
            }



            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            if (defaultPage.RoleID == 1)
            {
                ReqInvoiceNo.ErrorMessage = req;
                ReqInvoiceDate.ErrorMessage = req;
                ReqddlReceivingAction.ErrorMessage = req;
                ReqddlReason.ErrorMessage = req;
            }
            else
            {
                ReqtxtPackingSlipNo.ErrorMessage = req;
                reqPackingSlipDate.ErrorMessage = req;
                ReqReceivedDate.ErrorMessage = req;
            }

            rfvDateFrom.ErrorMessage = req;
            rfvDateTo.ErrorMessage = req;
            ReqdrdCorporate.ErrorMessage = req;
            Reqdrpfacility.ErrorMessage = req;
            Reqdrpvendor.ErrorMessage = req;
            rfvStatus.ErrorMessage = req;
        }

        public void RemoveAdminRequiredField()
        {
            ReqInvoiceNo.Enabled = false;
            ReqInvoiceDate.Enabled = false; 
            ReqddlReceivingAction.Enabled = false;
            ReqddlReason.Enabled = false;
            //ReqtxtOthers.Enabled = false;
        }
        public void SetAdminRequiredField()
        {
            ReqInvoiceNo.Enabled = false;
            ReqInvoiceDate.Enabled = false;
            ReqddlReceivingAction.Enabled = false;
            ReqddlReason.Enabled = true;
            //ReqtxtOthers.Enabled = true;
        }
        public void RemoveVoidRequiredField()
        {
            ReqReceivedDate.Enabled = false;
            ReqtxtPackingSlipNo.Enabled = false;
            reqPackingSlipDate.Enabled = false;
            ReqInvoiceNo.Enabled = false;
            ReqInvoiceDate.Enabled = false;
            ReqddlReceivingAction.Enabled = true;
            ReqddlReason.Enabled = true;
            //ReqtxtOthers.Enabled = false;
        }

        protected void lbMPONo_Click(object sender, EventArgs e)
        {
            try
            {
                mpereview.Show();
                string s = string.Empty;
                string status = string.Empty;
                RemoveAdminRequiredField();
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lbMPONo = (LinkButton)gvrow.FindControl("lbMPONo");
                LinkButton lbMPRONo = (LinkButton)gvrow.FindControl("lbMPRONo");
                HddOrderID.Value = gvrow.Cells[17].Text.Trim().Replace("&nbsp;", "");
                ViewState["CorporateID"] = Convert.ToString(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", ""));
                ViewState["FacilityID"] = Convert.ToString(gvrow.Cells[6].Text.Trim().Replace("&nbsp;", ""));
                Int64 MachinePartsReceivingMasterID = 0;
                Int64 MachinePartsReceivingDetailsID = 0;
                lblMasterNo.Text = lbMPONo.Text;
                MachinePartsReceivingMasterID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                status = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                List<GETMPOValues> llst = lclsservice.GETMPOValues(MachinePartsReceivingMasterID).ToList();
                MachinePartsReceivingDetailsID = llst[0].MachinePartsReceivingDetailsID;
                ViewState["MachinePartsReceivingMasterID"] = MachinePartsReceivingMasterID;
                ViewState["MachinePartsReceivingDetailsID"] = MachinePartsReceivingDetailsID;
                HddMasterID.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                HDDMPONo.Value = lbMPONo.Text;
                lblrcountpo.Visible = true;
                lblrcountro.Visible = false;
                if (lbMPRONo.Text != "")
                {
                    Reviewmpo();

                }
                else if (defaultPage.RoleID == 1)
                {
                    RemoveVoidRequiredField();
                    txtPackingSlipDate.Enabled = false;
                    txtPackingSlipNo.Enabled = false;
                    txtReceivingDate.Enabled = false;
                    txtInvoiceNo.Enabled = false;
                    txtInvoiceDate.Enabled = false;
                    if (status==StatusBackOrder)
                    {
                        BindReceivingAction("Add", "TYPE2");
                        ReqddlReason.Enabled = true;
                        ddlReason.Enabled = true;
                        ddlReceivingAction.ClearSelection();
                        ddlReceivingAction.Enabled = false;
                        ddlReceivingAction.Items.FindByValue(PartialOrderStatus).Selected = true;
                    }
                    else
                    {
                        BindReceivingAction("Add", "TYPE3");
                        ddlReceivingAction.ClearSelection();
                        ddlReceivingAction.Enabled = false;
                        ddlReceivingAction.Items.FindByValue(VoidOrderStatus).Selected = true;
                    }
                  
                    divinvoice.Style.Add("display", "block");
                    divreason.Style.Add("display", "block");
                    List<GETMPOValues> llstMPRDetails = lclsservice.GETMPOValues(MachinePartsReceivingMasterID).ToList();
                    txtPackingSlipNo.Text = llstMPRDetails[0].PackingSlipNo;
                    if (txtPackingSlipNo.Text != "")
                    {
                        txtPackingSlipDate.Text = Convert.ToDateTime(llstMPRDetails[0].PackingSlipDate).ToString("MM/dd/yyyy");
                        txtReceivingDate.Text = Convert.ToDateTime(llstMPRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        txtPackingSlipDate.Text = "";
                        txtReceivingDate.Text = "";
                    }

                    grdMPRReview.DataSource = llstMPRDetails;
                    grdMPRReview.DataBind();
                    Int32 a = Convert.ToInt32(HddGridCount.Value);
                    lblrcountpo.Text = "No of records : " + (llstMPRDetails.Count - a);
                    grdMPRReview.FooterRow.Enabled = false;
                    foreach (GridViewRow row in grdMPRReview.Rows)
                    {
                        Label lblbalqty = (Label)row.FindControl("lblbalqty");
                        TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                        TextBox txtComments = (TextBox)row.FindControl("txtComments");
                        Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                        TextBox txtsipcost = (TextBox)grdMPRReview.FooterRow.FindControl("txtsipcost");
                        TextBox txttax = (TextBox)grdMPRReview.FooterRow.FindControl("txttax");
                        TextBox txtTotalcost = (TextBox)grdMPRReview.FooterRow.FindControl("txtTotalcost");

                        lblbalqty.Text = llstMPRDetails[0].BalanceQty.ToString();
                        txtreceivedqty.Text = llstMPRDetails[0].ReceivedQty.ToString();
                        txtreceivedqty.Enabled = false;
                        txtsipcost.Enabled = false;
                        txttax.Enabled = false;
                        txtTotalcost.Enabled = false;
                        txtComments.Enabled = false;
                        lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[0].TotalPrice));
                    }
                }
                else
                {
                    //lblMasterNo.Text = lbMPONo.Text;
                    divinvoice.Style.Add("display", "none");
                    divreason.Style.Add("display", "none");
                    List<GETMPOValues> llstMPRDetails = lclsservice.GETMPOValues(MachinePartsReceivingMasterID).ToList();
                    grdMPRReview.DataSource = llstMPRDetails;
                    grdMPRReview.DataBind();
                    Int32 a = Convert.ToInt32(HddGridCount.Value);
                    lblrcountpo.Text = "No of records : " + (llstMPRDetails.Count - a);
                    grdMPRReview.FooterRow.Enabled = false;
                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

        protected void lbMPRONo_Click(object sender, EventArgs e)
        {
            try
            {
                if (defaultPage.RoleID == 1)
                {
                    mpereview.Show();
                    string s = string.Empty;
                    LinkButton btndetails = sender as LinkButton;
                    divinvoice.Style.Add("display", "block");
                    divreason.Style.Add("display", "block");
                    ReqddlReason.Enabled = false;
                    ddlReason.Enabled = false;
                    lblrcountpo.Visible = false;
                    lblrcountro.Visible = true;
                    Int64 MachinePartsReceivingMasterID = 0;
                    GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                    MachinePartsReceivingMasterID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                    ViewState["MachinePartsReceivingMasterID"] = MachinePartsReceivingMasterID;
                    LinkButton lbMPONo = (LinkButton)gvrow.FindControl("lbMPONo");
                    LinkButton lbMPRONo = (LinkButton)gvrow.FindControl("lbMPRONo");
                    HddMPRNONo.Value = lbMPRONo.Text;
                    HddMasterID.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                    lbMPRONo.Text = lbMPRONo.Text;
                    lblMasterNo.Text = lbMPRONo.Text;
                    List<GETMPROValues> llstMPRDetails = lclsservice.GETMPROValues(MachinePartsReceivingMasterID).ToList();
                    txtPackingSlipNo.Text = llstMPRDetails[0].PackingSlipNo;
                    txtPackingSlipDate.Text = Convert.ToDateTime(llstMPRDetails[0].PackingSlipDate).ToString("MM/dd/yyyy");
                    txtReceivingDate.Text = Convert.ToDateTime(llstMPRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                    txtReceivingDate.Text = Convert.ToDateTime(llstMPRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                    txtInvoiceNo.Text = llstMPRDetails[0].InvoiceNo;
                    txtPackingSlipDate.Enabled = false;
                    txtPackingSlipNo.Enabled = false;
                    txtReceivingDate.Enabled = false;
                    txtInvoiceDate.Text = "";
                    txtInvoiceNo.Enabled = true;
                    txtInvoiceDate.Enabled = true;
                    ddlReceivingAction.Enabled = true;
                    btnSave.Visible = true;
                    if (txtInvoiceNo.Text != "")
                    {
                        Reviewfields();
                    }
                    else
                    {
                        divinvoice.Style.Add("display", "block");
                        divreason.Style.Add("display", "block");
                        grdMPRReview.DataSource = llstMPRDetails;
                        grdMPRReview.DataBind();
                        Int32 a = Convert.ToInt32(HddGridCount.Value);
                        lblrcountro.Text = "No of records : " + (llstMPRDetails.Count - a);
                        int i = 0;  
                        foreach (GridViewRow row in grdMPRReview.Rows)
                        {
                            Label lblRowNumber = (Label)row.FindControl("lblRowNumber");
                            Label lblItemID = (Label)row.FindControl("lblItemID");
                            Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                            Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                            Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                            Label lblbalqty = (Label)row.FindControl("lblbalqty");
                            TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                            Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");

                            lblItemID.Text = llstMPRDetails[i].ItemID;
                            lblItemDescription.Text = llstMPRDetails[i].ItemDescription;
                            lblrevppqty.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].PricePerQty));
                            lblOrderQuantity.Text = llstMPRDetails[i].OrderQty.ToString();
                            lblbalqty.Text = llstMPRDetails[i].BalanceQty.ToString();
                            txtreceivedqty.Text = llstMPRDetails[i].ReceivedQty.ToString();
                            txtreceivedqty.Enabled = false;
                            lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].TotalPrice));
                            i++;
                        }
                        //CalType();
                        //string WF = Convert.ToString(ViewState["Type"]);
                        //if (WF != "" || WF == null)
                        BindReceivingAction("Add", "TYPE1");
                        ddlReceivingAction.ClearSelection();
                        ddlReceivingAction.Enabled = false;
                        ddlReceivingAction.Items.FindByValue(CloseOrderStatus).Selected = true;
                        TextBox txtsipcost = grdMPRReview.FooterRow.FindControl("txtsipcost") as TextBox;
                        TextBox txtTax = grdMPRReview.FooterRow.FindControl("txttax") as TextBox;
                        TextBox txtTotalCost = grdMPRReview.FooterRow.FindControl("txtTotalcost") as TextBox;
                        txtsipcost.Text = llstMPRDetails[0].ShippingCost;
                        txtTax.Text = llstMPRDetails[0].Tax;
                        txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[0].TotalCost));
                    
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

        public void CalType()
        {
            decimal orterqty = 0;
            decimal receivedoqty = 0;
            decimal Total = 0;
            decimal orterqtytotal = 0;
            decimal receivedoqtytotal = 0;
            foreach (GridViewRow row in grdMPRReview.Rows)
            {
                Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");

                orterqty = Convert.ToDecimal(lblOrderQuantity.Text);
                if (txtreceivedqty.Text != "")
                    receivedoqty = Convert.ToDecimal(txtreceivedqty.Text);
                orterqtytotal += orterqty;
                if (txtreceivedqty.Text != "")
                    receivedoqtytotal += receivedoqty;
            }
            Total = orterqtytotal - receivedoqtytotal;
            if (Total == 0)
            {
                ViewState["Type"] = Convert.ToString("TYPE1");
            }
            else
            {
                ViewState["Type"] = Convert.ToString("TYPE2");
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindMachinePartsReceiveGrid();
        }
  
        public Int32 CalBalQty(GridViewRow row)
        {
            Int32 BalQty = 0;
            try{
                Int32 receivedqty = 0;
                Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                string s = lblOrderQuantity.Text;
                TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                if (txtreceivedqty.Text !="")
                receivedqty = Convert.ToInt32(txtreceivedqty.Text);
                Label lblbalqty = (Label)row.FindControl("lblbalqty");
                BalQty = (Convert.ToInt32(s) - Convert.ToInt32(receivedqty));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }

            return BalQty;
        }

        public void Reviewfields()
        {
            try
            {
                btnSave.Visible = false;
                List<GETMPROValues> llstMPRDetails = lclsservice.GETMPROValues(Convert.ToInt64(ViewState["MachinePartsReceivingMasterID"])).ToList();
                txtInvoiceNo.Text = llstMPRDetails[0].InvoiceNo;
                txtInvoiceDate.Text = Convert.ToDateTime(llstMPRDetails[0].InvoiceDate).ToString("MM/dd/yyyy");
                ddlReceivingAction.SelectedItem.Text = llstMPRDetails[0].ReceivingAction;
                ddlReason.SelectedItem.Text = llstMPRDetails[0].Reason;
                txtOthers.Text = llstMPRDetails[0].Others;
                txtInvoiceNo.Enabled = false;
                txtInvoiceDate.Enabled = false;
                txtReceivingDate.Enabled = false;
                ddlReason.Enabled = false;
                ddlReceivingAction.Enabled = false;
                txtOthers.Enabled = false;
                grdMPRReview.DataSource = llstMPRDetails;
                grdMPRReview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountro.Text = "No of records : " + (llstMPRDetails.Count - a);
                int i = 0;
                foreach (GridViewRow row in grdMPRReview.Rows)
                {
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    TextBox txtComments = (TextBox)row.FindControl("txtComments");
                    lblbalqty.Text = llstMPRDetails[i].BalanceQty.ToString();
                    txtreceivedqty.Text = llstMPRDetails[i].ReceivedQty.ToString();
                    if (llstMPRDetails[i].Comments != null)
                        txtComments.Text = llstMPRDetails[i].Comments.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].TotalPrice));
                    i++;
                    txtreceivedqty.Enabled = false;
                    txtComments.Enabled = false;
                }
                TextBox txtsipcost = (TextBox)grdMPRReview.FooterRow.FindControl("txtsipcost");
                TextBox txtTax = (TextBox)grdMPRReview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalCost = (TextBox)grdMPRReview.FooterRow.FindControl("txtTotalcost") as TextBox;
                txtsipcost.Text = llstMPRDetails[0].ShippingCost;
                txtTax.Text = llstMPRDetails[0].Tax;
                txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[0].TotalCost));
                txtsipcost.Enabled = false;
                txtTax.Enabled = false;
                txtTotalCost.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

        public void Reviewmpo()
        {
            try
            {
                btnSave.Visible = false;
                List<GETMPOValues> llstMPRDetails = lclsservice.GETMPOValues(Convert.ToInt64(ViewState["MachinePartsReceivingMasterID"])).ToList();
                txtPackingSlipNo.Text = llstMPRDetails[0].PackingSlipNo;
                txtPackingSlipDate.Text = Convert.ToDateTime(llstMPRDetails[0].PackingSlipDate).ToString("MM/dd/yyyy");
                txtReceivingDate.Text = Convert.ToDateTime(llstMPRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                txtPackingSlipNo.Enabled = false;
                txtPackingSlipDate.Enabled = false;
                txtReceivingDate.Enabled = false;
                grdMPRReview.DataSource = llstMPRDetails;
                grdMPRReview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountpo.Text = "No of records : " + (llstMPRDetails.Count - a);
                int i = 0;
                foreach (GridViewRow row in grdMPRReview.Rows)
                {
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                    TextBox txtComments = (TextBox)row.FindControl("txtComments");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    lblbalqty.Text = llstMPRDetails[i].BalanceQty.ToString();
                    txtreceivedqty.Text = llstMPRDetails[i].ReceivedQty.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].TotalPrice));
                    if (llstMPRDetails[i].Comments != null)
                        txtComments.Text = llstMPRDetails[i].Comments.ToString();
                    i++;
                    txtComments.Enabled = false;
                    txtreceivedqty.Enabled = false;
                }
                TextBox txtsipcost = (TextBox)grdMPRReview.FooterRow.FindControl("txtsipcost");
                TextBox txttax = (TextBox)grdMPRReview.FooterRow.FindControl("txttax");
                TextBox txtTotalcost = (TextBox)grdMPRReview.FooterRow.FindControl("txtTotalcost");
                txtsipcost.Text = llstMPRDetails[0].ShippingCost;
                txttax.Text = llstMPRDetails[0].Tax;
                txtTotalcost.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[0].TotalCost));
                grdMPRReview.FooterRow.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

        public void SaveMachinePartsReceiveMaster()
        {
            try
            {
                Int64 InsertCodeID = 0;
                string meditem = string.Empty;
                Int64 medsave = 0;
                int rowcount = grdMPRReview.Rows.Count;
                Control control = null;
                lstMPR.MachinePartsReceiveMasterID = Convert.ToInt64(ViewState["MachinePartsReceivingMasterID"]);
                lstMPR.MPRMasterID = Convert.ToInt64(HddMasterID.Value);
                lstMPR.MachinePartsRequestOrderID = Convert.ToInt64(HddOrderID.Value);
                lstMPR.MPONo = HDDMPONo.Value;
                lstMPR.PackingSlipNo = txtPackingSlipNo.Text;
                if (txtPackingSlipDate.Text != "")
                {
                    lstMPR.PackingSlipDate = Convert.ToDateTime(txtPackingSlipDate.Text);
                }
                if (txtReceivingDate.Text != "")
                {
                    lstMPR.ReceivedDate = Convert.ToDateTime(txtReceivingDate.Text);
                }
                if (txtInvoiceNo.Text != "")
                {
                    lstMPR.InvoiceNo = Convert.ToString(txtInvoiceNo.Text);
                }
                if (txtInvoiceDate.Text != "")
                {
                    lstMPR.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
                }
                if (ddlReceivingAction.SelectedItem.Text != "")
                {
                    lstMPR.ReceivingAction = ddlReceivingAction.SelectedValue;
                }
                if (ddlReason.SelectedItem.Text != "")
                {
                    lstMPR.Reason = ddlReason.SelectedValue;
                }
                ViewState["Reason"] = lstMPR.Reason;
                lstMPR.Others = txtOthers.Text;
                CalType();
                lstMPR.Type = Convert.ToString(ViewState["Type"]);
                lstMPR.CreatedBy = defaultPage.UserId;
                lstMPR.LoggedinBy = defaultPage.UserId;
                lstMPR.FinalStatus = ddlReceivingAction.SelectedValue;
                if (defaultPage.RoleID == 1)
                {
                    lstMPR.Type = "TYPE3";
                }
                if (ddlReceivingAction.SelectedItem.Text == VoidOrderStatus)
                {
                    lstMPR.Type = "TYPE3";
                    List<UpdateMachinePartsReceivingMaster> lstMSRMaster = lclsservice.UpdateMachinePartsReceivingMaster(lstMPR).ToList();
                    SendEmail();
                }
                else
                {

                    TextBox txtsipcost = grdMPRReview.FooterRow.FindControl("txtsipcost") as TextBox;
                    TextBox txttax = grdMPRReview.FooterRow.FindControl("txttax") as TextBox;
                    TextBox txtTotalcost = grdMPRReview.FooterRow.FindControl("txtTotalcost") as TextBox;
                    if (txtsipcost.Text != "")
                    {
                        lstMPR.ShippingCost = txtsipcost.Text;
                    }
                    if (txttax.Text != "")
                    {
                        lstMPR.Tax = txttax.Text;
                    }
                    if (txtTotalcost.Text != "")
                    {
                        lstMPR.TotalCost = Convert.ToDecimal(txtTotalcost.Text);
                    }
                    List<UpdateMachinePartsReceivingMaster> lstMSRMaster = lclsservice.UpdateMachinePartsReceivingMaster(lstMPR).ToList();
                    InsertCodeID = Convert.ToInt64(lstMSRMaster[0].INSERTRECORDID);
                    foreach (GridViewRow row in grdMPRReview.Rows)
                    {

                        Label lblMachinePartsReceivingDetailsID = (Label)row.FindControl("lblMachinePartsReceivingDetailsID");
                        lstMPR.MachinePartsReceiveDetailsID = Convert.ToInt64(lblMachinePartsReceivingDetailsID.Text);
                        Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                        Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                        Label lblbalqty = (Label)row.FindControl("lblbalqty");
                        TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                        Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                        TextBox txtComments = (TextBox)row.FindControl("txtComments");

                        lstMPR.INSERTRECORDID = InsertCodeID;
                        lstMPR.BalanceQty = CalBalQty(row);
                        Int32 receivedqty = 0;
                        if (txtreceivedqty.Text != "")
                            receivedqty = Convert.ToInt32(txtreceivedqty.Text);
                        lstMPR.ReceivedQty = receivedqty;
                        if (txtComments.Text != "")
                        {
                            lstMPR.Comments = txtComments.Text;
                        }
                        decimal TotalPrice = 0;
                        TotalPrice = (Convert.ToDecimal(lblrevppqty.Text) * Convert.ToInt32(receivedqty));
                        lstMPR.TotalPrice = TotalPrice;
                        meditem = lclsservice.UpdateMachinePartsReceivingDetails(lstMPR);


                    }
                }
                if (meditem == "Saved Successfully")
                {
                    ViewState["Type"] = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveSaveMessage.Replace("<<MachinePartsReceive>>", ""), true);
                    clear();
                }

                SearchGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            } 
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorList = string.Empty;
                if (ddlReceivingAction.SelectedItem.Text == VoidOrderStatus)
                {
                    RemoveVoidRequiredField();
                    SaveMachinePartsReceiveMaster();
                }
                else
                {
                    mpereview.Show();
                    SaveMachinePartsReceiveMaster();
                    mpereview.Hide();
                    //foreach (GridViewRow grdfs in grdMPRReview.Rows)
                    //{
                    //    Label lblbalqty = (Label)grdfs.FindControl("lblbalqty");
                    //    Label lblOrderQuantity = (Label)grdfs.FindControl("lblOrderQuantity");
                    //    Label lblrevppqty = (Label)grdfs.FindControl("lblrevppqty");
                    //    TextBox txtreceivedqty = (TextBox)grdfs.FindControl("txtreceivedqty");
                    //    Label lblTotalPrice = (Label)grdfs.FindControl("lblTotalPrice");
                    //    TextBox txtComments = (TextBox)grdfs.FindControl("txtComments");

                    //    if (txtreceivedqty.Text == "" && lblOrderQuantity.Text!="0")
                    //    {
                    //        ErrorList = "Received field should not be empty";
                    //    }
                    //    else if (CalBalQty(grdfs) > 0 && txtComments.Text == "")
                    //    {
                    //        ErrorList = "Comments field should not be empty";
                    //    }
                    //}

                    //if (ErrorList == "")
                    //{
                    //    SaveMachinePartsReceiveMaster();
                    //}
                    //else
                    //{
                    //    lblmsg.Text = ErrorList;
                    //    mpereview.Show();
                    //}
                   
                }
                //clear();
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }   
        }
           

        public void clear()
        {
            txtPackingSlipNo.Text = "";
            txtReceivingDate.Text = "";
            txtPackingSlipDate.Text = "";
            txtInvoiceDate.Text = "";
            txtOthers.Text = "";
            lblmsg.Text = "";
            ddlReceivingAction.ClearSelection();
            ddlReceivingAction.SelectedIndex = 0;
            ddlReason.ClearSelection();
            ddlReason.SelectedIndex = 0;
            txtPackingSlipNo.Enabled = true;
            txtPackingSlipDate.Enabled = true;
            txtReceivingDate.Enabled = true;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            clear();
            mpereview.Hide();
        }
       
     
        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReason.SelectedItem.Text == "Other")
            {
                if (ddlReceivingAction.SelectedItem.Text == VoidOrderStatus)
                {
                    txtOthers.Visible = true;
                    //lblother.Visible = true;
                }
                else
                {
                    txtOthers.Visible = true;
                    //lblother.Visible = true;
                    //Totalcost();
                }
              
            }
            
            mpereview.Show();
        }

        public void Totalcost()
        {
            TextBox txtShippingCost = grdMPRReview.FooterRow.FindControl("txtsipcost") as TextBox;
            TextBox txtTax = grdMPRReview.FooterRow.FindControl("txttax") as TextBox;
            TextBox txtTotalCost = grdMPRReview.FooterRow.FindControl("txtTotalcost") as TextBox;
            decimal sum = 0;
            for (int i = 0; i < grdMPRReview.Rows.Count; ++i)
            {
                Label lblTotalPrice = grdMPRReview.Rows[i].FindControl("lblTotalPrice") as Label;
                sum += Convert.ToDecimal(lblTotalPrice.Text);
            }
            //(grdMPRReview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
            (grdMPRReview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
        }


        protected void grdMPRMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string status = string.Empty;
                    status = e.Row.Cells[14].Text;
                    //string b ="<img src= \"Images/Readmore.png\" />";
                    //Label lblAudit = (Label)e.Row.FindControl("lblAudit");
                    //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                    //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                    ImageButton imgsummary = (ImageButton)e.Row.FindControl("imgsummary");
                    ImageButton imgdetails = (ImageButton)e.Row.FindControl("imgdetails");
                    LinkButton lbMPONo = (LinkButton)e.Row.FindControl("lbMPONo");
                    LinkButton lbMPRONo = (LinkButton)e.Row.FindControl("lbMPRONo");
                    //if (lbMPRONo.Text != "")
                    //{
                    //    imgsummary.Enabled = true;
                    //    imgdetails.Enabled = true;
                    //}
                    //else
                    //{
                    //    imgsummary.Enabled = false;
                    //    imgdetails.Enabled = false;

                    //}
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
                        if (lbMPRONo.Text == "")
                        {
                            lbMPONo.Enabled = true;
                            imgsummary.Enabled = false;
                        }
                        else
                        {
                            lbMPONo.Enabled = false;
                            imgsummary.Enabled = true;
                        }
                    }
                    else
                    {
                        lbMPRONo.Enabled = false;

                        if (lbMPRONo.Text == "")
                        {
                            imgsummary.Enabled = false;
                        }
                        else
                        {
                            imgsummary.Enabled = true;
                        }
                    }

                    if (status == CloseOrderStatus || status == VoidOrderStatus)
                    {
                        lbMPONo.Enabled = false;
                        lbMPRONo.Enabled = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }



        protected void grdMPRReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblOrderQuantity = (Label)e.Row.FindControl("lblOrderQuantity");
                    TextBox txtreceivedqty = (TextBox)e.Row.FindControl("txtreceivedqty");
                    TextBox txtComments = (TextBox)e.Row.FindControl("txtComments");
                    if (lblOrderQuantity.Text == "0")
                    {
                        txtreceivedqty.Enabled = false;
                        txtComments.Enabled = false;
                        e.Row.Style.Add("display", "none");
                        POhdnrowcount = POhdnrowcount + 1;
                    }
                }
                HddGridCount.Value = Convert.ToString(POhdnrowcount);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalOrderMessage.Replace("<<CapitalOrder>>", ex.Message), true);
            }

        }

        public void SendEmail()
        {
            try
            {
                Int64 MPRMasterID = Convert.ToInt64(HddMasterID.Value);
                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                lstMPR.OrderContent = bytes;
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(ViewState["CorporateID"]);
                Int64 FacilityID = Convert.ToInt64(ViewState["FacilityID"]);
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                var MPONo = HDDMPONo.Value;
                EmailSetting(CorporateID, FacilityID, MPONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveemailMessage.Replace("<<MachinePartsReceive>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message), true);
            }
        }
        
        
        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string MPONo)
        {
            try
            {
                string Superadmin = string.Empty;
                List<GetSuperAdminDetails> lstsuperadmin = lclsservice.GetSuperAdminDetails(CorporateID, FacilityID).ToList();
                foreach (var value in lstsuperadmin)
                {
                    Superadmin += "<br/>" + value.UserName + "<br/>" + value.Email + "<br/>" + value.PhoneNo;
                }
                string reason = ViewState["Reason"].ToString();
                if (reason != "Other")
                {
                    objemail.vendorEmailcontent = string.Format("Due to " + reason + " " + MPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + Superadmin);
                }
                else
                {
                    objemail.vendorEmailcontent = string.Format("We are cancelling " + MPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + Superadmin);
                }
                objemail.vendoremailsubject = "Machine Parts Order – " + MPONo;
                string displayfilename = "Machine Parts Order – " + MPONo + ".pdf";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }

        public List<object> DetailsOrderReport(Int64 MPRMasterID)
        {
            List<object> llstarg = new List<object>();
            List<GetMPOrderContentPO> llstreview = lclsservice.GetMPOrderContentPO(MPRMasterID, defaultPage.UserId).ToList();
            rvMachinePoreport.ProcessingMode = ProcessingMode.Local;
            rvMachinePoreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachineReceivePDF.rdlc");
            ReportParameter[] p1 = new ReportParameter[1];
            p1[0] = new ReportParameter("MPRMasterID", Convert.ToString(MPRMasterID));
            this.rvMachinePoreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MachineReceivePDFReviewDS", llstreview);
            rvMachinePoreport.LocalReport.DataSources.Clear();
            rvMachinePoreport.LocalReport.DataSources.Add(datasource);
            rvMachinePoreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvMachinePoreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            string FromEmail = llstreview[0].FromEmail;
            string ToEmail = llstreview[0].ToEmail;
            Int64 CorporateID = Convert.ToInt64(llstreview[0].CorporateID);
            Int64 FacilityID = Convert.ToInt64(llstreview[0].FacilityID);
            string MPONo = llstreview[0].MPONo;
            llstarg.Insert(0, FromEmail);
            llstarg.Insert(1, ToEmail);
            llstarg.Insert(2, bytes);
            llstarg.Insert(3, CorporateID);
            llstarg.Insert(4, FacilityID);
            llstarg.Insert(5, MPONo);
            return llstarg;
        }

        protected void imgsummary_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Int64 MachinePartsRequestOrderID = 0;
            MachinePartsRequestOrderID = Convert.ToInt64(gvrow.Cells[17].Text.Trim().Replace("&nbsp;", ""));
            List<BindMachineReceiveSummaryReport> llstreview = lclsservice.BindMachineReceiveSummaryReport(MachinePartsRequestOrderID, Convert.ToInt64(defaultPage.UserId), "").ToList();
            rvMachineSummaryReport.ProcessingMode = ProcessingMode.Local;
            rvMachineSummaryReport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsSummaryReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("MachinePartsSummaryDS", llstreview);
            rvMachineSummaryReport.LocalReport.DataSources.Clear();
            rvMachineSummaryReport.LocalReport.DataSources.Add(datasource);
            rvMachineSummaryReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvMachineSummaryReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "MachinePartsReceiveOrder" + guid + ".pdf";
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

        protected void imgdetails_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Int64 MachinePartsRequestOrderID = 0;
            Int64 MachinePartsReceivingMasterID = 0;
            Int64 MPRMasterID = 0;
            MachinePartsRequestOrderID = Convert.ToInt64(gvrow.Cells[17].Text.Trim().Replace("&nbsp;", ""));
            MachinePartsReceivingMasterID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
            MPRMasterID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
            List<BindMachineReceivingDetailsReport> llstreview = lclsservice.BindMachineReceivingDetailsReport(MPRMasterID, MachinePartsReceivingMasterID, Convert.ToInt64(defaultPage.UserId), "").ToList();
            //List<BindMachinePartReceivingDetailsSubReport> llstsubreview = lclsservice.BindMachinePartReceivingDetailsSubReport(MachinePartsRequestOrderID, Convert.ToInt64(defaultPage.UserId), "").ToList();
            rvMachineDetailsReport.ProcessingMode = ProcessingMode.Local;
            rvMachineDetailsReport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsReceivingDetails.rdlc");
            ReportDataSource datasource = new ReportDataSource("MachineReceivingDetailsDS", llstreview);
            //ReportDataSource datasourcesub = new ReportDataSource("MachineReceivingDetailsSubDS", llstsubreview);
            rvMachineDetailsReport.LocalReport.DataSources.Clear();
            rvMachineDetailsReport.LocalReport.DataSources.Add(datasource);
            //rvMachineDetailsReport.LocalReport.DataSources.Add(datasourcesub);
            rvMachineDetailsReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvMachineDetailsReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "MachinePartsRecevingDetails" + guid + ".pdf";
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

        protected void btnClose_Click(object sender, EventArgs e)
        {
            BindCorporate();
            BindFacility(1, "Add");
            BindVendor(1, "Add");
            BindLookUp("Add");
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            SearchGrid();
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                BALMachinePartsReceiveOrder lstMp = new BALMachinePartsReceiveOrder();

                if (drpcor.SelectedValue == "All")
                {
                    lstMp.ListCorporateID = "ALL";
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

                    lstMp.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacility.SelectedValue == "All")
                {
                    lstMp.ListFacilityID = "ALL";
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
                    lstMp.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendor.SelectedValue == "All")
                {
                    lstMp.ListVendorID = "ALL";
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
                    lstMp.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    lstMp.FinalStatus = "ALL";
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
                    lstMp.FinalStatus = FinalString;
                }
                SB.Clear();


                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }                
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchMachinePartsReceiveSummaryReport> lstMSRMaster = lclsservice.SearchMachinePartsReceiveSummaryReport(lstMp).ToList();
                rvMachineSummaryPritnAll.ProcessingMode = ProcessingMode.Local;
                rvMachineSummaryPritnAll.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsReceivingSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("MachinePartsReceivingPrintAllDS", lstMSRMaster);
                rvMachineSummaryPritnAll.LocalReport.DataSources.Clear();
                rvMachineSummaryPritnAll.LocalReport.DataSources.Add(datasource);
                rvMachineSummaryPritnAll.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvMachineSummaryPritnAll.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MachinePartsReceivingOrderSummary" + guid + ".pdf";
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


                //List<object> llstresult = SearchOrderReport("");
                //byte[] bytes = (byte[])llstresult[0];
                //Guid guid = Guid.NewGuid();
                //_sessionPDFFileName = "SessionFile" + guid + ".pdf";
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                //path = Path.Combine(path, _sessionPDFFileName);
                //using (StreamWriter sw = new StreamWriter(File.Create(path)))
                //{
                //    sw.Write("");
                //}
                //FileStream fs = new FileStream(path, FileMode.Open);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();
                //ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
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
                BindFacility(1,"Add");
                DivMultiCorp.Style.Add("display", "none");
                divMPReceiveOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divMPReceiveOrder.Attributes["class"] = "mypanel-body";
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
                BindVendor(1,"Add");
                DivFacCorp.Style.Add("display", "none");
                divMPReceiveOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divMPReceiveOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {

            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divMPReceiveOrder.Attributes["class"] = "Upopacity";
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
            BindCorporate();
            BindFacility(1,"Add");
            BindVendor(1,"Add");
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
                    divMPReceiveOrder.Attributes["class"] = "Upopacity";
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
            BindFacility(1,"Add");
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