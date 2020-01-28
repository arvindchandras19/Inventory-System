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
'' Copyright (C) 2019. Itrope Technologies
'' Name      :   <<CorporateMaster>>
'' Type      :   C# File
'' Description  :<<To add,update,delete the CorporateMaster Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	04/10/2018		   V1.0				   Sairam P	                     New
''  12/27/2018         v1.0                Murali M                      Added Event log 
''--------------------------------------------------------------------------------
'*/
namespace Inventory
{
    public partial class CorporateMaster : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALCorporate ObjCorporate = new BALCorporate();
        Page_Controls defaultPage = new Page_Controls();
        string save = string.Empty;
        string update = string.Empty;
        private string _sessionPDFFileName;
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.WarningValidCorporateMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string logmsg = Constant.WarningValidCorporatecdescMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msgerr = Constant.CorporatedeleteMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.gvCorporate);
                scriptManager.RegisterPostBackControl(this.btnPrint);

                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    BindGrid();
                    if (defaultPage != null)
                    {

                        if (defaultPage.CorporatePage_View == false && defaultPage.CorporatePage_Edit == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                        if (defaultPage.CorporatePage_Edit == false && defaultPage.CorporatePage_View == true)
                        {

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
        }

        public void BindGrid()
        {
            SearchGrid();
            //List<BindCorporateMaster> lstCor = lclsservice.BindCorporateMaster().ToList();
            //gvCorporate.DataSource = lstCor;
            //gvCorporate.DataBind();
        }

        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            ReqCorporateFullName.ErrorMessage = req;
            ReqCorporateDesc.ErrorMessage = req;
            ReqtxtPOEmail.ErrorMessage = req;
            revtxtPOEmail.ErrorMessage = Constant.RequiredExpressionEmail;
        }
        /// <summary>
        /// Search the Transfer Out details from
        /// </summary>
        #region Bind Search Values
        public void SearchGrid()
        {
            try
            {
                if (txtCorporateSearch.Text == "")
                    ObjCorporate.CorporateName = null;
                else
                    ObjCorporate.CorporateName = txtCorporateSearch.Text;
                if (txtCorporatedescription.Text == "")
                    ObjCorporate.CorporateDescription = null;
                else
                    ObjCorporate.CorporateDescription = txtCorporatedescription.Text;

                ObjCorporate.Active = rdbstatus.SelectedValue;
                ObjCorporate.LoggedinBy = defaultPage.UserId;
                ObjCorporate.Filter = "";                
                List<SearchCorporateMaster> lstCorMaster = lclsservice.SearchCorporateMaster(ObjCorporate).ToList();
                //lblrowcount.Text = "No of records : " + lstCorMaster.Count.ToString();
                gvCorporate.DataSource = lstCorMaster;
                gvCorporate.DataBind();
            }

            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                btnSearch.Visible = false;
                txtCorporateSearch.Visible = false;
                txtCorporatedescription.Visible = false;
                lblseroutHeader.Visible = false;
                //chkrestore.Visible = false;
                //lblstatus.Visible = false;
                deletebtn.Style.Add("display","none");
                savebtn.Style.Add("display", "block");
                DivSearch.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                divCorporate.Style.Add("display","none");
                ClearDetails();
                chkstatus.Visible = false;
                SetRequiredFields();
                HdnStatus.Value = "Add";
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);

            }
            
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (HdnStatus.Value == "Edit")
                {
                    UpdateCorporate();
                }
                else
                {
                    SaveCorporate();
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
        }
        public void SaveCorporate()
        {
            try
            {
                EventLogger log = new EventLogger(config);
                bool isvalidcorp = false;
                bool isvalidcorpdesc = false;
                ObjCorporate.CorporateName = txtCorporateFullName.Text;
                ObjCorporate.CorporateDescription = txtCorporateDesc.Text;
                ObjCorporate.POEmail = txtPOEmail.Text;
                ObjCorporate.CreatedBy = defaultPage.UserId;
                List<BindCorporateMaster> lstCor = lclsservice.BindCorporateMaster().Where(a => a.CorporateName == ObjCorporate.CorporateName).ToList();
                if (lstCor.Count > 0)
                {
                    isvalidcorp = true;
                }
                else
                {
                    List<BindCorporateMaster> lstCorpdesc = lclsservice.BindCorporateMaster().Where(a => a.CorporateDescription == ObjCorporate.CorporateDescription).ToList();
                    if (lstCorpdesc.Count > 0)
                    {
                        isvalidcorpdesc = true;
                    }
                }
                if (isvalidcorp == false && isvalidcorpdesc == false)
                {
                    save = lclsservice.InsertCorporateMaster(ObjCorporate);
                    if (save == "Saved Successfully")
                    {
                        string msg = Constant.CorporateSaveMessage.Replace("ShowPopup('", "").Replace("<<Corporate>>", "").Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateSaveMessage.Replace("<<Corporate>>", ""), true);
                        BindGrid();
                        clear();
                    }
                   
                }
                else
                {
                    if (isvalidcorp == true)
                    {
                        log.LogWarning(msgwrn.Replace("<<Corporate>>", txtCorporateFullName.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningValidCorporateMessage.Replace("<<Corporate>>", txtCorporateFullName.Text), true);
                    }
                    else
                    {
                        log.LogWarning(logmsg.Replace("<<Corporate>>", txtCorporateDesc.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningValidCorporatecdescMessage.Replace("<<Corporate>>", txtCorporateDesc.Text), true);
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
        }

        public void UpdateCorporate()
        {
            try
            {
                EventLogger log = new EventLogger(config);
                bool iscorp = false;
                bool isCorpesc = false;
                ObjCorporate.CorporateID = Convert.ToInt64(HiddenCorporateMasterID.Value);
                ObjCorporate.CorporateName = txtCorporateFullName.Text;
                ObjCorporate.CorporateDescription = txtCorporateDesc.Text;
                ObjCorporate.POEmail = txtPOEmail.Text;
                ObjCorporate.LastModifiededBy = defaultPage.UserId;
                if(chkstatus.Checked== true)
                {
                    ObjCorporate.IsActive = true;
                }
                else
                {
                    ObjCorporate.IsActive = false;
                }
                List<BindCorporateMaster> lstCop = lclsservice.BindCorporateMaster().Where(a => a.CorporateName == ObjCorporate.CorporateName).ToList();
                if (lstCop.Count <= 0 || txtCorporateFullName.Text == "")
                {
                    iscorp = true;
                }
                else if (lstCop[0].CorporateID == Convert.ToInt64(ObjCorporate.CorporateID))
                {
                    iscorp = true;
                }

                List<BindCorporateMaster> lstCorpdesc = lclsservice.BindCorporateMaster().Where(a => a.CorporateDescription == ObjCorporate.CorporateDescription).ToList();
                if (lstCorpdesc.Count <= 0 || txtCorporateDesc.Text == "")
                {
                    isCorpesc = true;
                }
                else if (lstCorpdesc[0].CorporateID == Convert.ToInt64(ObjCorporate.CorporateID))
                {
                    isCorpesc = true;
                }

                if (iscorp == true && isCorpesc == true)
                {
                    update = lclsservice.UpdateCorporateMaster(ObjCorporate);
                    if (update == "Updated Successfully")
                    {
                        string msg = Constant.CorporateUpdateMessage.Replace("ShowPopup('", "").Replace("<<Corporate>>", "").Replace("');", "");
                        log.LogInformation(msg);
                        //lclsservice.DeleteCorporateDetails(Convert.ToInt64(HiddenCorporateMasterID.Value), true, defaultPage.UserId);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateUpdateMessage.Replace("<<Corporate>>", ""), true);
                        BindGrid();
                        clear();

                    }
                   
                }
                else
                {
                    if (iscorp == false)
                    {
                        log.LogWarning(msgwrn.Replace("<<Corporate>>", txtCorporateFullName.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningValidCorporateMessage.Replace("<<Corporate>>", txtCorporateFullName.Text), true);
                    }
                    else
                    {
                        log.LogWarning(logmsg.Replace("<<Corporate>>", txtCorporateDesc.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningValidCorporatecdescMessage.Replace("<<Corporate>>", txtCorporateDesc.Text), true);
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCorporateSearch.Text = "";
                txtCorporatedescription.Text = "";
                rdbstatus.ClearSelection();
                rdbstatus.SelectedValue = "1";
                clear();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }


        }

        public void clear()
        {
            try
            {
                deletebtn.Style.Add("display", "block");
                savebtn.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "none");
                DivSearch.Style.Add("display", "block");
                divCorporate.Style.Add("display", "block");
                txtCorporateSearch.Visible = true;
                txtCorporatedescription.Visible = true;
                btnSearch.Visible = true;
                chkstatus.Checked = true;
                lblseroutHeader.Visible = true;
                RemoveRequiredFields();
                BindGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
          
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                //CheckBox chkactive = (gvrow.Cells[5].FindControl("chkactive") as CheckBox);
                HdnStatus.Value = "Edit";
                btnSearch.Visible = false;
                txtCorporateSearch.Visible = false;
                txtCorporatedescription.Visible = false;
                deletebtn.Style.Add("display", "none");
                savebtn.Style.Add("display", "block");
                DivSearch.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                divCorporate.Style.Add("display", "none");
                List<BindCorporateMaster> lclcor = lclsservice.BindCorporateMaster().Where(a=>a.CorporateID==Convert.ToInt64(gvrow.Cells[1].Text)).ToList();
                HiddenCorporateMasterID.Value = Convert.ToString(lclcor[0].CorporateID);
                ViewState["CorporateID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtCorporateFullName.Text = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                txtCorporateDesc.Text = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                txtPOEmail.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                HiddenCorporateName.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkstatus.Visible = false;
                }
                else
                {
                    chkstatus.Visible = true;
                }
                //if (chkactive.Checked == false)
                //{
                //    chkrestore.Visible = true;
                //    lblstatus.Visible = true;
                //    chkrestore.Checked = true;
                //    HdnStatus.Value = "Restore";
                //}
                //else
                //{
                //    chkrestore.Visible = false;
                //    lblstatus.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
          
        }
        
        //protected void btndelete_Click(object sender, EventArgs e)
        //{
        //    RemoveRequiredFields();
        //    deletebtn.Style.Add("display", "none");
        //    savebtn.Style.Add("display", "block");
        //    div_ADDContent.Style.Add("display", "none");
        //    divCorporate.Style.Add("display","block");
        //    HdnStatus.Value = "Delete";
        //    foreach (GridViewRow gvrow in gvCorporate.Rows)
        //    {
        //        CheckBox chkactive = (CheckBox)gvrow.FindControl("chkactive");
        //        chkactive.Enabled = true;
        //    }
        //}



        protected void btndeleteimg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndelete = sender as ImageButton;
                GridViewRow row = (GridViewRow)btndelete.NamingContainer;
                HiddenCorporateMasterID.Value = row.Cells[1].Text.Trim().Replace("&nbsp;", "");
                txtCorporateFullName.Text = row.Cells[2].Text.Trim().Replace("&nbsp;", "");
                HiddenCorporateName.Value = row.Cells[2].Text.Trim().Replace("&nbsp;", "");
                ObjCorporate.CorporateID = Convert.ToInt64(HiddenCorporateMasterID.Value);
                ObjCorporate.LoggedinBy = defaultPage.UserId;
                ObjCorporate.Filter = "";
                List<chkvalidcorporate> lclscorp = lclsservice.chkvalidcorporate(ObjCorporate).ToList();
                if(lclscorp.Count <= 0)
                {
                    DeleteCorporate();
                }
                else
                {
                    //string corpfac = string.Empty;
                    //foreach (var lst in lclscorp)
                    //{
                    //    corpfac += lst.FacilityDescription+",";
                    //}
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msgerr.Replace("<<Corporate>>", txtCorporateFullName.Text));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporatedeleteMessage.Replace("<<Corporate>>", txtCorporateFullName.Text), true);
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
           
        }

        public void DeleteCorporate()
        {
            try
            {
                foreach (GridViewRow gvrow in gvCorporate.Rows)
                {
                    //CheckBox chkactive = (CheckBox)gvrow.FindControl("chkactive");
                    //if (chkactive.Checked == false)
                    //{
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningCorporateMessage.Replace("<<Corporate>>", ""), true);
                    //}
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
           
        }

        protected void ImageRemoveYes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string Delete = lclsservice.DeleteCorporateDetails(Convert.ToInt64(HiddenCorporateMasterID.Value), false, defaultPage.UserId);
                if(Delete=="Deleted Successfully")
                {
                    EventLogger log = new EventLogger(config);
                    log.LogWarning(msgerr.Replace("<<Corporate>>", HiddenCorporateName.Value));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateDeleteMessage.Replace("<<Corporate>>", HiddenCorporateName.Value), true);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
            }
           
        }

        //protected void chkactive_CheckedChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //    CheckBox chkactive = (CheckBox)row.FindControl("chkactive");
        //    //List<BindCorporateMaster> lclcor = lclsservice.BindCorporateMaster().Where(a => a.CorporateID == Convert.ToInt64(row.Cells[1].Text)).ToList();
            
        //}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            clear();
        }

        //public void RestoreCorporate()
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowRestorePopup()", true);
        //}


        //protected void ImgRestore_Click(object sender, ImageClickEventArgs e)
        //{
        //    string Delete = lclsservice.DeleteCorporateDetails(Convert.ToInt64(HiddenCorporateMasterID.Value), true, defaultPage.UserId);
        //    if (Delete == "Deleted Successfully")
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateRestoreMessage.Replace("<<Corporate>>", HiddenCorporateName.Value), true);
        //    }
        //}


        public void ClearDetails()
        {
            txtCorporateFullName.Text = "";
            txtCorporateDesc.Text = "";
            txtPOEmail.Text = "";
            txtCorporateSearch.Text = "";
            txtCorporatedescription.Text = "";
        }


        public void RemoveRequiredFields()
        {
            ReqCorporateFullName.Enabled = false;
            ReqCorporateDesc.Enabled = false;  
        }

        public void SetRequiredFields()
        {
            ReqCorporateFullName.Enabled = true;
            ReqCorporateDesc.Enabled = true;
            ReqtxtPOEmail.Enabled = true;
            revtxtPOEmail.Enabled = true;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = PrintCorporate();
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Corporate Master" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message.ToString()), true);
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintCorporate()
        {
            List<object> llstarg = new List<object>();
            //List<BindCorporateMasterReport> llstreview = null;
            if (txtCorporateSearch.Text == "")
                ObjCorporate.CorporateName = null;
            else
                ObjCorporate.CorporateName = txtCorporateSearch.Text;
            if (txtCorporatedescription.Text == "")
                ObjCorporate.CorporateDescription = null;
            else
                ObjCorporate.CorporateDescription = txtCorporatedescription.Text;

            ObjCorporate.Active = rdbstatus.SelectedValue;
            ObjCorporate.LoggedinBy = defaultPage.UserId;
            ObjCorporate.Filter = "";
            List<SearchCorporateMaster> lstCorMaster = lclsservice.SearchCorporateMaster(ObjCorporate).ToList();
            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvCorporatereport.ProcessingMode = ProcessingMode.Local;
            rvCorporatereport.LocalReport.ReportPath = Server.MapPath("~/Reports/CorporateSummary.rdlc");
            ReportDataSource datasource = new ReportDataSource("CorporateMasterDS", lstCorMaster);
            rvCorporatereport.LocalReport.DataSources.Clear();
            rvCorporatereport.LocalReport.DataSources.Add(datasource);
            rvCorporatereport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvCorporatereport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        protected void gvCorporate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lbldelete = (ImageButton)e.Row.FindControl("btndeleteimg");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    if (lblActive.Text == "Yes")
                    {
                        lbldelete.Enabled = true;
                        chkstatus.Visible = false;
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkstatus.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CorporateErrorMessage.Replace("<<Corporate>>", ex.Message), true);
            }
        }
    }
}