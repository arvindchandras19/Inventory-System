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
using Inventory.Class;
using Inventory.Inventoryserref;
using Microsoft.Reporting.WebForms;
using System.Configuration;


namespace Inventory
{
    public partial class CapitalReceivingOrder : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        EmailController objemail = new EmailController();
        BALCapitalReceiving llstcr = new BALCapitalReceiving();
        string Actionvoid = Constant.Actionvoid;
        string Actionclosed = Constant.Actionclosed;
        string ActionPartial = Constant.ActionPartial;
        string Statusnonuser = Constant.Statusnonuser;
        string Statususer = Constant.Statususer;
        string BackOrderStatus = Constant.BackOrderStatus;
        string Statemail = Constant.Statemail;
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        Int32 POhdnrowcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.grdCRReceivingPO);
                scriptManager.RegisterPostBackControl(this.btnPrintAll);
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        BindCorporate();
                        BindFacility();
                        BindVendor();
                        BindStatus("Add");
                        lclsservice.SyncCapitalReceivingorder();
                        SearchCapitalReceiving();
                        BindReasonAction("ADD", "");
                        BindReason("Add");
                        Hdnrole.Value = Convert.ToString(defaultPage.RoleID);
                        if (defaultPage.CapitalReceiving_Edit == false && defaultPage.CapitalReceiving_View == true)
                        {
                            btnsave.Visible = false;
                        }
                        if (defaultPage.CapitalReceiving_Edit == false && defaultPage.CapitalReceiving_View == false)
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }
        #endregion

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
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //if (defaultPage.RoleID == 1)
                //{
                //    if (drpcorsearch.SelectedValue != "All")
                //    {
                //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                //        drpfacilitysearch.DataTextField = "FacilityDescription";
                //        drpfacilitysearch.DataValueField = "FacilityID";
                //        drpfacilitysearch.DataBind();
                //        drpfacilitysearch.Items.Insert(0, lst);
                //        drpfacilitysearch.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        drpfacilitysearch.SelectedIndex = 0;
                //    }
                //}
                //else
                //{
                //    if (drpcorsearch.SelectedValue != "All")
                //    {
                //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == drpcorsearch.SelectedItem.Text).ToList();
                //        drpfacilitysearch.DataTextField = "FacilityName";
                //        drpfacilitysearch.DataValueField = "FacilityID";
                //        drpfacilitysearch.DataBind();
                //        drpfacilitysearch.Items.Insert(0, lst);
                //        drpfacilitysearch.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        drpfacilitysearch.SelectedIndex = 0;
                //    }
                //}
                BindVendor();
            }
          
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
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
                    lstvendor = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => a.MachineParts == true).ToList();
                    drpvendorsearch.DataSource = lstvendor;
                    drpvendorsearch.DataTextField = "VendorDescription";
                    drpvendorsearch.DataValueField = "VendorID";
                    drpvendorsearch.DataBind();
                    //ListItem lst = new ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
                    //drpvendorsearch.Items.Insert(0, lst);
                    //drpvendorsearch.SelectedIndex = 0;
                }
                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }

            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }

        #endregion

        public void BindStatus(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("CapitalReceiving", "Status", mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatussearch.Items.Insert(0, lst);
                //drpStatussearch.SelectedIndex = 0;
                if (defaultPage.RoleID == 1)
                {
                    drpStatussearch.Items.FindByText(Statususer).Selected = true;
                }
                else
                {
                    drpStatussearch.Items.FindByText(Statusnonuser).Selected = true;
                    drpStatussearch.Items.FindByText(BackOrderStatus).Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }

        }

        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            if(defaultPage.RoleID==1)
            {
                Reqtxtinvoiceno.ErrorMessage = req;
                ReqdrpStatus.ErrorMessage = req;
                Reqtxtinvoicedate.ErrorMessage = req;
                ReqddlreceivingAct.ErrorMessage = req;
            }
            else
            {
                Reqpackingsilpno.ErrorMessage = req;
                Reqtxtpackingslip.ErrorMessage = req;
                Reqtxtreceived.ErrorMessage = req;
            }
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

        public void SearchCapitalReceiving()
        {
            try
            {
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstcr.ListCorporateID = "ALL";
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

                    llstcr.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstcr.ListFacilityID = "ALL";
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
                    llstcr.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstcr.ListVendorID = "ALL";
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
                    llstcr.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstcr.Status = "ALL";
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
                    llstcr.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstcr.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstcr.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }                
                llstcr.LoggedinBy = defaultPage.UserId;
                List<SearchCapitalReceivingMaster> lstCRMaster = lclsservice.SearchCapitalReceivingMaster(llstcr).ToList();
                grdCRReceivingPO.DataSource = lstCRMaster;
                grdCRReceivingPO.DataBind();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
               
        }

     
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchCapitalReceiving();
        }

        protected void lbcrono_Click(object sender, EventArgs e)  
        {
            try
            {
                modalreviewreq.Show();
                grdreview.DataSource = null;
                grdreview.DataBind();
                string status = string.Empty;
                Int64 CPO = 0;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lbcrono = (LinkButton)gvrow.FindControl("lbcrono");
                LinkButton lbCRrno = (LinkButton)gvrow.FindControl("lbCRrno");
                status = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                HdnMasterID.Value =gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                Hdnorderid.Value = gvrow.Cells[17].Text.Trim().Replace("&nbsp;", "");
                ViewState["CorporateID"] = Convert.ToString(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", ""));
                ViewState["FacilityID"] = Convert.ToString(gvrow.Cells[6].Text.Trim().Replace("&nbsp;", ""));
                CPO = Convert.ToInt64( HdnMasterID.Value);
                lblmprreview.Text = lbcrono.Text;
                Hdncpo.Value = lblmprreview.Text;
                HdnMReqID.Value = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                lblrcountro.Visible = false;
                lblrcountpo.Visible = true;
                List<GetCpoDetails> llstCRDetails = lclsservice.GetCpoDetails(CPO).ToList();
                grdreview.DataSource = llstCRDetails;
                grdreview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountpo.Text = "No of records : " + (llstCRDetails.Count - a);
                if (status == Statususer)
                {
                    if (llstCRDetails[0].PackingSlipNo != "")
                        packingsilpno.Text = llstCRDetails[0].PackingSlipNo;
                    if (Convert.ToString(llstCRDetails[0].PackingDate) != "")
                        txtpackingslip.Text = Convert.ToDateTime(llstCRDetails[0].PackingDate).ToString("MM/dd/yyyy");
                    if (Convert.ToString(llstCRDetails[0].ReceivedDate) != "")
                        txtreceived.Text = Convert.ToDateTime(llstCRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                    TextBox txtTotalcost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                    if (Convert.ToString(llstCRDetails[0].TotalCost) != "")
                        txtTotalcost.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[0].TotalCost));
                    int i = 0;
                    foreach (GridViewRow row in grdreview.Rows)
                    {
                        TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                        TextBox txtcomments = (TextBox)row.FindControl("txtcomments");
                        Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");

                        if (llstCRDetails[i].ReceivedQty != 0)
                            txtreceivedqty.Text = llstCRDetails[i].ReceivedQty.ToString();
                        if (llstCRDetails[i].Comments != "")
                            txtcomments.Text = llstCRDetails[i].Comments;
                        if (llstCRDetails[i].TotalPrice != 0)
                            lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].TotalPrice));

                        i++;
                        txtreceivedqty.Enabled = false;
                        txtcomments.Enabled = false;
                    }
                }

                TextBox txtsipcost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txttax = grdreview.FooterRow.FindControl("txttax") as TextBox;

                if (defaultPage.RoleID == 1)
                {
                        btnsave.Enabled = true;
                        if (status == BackOrderStatus)
                        {
                            BindReasonAction("Add", "TYPE2");
                        ddlreceivingAct.ClearSelection();
                        ddlreceivingAct.Enabled = false;
                        ddlreceivingAct.Items.FindByValue(ActionPartial).Selected = true;
                    }
                        else
                        {
                            BindReasonAction("Add", "TYPE3");
                        ddlreceivingAct.ClearSelection();
                        ddlreceivingAct.Enabled = false;
                        ddlreceivingAct.Items.FindByValue(Actionvoid).Selected = true;
                    }
                        ddlreason.Enabled = true;
                        //ddlreceivingAct.Enabled = true;
                        Reqddlreason.Enabled = true;
                        ReqddlreceivingAct.Enabled = true;
                        txtpackingslip.Enabled = false;
                        packingsilpno.Enabled = false;
                        txtreceived.Enabled = false;
                        txtinvoiceno.Enabled = false;
                        txtinvoicedate.Enabled = false;
                        Reqpackingsilpno.Enabled = false;
                        Reqtxtpackingslip.Enabled = false;
                        Reqtxtreceived.Enabled = false;
                        Reqtxtinvoiceno.Enabled = false;
                        Reqtxtinvoicedate.Enabled = false;
                        txtsipcost.Enabled = false;
                        txttax.Enabled = false;
                 }
                
                else
                {
                    if (status == Statususer)
                    {
                        btnsave.Enabled = false;
                        txtpackingslip.Enabled = false;
                        packingsilpno.Enabled = false;
                        txtreceived.Enabled = false;
                        txtsipcost.Enabled = false;
                        txttax.Enabled = false;
                        txtinvoiceno.Enabled = false;
                        txtinvoicedate.Enabled = false;
                        ddlreceivingAct.Enabled = false;
                        ddlreason.Enabled = false;
                    }
                    else
                    {
                        btnsave.Enabled = true;
                        txtpackingslip.Enabled = true;
                        packingsilpno.Enabled = true;
                        txtreceived.Enabled = true;
                        txtsipcost.Enabled = false;
                        txttax.Enabled = false;
                        txtinvoiceno.Enabled = false;
                        txtinvoicedate.Enabled = false;
                        ddlreceivingAct.Enabled = false;
                        ddlreason.Enabled = false;
                        Reqtxtinvoiceno.Enabled = false;
                        Reqtxtinvoicedate.Enabled = false;
                        ReqddlreceivingAct.Enabled = false;
                        Reqddlreason.Enabled = false;
                       
                    }
                }
            }
               
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }

        protected void grdreview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtreceivedqty = (TextBox)e.Row.FindControl("txtreceivedqty");
                TextBox txtcomments = (TextBox)e.Row.FindControl("txtcomments");
                Label lblOrderQuantity = (Label)e.Row.FindControl("lblOrderQuantity");

                if (defaultPage.RoleID == 1 || lblOrderQuantity.Text == "0")
                {
                    txtreceivedqty.Enabled = false;
                    txtcomments.Enabled = false;
                }
                if (lblOrderQuantity.Text == "0")
                {
                    e.Row.Style.Add("display", "none");
                    POhdnrowcount = POhdnrowcount + 1;
                }
            }
            HddGridCount.Value = Convert.ToString(POhdnrowcount);
        }
        
        public void BindReasonAction(string mode,string type)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("CapitalReceiving", "Action", mode).ToList();
                if(type=="TYPE1")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == Actionclosed);
                }
                else if(type == "TYPE2")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == ActionPartial);
                }
                else if(type == "TYPE3")
                {
                    ddlreceivingAct.DataSource = lstLookUp.Where(a => a.InvenValue == Actionvoid);
                }
                //ddlreceivingAct.DataSource = lstLookUp;
                ddlreceivingAct.DataTextField = "InvenValue";
                ddlreceivingAct.DataValueField = "InvenValue";
                ddlreceivingAct.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Action--";
                ddlreceivingAct.Items.Insert(0, lst);
                ddlreceivingAct.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }

        }

        public void clear()
        {
            packingsilpno.Text = "";
            txtpackingslip.Text = "";
            txtreceived.Text = "";
            txtinvoicedate.Text = "";
            txtinvoiceno.Text = "";
            txtreason.Text = "";
            lblerror.Text = "";
            ddlreceivingAct.ClearSelection();
            //ddlreceivingAct.SelectedIndex = 0;
            ddlreason.ClearSelection();
            ddlreason.SelectedIndex = 0;
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        public void BindReason(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("CapitalReceiving", "Reason", mode).ToList();
                ddlreason.DataSource = lstLookUp;
                ddlreason.DataTextField = "InvenValue";
                ddlreason.DataValueField = "InvenValue";
                ddlreason.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Reason--";
                ddlreason.Items.Insert(0, lst);
                ddlreason.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }

        }

        protected void ddlreason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlreason.Items.FindByText("Other").Selected == true)
            {
                otherid.Visible = true;
            }
            else
            {
                otherid.Visible = false;
            }
            modalreviewreq.Show();
        }

        public void Totalcost()
        {
            decimal sum = 0;
            for (int i = 0; i < grdreview.Rows.Count; ++i)
            {
                Label lblTotalPrice = (Label)grdreview.FindControl("lblTotalPrice");
                if(lblTotalPrice.Text != "")
                lblTotalPrice = grdreview.Rows[i].FindControl("lblTotalPrice") as Label;
                sum += Convert.ToDecimal(lblTotalPrice.Text);
            }
            //(grdreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
            (grdreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                modalreviewreq.Show();
                InsertReceiving();
                clear();
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }   
        }


        public void CalType()
        {
            try
            {
                decimal orterqty = 0;
                decimal receivedoqty = 0;
                decimal Total = 0;
                decimal orterqtytotal = 0;
                decimal receivedoqtytotal = 0;
               
                foreach (GridViewRow row in grdreview.Rows)
                {
                    Label lbloqty = (Label)row.FindControl("lblOrderQuantity");
                    TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                    orterqty = Convert.ToDecimal(lbloqty.Text);
                    if (txtreceivedqty.Text != "")
                        receivedoqty = Convert.ToDecimal(txtreceivedqty.Text);
                    orterqtytotal += orterqty;
                    if (txtreceivedqty.Text != "")
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

                //}
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }

        public void InsertReceiving()
        {
            try
            {
                modalreviewreq.Show();
                string a = string.Empty;
                string b = string.Empty;
                Int64 InertCodeID = 0;
                llstcr.CapitalReceivingMasterID = Convert.ToInt64(HdnMasterID.Value);
                llstcr.CapitalItemMasterID = Convert.ToInt64(HdnMReqID.Value);
                llstcr.CapitalOrderID = Convert.ToInt64(Hdnorderid.Value);
                llstcr.CPONo = Convert.ToString(Hdncpo.Value);
                if (packingsilpno.Text !="")
                llstcr.PackingSlipNo = packingsilpno.Text;
                if (txtpackingslip.Text != "")
                llstcr.PackingDate = Convert.ToDateTime(txtpackingslip.Text);
                if (txtreceived.Text != "")
                llstcr.ReceivedDate = Convert.ToDateTime(txtreceived.Text);
                if (txtinvoiceno.Text != "")
                llstcr.InvoiceNo = txtinvoiceno.Text;
                if (ddlreceivingAct.SelectedValue != "")
                llstcr.InvoiceStatus = ddlreceivingAct.SelectedValue;
                llstcr.InvoicedBy = defaultPage.UserId;
                if (txtinvoicedate.Text != "")
                llstcr.InvoiceDate = Convert.ToDateTime(txtinvoicedate.Text);
                if (ddlreceivingAct.SelectedValue != "")
                llstcr.ReceivingAction = ddlreceivingAct.SelectedValue;
                if (ddlreason.SelectedValue != "")
                llstcr.Reason = ddlreason.SelectedValue;
                ViewState["Reason"] = llstcr.Reason;
                if (txtreason.Text != "")
                llstcr.OtherReason = txtreason.Text;
                if (ddlreceivingAct.SelectedValue != "")
                    llstcr.FinalStatus = ddlreceivingAct.SelectedValue;
                llstcr.CreatedBy = defaultPage.UserId;
                CalType();
                if (ViewState["Type"] != null)
                llstcr.Type = Convert.ToString(ViewState["Type"]);
                llstcr.LoggedinBy = defaultPage.UserId;
                if (defaultPage.RoleID == 1)
                {
                    llstcr.Type = "TYPE3";
                }
                TextBox txtsipcost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txttax = grdreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalcost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                llstcr.ShippingCost = txtsipcost.Text;
                llstcr.Tax = txttax.Text;
                if (txtTotalcost.Text != "")
                llstcr.TotalCost = Convert.ToDecimal(txtTotalcost.Text);

                List<UpdateCapitalRecevingMaster> lstCRMaster = lclsservice.UpdateCapitalReceivingMaster(llstcr).ToList();
                InertCodeID = Convert.ToInt64(lstCRMaster[0].INSERTRECORDID);
             
               foreach (GridViewRow grdfs in grdreview.Rows)
               {
                   Label lblbalqty = (Label)grdfs.FindControl("lblbalqty");
                   Label lblOrderQuantity = (Label)grdfs.FindControl("lblOrderQuantity");
                   Label lblrevppqty = (Label)grdfs.FindControl("lblrevppqty");
                   TextBox txtreceivedqty = (TextBox)grdfs.FindControl("txtreceivedqty");
                   Label lblTotalPrice = (Label)grdfs.FindControl("lblTotalPrice");
                   TextBox txtcomments = (TextBox)grdfs.FindControl("txtcomments");

                   Int32 receivedqty = 0;
                   llstcr.INSERTRECORDID = InertCodeID;
                   if (txtreceivedqty.Text != "")
                       receivedqty = Convert.ToInt32(txtreceivedqty.Text);
                   llstcr.ReceivedQty = receivedqty;
                   if (lblOrderQuantity.Text != "")
                   llstcr.OrderQty = Convert.ToInt32(lblOrderQuantity.Text);
                   if (lblrevppqty.Text != "")
                   llstcr.PricePerQty = Convert.ToDecimal(lblrevppqty.Text);
                   Int32 Balance = 0;
                   Balance = (Convert.ToInt32(lblOrderQuantity.Text) - Convert.ToInt32(receivedqty));
                   decimal TotalPrice = 0;
                   TotalPrice = (Convert.ToDecimal(lblrevppqty.Text) * Convert.ToInt32(receivedqty));
                   llstcr.BalenceQty = Balance;
                   llstcr.TotalPrice = TotalPrice;
                   llstcr.Comments = txtcomments.Text;
                   Label lbCRMasterID = (Label)grdfs.FindControl("lbCRMasterID");
                   llstcr.CapitalReceivingDetailsID = Convert.ToInt64(lbCRMasterID.Text);

                   b = lclsservice.UpdateCapitalReceivingDetails(llstcr);

                   txtreceivedqty.Text = "";
                   txtcomments.Text = "";
               }

               if (ddlreceivingAct.SelectedValue == Actionvoid || llstcr.FinalStatus == Actionvoid)
               {
                   SendEmail();
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceiveemailMessage.Replace("<<MajorItemReceiving>>", ""), true);
                   modalreviewreq.Hide();
               }
               else
               {
                   if (b == "Updated Successfully")
                   {
                       ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingSaveMessage.Replace("<<MajorItemReceiving>>", ""), true);
                       ViewState["Type"] = "";
                       modalreviewreq.Hide();
                   }
               }

             SearchCapitalReceiving();
            }   
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            clear();
            modalreviewreq.Hide();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            BindCorporate();
            BindFacility();
            BindVendor();
            BindStatus("Add");
            BindStatus("Add");
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            SearchCapitalReceiving();
        }

        protected void grdCRReceivingPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                LinkButton lbcrono = (LinkButton)e.Row.FindControl("lbcrono");
                LinkButton lbCRrno = (LinkButton)e.Row.FindControl("lbCRrno");
                Image imgprint = (Image)e.Row.FindControl("imgprint"); 
                string status = e.Row.Cells[14].Text;
                //if (lblRemarks.Text.Length > 150)
                //{
                //    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                //    imgreadmore.Visible = true;
                //}
                //else
                //    {
                //    imgreadmore.Visible = false;
                //    }

                //if (lblaudit.Text.Length > 150)
                //{
                //    lblaudit.Text = lblaudit.Text.Substring(0, 150) + "....";
                //    imgreadmore1.Visible = true;
                //}
                //    else
                //    {
                //    imgreadmore1.Visible = false;
                //    }

                if (defaultPage.RoleID == 1)
                {
                    if (lbCRrno.Text == "")
                    {
                        lbcrono.Enabled = true;
                        imgprint.Enabled = false;
                    }
                    else
                    {
                        lbcrono.Enabled = false;
                        imgprint.Enabled = true;
                    }
                }

                else
                {
                    lbCRrno.Enabled = false;

                    if (lbCRrno.Text == "")
                    {
                        imgprint.Enabled = false;
                    }
                    else
                    {
                        imgprint.Enabled = true;
                    }
                }

                if (status == Actionvoid  || status == ActionPartial)
                {
                    lbcrono.Enabled = false;
                }
            }
        }

        protected void lbCRrno_Click(object sender, EventArgs e)
        {
            try
            {
                modalreviewreq.Show();
                Int64 CPRO = 0;
                Int32 receiveqty = 0;
                string status = string.Empty;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                LinkButton lbCRrno = (LinkButton)gvrow.FindControl("lbCRrno");
                HdnMasterID.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                Hdnorderid.Value = gvrow.Cells[17].Text.Trim().Replace("&nbsp;", "");
                CPRO = Convert.ToInt64(HdnMasterID.Value);
                HdnMReqID.Value = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                lblmprreview.Text = lbCRrno.Text;
                status = gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "");
                List<GetCPROMasterReview> llstCRDetails = lclsservice.GetCPROMasterReview(CPRO).ToList();
                if (llstCRDetails[0].PackingSlipNo != "")
                    packingsilpno.Text = llstCRDetails[0].PackingSlipNo;
                if (Convert.ToString(llstCRDetails[0].PackingDate) != "")
                    txtpackingslip.Text = Convert.ToDateTime(llstCRDetails[0].PackingDate).ToString("MM/dd/yyyy");
                if (Convert.ToString(llstCRDetails[0].ReceivedDate) != "")
                    txtreceived.Text = Convert.ToDateTime(llstCRDetails[0].ReceivedDate).ToString("MM/dd/yyyy");
                if (llstCRDetails[0].ReceivedQty != 0)
                    receiveqty = Convert.ToInt32(llstCRDetails[0].ReceivedQty);
                ViewState["Receive"] = receiveqty;
                packingsilpno.Enabled = false;
                txtpackingslip.Enabled = false;
                txtreceived.Enabled = false;
                ddlreason.Enabled = false;
                if (status == Actionclosed)
                {
                    if (Convert.ToString(llstCRDetails[0].InvoiceDate) != "")
                    txtinvoicedate.Text = Convert.ToDateTime(llstCRDetails[0].InvoiceDate).ToString("MM/dd/yyyy");
                    if (llstCRDetails[0].InvoiceNo != "")
                    txtinvoiceno.Text = llstCRDetails[0].InvoiceNo;
                    if (llstCRDetails[0].ReceivingAction !="0")
                    ddlreceivingAct.SelectedItem.Text = llstCRDetails[0].ReceivingAction;
                    if (llstCRDetails[0].Reason != "0")
                    ddlreason.SelectedItem.Text = llstCRDetails[0].Reason;
                    if (llstCRDetails[0].OtherReason != "")
                    txtreason.Text = llstCRDetails[0].OtherReason;
                    txtinvoicedate.Enabled = false;
                    txtinvoiceno.Enabled = false;
                    ddlreceivingAct.Enabled = false;
                    ddlreason.Enabled = false;
                    txtreason.Enabled = false;
                    btnsave.Enabled = false;
                }
                else
                {
                    txtinvoicedate.Enabled = true;
                    txtinvoiceno.Enabled = true;
                    ddlreceivingAct.Enabled = true;
                    ddlreason.Enabled = false;
                    txtreason.Enabled = true;
                    btnsave.Enabled = true;
                    Reqtxtinvoiceno.Enabled = true;
                    Reqtxtinvoicedate.Enabled = true;
                    ReqddlreceivingAct.Enabled = true;
                    Reqddlreason.Enabled = false;
                }

                lblrcountro.Visible = true;
                lblrcountpo.Visible = false;
                grdreview.DataSource = llstCRDetails;
                grdreview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountro.Text = "No of records : " + (llstCRDetails.Count - a);
                //CalType();
                //string WF = Convert.ToString(ViewState["Type"]);
                //if (WF != "" || WF == null)
                //    BindReasonAction("Add", WF);

                BindReasonAction("Add", "TYPE1");
                ddlreceivingAct.ClearSelection();
                ddlreceivingAct.Enabled = false;
                ddlreceivingAct.Items.FindByValue(Actionclosed).Selected = true;
                int i = 0;
                foreach (GridViewRow row in grdreview.Rows)
                {
                    Label lblRowNumber = (Label)row.FindControl("lblRowNumber");
                    Label lblequrecat = (Label)row.FindControl("lblequrecat");
                    Label lblequrelst = (Label)row.FindControl("lblequrelst");
                    Label lblser = (Label)row.FindControl("lblser");
                    Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                    Label lblOrderQuantity = (Label)row.FindControl("lblOrderQuantity");
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtreceivedqty = (TextBox)row.FindControl("txtreceivedqty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    TextBox txtcomments = (TextBox)row.FindControl("txtcomments");

                    lblequrecat.Text = llstCRDetails[i].EquimentSubCategory;
                    lblequrelst.Text = llstCRDetails[i].EquipmentList;
                    lblser.Text = llstCRDetails[i].SerialNo.ToString();
                    lblrevppqty.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].PricePerQty));
                    lblOrderQuantity.Text = llstCRDetails[i].OrderQty.ToString();
                    lblbalqty.Text = llstCRDetails[i].BalanceQty.ToString();
                    txtreceivedqty.Text = llstCRDetails[i].ReceivedQty.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].TotalPrice));
                    txtcomments.Text = llstCRDetails[i].Comments.ToString();
                    i++;
                    txtreceivedqty.Enabled = false;
                    txtcomments.Enabled = false;
                }

                TextBox txtShippingCost = grdreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txtTax = grdreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalCost = grdreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                txtShippingCost.Text = llstCRDetails[0].ShippingCost;
                txtTax.Text = llstCRDetails[0].Tax;
                txtTotalCost.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[0].TotalCost));
                Divsupad.Visible = true;
                grdreview.FooterRow.Visible = true;
            }
          
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }


        public List<object> SearchOrderReport(string CapitalItemMasterID)
        {
                string smedmasterIds = string.Empty;
                List<object> llstarg = new List<object>();
                List<GetCapitalPOReport> llstreview = new List<GetCapitalPOReport>();
                if (CapitalItemMasterID == "")
                {
                    foreach (GridViewRow row in grdCRReceivingPO.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (smedmasterIds == string.Empty)
                                smedmasterIds = row.Cells[1].Text;
                            else
                                smedmasterIds = smedmasterIds + "," + row.Cells[1].Text;
                        }
                    }

                    llstreview = lclsservice.GetCapitalPOReport(null, smedmasterIds, defaultPage.UserId,defaultPage.UserId).ToList();
                }
                else
                {
                    llstreview = lclsservice.GetCapitalPOReport(CapitalItemMasterID, null, defaultPage.UserId, defaultPage.UserId).ToList();
                }
                //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
                rvCapitalPoreportReview.ProcessingMode = ProcessingMode.Local;
                rvCapitalPoreportReview.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalReceivePDF.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[3];
                p1[0] = new ReportParameter("CapitalItemMasterID", "0");
                p1[1] = new ReportParameter("SearchFilters", "test");
                p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

                this.rvCapitalPoreportReview.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("CapitalReceivePdf", llstreview);
                rvCapitalPoreportReview.LocalReport.DataSources.Clear();
                rvCapitalPoreportReview.LocalReport.DataSources.Add(datasource);
                rvCapitalPoreportReview.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = null;
                bytes = rvCapitalPoreportReview.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                llstarg.Insert(0, bytes);
                return llstarg;
            
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
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }

        }

         public List<object> DetailsOrderReport(Int64 CapitalItemMasterID)
           {
                List<object> llstarg = new List<object>();
            BALCapitalPO llstcrpo = new BALCapitalPO();
            llstcrpo.CapitalItemMasterID = CapitalItemMasterID;
            llstcrpo.LoggedinBy = defaultPage.UserId;
                List<GetCROrderContentPO> llstreview = lclsservice.GetCROrderContentPO(llstcrpo).ToList();
                objemail.CorporateEmail = llstreview[0].FromEmail;
                objemail.vendorContactEmail = llstreview[0].ToEmail;
                objemail.UserName = llstreview[0].OrderBy;
                objemail.UserEmail = llstreview[0].UserEmail;
                objemail.UserPhoneNo = llstreview[0].UserPhoneNo;
                string CPONO = Hdncpo.Value;
                CPONO = llstreview[0].CRONo;
                rvCapitalPoreportReview.ProcessingMode = ProcessingMode.Local;
                rvCapitalPoreportReview.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalReceivePDF.rdlc");
                ReportParameter[] p1 = new ReportParameter[1];
                p1[0] = new ReportParameter("CapitalItemMasterID", Convert.ToString(CapitalItemMasterID));
                this.rvCapitalPoreportReview.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("CapitalReceivePdf", llstreview);
                rvCapitalPoreportReview.LocalReport.DataSources.Clear();
                rvCapitalPoreportReview.LocalReport.DataSources.Add(datasource);
                rvCapitalPoreportReview.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = null;
                bytes = rvCapitalPoreportReview.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                string FromEmail = llstreview[0].FromEmail;
                string ToEmail = llstreview[0].ToEmail;
                Int64 CorporateID = Convert.ToInt64(llstreview[0].CorporateID);
                Int64 FacilityID = Convert.ToInt64(llstreview[0].FacilityID);
                string CPONo = llstreview[0].CRONo;
                llstarg.Insert(0, FromEmail);
                llstarg.Insert(1, ToEmail);
                llstarg.Insert(2, bytes);
                llstarg.Insert(3, CorporateID);
                llstarg.Insert(4, FacilityID);
                llstarg.Insert(5, CPONo);
                return llstarg;
          }
        public void SendEmail()
        {
            try
            {
                //byte[] bytes = DetailsOrderReport(Convert.ToInt64(HdnMReqID.Value));
                Int64 CRMasterID = Convert.ToInt64(HdnMReqID.Value);
                List<object> llstresult = DetailsOrderReport(CRMasterID);
                byte[] bytes = (byte[])llstresult[2];
                 llstcr.OrderContent = bytes;
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(ViewState["CorporateID"]);
                Int64 FacilityID = Convert.ToInt64(ViewState["FacilityID"]);
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                string CPONo = Hdncpo.Value;
                EmailSetting(CorporateID, FacilityID, CPONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }


        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string CPONo)
        {
            try
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
                    objemail.vendorEmailcontent = string.Format("Due to " + reason + " " + CPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>"  + Superadmin);
                }
                else
                {
                    objemail.vendorEmailcontent = string.Format("We are cancelling " + CPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>"    + Superadmin);
                }
                objemail.vendoremailsubject = " Major Item Order – " + CPONo;
                string displayfilename = "Major Item Order – " + CPONo + ".pdf";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message.ToString()), true);
            }
        }


        protected void imgprintedit_Click(object sender, ImageClickEventArgs e)
        {
             try
            {
                string ITRONo = string.Empty;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                HdnMReqID.Value = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                HdnMasterID.Value = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                List<BindCapitalDetailsReport> llstreview = lclsservice.BindCapitalDetailsReport(Convert.ToInt64(HdnMReqID.Value),Convert.ToInt64(HdnMasterID.Value), Convert.ToInt64(defaultPage.UserId), "").ToList();
                //List<BindCapitalReceivingDetailsSubReport> llstsubreview = lclsservice.BindCapitalReceivingDetailsSubReport(Convert.ToInt64(HdnMReqID.Value), Convert.ToInt64(defaultPage.UserId), "").ToList();
                rvCapitalPodetails.ProcessingMode = ProcessingMode.Local;
                rvCapitalPodetails.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalReceivingDetailsReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("CapitalDetailsReport", llstreview);
                //ReportDataSource datasourcesub = new ReportDataSource("CapitalReceivingDetailsSubReport", llstsubreview);
                rvCapitalPodetails.LocalReport.DataSources.Clear();
                rvCapitalPodetails.LocalReport.DataSources.Add(datasource);
                //rvCapitalPodetails.LocalReport.DataSources.Add(datasourcesub);
                rvCapitalPodetails.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvCapitalPodetails.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "CapitalReceivingDetails" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            SearchCapitalReceiving();
        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            LinkButton lbCRrno = (LinkButton)gvrow.FindControl("lbCRrno");
            Int64 CapitalOrderID = 0;
            CapitalOrderID = Convert.ToInt64(gvrow.Cells[17].Text.Trim().Replace("&nbsp;", ""));
            List<BindCapitalReceivingsummaryReport> llstreview = lclsservice.BindCapitalReceivingsummaryReport(Convert.ToInt64(CapitalOrderID), Convert.ToInt64(defaultPage.UserId), "").ToList();
            rvCapitalPoSummary.ProcessingMode = ProcessingMode.Local;
            rvCapitalPoSummary.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalReceivingSummaryReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("CapitalReceivingSummary", llstreview);
            rvCapitalPoSummary.LocalReport.DataSources.Clear();
            rvCapitalPoSummary.LocalReport.DataSources.Add(datasource);
            rvCapitalPoSummary.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvCapitalPoSummary.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "CapitalReceivingOrder" + guid + ".pdf";
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
                divMPOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divMPOrder.Attributes["class"] = "mypanel-body";
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
                divMPOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divMPOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divMPOrder.Attributes["class"] = "Upopacity";
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
                    divMPOrder.Attributes["class"] = "Upopacity";
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

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstcr.ListCorporateID = "ALL";
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

                    llstcr.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstcr.ListFacilityID = "ALL";
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
                    llstcr.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    llstcr.ListVendorID = "ALL";
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
                    llstcr.ListVendorID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstcr.Status = "ALL";
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
                    llstcr.Status = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstcr.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstcr.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstcr.LoggedinBy = defaultPage.UserId;
                List<SearchCapitalReceivingSummaryReport> lstCRMaster = lclsservice.SearchCapitalReceivingSummaryReport(llstcr).ToList();
                rvCapitalPoPrintAll.ProcessingMode = ProcessingMode.Local;
                rvCapitalPoPrintAll.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalReceivingSummaryPrintAllReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("CapitalReceivingSummaryPrintAllDS", lstCRMaster);
                rvCapitalPoPrintAll.LocalReport.DataSources.Clear();
                rvCapitalPoPrintAll.LocalReport.DataSources.Add(datasource);
                rvCapitalPoPrintAll.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvCapitalPoPrintAll.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MachinePartsReceivingOrderSummary" + guid + ".pdf";
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


                //List<object> llstresult = SearchOrderReport("");
                //byte[] bytes = (byte[])llstresult[0];
                //Guid guid = Guid.NewGuid();
                //_sessionPDFFileName = "SessionFile" + guid + ".pdf";
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                //path = Path.Combine(path, _sessionPDFFileName);
                //using (StreamWriter sw = new StreamWriter(File.Create(path)))
                //{
                //    sw.Write("");
                //}
                //FileStream fs = new FileStream(path, FileMode.Open);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();
                //ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalReceivingErrorMessage.Replace("<<MajorItemReceiving>>", ex.Message.ToString()), true);
            }
        }
    }
}