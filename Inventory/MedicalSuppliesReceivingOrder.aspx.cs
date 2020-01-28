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
using System.Globalization;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<MachinePartsRequestOrder>>
'' Type      :   C# File
'' Description  :<<To add,update the Medicalsupplies Receiving Order Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
 *  03/01/2018         V1.0                Dhanasekaran.C                   New 
 *  05/31/2018         V1.0                Sairam.P                         Receiving action is changed  
 ''--------------------------------------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class MedicalSuppliesReceivingOrder : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        EmailController objemail = new EmailController();
        BALMedicalSupplyReceivingOrder objmedsupplyreceive = new BALMedicalSupplyReceivingOrder();
        string StatusReceivedPendingInvoice = Constant.StatusReceivedPendingInvoice;
        string StatusOrderedPendingReceive = Constant.StatusOrderedPendingReceive;
        string StatusBackOrder = Constant.StatusBackOrder;
        string StatusVoidOrder = Constant.StatusVoidOrder;
        string StatusClosed = Constant.StatusClosed;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        string Actionvoid = Constant.Actionvoid;
        string Actionclosed = Constant.Actionclosed;
        string ActionPartial = Constant.ActionPartial;
        string Statusnonuser = Constant.Statusnonuser;
        string Statususer = Constant.Statususer;
        private string _sessionPDFFileName;
        Int32 POhdnrowcount = 0;
        Int32 ROhdnrowcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.grdReciveSearch);
            scriptManager.RegisterPostBackControl(this.btnPrintAll);
            if (!IsPostBack)
            {
                if (defaultPage != null)
                {
                    hdnuserrole.Value = Convert.ToString(defaultPage.RoleID);
                    lclsservice.SyncMedicalSuppliesReceivingorder();
                    BindCorporate();
                    //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                    BindFacility();
                    //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                    BindVendor();
                    BindStatus("Add");
                    //drpcorsearch.SelectedIndex = 0;
                    //drpfacilitysearch.SelectedIndex = 0;
                    BindGrid();
                    if (defaultPage.Rec_MedicalSuppliesPage_Edit == false && defaultPage.Rec_MedicalSuppliesPage_View == true)
                    {
                        btnsave.Visible = false;
                    }
                    if (defaultPage.Rec_MedicalSuppliesPage_Edit == false && defaultPage.Rec_MedicalSuppliesPage_View == false)
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

        #region Bind SyncMedicalSuppliesReceivingorder
        public void SyncMedicalSuppliesReceivingorder()
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                lst.Value = "All";
                lst.Text = "All";
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsservice.GetCorporateMaster().ToList();
                    drpcorsearch.DataSource = lstcrop;
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();
                    drpcorsearch.Items.Insert(0, lst);
                    drpcorsearch.SelectedIndex = 0;
                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();
                    drpcorsearch.Items.Insert(0, lst);
                    drpcorsearch.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion

        #region Bind GetList

        public void BindReason(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("MedicalSuppliesReceiving", "Reason", mode).ToList();
                ddlreason.DataSource = lstLookUp;
                ddlreason.DataTextField = "InvenValue";
                ddlreason.DataValueField = "InvenValue";
                ddlreason.DataBind();
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                ddlreason.Items.Insert(0, lst);
                ddlreason.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }

        }

        public void BindReceivingAction(string mode, string type)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("MedicalSuppliesReceiving", "Action", mode).ToList();
                if (type == "TYPE1")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == Actionclosed).ToList();
                }
                else if (type == "TYPE2")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == ActionPartial).ToList();
                }
                else if (type == "TYPE3")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == Actionvoid).ToList();
                }
                //ddlreceivingAct.DataSource = lstLookUp;
                ddlreceivingAct.DataTextField = "InvenValue";
                ddlreceivingAct.DataValueField = "InvenValue";
                ddlreceivingAct.DataBind();
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                lst.Value = "0";
                lst.Text = "--Select Action--";
                ddlreceivingAct.Items.Insert(0, lst);
                ddlreceivingAct.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }

        }

        //public void BindAction(string mode)
        //{
        //    try
        //    {
        //        List<GetList> lstLookUp = new List<GetList>();
        //        lstLookUp = lclsservice.GetList("MedicalSuppliesReceiving", "Action", mode).ToList();
        //        // Search Status Drop Down
        //        ddlreceivingAct.DataSource = lstLookUp;
        //        ddlreceivingAct.DataTextField = "InvenValue";
        //        ddlreceivingAct.DataValueField = "InvenValue";
        //        ddlreceivingAct.DataBind();
        //        ddlreceivingAct.Items.Insert(0, "--Select--");
        //    }
        //    catch (Exception ex)
        //    {
         //         ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
        //    }

        //}
        #endregion


        //protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindFacility();
        //}
        //protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindVendor();
        //}


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
                DivMedicalReceive.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            DivMedicalReceive.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                DivMedicalReceive.Attributes["class"] = "Upopacity";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion




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
                DivMedicalReceive.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }


        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            DivMedicalReceive.Attributes["class"] = "mypanel-body";
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
                    DivMedicalReceive.Attributes["class"] = "Upopacity";
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


        #region Bind Facility Values
        private void BindFacility()
        {
            try
            {
                //System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
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
                    //    //if (drpcorsearch.SelectedValue != "All")
                    //    //{
                    //    drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    //    drpfacilitysearch.DataTextField = "FacilityDescription";
                    //    drpfacilitysearch.DataValueField = "FacilityID";
                    //    drpfacilitysearch.DataBind();
                    //    //drpfacilitysearch.Items.Insert(0, lst);
                    //    //drpfacilitysearch.SelectedIndex = 0;
                    //    //}
                    //    //else
                    //    //{
                    //    //    drpfacilitysearch.SelectedIndex = 0;
                    //    //}
                    //}
                    //else
                    //{
                    //    //if (drpcorsearch.SelectedValue != "All")
                    //    //{
                    //    drpfacilitysearch.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == drpcorsearch.SelectedItem.Text).ToList();
                    //    drpfacilitysearch.DataTextField = "FacilityName";
                    //    drpfacilitysearch.DataValueField = "FacilityID";
                    //    drpfacilitysearch.DataBind();
                    //    //drpfacilitysearch.Items.Insert(0, lst);
                    //    //drpfacilitysearch.SelectedIndex = 0;
                    //    //}
                    //    //else
                    //    //{
                    //    //    drpfacilitysearch.SelectedIndex = 0;
                    //    //}
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion



        #region Bind Vendor Values
        public void BindVendor()
        {
            try
            {
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
                    //System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        #endregion

        public void BindStatus(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("MedicalSuppliesReceiving", "Status", mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                //System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatussearch.Items.Insert(0, lst);
                if (defaultPage.RoleID == 1)
                {
                    drpStatussearch.Items.FindByText(StatusReceivedPendingInvoice).Selected = true;
                }
                else
                {
                    drpStatussearch.Items.FindByText(StatusOrderedPendingReceive).Selected = true;
                    drpStatussearch.Items.FindByText(StatusBackOrder).Selected = true;
                }
                // drpStatussearch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #region Bind MedicalSupplies Receiving
        public void BindGrid()
        {
            try
            {
                BALMedicalSupplyReceivingOrder llstMSRSearch = new BALMedicalSupplyReceivingOrder();

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
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstMSRSearch.Status = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpStatussearch.Items)
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
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
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
                llstMSRSearch.LoggedinBy = defaultPage.UserId;
                List<SearchMedicalSuppliesReceiving> lstMSRMaster = lclsservice.SearchMedicalSuppliesReceiving(llstMSRSearch).ToList();
                grdReciveSearch.DataSource = lstMSRMaster;
                grdReciveSearch.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion

        #region Bind MedicalSupplies Receiving Details
        public void BindDetailGrid(Int64 PRmarsterID)
        {
            try
            {
                BALMedicalSupplyReceivingOrder llstMSRDeatilItem = new BALMedicalSupplyReceivingOrder();
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                llstMSRDeatilItem.MedicalSuppliesReceivingMasterID = PRmarsterID;
                llstMSRDeatilItem.SearchFilters = "";
                llstMSRDeatilItem.LockTimeOut = LockTimeOut;
                llstMSRDeatilItem.LoggedinBy = defaultPage.UserId;
                List<BindMedicalsupplyReceivingDetail> lstMSRMaster = lclsservice.BindMedicalsupplyReceivingDetail(llstMSRDeatilItem).ToList();
                grdmsritemedit.DataSource = lstMSRMaster;
                grdmsritemedit.DataBind();
                //lblrcount4.Text = "No of records : " + lstMSRMaster.Where(b=>b.OrderQty != 0).Count().ToString();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcount4.Text = "No of records : " + (lstMSRMaster.Count - a);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion

        #region Bind MedicalSupplies Receiving Details for Review
        public void BindReviewDetailGrid(Int64 PRmarsterID)
        {
            try
            {
                grdmsrreviewdeatils.Style.Add("display", "block");
                hdnreviewflag.Value = "1";
                grdmsritemedit.Style.Add("display", "none");
                BALMedicalSupplyReceivingOrder llstMSRDeatilItem = new BALMedicalSupplyReceivingOrder();
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                llstMSRDeatilItem.MedicalSuppliesReceivingMasterID = PRmarsterID;
                llstMSRDeatilItem.SearchFilters = "";
                llstMSRDeatilItem.LockTimeOut = LockTimeOut;
                llstMSRDeatilItem.LoggedinBy = defaultPage.UserId;
                List<BindMedicalsupplyReceivingDetail> lstMSRMaster = lclsservice.BindMedicalsupplyReceivingDetail(llstMSRDeatilItem).ToList();
                grdmsrreviewdeatils.DataSource = lstMSRMaster;
                grdmsrreviewdeatils.DataBind();
                //lblrcount5.Text = "No of records : " + lstMSRMaster.Where(b => b.OrderQty != 0).Count().ToString();
                Int32 a = Convert.ToInt32(RoHddgridcount.Value);
                lblrcount5.Text = "No of records : " + (lstMSRMaster.Count - a);
                TextBox txtshipcost = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txtshipcost");
                TextBox txttax = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txttax");
                TextBox txtTotalcost = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txtTotalcost");
                if (Convert.ToString(lstMSRMaster[0].Shipping) != "")
                    txtshipcost.Text = Convert.ToString(string.Format("{0:F2}", lstMSRMaster[0].Shipping));
                if (Convert.ToString(lstMSRMaster[0].Tax) != "")
                    txttax.Text = Convert.ToString(string.Format("{0:F2}", lstMSRMaster[0].Tax));
                if (Convert.ToString(lstMSRMaster[0].TotalCost) != "")
                    txtTotalcost.Text = Convert.ToString(string.Format("{0:F2}", lstMSRMaster[0].TotalCost));
                if (lstMSRMaster[0].PackingSlipNo != "")
                    packingsilpno.Text = lstMSRMaster[0].PackingSlipNo;
                if (lstMSRMaster[0].PackingDate != null)
                    txtpackingslipdate.Text = Convert.ToDateTime(lstMSRMaster[0].PackingDate).ToString("MM/dd/yyyy");
                if (lstMSRMaster[0].ReceivedDate != null)
                    txtreceiveddate.Text = Convert.ToDateTime(lstMSRMaster[0].ReceivedDate).ToString("MM/dd/yyyy");
                if (lstMSRMaster[0].InvoiceDate != null)
                    txtinvoicedate.Text = Convert.ToDateTime(lstMSRMaster[0].InvoiceDate).ToString("MM/dd/yyyy");
                if (lstMSRMaster[0].InvoiceNo != "")
                    txtinvoiceno.Text = lstMSRMaster[0].InvoiceNo;

                ddlreceivingAct.SelectedValue = lstMSRMaster[0].ReceivingAction;
                ddlreason.SelectedValue = lstMSRMaster[0].Reason;
                if (lstMSRMaster[0].OtherReason != "" && lstMSRMaster[0].OtherReason == "")
                {
                    otherid.Attributes.Add("display","block");
                    txtreason.Text = lstMSRMaster[0].OtherReason;
                }
                else
                {
                    otherid.Attributes.Add("display", "none");
                }
                btnsave.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void grdReciveSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    Label lblAudit = (Label)e.Row.FindControl("lblaudit");
                    System.Web.UI.WebControls.Image imgreadmore = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgreadmore");
                    System.Web.UI.WebControls.Image imgreadmoreaudit = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgreadmoreaudit");
                    System.Web.UI.WebControls.Image imgsummaryprint = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgsummaryprint"); 
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
                    LinkButton lkbtnpono = (LinkButton)e.Row.FindControl("lkbtnpono");
                    LinkButton lkbtnrono = (LinkButton)e.Row.FindControl("lkbtnrono");
                    string status = e.Row.Cells[16].Text;
                    if (defaultPage.RoleID == 1)
                    {
                        if (lkbtnrono.Text == "")
                        {
                            lkbtnpono.Enabled = true;
                            imgsummaryprint.Enabled = false;
                        }
                        else
                        {
                            lkbtnpono.Enabled = false;
                            imgsummaryprint.Enabled = true;
                        }
                    }
                    else
                    {
                        if (lkbtnrono.Text != "")
                        {
                            lkbtnrono.Enabled = false;
                            imgsummaryprint.Enabled = true;
                        }
                        else
                        {
                            lkbtnrono.Enabled = true;
                            imgsummaryprint.Enabled = false;
                        }
                    }
                    if (status == Actionvoid || status== ActionPartial)
                    {
                        lkbtnpono.Enabled = false;
                        lkbtnrono.Enabled = false;
                    }

                    //if (lkbtnrono.Text == "")
                    //{
                    //    imgsummaryprint.Enabled = false;
                    //}
                    //else
                    //{
                    //    imgsummaryprint.Enabled = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        public string GetReceivingActionStatus()
        {
            decimal orterqty = 0;
            decimal receivedoqty = 0;
            decimal Total = 0;
            decimal orterqtytotal = 0;
            decimal receivedoqtytotal = 0;
            if (hdnreviewflag.Value != "0")
            {
                foreach (GridViewRow row in grdmsrreviewdeatils.Rows)
                {
                    Label lbloqty = (Label)row.FindControl("lbloqty");
                    TextBox txtreceivedoqty = (TextBox)row.FindControl("txtreceivedoqty");

                    orterqty = Convert.ToDecimal(lbloqty.Text);
                    if (txtreceivedoqty.Text != "")
                        receivedoqty = Convert.ToDecimal(txtreceivedoqty.Text);
                    orterqtytotal += orterqty;
                    if (txtreceivedoqty.Text != "")
                        receivedoqtytotal += receivedoqty;
                }
            }
            else
            {
                foreach (GridViewRow row in grdmsritemedit.Rows)
                {
                    Label lbloqty = (Label)row.FindControl("lbloqty");
                    TextBox txtreceivedoqty = (TextBox)row.FindControl("txtreceivedoqty");

                    orterqty = Convert.ToDecimal(lbloqty.Text);
                    if (txtreceivedoqty.Text != "")
                        receivedoqty = Convert.ToDecimal(txtreceivedoqty.Text);
                    orterqtytotal += orterqty;
                    if (txtreceivedoqty.Text != "")
                        receivedoqtytotal += receivedoqty;
                }
            }
            string check = string.Empty;
            Total = orterqtytotal - receivedoqtytotal;
            if (Total == 0)
            {
                check = "TYPE1";
            }
            else
            {
                check = "TYPE2";
            }


            return check;
        }
        protected void lkbtnpono_Click(object sender, EventArgs e)
        {  
            mpemsrreviewrecive.Show();
            lblerrormsg.Text = "";
            ddlreceivingAct.Enabled = true;
            lblrcount4.Visible = true;
            lblrcount5.Visible = false;
            LinkButton btndetails = sender as LinkButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            LinkButton lkbtnpono = (LinkButton)gvrow.FindControl("lkbtnpono");
            LinkButton lkbtnrono = (LinkButton)gvrow.FindControl("lkbtnrono");
            Int64 MedicalSuppliesReceivingMasterID = 0;
            MedicalSuppliesReceivingMasterID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
            ViewState["MedicalSuppliesReceivingMasterID"] = MedicalSuppliesReceivingMasterID;
            ViewState["PRMasterID"] = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
            ViewState["PONo"] = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
            ViewState["MedicalSuppliesRequestOrderID"] = Convert.ToInt64(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", ""));
            ViewState["CorporateID"] = Convert.ToString(gvrow.Cells[6].Text.Trim().Replace("&nbsp;", ""));
            ViewState["FacilityID"] = Convert.ToString(gvrow.Cells[7].Text.Trim().Replace("&nbsp;", ""));
            string status = Convert.ToString(gvrow.Cells[16].Text.Trim().Replace("&nbsp;", ""));
            BindReason("Add");
            BindDetailGrid(MedicalSuppliesReceivingMasterID);
            BindReceivingAction("Add", GetReceivingActionStatus());
            //otherid.Visible = false;
            ClearDetails();
            lblmprreview.Text = lkbtnpono.Text;
            if (defaultPage.RoleID == 1)
            {
                if (defaultPage.RoleID == 1 && status == StatusBackOrder && lkbtnrono.Text == "")
                {
                    BindReceivingAction("Add", "TYPE2");
                }
                else if (defaultPage.RoleID == 1 && status == StatusOrderedPendingReceive && lkbtnrono.Text == "")
                {
                    BindReceivingAction("Add", "TYPE3");
                }
                packingsilpno.Enabled = false;
                txtpackingslipdate.Enabled = false;
                txtreceiveddate.Enabled = false;
                Reqpacksilpno.Enabled = false;
                Reqpacking.Enabled = false;
                Reqrecedate.Enabled = false;
                regexrecdate.Enabled = false;
                //superadmin
                txtinvoiceno.Enabled = false;
                Reqinvoiceno.Enabled = false;
                txtinvoicedate.Enabled = false;
                RegularExpressionValidator1.Enabled = false;
                Reqfldin.Enabled = false;               
                grdmsritemedit.Enabled = false;
                grdmsrreviewdeatils.Style.Add("display", "none");
                hdnreviewflag.Value = "0";
                grdmsritemedit.Style.Add("display", "block");
                Reqdrprecaction.Enabled = true;
                ddlreceivingAct.Enabled = true;
                ddlreason.Enabled = true;
                Reqreason.Enabled = true;
                ddlreceivingAct.ClearSelection();
                if (status == StatusBackOrder)
                    ddlreceivingAct.Items.FindByValue(ActionPartial).Selected = true;
                else
                    ddlreceivingAct.Items.FindByValue(Actionvoid).Selected = true;
                ddlreceivingAct.Enabled = false;

            }
            else if (defaultPage.RoleID != 1 && lkbtnrono.Text != "")
            {
                nonsuperadmin();
                BindReviewDetailGrid(MedicalSuppliesReceivingMasterID);
            }
            else
            {
                nonsuperadmin();
            }
        }
        public void BindNonsuperItemdetails(Int64 MedicalSuppliesReceivingMasterID)
        {
            try
            {
                grdmsrreviewdeatils.Style.Add("display", "block");
                grdmsritemedit.Style.Add("display", "none");
                BALMedicalSupplyReceivingOrder llstMSRDeatilItem = new BALMedicalSupplyReceivingOrder();
                llstMSRDeatilItem.MedicalSuppliesReceivingMasterID = MedicalSuppliesReceivingMasterID;
                llstMSRDeatilItem.LoggedinBy = defaultPage.UserId;
                llstMSRDeatilItem.SearchFilters = "";
                List<BindMedicalsupplyReceivingDetail> lstMSRMaster = lclsservice.BindMedicalsupplyReceivingDetail(llstMSRDeatilItem).ToList();
                grdmsrreviewdeatils.DataSource = lstMSRMaster;
                grdmsrreviewdeatils.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        public void nonsuperadmin()
        {
            txtinvoiceno.Enabled = false;
            Reqinvoiceno.Enabled = false;
            txtinvoicedate.Enabled = false;
            RegularExpressionValidator1.Enabled = false;
            Reqfldin.Enabled = false;
            Reqdrprecaction.Enabled = false;
            ddlreceivingAct.Enabled = false;
            ddlreason.Enabled = false;
            Reqreason.Enabled = false;
            grdmsrreviewdeatils.Style.Add("display", "none");
            hdnreviewflag.Value = "0";
            grdmsritemedit.Style.Add("display", "block");

        }
        public void RemoveAdminRequiredField()
        {
            Reqinvoiceno.Enabled = false;
            Reqfldin.Enabled = false;
            RegularExpressionValidator1.Enabled = false;
            Reqdrprecaction.Enabled = false;
            Reqreason.Enabled = false;
        }
        public void SetAdminRequiredField()
        {
            //Reqinvoiceno.Enabled = false;
            //Reqfldin.Enabled = false;
            //RegularExpressionValidator1.Enabled = false;
            Reqdrprecaction.Enabled = true;
            Reqreason.Enabled = true;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearDetails();
        }
        private void ClearDetails()
        {
            packingsilpno.Text = "";
            txtpackingslipdate.Text = "";
            txtreceiveddate.Text = "";
            txtinvoiceno.Text = "";
            txtinvoicedate.Text = "";
            ddlreceivingAct.SelectedIndex = 0;
            ddlreason.SelectedIndex = 0;
            txtreason.Text = "";
            grdmsritemedit.Enabled = true;
            btnsave.Enabled = true;

        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            lclsservice.SyncMedicalSuppliesReceivingorder();
            BindGrid();
        }

        #region  Set Flag Type Function

        public void SetFlagType()
        {
            decimal orterqty = 0;
            decimal receivedoqty = 0;
            decimal Total = 0;
            decimal orterqtytotal = 0;
            decimal receivedoqtytotal = 0;
            if (Convert.ToString(ViewState["Type"]) != "TYPE3")
            {
                foreach (GridViewRow row in grdmsritemedit.Rows)
                {
                    Label lbloqty = (Label)row.FindControl("lbloqty");
                    TextBox txtreceivedoqty = (TextBox)row.FindControl("txtreceivedoqty");

                    orterqty = Convert.ToDecimal(lbloqty.Text);
                    if (txtreceivedoqty.Text != "")
                        receivedoqty = Convert.ToDecimal(txtreceivedoqty.Text);
                    orterqtytotal += orterqty;
                    if (txtreceivedoqty.Text != "")
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
            else
            {
                ViewState["Type"] = Convert.ToString("TYPE3");
            }
        }

        #endregion
      
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 InertCodeID = 0;
                string ErrorList = string.Empty;
                string meditem = string.Empty;
                int rowcount = grdmsritemedit.Rows.Count;
                objmedsupplyreceive.MedicalSuppliesReceivingMasterID = Convert.ToInt64(ViewState["MedicalSuppliesReceivingMasterID"]);
                objmedsupplyreceive.PRMasterID = Convert.ToInt64(ViewState["PRMasterID"]);
                objmedsupplyreceive.PONo = Convert.ToString(ViewState["PONo"]);
                objmedsupplyreceive.MedicalSuppliesRequestOrderID = Convert.ToInt64(ViewState["MedicalSuppliesRequestOrderID"]);
                if (packingsilpno.Text != "")
                    objmedsupplyreceive.PackingSlipNo = packingsilpno.Text;
                if (txtpackingslipdate.Text != "")
                    objmedsupplyreceive.PackingSlipDate = Convert.ToDateTime(txtpackingslipdate.Text);
                if (txtreceiveddate.Text != "")
                    objmedsupplyreceive.ReceivedDate = Convert.ToDateTime(txtreceiveddate.Text);
                if (txtinvoiceno.Text != "")
                    objmedsupplyreceive.InvoiceNo = Convert.ToString(txtinvoiceno.Text);
                if (txtinvoicedate.Text != "")
                    objmedsupplyreceive.InvoiceDate = Convert.ToDateTime(txtinvoicedate.Text);
                if (ddlreceivingAct.SelectedValue != "0")
                    objmedsupplyreceive.ReceivingAction = ddlreceivingAct.SelectedValue;
                if (ddlreason.SelectedValue != "0")
                    objmedsupplyreceive.Reason = ddlreason.SelectedValue;
                ViewState["Reason"] = objmedsupplyreceive.Reason;
                if (txtreason.Text != "")
                    objmedsupplyreceive.OtherReason = txtreason.Text;
                SetFlagType();
                objmedsupplyreceive.Type = Convert.ToString(ViewState["Type"]);
                if (defaultPage.RoleID == 1)
                {
                    objmedsupplyreceive.Type = "TYPE3";
                }
                objmedsupplyreceive.CreatedBy = defaultPage.UserId;
                objmedsupplyreceive.LoggedinBy = defaultPage.UserId;
                objmedsupplyreceive.FinalStatus = ddlreceivingAct.SelectedValue;
                if (hdnreviewflag.Value == "1")
                {
                    TextBox txtshipcost = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txtshipcost");
                    TextBox txttax = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txttax");
                    TextBox txtTotalcost = (TextBox)grdmsrreviewdeatils.FooterRow.FindControl("txtTotalcost");
                    if (txtshipcost.Text != "")
                        objmedsupplyreceive.ShippingCost = Convert.ToDecimal(txtshipcost.Text);
                    if (txttax.Text != "")
                        objmedsupplyreceive.Tax = Convert.ToDecimal(txttax.Text);
                    if (txtTotalcost.Text != "")
                        objmedsupplyreceive.TotalCost = Convert.ToDecimal(txtTotalcost.Text);
                }
                else
                {
                    TextBox txtTotalcost = (TextBox)grdmsritemedit.FooterRow.FindControl("txtTotalcost");
                    if (txtTotalcost.Text != "")
                        objmedsupplyreceive.TotalCost = Convert.ToDecimal(txtTotalcost.Text);
                }
                List<ValidateMedicalSuppliesOrder> llstmedicaldetail = lclsservice.ValidateMedicalSuppliesOrder(objmedsupplyreceive.PRMasterID).ToList();
                if(llstmedicaldetail.Count > 0 && hdnreviewflag.Value == "1")
                {
                    string Order = Convert.ToDateTime(llstmedicaldetail[0].OrderDate).ToString("MM/dd/yyyy");
                    string Invoice = Convert.ToDateTime(objmedsupplyreceive.InvoiceDate).ToString("MM/dd/yyyy");

                    DateTime Orderdt = Convert.ToDateTime(Order);
                    DateTime Invoicedt = Convert.ToDateTime(Invoice);


                    if (Orderdt > Invoicedt)
                    {
                        ErrorList = "Invoice date should be greater than purchase Order date";
                    }
                }
               if(ErrorList == "" )
                {
                    List<UpdateMSRReceivingMaster> lstMSRMaster = lclsservice.UpdateMSRReceivingMaster(objmedsupplyreceive).ToList();
                    InertCodeID = Convert.ToInt64(lstMSRMaster[0].INSERTRECORDID);
                    if (hdnreviewflag.Value != "1")
                    {
                        foreach (GridViewRow row in grdmsritemedit.Rows)
                        {
                            Label lblMedicalSuppliesReceivingDetailsID = (Label)row.FindControl("lblMedicalSuppliesReceivingDetailsID");
                            objmedsupplyreceive.MedicalSuppliesReceivingDetailsID = Convert.ToInt64(lblMedicalSuppliesReceivingDetailsID.Text);
                            Label lbloqty = (Label)row.FindControl("lbloqty");
                            Label lblPrice = (Label)row.FindControl("lblPrice");
                            Label lblbalanceqty = (Label)row.FindControl("lblbalanceqty");
                            TextBox txtreceivedoqty = (TextBox)row.FindControl("txtreceivedoqty");
                            Label lbltotprice = (Label)row.FindControl("lbltotprice");

                            Int64 oqty = 0;
                            oqty = Convert.ToInt64(lbloqty.Text);
                            Int64 receivedoqty = 0;
                            if (txtreceivedoqty.Text != "")
                                receivedoqty = Convert.ToInt64(txtreceivedoqty.Text);
                            decimal price = 0;
                            price = Convert.ToDecimal(lblPrice.Text);
                            TextBox txtcomments = (TextBox)row.FindControl("txtcomments");
                            Int32 BalQty = (Convert.ToInt32(oqty) - Convert.ToInt32(receivedoqty));
                            objmedsupplyreceive.INSERTRECORDID = InertCodeID;
                            objmedsupplyreceive.BalanceQty = BalQty;
                            if (txtreceivedoqty.Text != "")
                            {
                                objmedsupplyreceive.ReceivedQty = Convert.ToInt32(txtreceivedoqty.Text);
                            }
                            else
                            {
                                objmedsupplyreceive.ReceivedQty = 0;
                            }
                            if (txtcomments.Text != "")
                            {
                                objmedsupplyreceive.Comments = txtcomments.Text;
                            }
                            else
                            {
                                objmedsupplyreceive.Comments = null;
                            }
                            decimal A = Convert.ToDecimal(price) * Convert.ToInt32(receivedoqty);
                            objmedsupplyreceive.TotalPrice = A;
                            meditem = lclsservice.UpdateMSRReceivingDetails(objmedsupplyreceive);
                        }
                    }
                    else
                    {
                        foreach (GridViewRow row in grdmsrreviewdeatils.Rows)
                        {
                            Label lblMedicalSuppliesReceivingDetailsID = (Label)row.FindControl("lblMedicalSuppliesReceivingDetailsID");
                            objmedsupplyreceive.MedicalSuppliesReceivingDetailsID = Convert.ToInt64(lblMedicalSuppliesReceivingDetailsID.Text);
                            meditem = lclsservice.UpdateMSRReceivingDetails(objmedsupplyreceive);
                        }
                    }
                    if (objmedsupplyreceive.FinalStatus == StatusVoidOrder)
                    {
                        SendEmail();
                        BindGrid();
                        ViewState["Type"] = "";
                        ClearDetails();
                    }
                    if (meditem == "Saved Successfully" && objmedsupplyreceive.FinalStatus != StatusVoidOrder)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingSaveMsg.Replace("<<MedicalSupplyReceivingDescription>>", ""), true);
                        BindGrid();
                        ViewState["Type"] = "";
                        ClearDetails();
                    }
                }
                else
                {
                    mpemsrreviewrecive.Show();
                    lblerrormsg.Text = Constant.WarningValidInvoiceDateMessage;
                    txtinvoicedate.Text = "";
                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        public void SendEmail()
        {
            try
            {
                Int64 PRmasterID = Convert.ToInt64(ViewState["PRMasterID"]);
                List<object> llstresult = DetailsOrderReport(PRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                objmedsupplyreceive.OrderContent = bytes;
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(ViewState["CorporateID"]);
                Int64 FacilityID = Convert.ToInt64(ViewState["FacilityID"]);
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                var PONo = lblmprreview.Text;
                EmailSetting(CorporateID, FacilityID, PONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingemailMessage.Replace("<<MedicalSuppliesReceivingemailMessage>>", ""), true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string PONo)
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
                objemail.vendorEmailcontent = string.Format("Due to " + reason + " " + PONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + Superadmin);
            }
            else
            {
                objemail.vendorEmailcontent = string.Format("We are cancelling " + PONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + Superadmin);
            }
            objemail.vendoremailsubject = "Purchase Order – " + PONo;
            string displayfilename = "Purchase Order – " + PONo + ".pdf";
        }

        public List<object> DetailsOrderReport(Int64 PRmasterID)
        {
            List<object> llstarg = new List<object>();
            List<GetMSROrderContentPO> llstreview = lclsservice.GetMSROrderContentPO(PRmasterID, defaultPage.UserId).ToList();
            rvmedicalsupplyreportVoidPDF.ProcessingMode = ProcessingMode.Local;
            rvmedicalsupplyreportVoidPDF.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalsupplyReceiveVoidPDF.rdlc");
            ReportParameter[] p1 = new ReportParameter[1];
            p1[0] = new ReportParameter("MedicalSuppliesID", Convert.ToString(PRmasterID));
            this.rvmedicalsupplyreportVoidPDF.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("MedicalSupplyReceiveDS", llstreview);
            rvmedicalsupplyreportVoidPDF.LocalReport.DataSources.Clear();
            rvmedicalsupplyreportVoidPDF.LocalReport.DataSources.Add(datasource);
            rvmedicalsupplyreportVoidPDF.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvmedicalsupplyreportVoidPDF.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
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

        protected void ddlreason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlreason.SelectedItem.Text == "Other")
            {
                otherid.Attributes.Add("display", "block");
                //otherid.Visible = true;
            }
            else
            {
                otherid.Attributes.Add("display", "none");
                //otherid.Visible = false;
            }
            mpemsrreviewrecive.Show();
        }

        protected void lkbtnrono_Click(object sender, EventArgs e)
        {
            try
            {
                if (defaultPage.RoleID == 1)
                {
                    mpemsrreviewrecive.Show();
                    lblerrormsg.Text = "";
                    AllControlsEnable();
                    LinkButton btndetails = sender as LinkButton;
                    GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                    LinkButton lkbtnpono = (LinkButton)gvrow.FindControl("lkbtnpono");
                    LinkButton lkbtnrono = (LinkButton)gvrow.FindControl("lkbtnrono");                   
                    Int64 MedicalSuppliesReceivingMasterID = 0;
                    MedicalSuppliesReceivingMasterID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
                    string Status = string.Empty;
                    Status = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");                    
                    ViewState["MedicalSuppliesReceivingMasterID"] = MedicalSuppliesReceivingMasterID;
                    ViewState["PRMasterID"] = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                    ViewState["PONo"] = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                    ViewState["MedicalSuppliesRequestOrderID"] = Convert.ToInt64(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", ""));
                    BindReason("Add");
                    BindReceivingAction("Add", "TYPE1");                    
                    BindReviewDetailGrid(MedicalSuppliesReceivingMasterID);
                    lblmprreview.Text = lkbtnrono.Text;
                    btnsave.Enabled = true;
                    //nonsuperadmin start 
                    Reqpacksilpno.Enabled = false;
                    Reqpacking.Enabled = false;
                    Reqrecedate.Enabled = false;
                    regexrecdate.Enabled = false;
                    packingsilpno.Enabled = false;
                    txtpackingslipdate.Enabled = false;
                    txtreceiveddate.Enabled = false;
                    lblrcount4.Visible = false;
                    lblrcount5.Visible = true;
                    //nonsuperadmin end
                    //  SetAdminRequiredField();
                    ddlreason.Enabled = false;
                    Reqreason.Enabled = false;
                    if (Status == Actionclosed)
                    {
                        btnsave.Enabled = false;
                        if (ddlreceivingAct != null)
                            ddlreceivingAct.Items.FindByValue(Actionclosed).Selected = true;
                        txtinvoiceno.Enabled = false;
                        Reqinvoiceno.Enabled = false;
                        txtinvoicedate.Enabled = false;
                        RegularExpressionValidator1.Enabled = false;
                        Reqfldin.Enabled = false;
                        Reqdrprecaction.Enabled = false;
                        ddlreceivingAct.Enabled = false;
                        ddlreason.Enabled = false;
                        Reqreason.Enabled = false;

                    }
                    ViewState["Type"] = Convert.ToString("TYPE3");
                    if (ddlreceivingAct != null)
                        ddlreceivingAct.Items.FindByValue(Actionclosed).Selected = true;
                    ddlreceivingAct.Enabled = false;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }

        }
        private void AllControlsEnable()
        {
            btnsave.Enabled = true;    
            txtinvoiceno.Enabled = true;
            Reqinvoiceno.Enabled = true;
            txtinvoicedate.Enabled = true;
            RegularExpressionValidator1.Enabled = true;
            Reqfldin.Enabled = true;
            Reqdrprecaction.Enabled = true;
            ddlreceivingAct.Enabled = true;
            ddlreason.Enabled = true;
            Reqreason.Enabled = true;
        }

        protected void grdmsritemedit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbloqty = (Label)e.Row.FindControl("lbloqty");
                    TextBox txtreceivedoqty = (TextBox)e.Row.FindControl("txtreceivedoqty");
                    TextBox txtcomments = (TextBox)e.Row.FindControl("txtcomments");
                    if (lbloqty.Text == "0")
                    {
                        e.Row.Style.Add("display","none");
                        POhdnrowcount = POhdnrowcount + 1;
                    }
                }

                HddGridCount.Value = Convert.ToString(POhdnrowcount);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        protected void imgsummaryprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string PRONo = string.Empty;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lkbtnrono = (LinkButton)gvrow.FindControl("lkbtnrono");
                Int64 PRMasterID = 0;
                PRMasterID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                objmedsupplyreceive.PRMasterID = PRMasterID;
                objmedsupplyreceive.LoggedinBy = defaultPage.UserId;
                objmedsupplyreceive.SearchFilters = "";
                List<GetMSRReceivingsummaryReport> llstreview = lclsservice.GetMSRReceivingsummaryReport(objmedsupplyreceive).ToList();
                rvsummaryreport.ProcessingMode = ProcessingMode.Local;
                rvsummaryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalsuppliesReceivingSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("MSRSummaryDS", llstreview);
                rvsummaryreport.LocalReport.DataSources.Clear();
                rvsummaryreport.LocalReport.DataSources.Add(datasource);
                rvsummaryreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                DataTable dtsummaryreport = ToDataTable(llstreview);

                //byte[] bytes = rvsummaryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MSReceivingOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                dtsummaryreport.Columns.RemoveAt(0);
                createPDF(dtsummaryreport, path);
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
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
                        System.IO.File.Delete(path);
                        Response.End();
                    }
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        protected void imgdetailprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string PRONo = string.Empty;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lkbtnrono = (LinkButton)gvrow.FindControl("lkbtnrono");
                Int64 PRMasterID = 0;
                PRMasterID = Convert.ToInt64(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""));
                Int64 MedicalSupReceiveMasterID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
                objmedsupplyreceive.PRMasterID = PRMasterID;
                objmedsupplyreceive.MedicalSuppliesReceivingMasterID = MedicalSupReceiveMasterID;
                objmedsupplyreceive.LoggedinBy = defaultPage.UserId;
                objmedsupplyreceive.SearchFilters = "";
                List<BindMedicalSupplyDetailsReport> llstreview = lclsservice.BindMedicalSupplyDetailsReport(objmedsupplyreceive).ToList();
                //List<BindMedicalSupplyDetailsSubReport> llstsubreport = lclsservice.BindMedicalSupplyDetailsSubReport(objmedsupplyreceive).ToList();
                rvdetailreport.ProcessingMode = ProcessingMode.Local;
                rvdetailreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalSupplyReceivingDetail.rdlc");
                ReportDataSource datasource = new ReportDataSource("MSReceivingDetailsDataset", llstreview);
                //ReportDataSource datasourcesub = new ReportDataSource("MSReceivingDetailSubReportDataSet", llstsubreport);
                rvdetailreport.LocalReport.DataSources.Clear();
                rvdetailreport.LocalReport.DataSources.Add(datasource);
                //rvdetailreport.LocalReport.DataSources.Add(datasourcesub);
                rvdetailreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                DataTable dtsummaryreport = ToDataTable(llstreview);
                dtsummaryreport.Columns.RemoveAt(0);
                
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MSReceivingOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                createDetailedPDF(dtsummaryreport, path);
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }
        public void ClearSearch()
        {
            try
            {
                BindCorporate();                
                BindFacility();
                BindVendor();                
                BindStatus("Add");
                HddListCorpID.Value = "";
                HddListFacID.Value = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestErrorMessage.Replace("<<MedicalSupplyRequestDescription>>", ex.Message), true);
            }
        }

        protected void btnSearchcancel_Click1(object sender, EventArgs e)
        {
            ClearSearch();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            BindGrid();
            BindStatus("Add");
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

        protected void grdmsrreviewdeatils_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbloqty = (Label)e.Row.FindControl("lbloqty");
                    TextBox txtreceivedoqty = (TextBox)e.Row.FindControl("txtreceivedoqty");
                    if (lbloqty.Text == "0")
                    {
                        e.Row.Style.Add("display", "none");
                        ROhdnrowcount = ROhdnrowcount + 1;
                    }
                }
                RoHddgridcount.Value = Convert.ToString(ROhdnrowcount);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                BALMedicalSupplyReceivingOrder llstMSRSearch = new BALMedicalSupplyReceivingOrder();

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
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstMSRSearch.Status = "ALL";
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem lst in drpStatussearch.Items)
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
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
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
                llstMSRSearch.LoggedinBy = defaultPage.UserId;
                List<SearchMedicalSuppliesReceivingSummaryReport> lstMSRMaster = lclsservice.SearchMedicalSuppliesReceivingSummaryReport(llstMSRSearch).ToList();
                grdReciveSearch.DataSource = lstMSRMaster;
                grdReciveSearch.DataBind();
                rvMedicalSupplyReceivingOrderSummary.ProcessingMode = ProcessingMode.Local;
                rvMedicalSupplyReceivingOrderSummary.LocalReport.ReportPath = Server.MapPath("~/Reports/MedicalSuppliesReceivingSummaryPrintAllReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("MedicalSuppliesReceivingPrintAllDS", lstMSRMaster);
                rvMedicalSupplyReceivingOrderSummary.LocalReport.DataSources.Clear();
                rvMedicalSupplyReceivingOrderSummary.LocalReport.DataSources.Add(datasource);
                rvMedicalSupplyReceivingOrderSummary.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvMedicalSupplyReceivingOrderSummary.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "CapitalReceivingSummary" + guid + ".pdf";
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }

        protected void grdReciveSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdReciveSearch.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}