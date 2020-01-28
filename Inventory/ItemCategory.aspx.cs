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
'' Name      :   <<ItemCategory>>
'' Type      :   C# File
'' Description  :<<To add,update the ItemCategory Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 08/12/2017		   V1.0				   Murali.M			                  New
 * 10/21/2017          V2.0                Sairam.P                           Validation Check for all mandatory fields
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class ItemCategory : System.Web.UI.Page
    {
        #region Declartions
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        InventoryServiceClient lclsService = new InventoryServiceClient();
        BALPGroup lcls = new BALPGroup();
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
                    bindgrid();
                    reactive.SelectedValue = "1";
                    if (defaultPage != null)
                    {

                        if (defaultPage.ItemCategoryPage_Edit == false && defaultPage.ItemCategoryPage_View == true)
                        {
                            btnadd.Visible = false;
                            btnsave.Visible = false;
                            btnsearch.Visible = true;
                            btnprint.Visible = true;
                        }

                        if (defaultPage.ItemCategoryPage_Edit == false && defaultPage.ItemCategoryPage_View == false)
                        {
                            div_ADDContent.Visible = false;
                            grdpgroup.Visible = false;
                            btnsave.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            Reqtxtgroupname.ErrorMessage = req;
        }

        public void bindgrid()
        {
            try
            {
                Search();
                //List<GetItemCategory> lstcol = lclsService.GetItemCategory().Where(a => a.IsActive == true).ToList();
                //grdpgroup.DataSource = lstcol;
                //grdpgroup.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        public void clear()
        {
            txtGroupName.Text = "";
            chkstan.Checked = false;
            chknonstan.Checked = false;
            div_ADDContent.Style.Add("display", "block");
            divsearchgrid.Style.Add("display", "block");
            divItemadd.Style.Add("display", "none");
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnfield.Value != "0")
                    lcls.CategoryID = Convert.ToInt64(hdnfield.Value);
                lcls.CategoryName = txtGroupName.Text;
                if (chkstan.Checked == true)
                {
                    lcls.Usage = "1";
                }
                else if (chknonstan.Checked == true)
                {
                    lcls.Usage = "0";
                }
                else
                {
                    lcls.Usage = null;
                }
                if (chkactive.Checked == true)
                {
                    lcls.IsActive = true;
                }
                else
                {
                    lcls.IsActive = false;
                }
                lcls.CreatedOn = DateTime.Now;
                lcls.CreatedBy = defaultPage.UserId;
                lcls.LastModifiedBy = defaultPage.UserId;
                string ErrMsg = string.Empty;
                List<GetItemCategory> lstcat = lclsService.GetItemCategory().Where(a => a.CategoryName == txtGroupName.Text).ToList();

                if (lstcat.Count > 0)
                {
                    if (lcls.CategoryID == 0)
                    {
                        ErrMsg = "FC";
                    }
                    else
                    {
                        if (lcls.CategoryID != lstcat[0].CategoryID)
                        {
                            ErrMsg = "FC";
                        }
                    }
                }

                if (ErrMsg == "")
                {
                    string lstrMessage = lclsService.InsertUpdateCategory(lcls);
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.ItemCategorySaveMessage.Replace("ShowPopup('", "").Replace("<<ItemCategory>>", txtGroupName.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategorySaveMessage.Replace("<<ItemCategory>>", txtGroupName.Text), true);
                    hdnfield.Value = "0";
                    bindgrid();
                    clear();
                }
                else
                {
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.ItemCategoryWarningMessage.Replace("ShowwarningPopup('", "").Replace("<<ItemCategory>>", txtGroupName.Text).Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryWarningMessage.Replace("<<ItemCategory>>", txtGroupName.Text), true);
                }
                //Functions objfun = new Functions();
                //objfun.MessageDialog(this, "Saved Successfully");


            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }
        //protected void grdpgruop_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdpgroup.PageIndex = e.NewPageIndex;
        //        bindgrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
        //    }

        //}
        protected void lbedit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnfield.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                ViewState["CategoryID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtGroupName.Text = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                if (gvrow.Cells[3].Text != "&nbsp;")
                {
                    if (gvrow.Cells[3].Text == "Standard")
                    {
                        chkstan.Checked = true;
                    }
                    else if (gvrow.Cells[3].Text == "Non Standard")
                    {
                        chknonstan.Checked = true;
                    }
                }
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                    DivCheckbox.Style.Add("display", "none");
                }
                else
                {
                    chkactive.Visible = true;
                    DivCheckbox.Style.Add("display", "block");
                }
                div_ADDContent.Style.Add("display", "none");
                divsearchgrid.Style.Add("display", "none");
                divItemadd.Style.Add("display", "block");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "editfacility();", true);

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }
        //protected void lbdelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ImageButton btndetails = sender as ImageButton;
        //        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        //        hdnfield.Value = gvrow.Cells[2].Text;
        //        // Label2.Text = "Are you sure you want to delete this Record?";
        //        ModalPopupExtender2.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmsg.Text = ex.Message;
        //    }
        //}

        //protected void chkActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //    CheckBox chkactive = (CheckBox)row.FindControl("chkActive");

        //    InventoryServiceClient lclsService = new InventoryServiceClient();
        //    Int64 CreatedBy = Convert.ToInt64(defaultPage.UserId);
        //    if (chkactive.Checked == true)
        //    {
        //        lclsService.DeleteItemCategory(Convert.ToInt64(row.Cells[1].Text), CreatedBy, true);
        //    }
        //    else
        //    {
        //        lclsService.DeleteItemCategory(Convert.ToInt64(row.Cells[1].Text), CreatedBy, false);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryDeleteMessage.Replace("<<ItemCategory>>", row.Cells[2].Text.Trim()), true);
        //    }

        //    bindgrid();
        //}
        #endregion
        protected void btnprint_Click(object sender, EventArgs e)
        {
            lcls.SearchItem = txtitemcategory.Text;
            lcls.IsStrActive = reactive.SelectedValue;
            lcls.LoggedinBy = defaultPage.UserId;
            List<GetCategoryReport> llstreview = lclsService.GetCategoryReport(lcls).ToList();
            rvitemcategoryreport.ProcessingMode = ProcessingMode.Local;
            rvitemcategoryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ItemCategoryReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("ItemCategoryReportDS", llstreview);
            rvitemcategoryreport.LocalReport.DataSources.Clear();
            rvitemcategoryreport.LocalReport.DataSources.Add(datasource);
            rvitemcategoryreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvitemcategoryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "ItemCategory" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            div_ADDContent.Style.Add("display", "none");
            divsearchgrid.Style.Add("display", "none");
            divItemadd.Style.Add("display", "block");
            DivCheckbox.Style.Add("display", "none");

            hdnfield.Value ="0";
        }

        protected void btncanceladd_Click(object sender, EventArgs e)
        {
            txtGroupName.Text = "";
            chkstan.Checked = false;
            chknonstan.Checked = false;
            div_ADDContent.Style.Add("display", "block");
            divsearchgrid.Style.Add("display", "block");
            divItemadd.Style.Add("display", "none");
            //reactive.SelectedValue = "1";
            bindgrid();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        public void Search()
        {
            try
            {
                lcls.SearchItem = txtitemcategory.Text;
                lcls.IsStrActive = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                List<SearchItemCategory> lstcol = lclsService.SearchItemCategory(lcls).ToList();
                //lblrowcount.Text = "No of records : " + lstcol.Count.ToString();
                grdpgroup.DataSource = lstcol;
                grdpgroup.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }

        }

        //protected void Chkact_CheckedChanged(object sender, EventArgs e)
        //{
        //    chkinact.Checked = false;
        //    chkall.Checked = false;
        //}

        //protected void Chkinact_CheckedChanged(object sender, EventArgs e)
        //{
        //    chkact.Checked = false;
        //    chkall.Checked = false;
        //}

        //protected void chkall_CheckedChanged(object sender, EventArgs e)
        //{
        //    chkact.Checked = false;
        //    chkinact.Checked = false;
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reactive.SelectedValue = "1";
            //chkinact.Checked = false;
            //chkall.Checked = false;
            //chkact.Checked = true;
            txtitemcategory.Text = "";
            bindgrid();
        }

        protected void chkstan_CheckedChanged(object sender, EventArgs e)
        {
            chknonstan.Checked = false;
        }

        protected void chknonstan_CheckedChanged(object sender, EventArgs e)
        {
            chkstan.Checked = false;
        }

        protected void lbldelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnfield.Value = gvrow.Cells[1].Text;
                hdnitemname.Value = gvrow.Cells[2].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }

        }
        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteItemCategory(Convert.ToInt64(hdnfield.Value), defaultPage.UserId, false);
                if (lstrMessage == "Deleted Successfully")
                {
                    bindgrid();
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.ItemCategoryDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<ItemCategory>>", hdnitemname.Value).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryDeleteMessage.Replace("<<ItemCategory>>", hdnitemname.Value), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }

        protected void grdpgroup_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        DivCheckbox.Style.Add("display", "none");
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkactive.Visible = true;
                        DivCheckbox.Style.Add("display", "block");
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

        protected void grdFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdpgroup.PageIndex = e.NewPageIndex;
                bindgrid();

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                lblmsg.Text = ex.Message;
            }

        }
    }
}

