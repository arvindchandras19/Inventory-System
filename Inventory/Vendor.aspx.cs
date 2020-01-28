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
'' Name      :   <<Vendor>>
'' Type      :   C# File
'' Description  :<<To add,update the Vendor Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Mahalakshmi.S		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class Vendor : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALVendor lcls = new BALVendor();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgern = Constant.VendorSaveMessage.Replace("ShowPopup('", "").Replace("');", "");
        string msgdelte = Constant.VendorDeleteMessage.Replace("ShowdelPopup('", "").Replace("');", "");
        #endregion
        #region Page load 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    BindvendorGrid();
                    GetState();
                    if (defaultPage != null)
                    {
                        if (defaultPage.RoleID != 1 && defaultPage.VendorPage_Edit == false && defaultPage.VendorPage_View == false)
                        {
                            gvvendor.Visible = false;
                            //btnsave.Visible = false;
                            // savebtn.Style.Add("display", "none");
                            div_searchContent.Style.Add("display", "none");
                            User_Permission_Message.Visible = true;
                            divsearchgrid.Visible = false;
                        }
                        else
                        {
                            div_ADDContent.Style.Add("display", "none");
                            if (defaultPage.VendorPage_Edit == false && defaultPage.VendorPage_View == true)
                           {
                                btnsave.Visible = false;
                                // savebtn.Style.Add("display", "none");                               
                            }

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            revEmail.ErrorMessage = Constant.RequiredExpressionEmail;
            reqContactEmail.ErrorMessage = Constant.RequiredFieldValidator;
            cusvalvendortype.ErrorMessage = Constant.cusvalvendortype;
            Reqvendorcode.ErrorMessage = Constant.RequiredFieldValidator;
            Reqvendordesc.ErrorMessage = Constant.RequiredFieldValidator;
            regpomail.ErrorMessage = Constant.RequiredExpressionEmail;
            //cusvalvendorname.ErrorMessage = Constant.cusvalvendortype;
            //cusvalvendordesc.ErrorMessage = Constant.cusvalvendortype;
        }
        #endregion
        #region Bind State Values
        public void GetState()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetState> lstpatin = lclsservice.GetState().ToList();
                ddlState.DataSource = lstpatin;
                ddlState.DataValueField = "Invenkey";
                ddlState.DataTextField = "InvenValue";
                ddlState.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlState.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
        #endregion
        #region Save event
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                BALVendor objbalvendor = new BALVendor();
                string a = string.Empty;
                if (hiddenVendorID.Value != null)
                    objbalvendor.VendorID = Convert.ToInt64(hiddenVendorID.Value);
                objbalvendor.VendorUIID = txtvendorID.Text;
                objbalvendor.VendorName = txtvendorname.Text;
                //objbalvendor.Street = txtvenstreet.Text;
                objbalvendor.City = txtcity.Text;
                objbalvendor.Address1 = txtaddress1.Text;
                objbalvendor.Address2 = txtaddress2.Text;
                objbalvendor.State = Convert.ToInt64(ddlState.SelectedValue);
                objbalvendor.Zip = txtzipcode.Text + "-" + txtzipcode1.Text;
                //objbalvendor.Country = txtcountry.Text;
                objbalvendor.Phone = txtphone.Text;
                if (txtxtn.Text != "")
                    objbalvendor.Xtn = Convert.ToInt64(txtxtn.Text);
                objbalvendor.Fax = txtfax.Text;
                objbalvendor.ContactName = txtcontactperson.Text;
                objbalvendor.ContactEmail = txtcontactemail.Text;
                objbalvendor.ContactPhone = txtcontactph.Text;
                objbalvendor.POEmail = txtpoemail.Text;
                objbalvendor.AlternateEmail = txtaltenateemail.Text;
                if (chkf.Checked)
                    objbalvendor.All = true;
                else
                    objbalvendor.All = false;
                if (chks.Checked)
                    objbalvendor.ServiceOrder = true;
                else
                    objbalvendor.ServiceOrder = false;
                if (chkf.Checked)
                    objbalvendor.BuildingMaintenance = true;
                else
                    objbalvendor.BuildingMaintenance = false;
                if (chkh.Checked)
                    objbalvendor.RegularSupplies = true;
                else
                    objbalvendor.RegularSupplies = false;
                if (chkm.Checked)
                    objbalvendor.MachineParts = true;
                else
                    objbalvendor.MachineParts = false;
                if (chkIT.Checked)
                    objbalvendor.IT = true;
                else
                    objbalvendor.IT = false;
                objbalvendor.CreatedBy = defaultPage.UserId;
                objbalvendor.UpdatedBy = defaultPage.UserId;
                if (chkactive.Checked == true)
                {
                    objbalvendor.IsActive = true;
                }
                else
                {
                    objbalvendor.IsActive = false;
                }
                List<GETVendorUniqueName> lstvuname = new List<GETVendorUniqueName>();

                lstvuname = lclserinven.GETVendorUniqueName(txtvendorID.Text).ToList();
                if (lstvuname.Count <= 0)
                {
                    a = lclserinven.InsertUpdateVendorDetails(objbalvendor);
                    if (a == "Saved Successfully")
                    {
                        btnprint.Visible = true;
                        log.LogInformation(msgern.Replace("<<Vendor>>", txtvendorname.Text.ToString()));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorSaveMessage.Replace("<<Vendor>>", txtvendorname.Text.ToString()), true);
                        BindvendorGrid();
                        clear();
                    }
                }

                else
                {
                    if (lstvuname[0].VendorID == Convert.ToInt64(objbalvendor.VendorID))
                    {
                        a = lclserinven.InsertUpdateVendorDetails(objbalvendor);
                        {
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "Saved Successfully"); 
                            log.LogInformation(msgern.Replace("<<Vendor>>", txtvendorname.Text.ToString()));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorSaveMessage.Replace("<<Vendor>>", txtvendorname.Text.ToString()), true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", "Vendor Code already exists create a new code to save"), true);
                            btnprint.Visible = true;
                            hiddenVendorID.Value = "0";
                            clear();
                        }
                        BindvendorGrid();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", "Vendor Code already exists create a new code to save"), true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "VendorcodeError();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
           
        }
        #endregion
        #region Bind Gridview
        public void BindvendorGrid()
        {
            try
            {
                if (txtSearchVendor.Text == "")
                    lcls.VendorUIID = null;
                else
                    lcls.VendorUIID = txtSearchVendor.Text;
                if (txtSearchdesc.Text == "")
                    lcls.VendorName = null;
                else
                    lcls.VendorName = txtSearchdesc.Text;
                lcls.IsStrActive = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetVendorReport> llstreview = lclsservice.GetVendorReport(lcls).ToList();
                //lblrowcount.Text = "No of records : " + llstreview.Count.ToString();
                gvvendor.DataSource = llstreview;
                gvvendor.DataBind();
                //InventoryServiceClient lclsvenservice = new InventoryServiceClient();
                //List<GetvendorDetails> lstvendordetails = new List<GetvendorDetails>();
                //string SearchText = string.Empty;
                //lstvendordetails = lclsvenservice.GetvendorDetails(SearchText).ToList();
                //gvvendor.DataSource = lstvendordetails;
                //gvvendor.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }

        }
        #endregion
        #region Edit Button Click
        protected void lbedit_Click(object sender, EventArgs e)
        {
            try
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowContent();", true);
                //btnprint.Visible = false;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["VendorID"] = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                hiddenVendorID.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                txtvendorID.Text = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                txtvendorname.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                // txtvenstreet.Text = gvrow.Cells[4].Text;
                txtaddress1.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                txtaddress2.Text = gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "");
                txtcity.Text = gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "");
                ddlState.ClearSelection();
                if (gvrow.Cells[8].Text == "&nbsp;")
                {
                    ddlState.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlState.SelectedValue = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                }

                //ddlState.SelectedValue = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                //txtstate.Text = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                txtzipcode.Text = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                if (txtzipcode.Text != "")
                {
                    string[] a = txtzipcode.Text.Split('-');
                    txtzipcode.Text = a[0].ToString();
                    if (a.Length >= 2)
                        txtzipcode1.Text = a[1].ToString();
                }
                // txtcountry.Text = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                txtphone.Text = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                txtfax.Text = gvrow.Cells[13].Text.Trim().Replace("&nbsp;", "");
                txtcontactperson.Text = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                txtcontactph.Text = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                txtcontactemail.Text = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                txtpoemail.Text = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                txtaltenateemail.Text = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");
                Label lblAll = (Label)gvrow.FindControl("lblall");
                Label lblRegularSupplies = (Label)gvrow.FindControl("lblregularsuplies");
                Label lblMachineParts = (Label)gvrow.FindControl("lblMachineparts");
                Label lblServiceOrder = (Label)gvrow.FindControl("lblserviceorder");
                Label lblBuildingMaintenance = (Label)gvrow.FindControl("lblBuildingMaintenance");
                Label lblIT = (Label)gvrow.FindControl("lblIT");
                Label lblXtn = (Label)gvrow.FindControl("lblXtn");
                if (lblAll.Text != "")
                {
                    if (Convert.ToBoolean(lblAll.Text) == true)
                        chkf.Checked = true;
                    else
                        chkf.Checked = false;
                }
                if (lblRegularSupplies.Text != "")
                {
                    if (Convert.ToBoolean(lblRegularSupplies.Text) == true)
                        chkh.Checked = true;
                    else
                        chkh.Checked = false;
                }
                if (lblMachineParts.Text != "")
                {
                    if (Convert.ToBoolean(lblMachineParts.Text) == true)
                        chkm.Checked = true;
                    else
                        chkm.Checked = false;
                }
                if (lblServiceOrder.Text != "")
                {
                    if (Convert.ToBoolean(lblServiceOrder.Text) == true)
                        chks.Checked = true;
                    else
                        chks.Checked = false;
                }
                if (lblBuildingMaintenance.Text != "")
                {
                    if (Convert.ToBoolean(lblBuildingMaintenance.Text) == true)
                        chkBM.Checked = true;
                    else
                        chkBM.Checked = false;
                }
                if (lblIT.Text != "")
                {
                    if (Convert.ToBoolean(lblIT.Text) == true)
                        chkIT.Checked = true;
                    else
                        chkIT.Checked = false;
                }
                if (lblXtn.Text == "0")
                {
                    txtxtn.Text = "";
                }
                else
                {
                    txtxtn.Text = lblXtn.Text.Trim().Replace("&nbsp;", "");
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
                //Mpevendor.Show();
                // divaddven.Style.Add("Display", "block");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowContent();", true);
                //btnaddnew.Visible = false;

                div_searchContent.Style.Add("display", "none");
                divsearchgrid.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                btnprintdt.Visible = true;
                btnprint.Visible = false;

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
          
        #endregion
        #region Delete Button Click
        protected void lbdelete_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hiddenVendorID.Value = gvrow.Cells[2].Text;
                hdnvendor.Value = gvrow.Cells[4].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }

        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteVendor(Convert.ToInt64(hiddenVendorID.Value), false, defaultPage.UserId);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindvendorGrid();
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgdelte.Replace("<<Vendor>>", hdnvendor.Value));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorDeleteMessage.Replace("<<Vendor>>", hdnvendor.Value), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
        #endregion
        #region Active Checkbox changed Event
        protected void chkactive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
                CheckBox chkactive = (CheckBox)row.FindControl("chkactive");

                InventoryServiceClient lclsService = new InventoryServiceClient();

                if (chkactive.Checked == true)
                {
                    lclsService.DeleteVendor(Convert.ToInt64(row.Cells[2].Text), true, defaultPage.UserId);
                }
                else
                {
                    lclsService.DeleteVendor(Convert.ToInt64(row.Cells[2].Text), false, defaultPage.UserId);
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgdelte.Replace("<<Vendor>>", row.Cells[4].Text.ToString()));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorDeleteMessage.Replace("<<Vendor>>", row.Cells[4].Text.ToString()), true);
                }
                BindvendorGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
          
        }
        #endregion
        #region Search Vendor Name
        protected void btnSearchVendor_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsvenservice = new InventoryServiceClient();
                List<GetvendorDetails> lstvendordetails = new List<GetvendorDetails>();
                lstvendordetails = lclsvenservice.GetvendorDetails(txtSearchVendor.Text).ToList();
                gvvendor.DataSource = lstvendordetails;
                gvvendor.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
          
        }
        #endregion

        protected void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearchVendor.Text == "")
                    lcls.VendorUIID = null;
                else
                    lcls.VendorUIID = txtSearchVendor.Text;
                if (txtSearchdesc.Text == "")
                    lcls.VendorName = null;
                else
                    lcls.VendorName = txtSearchdesc.Text;
                lcls.IsStrActive = reactive.SelectedValue;
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                List<GetVendorReport> llstreview = lclsservice.GetVendorReport(lcls).ToList();
                rvvendorreport.ProcessingMode = ProcessingMode.Local;
                rvvendorreport.LocalReport.ReportPath = Server.MapPath("~/Reports/VendorSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("VendorReportDS", llstreview);
                rvvendorreport.LocalReport.DataSources.Clear();
                rvvendorreport.LocalReport.DataSources.Add(datasource);
                rvvendorreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvvendorreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "VendorSummary" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }

        protected void btnprintdt_Click(object sender, EventArgs e)
        {
            try
            {
                lcls.VendorID = Convert.ToInt64(hiddenVendorID.Value);
                lcls.LoggedinBy = defaultPage.UserId;
                lcls.Filter = "";
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetVendorDetailsReport> llstdetailreview = lclsservice.GetVendorDetailsReport(lcls).ToList();
                rvvendorreport.ProcessingMode = ProcessingMode.Local;
                rvvendorreport.LocalReport.ReportPath = Server.MapPath("~/Reports/VendorDetailsReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("VendorDetailsReportDS", llstdetailreview);
                rvvendorreport.LocalReport.DataSources.Clear();
                rvvendorreport.LocalReport.DataSources.Add(datasource);
                rvvendorreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvvendorreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "VendorDetails" + guid + ".pdf";
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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowContent();", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
         
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            BindvendorGrid();
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                div_searchContent.Style.Add("display", "none");
                divsearchgrid.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                btnprint.Visible = false;
                btnprintdt.Visible = false;
                hiddenVendorID.Value = "0";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
          
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchVendor.Text = "";
                txtSearchdesc.Text = "";
                reactive.SelectedValue = "1";
                BindvendorGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
          

        protected void btncanceladd_Click(object sender, EventArgs e)
        {
            //div_searchContent.Style.Add("display", "block");
            //divsearchgrid.Style.Add("display", "block");
            //div_ADDContent.Style.Add("display", "none");
            clear();
            btnprint.Visible = true;
        }
        public void clear()
        {
            try
            {
                txtvendorname.Text = "";
                txtcontactperson.Text = "";
                txtcontactph.Text = "";
                txtcontactemail.Text = "";
                txtpoemail.Text = "";
                txtvendorID.Text = "";
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtcity.Text = "";
                txtzipcode.Text = "";
                txtzipcode1.Text = "";
                ddlState.SelectedValue = "0";
                txtphone.Text = "";
                txtxtn.Text = "";
                txtaltenateemail.Text = "";
                txtcountry.Text = "";
                txtfax.Text = "";
                chkf.Checked = false;
                chks.Checked = false;
                chkh.Checked = false;
                chkm.Checked = false;
                chkBM.Checked = false;
                chkIT.Checked = false;
                hiddenVendorID.Value = "0";
                div_searchContent.Style.Add("display", "block");
                divsearchgrid.Style.Add("display", "block");
                div_ADDContent.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
         
        }

        protected void gvvendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lbldelete = (ImageButton)e.Row.FindControl("lbdelete");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorErrorMessage.Replace("<<Vendor>>", ex.Message), true);
            }
        }
    }
}