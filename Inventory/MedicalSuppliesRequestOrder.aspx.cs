using Inventory.Class;
using Inventory.Inventoryserref;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
using iTextSharp.text;
using System.Reflection;
using iTextSharp.text.pdf;

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<MachinePartsRequestOrder>>
'' Type      :   C# File
'' Description  :<<To add,update the Medicalsupplies Order Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
 *  JAN/04/2018         V1.0                Dhanasekaran.C                         New 
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class MedicalSuppliesRequestOrder : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALMedicalSuppliesRequest llstMedicalSupplyRequest = new BALMedicalSuppliesRequest();
        EmailController objemail = new EmailController();
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
        StringBuilder SB = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.gvmedreview);
            //scriptManager.RegisterPostBackControl(this.btngenerateorder);
            scriptManager.RegisterPostBackControl(this.btnsearchprint);
            scriptManager.RegisterPostBackControl(this.grdOrderSearch);
            scriptManager.RegisterPostBackControl(this.gvmedreview);
            if (!IsPostBack)
            {
                if (defaultPage != null)
                {
                    if (defaultPage.RoleID == 1)
                    {
                        BindCorporate();
                        //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility();
                        //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindVendor();
                        BindLookUp("Add");
                        BindGrid();                        
                    }
                    else
                    {
                        updmain.Visible = false;
                        User_Permission_Message.Visible = true;
                    }
                    if (defaultPage.MedicalSuppliesOrder_Edit == false && defaultPage.MedicalSuppliesOrder_View == true)
                    {

                    }
                    if (defaultPage.MedicalSuppliesOrder_Edit == false && defaultPage.MedicalSuppliesOrder_View == false)
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


        protected void btnMultiCorpselect_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (System.Web.UI.WebControls.ListItem lst1 in drpcorsearch.Items)
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
                        foreach (System.Web.UI.WebControls.ListItem lst1 in drpcorsearch.Items)
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
                DivMedicalRequestOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            DivMedicalRequestOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                DivMedicalRequestOrder.Attributes["class"] = "Upopacity";
                int i = 0;
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    GrdMultiCorp.DataSource = lstcrop;
                    GrdMultiCorp.DataBind();
                    foreach (System.Web.UI.WebControls.ListItem lst1 in drpcorsearch.Items)
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
                    foreach (System.Web.UI.WebControls.ListItem lst1 in drpcorsearch.Items)
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
            BindCorporate();
            BindFacility();
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }


        #region Bind Corporate Values
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    drpcorsearch.DataSource = lstcrop;
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();
                    //drpcorsearch.Items.Insert(0, lst);
                    //drpcorsearch.SelectedIndex = 0;
                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();
                    //drpcorsearch.Items.Insert(0, lst);
                    //drpcorsearch.SelectedIndex = 0;
                }
                foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        #endregion

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
                foreach (System.Web.UI.WebControls.ListItem lst1 in drpfacilitysearch.Items)
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
                        foreach (System.Web.UI.WebControls.ListItem lst1 in drpfacilitysearch.Items)
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
                DivMedicalRequestOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }


        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            DivMedicalRequestOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiFac_Click(object sender, EventArgs e)
        {
            int j = 0;
            try
            {
                if (drpcorsearch.SelectedValue != "")
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
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
                    DivMedicalRequestOrder.Attributes["class"] = "Upopacity";
                    foreach (System.Web.UI.WebControls.ListItem lst1 in drpfacilitysearch.Items)
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
            BindFacility();
        }



        #region Bind Facility Values

        private void BindFacility()
        {
            try
            {
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                if (drpcorsearch.SelectedValue != "")
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
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
                    //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    //        drpfacilitysearch.DataTextField = "FacilityDescription";
                    //        drpfacilitysearch.DataValueField = "FacilityID";
                    //        drpfacilitysearch.DataBind();
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
                    //        //drpfacilitysearch.Items.Insert(0, lst);
                    //        //drpfacilitysearch.SelectedIndex = 0;
                    //    }
                    //    else
                    //    {
                    //        drpfacilitysearch.SelectedIndex = 0;
                    //    }
                    //}
                }
                foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
                BindVendor();
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
        public void BindVendor()
        {
            try
            {
                //ListItem lst = new ListItem();
                //lst.Value = "0";
                //lst.Text = "ALL";
                if (drpfacilitysearch.SelectedValue != "")
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
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
                    //drpvendorsearch.Items.Insert(0, lst);
                    //drpvendorsearch.SelectedIndex = 0;
                }
                foreach (System.Web.UI.WebControls.ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message.ToString()), true);

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
                lstLookUp = lclsservice.GetList("MedicalSuppliesRequestPO", "Status", Mode).ToList();
                //ListItem lst = new ListItem();
                //lst.Value = "0";
                //lst.Text = "All";
                // Search Status Drop Down
                drpStatus.DataSource = lstLookUp;
                drpStatus.DataTextField = "InvenValue";
                drpStatus.DataValueField = "InvenValue";
                drpStatus.DataBind();
                //drpStatus.Items.Insert(0, lst);
                drpStatus.Items.FindByText(StatusApprove).Selected = true;
                //drpStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


        #region Bind MedicalSupplies Request Master Values
        public void BindGrid()
        {
            try
            {
                BALMedicalSupplyRequestPo llstMSRSearch = new BALMedicalSupplyRequestPo();
               
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstMSRSearch.CorporateName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
                    {
                        if (lst.Selected && drpcorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    llstMSRSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstMSRSearch.FacilityName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
                    {
                        if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstMSRSearch.VendorName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpvendorsearch.Items)
                    {
                        if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.VendorName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    llstMSRSearch.Status = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpStatus.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMSRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMSRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                if (llstMSRSearch.Status == "0")
                    llstMSRSearch.Status = "All";
                llstMSRSearch.LoggedinBy = defaultPage.UserId;
                //  List<GetMedicalRequestPODetails> lstMSRMaster = lclsservice.GetMedicalRequestPODetails().ToList();
                List<SearchMedicalSupplyRequestPo> lstMSRMaster = lclsservice.SearchMedicalSupplyRequestPo(llstMSRSearch).ToList();
                grdOrderSearch.DataSource = lstMSRMaster;
                grdOrderSearch.DataBind();

                int i = 0;

                foreach (GridViewRow row in grdOrderSearch.Rows)
                {
                    DropDownList drp1 = (DropDownList)row.FindControl("drpaction");
                    List<string> SplitAction = new List<string>();
                    SplitAction = lstMSRMaster[i].Action.Split(',').ToList();

                    drp1.DataSource = SplitAction;
                    drp1.DataBind();
                    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Action--";
                    drp1.Items.Insert(0, lst);
                    drp1.SelectedIndex = 0;
                    i++;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        #endregion

        //protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindFacility();
        //}

        //protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindVendor();
        //}

        protected void grdOrderSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    Label lblAudit = (Label)e.Row.FindControl("lblaudit");
                    System.Web.UI.WebControls.Image imgreadmore = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgreadmore");
                    System.Web.UI.WebControls.Image imgreadmoreaudit = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgreadmoreaudit");
                    ImageButton imgsend = (e.Row.FindControl("imgsend") as ImageButton);
                    DropDownList drpaction = (e.Row.FindControl("drpaction") as DropDownList);
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
                    string Status = e.Row.Cells[12].Text;
                    if (Status == StatusOrder)
                    {
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;
                        imgsend.Visible = true;
                    }
                    else if (Status == StatusDeny)
                    {
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;
                    }
                    else
                    {
                        drpaction.Enabled = true;
                        imgbtnEdit.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void btnorderall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnorderall.Text == "Order All")
                {
                    HdnMSRDetailID.Value = "";
                    foreach (GridViewRow grdfs in grdOrderSearch.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HdnMSRDetailID.Value += rowindex + ",";
                        if (drpaction.Enabled == true)
                        {
                            drpaction.SelectedValue = actionOrder;
                        }
                        btnorderall.Text = "Clear All";
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdOrderSearch.Rows)
                    {
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        drpaction.SelectedIndex = 0;
                    }
                    btnorderall.Text = "Order All";
                    HdnMSRDetailID.Value = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MedicalSuppliesID");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("CorporateID");
            dt.Columns.Add("FacilityID");
            dt.Columns.Add("VendorID");
            dt.Columns.Add("CorporateName");
            dt.Columns.Add("FacilityShortName");
            dt.Columns.Add("VendorShortName");
            dt.Columns.Add("PRNo");
            dt.Columns.Add("PoNo");
            dt.Columns.Add("TotalPrice");
            dt.Columns.Add("Status");
            dt.Columns.Add("Action");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Remarks");
            dt.AcceptChanges();
            return dt;
        }

        protected void btnrevieworder_Click(object sender, EventArgs e)
        {
            string OrderIDS = Convert.ToString(HdnMSRDetailID.Value);
            DataTable dt = CreateDataTable();
            if (OrderIDS != "")
            {
                OrderIDS = OrderIDS.Substring(0, OrderIDS.Length - 1);
                string[] values = OrderIDS.Split(',');
                string[] c = values.Distinct().ToArray();
                for (int i = 0; i < c.Length; i++)
                {
                    GridViewRow row = grdOrderSearch.Rows[Convert.ToInt16(c[i])];
                    string MedicalSuppliesID = row.Cells[1].Text;
                    string CorporateID = row.Cells[2].Text;
                    string FacilityID = row.Cells[3].Text;
                    string VendorID = row.Cells[4].Text;
                    string CreatedOn = row.Cells[5].Text;
                    string CorporateName = row.Cells[6].Text;
                    string FacilityShortName = row.Cells[7].Text;
                    string VendorShortName = row.Cells[8].Text;

                    LinkButton lkbtnprno = (LinkButton)row.FindControl("lkbtnprno");
                    LinkButton lkbtnpono = (LinkButton)row.FindControl("lkbtnpono");
                    string TotalPrice = row.Cells[11].Text;
                    string Status = row.Cells[12].Text;
                    DropDownList drpaction = (DropDownList)row.FindControl("drpaction");
                    if (drpaction.SelectedItem.Text == actionApprove)
                    {
                        Status = StatusApprove;
                    }
                    else if (drpaction.SelectedItem.Text == actionOrder)
                    {
                        Status = StatusOrder;
                    }
                    else if (drpaction.SelectedItem.Text == actionDeny)
                    {
                        Status = StatusDeny;
                    }
                    else if (drpaction.SelectedItem.Text == actionHold)
                    {
                        Status = StatusHold;
                    }
                    Label lblRemarks = (Label)row.FindControl("lblRemarks");
                    Label lblaudit = (Label)row.FindControl("lblaudit");
                    string CheckStatus = row.Cells[12].Text;
                    if ((drpaction.SelectedIndex != 0) && (CheckStatus != StatusOrder) && (CheckStatus != StatusDeny))
                    {
                        DataRow dr = dt.NewRow();
                        dr["MedicalSuppliesID"] = MedicalSuppliesID;
                        dr["CorporateID"] = CorporateID;
                        dr["FacilityID"] = FacilityID;
                        dr["VendorID"] = VendorID;
                        dr["CreatedOn"] = CreatedOn;
                        dr["CorporateName"] = CorporateName;
                        dr["FacilityShortName"] = FacilityShortName;
                        dr["VendorShortName"] = VendorShortName;
                        dr["PRNo"] = lkbtnprno.Text;
                        dr["PONo"] = lkbtnpono.Text;
                        dr["TotalPrice"] = TotalPrice;
                        dr["Status"] = Status;
                        dr["Action"] = drpaction.SelectedItem.Text;
                        dr["Audit"] = lblaudit.Text;
                        dr["Remarks"] = lblRemarks.Text;
                        dt.Rows.Add(dr);
                    }
                }
                gvmedreview.DataSource = dt;
                gvmedreview.DataBind();
                btnreviewcancel.Text = "Cancel";
                btngenerateorder.Enabled = true;
                btngenerateorder.Visible = true;
                lblmessage.Text = "";
                mpemedsupplyReview.Show();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyPoNoaction.Replace("<<MedicalSupplyRequestPoDescription>>", ""), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        #region Generateorder Function (Save order table)
        protected void btngenerateorder_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {                
                BALMedicalSupplyRequestPo objmedsupplyrequest = new BALMedicalSupplyRequestPo();
                Functions objfun = new Functions();
                string meditem = string.Empty;
                int ReviewCount = gvmedreview.Rows.Count;
                if (ReviewCount != 0)
                {
                    foreach (GridViewRow row in gvmedreview.Rows)
                    {
                        try
                        {
                            #region multi order generation start
                            // GridViewRow row = gvmedreview.Rows[i];
                            LinkButton lkbtnprno = (LinkButton)row.FindControl("lkbtnprnoreview");
                            string result = lkbtnprno.Text;
                            string[] words = result.Split('-');

                            //var PONo = "PO" + words[2].Substring(2);
                            //PONo = words[0] + "-" + words[1] + "-" + PONo;


                            var PONo = "";
                            int i = 1;
                            //if (words.Length > 0)
                            //    FinalPONo = "PO" + words[words.Length - 1].Substring(2);


                            foreach (var list in words)
                            {
                                if (i == 1)
                                    PONo = list + "-";
                                else if (i == words.Length)
                                {
                                    PONo = PONo + "PO" + list.Substring(2);
                                }
                                else
                                    PONo = PONo + list + "-";

                                i++;
                            }

                            i = 1;

                            LinkButton lkbtnpono = (LinkButton)row.FindControl("lkbtnponoreview");
                            string lbltotal = row.Cells[11].Text;
                            //throw new Exception(PONo + "Manual generated error");
                            string totprice = lbltotal.Substring(1);

                            objmedsupplyrequest.CorporateID = Convert.ToInt64(row.Cells[3].Text);
                            objmedsupplyrequest.FacilityID = Convert.ToInt64(row.Cells[4].Text);
                            objmedsupplyrequest.Vendor = Convert.ToInt64(row.Cells[5].Text);
                            objmedsupplyrequest.OrderDate = Convert.ToDateTime(row.Cells[2].Text);
                            objmedsupplyrequest.PRMasterID = Convert.ToInt64(row.Cells[1].Text);
                            Int64 PRmasterID = objmedsupplyrequest.PRMasterID;
                            String PRNos = Convert.ToString(PRmasterID) + ',';
                            ViewState["PRMatserID"] += row.Cells[1].Text + ",";
                            objmedsupplyrequest.PONo = PONo;
                            byte[] data = new byte[0];
                            objmedsupplyrequest.OrderContent = data;
                            objmedsupplyrequest.TotalPrice = Convert.ToDecimal(totprice);
                            Label lblStatus = (Label)row.FindControl("lblStatus");
                            objmedsupplyrequest.Status = lblStatus.Text;
                            TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                            objmedsupplyrequest.Remarks = txtremarks.Text;
                            objmedsupplyrequest.CreatedBy = defaultPage.UserId;
                            objmedsupplyrequest.LastModifiedBy = defaultPage.UserId;
                            if (lblStatus.Text == StatusOrder)
                            {
                                meditem = lclsservice.InsertMedicalsupplyPO(objmedsupplyrequest);
                                List<object> llstresult = DetailsOrderReport(PRmasterID);
                                byte[] bytes = (byte[])llstresult[2];
                                objmedsupplyrequest.OrderContent = bytes;
                                MemoryStream attachstream = new MemoryStream(bytes);
                                Int64 CorporateID = objmedsupplyrequest.CorporateID;
                                Int64 FacilityID = objmedsupplyrequest.FacilityID;
                                objemail.FromEmail = llstresult[0].ToString();
                                objemail.ToEmail = llstresult[1].ToString();
                                EmailSetting(CorporateID, FacilityID, PONo);

                                meditem = lclsservice.UpdateMSRPoOrderContent(objmedsupplyrequest);
                                #region SendEmail code block
                                try
                                {
                                    objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                                }
                                catch (Exception sendemailce)
                                {
                                    LinkButton lkbtnprno1 = (LinkButton)row.FindControl("lkbtnprnoreview");
                                    string result1 = lkbtnprno.Text;
                                    var PONo1 = "PO" + result1.Substring(2);
                                    errmsg = errmsg + "Error in PONO[" + PONo + "] - Send Email- " + sendemailce.Message.ToString();

                                }
                                #endregion SendEmail code block
                            }
                            else
                            {
                                meditem = lclsservice.UpdateMSRPoStatus(objmedsupplyrequest);
                            }
                            #endregion multi order generation end
                        }
                        catch (Exception innerce)
                        {
                            LinkButton lkbtnprno = (LinkButton)row.FindControl("lkbtnprnoreview");
                            string result = lkbtnprno.Text;
                            var PONo = "PO" + result.Substring(2);
                            errmsg = errmsg + "Error in PONO[" + PONo + "] - " + innerce.Message.ToString();
                        }
                    }
                    if (errmsg != string.Empty) throw new Exception(errmsg);
                    if (meditem == "Saved Successfully")
                    {
                        mpemedsupplyReview.Show();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoSaveMsg.Replace("<<MedicalSupplyRequestPoDescription>>", "Purchase Order is generated Successfully"), true);
                        lblmessage.Text = "Purchase Order changes are updated/generated Successfully";
                        btnreviewcancel.Text = "Go back";
                        btngenerateorder.Visible = false;
                        btnorderall.Text = "Order All";
                        List<GetMSRMultipleIDs> lstrwpo = lclsservice.GetMSRMultipleIDs(Convert.ToString(ViewState["PRMatserID"])).ToList();
                        gvmedreview.DataSource = lstrwpo;
                        gvmedreview.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMedsupplyPoNorecordreview.Replace("<<MedicalSupplyRequestPoDescription>>", ""), true);
                }
                BindGrid();
                ViewState["PRMatserID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        #endregion


        public List<object> DetailsOrderReport(Int64 PRmasterID)
        {
            List<object> llstarg = new List<object>();
            List<GetMSROrderContentPO> llstreview = lclsservice.GetMSROrderContentPO(PRmasterID, defaultPage.UserId).ToList();
            rvmedicalsupplyreportPDF.ProcessingMode = ProcessingMode.Local;
            rvmedicalsupplyreportPDF.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalsupplyPDFPO.rdlc");
            ReportParameter[] p1 = new ReportParameter[1];
            p1[0] = new ReportParameter("MedicalSuppliesID", Convert.ToString(PRmasterID));
            this.rvmedicalsupplyreportPDF.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MedicalSupplyPOpdfDS", llstreview);
            rvmedicalsupplyreportPDF.LocalReport.DataSources.Clear();
            rvmedicalsupplyreportPDF.LocalReport.DataSources.Add(datasource);
            rvmedicalsupplyreportPDF.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvmedicalsupplyreportPDF.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            string FromEmail = llstreview[0].FromEmail;
            string ToEmail = llstreview[0].ToEmail;
            Int64 CorporateID = Convert.ToInt64(llstreview[0].CorporateID);
            Int64 FacilityID = Convert.ToInt64(llstreview[0].FacilityID);
            string PONo = llstreview[0].PONo;
            llstarg.Insert(0, FromEmail);
            llstarg.Insert(1, ToEmail);
            llstarg.Insert(2, bytes);
            llstarg.Insert(3, CorporateID);
            llstarg.Insert(4, FacilityID);
            llstarg.Insert(5, PONo);
            return llstarg;
        }

        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string PONo)
        {
            string Superadmin = string.Empty;
            List<GetSuperAdminDetails> lstsuperadmin = lclsservice.GetSuperAdminDetails(CorporateID, FacilityID).ToList();
            foreach (var value in lstsuperadmin)
            {
                Superadmin += "<br/>" + value.UserName + "<br/>" + value.Email + "<br/>" + value.PhoneNo;
            }
            objemail.vendorEmailcontent = string.Format("Please see the attached document for order details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br />Thank you for service <br/><br/>" + Superadmin);

            objemail.vendoremailsubject = "Purchase Order – " + PONo;
            string displayfilename = "Purchase Order – " + PONo + ".pdf";
        }
        public List<GetMedicalSupplyPoReportDetails> SearchOrderReport(string PRmasterID)
        {
            string smedmasterIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<GetMedicalSupplyPoReportDetails> llstreview = new List<GetMedicalSupplyPoReportDetails>();
            if (PRmasterID == "")
            {
                foreach (GridViewRow row in grdOrderSearch.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (smedmasterIds == string.Empty)
                            smedmasterIds = row.Cells[1].Text;
                        else
                            smedmasterIds = smedmasterIds + "," + row.Cells[1].Text;
                    }
                }

                llstreview = lclsservice.GetMedicalSupplyPoReportDetails(null, smedmasterIds, defaultPage.UserId,defaultPage.UserId).ToList();
            }
            else
            {
                llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId,defaultPage.UserId).ToList();
            }
            return llstreview;
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Int64 MedicalsupplyID = 0;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                MedicalsupplyID = Convert.ToInt64(gvrow.Cells[1].Text);
                Response.Redirect("MedicalSuppliesRequest.aspx?MedicalsupplyID=" + MedicalsupplyID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }

        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 PRmasterID;
                PRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(PRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);    
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
                mpemedsupplyReview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void imgsearchprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string MedreqID = string.Empty;
                MedreqID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetMedicalSupplyPoReportDetails> llstresult = SearchOrderReport(MedreqID);                
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MedicalSupplies" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                DataTable dtsummaryreport = ToDataTable(llstresult);
                dtsummaryreport.Columns.RemoveAt(0);
                createPDF(dtsummaryreport, path);
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public void createPDF(DataTable dataTable, string destinationPath)
        {
            Document document = new Document(PageSize.A0);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
            document.Open();

            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            float[] widths = new float[] { 2, 5f, 7f, 2, 2, 7f, 2, 2, 7f, 2, 7f, 7f, 7f, 2, 7f, 7f, 2, 2, 2 };
            //table.SetWidths(widths);
            table.SetTotalWidth(widths);
            //Set columns names in the pdf file
            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(17, 86, 86);

                table.AddCell(cell);
            }

            //Add values of DataTable in pdf file
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));
                    //Align the cell in the center
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
        }

        public void createDetailedPDF(DataTable dataTable, string destinationPath)
        {
            Document document = new Document(PageSize.A0);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
            document.Open();

            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            float[] widths = new float[] { 2, 2f, 2f, 2, 2, 2f, 2, 2, 2f, 2f, 2f, 2f, 2f, 2, 2f, 2f, 2, 2, 2, 2, 2f, 2f, 2, 2, 2f, 2, 2, 2f, 2, 2f, 2f, 2f, 2, 2f, 2f, 2, 2, 2, 2, 2, 2 };
            //table.SetWidths(widths);
            table.SetTotalWidth(widths);
            //Set columns names in the pdf file
            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(17, 86, 86);

                table.AddCell(cell);
            }

            //Add values of DataTable in pdf file
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));
                    //Align the cell in the center
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
        }

        protected void btnsearchprint_Click(object sender, EventArgs e)
        {
            try
            {
                BALMedicalSupplyRequestPo llstMSRSearch = new BALMedicalSupplyRequestPo();

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstMSRSearch.CorporateName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
                    {
                        if (lst.Selected && drpcorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    llstMSRSearch.CorporateName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstMSRSearch.FacilityName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
                    {
                        if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.FacilityName = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstMSRSearch.VendorName = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpvendorsearch.Items)
                    {
                        if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.VendorName = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatus.SelectedValue == "All")
                {
                    llstMSRSearch.Status = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpStatus.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstMSRSearch.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMSRSearch.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstMSRSearch.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                if (llstMSRSearch.Status == "0")
                    llstMSRSearch.Status = "All";
                llstMSRSearch.LoggedinBy = defaultPage.UserId;
                //  List<GetMedicalRequestPODetails> lstMSRMaster = lclsservice.GetMedicalRequestPODetails().ToList();
                List<SearchMedicalSupplyRequestPo> lstMSRMaster = lclsservice.SearchMedicalSupplyRequestPo(llstMSRSearch).ToList();
                rvMSRPoReport.ProcessingMode = ProcessingMode.Local;
                rvMSRPoReport.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalSuppliesOrderSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetMedicalSuppliesOrderSummaryDS", lstMSRMaster);
                rvMSRPoReport.LocalReport.DataSources.Clear();
                rvMSRPoReport.LocalReport.DataSources.Add(datasource);
                rvMSRPoReport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvMSRPoReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
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
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //_sessionPDFFileName = "MedicalSupplies" + guid + ".pdf";
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
                //ViewState["ReportMedSupplyID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void lkbtnpono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 PRmasterID;
                PRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lkbtnprno = (LinkButton)gvrow.FindControl("lkbtnprno");
                List<object> llstresult = DetailsOrderReport(PRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        protected void lkbtnponoreview_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 PRmasterID;
                PRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lkbtnprno = (LinkButton)gvrow.FindControl("lkbtnponoreview");
                List<object> llstresult = DetailsOrderReport(PRmasterID);
                byte[] bytes = (byte[])llstresult[2];
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
                mpemedsupplyReview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void lkbtnprno_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                String PRmasterID = String.Empty;
                PRmasterID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                LinkButton lkbtnprno = (LinkButton)gvrow.FindControl("lkbtnprno");
                string result = lkbtnprno.Text;
                var PONo = "PO" + result.Substring(2);
                PurchaseItemDetails(PRmasterID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        protected void lkbtnprnoreview_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                String PRmasterID = String.Empty;
                PRmasterID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                LinkButton lkbtnprno = (LinkButton)gvrow.FindControl("lkbtnprnoreview");
                if (lkbtnprno != null)
                    mpemedsupplyReview.Show();
                string result = lkbtnprno.Text;
                var PONo = "PO" + result.Substring(2);
                PurchaseItemDetails(PRmasterID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        public void PurchaseItemDetails(string PRmasterID)
        {
            try
            {
                List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(null, PRmasterID, defaultPage.UserId, defaultPage.UserId).ToList();
                lblrwprno.Text = llstreview[0].PRNo;
                lblmsrcorporate.Text = llstreview[0].CorporateName;
                lblmsrfacility.Text = llstreview[0].FacilityDescription;
                lblmsrvendor.Text = llstreview[0].VendorDescription;
                lblmsrorderperiod.Text = llstreview[0].OrderPeriod;
                lblmsrshipping.Text = llstreview[0].Shipping;
                lblmsrordertype.Text = llstreview[0].OrderType;
                lblmsrtimedelivery.Text = llstreview[0].TimeDelivery;
                grdmsrreview.DataSource = llstreview;
                grdmsrreview.DataBind();
                ReviewGrandTotalcalculation();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        public void ReviewGrandTotalcalculation()
        {
            try
            {
                Decimal FootTotal = 0;
                foreach (GridViewRow gvrow in grdmsrreview.Rows)
                //for (int i = 0; i <= grdmsrreview.Rows.Count - 1; i++)
                {
                    // GridViewRow gvrow = grdmsrreview.Rows[i];
                    string lbltotprice = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                    string lblreviewtotal = lbltotprice.TrimStart('$');
                    string totprice = lblreviewtotal.Trim();
                    if (totprice != "")
                        FootTotal += Convert.ToDecimal(totprice);
                }
                lblrwgrandtotal.Text = string.Format("{0:c}", FootTotal);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void imgsend_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 PRmasterID;
                PRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(PRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                MemoryStream attachstream = new MemoryStream(bytes);
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                Int64 CorporateID = Convert.ToInt64(llstresult[3].ToString());
                Int64 FacilityID = Convert.ToInt64(llstresult[4].ToString());
                string PONo = llstresult[5].ToString();
                EmailSetting(CorporateID, FacilityID, PONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoemailMessage.Replace("<<MedicalSuppliesRequestPoemailMessage>>", ""), true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }

        protected void btnreviewcancel_Click(object sender, EventArgs e)
        {
            mpemedsupplyReview.Hide();
        }

        protected void gvmedreview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus = (e.Row.FindControl("lblStatus") as Label);
                    TextBox txtremarks = (e.Row.FindControl("txtremarks") as TextBox);
                    Label lblrwaudit = (e.Row.FindControl("lblrwaudit") as Label);
                    System.Web.UI.WebControls.Image imgrwreadmoreaudit = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgrwreadmoreaudit");
                    string Status = lblStatus.Text;
                    if (Status == StatusDeny || Status == StatusHold)
                    {
                        txtremarks.Visible = true;
                    }
                    else
                    {
                        txtremarks.Visible = false;
                    }
                    //if (lblrwaudit.Text.Length > 150)
                    //{
                    //    lblrwaudit.Text = lblrwaudit.Text.Substring(0, 150) + "....";
                    //    imgrwreadmoreaudit.Visible = true;
                    //}
                    //else
                    //{
                    //    imgrwreadmoreaudit.Visible = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
            }
        }
        private void ClearDetails()
        {
            drpcorsearch.ClearSelection();
            drpfacilitysearch.ClearSelection();
            drpvendorsearch.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatus.ClearSelection();
            //drpcorsearch.SelectedIndex = 0;
            //drpfacilitysearch.SelectedIndex = 0;
            BindCorporate();
            BindFacility();
            BindVendor();
            BindLookUp("Add");
            grdOrderSearch.DataSource = null;
            grdOrderSearch.DataBind();
            ViewState["PRMatserID"] = "";
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        protected void btnrevclose_Click(object sender, EventArgs e)
        {
            gvmedreview.DataSource = null;
            gvmedreview.DataBind();
            mpemedsupplyReview.Hide();

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearDetails();
            BindGrid();
        }

        protected void lnkClearAllCorp_Click(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (System.Web.UI.WebControls.ListItem lst in drpvendorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }

        protected void lnkClearAllFac_Click(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (System.Web.UI.WebControls.ListItem lst in drpvendorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }

        protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }


            if (i == 1)
            {
                BindFacility();
                foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListCorpID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
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
                foreach (System.Web.UI.WebControls.ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListCorpID.Value = "";
                }
                BindFacility();
            }


        }

        protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }

            if (i == 1)
            {
                BindVendor();
                foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListFacID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
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
                foreach (System.Web.UI.WebControls.ListItem lst in drpfacilitysearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListFacID.Value = "";
                }
                BindVendor();
            }

        }

    }
}