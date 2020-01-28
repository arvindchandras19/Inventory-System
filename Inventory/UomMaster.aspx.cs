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
'' Copyright (C) 2019. Itrope Technologies
'' Name      :   <<UOM>>
'' Type      :   C# File
'' Description  :<<To add,update,delete the UOM Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Murali.M			                  New
''  08/16/2017         V1.0               Vivekanand.S                 Hidden field is cleared while Save Button click Event
''  12/27/2018         v1.0                Murali M                      Added Event log 
''--------------------------------------------------------------------------------
'*/
#endregion



namespace Inventory
{
    public partial class UomMaster : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        BALUom lcls = new BALUom();
        string uomname = string.Empty;
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.UOMSaveMessage.Replace("ShowPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Declartions
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnprint);
                if (!IsPostBack)
                {

                    RequiredFieldValidatorMessage();
                    bindUom();
                    
                    if (defaultPage != null)
                    {
                        if (defaultPage.UomMaster_Edit == false && defaultPage.UomMaster_View == true)
                        {
                            //SaveCancel.Visible = false;
                            btnsave.Visible = false;
                        }

                        if (defaultPage.UomMaster_Edit == false && defaultPage.UomMaster_View == false)
                        {
                            div_ADDContent.Visible = false;
                            grdUom.Visible = false;
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
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            reqfieldUomName.ErrorMessage = req;

        }
        public void bindUom()
        {
            try
            {
                //InventoryServiceClient lcls = new InventoryServiceClient();
                //List<GetUom> lstcol = lcls.GetUom().ToList();
                //grdUom.DataSource = lstcol;
                //grdUom.DataBind();
                               
                lcls.UomName = txtUomNamesearch.Text;
                lcls.IsActivestr = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetUomReport> llstreview = lclsservice.GetUomReport(lcls).ToList();
                //lblrowcount.Text = "No of records : " + llstreview.Count.ToString();
                grdUom.DataSource = llstreview;
                grdUom.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclsService = new InventoryServiceClient();
                if (hdnfield.Value != "0")
                    lcls.UomID = Convert.ToInt64(hdnfield.Value);
                lcls.UomName = txtUomName.Text;
                lcls.CreatedBy = defaultPage.UserId;
                lcls.CreatedOn = DateTime.Now;
                lcls.LastModifiedBy = defaultPage.UserId;
                if (chkactive.Checked == true)
                {
                    lcls.IsActive = true;
                }
                else
                {
                    lcls.IsActive = false;
                }
                List<GetUOMName> lstuomname = new List<GetUOMName>();
                lstuomname = lclsService.GetUOMName(txtUomName.Text).ToList();
                if (lstuomname.Count <= 0)
                {
                    string lstrMessage = lclsService.InsertUOM(lcls);
                    {
                        List<GetUom> lstUOM = lclsService.GetUom().ToList();
                        log.LogInformation(msgwrn.Replace("<<UOM>>", txtUomName.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMSaveMessage.Replace("<<UOM>>", txtUomName.Text), true);
                        hdnfield.Value = "0";
                    }
                    bindUom();
                    clear();                    
                }
                else
                {
                    if (lstuomname[0].UomID == Convert.ToInt64(lcls.UomID))
                    {
                        string lstrMessage = lclsService.InsertUOM(lcls);
                        {
                            List<GetUom> lstUOM = lclsService.GetUom().ToList();
                            log.LogInformation(msgwrn.Replace("<<UOM>>", txtUomName.Text));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMSaveMessage.Replace("<<UOM>>", txtUomName.Text), true);
                            hdnfield.Value = "0";
                        }
                        bindUom();
                        clear();
                    }
                    else
                    {
                        string msg = Constant.UOMDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<UOM>>", "Unit Of Measurement value already exists create a new UOM").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMDeleteMessage.Replace("<<UOM>>", "Unit Of Measurement value already exists create a new UOM"), true);
                    }                   
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }

        }
        //protected void grdpgruop_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdUom.PageIndex = e.NewPageIndex;
        //    bindUom();
        //}

        private void clear()
        {
            try
            {
                txtUomName.Text = "";
                div_ADDContent.Style.Add("display", "block");
                divsearchgrid.Style.Add("display", "block");
                divItemadd.Style.Add("display", "none");
                //txtUomName.Style.Remove("border");
                hdnfield.Value = "0";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }

        }
        protected void lbedit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnfield.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                ViewState["UomID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtUomName.Text = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                }
                else
                {
                    chkactive.Visible = true;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        //protected void chkActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //    CheckBox chkactive = (CheckBox)row.FindControl("chkActive");
        //    Int64 CreatedBy = Convert.ToInt64(defaultPage.UserId);
        //    InventoryServiceClient lclsService = new InventoryServiceClient();

        //    if (chkactive.Checked == true)
        //    {
        //        lclsService.DeleteUom(Convert.ToInt64(row.Cells[1].Text), CreatedBy, true);
        //    }
        //    else
        //    {
        //        lclsService.DeleteUom(Convert.ToInt64(row.Cells[1].Text), CreatedBy, false);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMDeleteMessage.Replace("<<UOM>>", row.Cells[2].Text), true);
        //    }

        //    bindUom();
        //}
       #endregion

        protected void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                lcls.UomName = txtUomNamesearch.Text;
                lcls.IsActivestr = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetUomReport> llstreview = lclsservice.GetUomReport(lcls).ToList();
                rvuomreport.ProcessingMode = ProcessingMode.Local;
                rvuomreport.LocalReport.ReportPath = Server.MapPath("~/Reports/UomReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetUomReportDS", llstreview);
                rvuomreport.LocalReport.DataSources.Clear();
                rvuomreport.LocalReport.DataSources.Add(datasource);
                rvuomreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvuomreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Uom" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                div_ADDContent.Style.Add("display", "none");
                divsearchgrid.Style.Add("display", "none");
                divItemadd.Style.Add("display", "block");
                chkactive.Visible = false;
                hdnfield.Value = "0";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }

        }

        protected void btnCancelsearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtUomNamesearch.Text = "";
                reactive.SelectedValue = "1";
                bindUom();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        protected void btncanceladd_Click(object sender, EventArgs e)
        {
            try
            {
                txtUomName.Text = "";
                div_ADDContent.Style.Add("display", "block");
                divsearchgrid.Style.Add("display", "block");
                divItemadd.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                lcls.UomName = txtUomNamesearch.Text;
                lcls.IsActivestr = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetUomReport> llstreview = lclsservice.GetUomReport(lcls).ToList();
                //lblrowcount.Text = "No of records : " + llstreview.Count.ToString();
                grdUom.DataSource = llstreview;
                grdUom.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }

        }

        protected void lbldelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnfield.Value = gvrow.Cells[1].Text;
                hdnuomname.Value = gvrow.Cells[2].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }

        }
        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteUom(Convert.ToInt64(Convert.ToInt64(hdnfield.Value)), defaultPage.UserId, false);
                if (lstrMessage == "Deleted Successfully")
                {
                    bindUom();
                    string msg = Constant.UOMDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<UOM>>", hdnuomname.Value).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMDeleteMessage.Replace("<<UOM>>", hdnuomname.Value), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }

        protected void grdUom_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        //divactive.Style.Add("display", "block");
                        chkactive.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.UOMErrorMessage.Replace("<<UOM>>", ex.Message), true);
            }
        }
    }
}