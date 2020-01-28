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
'' Name      :   <<MachinePartsRequestOrder>>
'' Type      :   C# File
'' Description  :<<To add,update the Machine Parts Request Order Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
 *  12/26/2017         V2.0                Sairam.P                         New
 
 ''--------------------------------------------------------------------------------
'*/
#endregion


namespace Inventory
{
    public partial class MachinePartsOrder : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        #endregion
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALMPRMaster llstMPRMaster = new BALMPRMaster();
        EmailController objemail = new EmailController();
        string a = string.Empty;
        string b = string.Empty;
        string ErrorList = string.Empty;
        string actionOrder = Constant.actionOrder;
        string actionApprove = Constant.actionApprove;
        string actionDeny = Constant.actionDeny;
        string actionHold = Constant.actionHold;
        string StatusPendngApproval = Constant.PendingApprovalStatus;
        string StatusOrder = Constant.OrderStatus;
        string StatusApprove = Constant.PendingOrderStatus;
        string StatusDeny = Constant.DeniedStatus;
        string StatusHold = Constant.HoldStatus;
        string GeneratApprove = Constant.btnGenerateApprove;
        string GenerateOrder = Constant.btnGenerateOrder;
        string statusdrp = Constant.PendingApprovalfornonuser;
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
               // scriptManager.RegisterPostBackControl(this.btnGenerateOrder);
                scriptManager.RegisterPostBackControl(this.btnPrintAll);
                scriptManager.RegisterPostBackControl(this.grdMPRMaster);
                scriptManager.RegisterPostBackControl(this.grdreviewMPO);

                if (!IsPostBack)
                {//BindCorporate();

                    if (defaultPage != null)
                    {
                        BindCorporate();
                        //drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility();
                        //drpfacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindVendor();
                        BindLookUp("Add");
                        BindMachinePartsGrid();
                        //drpcor.SelectedIndex = 0;
                        //drpfacility.SelectedIndex = 0;
                      
                        if (defaultPage.RoleID == 1)
                        {
                            btnGenerateOrder.Text = GenerateOrder;
                            //foreach (GridViewRow row in grdMPRMaster.Rows)
                            //{
                            //    ImageButton imgsend = (ImageButton)row.FindControl("imgsend");
                            //    imgsend.Visible = true;
                            //}
                            btnOrderAll.Visible = true;
                            btnApproveAll.Visible = false;
                            drpcor.Enabled = true;
                            drpfacility.Enabled = true;
                        }
                        else
                        {
                            //foreach (GridViewRow row in grdMPRMaster.Rows)
                            //{
                            //    ImageButton imgsend = (ImageButton)row.FindControl("imgsend");
                            //    imgsend.Visible = false;
                            //}
                            btnOrderAll.Visible = false;
                            btnApproveAll.Visible = true;
                            btnGenerateOrder.Text = GeneratApprove;
                            //drpcor.Enabled = false;
                            //drpfacility.Enabled = false;

                        }
                        if (defaultPage.MachinePartsOrder_Edit == false && defaultPage.MachinePartsOrder_View == true)
                        {
                            //btnAdd.Visible = false;
                            btnOrderAll.Visible = false;
                            btnReview.Visible = false;
                            btnImgDeletePopUp.Visible = false;

                        }
                        if (defaultPage.MachinePartsOrder_Edit == false && defaultPage.MachinePartsOrder_View == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                    //BindCorporate();
                    //BindFacility();
                    //BindVendor();
                    //BindLookUp("Add");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }
        #endregion


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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
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
                BindVendor();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
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

                //}
                //else
                //{
                //    lstvendordetails = lclsservice.GetFacilityVendorAccount(drpfacility.SelectedItem.Text).Where(a => a.MachineParts == true).Distinct().ToList();
                //    drpvendor.DataSource = lstvendordetails;
                //    drpvendor.DataTextField = "VendorDescription";
                //    drpvendor.DataValueField = "VendorID";
                //    drpvendor.DataBind();
                //    ListItem lst = new ListItem();
                //    lst.Value = "All";
                //    lst.Text = "All";
                //    drpvendor.Items.Insert(0, lst);
                //    drpvendor.SelectedIndex = 0;
                //}


            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);

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
                lstLookUp = lclsservice.GetList("MachinePO", "Status", Mode).ToList();
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatus.Items.Insert(0, lst);
                  if(defaultPage.RoleID == 1)
                  {
                      drpStatus.Items.FindByText(StatusApprove).Selected = true;
                  }
                  else
                  {
                      drpStatus.Items.FindByText(statusdrp).Selected = true;
                  }
               // drpStatussearch.SelectedIndex = 0;
            }
               
                // Search Status Drop Down
               

            
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Machine Parts Request Master details from MPRMaster table to Grid control 
        /// </summary>
        #region Bind Machine Parts Request Master Values
        public void BindMachinePartsGrid()
        {
            try
            {
                BALMachinePartsOrder lstMp = new BALMachinePartsOrder();
                
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
                    lstMp.Status = "ALL";
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
                    lstMp.Status = FinalString;
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
                lstMp.Status = Convert.ToString(drpStatus.SelectedValue);
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchMPRMasterOrder> lstMSRMaster = lclsservice.SearchMPRMasterOrder(lstMp).ToList();
                grdMPRMaster.DataSource = lstMSRMaster;
                grdMPRMaster.DataBind();
                int i = 0;
                foreach (GridViewRow row in grdMPRMaster.Rows)
                {
                    DropDownList drp1 = (DropDownList)row.FindControl("drpaction");
                    List<string> SplitAction = new List<string>();
                    SplitAction = lstMSRMaster[i].Action.Split(',').ToList();

                    drp1.DataSource = SplitAction;
                    drp1.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Action--";
                    drp1.Items.Insert(0, lst);
                    drp1.SelectedIndex = 0;
                    i++;
                }
             
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
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
                BALMachinePartsOrder lstMp = new BALMachinePartsOrder();

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
                    lstMp.Status = "ALL";
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
                    lstMp.Status = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text != "") lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                if (txtDateTo.Text != "") lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);                
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchMPRMasterOrder> lstMSRMaster = lclsservice.SearchMPRMasterOrder(lstMp).ToList();
                grdMPRMaster.DataSource = lstMSRMaster;
                grdMPRMaster.DataBind();
                 int i = 0;
                foreach (GridViewRow row in grdMPRMaster.Rows)
                {
                    DropDownList drp1 = (DropDownList)row.FindControl("drpaction");
                    List<string> SplitAction = new List<string>();
                    SplitAction = lstMSRMaster[i].Action.Split(',').ToList();

                    drp1.DataSource = SplitAction;
                    drp1.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Action--";
                    drp1.Items.Insert(0, lst);
                    drp1.SelectedIndex = 0;
                    i++;
                }
             
            }
                //ViewState["ReportMachinePartsID"] = "";

            
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
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
                BindFacility();
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
                BindFacility();
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
                BindVendor();
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
                BindVendor();
            }
        }
        #endregion


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }



        private void ClearDetails()
        {
            try
            {
                drpcor.ClearSelection();
                drpfacility.ClearSelection();
                drpvendor.ClearSelection();
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                drpStatus.ClearSelection();
                //drpcor.SelectedIndex = 0;
                //drpfacility.SelectedIndex = 0;
                BindCorporate();
                BindFacility();
                BindVendor();
                BindLookUp("Add");
                grdMPRMaster.DataSource = null;
                grdMPRMaster.DataBind();
                ViewState["MPRID"] = "";
                HddListCorpID.Value = "";
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }
        protected void drpaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSpin2 = (DropDownList)sender;
            GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
            int RowIndex = gridrow.RowIndex;
            DropDownList drpaction = (DropDownList)grdMPRMaster.Rows[RowIndex].FindControl("drpaction");
            TextBox txtremarks = (TextBox)grdMPRMaster.Rows[RowIndex].FindControl("txtremarks");    
        }

        protected void btnapproveall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnApproveAll.Text == "Approve All")
                {
                    HdnMPRDetailsID.Value = "";
                    foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        //HddDetailsID.Value += grdfs.Cells[15].Text + ",";
                        HdnMPRDetailsID.Value += rowindex + ",";
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;
                        if (status != StatusOrder && status != StatusApprove && status!=StatusDeny)
                        {
                            drpaction.SelectedValue = actionApprove;
                            btnApproveAll.Text = "Clear All";
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                    {
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;
                        if (status != StatusOrder && status != StatusDeny)
                        {
                            DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                            drpaction.SelectedIndex = 0;
                            btnApproveAll.Text = "Approve All";
                        }
                    }
                    HdnMPRDetailsID.Value = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void btnorderall_Click(object sender, EventArgs e)
        {
            try
            {
                string status = string.Empty;
                if (btnOrderAll.Text == "Order All")
                {
                    HdnMPRDetailsID.Value = "";
                    foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HdnMPRDetailsID.Value += rowindex + ",";
                        string Status = string.Empty;
                        Status = grdfs.Cells[12].Text;
                        if (Status != StatusOrder && Status != StatusDeny)
                        {
                            drpaction.SelectedValue = actionOrder;
                        }
                        btnOrderAll.Text = "Clear All";

                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                    {
                        string Status = string.Empty;
                        Status = grdfs.Cells[12].Text;
                        if (Status != StatusOrder && Status != StatusDeny)
                        {
                            DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                            if (drpaction.Enabled == true)
                            {
                                drpaction.SelectedIndex = 0;
                            }
                          
                        }
                        btnOrderAll.Text = "Order All";
                    }
                    HdnMPRDetailsID.Value = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindMachinePartsGrid();
         
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MPRMasterID");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("CorporateID");
            dt.Columns.Add("FacilityID");
            dt.Columns.Add("VendorID");
            dt.Columns.Add("CorporateName");
            dt.Columns.Add("FacilityShortName");
            dt.Columns.Add("VendorShortName");
            dt.Columns.Add("MPRNo");
            dt.Columns.Add("MPONo");
            dt.Columns.Add("TotalCost");
            dt.Columns.Add("Status");
            dt.Columns.Add("Action");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Remarks");
            //dt.Columns.Add("CreatedBy");
            dt.Columns.Add("OldStatus");
            dt.AcceptChanges();
            return dt;
        }


        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                Int64 MPRID = 0;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                MPRID = Convert.ToInt64(gvrow.Cells[1].Text);
                Response.Redirect("MachinePartRequest.aspx?MPRID=" + MPRID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void lbMPONo_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 MPRMasterID;
                MPRMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbMPONo = (LinkButton)gvrow.FindControl("lbMPONo");
                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Machine Parts Order – " + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                //path = Path.Combine(path,_sessionPDFFileName);
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalOrderMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }

        }
        protected void btnReview_Click(object sender, EventArgs e)
        {
            BindReview();
        }

        private void BindReview()
        {
          try
            {
               string OrderIDS = Convert.ToString(HdnMPRDetailsID.Value);
                DataTable dt = CreateDataTable();
                ViewState["MPRID"] = "";
                if (OrderIDS != "")
                {
                    OrderIDS = OrderIDS.Substring(0, OrderIDS.Length - 1);
                    string[] values = OrderIDS.Split(',');
                    string[] c = values.Distinct().ToArray();
                    //for (int i = 0; i < c.Length; i++)
                    //{
                    foreach(string val in c)
                    {
                        if (val != "")
                        {
                            GridViewRow row = grdMPRMaster.Rows[Convert.ToInt32(val)];
                            string MPRMasterID = row.Cells[1].Text;
                            string CorporateID = row.Cells[3].Text;
                            string FacilityID = row.Cells[4].Text;
                            string VendorID = row.Cells[5].Text;
                            string CreatedOn = row.Cells[2].Text;
                            string CorporateName = row.Cells[6].Text;
                            string FacilityShortName = row.Cells[7].Text;
                            string VendorShortName = row.Cells[8].Text;
                            LinkButton lbMPRNo = (LinkButton)row.FindControl("lbMPRNo");
                            LinkButton lbMPONo = (LinkButton)row.FindControl("lbMPONo");
                            string TotalPrice = row.Cells[11].Text;
                            string Status = row.Cells[12].Text;
                            DropDownList drpaction = (DropDownList)row.FindControl("drpaction");
                            //Label CreatedBy = (Label)row.FindControl("CreatedBy");
                            Image imgreadmore1 = (Image)row.FindControl("imgreadmore1");
                            Label lblAudit = (Label)row.FindControl("lblAudit");

                            if (drpaction.SelectedItem.Text == actionApprove)
                            {
                                Status = StatusApprove;
                                btnGenerateOrder.Text = GeneratApprove;
                                btnGenerateOrder.Visible = true;

                            }
                            else if (drpaction.SelectedItem.Text == actionOrder)
                            {
                                Status = StatusOrder;
                                btnGenerateOrder.Text = GenerateOrder;
                                btnGenerateOrder.Visible = true;
                            }
                            else if (drpaction.SelectedItem.Text == actionDeny)
                            {
                                Status = StatusDeny;
                                btnGenerateOrder.Text = GeneratApprove;
                                btnGenerateOrder.Visible = true;

                            }
                            else if (drpaction.SelectedItem.Text == actionHold)
                            {
                                Status = StatusHold;
                                btnGenerateOrder.Text = GeneratApprove;
                                btnGenerateOrder.Visible = true;
                            }


                            //TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                            Label lblRemarks = (Label)row.FindControl("lblRemarks");
                            DataRow dr = dt.NewRow();
                            if (drpaction.SelectedIndex != 0)
                            {
                                //DataRow dr = dt.NewRow();
                                dr["MPRMasterID"] = MPRMasterID;
                                dr["CreatedOn"] = CreatedOn;
                                dr["CorporateID"] = CorporateID;
                                dr["FacilityID"] = FacilityID;
                                dr["VendorID"] = VendorID;
                                dr["CorporateName"] = CorporateName;
                                dr["FacilityShortName"] = FacilityShortName;
                                dr["VendorShortName"] = VendorShortName;
                                dr["MPRNo"] = lbMPRNo.Text;
                                dr["MPONo"] = lbMPONo.Text;
                                dr["TotalCost"] = TotalPrice;
                                dr["Status"] = Status;
                                dr["Action"] = drpaction.SelectedItem.Text; ;
                                dr["Audit"] = lblAudit.Text;
                                dr["Remarks"] = lblRemarks.Text;
                                //dr["CreatedBy"] = CreatedBy.Text;
                                dr["OldStatus"] = row.Cells[12].Text;
                                dt.Rows.Add(dr);
                            }
                        }
                        ViewState["OldStatus"] = dt;
                        grdreviewMPO.DataSource = dt;
                        grdreviewMPO.DataBind();
                        mpereview.Show();

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarngActionMessage.Replace("<<MajorItemOrder>>", ""), true);
                }
            }
                catch(Exception ex)
            {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCapitalOrderMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        public void GetMPONo(string MPRMasterID)
        {
            List<GetMachinePartsOrderMPONo> lclMPO = lclsservice.GetMachinePartsOrderMPONo(MPRMasterID).ToList();
            grdreviewMPO.DataSource = lclMPO;
            grdreviewMPO.DataBind();
          //BindReview();
        }

        protected void btngenerateorder_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {
                BALMachinePartsOrder objmachinerequest = new BALMachinePartsOrder();
                Functions objfun = new Functions();
                string meditem = string.Empty;
                Int64 medsave = 0;
                //  int rowcount = gvmedreview.Rows.Count;
                string MPOIns = string.Empty;
                string MPOUpd = string.Empty;
                int ReviewCount = grdreviewMPO.Rows.Count;
                if (ReviewCount != 0)
                {
                    foreach (GridViewRow row in grdreviewMPO.Rows)
                    {
                        try
                        {
                            #region multi order generation start
                            //GridViewRow row = grdreviewMPO.Rows[i];
                            LinkButton lbrevMPRNo = (LinkButton)row.FindControl("lbrevMPRNo");
                            string result = lbrevMPRNo.Text;
                            var MPONo = "MPO" + result.Substring(2);
                            LinkButton lbrevMPONo = (LinkButton)row.FindControl("lbrevMPONo");
                            string lbltotal = row.Cells[11].Text;
                            string totprice = lbltotal.Substring(1);
                            string Status = row.Cells[12].Text;
                            objmachinerequest.CorporateID = Convert.ToInt64(row.Cells[3].Text);
                            objmachinerequest.FacilityID = Convert.ToInt64(row.Cells[4].Text);
                            objmachinerequest.VendorID = Convert.ToInt64(row.Cells[5].Text);
                            objmachinerequest.OrderDate = Convert.ToDateTime(row.Cells[2].Text);
                            objmachinerequest.MPRMasterID = Convert.ToInt64(row.Cells[1].Text);
                            ViewState["MPRID"] += row.Cells[1].Text + ",";
                            ViewState["MPRMID"] = objmachinerequest.MPRMasterID;
                            Int64 MPRMasterID = objmachinerequest.MPRMasterID;
                            objmachinerequest.MPONo = MPONo;
                            // throw new Exception(MPONo + "Manual generated error");
                            byte[] data = new byte[0];
                            objmachinerequest.OrderContent = data;
                            objmachinerequest.TotalPrice = Convert.ToDecimal(totprice);
                            objmachinerequest.Status = Status;
                            //Label lblremarks = (Label)row.FindControl("lblRemarks");
                            TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                            objmachinerequest.Remarks = txtremarks.Text;
                            objmachinerequest.CreatedBy = defaultPage.UserId;
                            //objmachinerequest.LastModifiedBy = defaultPage.UserId;

                            if (Status == StatusOrder)
                            {
                                meditem = lclsservice.InsertMachinePO(objmachinerequest);
                                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                                byte[] bytes = (byte[])llstresult[2];
                                objmachinerequest.OrderContent = bytes;
                                MemoryStream attachstream = new MemoryStream(bytes);
                                Int64 CorporateID = objmachinerequest.CorporateID;
                                Int64 FacilityID = objmachinerequest.FacilityID;
                                objemail.FromEmail = llstresult[0].ToString();
                                objemail.ToEmail = llstresult[1].ToString();
                                EmailSetting(CorporateID, FacilityID, MPONo);
                                meditem = lclsservice.UpdateMachinePO(objmachinerequest);
                                #region SendEmail code block
                                try
                                {
                                    objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                                }
                                catch (Exception sendemailce)
                                {
                                    LinkButton lbMPRNo = (LinkButton)row.FindControl("lbrevMPRNo");
                                    string result1 = lbMPRNo.Text;
                                    var MPO = "MPO" + result1.Substring(2);
                                    errmsg = errmsg + "Error in MPO[" + MPO + "] - Send Email- " + sendemailce.Message.ToString();
                                }
                                #endregion SendEmail code block
                                mpereview.Show();
                                btnGenerateOrder.Visible = true;
                                btnreviewcancel.Text = "Go Back";
                                txtremarks.Visible = false;
                                BindMachinePartsGrid();
                            }
                            if (Status == StatusApprove || Status == StatusHold || Status == StatusDeny)
                            {
                                btnGenerateOrder.Visible = true;
                                MPOIns = lclsservice.InsertMachineApprove(objmachinerequest);
                                mpereview.Show();
                                txtremarks.Text = "";
                                btnreviewcancel.Text = "Go Back";
                                btnGenerateOrder.Visible = false;
                                txtremarks.Visible = false;
                                BindMachinePartsGrid();
                            }
                            #endregion multi order generation end
                        }
                        catch (Exception innerce)
                        {
                            LinkButton lbtnMPRNo = (LinkButton)row.FindControl("lbrevMPRNo");
                            string result = lbtnMPRNo.Text;
                            var MPO = "MPO" + result.Substring(2);
                            errmsg = errmsg + "Error in MPO[" + MPO + "] - " + innerce.Message.ToString();
                        }
                    }
                }
                if (errmsg != string.Empty) throw new Exception(errmsg);
                if (MPOIns == "Saved Successfully")
                {
                    lblmsg.Text = Constant.MachinePartsOrderUpdateReq;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderupdateMessage.Replace("<<MachinePartsOrder>>", ""), true);
                }
                if (meditem == "Saved Successfully")
                {

                    lblmsg.Text = Constant.MachinePartsOrderReq;
                    List<GetMachinePartsOrderMPONo> lstrwpo = lclsservice.GetMachinePartsOrderMPONo(Convert.ToString(ViewState["MPRID"])).ToList();
                    btnOrderAll.Text = "Order All";
                    grdreviewMPO.DataSource = lstrwpo;
                    grdreviewMPO.DataBind();
                    btnGenerateOrder.Visible = false;
                    mpereview.Show();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }

        }

        public List<object> DetailsOrderReport(Int64 MPRMasterID)
        {

            List<object> llstarg = new List<object>();
            List<GetMPOrderContentPO> llstreview = lclsservice.GetMPOrderContentPO(MPRMasterID,defaultPage.UserId).ToList();
            rvMachinePoreport.ProcessingMode = ProcessingMode.Local;
            rvMachinePoreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsPOPDF.rdlc");
            ReportParameter[] p1 = new ReportParameter[1];
            p1[0] = new ReportParameter("MPRMasterID", Convert.ToString(MPRMasterID));
            this.rvMachinePoreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MachinePartspdfDS", llstreview);
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
            llstarg.Insert(0, FromEmail);
            llstarg.Insert(1, ToEmail);
            llstarg.Insert(2, bytes);
            llstarg.Insert(3, CorporateID);
            llstarg.Insert(4, FacilityID);
            return llstarg;
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
                objemail.vendorEmailcontent = string.Format("Please see the attached document for order details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br />Thank you for service <br/><br/>" + Superadmin);

                objemail.vendoremailsubject = "Machine Parts Order – " + MPONo;
                string displayfilename = "Machine Parts Order – " + MPONo + ".pdf";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        public List<object> SearchOrderReport(string MPRMasterID)
        {
            string smedmasterIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<BindMachinePOReport> llstreview = new List<BindMachinePOReport>();
            if (MPRMasterID == "")
            {
                foreach (GridViewRow row in grdMPRMaster.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (smedmasterIds == string.Empty)
                            smedmasterIds = row.Cells[1].Text;
                        else
                            smedmasterIds = smedmasterIds + "," + row.Cells[1].Text;
                    }
                }

                llstreview = lclsservice.BindMachinePOReport(null, smedmasterIds, defaultPage.UserId,defaultPage.UserId).ToList();
            }
            else
            {
                llstreview = lclsservice.BindMachinePOReport(MPRMasterID, null, defaultPage.UserId,defaultPage.UserId).ToList();
            }
            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvMachinePoreportReview.ProcessingMode = ProcessingMode.Local;
            rvMachinePoreportReview.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePOReview.rdlc");
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("MPRMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

            this.rvMachinePoreportReview.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MachinePartsOrderReview", llstreview);
            rvMachinePoreportReview.LocalReport.DataSources.Clear();
            rvMachinePoreportReview.LocalReport.DataSources.Add(datasource);
            rvMachinePoreportReview.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvMachinePoreportReview.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }
        protected void lbMPRNo_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                string s = string.Empty;
                Int64 MPRID = 0;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                MPRID = Convert.ToInt64(gvrow.Cells[1].Text);
                HddMasterID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetMPRMaster> lstMPR = lclsservice.GetMPRMaster().Where(a => a.MPRMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                //foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                //{
                //    if (HddMasterID.Value == gvrow.Cells[1].Text)
                //    {
                //        s = gvrow.Cells[8].Text;
                //    }

                //}  
                Div1.Style.Add("display", "block");
                lblMPa.Text = lstMPR[0].MPRNo;
                lblMPRCorporate.Text = lstMPR[0].CorporateName;
                lblMPRFac.Text = lstMPR[0].FacilityDescription;
                lblMPRVendor.Text = lstMPR[0].VendorDescription;
                lblMPREquip.Text = lstMPR[0].EquipmentCategory;
                lblMPREquipSubCat.Text = lstMPR[0].EquipmentSubCategory;
                lblMPREquiplist.Text = lstMPR[0].EquipementList;
                lblMPRhours.Text = lstMPR[0].Hoursonmachine;
                lblSNo.Text = lstMPR[0].SerialNo;
                lblMPRship.Text = lstMPR[0].Shipping;
                lblMPa.Text = lstMPR[0].MPRNo;
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetMPRDetailsbyMPRMasterID> llstMPRDetails = lclsservice.GetMPRDetailsbyMPRMasterID(Convert.ToInt64(HddMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();

                grdMPRreview.DataSource = llstMPRDetails;
                grdMPRreview.DataBind();
                int i = 0;


                foreach (GridViewRow row in grdMPRreview.Rows)
                {
                    Label lblRowNumber = (Label)row.FindControl("lblRowNumber");
                    Label lblItemID = (Label)row.FindControl("lblItemID");
                    Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                    Label lblUOM = (Label)row.FindControl("lblUOM");
                    Label lblPricePerUnit = (Label)row.FindControl("lblPricePerUnit");
                    Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    Label lbMPRMasterID = (Label)row.FindControl("lbMPRMasterID");
                    Label lbMPRDetailsID = (Label)row.FindControl("lbMPRDetailsID");


                    lblItemID.Text = llstMPRDetails[i].ItemID;
                    lblItemDescription.Text = llstMPRDetails[i].ItemDescription;
                    lblUOM.Text = llstMPRDetails[i].UOM;
                    lblPricePerUnit.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].PricePerUnit));
                    lblOrderQuantity.Text = llstMPRDetails[i].OrderQuantity.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].TotalPrice));
                    lbMPRMasterID.Text = llstMPRDetails[i].MPRMasterID.ToString();
                    lbMPRDetailsID.Text = llstMPRDetails[i].MPRDetailsID.ToString();
                    i++;
                }
                TextBox txtShippingCost = grdMPRreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txtTax = grdMPRreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalCost = grdMPRreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                txtShippingCost.Text = lstMPR[0].ShippingCost;
                txtTax.Text = lstMPR[0].Tax;
                txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", lstMPR[0].TotalCost));
                //decimal sum = 0;
                //for (int r = 0; r < grdMPRreview.Rows.Count; ++r)
                //{
                //    Label lblTotalPrice = grdMPRreview.Rows[r].FindControl("lblTotalPrice") as Label;
                //    sum += Convert.ToDecimal(lblTotalPrice.Text);
                //}
                //(grdMPRreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
                //if (lstMPR[0].ShippingCost != "" && lstMPR[0].Tax != "")
                //{
                //    sum += Convert.ToDecimal(lstMPR[0].ShippingCost) + Convert.ToDecimal(lstMPR[0].Tax);
                //    (grdMPRreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                //}
                //else
                //{
                //    sum += Convert.ToDecimal(0) + Convert.ToDecimal(0);
                //    (grdMPRreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
                //}

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                BALMachinePartsOrder lstMp = new BALMachinePartsOrder();

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
                    lstMp.Status = "ALL";
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
                    lstMp.Status = FinalString;
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
                List<SearchMPRMasterOrder> lstMSRMaster = lclsservice.SearchMPRMasterOrder(lstMp).ToList();
                rvMachinePoreportReview.ProcessingMode = ProcessingMode.Local;
                rvMachinePoreportReview.LocalReport.ReportPath = Server.MapPath("~/Reports/MachinePartsOrderSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetMPRMasterOrderDS", lstMSRMaster);
                rvMachinePoreportReview.LocalReport.DataSources.Clear();
                rvMachinePoreportReview.LocalReport.DataSources.Add(datasource);
                rvMachinePoreportReview.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvMachinePoreportReview.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MachinePartsOrderSummary" + guid + ".pdf";
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
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string MPRMasterID = string.Empty;
                MPRMasterID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void imgsend_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 MPRMasterID;
                MPRMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbMPONo = (LinkButton)gvrow.FindControl("lbMPONo");
                string MPONo = lbMPONo.Text;
                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(llstresult[3].ToString());
                Int64 FacilityID = Convert.ToInt64(llstresult[4].ToString());
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                EmailSetting(CorporateID, FacilityID,MPONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderemailMessage.Replace("<<MachinePartsOrder>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }


        protected void btnreviewcancel_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
            lblmsg.Text = "";

        }

        protected void imgrevprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 MPRMasterID;
                MPRMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
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
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void grdreviewMPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string ActionStatus = string.Empty;
                string remarks = string.Empty;
                //string Status = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtremarks = (TextBox)e.Row.FindControl("txtremarks");
                    ActionStatus = e.Row.Cells[13].Text;
                    //Label lblaction = (Label)e.Row.FindControl("lblaction");
                    RequiredFieldValidator rxd = (RequiredFieldValidator)e.Row.FindControl("rfvRemarks");
                    DataTable dt = (DataTable)ViewState["OldStatus"];
                    remarks = Constant.RfvRemarkMPO;
                    rxd.Text = remarks;
                    string Status = string.Empty;
                    Status = e.Row.Cells[12].Text;
                    string Oldstatus = dt.Rows[e.Row.RowIndex]["Oldstatus"].ToString();
                    if ((Oldstatus == StatusPendngApproval && ActionStatus == actionApprove) || (Oldstatus == StatusPendngApproval && ActionStatus == actionOrder))
                    {
                        txtremarks.Visible = false;
                        rxd.Visible = false;
                    }
                    else if ((Oldstatus == StatusApprove) && (ActionStatus == actionOrder))
                    {
                        txtremarks.Visible = false;
                        rxd.Visible = false;
                    }
                    else
                    {
                        txtremarks.Visible = true;
                        rxd.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void grdMPRMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string Status = string.Empty;

                foreach (GridViewRow row in grdMPRMaster.Rows)
                {

                    Status = row.Cells[12].Text;
                    DropDownList drpaction = (DropDownList)row.FindControl("drpaction");
                    ImageButton imgbtnEdit = (ImageButton)row.FindControl("imgbtnEdit");
                    ImageButton imgsend = (ImageButton)row.FindControl("imgsend");
                    //Label lblremarks = (Label)row.FindControl("lblRemarks");
                    //Label lblAudit = (Label)row.FindControl("lblAudit");
                    //Image imgreadmore = (Image)row.FindControl("imgreadmore");
                    //Image imgreadmore1 = (Image)row.FindControl("imgreadmore1");

                    if (Status == StatusOrder || Status == StatusDeny)
                    {
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;
                        imgsend.Visible = true;
                        if (Status == StatusDeny || defaultPage.UserId != 1)
                        {
                            imgsend.Visible = false;
                        }
                    }
                    else
                    {
                        drpaction.Enabled = true;
                        imgbtnEdit.Visible = true;
                        imgsend.Visible = false;
                    }
                    //if (lblremarks.Text != "")
                    //{
                    //    if (lblremarks.Text.Length > 150)
                    //    {
                    //        lblremarks.Text = lblremarks.Text.Substring(0, 150) + "....";
                    //        imgreadmore.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        imgreadmore.Visible = false;
                    //    }

                    //}

                    //if (lblAudit.Text.Length > 150)
                    //{
                    //    lblAudit.Text = lblAudit.Text.Substring(0, 150) + "....";
                    //    imgreadmore1.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmore1.Visible = false;
                    //}


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void lbrevMPRNo_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                string s = string.Empty;
                Int64 MPRID = 0;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                MPRID = Convert.ToInt64(gvrow.Cells[1].Text);
                HddMasterID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetMPRMaster> lstMPR = lclsservice.GetMPRMaster().Where(a => a.MPRMasterID == Convert.ToInt64(HddMasterID.Value)).ToList();
                //foreach (GridViewRow grdfs in grdMPRMaster.Rows)
                //{
                //    if (HddMasterID.Value == gvrow.Cells[1].Text)
                //    {
                //        s = gvrow.Cells[8].Text;
                //    }

                //}  
                Div1.Style.Add("display", "block");
                lblMPRCorporate.Text = lstMPR[0].CorporateName;
                lblMPRFac.Text = lstMPR[0].FacilityDescription;
                lblMPRVendor.Text = lstMPR[0].VendorDescription;
                lblMPREquip.Text = lstMPR[0].EquipmentCategory;
                lblMPREquipSubCat.Text = lstMPR[0].EquipmentSubCategory;
                lblMPREquiplist.Text = lstMPR[0].EquipementList;
                lblMPRhours.Text = lstMPR[0].Hoursonmachine;
                lblSNo.Text = lstMPR[0].SerialNo;
                lblMPRship.Text = lstMPR[0].Shipping;
                lblMPa.Text = lstMPR[0].MPRNo;
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetMPRDetailsbyMPRMasterID> llstMPRDetails = lclsservice.GetMPRDetailsbyMPRMasterID(Convert.ToInt64(HddMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();

                grdMPRreview.DataSource = llstMPRDetails;
                grdMPRreview.DataBind();
                int i = 0;


                foreach (GridViewRow row in grdMPRreview.Rows)
                {
                    Label lblRowNumber = (Label)row.FindControl("lblRowNumber");
                    Label lblItemID = (Label)row.FindControl("lblItemID");
                    Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                    Label lblUOM = (Label)row.FindControl("lblUOM");
                    Label lblPricePerUnit = (Label)row.FindControl("lblPricePerUnit");
                    Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    Label lbMPRMasterID = (Label)row.FindControl("lbMPRMasterID");
                    Label lbMPRDetailsID = (Label)row.FindControl("lbMPRDetailsID");


                    lblItemID.Text = llstMPRDetails[i].ItemID;
                    lblItemDescription.Text = llstMPRDetails[i].ItemDescription;
                    lblUOM.Text = llstMPRDetails[i].UOM;
                    lblPricePerUnit.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].PricePerUnit));
                    lblOrderQuantity.Text = llstMPRDetails[i].OrderQuantity.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstMPRDetails[i].TotalPrice));
                    lbMPRMasterID.Text = llstMPRDetails[i].MPRMasterID.ToString();
                    lbMPRDetailsID.Text = llstMPRDetails[i].MPRDetailsID.ToString();
                    i++;
                }
                TextBox txtShippingCost = grdMPRreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txtTax = grdMPRreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalCost = grdMPRreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                txtShippingCost.Text = lstMPR[0].ShippingCost;
                txtTax.Text = lstMPR[0].Tax;
                txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", lstMPR[0].TotalCost));
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void lbrevMPONo_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 MPRMasterID;
                MPRMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbrevMPONo = (LinkButton)gvrow.FindControl("lbrevMPONo");
                List<object> llstresult = DetailsOrderReport(MPRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();
                _sessionPDFFileName = "Machine Parts Order – " + guid + ".pdf";
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                using (StreamWriter sw = new StreamWriter(File.Create(Server.MapPath(_sessionPDFFileName))))
                {
                    sw.Write("");
                }
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path);
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClearDetails();
            BindMachinePartsGrid();
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
                BindFacility();
                DivMultiCorp.Style.Add("display", "none");
                divMPOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divMPOrder.Attributes["class"] = "mypanel-body";
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
                BindVendor();
                DivFacCorp.Style.Add("display", "none");
                divMPOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divMPOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divMPOrder.Attributes["class"] = "Upopacity";
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
            BindFacility();
            BindVendor();
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
                    divMPOrder.Attributes["class"] = "Upopacity";
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
            BindFacility();
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