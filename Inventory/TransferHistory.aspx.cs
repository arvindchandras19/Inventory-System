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

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<TransferIn>>
'' Type      :   C# File
'' Description  :<<To add,update the TransferIn Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
 * 19/Mar/2018         V1.0                Dhanasekaran.C                    New 
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class TransferHistory : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        EmailController objemail = new EmailController();
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        string StatusComplete = Constant.StatusComplete;
        string StatusPendingTransfer = Constant.StatusPendingTransfer;
        string StatusVoid = Constant.StatusVoid;
        private string _sessionPDFFileName;
        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnPrint);
            if (!IsPostBack)
            {
                if (defaultPage != null)
                {
                    BindCorporate();
                    // drpcorsearch.SelectedValue = defaultPage.CorporateID.ToString();
                    BindFacility();
                    BindCategory("Add");
                    BindStatus("Add");
                    BindGrid();
                    if (defaultPage.TransferHistoryPage_Edit == false && defaultPage.TransferHistoryPage_View == true)
                    {
                        // btnsave.Visible = false;
                    }
                    if (defaultPage.TransferHistoryPage_Edit == false && defaultPage.TransferHistoryPage_View == false)
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
                    //ListItem lst = new ListItem();
                    //lst.Value = "0";
                    //lst.Text = "--Select Corporate--";
                    //drpcorsearch.Items.Insert(0, lst);
                    //drpcorsearch.SelectedIndex = 0;
                }

                foreach (ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryDeleteMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion


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
                    foreach (ListItem lst in drpcorsearch.Items)
                    {
                        if (lst.Selected && drpcorsearch.SelectedValue != "")
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
                BindCategory("Add");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryDeleteMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        #region BindCategory

        public void BindCategory(string mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();  
                foreach (ListItem lst in drpfacilitysearch.Items)
                {
                    if (lst.Selected && drpfacilitysearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                drpItemCategorysearch.DataSource = lclsservice.GetCategoryByListFacilityID(FinalString).ToList(); ;
                drpItemCategorysearch.DataValueField = "CategoryID";
                drpItemCategorysearch.DataTextField = "CategoryName";
                drpItemCategorysearch.DataBind();

                foreach (ListItem lst in drpItemCategorysearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }
        #endregion
        #region Bind Status
        public void BindStatus(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("Transfer", "Status", mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                drpStatussearch.Items.FindByText(StatusPendingTransfer).Selected = true;
                //foreach (ListItem lst in drpStatussearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryDeleteMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

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
                BindFacility();
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
                BindFacility();
            }
        }

        public void BindGrid()
        {
            try
            {
                BALTransferIn objbaltransferin = new BALTransferIn();

                if (drpcorsearch.SelectedValue == "All")
                {
                    objbaltransferin.ListCorporateID = "ALL";
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

                    objbaltransferin.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbaltransferin.ListFacilityID = "ALL";
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
                    objbaltransferin.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpItemCategorysearch.SelectedValue == "All")
                {
                    objbaltransferin.ListCategoryID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpItemCategorysearch.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbaltransferin.ListCategoryID = FinalString;
                }
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    objbaltransferin.Status = "ALL";
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
                    objbaltransferin.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    DateTime firstDayLastMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-2);
                    txtDateFrom.Text = firstDayLastMonth.ToString("MM/dd/yyyy"); 
                }
                else
                {
                    objbaltransferin.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbaltransferin.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                objbaltransferin.LoggedinBy = defaultPage.UserId;
                List<SearchTransferIn> lstTRIMaster = lclsservice.SearchTransferIn(objbaltransferin).ToList();
                grdTransferin.DataSource = lstTRIMaster;
                grdTransferin.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryErrorMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }


        public void BindTransferOutGrid()
        {
            try
            {
                BALTransferOut objbaltransferout = new BALTransferOut();

                if (drpcorsearch.SelectedValue == "All")
                {
                    objbaltransferout.ListCorporateID = "ALL";
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

                    objbaltransferout.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbaltransferout.ListFacilityID = "ALL";
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
                    objbaltransferout.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpItemCategorysearch.SelectedValue == "All")
                {
                    objbaltransferout.ListCategoryID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpItemCategorysearch.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbaltransferout.ListCategoryID = FinalString;
                }
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    objbaltransferout.ListStatus = "ALL";
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
                    objbaltransferout.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    DateTime Datefrom = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-2);
                    txtDateFrom.Text = Datefrom.ToString("MM/dd/yyyy"); 
                }
                else
                {
                    objbaltransferout.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbaltransferout.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                objbaltransferout.LoggedinBy = defaultPage.UserId;
                List<SearchTransferOutHistory> lstTRIMaster = lclsservice.SearchTransferOutHistory(objbaltransferout).ToList();
                grdTransferOut.DataSource = lstTRIMaster;
                grdTransferOut.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryErrorMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rdbtransferin.Checked == true)
            {
                BindGrid();
                divsearchOut.Style.Add("display", "none");
                divsearch.Style.Add("display", "block");
            }
            else
            {
                BindTransferOutGrid();
                divsearchOut.Style.Add("display", "block");
                divsearch.Style.Add("display", "none");
            }
        }

        protected void rdbtransferin_CheckedChanged(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            BindGrid();
            rdbtransferout.Checked = false;
            divsearchOut.Style.Add("display", "none");
            divsearch.Style.Add("display", "block");
        }

        protected void rdbtransferout_CheckedChanged(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            rdbtransferin.Checked = false;
            BindTransferOutGrid();
            divsearchOut.Style.Add("display", "block");
            divsearch.Style.Add("display", "none");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbtransferin.Checked == true)
                {
                    List<object> llstresult = PrintReport();
                    byte[] bytes = (byte[])llstresult[0];
                    Guid guid = Guid.NewGuid();
                    string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                    _sessionPDFFileName = "Transfer-In History" + guid + ".pdf";
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
                else
                {
                    List<object> llstresult = PrintTransferOutReport();
                    byte[] bytes = (byte[])llstresult[0];
                    Guid guid = Guid.NewGuid();
                    string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                    _sessionPDFFileName = "Transfer-Out History" + guid + ".pdf";
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryDeleteMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryDeleteMessage.Replace("<<TransferHistoryDescription>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintReport()
        {
            string TransferOutIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<GetTransferInHistoryReport> llstreview = new List<GetTransferInHistoryReport>();
            foreach (GridViewRow row in grdTransferin.Rows)
            {
                Label lblTransferOutID = (Label)row.FindControl("lblTransferOutID");
                if (TransferOutIds == string.Empty)
                    TransferOutIds = lblTransferOutID.Text;
                else
                {
                    TransferOutIds = TransferOutIds + "," + lblTransferOutID.Text;
                }
            }
            llstreview = lclsservice.GetTransferInHistoryReport(null, TransferOutIds, defaultPage.UserId,defaultPage.UserId).ToList();

            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvtransferinReport.ProcessingMode = ProcessingMode.Local;
            rvtransferinReport.LocalReport.ReportPath = Server.MapPath("~/Reports/TransferInHistory.rdlc");
            ReportDataSource datasource = new ReportDataSource("TransferInHistoryDS", llstreview);
            rvtransferinReport.LocalReport.DataSources.Clear();
            rvtransferinReport.LocalReport.DataSources.Add(datasource);
            rvtransferinReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvtransferinReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        public List<object> PrintTransferOutReport()
        {
            string TransferOutIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<BindTransferOutHistoryReport> llstreview = new List<BindTransferOutHistoryReport>();
            foreach (GridViewRow row in grdTransferOut.Rows)
            {
                Label lblTransferOutID = (Label)row.FindControl("lblTransferOutID");
                if (TransferOutIds == string.Empty)
                    TransferOutIds = lblTransferOutID.Text;
                else
                {
                    TransferOutIds = TransferOutIds + "," + lblTransferOutID.Text;
                }
            }
            llstreview = lclsservice.BindTransferOutHistoryReport(null, TransferOutIds, defaultPage.UserId,defaultPage.UserId).ToList();

            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvtransferoutreport.ProcessingMode = ProcessingMode.Local;
            rvtransferoutreport.LocalReport.ReportPath = Server.MapPath("~/Reports/TransferOutHistory.rdlc");
            ReportDataSource datasource = new ReportDataSource("TransferOutHistoryDS", llstreview);
            rvtransferoutreport.LocalReport.DataSources.Clear();
            rvtransferoutreport.LocalReport.DataSources.Add(datasource);
            rvtransferoutreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvtransferoutreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        public decimal TotalPrice(GridViewRow row)
        {
            decimal TotalPrice = 0;
            Label lblprice = (Label)row.FindControl("lblPrice");
            TextBox txtTransferQty = (TextBox)row.FindControl("txtTransferqty");
            Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
            string Price = lblprice.Text;
            string SubPrice = Price.Substring(1);
            lblTotalPrice.Text = (Convert.ToDecimal(SubPrice) * Convert.ToInt64(txtTransferQty.Text)).ToString();
            string TotalPriceStr = lblTotalPrice.Text;         
            TotalPrice = Convert.ToDecimal(TotalPriceStr);
            return (TotalPrice);
        }


        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool isquantitychanged = false;
                string TransferOut = string.Empty;
                BALTransferOut objbaltransferout = new BALTransferOut();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ImageButton imgVoid = (ImageButton)gvrow.FindControl("imgVoid");
                objbaltransferout.LastModifiedBy = defaultPage.UserId;
                Label lblTransferOutID = (Label)gvrow.FindControl("lblTransferOutID");
                Label lblprice = (Label)gvrow.FindControl("lblprice");
                Label lblTotalPrice = (Label)gvrow.FindControl("lblTotalPrice");
                Label lblStatus = (Label)gvrow.FindControl("lblStatus");
                TextBox txtTransferqty = (TextBox)gvrow.FindControl("txtTransferqty");
                TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                Label lbltransferqty = (Label)gvrow.FindControl("lbltransferqty");

                objbaltransferout.TransferID = Convert.ToInt64(lblTransferOutID.Text);
                objbaltransferout.TotalPrice = TotalPrice(gvrow);
                objbaltransferout.ListStatus = Convert.ToString(lblStatus.Text);
                objbaltransferout.TransferQty = Convert.ToInt64(txtTransferqty.Text);
                if(txtRemarks.Text!="")
                objbaltransferout.Remarks = Convert.ToString(txtRemarks.Text);

                if (txtTransferqty.Text != lbltransferqty.Text)
                {
                    if (txtRemarks.Text == "")
                    {
                        isquantitychanged = true;
                        txtRemarks.Style.Add("border", "Solid 1px red");
                    }

                }
                if (isquantitychanged == true)
                {       
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferOutRemarks.Replace("<<TransferOut>>", ""), true);
                    txtRemarks.Visible = true;
                }
                else
                {
                    TransferOut = lclsservice.UpdateTransferDetails(objbaltransferout);
                }
                if (TransferOut == "Updated Successfully")
                {
                    HiddenIsVoid.Value = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutUpdateMessage.Replace("<<TransferOut>>", ""), true);
                    BindTransferOutGrid();
                }
              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message), true);
            }
        }

        protected void grdTransferOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                foreach (GridViewRow row in grdTransferOut.Rows)
                {
                    //Image imgreadaudit = (Image)row.FindControl("imgreadaudit");
                    //Image imgreadremarks = (Image)row.FindControl("imgreadremarks");
                    ImageButton imgEdit = (ImageButton)row.FindControl("imgEdit");
                    ImageButton imgVoid = (ImageButton)row.FindControl("imgVoid");
                    ImageButton imgSave = (ImageButton)row.FindControl("imgSave");
                    TextBox txtTransferqty = (TextBox)row.FindControl("txtTransferqty");
                    TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                    Label lblStatus = (Label)row.FindControl("lblStatus");
                    //Label lblaudit = (Label)row.FindControl("lblaudit");
                    //Label lblRemarks = (Label)row.FindControl("lblRemarks");
                    Label lblTransferDate = (Label)row.FindControl("lblTransferDate");

                    string PreviousMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("MM/dd/yyyy");
                    string checktodate = DateTime.Now.ToString("MM/dd/yyyy");
                    string datafrom = txtDateFrom.Text;
                    string dateto = txtDateTo.Text;
                    string TrransferDate = lblTransferDate.Text;
                    if ((Convert.ToDateTime(TrransferDate) < Convert.ToDateTime(PreviousMonth)) || (Convert.ToDateTime(dateto) > Convert.ToDateTime(checktodate)))
                    {
                        imgVoid.Visible = false;
                        imgEdit.Visible = false;
                        imgSave.Visible = false;
                    }
                    //if (lblaudit.Text != "")
                    //{
                    //    if (lblaudit.Text.Length > 150)
                    //    {
                    //        lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                    //        imgreadaudit.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        imgreadaudit.Visible = false;
                    //    }
                    //}
                    //if (lblRemarks.Text != "")
                    //{
                    //    if (lblRemarks.Text.Length > 150)
                    //    {
                    //        lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                    //        imgreadremarks.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        imgreadremarks.Visible = false;
                    //    }
                    //}
                    if (lblStatus.Text == StatusPendingTransfer)
                    {
                        imgVoid.Enabled = true;
                    }

                   if (lblStatus.Text == StatusVoid)
                    {
                        imgVoid.Enabled = false;
                        imgEdit.Enabled = false;
                        imgSave.Enabled = false;
                    }
                    else if (defaultPage.RoleID != 1)
                    {
                        imgVoid.Enabled = false;
                        imgEdit.Enabled = false;
                        imgSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message), true);
            }
        }

        protected void imgEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;       
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ImageButton imgSave = (ImageButton)gvrow.FindControl("imgSave");
                TextBox txtTransferqty = (TextBox)gvrow.FindControl("txtTransferqty");
                TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                txtTransferqty.BorderColor = System.Drawing.Color.Black;
                imgSave.Visible = true;
                txtTransferqty.Enabled = true;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message), true);
            }
        }

        protected void imgVoid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string voidsave=string.Empty;
                bool isvoidchanged = false;
                BALTransferOut objbaltransferout = new BALTransferOut();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ImageButton imgVoid = (ImageButton)gvrow.FindControl("imgVoid");
                ImageButton imgSave = (ImageButton)gvrow.FindControl("imgSave");
                Label lblTransferOutID = (Label)gvrow.FindControl("lblTransferOutID");
                Label lblStatus = (Label)gvrow.FindControl("lblStatus");
                TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                HiddenIsVoid.Value = StatusVoid;
                objbaltransferout.TransferID = Convert.ToInt64(lblTransferOutID.Text);
                objbaltransferout.LastModifiedBy = defaultPage.UserId;
                objbaltransferout.ListStatus = StatusVoid;
                if (txtRemarks.Text != "")
                objbaltransferout.Remarks = txtRemarks.Text;
                if (HiddenIsVoid.Value == StatusVoid)
                {
                    if (txtRemarks.Text == "")
                    {
                        isvoidchanged = true;
                        txtRemarks.Style.Add("border", "Solid 1px red");
                    }
                }
                if (isvoidchanged == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferOutRemarks.Replace("<<TransferOut>>", ""), true);
                    txtRemarks.Visible = true;
                }
                else
                {
                     voidsave = lclsservice.UpdateTransferDetails(objbaltransferout);
                }     
                if (voidsave == "Updated Successfully")
                { 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutUpdateMessage.Replace("<<TransferOut>>", ""), true);
                    BindTransferOutGrid();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (rdbtransferin.Checked == true)
            {
                BindCorporate();
                BindFacility();
                BindCategory("Add");
                BindStatus("Add");
                BindGrid();
            }
            else
            {
                BindCorporate();
                BindFacility();
                BindCategory("Add");
                BindStatus("Add");
                BindTransferOutGrid();
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
                BindCategory("Add");
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
                BindCategory("Add");
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
                BindFacility();
                DivMultiCorp.Style.Add("display", "none");
                divTransdferHistory.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divTransdferHistory.Attributes["class"] = "mypanel-body";
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
                BindCategory("Add");
                DivFacCorp.Style.Add("display", "none");
                divTransdferHistory.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divTransdferHistory.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divTransdferHistory.Attributes["class"] = "Upopacity";
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
            BindCorporate();
            BindFacility();
            BindCategory("Add");
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

            foreach (ListItem lst in drpItemCategorysearch.Items)
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
                    divTransdferHistory.Attributes["class"] = "Upopacity";
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
            BindFacility();
        }

        protected void lnkClearAllFac_Click(object sender, EventArgs e)
        {
            foreach (ListItem lst in drpfacilitysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

            foreach (ListItem lst in drpItemCategorysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }
    }
}