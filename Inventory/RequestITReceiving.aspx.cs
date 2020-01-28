using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using Inventory.Class;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Text;
namespace Inventory
{
    public partial class RequestITReceiving : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALRequestITReceiving objITReceiving = new BALRequestITReceiving();
        Page_Controls defaultPage = new Page_Controls();
        string StatusReceivingOrder = Constant.ReceivingStatus;
        string StatusOrder = Constant.OrderStatus;
        string StatusBackOrder = Constant.BackOrderStatus;
        string PartialOrderStatus = Constant.PartialOrderStatus;
        string VoidOrderStatus = Constant.VoidOrderStatus;
        string CloseOrderStatus = Constant.CloseOrderStatus;
        string Actionvoid = Constant.Actionvoid;
        string Actionclosed = Constant.Actionclosed;
        string ActionPartial = Constant.ActionPartial;
        string BackOrderStatus = Constant.BackOrderStatus;
        string StatusClosed = Constant.StatusClosed;
        string StatusOrderedPendingReceive = Constant.StatusOrderedPendingReceive;
        string StatusReceivedPendingInvoice = Constant.StatusReceivedPendingInvoice;
        string StatusVoidOrder = Constant.StatusVoidOrder;
        private string _sessionPDFFileName;
        EmailController objemail = new EmailController();
        string FinalString = "";
        StringBuilder SB = new StringBuilder();

        Int32 POhdnrowcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.grdITRequestReceiving);
            scriptManager.RegisterPostBackControl(this.btnPrintAll);
            if (!IsPostBack)
            {

                if (defaultPage != null)
                {

                    BindCorporate();
                    BindFacility(1, "Add");
                    BindVendor(1, "Add");
                    BindStatus("Add");
                    lclsservice.SynITReceivingorder();
                    BindReceivingGrid();

                    Hdnrole.Value = Convert.ToString(defaultPage.RoleID);
                    if (defaultPage.RoleID == 1)
                    {
                        foreach (GridViewRow row in grdITRequestReceiving.Rows)
                        {
                            LinkButton lbitono = (LinkButton)row.FindControl("lbitono");
                            lbitono.Enabled = false;
                        }
                    }
                    if (defaultPage.ITReceiving_Edit == false && defaultPage.ITReceiving_View == true)
                    {
                        btnitnnosave.Visible = false;
                    }
                    if (defaultPage.ITReceiving_Edit == false && defaultPage.ITReceiving_View == false)
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

        /// <summary>
        /// Bind the Corporate details to dropdown control 
        /// </summary>
        #region Bind Corporate Values
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstfacility = new List<BALUser>();
                if (defaultPage.RoleID == 1)
                {
                    lstfacility = lclsservice.GetCorporateMaster().ToList();

                    // Search Drop Down
                    drpcorsearch.DataSource = lstfacility;
                    drpcorsearch.DataTextField = "CorporateName";
                    drpcorsearch.DataValueField = "CorporateID";
                    drpcorsearch.DataBind();
                }
                else
                {
                    lstfacility = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcorsearch.DataSource = lstfacility.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
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
        private void BindFacility(int Search, string mode)
        {
            try
            {
                if (defaultPage.RoleID == 1)
                {
                    if (Search == 1)
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
                    }
                }
                else
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
                }
                BindVendor(1, "Add");
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
        public void BindVendor(int Search, string mode)
        {
            try
            {
                List<GetVendorByFacilityID> lstvendordetails = new List<GetVendorByFacilityID>();
                if (Search == 1)
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
                        lstvendordetails = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => (a.IT == true)).Distinct().ToList();
                        drpvendorsearch.DataSource = lstvendordetails;
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
                else
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
                        lstvendordetails = lclsservice.GetVendorByFacilityID(FinalString, defaultPage.UserId).Where(a => (a.IT == true)).Distinct().ToList();
                        drpvendorsearch.DataSource = lstvendordetails;
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
                lstLookUp = lclsservice.GetList("ReceivingITRequest", "Status", mode).ToList();
                // Search Status Drop Down
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                if (defaultPage.RoleID == 1)
                {
                    drpStatussearch.Items.FindByText(StatusReceivingOrder).Selected = true;
                }
                else
                {
                    drpStatussearch.Items.FindByText(StatusOrder).Selected = true;
                    drpStatussearch.Items.FindByText(BackOrderStatus).Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }


        public void BindReason(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("ReceivingITRequest", "Reason", mode).ToList();
                drpitnnoreason.DataSource = lstLookUp;
                drpitnnoreason.DataTextField = "InvenValue";
                drpitnnoreason.DataValueField = "InvenValue";
                drpitnnoreason.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select Reason";
                drpitnnoreason.Items.Insert(0, lst);
                drpitnnoreason.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }


        public void BindAction(string mode, string type)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("ReceivingITRequest", "Action", mode).ToList();
                if (type == "TYPE1")
                {
                    drpitnnorecaction.DataSource = lstLookUp.Where(a => a.InvenValue == Actionclosed);
                }
                else if (type == "TYPE2")
                {
                    drpitnnorecaction.DataSource = lstLookUp.Where(a => a.InvenValue == ActionPartial);
                }
                else if (type == "TYPE3")
                {
                    drpitnnorecaction.DataSource = lstLookUp.Where(a => a.InvenValue == Actionvoid);
                }
                drpitnnorecaction.DataTextField = "InvenValue";
                drpitnnorecaction.DataValueField = "InvenValue";
                drpitnnorecaction.DataBind();
                drpitnnorecaction.Items.Insert(0, "Select Action");
                drpitnnorecaction.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
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
                BindFacility(1,"Add");
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
                BindFacility(1,"Add");
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
                BindVendor(1,"Add");
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
                BindVendor(1, "Add");
            }

        }
        #endregion

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
        public void BindReceivingGrid()
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
        public void SearchGrid()
        {
            try
            {

                BALRequestITReceiving lstITRe = new BALRequestITReceiving();
                if (drpcorsearch.SelectedValue == "All")
                {
                    lstITRe.CorporateIDs = "ALL";
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

                    lstITRe.CorporateIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    lstITRe.FacilityIDs = "ALL";
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
                    lstITRe.FacilityIDs = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    lstITRe.VendorIDs = "ALL";
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
                    lstITRe.VendorIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    lstITRe.FinalStatus = "ALL";
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
                    lstITRe.FinalStatus = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                    lstITRe.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                else
                {
                    lstITRe.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    lstITRe.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                else
                {
                    lstITRe.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                lstITRe.LoggedinBy = Convert.ToInt64(defaultPage.UserId);
                List<SearchITReceiving> lstMSRMaster = lclsservice.SearchITReceiving(lstITRe).ToList();
                grdITRequestReceiving.DataSource = lstMSRMaster;
                grdITRequestReceiving.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITRequestPOErrorMessage.Replace("<<ITRequestPODescription>>", ex.Message.ToString()), true);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            BindCorporate();
            BindFacility(1, "Add");
            BindVendor(1, "Add");
            BindStatus("Add");
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            SearchGrid();
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

        protected void lbitrno_Click(object sender, EventArgs e)
        {
            if (defaultPage.RoleID == 1)
            {
                mpeitnno.Show();
                BindReason("Add");
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label lblpostatus = (Label)gvrow.FindControl("lblpostatus");
                LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
                ViewState["ITRequestMasterID"] = Convert.ToInt64(gvrow.Cells[2].Text.Replace("&nbsp;", ""));
                ViewState["ITReceivingMasterID"] = Convert.ToInt64(gvrow.Cells[17].Text);
                ViewState["ITRequestOrderID"] = Convert.ToInt64(gvrow.Cells[16].Text);
                List<BindITNNOvalues> lstitnnno = lclsservice.BindITNNOvalues(Convert.ToInt64(gvrow.Cells[17].Text)).ToList();
                string ITRONo = string.Empty;
                ITRONo = lbitrno.Text;
                lblMasterNo.Text = ITRONo;
                ViewState["ITNNo"] = lstitnnno[0].ITNNo;
                if (lstitnnno[0].PackingSlipNo != "")
                    txtpckslipno.Text = lstitnnno[0].PackingSlipNo;
                if (Convert.ToString(lstitnnno[0].PackingDate) != "")
                    txtpckslipdate.Text = Convert.ToDateTime(lstitnnno[0].PackingDate).ToString("MM/dd/yyyy");
                if (Convert.ToString(lstitnnno[0].ReceivedDate) != "")
                    txtreceiveddate.Text = Convert.ToDateTime(lstitnnno[0].ReceivedDate).ToString("MM/dd/yyyy");
                txtpckslipno.Enabled = false;
                txtpckslipdate.Enabled = false;
                txtreceiveddate.Enabled = false;
                BindAction("Add", "TYPE1");
                lblrcountpo.Visible = false;
                lblrcountro.Visible = true;
                gvitnreview.DataSource = lstitnnno;
                gvitnreview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountro.Text = "No of records : " + (lstitnnno.Count - a);
                int i = 0;
                foreach (GridViewRow row in gvitnreview.Rows)
                {
                    Label lblequrecat = (Label)row.FindControl("lblequrecat");
                    Label lblequrelst = (Label)row.FindControl("lblequrelst");
                    Label lblitrserno = (Label)row.FindControl("lblitrserno");
                    Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                    Label lblorderqty = (Label)row.FindControl("lblorderqty");
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    TextBox txtcmts = (TextBox)row.FindControl("txtcmts");

                    lblequrecat.Text = lstitnnno[i].EquimentSubCategory;
                    lblequrelst.Text = lstitnnno[i].EquipmentList;
                    lblitrserno.Text = lstitnnno[i].SerialNo.ToString();
                    lblrevppqty.Text = Convert.ToString(string.Format("{0:F2}", lstitnnno[i].PricePerQty));
                    lblorderqty.Text = lstitnnno[i].OrderQty.ToString();
                    lblbalqty.Text = lstitnnno[i].BalanceQty.ToString();
                    txtrcdqty.Text = lstitnnno[i].ReceivingQty.ToString();
                    lblTotalPrice.Text = Convert.ToString(string.Format("{0:F2}", lstitnnno[i].TotalPrice));
                    if (lstitnnno[i].Comments != null)
                        txtcmts.Text = lstitnnno[i].Comments.ToString();
                    i++;
                }

                TextBox txtsipcost = gvitnreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txttax = gvitnreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalcost = gvitnreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                if (lstitnnno[0].ShippingCost != "")
                    txtsipcost.Text = lstitnnno[0].ShippingCost;
                if (lstitnnno[0].Tax != "")
                    txttax.Text = lstitnnno[0].Tax;
                if (Convert.ToString(lstitnnno[0].TotalCost) != "")
                    txtTotalcost.Text = Convert.ToString(string.Format("{0:F2}", lstitnnno[0].TotalCost));
                Divsupad.Visible = true;
                divnonsuperadmin.Visible = true;
                gvitnreview.FooterRow.Visible = true;
                if (lblpostatus.Text == StatusClosed)
                {
                    txtpckslipno.Enabled = false;
                    txtpckslipdate.Enabled = false;
                    txtreceiveddate.Enabled = false;
                    txtinvoicedate.Enabled = false;
                    drpitnnorecaction.Enabled = false;
                    drpitnnoreason.Enabled = false;
                    txtinvoiceno.Enabled = false;
                    gvitnreview.FooterRow.Enabled = false;
                    txtinvoiceno.Text = lstitnnno[0].InvoiceNo;
                    if (lstitnnno[0].InvoiceDate != null)
                        txtinvoicedate.Text = Convert.ToDateTime(lstitnnno[0].InvoiceDate).ToString("MM/dd/yyyy");
                    if (lstitnnno[0].ReceivingAction != null)
                        drpitnnorecaction.SelectedItem.Text = lstitnnno[0].ReceivingAction;
                    if (lstitnnno[0].Reason != "" && lstitnnno[0].Reason != "0")
                        drpitnnoreason.SelectedItem.Text = lstitnnno[0].Reason;
                    btnitnnosave.Visible = false;
                }
                if (lblpostatus.Text == StatusReceivedPendingInvoice)
                {
                    txtinvoiceno.Enabled = true;
                    txtinvoicedate.Enabled = true;
                    drpitnnorecaction.ClearSelection();
                    drpitnnorecaction.Enabled = false;
                    drpitnnorecaction.Items.FindByValue(Actionclosed).Selected = true;
                    drpitnnoreason.Enabled = false;
                    reqreason.Enabled = false;
                    txtinvoiceno.Text = "";
                    txtinvoicedate.Text = "";
                    btnitnnosave.Visible = true;
                }

            }

        }

        protected void lbitono_Click(object sender, EventArgs e)
        {
            try
            {
                mpeitnno.Show();
                gvitnreview.DataSource = null;
                gvitnreview.DataBind();
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["ITRequestMasterID"] = Convert.ToInt64(gvrow.Cells[2].Text.Replace("&nbsp;", ""));
                ViewState["ITRequestOrderID"] = Convert.ToInt64(gvrow.Cells[16].Text);
                LinkButton lbitono = (LinkButton)gvrow.FindControl("lbitono");
                LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
                string ITNNO = string.Empty;
                ITNNO = lbitono.Text;
                ViewState["ITNNo"] = ITNNO;
                BindReason("Add");
                ViewState["ITReceivingMasterID"] = Convert.ToInt64(gvrow.Cells[17].Text);
                lblrcountpo.Visible = true;
                lblrcountro.Visible = false;
                List<BindITNNOvalues> lstitnnno = lclsservice.BindITNNOvalues(Convert.ToInt64(gvrow.Cells[17].Text)).ToList();
                Label lblpostatus = (Label)gvrow.FindControl("lblpostatus");
                gvitnreview.DataSource = lstitnnno;
                gvitnreview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountpo.Text = "No of records : " + (lstitnnno.Count - a);
                BindAction("Add", "");
                lblMasterNo.Text = ITNNO;
                if (lbitrno.Text != "")
                {
                    Reviewfields();

                }
                else
                {
                    txtinvoiceno.Enabled = false;
                    txtinvoicedate.Enabled = false;
                }


                if (defaultPage.RoleID == 1)
                {
                    Divsupad.Visible = true;
                    divnonsuperadmin.Visible = true;
                    txtpckslipno.Text = "";
                    txtpckslipdate.Text = "";
                    txtreceiveddate.Text = "";
                    txtpckslipno.Enabled = false;
                    txtpckslipdate.Enabled = false;
                    txtreceiveddate.Enabled = false;
                    gvitnreview.FooterRow.Enabled = false;
                    reqvalindate.Enabled = true;
                    reqvaliinno.Enabled = true;
                    ReqddlReceivingAction.Enabled = true;
                    btnitnnosave.Enabled = true;
                    if (lblpostatus.Text == BackOrderStatus)
                    {
                        BindAction("Add", "TYPE2");
                        drpitnnorecaction.ClearSelection();
                        drpitnnorecaction.Enabled = false;
                        drpitnnorecaction.Items.FindByValue(ActionPartial).Selected = true;
                        reqvalindate.Enabled = false;
                        reqvaliinno.Enabled = false;
                        drpitnnoreason.Enabled = true;
                        txtinvoicedate.Text = "";
                    }
                    else
                    {
                        BindAction("Add", "TYPE3");
                        drpitnnorecaction.ClearSelection();
                        drpitnnorecaction.Enabled = false;
                        drpitnnorecaction.Items.FindByValue(Actionvoid).Selected = true;
                    }

                    reqreason.Enabled = true;
                    Reqpackingsilpno.Enabled = false;
                    Reqtxtpackingslip.Enabled = false;
                    Reqtxtreceived.Enabled = false;
                    foreach (GridViewRow row in gvitnreview.Rows)
                    {
                        Label lblreceivingID = (Label)row.FindControl("lblreceivingID");
                        ViewState["ITReceivingMasterID"] = lblreceivingID.Text;
                        Label lblbalqty = (Label)row.FindControl("lblbalqty");
                        TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");
                        lblbalqty.Text = lstitnnno[0].BalanceQty.ToString();
                        txtrcdqty.Text = lstitnnno[0].ReceivingQty.ToString();
                    }
                    if (txtpckslipno.Text != "")
                    {
                        txtpckslipdate.Text = Convert.ToDateTime(lstitnnno[0].PackingDate).ToString("MM/dd/yyyy");
                        txtreceiveddate.Text = Convert.ToDateTime(lstitnnno[0].ReceivedDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        txtpckslipdate.Text = "";
                        txtreceiveddate.Text = "";
                    }
                    if (lblpostatus.Text == StatusOrderedPendingReceive)
                    {
                        drpitnnorecaction.Enabled = false;
                        drpitnnoreason.Enabled = true;
                        reqvalindate.Enabled = false;
                        reqvaliinno.Enabled = false;
                    }

                }
                else
                {
                    if (lblpostatus.Text == BackOrderStatus)
                    {
                        txtpckslipno.Text = "";
                        txtpckslipdate.Text = "";
                        txtreceiveddate.Text = "";
                        btnitnnosave.Visible = true;
                        txtinvoicedate.Text = "";
                    }
                    if (lblpostatus.Text == StatusOrderedPendingReceive)
                    {
                        btnitnnosave.Visible = true;
                        txtpckslipno.Text = "";
                        txtpckslipdate.Text = "";
                        txtreceiveddate.Text = "";
                    }
                    if (lblpostatus.Text == StatusReceivedPendingInvoice)
                    {
                        txtinvoicedate.Text = "";
                    }

                    Divsupad.Visible = true;
                    txtinvoiceno.Enabled = false;
                    txtinvoicedate.Enabled = false;
                    drpitnnorecaction.Enabled = false;
                    drpitnnoreason.Enabled = false;
                    txtpckslipdate.Enabled = true;
                    txtpckslipno.Enabled = true;
                    txtreceiveddate.Enabled = true;
                    gvitnreview.FooterRow.Enabled = false;
                    divnonsuperadmin.Visible = true;
                    reqvalindate.Enabled = false;
                    reqvaliinno.Enabled = false;
                    ReqddlReceivingAction.Enabled = false;
                    reqreason.Enabled = false;
                    Reqpackingsilpno.Enabled = true;
                    Reqtxtpackingslip.Enabled = true;
                    Reqtxtreceived.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITReceivingErrorMessage.Replace("<<ITRequestReceive>>", ex.Message.ToString()), true);
            }

        }


        public void Reviewfields()
        {
            btnitnnosave.Visible = false;
            lblrcountpo.Visible = true;
            lblrcountro.Visible = false;
            if (ViewState["ITReceivingMasterID"] != null)
            {
                List<BindITNNOvalues> lstitnnno = lclsservice.BindITNNOvalues(Convert.ToInt64(ViewState["ITReceivingMasterID"])).ToList();
                txtinvoiceno.Text = lstitnnno[0].InvoiceNo;
                if (lstitnnno[0].InvoiceDate != null)
                    txtinvoicedate.Text = Convert.ToDateTime(lstitnnno[0].InvoiceDate).ToString("MM/dd/yyyy");
                if (lstitnnno[0].ReceivingAction != null)
                    drpitnnorecaction.SelectedItem.Text = lstitnnno[0].ReceivingAction;
                if (lstitnnno[0].Reason != "")
                    drpitnnoreason.SelectedItem.Text = lstitnnno[0].Reason;
                txtpckslipno.Text = lstitnnno[0].PackingSlipNo;
                txtpckslipdate.Text = Convert.ToDateTime(lstitnnno[0].PackingDate).ToString("MM/dd/yyyy");
                txtreceiveddate.Text = Convert.ToDateTime(lstitnnno[0].ReceivedDate).ToString("MM/dd/yyyy");
                txtitnnoother.Text = lstitnnno[0].OtherReason;
                txtinvoiceno.Enabled = false;
                txtinvoicedate.Enabled = false;
                txtreceiveddate.Enabled = false;
                drpitnnoreason.Enabled = false;
                drpitnnorecaction.Enabled = false;
                txtitnnoother.Enabled = false;
                gvitnreview.DataSource = lstitnnno;
                gvitnreview.DataBind();
                Int32 a = Convert.ToInt32(HddGridCount.Value);
                lblrcountpo.Text = "No of records : " + (lstitnnno.Count - a);
                int i = 0;
                foreach (GridViewRow row in gvitnreview.Rows)
                {
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtreceivedqty = (TextBox)row.FindControl("txtrcdqty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    TextBox txtComments = (TextBox)row.FindControl("txtcmts");
                    lblbalqty.Text = lstitnnno[i].BalanceQty.ToString();
                    txtreceivedqty.Text = lstitnnno[i].ReceivingQty.ToString();
                    if (lstitnnno[i].Comments != null)
                        txtComments.Text = lstitnnno[i].Comments.ToString();
                    txtreceivedqty.Enabled = false;
                    txtComments.Enabled = false;
                    lblTotalPrice.Text = lstitnnno[i].TotalPrice.ToString();
                    i++;
                }
                TextBox txtsipcost = (TextBox)gvitnreview.FooterRow.FindControl("txtsipcost");
                TextBox txtTax = (TextBox)gvitnreview.FooterRow.FindControl("txttax") as TextBox;
                TextBox txtTotalCost = (TextBox)gvitnreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                txtTotalCost.Text = Convert.ToString(lstitnnno[0].TotalCost);
                txtsipcost.Enabled = false;
                txtTax.Enabled = false;
            }
        }

        protected void drpitnnoreason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpitnnoreason.Items.FindByText("Other").Selected == true)
            {
                otherid.Visible = true;
            }
            else
            {
                otherid.Visible = false;
            }
            TotalPrice();
            Totalcost();
            mpeitnno.Show();
        }
        public void TotalPrice()
        {
            foreach (GridViewRow row in gvitnreview.Rows)
            {
                TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");
                Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                if (txtrcdqty.Text != "" && lblrevppqty.Text != "")
                {
                    decimal TotalPrice = Convert.ToDecimal(txtrcdqty.Text) * Convert.ToDecimal(lblrevppqty.Text);
                    lblTotalPrice.Text = TotalPrice.ToString();
                }


            }
        }

        public void Totalcost()
        {
            TextBox txtShippingCost = gvitnreview.FooterRow.FindControl("txtsipcost") as TextBox;
            TextBox txtTax = gvitnreview.FooterRow.FindControl("txttax") as TextBox;
            TextBox txtTotalCost = gvitnreview.FooterRow.FindControl("txtTotalcost") as TextBox;
            decimal sum = 0;
            for (int i = 0; i < gvitnreview.Rows.Count; ++i)
            {
                Label lblTotalPrice = gvitnreview.Rows[i].FindControl("lblTotalPrice") as Label;
                if (lblTotalPrice.Text != "")
                    sum += Convert.ToDecimal(lblTotalPrice.Text);
            }
            (gvitnreview.FooterRow.FindControl("lblToatalcost") as TextBox).Text = sum.ToString();
            (gvitnreview.FooterRow.FindControl("txtTotalcost") as TextBox).Text = sum.ToString();
        }


        public void CalType()
        {
            decimal orterqty = 0;
            decimal receivedoqty = 0;
            decimal orterqtytotal = 0;
            decimal Total = 0;
            decimal receivedoqtytotal = 0;
            if (Convert.ToString(ViewState["Type"]) != "TYPE3")
            {
                foreach (GridViewRow row in gvitnreview.Rows)
                {
                    Label lblorderqty = (Label)row.FindControl("lblorderqty");
                    TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");

                    orterqty = Convert.ToDecimal(lblorderqty.Text);
                    if (txtrcdqty.Text != "")
                        receivedoqty = Convert.ToDecimal(txtrcdqty.Text);
                    orterqtytotal += orterqty;
                    if (txtrcdqty.Text != "")
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
            }

        }
        protected void btnitnnosave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveITReceiving();
                clear();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITReceivingErrorMessage.Replace("<<ITRequestReceive>>", ex.Message.ToString()), true);
            }
        }



        public void SaveITReceiving()
        {
            Int64 InsertCodeID = 0;
            string meditem = string.Empty;
            int rowcount = gvitnreview.Rows.Count;
            BALRequestITReceiving objreqitrecing = new BALRequestITReceiving();
            string msgrecngdetail = string.Empty;
            List<UpdateITRecevingMaster> msgrecngmaster = new List<UpdateITRecevingMaster>();
            objreqitrecing.ITReceivingMasterID = Convert.ToInt64(ViewState["ITReceivingMasterID"]);
            objreqitrecing.RequestITMasterID = Convert.ToInt64(ViewState["ITRequestMasterID"]);
            objreqitrecing.PackingSlipNo = txtpckslipno.Text;
            if (txtpckslipdate.Text != "")
            {
                objreqitrecing.PackingDate = Convert.ToDateTime(txtpckslipdate.Text);
            }
            if (txtreceiveddate.Text != "")
            {
                objreqitrecing.ReceivedDate = Convert.ToDateTime(txtreceiveddate.Text);
            }
            if (txtinvoicedate.Text != "")
            {
                objreqitrecing.InvoiceDate = Convert.ToDateTime(txtinvoicedate.Text);
            }
            objreqitrecing.Receivingaction = drpitnnorecaction.SelectedValue;
            objreqitrecing.Reason = drpitnnoreason.SelectedValue;
            ViewState["Reason"] = objreqitrecing.Reason;
            objreqitrecing.OtherReason = txtitnnoother.Text;
            CalType();
            objreqitrecing.Type = Convert.ToString(ViewState["Type"]);
            objreqitrecing.CreatedBy = defaultPage.UserId;
            objreqitrecing.LoggedinBy = defaultPage.UserId;
            objreqitrecing.FinalStatus = drpitnnorecaction.SelectedValue;
            objreqitrecing.ITNNo = Convert.ToString(ViewState["ITNNo"]);
            objreqitrecing.RequestITOrderID = Convert.ToInt64(ViewState["ITRequestOrderID"]);
            if (txtinvoiceno.Text != "")
                objreqitrecing.InvoiceNo = Convert.ToString(txtinvoiceno.Text);
            if (defaultPage.RoleID == 1)
            {
                objreqitrecing.Type = "TYPE3";
            }

            if (drpitnnorecaction.SelectedValue == VoidOrderStatus)
            {
                objreqitrecing.Type = "TYPE3";
                msgrecngmaster = lclsservice.UpdateITReceivingMaster(objreqitrecing).ToList();
                SendEmail();
            }

            else
            {
                TextBox txtsipcost = gvitnreview.FooterRow.FindControl("txtsipcost") as TextBox;
                TextBox txttax = gvitnreview.FooterRow.FindControl("txttax") as TextBox;
                objreqitrecing.Shippingcost = txtsipcost.Text;
                objreqitrecing.Tax = txttax.Text;
                TextBox txtTotalcost = gvitnreview.FooterRow.FindControl("txtTotalcost") as TextBox;
                if (txtTotalcost.Text != "")
                    objreqitrecing.TotalCost = Convert.ToDecimal(txtTotalcost.Text);
                msgrecngmaster = lclsservice.UpdateITReceivingMaster(objreqitrecing).ToList();
                InsertCodeID = Convert.ToInt64(msgrecngmaster[0].INSERTRECORDID);
                foreach (GridViewRow row in gvitnreview.Rows)
                {

                    Label lblreceivingdetailsID = (Label)row.FindControl("lblreceivingdetailsID");
                    objreqitrecing.ITReceivingDetailsID = Convert.ToInt64(lblreceivingdetailsID.Text);
                    Label lblrevppqty = (Label)row.FindControl("lblrevppqty");
                    Label lblorderqty = (Label)row.FindControl("lblorderqty");
                    Label lblreceivingID = (Label)row.FindControl("lblreceivingID");
                    Label lblbalqty = (Label)row.FindControl("lblbalqty");
                    TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");
                    Label lblTotalPrice = (Label)row.FindControl("lblTotalPrice");
                    Label lblitruser = (Label)row.FindControl("lblitruser");
                    TextBox txtcmts = (TextBox)row.FindControl("txtcmts");
                    Int32 Receivedqty = 0;
                    objreqitrecing.InsertRecordID = InsertCodeID;
                    objreqitrecing.BalacedQty = Balqunaity(row);
                    if (txtrcdqty.Text != "")
                        Receivedqty = Convert.ToInt32(txtrcdqty.Text);
                    objreqitrecing.ReceivedQty = Receivedqty;
                    objreqitrecing.Comments = txtcmts.Text;
                    objreqitrecing.User = lblitruser.Text;
                    decimal TotalPrice = 0;
                    TotalPrice = (Convert.ToDecimal(lblrevppqty.Text) * Receivedqty);
                    objreqitrecing.TotalPrice = TotalPrice;
                    meditem = lclsservice.UpdateITReceivingDetails(objreqitrecing);
                }
            }

            if (meditem == "Updated Successfully")
            {
                mpeitnno.Hide();
                ViewState["Type"] = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITReceivingSaveMessage.Replace("<<ITRequestReceive>>", ""), true);
            }

            SearchGrid();
        }
        public void SendEmail()
        {
            try
            {
                byte[] bytes = DetailsOrderReport(Convert.ToInt64(ViewState["ITRequestMasterID"]));
                string ITNNO = Convert.ToString(ViewState["ITNNo"]);
                objemail.vendoremailsubject = "Cancel Purchase Order – " + ITNNO;
                MemoryStream attachstream = new MemoryStream(bytes);

                string displayfilename1 = "Purchase Void Order – " + ITNNO + ".pdf";
                objemail.SendEmailPDFContent(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, displayfilename1 + ".pdf");
                mpeitnno.Hide();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ITReceiveemailMessage.Replace("<<ITRequestReceive>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachinePartsReceiveErrorMessage.Replace("<<MachinePartsReceive>>", ex.Message), true);
            }
        }

        public byte[] DetailsOrderReport(Int64 ITRequestMasterID)
        {
            byte[] bytes = null;
            try
            {
                List<object> llstarg = new List<object>();

                List<GetITROrderContentPO> llstreview = lclsservice.GetITROrderContentPO(ITRequestMasterID, defaultPage.UserId).ToList();
                objemail.CorporateEmail = llstreview[0].FromEmail;
                objemail.vendorContactEmail = llstreview[0].ToEmail;
                objemail.UserName = llstreview[0].OrderBy;
                objemail.UserEmail = llstreview[0].UserEmail;
                objemail.UserPhoneNo = llstreview[0].UserPhoneNo;
                string ITNNo = string.Empty;
                ITNNo = llstreview[0].ITNNo;
                ViewState["ITNNo"] = ITNNo;
                string reason = ViewState["Reason"].ToString();
                if (reason != "Other")
                {
                    objemail.vendorEmailcontent = string.Format("Due to " + reason + " " + ITNNo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + objemail.UserName + "<br/>" + objemail.UserEmail + "<br/>" + objemail.UserPhoneNo);
                }
                else
                {
                    objemail.vendorEmailcontent = string.Format("We are cancelling " + ITNNo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + objemail.UserName + "<br/>" + objemail.UserEmail + "<br/>" + objemail.UserPhoneNo);
                }
                rvITReceivingreport.ProcessingMode = ProcessingMode.Local;
                rvITReceivingreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITReceivingPDFVoid.rdlc");
                ReportDataSource datasource = new ReportDataSource("ReceivingVoidDS", llstreview);
                rvITReceivingreport.LocalReport.DataSources.Clear();
                rvITReceivingreport.LocalReport.DataSources.Add(datasource);
                rvITReceivingreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                bytes = rvITReceivingreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            catch (Exception ex)
            {
            }
            return bytes;
        }
        public Int32 Balqunaity(GridViewRow row)
        {

            Label lblorderqty = (Label)row.FindControl("lblorderqty");
            Label lblbalqty = (Label)row.FindControl("lblbalqty");
            TextBox txtrcdqty = (TextBox)row.FindControl("txtrcdqty");
            Int32 Balancequantity = 0;
            Int32 ReceivedQty = 0;
            if (txtrcdqty.Text != "")
                ReceivedQty = Convert.ToInt32(txtrcdqty.Text);
            Balancequantity = Convert.ToInt32(lblorderqty.Text) - ReceivedQty;
            return Balancequantity;
        }

        protected void grdITRequestReceiving_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                Image imgsummary = (Image)e.Row.FindControl("imgsummary");
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
                LinkButton lbitrno = (LinkButton)e.Row.FindControl("lbitrno");
                LinkButton lbitono = (LinkButton)e.Row.FindControl("lbitono");
                Label lblpostatus = (Label)e.Row.FindControl("lblpostatus");

                if (defaultPage.RoleID == 1)
                {
                    if (lbitrno.Text == "" && lblpostatus.Text != VoidOrderStatus)
                    {
                        lbitono.Enabled = true;
                        imgsummary.Enabled = false;

                    }
                    else
                    {
                        lbitono.Enabled = false;
                        imgsummary.Enabled = true;
                    }
                }
                else
                {
                    lbitrno.Enabled = false;

                    if (lbitrno.Text == "")
                    {
                        imgsummary.Enabled = false;
                    }
                    else
                    {
                        imgsummary.Enabled = true;
                    }
                }

                if (lblpostatus.Text == StatusVoidOrder || lblpostatus.Text == PartialOrderStatus)
                {
                    lbitono.Enabled = false;
                }

            }

        }
        protected void txtrcdqty_TextChanged(object sender, EventArgs e)
        {
            mpeitnno.Show();
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            SearchGrid();
        }

        protected void gvitnreview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (defaultPage.RoleID == 1)
                    {
                        TextBox txtrcdqty = (TextBox)e.Row.FindControl("txtrcdqty");
                        TextBox txtcmts = (TextBox)e.Row.FindControl("txtcmts");

                        txtrcdqty.Enabled = false;
                        txtcmts.Enabled = false;
                    }
                    Label lbloqty = (Label)e.Row.FindControl("lblorderqty");
                    TextBox txtreceivedoqty = (TextBox)e.Row.FindControl("txtrcdqty");
                    if (lbloqty.Text == "0")
                    {
                        e.Row.Style.Add("display", "none");
                        POhdnrowcount = POhdnrowcount + 1;
                    }
                }
                HddGridCount.Value = Convert.ToString(POhdnrowcount);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesReceivingErrorMessage.Replace("<<MedicalSupplyReceivingDescription>>", ex.Message), true);
            }
        }


        protected void imgsummary_Click(object sender, ImageClickEventArgs e)
        {
            string ITRONo = string.Empty;
            string ITNNO = string.Empty;
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            LinkButton lbitrno = (LinkButton)gvrow.FindControl("lbitrno");
            LinkButton lbitono = (LinkButton)gvrow.FindControl("lbitono");
            ITRONo = lbitrno.Text;
            ITNNO = lbitono.Text;
            List<BindITReceivingsummaryReport> llstreview = lclsservice.BindITReceivingsummaryReport(ITNNO, Convert.ToInt64(defaultPage.UserId), "").ToList();
            rvsummaryreport.ProcessingMode = ProcessingMode.Local;
            rvsummaryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITReceivingSummaryReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("ITReceivingSummary", llstreview);
            rvsummaryreport.LocalReport.DataSources.Clear();
            rvsummaryreport.LocalReport.DataSources.Add(datasource);
            rvsummaryreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvsummaryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "ITReceivingOrder" + guid + ".pdf";
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
            catch
            {

            }
        }

        protected void imgdetails_Click(object sender, ImageClickEventArgs e)
        {
            string ITRONo = string.Empty;
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            List<BindITReceivingDetailsReport> llstreview = lclsservice.BindITReceivingDetailsReport(Convert.ToInt64(gvrow.Cells[2].Text), Convert.ToInt64(gvrow.Cells[17].Text), Convert.ToInt64(defaultPage.UserId), "").ToList();
            //List<BindITReceivingDetailsSubReport> llstsubreview = lclsservice.BindITReceivingDetailsSubReport(Convert.ToInt64(gvrow.Cells[2].Text), Convert.ToInt64(defaultPage.UserId), "").ToList();
            rvRecedetailsReport.ProcessingMode = ProcessingMode.Local;
            rvRecedetailsReport.LocalReport.ReportPath = Server.MapPath("~/Reports/ITReceivingDetailsReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("BindITReceivingDetailsDS", llstreview);
            //ReportDataSource datasourcesub = new ReportDataSource("BindITReceivingSubDetailDS", llstsubreview);
            rvRecedetailsReport.LocalReport.DataSources.Clear();
            rvRecedetailsReport.LocalReport.DataSources.Add(datasource);
            //rvRecedetailsReport.LocalReport.DataSources.Add(datasourcesub);
            rvRecedetailsReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rvRecedetailsReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Guid guid = Guid.NewGuid();
            string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
            _sessionPDFFileName = "ITReceivingDetails" + guid + ".pdf";
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

        protected void btnitnnocancel_Click(object sender, EventArgs e)
        {
            clear();
            mpeitnno.Hide();
        }
        public void clear()
        {
            txtpckslipno.Text = "";
            txtpckslipdate.Text = "";
            txtreceiveddate.Text = "";
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                //ViewState["RequestITMasterID"] = null;
                //Print();

                if (drpcorsearch.SelectedValue == "All")
                {
                    objITReceiving.CorporateIDs = "ALL";
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

                    objITReceiving.CorporateIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    objITReceiving.FacilityIDs = "ALL";
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
                    objITReceiving.FacilityIDs = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpvendorsearch.SelectedValue == "All")
                {
                    objITReceiving.VendorIDs = "ALL";
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
                    objITReceiving.VendorIDs = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    objITReceiving.FinalStatus = "ALL";
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
                    objITReceiving.FinalStatus = FinalString;
                }
                SB.Clear();

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    objITReceiving.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    objITReceiving.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }

                objITReceiving.LoggedinBy = Convert.ToInt64(defaultPage.UserId);
                List<SearchITReceivingSummaryReport> lstMSRMaster = lclsservice.SearchITReceivingSummaryReport(objITReceiving).ToList();
                rvReceivingSummaryPrintAll.ProcessingMode = ProcessingMode.Local;
                rvReceivingSummaryPrintAll.LocalReport.ReportPath = Server.MapPath("~/Reports/ITReceivingSummaryPrintAllReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("ITReceivingPrintAllDS", lstMSRMaster);
                rvReceivingSummaryPrintAll.LocalReport.DataSources.Clear();
                rvReceivingSummaryPrintAll.LocalReport.DataSources.Add(datasource);
                rvReceivingSummaryPrintAll.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvReceivingSummaryPrintAll.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ITReceivingOrderSummary" + guid + ".pdf";
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
                BindFacility(1, "Add");
                DivMultiCorp.Style.Add("display", "none");
                divITOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            divITOrder.Attributes["class"] = "mypanel-body";
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
                BindVendor(1,"Add");
                DivFacCorp.Style.Add("display", "none");
                divITOrder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            divITOrder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {

            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                divITOrder.Attributes["class"] = "Upopacity";
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
            BindFacility(1,"Add");
            BindVendor(1,"Add");
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
                    divITOrder.Attributes["class"] = "Upopacity";
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
            BindFacility(1,"Add");
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