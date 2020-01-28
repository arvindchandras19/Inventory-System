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
'' Name      :   <<Transfer Out>>
'' Type      :   C# File
'' Description  :<<To add,update,delete the Transfer Out Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	03/13/2018		   V1.0				   Sairam P	                     New
''--------------------------------------------------------------------------------
'*/
namespace Inventory
{
    public partial class TransferOut : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALTransferOut lstMPR = new BALTransferOut();
        Page_Controls defaultPage = new Page_Controls();
        EmailController objemail = new EmailController();
        private string _sessionPDFFileName;
        string FinalString = "";
        string StatusComplete = Constant.StatusComplete;
        string StatusPendingTransfer = Constant.StatusPendingTransfer;
        string StatusVoid = Constant.StatusVoid;
        StringBuilder SB = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                if (!IsPostBack)
                {
                    RequiredFieldValidator();
                    BindCorporate();
                    if (defaultPage != null)
                    {
                        BindCorporate();
                        BindTransferFrom();
                        BindCategory("Add");
                        BindTransferTo();
                        SearchGrid();
                        if (defaultPage.TransferOutPage_Edit == false && defaultPage.TransferOutPage_View == true)
                        {
                            // btnsave.Visible = false;
                        }
                        if (defaultPage.TransferOutPage_Edit == false && defaultPage.TransferOutPage_View == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                        if (defaultPage.RoleID == 1)
                        {
                            superadmin();
                        }
                        else
                        {
                            nonsuperadmin();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsOrderrErrorMessage.Replace("<<MachinePartsOrder>>", ex.Message), true);
            }
        }

        public void nonsuperadmin()
        {
            drpCorporate.Enabled = false;
            drpTransFrom.Enabled = false;
        }

        public void superadmin()
        {
            drpCorporate.Enabled = true;
            drpTransFrom.Enabled = true;
        }

        public void RequiredFieldValidator()
        {
            string req = Constant.RequiredFieldValidator;
            ReqdrpCorporate.ErrorMessage = req;
            ReqItemCategory.ErrorMessage = req;
            ReqTransferDate.ErrorMessage = req;
            ReqTrasnsferfrom.ErrorMessage = req;
            ReqTrasnsferTo.ErrorMessage = req;
        }

        /// <summary>
        /// Bind the Corporate details to dropdown control 
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
                    drpCorporate.DataSource = lstcrop;
                    drpCorporate.DataTextField = "CorporateName";
                    drpCorporate.DataValueField = "CorporateID";
                    drpCorporate.DataBind();
                    //ListItem lst = new ListItem();
                    //lst.Value = "0";
                    //lst.Text = "Select";
                    //drpCorporate.Items.Insert(0, lst);
                    //drpCorporate.SelectedIndex = 0;

                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpCorporate.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct().Where(a => a.CorporateID == defaultPage.CorporateID);
                    drpCorporate.DataTextField = "CorporateName";
                    drpCorporate.DataValueField = "CorporateID";
                    drpCorporate.DataBind();
                    //ListItem lst = new ListItem();
                    //lst.Value = "0";
                    //lst.Text = "Select";
                    //drpCorporate.Items.Insert(0, lst);
                    //drpCorporate.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        public void BindTransferFrom()
        {
            try
            {
                List<GetFacilityByListCorporateID> lstItem = new List<GetFacilityByListCorporateID>();
                lstItem = lclsservice.GetFacilityByListCorporateID(drpCorporate.SelectedValue, defaultPage.UserId, defaultPage.RoleID).ToList();
                if (defaultPage.RoleID == 1)
                {
                    drpTransFrom.DataSource = lstItem.Select(a => new { a.FacilityID, a.FacilityDescription }).Distinct();
                    drpTransFrom.DataTextField = "FacilityDescription";
                    drpTransFrom.DataValueField = "FacilityID";
                    drpTransFrom.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    drpTransFrom.Items.Insert(0, lst);
                    drpTransFrom.SelectedIndex = 0;
                }
                else
                {
                    drpTransFrom.DataSource = lstItem.Select(a => new { a.FacilityID, a.FacilityDescription }).Distinct().Where(a => a.FacilityID == defaultPage.FacilityID);
                    drpTransFrom.DataTextField = "FacilityDescription";
                    drpTransFrom.DataValueField = "FacilityID";
                    drpTransFrom.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "Select";
                    drpTransFrom.Items.Insert(0, lst);
                    drpTransFrom.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }
        }

        public void BindCategory(string mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetCatagoryByFacilityID> lstcategory = new List<GetCatagoryByFacilityID>();
                BALEndingInventory lstEndInv = new BALEndingInventory();
                if (mode == "Add")
                {

                    lstEndInv.FacilityID = Convert.ToInt64(drpTransFrom.SelectedValue);
                    lstcategory = lclsservice.GetCatagoryByFacilityID(lstEndInv).ToList();
                }
                else
                {
                    lstcategory = lclsservice.GetCatagoryByFacilityID(lstEndInv).ToList();
                }
                drpItemCategory.DataSource = lstcategory;
                drpItemCategory.DataValueField = "CategoryID";
                drpItemCategory.DataTextField = "CategoryName";
                drpItemCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpItemCategory.Items.Insert(0, lst);
                drpItemCategory.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }

        }

        public void BindTransferTo()
        {
            try
            {
                List<GetFacilityByListCorporateID> lstItem = new List<GetFacilityByListCorporateID>();
                lstItem = lclsservice.GetFacilityByListCorporateID(drpCorporate.SelectedValue, defaultPage.UserId, defaultPage.RoleID).ToList();
                drpTransTo.DataSource = lstItem.Select(a => new { a.FacilityID, a.FacilityDescription }).Distinct().Where(a => a.FacilityID != Convert.ToInt64(drpTransFrom.SelectedValue));
                drpTransTo.DataTextField = "FacilityDescription";
                drpTransTo.DataValueField = "FacilityID";
                drpTransTo.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpTransTo.Items.Insert(0, lst);
                drpTransTo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }
        }

        /// <summary>
        /// Search the Transfer Out details from
        /// </summary>
        #region Bind Search Values
        public void SearchGrid()
        {
            try
            {
                BALTransferOut lstMp = new BALTransferOut();
                lstMp.CorporateID = Convert.ToInt64(drpCorporate.SelectedValue);
                lstMp.FacilityIDFrom = Convert.ToInt64(drpTransFrom.SelectedValue);
                lstMp.FacilityIDTo = Convert.ToInt64(drpTransTo.SelectedValue);
                lstMp.ItemCategory = Convert.ToInt64(drpItemCategory.SelectedValue);
                if (txtTransferDate.Text =="")
                {
                    txtTransferDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    lstMp.TransferDate = Convert.ToDateTime(txtTransferDate.Text);
                }
                else
                {
                    lstMp.TransferDate = Convert.ToDateTime(txtTransferDate.Text);
                }
                lstMp.LoggedinBy = defaultPage.UserId;
                List<SearchTransferOut> lstMSRMaster = lclsservice.SearchTransferOut(lstMp).ToList();
                grdTransfer.DataSource = lstMSRMaster;
                grdTransfer.DataBind();
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransferNo");
            dt.Columns.Add("TransferDate");
            dt.Columns.Add("TransferTo");
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemDescription");
            dt.Columns.Add("UomName");
            dt.Columns.Add("UOMID");
            dt.Columns.Add("QtyPack");
            dt.Columns.Add("Price");
            dt.Columns.Add("TransferQty");
            dt.Columns.Add("TotalPrice");
            dt.AcceptChanges();
            return dt;
        }

        public void TransferNo()
        {
            Int64 TransferFrom = 0;
            Int64 TransferTo = 0;
            TransferFrom =Convert.ToInt64(drpTransFrom.SelectedValue);
            TransferTo = Convert.ToInt64(drpTransTo.SelectedValue);
            List<GetTransferNo> lstrTrans = lclsservice.GetTransferNo(TransferFrom, TransferTo).ToList();
            HdnTransferNo.Value = lstrTrans[0].TransferNo;
            //String sDate = txtTransferDate.Text;
            //sDate = DateTime.Now.ToString();
            //DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString();
            //String mn = datevalue.Month.ToString();
            //String yy = datevalue.Year.ToString();
            //string transfrom=string.Empty;
            //string transTo=string.Empty;
            //List<BindFacility> lstfac = lclsservice.BindFacility(drpTransFrom.SelectedItem.Text).ToList();
            //transfrom=lstfac[0].FacilityShortName;
            //List<BindFacility> lstfacTo = lclsservice.BindFacility(drpTransTo.SelectedItem.Text).ToList();
            //transTo=lstfacTo[0].FacilityShortName;
            //string TransferNo = transfrom + "X" + transTo + mn + yy + "";
            //HdnTransferNo.Value = TransferNo;
        }

        public decimal TotalPrice(GridViewRow row)
        {
            decimal TotalPrice = 0;
            Label lblprice = (Label)row.FindControl("lblprice");
            TextBox txtTransferQty = (TextBox)row.FindControl("txtTransferQty");
            Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
            string Price = lblprice.Text;
            string Subprice = Price.Substring(1);
            lblTotalPrice.Text = (Convert.ToDecimal(Subprice) * Convert.ToInt64(txtTransferQty.Text)).ToString();
            string TotalPric = lblTotalPrice.Text;
            TotalPrice = Convert.ToDecimal(TotalPric);
            return (TotalPrice);
        }

        private void BindReview()
        {
            try
            {
                TransferNo();
                DataTable dt = CreateDataTable();
                foreach (GridViewRow row in grdTransfer.Rows)
                {
                    TextBox txtTransferQty = (TextBox)row.FindControl("txtTransferQty");
                    string UOMID = row.Cells[3].Text;
                    string ItemID = row.Cells[4].Text;
                    string ItemDescription =HttpUtility.HtmlDecode(row.Cells[6].Text.Trim().Replace("&nbsp;", ""));
                    string UomName = row.Cells[7].Text;
                    Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                    Label lblprice = (Label)row.FindControl("lblprice");
                    string Price = lblprice.Text;
                    string Subprice = Price.Substring(1);
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    if (txtTransferQty.Text != "")
                    {
                        DataRow dr = dt.NewRow();
                        dr["TransferNo"] = HdnTransferNo.Value;
                        dr["TransferDate"] = txtTransferDate.Text;
                        dr["TransferTo"] = drpTransTo.SelectedItem.Text;
                        dr["ItemID"] = ItemID;
                        dr["ItemDescription"] = ItemDescription;
                        dr["UomName"] = UomName;
                        dr["UOMID"] = UOMID;
                        dr["QtyPack"] = lblQtyPack.Text;
                        dr["Price"] = Subprice;
                        dr["TransferQty"] = txtTransferQty.Text;
                        dr["TotalPrice"] = TotalPrice(row);
                        dt.Rows.Add(dr);
                    }
                }
                if(dt.Rows.Count > 0)
                {
                    grdTRReview.DataSource = dt;
                    grdTRReview.DataBind();
                    mpereview.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferOutTransferQtyMessage.Replace("<<TransferOut>>", ""), true);
                }
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }

        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            List<GetEmailNotificationforTransfer> lstEmail = lclsservice.GetEmailNotificationforTransfer().ToList();
            if (lstEmail.Count >0)
            {
                BindReview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferOutEmailNotify.Replace("<<TransferOut>>", ""), true);
            }
        }
        public void clear()
        {
            txtTransferDate.Text = "";
            drpCorporate.ClearSelection();
            drpItemCategory.ClearSelection();
            drpTransFrom.ClearSelection();
            drpTransTo.ClearSelection();
            SearchGrid();
        }

        public void SaveTransferOutMaster()
        {
            string Trans = string.Empty;
            try
            {
                lstMPR.TransferNo = HdnTransferNo.Value;
                lstMPR.TransferDate = Convert.ToDateTime(txtTransferDate.Text);
                lstMPR.CorporateIDFrom = Convert.ToInt64(drpCorporate.SelectedValue);
                lstMPR.CorporateIDTo = Convert.ToInt64(drpCorporate.SelectedValue);
                lstMPR.FacilityIDFrom = Convert.ToInt64(drpTransFrom.SelectedValue);
                lstMPR.FacilityIDTo = Convert.ToInt64(drpTransTo.SelectedValue);
                lstMPR.ItemCategory = Convert.ToInt64(drpItemCategory.SelectedValue);
                lstMPR.ListStatus = StatusPendingTransfer;
                lstMPR.CreatedBy = defaultPage.UserId;
                foreach (GridViewRow row in grdTRReview.Rows)
                {
                    Label ItemID = (Label)row.FindControl("ItemID");
                    Label ItemDescription = (Label)row.FindControl("ItemDescription");
                    Label UOMID = (Label)row.FindControl("UOMID");
                    Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                    Label lblprice = (Label)row.FindControl("lblprice");
                    TextBox txtTransferQty = (TextBox)row.FindControl("txtTransferQty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    lstMPR.ItemID = Convert.ToInt64(ItemID.Text);
                    lstMPR.ItemDescription = ItemDescription.Text;
                    lstMPR.UOMID = Convert.ToInt64(UOMID.Text);
                    lstMPR.QtyPack = Convert.ToInt64(lblQtyPack.Text);
                    lstMPR.TransferQty = Convert.ToInt64(txtTransferQty.Text);
                    string Price = lblprice.Text;
                    string Subprice = Price.Substring(1);
                    lstMPR.Price = Convert.ToDecimal(Subprice);
                    lstMPR.TotalPrice = TotalPrice(row);
                    Trans = lclsservice.InsertTransferDetails(lstMPR);
                }
                if (Trans == "Saved Successfully")
                {
                    SendEmail();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutSaveMessage.Replace("<<TransferOut>>", ""), true);
                    clear();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveTransferOutMaster();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            clear();
            SearchGrid();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferOutGoBackMessage.Replace("<<TransferOut>>", ""), true);
        }

        protected void drpTransFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTransferTo();
            BindCategory("Add");
        }

        protected void drpCorporate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTransferFrom();
            BindCategory("Add");
            BindTransferTo();
        }
        public void EmailSetting()
        {
            try
            {
                string transfrom = string.Empty;
                string transTo = string.Empty;
                BALFacility lclfacility = new BALFacility();
                lclfacility.SearchText = drpTransFrom.SelectedItem.Text;
                lclfacility.Active = "";
                lclfacility.LogginBy = defaultPage.UserId;
                lclfacility.Filter = "";
                List<BindFacility> lstfac = lclsservice.BindFacility(lclfacility).ToList();
                transfrom = lstfac[0].FacilityShortName;
                lclfacility.SearchText = drpTransTo.SelectedItem.Text;
                List<BindFacility> lstfacTo = lclsservice.BindFacility(lclfacility).ToList();
                transTo = lstfacTo[0].FacilityShortName;
                string FirstMessage = string.Empty;
                string NextMessage = string.Empty;
                string FinalMessage = string.Empty;
                FirstMessage = string.Format("Transfer from " + transfrom +" "+ "to" +" "+ transTo +" "+ "was created on" +" "+ txtTransferDate.Text +" " +"<br/>"+ "Please see information below");
                foreach (GridViewRow row in grdTRReview.Rows)
                {
                    string TransferID = row.Cells[0].Text;
                    HdnTransferOutID.Value = TransferID;
                    string TransferDate = row.Cells[1].Text;
                    string TransferFrom = drpTransFrom.SelectedItem.Text;
                    Label ItemID = (Label)row.FindControl("ItemID");
                    string ItemCategory = drpItemCategory.SelectedItem.Text;
                    Label ItemDescription = (Label)row.FindControl("ItemDescription");
                    Label UomName = (Label)row.FindControl("UomName");
                    Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                    Label lblprice = (Label)row.FindControl("lblprice");
                    TextBox txtTransferQty = (TextBox)row.FindControl("txtTransferQty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    string status = StatusPendingTransfer;
                    List<GetFromEmailforTransfer> Audit = lclsservice.GetFromEmailforTransfer(HdnTransferNo.Value).Where(a => a.ItemID == Convert.ToInt64(ItemID.Text)).ToList();
                    string Append = "<tr style='border: 1px solid black;'><td style='border: 1px solid bla'>" + TransferID + "</td><td style='border: 1px solid black;'>" + TransferDate + "</td><td style='border: 1px solid black;'>" + TransferFrom + "</td><td style='border: 1px solid black;'>" + ItemID.Text + "</td><td style='border: 1px solid black;'>" + ItemCategory + "</td><td style='border: 1px solid black;'>" + ItemDescription.Text + "</td><td style='border: 1px solid black;'>" + UomName.Text + "</td><td style='border: 1px solid black;'>" + lblQtyPack.Text + "</td><td style='border: 1px solid black;'>" + lblprice.Text + "</td><td style='border: 1px solid black;'>" + txtTransferQty.Text + "</td><td style='border: 1px solid black;'>" + lblTotalPrice.Text + "</td><td style='border: 1px solid black;'>" + status + "</td><td style='border: 1px solid black;'>" + Audit[0].Audit + "</td></tr>";
                    NextMessage += Append;
                }
                NextMessage = "<table style='width: 1080px;text-align: left;background-color:#ededee;border-collapse: collapse;border: 1px solid black'> <tr style='background-color:#006dd7; color: white;padding: 10px;border: 1px solid goldenrod;'> <th style='border: 1px solid black;'>Transfer Id</th><th style='border: 1px solid black;'>Transfer Date</th><th style='border: 1px solid black;'>Transferred from</th><th style='border: 1px solid black;'>SNG Item ID</th><th style='border: 1px solid black;'>Item Category</th><th style='border: 1px solid black;'>Item Description</th><th style='border: 1px solid black;'>Uom</th><th style='border: 1px solid black;'>Qty/Pack</th><th style='border: 1px solid black;'>Price</th><th style='border: 1px solid black;'>Received Qty(Each)</th><th style='border: 1px solid black;'>Total Price</th><th style='border: 1px solid black;'>Status</th><th style='border: 1px solid black;'>Audit Trail</th></tr>" + NextMessage + "</table>";
                objemail.vendorEmailcontent = FirstMessage + NextMessage + FinalMessage;
                objemail.vendoremailsubject = "Transfer Order – " + HdnTransferNo.Value;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message.ToString()), true);
            }
        }

        public void SendEmail()
        {
            try
            {
                List<GetEmailNotificationforTransfer> lstToemail = lclsservice.GetEmailNotificationforTransfer().ToList();
                List<GetFromEmailforTransfer>lstFromEmail=lclsservice.GetFromEmailforTransfer(HdnTransferNo.Value).ToList();
                objemail.FromEmail = lstFromEmail[0].FromEmail;
                objemail.ToEmail = lstToemail[0].ToEmail;
                
                EmailSetting();
                objemail.SendEmailTransferOut(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferHistoryemailMessage.Replace("<<TransferHistoryDescriptionemailMessage>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferOutErrorMessage.Replace("<<TransferOut>>", ex.Message), true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            List<object> llstresult = PrintTransferOut();
            byte[] bytes = (byte[])llstresult[0];
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "Transfer Out Summary " + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityVendorAccountErrorMessage.Replace("<<FacilityDescription>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintTransferOut()
        {
            List<object> llstarg = new List<object>();
            List<BindTransferOutReport> llstreview = lclsservice.BindTransferOutReport(defaultPage.UserId).ToList();
            rvTransferOutReport.ProcessingMode = ProcessingMode.Local;
            rvTransferOutReport.LocalReport.ReportPath = Server.MapPath("~/Reports/TransferOutSummary.rdlc");
            ReportDataSource datasource = new ReportDataSource("TransferOutSummaryDS", llstreview);
            rvTransferOutReport.LocalReport.DataSources.Clear();
            rvTransferOutReport.LocalReport.DataSources.Add(datasource);
            rvTransferOutReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvTransferOutReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

    }
}