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
 * 14/Mar/2018         V1.0                Dhanasekaran.C                    New 
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class TransferIn : System.Web.UI.Page
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
                    if (defaultPage.RoleID == 1)
                    {
                        drpcorsearch.Attributes.Remove("disabled");
                        drpfacilitysearch.Attributes.Remove("disabled");
                        drpfacilitysearch.Attributes.Add("style", "cursor:not-allowed");            
                    }
                    else
                    {
                        drpcorsearch.Attributes.Add("disabled", "true");
                        drpfacilitysearch.Attributes.Add("disabled", "true");
                        drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                    }
                    if (defaultPage.TransferInPage_Edit == false && defaultPage.TransferInPage_View == true)
                    {
                        // btnsave.Visible = false;
                    }
                    if (defaultPage.TransferInPage_Edit == false && defaultPage.TransferInPage_View == false)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
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
                }
                foreach (ListItem lst in drpfacilitysearch.Items)
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
                drpcategorysearch.DataSource = lclsservice.GetCategoryByListFacilityID(FinalString).ToList(); ;
                drpcategorysearch.DataValueField = "CategoryID";
                drpcategorysearch.DataTextField = "CategoryName";
                drpcategorysearch.DataBind();

                foreach (ListItem lst in drpcategorysearch.Items)
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
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
                foreach (ListItem lst in drpcorsearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListCorpID.Value = lst.Value;
                    }
                }
                BindFacility();
                BindCategory("Add");
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
                BindCategory("Add");
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
                if (drpcategorysearch.SelectedValue == "All")
                {
                    objbaltransferin.ListCategoryID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpcategorysearch.Items)
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
                    DateTime firstDayLastMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);                    
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
                grdtransferinsearch.DataSource = lstTRIMaster;
                grdtransferinsearch.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnrefesh_Click(object sender, EventArgs e)
        {
            BindGrid();
            btntransferinall.Text = "Transfer In All";
        }

        protected void btntransferinall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btntransferinall.Text == "Transfer In All")
                {
                    HdnTransferIn.Value = "";
                    foreach (GridViewRow grdfs in grdtransferinsearch.Rows)
                    {
                        CheckBox ckboxIn = (CheckBox)grdfs.FindControl("ckboxIn");
                        Label lblStatus = (Label)grdfs.FindControl("lblStatus");
                        if (lblStatus.Text == StatusPendingTransfer)
                        {
                            int rowindex = grdfs.RowIndex;
                            ckboxIn.Checked = true;
                            HdnTransferIn.Value += rowindex + ",";
                        }
                    }
                    btntransferinall.Text = "Clear All";
                }
                else
                {
                    foreach (GridViewRow grdfs in grdtransferinsearch.Rows)
                    {
                        CheckBox ckboxIn = (CheckBox)grdfs.FindControl("ckboxIn");
                        Label lblStatus = (Label)grdfs.FindControl("lblStatus");
                        ckboxIn.Checked = false;
                    }
                    btntransferinall.Text = "Transfer In All";
                    HdnTransferIn.Value = "";
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
            }
        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransferOutID");
            dt.Columns.Add("TransferNo");
            dt.Columns.Add("TransferDate");
            dt.Columns.Add("CorporateIDFrom");
            dt.Columns.Add("CorporateIDTo");
            dt.Columns.Add("FacilityIDFrom");
            dt.Columns.Add("FacilityIDTo");
            dt.Columns.Add("FacilityFrom");
            dt.Columns.Add("ItemID");
            dt.Columns.Add("CategoryID");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("ItemDescription");
            dt.Columns.Add("QtyPack");
            dt.Columns.Add("UOMID");
            dt.Columns.Add("UomName");
            dt.Columns.Add("Price");
            dt.Columns.Add("Transferqty");
            dt.Columns.Add("TotalPrice");
            dt.Columns.Add("Status");
            dt.Columns.Add("Action");
            dt.AcceptChanges();
            return dt;
        }
        protected void btnreview_Click(object sender, EventArgs e)
        {
            try
            {
                string TransferIDS = Convert.ToString(HdnTransferIn.Value);
                if (TransferIDS != "")
                {
                    mpetransferinReview.Show();
                    DataTable dt = CreateDataTable();
                    TransferIDS = TransferIDS.Substring(0, TransferIDS.Length - 1);
                    string[] values = TransferIDS.Split(',');
                    string[] c = values.Distinct().ToArray();
                    foreach (string item in c)
                    {
                        GridViewRow row = grdtransferinsearch.Rows[Convert.ToInt32(item)];
                        Label lblTransferOutID = (Label)row.FindControl("lblTransferOutID");
                        Label lblTransferNo = (Label)row.FindControl("lblTransferNo");
                        Label lblTransferDate = (Label)row.FindControl("lblTransferDate");
                        Label lblCorporateFromID = (Label)row.FindControl("lblCorporateFromID");
                        Label lblCorporateToID = (Label)row.FindControl("lblCorporateToID");
                        Label lblFacilityIDFrom = (Label)row.FindControl("lblFacilityIDFrom");
                        Label lblFacilityIDTo = (Label)row.FindControl("lblFacilityIDTo");
                        Label lblFacilityNameFrom = (Label)row.FindControl("lblFacilityNameFrom");
                        Label lblItemID = (Label)row.FindControl("lblItemID");
                        Label lblCategoryID = (Label)row.FindControl("lblCategoryID");
                        Label lblCategoryName = (Label)row.FindControl("lblCategoryName");
                        Label lblItemDescription = (Label)row.FindControl("lblItemDescription");
                        Label lblQtyPack = (Label)row.FindControl("lblQtyPack");
                        Label lbluomID = (Label)row.FindControl("lbluomID");
                        Label lbluomName = (Label)row.FindControl("lbluomName");
                        Label lblPrice = (Label)row.FindControl("lblPrice");
                        Label lblTransferqty = (Label)row.FindControl("lblTransferqty");
                        Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                        Label lblStatus = (Label)row.FindControl("lblStatus");
                        CheckBox ckboxIn = (CheckBox)row.FindControl("ckboxIn");
                        if (ckboxIn.Checked == true)
                        {
                            DataRow dr = dt.NewRow();
                            dr["TransferOutID"] = lblTransferOutID.Text;
                            dr["TransferNo"] = lblTransferNo.Text;
                            dr["TransferDate"] = lblTransferDate.Text;
                            dr["CorporateIDFrom"] = lblCorporateFromID.Text;
                            dr["CorporateIDTo"] = lblCorporateToID.Text;
                            dr["FacilityIDFrom"] = lblFacilityIDFrom.Text;
                            dr["FacilityIDTo"] = lblFacilityIDTo.Text;
                            dr["FacilityFrom"] = lblFacilityNameFrom.Text;
                            dr["ItemID"] = lblItemID.Text;
                            dr["CategoryID"] = lblCategoryID.Text;
                            dr["CategoryName"] = lblCategoryName.Text;
                            dr["ItemDescription"] = lblItemDescription.Text;
                            dr["QtyPack"] = lblQtyPack.Text;
                            dr["UOMID"] = lbluomID.Text;
                            dr["UOMName"] = lbluomName.Text;
                            dr["Price"] = lblPrice.Text;
                            dr["Transferqty"] = lblTransferqty.Text;
                            dr["TotalPrice"] = lblTotalPrice.Text;
                            dr["Status"] = lblStatus.Text;
                            dr["Action"] = StatusComplete;
                            dt.Rows.Add(dr);
                        }
                        grdtransferinreview.DataSource = dt;
                        grdtransferinreview.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferInNoAction.Replace("<<TransferInDescription>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {
                BALTransferIn objtransferin = new BALTransferIn();
                Functions objfun = new Functions();
                string meditem = string.Empty;
                int ReviewCount = grdtransferinreview.Rows.Count;
                if (ReviewCount != 0)
                {
                    foreach (GridViewRow row in grdtransferinreview.Rows)
                    {
                        try
                        {
                            #region multi order generation start
                            objtransferin.TransferOutID = Convert.ToInt64(row.Cells[0].Text);
                            objtransferin.TransferNo = row.Cells[1].Text;
                            objtransferin.TransferOutDate = Convert.ToDateTime(row.Cells[2].Text);
                            objtransferin.CorporateIDfrom = Convert.ToInt64(row.Cells[3].Text);
                            objtransferin.CorporateIDTo = Convert.ToInt64(row.Cells[4].Text);
                            objtransferin.FacilityIDFrom = Convert.ToInt64(row.Cells[5].Text);
                            objtransferin.FacilityIDTo = Convert.ToInt64(row.Cells[6].Text);
                            objtransferin.FacilityFromName = row.Cells[7].Text;
                            objtransferin.ItemID = Convert.ToInt64(row.Cells[8].Text);
                            objtransferin.CategoryID = Convert.ToInt64(row.Cells[9].Text);
                            objtransferin.ItemDescription = HttpUtility.HtmlDecode(row.Cells[11].Text);
                            objtransferin.QtyPack = Convert.ToInt64(row.Cells[12].Text);
                            objtransferin.UOMID = Convert.ToInt64(row.Cells[13].Text);
                            string Price = row.Cells[15].Text;
                            string Subprice = Price.Substring(1);
                            objtransferin.Price = Convert.ToDecimal(Subprice);
                            //objtransferin.Price = Convert.ToDecimal(row.Cells[15].Text);
                            objtransferin.Transferqty = Convert.ToInt64(row.Cells[16].Text);
                            string TotalPrice = row.Cells[17].Text;
                            string SubTotalprice = TotalPrice.Substring(1);
                            objtransferin.TotalPrice = Convert.ToDecimal(SubTotalprice);
                            objtransferin.Status = row.Cells[19].Text;
                            objtransferin.LoggedinBy = defaultPage.UserId;
                            meditem = lclsservice.InsertTransferIn(objtransferin);
                            #endregion multi order generation end
                        }
                        catch (Exception innerce)
                        {
                            errmsg = errmsg + "Error in TransferID[" + objtransferin.TransferNo + "] - " + innerce.Message.ToString();
                        }
                    }
                    if (errmsg != string.Empty) throw new Exception(errmsg);
                    if (meditem == "Saved Successfully")
                    {
                        BindGrid();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInSaveMsg.Replace("<<TransferInDescription>>", ""), true);                     
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningTransferInNoRecord.Replace("<<TransferInDescription>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            grdtransferinreview.DataSource = null;
            grdtransferinreview.DataBind();
            mpetransferinReview.Hide();
        }
        protected void btnrevclose_Click(object sender, EventArgs e)
        {
            grdtransferinreview.DataSource = null;
            grdtransferinreview.DataBind();
            mpetransferinReview.Hide();

        }

        protected void grdtransferinsearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus = (e.Row.FindControl("lblStatus") as Label);
                    CheckBox ckboxIn = (e.Row.FindControl("ckboxIn") as CheckBox);
                    if (lblStatus.Text == StatusComplete || lblStatus.Text == StatusVoid)
                    {
                        ckboxIn.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.TransferInErrorMessage.Replace("<<TransferInDescription>>", ex.Message), true);
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            BindCorporate();
            BindFacility();
            BindCategory("Add");
            BindGrid();
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            List<object> llstresult = PrintTransferIn();
            byte[] bytes = (byte[])llstresult[0];
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "Transfer In Summary " + guid + ".pdf";
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
        public List<object> PrintTransferIn()
        {
            List<object> llstarg = new List<object>();
            //List<BindTransferInReport> llstreview = lclsservice.BindTransferInReport(defaultPage.UserId).ToList();
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
            if (drpcategorysearch.SelectedValue == "All")
            {
                objbaltransferin.ListCategoryID = "ALL";
            }
            else
            {
                foreach (ListItem lst in drpcategorysearch.Items)
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
                DateTime firstDayLastMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);
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
            List<SearchTransferIn> llstreview = lclsservice.SearchTransferIn(objbaltransferin).ToList();
            rvTransferInReport.ProcessingMode = ProcessingMode.Local;
            rvTransferInReport.LocalReport.ReportPath = Server.MapPath("~/Reports/TransferInSummary.rdlc");
            ReportDataSource datasource = new ReportDataSource("TransferInSummaryDS", llstreview);
            rvTransferInReport.LocalReport.DataSources.Clear();
            rvTransferInReport.LocalReport.DataSources.Add(datasource);
            rvTransferInReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvTransferInReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
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
                BindCategory("Add");
                DivMultiCorp.Style.Add("display", "none");
                divTransferInSearch.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divTransferInSearch.Attributes["class"] = "mypanel-body";
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
                divTransferInSearch.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divTransferInSearch.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divTransferInSearch.Attributes["class"] = "Upopacity";
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

            foreach (ListItem lst in drpcategorysearch.Items)
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
                    divTransferInSearch.Attributes["class"] = "Upopacity";
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

            foreach (ListItem lst in drpcategorysearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }
    }
}