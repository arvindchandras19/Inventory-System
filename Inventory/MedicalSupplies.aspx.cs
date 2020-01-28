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
'' Name      :   <<MedicalSupplies>>
'' Type      :   C# File
'' Description  :<<To add,update the Item Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Murali.M			                  New
 *  10/25/2017         V2.0                Sairam.P                     Validate mandatory fields and Check the active and inactive records in dropdown.
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class MedicalSupplies : System.Web.UI.Page
    {
        #region Declartions
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        string Currency = Constant.Currency;
        StringBuilder SB = new StringBuilder();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnprintsummary);
                scriptManager.RegisterPostBackControl(this.btnprintdetail);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    if (defaultPage != null)
                    {
                        BindItem();
                        LoadLookups("Add");
                        BindUnitPriceCurrency();
                        BindsearchCategory();
                        txtQty.Attributes.Add("onchange", "GetEachPrice();");
                        txtunitvalue.Attributes.Add("onchange", "GetEachPrice();");
                        rdsearcate.Checked = true;
                        reactive.SelectedValue = "1";

                        if (defaultPage.MedicalSuppliesPage_Edit == false && defaultPage.MedicalSuppliesPage_View == true)
                        {
                            btnsave.Visible = false;
                            btnAdd.Visible = false;
                            btnprintsummary.Visible = true;
                            //btnSearchItems.Visible = true;
                        }

                        if (defaultPage.MedicalSuppliesPage_Edit == false && defaultPage.MedicalSuppliesPage_View == false)
                        {
                            div_ADDContent.Visible = false;
                            UpdatePanel2.Visible = false;
                            grditem.Visible = false;
                            btnsave.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            //reqfielditemid.ErrorMessage = req;
            reqfielditemDesc.ErrorMessage = req;
            ReqfielddrdItemCategory.ErrorMessage = req;
            ReqfieldQty.ErrorMessage = req;
            RequiredFieldddlUOM.ErrorMessage = req;
            RequiredFieldrdUnitpriceType.ErrorMessage = req;
            RequiredFieldunitvalue.ErrorMessage = req;

        }

        public void BindItem()
        {
            try
            {
                search();
                //BALItem lcls = new BALItem();
                //List<BindItem> lstcol = lclsservice.binditem("").Where(a => a.IsActive == true).ToList();
                //grditem.DataSource = lstcol;
                //grditem.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }
        public void BindCategory(string mode)
        {
            try
            {
                List<GetItemCategory> lstcategory = new List<GetItemCategory>();
                if (mode == "Add")
                {
                    lstcategory = lclsservice.GetItemCategory().Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    lstcategory = lclsservice.GetItemCategory().ToList();
                }
                drdItemCategory.DataSource = lstcategory;
                drdItemCategory.DataValueField = "CategoryID";
                drdItemCategory.DataTextField = "CategoryName";
                drdItemCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdItemCategory.Items.Insert(0, lst);
                drdItemCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        public void BindsearchCategory()
        {
            try
            {
                List<GetItemCategory> lstcategory = new List<GetItemCategory>();
                lstcategory = lclsservice.GetItemCategory().ToList();
                drpitemcategory.DataSource = lstcategory;
                drpitemcategory.DataTextField = "CategoryName";
                drpitemcategory.DataValueField = "CategoryID";
                drpitemcategory.DataBind();

                foreach (ListItem lst in drpitemcategory.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }


        public void BindUom(string mode)
        {
            try
            {
                List<GetUom> lstpatin = new List<GetUom>();
                if (mode == "Add")
                {
                    lstpatin = lclsservice.GetUom().Where(a => a.IsActivestr == "1").ToList();
                }
                else
                {
                    lstpatin = lclsservice.GetUom().ToList();
                }
                ddlUOM.DataSource = lstpatin;
                ddlUOM.DataValueField = "UomID";
                ddlUOM.DataTextField = "UomName";
                ddlUOM.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlUOM.Items.Insert(0, lst);
                ddlUOM.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }
        public void BindUnitPriceCurrency()
        {
            try
            {
                List<ddlCurrency> lstprice = lclsservice.ddlCurrency().ToList();
                drdUnitpriceType.DataSource = lstprice;
                drdUnitpriceType.DataValueField = "CurrencyID";
                drdUnitpriceType.DataTextField = "CurrencyName";
                drdUnitpriceType.DataBind();
                drdUnitpriceType.Items.FindByText(Currency).Selected = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALItem lcls = new BALItem();
                if (ItemID.Value != null && ItemID.Value != "")
                    lcls.ItemID = Convert.ToInt64(ItemID.Value);
                //txtitemid.Text = Convert.ToString(lcls.ItemID);
                //lcls.ItemShortName = txtitemid.Text;
                lcls.ItemDescription = txtItemDesc.Text;
                lcls.CategoryID = Convert.ToInt64(drdItemCategory.SelectedValue);
                lcls.QtyPack = Convert.ToInt64(txtQty.Text);
                lcls.NDC = txtNDC.Text;
                decimal E = 0;
                E = Convert.ToDecimal(txtunitvalue.Text) / Convert.ToInt64(txtQty.Text);
                lcls.EachPrice = Convert.ToDecimal(E.ToString());
                lcls.UnitPriceCurrency = Convert.ToString(drdUnitpriceType.SelectedValue);
                lcls.UnitPriceValue = Convert.ToDecimal(txtunitvalue.Text);
                lcls.UOM = Convert.ToInt64(ddlUOM.SelectedValue);
                lcls.GPBillingCode = txtgpbill.Text;
                //if (chkstan.Checked == true)
                //{
                //    lcls.Standard = true;
                //}
                //else
                //{
                //    lcls.Standard = false;
                //}
                //if (chknonstan.Checked == true)
                //{
                //    lcls.NonStandard = true;
                //}
                //else
                //{
                //    lcls.NonStandard = false;
                //}
                lcls.CreatedBy = defaultPage.UserId;
                lcls.CreatedOn = DateTime.Now;
                lcls.LastModifiedBy = defaultPage.UserId;
                lcls.LastModifiedOn = DateTime.Now;
                if (!ValidateLoookups(lclsservice)) return;
                bool isdescvalid = false;
                bool idgpcdoe = false;
                if (chkactive.Checked == true)
                {
                    lcls.IsActive = true;
                }
                else
                {
                    lcls.IsActive = false;
                }
                List<GetItemDescName> lstitemdesc = lclsservice.GetItemDescName(txtItemDesc.Text).ToList();
                if (lstitemdesc.Count <= 0 || txtItemDesc.Text == "")
                {
                    isdescvalid = true;
                }
                else if (lstitemdesc[0].ItemID == Convert.ToInt64(lcls.ItemID))
                {
                    isdescvalid = true;
                }
                List<Validgpbillcode> lstMaster = lclsservice.Validgpbillcode(txtgpbill.Text).ToList();
                if (lstMaster.Count <= 0 || txtgpbill.Text == "")
                {
                    idgpcdoe = true;
                }
                else if (lstMaster[0].ItemID == Convert.ToInt64(lcls.ItemID))
                {
                    idgpcdoe = true;
                }

                if (isdescvalid == true && idgpcdoe == true)
                {
                    string lstrMessage = lclsservice.InsertUpdateItem(lcls);
                    string msg = Constant.MedicalSuppliesSaveMessage.Replace("ShowPopup('", "").Replace("<<Itemname>>", txtItemDesc.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesSaveMessage.Replace("<<Itemname>>", txtItemDesc.Text), true);
                    BindItem();
                    txtSearchItem.Text = "";
                    Clear();
                }
                else
                {
                    if (isdescvalid == false)
                    {
                        string msg = Constant.MedicalSuppliesvailddescMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<Itemname>>", txtItemDesc.Text).Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesvailddescMessage.Replace("<<Itemname>>", txtItemDesc.Text), true);
                    }
                    else
                    {
                        string msg = Constant.MedicalSuppliesvaildgpMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<Itemname>>", txtgpbill.Text).Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesvaildgpMessage.Replace("<<Itemname>>", txtgpbill.Text), true);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        public void Clear()
        {
            try
            {
                txtitemid.Text = "";
                txtItemDesc.Text = "";
                drdItemCategory.ClearSelection();
                drdItemCategory.SelectedIndex = 0;
                txtQty.Text = "";
                ddlUOM.ClearSelection();
                ddlUOM.SelectedIndex = 0;
                txtunitvalue.Text = "";
                drdUnitpriceType.ClearSelection();
                drdUnitpriceType.SelectedIndex = 0;
                txtEachprice.Text = "";
                txtNDC.Text = "";
                txtgpbill.Text = "";
                searchdiv.Style.Add("display", "block");
                div_ADDContent.Style.Add("display", "none");
                itemmap.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        public void binditemmap()
        {
            try
            {
                List<GetItemMapping> lstcol = lclsservice.GetItemMapping(Convert.ToInt64(hdnitemid.Value)).ToList();
                grditemmap.DataSource = lstcol;
                grditemmap.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void lbedit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                if (ItemID != null)
                    ItemID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtitemid.Text = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                hdnitemid.Value = Convert.ToString(txtitemid.Text);
                txtItemDesc.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", ""));
                LoadLookups("Edit");
                drdItemCategory.ClearSelection();
                drdItemCategory.Items.FindByText(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                Label lblValue = grditem.Rows[gvrow.RowIndex].FindControl("lblValue") as Label;
                txtunitvalue.Text = string.Format("{0:F2}", lblValue.Text).ToString();
                ddlUOM.ClearSelection();
                if (gvrow.Cells[7].Text == "&nbsp;")
                {
                    ddlUOM.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlUOM.Items.FindByText(gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                }
                txtQty.Text = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                Label lblCurrency = grditem.Rows[gvrow.RowIndex].FindControl("lblCurrency") as Label;
                drdUnitpriceType.ClearSelection();
                if (lblCurrency.Text == "")
                {
                    drdUnitpriceType.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    drdUnitpriceType.Items.FindByText(lblCurrency.Text).Selected = true;
                }
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                }
                else
                {
                    chkactive.Visible = true;
                }
                decimal de = Convert.ToDecimal(gvrow.Cells[10].Text.Trim().Replace("&nbsp;", ""));
                txtEachprice.Text = string.Format("{0:F2}", de).ToString();
                txtNDC.Text = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                txtgpbill.Text = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");


                searchdiv.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                itemmap.Style.Add("display", "none");
                btnsave.Visible = true;
                btncancelsave.Visible = true;
                btnsave.Enabled = true;
                btnprintdetail.Visible = true;
                btnprintdetail.Enabled = true;
                lnkPrice.Enabled = true;
                txtItemDesc.Enabled = true;
                drdItemCategory.Enabled = true;
                txtQty.Enabled = true;
                ddlUOM.Enabled = true;
                txtunitvalue.Enabled = true;
                drdUnitpriceType.Enabled = true;
                txtEachprice.Enabled = true;
                txtNDC.Enabled = true;
                txtgpbill.Enabled = true;
                Save.Enabled = true;
                binditemmap();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        private void LoadLookups(string mode)
        {
            BindCategory(mode);
            BindUom(mode);
        }

        private bool ValidateLoookups(InventoryServiceClient service)
        {
            bool result = true;
            string errmessage = "";
            //Item Category Lookup
            List<GetItemCategory> lstCat = service.GetItemCategory().Where(a => a.IsActive == true && a.CategoryID == Convert.ToInt64(drdItemCategory.SelectedValue)).ToList();
            if (lstCat.Count == 0)
            {
                errmessage += "ItemCategory (" + drdItemCategory.SelectedItem.Text + ") , ";
                result = false;
            }

            //UOM Lookup
            List<GetUom> lstUOM = service.GetUom().Where(a => a.IsActivestr == "1" && a.UomID == Convert.ToInt64(ddlUOM.SelectedValue)).ToList();
            if (lstUOM.Count == 0)
            {
                errmessage += "UOM (" + ddlUOM.SelectedItem.Text + ") , ";
                result = false;
            }

            if (!result)
            {
                LoadLookups("Add");
                EventLogger log = new EventLogger(config);
                string msg = Constant.WarningLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<values>>", errmessage).Replace("');", "");
                log.LogWarning(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningLookupMessage.Replace("<<values>>", errmessage), true);
            }

            return result;
        }
        //protected void chkActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //        CheckBox chkactive = (CheckBox)row.FindControl("chkActive");

        //        if (chkactive.Checked == true)
        //        {
        //            lclsservice.DeleteItem(Convert.ToInt64(row.Cells[2].Text), true, Convert.ToInt64(defaultPage.UserId));
        //        }
        //        else
        //        {
        //            lclsservice.DeleteItem(Convert.ToInt64(row.Cells[2].Text), false, Convert.ToInt64(defaultPage.UserId));
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesDeleteMessage.Replace("<<Itemname>>", row.Cells[4].Text.ToString()), true);
        //        }

        //        BindItem();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
        //    }
        //}
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                BALItem lcls = new BALItem();
                if (txtCurrency.Text == "")
                {
                    txtCurrency.Style.Add("border", "Solid 1px red");
                    mpebudget.Show();
                }
                else
                {
                    lcls.CurrencyName = txtCurrency.Text;
                    string lstrMessage = lclsservice.InsertCurrency(lcls);
                    if (lstrMessage == "Saved Successfully")
                    {
                        EventLogger log = new EventLogger(config);
                        string msg = Constant.MedicalSuppliesSaveCurrencyMessage.Replace("ShowPopup('", "").Replace("<<Itemname>>", txtCurrency.Text).Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesSaveCurrencyMessage.Replace("<<Itemname>>", txtCurrency.Text), true);
                    }
                }
                txtCurrency.Text = "";
                BindUnitPriceCurrency();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }


        protected void btnSearchItems_Click(object sender, EventArgs e)
        {
            try
            {
                if (rditem.Checked == true && rditemid.SelectedValue == "1")
                {
                    txtSearchItem.Enabled = true;
                    textsearchdesc.Text = "";
                    textsearchdesc.Enabled = false;
                }
                else if (rditem.Checked == true && rditemid.SelectedValue == "2")
                {
                    textsearchdesc.Enabled = true;
                    txtSearchItem.Text = "";
                    txtSearchItem.Enabled = false;
                }
                search();

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }
        #endregion

        public void search()
        {
            try
            {
                BALItem lcls = new BALItem();
                List<GetItemSummaryReport> list = new List<GetItemSummaryReport>();
                if(rditem.Checked== true)
                {
                    drpitemcategory.ClearSelection();
                    rditemid.Enabled = true;
                }
                foreach (ListItem lst in drpitemcategory.Items)
                {
                    if (lst.Selected && drpitemcategory.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    lcls.CategorylistID = SB.ToString().Substring(0, (SB.Length - 1));
                SB.Clear();
                if (txtSearchItem.Text != "")
                    lcls.ItemID = Convert.ToInt64(txtSearchItem.Text);
                if (textsearchdesc.Text != "")
                    lcls.ItemDescription = textsearchdesc.Text;
                lcls.IsStrActive = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = null;
                List<GetItemSummaryReport> lstcol = lclsservice.GetItemSummaryReport(lcls).ToList();
                //lblrowcount.Text = "No of records : " + lstcol.Count.ToString();
                grditem.DataSource = lstcol;
                grditem.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            LoadLookups("Add");
        }

        protected void btnprintsummary_Click(object sender, EventArgs e)
        {
            try
            {
                BALItem lcls = new BALItem();
                List<GetItemSummaryReport> list = new List<GetItemSummaryReport>();
                foreach (ListItem lst in drpitemcategory.Items)
                {
                    if (lst.Selected && drpitemcategory.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    lcls.CategorylistID = SB.ToString().Substring(0, (SB.Length - 1));
                SB.Clear();
                if (txtSearchItem.Text != "")
                    lcls.ItemID = Convert.ToInt64(txtSearchItem.Text);
                if (textsearchdesc.Text != "")
                    lcls.ItemDescription = textsearchdesc.Text;
                lcls.IsStrActive = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetItemSummaryReport> llstreview = lclsservice.GetItemSummaryReport(lcls).ToList();
                rvitemsummaryreport.ProcessingMode = ProcessingMode.Local;
                rvitemsummaryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ItemSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetItemSummaryReportDS", llstreview);
                rvitemsummaryreport.LocalReport.DataSources.Clear();
                rvitemsummaryreport.LocalReport.DataSources.Add(datasource);
                rvitemsummaryreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvitemsummaryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ItemSummary" + guid + ".pdf";
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }

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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void btnprintdetail_Click(object sender, EventArgs e)
        {
            try
            {
                BALItem lcls = new BALItem();
                lcls.ItemID = Convert.ToInt64(hdnitemid.Value);
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetItemDetailsReport> llstdetailreview = lclsservice.GetItemDetailsReport(lcls).ToList();
                rvitemdetailreport.ProcessingMode = ProcessingMode.Local;
                rvitemdetailreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ItemDetailsReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetItemDetailsReportDS", llstdetailreview);
                rvitemdetailreport.LocalReport.DataSources.Clear();
                rvitemdetailreport.LocalReport.DataSources.Add(datasource);
                rvitemdetailreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvitemdetailreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ItemDetails" + guid + ".pdf";
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
          
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                searchdiv.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                itemmap.Style.Add("display", "none");
                LoadLookups("Add");
                BindUnitPriceCurrency();
                btnsave.Visible = true;
                btncancelsave.Visible = true;
                btnprintdetail.Visible = false;
                lnkPrice.Enabled = true;
                btnsave.Enabled = true;
                lnkPrice.Enabled = true;
                txtItemDesc.Enabled = true;
                drdItemCategory.Enabled = true;
                txtQty.Enabled = true;
                ddlUOM.Enabled = true;
                txtunitvalue.Enabled = true;
                drdUnitpriceType.Enabled = true;
                txtEachprice.Enabled = true;
                txtNDC.Enabled = true;
                txtgpbill.Enabled = true;
                chkactive.Visible = false;
                ItemID.Value = "";
                hdnitemid.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void btncancelsave_Click(object sender, EventArgs e)
        {
            try
            {
                searchdiv.Style.Add("display", "block");
                div_ADDContent.Style.Add("display", "none");
                itemmap.Style.Add("display", "none");
                Clear();
                drdItemCategory.ClearSelection();
                drdItemCategory.SelectedIndex = 0;
                txtQty.Text = "";
                ddlUOM.ClearSelection();
                ddlUOM.SelectedIndex = 0;
                txtunitvalue.Text = "";
                drdUnitpriceType.ClearSelection();
                drdUnitpriceType.SelectedIndex = 0;
                txtEachprice.Text = "";
                txtNDC.Text = "";
                txtgpbill.Text = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void lblview_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                searchdiv.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                itemmap.Style.Add("display", "block");
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                if (ItemID != null)
                    ItemID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtitemid.Text = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                hdnitemid.Value = Convert.ToString(txtitemid.Text);
                txtItemDesc.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", ""));
                LoadLookups("Edit");
                drdItemCategory.ClearSelection();
                drdItemCategory.Items.FindByText(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                Label lblValue = grditem.Rows[gvrow.RowIndex].FindControl("lblValue") as Label;
                txtunitvalue.Text = string.Format("{0:F2}", lblValue.Text).ToString();
                ddlUOM.ClearSelection();
                if (gvrow.Cells[7].Text == "&nbsp;")
                {
                    ddlUOM.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlUOM.Items.FindByText(gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                }
                txtQty.Text = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                Label lblCurrency = grditem.Rows[gvrow.RowIndex].FindControl("lblCurrency") as Label;
                drdUnitpriceType.ClearSelection();
                if (lblCurrency.Text == "")
                {
                    drdUnitpriceType.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    drdUnitpriceType.Items.FindByText(lblCurrency.Text).Selected = true;
                }
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                }
                else
                {
                    chkactive.Visible = true;
                }
                decimal de = Convert.ToDecimal(gvrow.Cells[10].Text.Trim().Replace("&nbsp;", ""));
                txtEachprice.Text = string.Format("{0:F2}", de).ToString();
                txtNDC.Text = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                txtgpbill.Text = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");

                btnprintdetail.Visible = false;
                btnsave.Visible = false;
                btnsave.Enabled = false;
                btnprintdetail.Enabled = false;
                lnkPrice.Enabled = false;
                txtItemDesc.Enabled = false;
                drdItemCategory.Enabled = false;
                txtQty.Enabled = false;
                ddlUOM.Enabled = false;
                txtunitvalue.Enabled = false;
                drdUnitpriceType.Enabled = false;
                txtEachprice.Enabled = false;
                txtNDC.Enabled = false;
                txtgpbill.Enabled = false;
                Save.Enabled = false;
                binditemmap();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                drpitemcategory.ClearSelection();
                BindsearchCategory();
                //drpitemcategory.SelectedIndex = -1;
                drpitemcategory.Attributes.Remove("disabled");
                txtSearchItem.Text = "";
                textsearchdesc.Text = "";
                textsearchdesc.Enabled = false;
                txtSearchItem.Enabled = false;
                drpitemcategory.Enabled = true;
                rdsearcate.Checked = true;
                rditem.Checked = false;
                rditemid.Enabled = false;
                rditemid.ClearSelection();
                reactive.SelectedValue = "1";
                BindItem();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }
        }

        protected void rdsearcate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rditem.Checked = false;
                rditemid.ClearSelection();
                rditemid.Enabled = false;
                txtSearchItem.Text = "";
                txtItemDesc.Text = "";
                BindsearchCategory();
                drpitemcategory.Enabled = true;
                drpitemcategory.Attributes.Remove("disabled");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }

        }

        protected void rditem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rdsearcate.Checked = false;
                drpitemcategory.SelectedIndex = -1;
                drpitemcategory.Attributes.Add("disabled", "true");
                rditemid.Enabled = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }

        }

        protected void lbldelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnitemid.Value = gvrow.Cells[1].Text;
                hdnitemname.Value = gvrow.Cells[4].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message), true);
            }

        }
        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteItem(Convert.ToInt64(hdnitemid.Value), false, defaultPage.UserId);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindItem();
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.MedicalSuppliesDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<Itemname>>", hdnitemname.Value).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesDeleteMessage.Replace("<<Itemname>>", hdnitemname.Value), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesErrorMessage.Replace("<<Itemname>>", ex.Message.ToString()), true);
            }
        }

        protected void grditem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lbldelete = (ImageButton)e.Row.FindControl("lbldelete");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    if (lblActive.Text == "Yes")
                    {
                        lbldelete.Enabled = true;
                        chkactive.Visible = false;
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        chkactive.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        protected void grditem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void grditem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grditem.PageIndex = e.NewPageIndex;
            search();
        }
    }
}
