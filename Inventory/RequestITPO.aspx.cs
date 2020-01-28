using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using Inventory.Class;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;
using System.Text;

namespace Inventory
{
    public partial class RequestIT_PO : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        EmailController objemail = new EmailController();
        BALRequestITPO objbalreqITPO = new BALRequestITPO();
        string actionOrder = Constant.actionOrder;
        string actionApprove = Constant.actionApprove;
        string actionDeny = Constant.actionDeny;
        string actionHold = Constant.actionHold;
        string StatusOrder = Constant.OrderStatus;
        string StatusApprove = Constant.PendingOrderStatus;
        string StatusDeny = Constant.DeniedStatus;
        string StatusHold = Constant.HoldStatus;
        string OldStatusPend = Constant.OldStatusPend;
        string statusdrp = Constant.PendingApprovalfornonuser;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                //scriptManager.RegisterPostBackControl(this.btnGenerateOrder);
                scriptManager.RegisterPostBackControl(this.grdITRequestPO);
                scriptManager.RegisterPostBackControl(this.grdreviewreqpo);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                defaultPage = (Page_Controls)Session["Permission"];
                if (!IsPostBack)
                {

                    if (defaultPage != null)
                    {
                        BindCorporate();                       
                        BindFacility();                     
                        BindVendor();
                        BindStatus("Add");                       
                        BindRequestPOGrid();

                        if (defaultPage.RoleID == 1)
                        {
                            btnapproveall.Visible = false;
                        }
                        else
                        {                          
                            btnorderall.Visible = false;
                            if (defaultPage.Req_RequestITPOPage_Edit == false && defaultPage.Req_RequestITPOPage_View == true)
                            {
                                btnorderall.Visible = false;
                            }
                            if (defaultPage.Req_RequestITPOPage_Edit == false && defaultPage.Req_RequestITPOPage_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                            }

                        }                       
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

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
                    drpcorsearch.DataSource = lstcrop;
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();                   
                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();                    
                }
                foreach (ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Facility details from Facility marter table to dropdown control 
        /// </summary>
        #region Bind Facility Values
        private void BindFacility()
        {
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
                BindVendor();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
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
                if (drpfacilitysearch.SelectedValue != "")
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

                    string SearchText = string.Empty;
                    List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
                    lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => (a.IT == true)).Distinct().ToList();
                    drpvendorsearch.DataSource = lstvendor;
                    drpvendorsearch.DataTextField = "VendorDescription";
                    drpvendorsearch.DataValueField = "VendorID";
                    drpvendorsearch.DataBind();                   
                }
                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }

        public void BindStatus(string mode)
        {
            try
            {                
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("ITRequestPO", "Status", mode).ToList();
                // Search Status Drop Down
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();              
                if (defaultPage.RoleID == 1)
                {
                    drpStatussearch.Items.FindByText(StatusApprove).Selected = true;
                }
                else
                {
                    drpStatussearch.Items.FindByText(statusdrp).Selected = true;
                }               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }



        public void BindRequestPOGrid()
        {
            try
            {
                SearchGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            try
            {
                BindRequestPOGrid();
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void grdITRequestPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                    //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                    //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                    //if (lblRemarks.Text.Length > 150)
                    //{
                    //    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                    //    imgreadmore.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmore.Visible = false;
                    //}
                    //if (lblaudit.Text.Length > 150)
                    //{
                    //    lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                    //    imgreadmore1.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmore1.Visible = false;
                    //}
                    DropDownList drpaction = (e.Row.FindControl("drpaction") as DropDownList);
                    ImageButton imgbtnEdit = (e.Row.FindControl("imgbtnEdit") as ImageButton);
                    ImageButton imgresend = (e.Row.FindControl("imgresend") as ImageButton);
                    Label lblpostatus = e.Row.FindControl("lblpostatus") as Label;
                    string Status = lblpostatus.Text;
                    if (defaultPage.RoleID == 1 && Status == StatusOrder)
                    {
                        imgresend.Visible = true;
                    }
                    else
                    {
                        imgresend.Visible = false;
                    }

                    if (Status == StatusApprove)
                    {
                        drpaction.Enabled = true;

                    }
                    else if (Status == StatusOrder)
                    {                       
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;
                    }
                    else if (Status == StatusDeny)
                    {
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;                              
                    }
                    else if (Status == StatusHold)
                    {
                        drpaction.Enabled = true;
                        imgbtnEdit.Visible = true;
                    }
                    else
                    {
                        drpaction.Enabled = true;
                        imgbtnEdit.Visible = true;                     
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnapproveall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnapproveall.Text == "Approve All")
                {
                    HdnMSRDetailID.Value = "";
                    foreach (GridViewRow grdfs in grdITRequestPO.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HdnMSRDetailID.Value += rowindex + ",";
                        if (drpaction.Enabled == true)
                        {                          
                            drpaction.SelectedValue = actionApprove;
                        }
                        btnapproveall.Text = "Clear All";
                    }
                }

                else
                {
                    foreach (GridViewRow grdfs in grdITRequestPO.Rows)
                    {                      
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        if (drpaction.Enabled == true)
                        {
                            drpaction.SelectedValue = "0";
                        }
                        btnapproveall.Text = "Approve All";                        
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }

        protected void btnorderall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnorderall.Text == "Order All")
                {
                    HdnMSRDetailID.Value = "";
                    foreach (GridViewRow grdfs in grdITRequestPO.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HdnMSRDetailID.Value += rowindex + ",";
                        if (drpaction.Enabled == true)
                        {
                            // drpaction.SelectedIndex = 3;
                            drpaction.SelectedValue = actionOrder;
                        }
                        btnorderall.Text = "Clear All";
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdITRequestPO.Rows)
                    {
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;                        
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        if (drpaction.Enabled == true)
                        {
                            drpaction.SelectedValue = "0";
                        }
                        btnorderall.Text = "Order All";                      
                    }
                    HdnMSRDetailID.Value = "";
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("RequestITMasterID");
                dt.Columns.Add("CreatedOn");
                dt.Columns.Add("CorporateID");
                dt.Columns.Add("Facility");
                dt.Columns.Add("VendorID");
                dt.Columns.Add("CorporateName");
                dt.Columns.Add("FacilityShortName");
                dt.Columns.Add("VendorShortName");
                dt.Columns.Add("ITRNo");
                dt.Columns.Add("ITNNo");
                dt.Columns.Add("TotalCost");
                dt.Columns.Add("Status");
                dt.Columns.Add("Action");
                dt.Columns.Add("Audit");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("CreatedBy");
                dt.Columns.Add("RequestITOrderID");
                dt.Columns.Add("OldStatus");
                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
            return dt;
        }

        protected void btnbtmreview_Click(object sender, EventArgs e)
        {
            try
            {               
                btnGenerateOrder.Visible = true;
                grdreviewreqpo.DataSource = null;
                grdreviewreqpo.DataBind();
                ViewState["ReqID"] = "";
                lblordermsg.Text = "";
                string OrderIDS = Convert.ToString(HdnMSRDetailID.Value);
                DataTable dt = CreateDataTable();
                if (OrderIDS != "")
                {
                    OrderIDS = OrderIDS.Substring(0, OrderIDS.Length - 1);
                    string[] values = OrderIDS.Split(',');
                    string[] c = values.Distinct().ToArray();                    
                    foreach (string val in c)
                    {
                        if (val != "")
                        {
                            GridViewRow row = grdITRequestPO.Rows[Convert.ToInt32(val)];
                            DropDownList drpaction = (DropDownList)row.FindControl("drpaction");
                            Label lblpostatus = (Label)row.FindControl("lblpostatus");
                            string ITRequestMasterID = row.Cells[1].Text;
                            string CorporateID = row.Cells[2].Text;
                            string FacilityID = row.Cells[3].Text;
                            string VendorID = row.Cells[4].Text;
                            string CreatedOn = row.Cells[5].Text;
                            string CorporateName = row.Cells[6].Text;
                            string FacilityShortName = row.Cells[7].Text;
                            string VendorShortName = row.Cells[8].Text;
                            lblCorp.Text = drpcorsearch.SelectedItem.Text;
                            lblFac.Text = drpfacilitysearch.SelectedItem.Text;
                            lblVen.Text = VendorShortName;
                            LinkButton lbitrno = (LinkButton)row.FindControl("lbitrno");
                            LinkButton lbitono = (LinkButton)row.FindControl("lbitono");
                            Label lblaudit = (Label)row.FindControl("lblaudit");
                            string TotalPrice = row.Cells[11].Text;
                            string Status = lblpostatus.Text;

                            Int64 RequestITOrderID = 0;
                            if (row.Cells[16].Text != "&nbsp;")
                                RequestITOrderID = Convert.ToInt64(row.Cells[16].Text);                           
                            String Reviewstatus = string.Empty;
                            Image imgreadmore1 = (Image)row.FindControl("imgreadmore1");
                            if (drpaction.SelectedItem.Text == actionApprove)
                            {
                                Reviewstatus = StatusApprove;
                                //btnGenerateOrder.Text = "Approve Order";
                            }
                            else if (drpaction.SelectedItem.Text == actionOrder)
                            {
                                Reviewstatus = StatusOrder;
                               // btnGenerateOrder.Text = "Generate/Approve Order";
                            }
                            else if (drpaction.SelectedItem.Text == actionDeny)
                            {
                                Reviewstatus = StatusDeny;
                               // btnGenerateOrder.Text = "Approve Order";
                            }
                            else if (drpaction.SelectedItem.Text == actionHold)
                            {
                                Reviewstatus = StatusHold;
                               // btnGenerateOrder.Text = "Approve Order";
                            }                          
                            
                            if ((drpaction.SelectedIndex != 0) && (Status != "Ordered"))
                            {

                                DataRow dr = dt.NewRow();
                                dr["RequestITMasterID"] = ITRequestMasterID;
                                dr["CorporateID"] = CorporateID;
                                dr["Facility"] = FacilityID;
                                dr["VendorID"] = VendorID;
                                dr["CreatedOn"] = CreatedOn;                               
                                dr["CorporateName"] = CorporateName;
                                dr["FacilityShortName"] = FacilityShortName;
                                dr["VendorShortName"] = VendorShortName;
                                dr["ITRNo"] = lbitrno.Text;
                                if (lbitono.Text != "")
                                    dr["ITNNo"] = lbitono.Text;
                                else
                                    dr["ITNNo"] = "";
                                dr["TotalCost"] = TotalPrice;
                                dr["Status"] = Reviewstatus;
                                dr["OldStatus"] = Status;                                
                                dr["Action"] = drpaction.SelectedItem.Text;
                                if (drpaction.SelectedValue != actionOrder)
                                {
                                    Label lblRemarks = (Label)row.FindControl("lblRemarks");                                   
                                    dr["Remarks"] = lblRemarks.Text;

                                }
                                else
                                {
                                    dr["Remarks"] = "";
                                }                               
                                dr["RequestITOrderID"] = RequestITOrderID;
                                dr["Audit"] = lblaudit.Text;
                                dt.Rows.Add(dr);
                            }                          

                            ViewState["OldStatus"] = dt;
                            grdreviewreqpo.DataSource = dt;
                            grdreviewreqpo.DataBind();
                            mpereview.Show();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITreqPOMessage.Replace("<<ITRequestPODescription>>", "No action performed"), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        public void ReviewGrid()
        {
            try
            {
                List<GETITRequestPODetails> lstMSRMaster = lclsservice.GETITRequestPODetails().ToList();
                grdreviewreqpo.DataSource = lstMSRMaster;
                grdreviewreqpo.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void drpaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSpin2 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
                int RowIndex = gridrow.RowIndex;
                Label lblpostatus = (Label)grdITRequestPO.Rows[RowIndex].Cells[11].FindControl("lblpostatus");
                foreach (GridViewRow grdfs in grdITRequestPO.Rows)
                {
                    if (ddlSpin2.SelectedItem.Text == actionOrder)
                        lblpostatus.Text = StatusOrder;
                    if (ddlSpin2.SelectedItem.Text == actionHold)
                        lblpostatus.Text = StatusHold;
                    if (ddlSpin2.SelectedItem.Text == actionDeny)
                        lblpostatus.Text = StatusDeny;
                    if (ddlSpin2.SelectedItem.Text == actionApprove)
                        lblpostatus.Text = StatusApprove;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
                {
                    SearchGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningITReqPOMessagedate.Replace("<<ITRequestPODescription>>", "Date from should be less than date to"), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        public void SearchGrid()
        {
            try
            {
                if (drpcorsearch.SelectedValue == "All")
                {
                    objbalreqITPO.CorporateIDs = "ALL";
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

                    objbalreqITPO.CorporateIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbalreqITPO.FacilityIDs = "ALL";
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
                    objbalreqITPO.FacilityIDs = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    objbalreqITPO.VendorIDs = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpvendorsearch.Items)
                    {
                        if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbalreqITPO.VendorIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    objbalreqITPO.Status = "ALL";
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
                    objbalreqITPO.Status = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalreqITPO.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalreqITPO.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

               objbalreqITPO.LoggedinBy = Convert.ToInt64(defaultPage.UserId);
                List<SearchRequestITPO> lstMSRMaster = lclsservice.SearchRequestITPO(objbalreqITPO).ToList();
                grdITRequestPO.DataSource = lstMSRMaster;
                grdITRequestPO.DataBind();

                int i = 0;
                foreach (GridViewRow row in grdITRequestPO.Rows)
                {
                    DropDownList drp1 = (DropDownList)row.FindControl("drpaction");
                    List<string> SplitAction = new List<string>();
                    SplitAction = lstMSRMaster[i].Action.Split(',').ToList();
                    drp1.DataSource = SplitAction;
                    drp1.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Action--";
                    drp1.Items.Insert(0, lst);
                    drp1.SelectedIndex = 0;
                    i++;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
                Response.Redirect("RequestIT.aspx?ITRNo=" + lbitrno.Text, false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnGenerateOrder_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {
                BALRequestITPO objrequestITPO = new BALRequestITPO();
                Functions objfun = new Functions();
                string meditem = string.Empty;
                int ReviewCount = grdreviewreqpo.Rows.Count;
                Int64 RequestITOrderID = 0;
                string result = string.Empty;
                if (ReviewCount != 0)
                {
                    foreach (GridViewRow row in grdreviewreqpo.Rows)
                    {
                        try
                        {
                            #region multi order generation start
                            objrequestITPO.Remarks = string.Empty;
                            //GridViewRow row = grdreviewreqpo.Rows[i];
                            if (row.Cells[16].Text != "")
                                RequestITOrderID = Convert.ToInt64(row.Cells[16].Text);
                            LinkButton lbrevitrno = (LinkButton)row.FindControl("lbrevitrno");
                            result = lbrevitrno.Text;
                            var ITNNo = "ITN" + result.Substring(3);
                            LinkButton lbrevitono = (LinkButton)row.FindControl("lbrevitono");
                            Label lblrevpostatus = (Label)row.FindControl("lblrevpostatus");
                            // throw new Exception(ITNNo + "Manual generated error");
                            string lbltotal = row.Cells[10].Text;
                            string totprice = lbltotal.Substring(1);
                            string Status = lblrevpostatus.Text;
                            objrequestITPO.ITRNo = lbrevitrno.Text;
                            objrequestITPO.CorporateID = Convert.ToInt64(row.Cells[1].Text);
                            objrequestITPO.FacilityID = Convert.ToInt64(row.Cells[2].Text);
                            objrequestITPO.VendorID = Convert.ToInt64(row.Cells[3].Text);
                            objrequestITPO.OrderDate = Convert.ToDateTime(row.Cells[4].Text);
                            objrequestITPO.RequestITMasterID = Convert.ToInt64(row.Cells[15].Text);
                            ViewState["ReqID"] += row.Cells[15].Text + ",";
                            ViewState["RequestITMasterID"] = objrequestITPO.RequestITMasterID;
                            Int64 ITRequestMasterID = objrequestITPO.RequestITMasterID;
                            objrequestITPO.TotalPrice = Convert.ToDecimal(totprice);
                            objrequestITPO.Status = Status;
                            objrequestITPO.SortOrder = 1;
                            if (Status != StatusOrder)
                            {
                                TextBox lblremarks = (TextBox)row.FindControl("lblRemarks");
                                objrequestITPO.Remarks = lblremarks.Text;
                            }
                            objrequestITPO.CreatedBy = defaultPage.UserId;                            
                            byte[] data = new byte[0];
                            objrequestITPO.OrderContent = data;
                            meditem = lclsservice.InsertrequestPO(objrequestITPO);
                            List<GetITROrderContentPO> llstreview = lclsservice.GetITROrderContentPO(ITRequestMasterID, defaultPage.UserId).ToList();
                            if (Status != StatusApprove && Status != StatusHold)
                            {
                                List<object> llstresult = DetailsOrderReport(ITRequestMasterID);
                                byte[] bytes = (byte[])llstresult[2];
                                objrequestITPO.OrderContent = bytes;
                                MemoryStream attachstream = new MemoryStream(bytes);
                                Int64 CorporateID = objrequestITPO.CorporateID;
                                Int64 FacilityID = objrequestITPO.FacilityID;
                                objemail.FromEmail = llstresult[0].ToString();
                                objemail.ToEmail = llstresult[1].ToString();
                                EmailSetting(CorporateID, FacilityID, ITNNo);
                                if (Convert.ToInt64(llstreview[0].RequestITOrderID) != 0)
                                    RequestITOrderID = Convert.ToInt64(llstreview[0].RequestITOrderID);
                                objrequestITPO.RequestITOrderID = RequestITOrderID;
                                string reqit = lclsservice.UpdateITRequestPO(objrequestITPO);

                                #region SendEmail code block
                                try
                                {
                                    objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                                }
                                catch (Exception sendemailce)
                                {
                                    LinkButton lbitrno = (LinkButton)row.FindControl("lbrevitrno");
                                    string result1 = lbitrno.Text;
                                    var ITNNumber = "ITN" + result1.Substring(3);
                                    errmsg = errmsg + "Error in ITN[" + ITNNumber + "] - Send Email- " + sendemailce.Message.ToString();
                                }
                                #endregion SendEmail code block
                            }
                                lblordermsg.Visible = true;
                                lblordermsg.Text = "ITRequest Order changes are  updated/approved successfully";
                                lblordermsg.ForeColor = System.Drawing.Color.Green;
                                lblordermsg.Style.Add("font-weight", "bold");
                            
                            #endregion multi order generation end
                        }
                        catch (Exception innerce)
                        {
                            LinkButton lbrevitrno = (LinkButton)row.FindControl("lbrevitrno");
                            string result1 = lbrevitrno.Text;
                            var PONo = "ITN" + result1.Substring(3);
                            errmsg = errmsg + "Error in ITN[" + PONo + "] - " + innerce.Message.ToString();
                        }
                    }
                    if (errmsg != string.Empty) throw new Exception(errmsg);
                    mpereview.Show();
                    btnrevwcancel.Text = "Go Back";
                    btnorderall.Text = "Order All";
                    btnGenerateOrder.Visible = false;
                    List<GetRwdlsafterordergeneration> lstrwpo = lclsservice.GetRwdlsafterordergeneration(Convert.ToString(ViewState["ReqID"])).ToList();
                    grdreviewreqpo.DataSource = lstrwpo;
                    grdreviewreqpo.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EmptyGridITOrderMessage.Replace("<<ITRequestPODescription>>", ""), true);
                }
                SearchGrid();
                ViewState["ReqID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message), true);
            }
        }

        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string ITNNO)
        {
            try
            {
                string Superadmin = string.Empty;
                List<GetSuperAdminDetails> lstsuperadmin = lclsservice.GetSuperAdminDetails(CorporateID, FacilityID).ToList();
                foreach (var value in lstsuperadmin)
                {
                    Superadmin += "<br/>" + value.UserName + "<br/>" + value.Email + "<br/>" + value.PhoneNo;
                }
                objemail.vendorEmailcontent = string.Format("Please see the attached document for order details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br />Thank you for service <br/><br/>" + Superadmin);

                objemail.vendoremailsubject = "Purchase Order – " + ITNNO;
                string displayfilename = "Purchase Order – " + ITNNO + ".pdf";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }


        public List<object> DetailsOrderReport(Int64 ITRequestMasterID)
        {
            List<object> llstarg = new List<object>();
            try
            {
                List<GetITROrderContentPO> llstreview = lclsservice.GetITROrderContentPO(ITRequestMasterID, defaultPage.UserId).ToList();
                rvITRequestPOreport.ProcessingMode = ProcessingMode.Local;
                rvITRequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITRequestPOpdfDS.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[1];
                p1[0] = new ReportParameter("ITRequestMasterID", Convert.ToString(ITRequestMasterID));
                this.rvITRequestPOreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("ITRequestPODSpdf", llstreview);
                rvITRequestPOreport.LocalReport.DataSources.Clear();
                rvITRequestPOreport.LocalReport.DataSources.Add(datasource);
                rvITRequestPOreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvITRequestPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                string FromEmail = llstreview[0].FromEmail;
                string ToEmail = llstreview[0].ToEmail;
                Int64 CorporateID = Convert.ToInt64(llstreview[0].CorporateID);
                Int64 FacilityID = Convert.ToInt64(llstreview[0].Facility);
                llstarg.Insert(0, FromEmail);
                llstarg.Insert(1, ToEmail);
                llstarg.Insert(2, bytes);
                llstarg.Insert(3, CorporateID);
                llstarg.Insert(4, FacilityID);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
            return llstarg;
        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string ITRequestID = string.Empty;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["RequestITMasterID"] = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                Print();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        public void Print()
        {
            try
            {
                string ITRequestID = string.Empty;
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                List<BindITRequestPOReport> llstreview = new List<BindITRequestPOReport>();
                if ((ViewState["RequestITMasterID"] == null) || (Convert.ToString(ViewState["RequestITMasterID"]) == ""))
                {
                    //SearchGrid();
                    if (grdreviewreqpo.Rows.Count != 0)
                    {
                        foreach (GridViewRow row in grdreviewreqpo.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                if (ITRequestID == string.Empty)
                                    ITRequestID = row.Cells[1].Text;
                                else
                                    ITRequestID = ITRequestID + "," + row.Cells[1].Text.Trim().Replace("&nbsp;", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (GridViewRow row in grdITRequestPO.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                if (ITRequestID == string.Empty)
                                    ITRequestID = row.Cells[1].Text;
                                else
                                    ITRequestID = ITRequestID + "," + row.Cells[1].Text.Trim().Replace("&nbsp;", "");
                            }
                        }

                    }
                    llstreview = lclsservice.BindITRequestPOReport(null, ITRequestID, defaultPage.UserId,defaultPage.UserId).ToList();
                }
                else
                {

                    ITRequestID = ViewState["RequestITMasterID"].ToString();
                    ITRequestID = ITRequestID.Replace(",", "");
                    llstreview = lclsservice.BindITRequestPOReport(ITRequestID, null, defaultPage.UserId,defaultPage.UserId).ToList();
                }
                rvitreqreviewrpt.ProcessingMode = ProcessingMode.Local;
                rvitreqreviewrpt.LocalReport.ReportPath = Server.MapPath("~/Reports/ITRequestPoReport.rdlc");
                string s = ITRequestID;               
                string q = "1";
                Int64 r = defaultPage.UserId;               
                ReportDataSource datasource = new ReportDataSource("ITRequestPoDS", llstreview);
                rvitreqreviewrpt.LocalReport.DataSources.Clear();
                rvitreqreviewrpt.LocalReport.DataSources.Add(datasource);
                rvitreqreviewrpt.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvitreqreviewrpt.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                _sessionPDFFileName = "ITOrder" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //ViewState["RequestITMasterID"] = null;
                //Print();

                if (drpcorsearch.SelectedValue == "All")
                {
                    objbalreqITPO.CorporateIDs = "ALL";
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

                    objbalreqITPO.CorporateIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objbalreqITPO.FacilityIDs = "ALL";
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
                    objbalreqITPO.FacilityIDs = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    objbalreqITPO.VendorIDs = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpvendorsearch.Items)
                    {
                        if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    objbalreqITPO.VendorIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    objbalreqITPO.Status = "ALL";
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
                    objbalreqITPO.Status = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalreqITPO.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objbalreqITPO.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                objbalreqITPO.LoggedinBy = Convert.ToInt64(defaultPage.UserId);
                List<SearchRequestITPO> lstMSRMaster = lclsservice.SearchRequestITPO(objbalreqITPO).ToList();
                rvitreqreviewrpt.ProcessingMode = ProcessingMode.Local;
                rvitreqreviewrpt.LocalReport.ReportPath = Server.MapPath("~/Reports/ITOrderSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetITOredrsummaryDS", lstMSRMaster);
                rvitreqreviewrpt.LocalReport.DataSources.Clear();
                rvitreqreviewrpt.LocalReport.DataSources.Add(datasource);
                rvitreqreviewrpt.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvitreqreviewrpt.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITOrderSummary" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void lbpopprint_Click(object sender, EventArgs e)
        {
            Print();
        }

        protected void lbitrno_Click(object sender, EventArgs e)
        {
            try
            {
                mpeitrreview.Show();
                string s = string.Empty;                
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;               
                if (grdreviewreqpo.Rows.Count != 0)
                    HdnITDetailID.Value = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                else
                    HdnITDetailID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                LinkButton lbrevitrno = (LinkButton)gvrow.FindControl("lbrevitrno");
                if (lbrevitrno != null)
                    mpereview.Show();
                List<GetRequestITMaster> lstITRMaster = lclsservice.GetRequestITMaster().Where(a => a.RequestITMasterID == Convert.ToInt64(HdnITDetailID.Value)).ToList();
                lblitrcor.Text = lstITRMaster[0].Corporate;
                lblitrfac.Text = lstITRMaster[0].Facility;
                lblitrven.Text = lstITRMaster[0].Vendor;
                lblship.Text = lstITRMaster[0].Shipping;
                lblreqtype.Text = lstITRMaster[0].Reequestype;
                lblreqdate.Text = Convert.ToString(lstITRMaster[0].CreatedOn);
                lblmprreview.Text = lstITRMaster[0].ITRNo;
                lablitrno.Text = lstITRMaster[0].ITRNo;
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetITRequestDetailsbyMasterID> llstITRDetails = lclsservice.GetITRequestDetailsbyMasterID(Convert.ToInt64(HdnITDetailID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();
                grdreview.DataSource = llstITRDetails;
                grdreview.DataBind();
                int i = 0;
                foreach (GridViewRow grdfs in grdreview.Rows)
                {
                    Label lblEquipCategory = (Label)grdfs.FindControl("lblequrecat");
                    Label lblEquipList = (Label)grdfs.FindControl("lblequrelst");
                    Label lblser = (Label)grdfs.FindControl("lblitrserno");
                    Label txtqty = (Label)grdfs.FindControl("lblrevqty");
                    Label txtppq = (Label)grdfs.FindControl("lblrevppqty");
                    Label txttotprice = (Label)grdfs.FindControl("lblTotalPrice");
                    Label txtreason = (Label)grdfs.FindControl("lblrevreason");
                    Label lblitruser = (Label)grdfs.FindControl("lblitruser");                   
                    lblEquipCategory.Text = llstITRDetails[i].EquipmentSubCategory;
                    lblEquipList.Text = llstITRDetails[i].EquipmentList;
                    lblser.Text = llstITRDetails[i].SerialNo;
                    txtqty.Text = llstITRDetails[i].OrderQuantity.ToString();
                    txtppq.Text = Convert.ToString(string.Format("{0:F2}", llstITRDetails[i].PriceperUnit));
                    txttotprice.Text = Convert.ToString(string.Format("{0:F2}", llstITRDetails[i].TotalPrice));
                    txtreason.Text = llstITRDetails[i].Reason.ToString();                   
                    i++;
                }
                Label lblshippingcost = grdreview.FooterRow.FindControl("lblshippingcost") as Label;
                Label lbltax = grdreview.FooterRow.FindControl("lbltax") as Label;
                Label lbltotalcost = grdreview.FooterRow.FindControl("lbltotalcost") as Label;
                lblshippingcost.Text = lstITRMaster[0].Shippingcost;
                lbltax.Text = lstITRMaster[0].Tax;
                lbltotalcost.Text = Convert.ToString(string.Format("{0:F2}", lstITRMaster[0].TotalCost));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }

        protected void lbitono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 ITRequestMasterID;
                string result = string.Empty;
                if (grdreviewreqpo.Rows.Count != 0)
                    ITRequestMasterID = Convert.ToInt64(gvrow.Cells[15].Text.Trim().Replace("&nbsp;", ""));
                else
                    ITRequestMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
                LinkButton lbrevitrno = (LinkButton)gvrow.FindControl("lbrevitrno");
                if (lbitrno != null)
                    result = lbitrno.Text;
                if (lbrevitrno != null)
                {
                    result = lbrevitrno.Text;
                    mpereview.Show();
                }

                var ITNo = "ITN" + result.Substring(2);
                List<object> llstresult = DetailsOrderReport(ITRequestMasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITOrder" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
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
                   
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message), true);
            }

        }

        protected void btnrevwcancel_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
            grdreviewreqpo.DataSource = null;
            grdreviewreqpo.DataBind();           
        }

        protected void imgrevprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 ITRequestMasterID;
                ITRequestMasterID = Convert.ToInt64(gvrow.Cells[15].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(ITRequestMasterID);
                byte[] bytes = (byte[])llstresult[2];                     
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITOrder" + guid + ".pdf";
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path);
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }

        protected void imgresend_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                string result = string.Empty;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
                result = lbitrno.Text;
                var ITNNo = "ITN" + result.Substring(3);
                Int64 ITRequestMasterID;
                ITRequestMasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(ITRequestMasterID);
                byte[] bytes = (byte[])llstresult[2];
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(llstresult[3].ToString());
                Int64 FacilityID = Convert.ToInt64(llstresult[4].ToString());
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                EmailSetting(CorporateID, FacilityID, ITNNo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITPOemailMessage.Replace("<<ITRequestPODescription>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message), true);
            }
        }

        protected void grdreviewreqpo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lblaction = (e.Row.FindControl("lblaction") as Label);
                    Label lblrevpostatus = (e.Row.FindControl("lblrevpostatus") as Label);
                    TextBox lblRemarks = (e.Row.FindControl("lblRemarks") as TextBox);
                    RequiredFieldValidator rfvRemarks = (e.Row.FindControl("rfvRemarks") as RequiredFieldValidator);
                    //Label lblaudit = (e.Row.FindControl("lblaudit") as Label);
                    //Image imgreadmore1 = (e.Row.FindControl("imgreadmore1") as Image);
                    DataTable dt = (DataTable)ViewState["OldStatus"];
                    string Status = string.Empty;
                    Status = lblrevpostatus.Text;
                    string Oldstatus = dt.Rows[e.Row.RowIndex]["Oldstatus"].ToString();
                    if ((Oldstatus == OldStatusPend && lblaction.Text == actionApprove) || (Oldstatus == OldStatusPend && lblaction.Text == actionOrder))
                    {
                        lblRemarks.Visible = false;                      
                    }
                    else if (Oldstatus == StatusApprove && lblaction.Text == actionOrder)
                    {
                        lblRemarks.Visible = false;                      
                    }
                    else
                    {                      
                        lblRemarks.Visible = true;                       
                    }
                    //if (lblaudit.Text.Length > 150)
                    //{
                    //    lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                    //    imgreadmore1.Visible = true;
                    //}
                    //else
                    //{
                    //    imgreadmore1.Visible = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }
        }
        private void ClearDetails()
        {
            drpcorsearch.ClearSelection();
            drpfacilitysearch.ClearSelection();
            drpvendorsearch.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            drpStatussearch.ClearSelection();
            //drpcorsearch.SelectedIndex = 0;
            //drpfacilitysearch.SelectedIndex = 0;
            BindCorporate();
            BindFacility();
            BindVendor();
            BindStatus("Add");          
            ViewState["ReqID"] = "";
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClearDetails();
            BindRequestPOGrid();
        }

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
                BindVendor();
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
                BindVendor();
            }

        }

        //Multi Select for dropdowns
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
                divMPRMaster.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divMPRMaster.Attributes["class"] = "mypanel-body";
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
                BindVendor();
                DivFacCorp.Style.Add("display", "none");
                divMPRMaster.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divMPRMaster.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divMPRMaster.Attributes["class"] = "Upopacity";
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
            BindVendor();
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

            foreach (ListItem lst in drpvendorsearch.Items)
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
                    divMPRMaster.Attributes["class"] = "Upopacity";
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

            foreach (ListItem lst in drpvendorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
        }
    }
}