using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using Inventory.Class;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net;
using System.Configuration;
using System.Text;

namespace Inventory
{
    public partial class ServiceRequestPO : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        string StatusApprove = Constant.PendingOrderStatus;
        string statusdrp = Constant.PendingApprovalfornonuser;
        private string _sessionPDFFileName;
        EmailController objemail = new EmailController();
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.grdSRPO);
                scriptManager.RegisterPostBackControl(this.grdSROrder);
                scriptManager.RegisterPostBackControl(this.grdreviewreqpo);
                //scriptManager.RegisterPostBackControl(this.btnGenerateOrder);
                scriptManager.RegisterPostBackControl(this.GrdUploadFile);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningServiceRequestPOMessage.Replace("<<ServiceRequestPO>>", "No changes made in the Service Request PO. "), true);
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        BindCorporate();
                        //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility();
                        //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindStatus("Add");
                        BindRequestPOGrid("");                       
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
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
                    //if (defaultPage.RoleID == 1)
                    //{
                    //    // Search Drop Down
                    //    drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    //    drpfacilitysearch.DataTextField = "FacilityDescription";
                    //    drpfacilitysearch.DataValueField = "FacilityID";
                    //    drpfacilitysearch.DataBind();
                    //    //drpfacilitysearch.Items.Insert(0, lst);
                    //    //drpfacilitysearch.SelectedIndex = 0;                       

                    //}
                    //else
                    //{
                    //    drpfacilitysearch.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateName == drpcorsearch.SelectedItem.Text).ToList();
                    //    drpfacilitysearch.DataTextField = "FacilityName";
                    //    drpfacilitysearch.DataValueField = "FacilityID";
                    //    drpfacilitysearch.DataBind();
                    //    //ListItem lst = new ListItem();
                    //    //lst.Value = "0";
                    //    //lst.Text = "--Select Facility--";
                    //    //drpfacilitysearch.Items.Insert(0, lst);
                    //    //drpfacilitysearch.SelectedIndex = 0;
                    //}
                    foreach (ListItem lst in drpfacilitysearch.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        public void BindStatus(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("ServiceRequest", "Status", mode).ToList();
                // Search Status Drop Down

                //ListItem lst1 = new ListItem();
                //lst1.Value = "-1";
                //lst1.Text = "---Select Status---";

                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
                //drpStatussearch.Items.Insert(0, lst1);

                //ListItem lst = new ListItem();
                //lst.Value = "All";
                //lst.Text = "All";
                //drpStatussearch.Items.Insert(0, lst);

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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);

            }

        }



        public void BindRequestPOGrid(string search)
        {
            try
            {                
                BALServiceRequest llstSRPOdetails = new BALServiceRequest();
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListCorporateID = "ALL";
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

                    llstSRPOdetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListFacilityID = "ALL";
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
                    llstSRPOdetails.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListStatus = "ALL";
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
                    llstSRPOdetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSRPOdetails.LoggedinBy = defaultPage.UserId;
                if (search == "")
                {
                    List<SearchServiceRequestPurchaseOrder> lstSRPOMaster = lclsservice.SearchServiceRequestPurchaseOrder(llstSRPOdetails).Where(a => a.isEdit != 0).ToList();
                    grdSRPO.DataSource = lstSRPOMaster;
                    grdSRPO.DataBind();
                }
                else
                {
                    List<SearchServiceRequestPurchaseOrder> lstSRPOMaster = lclsservice.SearchServiceRequestPurchaseOrder(llstSRPOdetails).ToList();
                    grdSRPO.DataSource = lstSRPOMaster;
                    grdSRPO.DataBind();
                }



                foreach (GridViewRow row in grdSRPO.Rows)
                {
                    ImageButton imgbtnEdit = (ImageButton)row.FindControl("imgbtnEdit");
                    ImageButton imgsend = (ImageButton)row.FindControl("imgbtnEmail");
                    Label lblpostatus = (Label)row.FindControl("lblpostatus");
                    string Status = lblpostatus.Text;

                    if (Status == Constant.OrderStatus || Status == Constant.DeniedStatus)
                    {
                        imgbtnEdit.Visible = false;
                        imgsend.Visible = true;
                        if (Status == Constant.DeniedStatus || defaultPage.RoleID != 1)
                        {
                            imgsend.Visible = false;
                        }
                    }

                    else
                    {
                        imgbtnEdit.Visible = true;
                        imgsend.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        //protected void grdSRPO_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        Label lblCreatedBy = (Label)e.Row.FindControl("lblCreatedBy");
        //        Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
        //        Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
        //        Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (lblRemarks.Text != "")
        //            {
        //                if (lblRemarks.Text.Length > 150)
        //                {
        //                    lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
        //                    imgreadmore.Visible = true;
        //                }
        //                else
        //                {
        //                    imgreadmore.Visible = false;
        //                }
        //            }
        //            if (lblCreatedBy.Text != "")
        //            {
        //                if (lblCreatedBy.Text.Length > 150)
        //                {
        //                    lblCreatedBy.Text = lblCreatedBy.Text.Substring(0, 150) + "....";
        //                    imgreadmore1.Visible = true;
        //                }
        //                else
        //                {
        //                    imgreadmore1.Visible = false;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
        //    }

        //}


        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("ServiceRequestPO.aspx");
        }

        protected void drpaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSpin2 = (DropDownList)sender;
                GridViewRow gridrow = (GridViewRow)ddlSpin2.NamingContainer;
                int RowIndex = gridrow.RowIndex;
                Label lblpostatus = (Label)grdSRPO.Rows[RowIndex].Cells[11].FindControl("lblpostatus");
                foreach (GridViewRow grdfs in grdSRPO.Rows)
                {
                    if (ddlSpin2.SelectedItem.Text == Constant.actionOrder)
                        lblpostatus.Text = Constant.OrderStatus;
                    if (ddlSpin2.SelectedItem.Text == Constant.HoldStatus)
                        lblpostatus.Text = Constant.HoldStatus;
                    if (ddlSpin2.SelectedItem.Text == Constant.actionDeny)
                        lblpostatus.Text = Constant.DeniedStatus;
                    if (ddlSpin2.SelectedItem.Text == Constant.actionApprove)
                        lblpostatus.Text = Constant.Approved;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindRequestPOGrid("Search");
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Int64 SRPOID = 0;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                SRPOID = Convert.ToInt64(gvrow.Cells[1].Text);
                HddServiceDetailsID.Value = gvrow.Cells[1].Text;
                Response.Redirect("ServiceRequest.aspx?SRPOID=" + SRPOID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        protected void lbitrno_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                BALServiceRequest llstSRPOdetails = new BALServiceRequest();
                HddServiceDetailsID.Value = gvrow.Cells[1].Text;
                var FinalString = "";
                var SB = new StringBuilder();
                LinkButton txtSRNo = (LinkButton)gvrow.Cells[6].FindControl("lbrevSRno");
                //Label lblAction = (Label)gvrow.Cells[6].FindControl("lblAction");
                //Label lblIsEdit = (Label)gvrow.Cells[6].FindControl("lblIsEdit");
                HddServiceMasterNo.Value = txtSRNo.Text;
                lblSRNo.Text = txtSRNo.Text;

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListCorporateID = "ALL";
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

                    llstSRPOdetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListFacilityID = "ALL";
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
                    llstSRPOdetails.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListStatus = "ALL";
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
                    llstSRPOdetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSRPOdetails.LoggedinBy = defaultPage.UserId;


                List<SearchServiceRequestPurchaseOrder> llstSRMasterDetails = lclsservice.SearchServiceRequestPurchaseOrder(llstSRPOdetails).ToList();


                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", "LoggedIN user do not have permission to perform action"), true);
                GetServiceRequestDeatilsWithAction(llstSRMasterDetails);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        public void GetServiceRequestDeatilsWithAction(List<SearchServiceRequestPurchaseOrder> llstSRMasterDetails)
        {
            try
            {
                mpserviceorder.Show();
                DivShowError.Style.Add("display", "none");
                lblShowError.Text = "";
                BALServiceRequest llstSRPOdetails = new BALServiceRequest();
                llstSRPOdetails.ServiceRequestMasterID = Convert.ToInt64(HddServiceDetailsID.Value);

                llstSRPOdetails.LoggedinBy = defaultPage.UserId;
                List<SearchServiceRequestPurchaseOrderDetails> llstSRDetails = lclsservice.SearchServiceRequestPurchaseOrderDetails(llstSRPOdetails).ToList();

                //string LockTimeOut = "";
                //LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                //List<GetServiceRequestetailsbyServiceRequestMasterID> llstSRDetails = lclsservice.GetServiceRequestetailsbyServiceRequestMasterID(Convert.ToInt64(HddServiceDetailsID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();

                grdSROrder.DataSource = llstSRDetails;
                grdSROrder.DataBind();
                int i = 0;

                foreach (GridViewRow row in grdSROrder.Rows)
                {
                    Label lblVendor = (Label)row.FindControl("lblVendor");
                    TextBox txtService = (TextBox)row.FindControl("txtService");
                    TextBox txtUnit = (TextBox)row.FindControl("txtUnit");
                    TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                    DropDownList drpaction = (DropDownList)row.FindControl("drpaction");
                    TextBox txtremarksAfterOrder = (TextBox)row.FindControl("txtremarksAfterOrder");
                    TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                    Label lbServiceRequestMasterID = (Label)row.FindControl("lbServiceRequestMasterID");
                    Label lbServiceRequestDetailsID = (Label)row.FindControl("lbServiceRequestDetailsID");

                    txtremarksAfterOrder.Text = "";
                    txtremarks.Text = "";
                    lblVendor.Text = llstSRDetails[i].VendorName;
                    txtService.Text = llstSRDetails[i].Service;
                    txtUnit.Text = llstSRDetails[i].Unit;
                    txtPrice.Text = Convert.ToString(string.Format("{0:F2}", llstSRDetails[i].Price));
                    //string Mode = "Add";

                    List<GetServiceRequestActionByMasterID> llstSRAction = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(lbServiceRequestMasterID.Text), Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    List<GetServiceRequestMaster> llstServiceRequestMaster = lclsservice.GetServiceRequestMaster().Where(a => a.ServiceRequestMasterID == Convert.ToInt64(lbServiceRequestMasterID.Text)).ToList();
                    List<GetServiceRequestActionByMasterID> llstSRActionbyServicedetails = llstSRAction.Where(a => a.ServiceRequestDetailsID == Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    ListItem lst1 = new ListItem();
                    drpaction.ClearSelection();
                    List<string> SplitAction = new List<string>();
                    llstSRMasterDetails = llstSRMasterDetails.Where(a => a.ServiceRequestMasterID == Convert.ToInt64(HddServiceDetailsID.Value)).ToList();
                    if (llstSRMasterDetails[0].Action != null)
                    {
                        SplitAction = llstSRMasterDetails[0].Action.Split(',').ToList();
                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select Action--";
                        drpaction.DataSource = SplitAction;
                        drpaction.DataBind();
                        drpaction.Items.Insert(0, lst);
                    }
                    else
                    {
                        ListItem lst = new ListItem();
                        lst.Value = "0";
                        lst.Text = "--Select Action--";
                        //drpaction.DataSource = null;
                        //drpaction.DataBind();
                        drpaction.Items.Insert(0, lst);
                    }


                    if (llstSRAction.Count > 0)
                    {
                        if (llstSRActionbyServicedetails[0].ServiceRequestDetailsID == Convert.ToInt64(lbServiceRequestDetailsID.Text))
                        {
                            if (llstSRActionbyServicedetails[0].Action == null || llstSRActionbyServicedetails[0].Action == "")
                            {
                                drpaction.SelectedIndex = 0;
                            }
                            else
                            {
                                if (llstServiceRequestMaster[0].Status == Constant.OrderStatus || llstServiceRequestMaster[0].Status == Constant.DeniedStatus)
                                {
                                    txtremarksAfterOrder.Text = llstSRActionbyServicedetails[0].Remarks;
                                    drpaction.Enabled = false;
                                    txtremarksAfterOrder.ReadOnly = true;
                                    btnActionSave.Enabled = false;
                                    btnReset.Enabled = false;
                                    txtremarks.Visible = false;
                                    txtremarksAfterOrder.Visible = true;

                                    if (llstServiceRequestMaster[0].Status == Constant.OrderStatus && llstSRActionbyServicedetails[0].Action != Constant.actionDeny)
                                    {
                                        lst1.Value = "1";
                                        lst1.Text = Constant.actionOrder;
                                        drpaction.Items.Insert(1, lst1);
                                        drpaction.Items.FindByText(Constant.actionOrder).Selected = true;
                                    }
                                    else if ((llstServiceRequestMaster[0].Status == Constant.OrderStatus || llstServiceRequestMaster[0].Status == Constant.DeniedStatus) && llstSRActionbyServicedetails[0].Action == Constant.actionDeny)
                                    {
                                        lst1.Value = "1";
                                        lst1.Text = Constant.actionDeny;
                                        drpaction.Items.Insert(1, lst1);
                                        //drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                        drpaction.SelectedIndex = 1;
                                    }
                                    else
                                    {
                                        //drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                        drpaction.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    txtremarks.Text = llstSRActionbyServicedetails[0].Remarks;
                                    if (llstServiceRequestMaster[0].Status == Constant.PendingOrderStatus)
                                    {
                                        if (llstSRActionbyServicedetails[0].Action == Constant.actionApprove)
                                        {
                                            lst1.Value = "1";
                                            lst1.Text = Constant.actionApprove;
                                            drpaction.Items.Insert(1, lst1);
                                            drpaction.Items.FindByText(Constant.actionApprove).Selected = true;
                                            if (llstSRActionbyServicedetails[0].CreatedBy == defaultPage.UserId)
                                            {
                                                btnActionSave.Enabled = false;
                                                btnReset.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (llstSRDetails[i].Action == "")
                                            {
                                                lst1.Value = "1";
                                                lst1.Text = llstSRActionbyServicedetails[0].Action;
                                                drpaction.Items.Insert(1, lst1);
                                                drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                            }
                                            else
                                            {
                                                drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                            }
                                            //drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        if (llstServiceRequestMaster[0].Status == Constant.HoldStatus)
                                        {
                                            if (llstSRActionbyServicedetails[0].Action == Constant.HoldStatus)
                                            {
                                                lst1.Value = "1";
                                                lst1.Text = Constant.HoldStatus;
                                                drpaction.Items.Insert(1, lst1);
                                                drpaction.Items.FindByText(Constant.HoldStatus).Selected = true;
                                            }
                                            else
                                            {
                                                if (llstSRDetails[i].Action == "")
                                                {
                                                    lst1.Value = "1";
                                                    lst1.Text = llstSRActionbyServicedetails[0].Action;
                                                    drpaction.Items.Insert(1, lst1);
                                                    drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                                }
                                                else
                                                {
                                                    drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                                }
                                            }

                                        }
                                        if (llstServiceRequestMaster[0].Status == Constant.PendingApproval)
                                        {
                                            drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action).Selected = true;
                                        }

                                        //if (llstSRActionbyServicedetails[0].Action != "Deny")
                                        //{
                                        //    drpaction.Items.Remove(drpaction.Items.FindByText(llstSRActionbyServicedetails[0].Action));
                                        //}
                                        drpaction.Enabled = true;
                                        txtremarks.ReadOnly = false;
                                        txtremarks.Visible = true;
                                        txtremarksAfterOrder.Visible = false;
                                        btnActionSave.Enabled = true;
                                        btnReset.Enabled = true;
                                    }
                                    if (defaultPage.RoleID == 1)
                                    {
                                        drpaction.Enabled = true;
                                        txtremarks.ReadOnly = false;
                                        txtremarks.Visible = true;
                                        txtremarksAfterOrder.Visible = false;
                                        btnActionSave.Enabled = true;
                                        btnReset.Enabled = true;
                                    }
                                    else if (llstSRDetails[i].Action == "")
                                    {
                                        btnActionSave.Enabled = false;
                                        btnReset.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        drpaction.SelectedIndex = 0;
                        drpaction.Enabled = true;
                        txtremarks.ReadOnly = false;
                        txtremarks.Visible = true;
                        txtremarksAfterOrder.Visible = false;
                        btnActionSave.Enabled = true;
                        btnReset.Enabled = true;
                    }

                    lbServiceRequestMasterID.Text = llstSRDetails[i].ServiceRequestMasterID.ToString();
                    lbServiceRequestDetailsID.Text = llstSRDetails[i].ServiceRequestDetailsID.ToString();
                    i++;
                    if (HddReviewPOPCheck.Value == "1")
                    {
                        drpaction.Enabled = false;
                        txtremarks.ReadOnly = true;
                        btnActionSave.Visible = false;
                        btnReset.Visible = false;
                    }
                    else
                    {
                        drpaction.Enabled = true;
                        txtremarks.ReadOnly = false;
                        btnActionSave.Visible = true;
                        btnReset.Visible = true;
                    }
                    if (llstSRMasterDetails[0].isEdit != 0 && llstSRMasterDetails[0].Action != null && llstSRMasterDetails[0].Action != "")
                    {
                        btnActionSave.Enabled = true;
                        btnReset.Enabled = true;
                    }
                    else
                    {
                        btnActionSave.Enabled = false;
                        btnReset.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        protected void grdSROrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Int64 CorporateID = Convert.ToInt64(drpcorsearch.SelectedValue);
                    string Mode = "Add";
                    DropDownList drpaction = (e.Row.FindControl("drpaction") as DropDownList);
                    List<GetList> lstLookUp = new List<GetList>();
                    lstLookUp = lclsservice.GetList("ServiceRequestPO", "Action", Mode).ToList();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Action--";
                    // Search Status Drop Down
                    drpaction.DataSource = lstLookUp;
                    drpaction.DataTextField = "InvenValue";
                    drpaction.DataValueField = "InvenValue";
                    drpaction.DataBind();
                    drpaction.Items.Insert(0, lst);
                    drpaction.SelectedIndex = 0;
                    //string Status = e.Row.Cells[12].Text;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void btnActionSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceRquestPOAction();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }


        }

        public void UpdateServiceRquestPOAction()
        {
            try
            {
                string ErrorList = string.Empty;
                string NoChanges = string.Empty;
                string ChangesMade = string.Empty;
                string NewUser = string.Empty;
                string AlreadyActionDone = string.Empty;

                Int32 CheckOrder = 1;




                string SRStatus = string.Empty;
                string ReportMessage = string.Empty;

                int i = 0;

                foreach (GridViewRow row in grdSROrder.Rows)
                {
                    DropDownList drpactionCheck = (DropDownList)row.FindControl("drpaction");
                    TextBox txtremarks = (TextBox)row.FindControl("txtremarks");

                    Label lbServiceRequestMasterID = (Label)row.FindControl("lbServiceRequestMasterID");
                    Label lbServiceRequestDetailsID = (Label)row.FindControl("lbServiceRequestDetailsID");

                    //List<GetServiceRequestActionByMasterID> llstSRAction = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(lbServiceRequestMasterID.Text), Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    //List<GetServiceRequestActionByMasterID> llstSRActionbyServicedetails = llstSRAction.Where(a => a.ServiceRequestDetailsID == Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();

                    //int ActionCount = llstSRActionbyServicedetails.Where(a => a.Action == "Order").ToList().Count;

                    //if (drpactionCheck.SelectedValue == "1")
                    //{
                    //    AlreadyActionDone = drpactionCheck.SelectedItem.Text;
                    //}

                    if (drpactionCheck.SelectedItem.Text == "--Select Action--")
                    {
                        ErrorList = "Action is not selected";
                    }

                    List<GetServiceRequestActionByMasterID> llstSRAction = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(lbServiceRequestMasterID.Text), Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    List<GetServiceRequestActionByMasterID> llstfilterbyServiceRequestID = llstSRAction.Where(a => a.ServiceRequestDetailsID == Convert.ToInt64(lbServiceRequestDetailsID.Text)).ToList();
                    if (llstSRAction.Count == 0)
                    {
                        if (txtremarks.Text == "" && (drpactionCheck.SelectedItem.Text == Constant.actionDeny || drpactionCheck.SelectedItem.Text == Constant.HoldStatus))
                        {
                            NoChanges = "Remarks Should be added for Respected action";
                        }
                    }
                    else
                    {
                        if (llstfilterbyServiceRequestID[0].Action != drpactionCheck.SelectedItem.Text)
                        {
                            if (llstfilterbyServiceRequestID[0].Remarks == txtremarks.Text)
                            {
                                NoChanges = "Remarks Should be added for Changed Action";
                            }
                        }
                    }

                    if (llstfilterbyServiceRequestID.Count != 0)
                    {
                        if (llstfilterbyServiceRequestID[0].CreatedBy != defaultPage.UserId)
                        {
                            NewUser = "New user";
                        }

                    }

                    if (drpactionCheck.SelectedItem.Text == Constant.actionOrder)
                    {
                        SRStatus = Constant.OrderStatus;
                        if (llstSRAction.Count == 0)
                        {
                            CheckOrder = 2;
                        }
                    }
                    else if (drpactionCheck.SelectedItem.Text == Constant.actionApprove)
                    {
                        SRStatus = Constant.Approved;
                        CheckOrder = 1;
                    }
                }



                if (SRStatus == "")
                {
                    SRStatus = Constant.HoldStatus;
                }

                BALServiceRequest llstSRPOdetails = new BALServiceRequest();

                if (HddSelectedActionIndex.Value != "" || NewUser != "")
                {
                    if (ErrorList == "" && NoChanges == "" && AlreadyActionDone == "")
                    {
                        for (int j = 0; j < CheckOrder; j++)
                        {
                            foreach (GridViewRow row in grdSROrder.Rows)
                            {
                                DropDownList drpaction1 = (DropDownList)row.FindControl("drpaction");
                                TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                                Label lbServiceRequestMasterID = (Label)row.FindControl("lbServiceRequestMasterID");
                                Label lbServiceRequestDetailsID = (Label)row.FindControl("lbServiceRequestDetailsID");

                                llstSRPOdetails.ServiceRequestMasterID = Convert.ToInt64(lbServiceRequestMasterID.Text);
                                llstSRPOdetails.ServiceRequestDetailID = Convert.ToInt64(lbServiceRequestDetailsID.Text);
                                llstSRPOdetails.Status = "";
                                if (j == 0 && CheckOrder == 2 && drpaction1.SelectedItem.Text == Constant.actionOrder)
                                {
                                    llstSRPOdetails.Action = "FirstApprove";
                                    llstSRPOdetails.Remarks = Constant.actionApprove;
                                }
                                else
                                {
                                    llstSRPOdetails.Action = drpaction1.SelectedItem.Text;
                                    llstSRPOdetails.Remarks = txtremarks.Text;
                                }

                                llstSRPOdetails.CreatedBy = defaultPage.UserId;
                                llstSRPOdetails.LastModifiedBy = defaultPage.UserId;

                                ReportMessage = lclsservice.InsertServiceRequestApproveAction(llstSRPOdetails);

                            }
                        }

                        if (ReportMessage == "Saved Successfully")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOSaveMessage.Replace("<<ServiceRequestPO>>", HddServiceMasterNo.Value), true);
                            BindRequestPOGrid("");
                            DivShowError.Style.Add("display", "none");
                            lblShowError.Text = "";
                            HddSelectedActionIndex.Value = "";
                        }
                    }
                    else
                    {
                        mpserviceorder.Show();
                        DivShowError.Style.Add("display", "block");
                        if (ErrorList != "")
                        {
                            lblShowError.Text = "Action is not selected";
                        }
                        else if (NoChanges != "")
                        {
                            lblShowError.Text = NoChanges;
                        }
                        else if (AlreadyActionDone != "")
                        {
                            lblShowError.Text = "Service is already " + AlreadyActionDone + " for loggedIN user";
                        }

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningServiceRequestPOMessage.Replace("<<ServiceRequestPO>>", ""), true);
                    }


                    //else if (ReportMessage == "Updated Successfully")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOUpdateMessage.Replace("<<ServiceRequestPO>>",  HddServiceMasterNo.Value), true);
                    //    BindRequestPOGrid();
                    //}
                }
                else
                {
                    mpserviceorder.Show();
                    DivShowError.Style.Add("display", "block");
                    lblShowError.Text = "No changes made in the Service Request purchase order";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningServiceRequestPOMessage.Replace("<<ServiceRequestPO>>", "No changes made in the Service Request purchase order"), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            try
            {
                string lstOrderCheck = string.Empty;
                string ServiceMasterId = string.Empty;
                string ActionDone = string.Empty;

                mpereview.Show();
                if (defaultPage.RoleID == 1)
                {
                    btnGenerateOrder.Text = "Approve/Generate Order";
                }
                else
                {
                    btnGenerateOrder.Text = "Approve Order";
                }
                DivReportMessage.Style.Add("display", "none");
                lblReprotMessage.Text = "";
                btnrevwcancel.Text = "Cancel";

                foreach (GridViewRow row in grdSRPO.Rows)
                {
                    Label lblpostatus = (Label)row.FindControl("lblpostatus");
                    Label lblIsEdit = (Label)row.FindControl("lblIsEdit");

                    if (lblpostatus.Text == Constant.PendingApproval || lblpostatus.Text == Constant.PendingOrderStatus || lblpostatus.Text == Constant.HoldStatus || lblpostatus.Text == Constant.DeniedStatus)
                    {
                        ServiceMasterId = row.Cells[1].Text.Trim().Replace("&nbsp;", "");
                        List<GetServiceRequestActionByMasterID> llstSRAction = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(ServiceMasterId), Convert.ToInt64(ServiceMasterId)).ToList();
                        List<GetServiceRequestPoReportDetails> llstSRdetails = lclsservice.GetServiceRequestPoReportDetails(ServiceMasterId, null, defaultPage.UserId, defaultPage.UserId).ToList();
                        string NonUserCheck = string.Empty;

                        if (llstSRAction.Count > 0)
                        {
                            for (int i = 0; i < llstSRAction.Count; i++)
                            {
                                if (llstSRAction[i].Action == Constant.actionOrder)
                                {
                                    NonUserCheck = Constant.OrderStatus;
                                }
                                if (llstSRAction[i].Action == Constant.actionApprove && lblpostatus.Text == Constant.PendingOrderStatus)
                                {
                                    ActionDone = Constant.Completed;
                                }
                                else if (llstSRAction[i].Action == Constant.HoldStatus && lblpostatus.Text == Constant.HoldStatus)
                                {
                                    ActionDone = Constant.Completed;
                                }
                                else if (llstSRAction[i].Action == Constant.actionDeny && lblpostatus.Text == Constant.DeniedStatus)
                                {
                                    ActionDone = Constant.Completed;
                                }

                            }
                            if (defaultPage.RoleID == 1 && ActionDone != Constant.Completed)
                            {
                                lstOrderCheck += row.Cells[1].Text.Trim().Replace("&nbsp;", "") + ",";
                            }
                            else if (NonUserCheck == "" && ActionDone != Constant.Completed && lblIsEdit.Text == "1")
                            {
                                lstOrderCheck += row.Cells[1].Text.Trim().Replace("&nbsp;", "") + ",";
                            }
                            //if (lblpostatus.Text == "Pending Order" && defaultPage.UserId == 1)
                            //{
                            //    lstOrderCheck += row.Cells[1].Text.Trim().Replace("&nbsp;", "") + ",";
                            //}
                            //else if (NonUserCheck == "" && lblpostatus.Text == "Pending Approval" || lblpostatus.Text == "Denied" || lblpostatus.Text == "Hold")
                            //{
                            //    lstOrderCheck += row.Cells[1].Text.Trim().Replace("&nbsp;", "") + ",";
                            //}
                            ActionDone = "";
                        }
                    }
                }

                if (lstOrderCheck != "")
                    lstOrderCheck = lstOrderCheck.Substring(0, lstOrderCheck.Length - 1).ToString();

                HddListofSRMasterID.Value = lstOrderCheck;


                List<GetServiceRequestPOGenerateDetails> llstSRPOGen = new List<GetServiceRequestPOGenerateDetails>();
                llstSRPOGen = lclsservice.GetServiceRequestPOGenerateDetails(lstOrderCheck, defaultPage.UserId).ToList();
                //lblCorp.Text = llstSRPOGen[0].CorporateName;
                //lblFac.Text = llstSRPOGen[0].FacilityDescription;

                grdreviewreqpo.DataSource = llstSRPOGen;
                grdreviewreqpo.DataBind();

                HddReviewPOPCheck.Value = "1";

                if (llstSRPOGen.Count == 0)
                {
                    btnGenerateOrder.Enabled = false;
                    DivReportMessage.Style.Add("display", "block");
                    lblReprotMessage.Text = "No record for Generate Order or Approval";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }

        }

        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string SReqID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                byte[] bytes = DetailsContentOrderReport(SReqID, "No");
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path, "");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        private void ShowPDFFile(string path, string serviceattach)
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

                        //System.IO.File.Delete(path);
                        Response.End();
                    }
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    if (serviceattach == "")
                    {
                        Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                    }
                    else
                    {
                        Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path) + ",SR"));
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        public byte[] DetailsOrderAttachment(Int64 ServiceRequestDetailsID)
        {
            byte[] buffer = null;
            try
            {
                List<GetServiceAttachment> llstServiceAttachment = lclsservice.GetServiceAttachment(ServiceRequestDetailsID).ToList();

                string filePath = llstServiceAttachment[0].LocationOfTheFile;
                //string filepdfpath = Server.MapPath(filePath);
                //string output = Regex.Replace(filepdfpath, "[\\]", "/");
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string extension = System.IO.Path.GetExtension(filePath);
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                path = Path.Combine(path, filePath);
                System.Net.WebClient client = new System.Net.WebClient();
                buffer = client.DownloadData(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
            return buffer;
        }

        public byte[] DetailsContentOrderReport(string ListServiceRequestID, string MultiPrint)
        {
            byte[] bytes = null;
            try
            {
                List<GetServiceRequestPoReportDetails> llstreview = null;
                if (MultiPrint == "Yes")
                {
                    llstreview = lclsservice.GetServiceRequestPoReportDetails(null, ListServiceRequestID, defaultPage.UserId, defaultPage.UserId).ToList();
                }
                else if (MultiPrint == "No")
                {
                    llstreview = lclsservice.GetServiceRequestPoReportDetails(ListServiceRequestID, null, defaultPage.UserId, defaultPage.UserId).ToList();
                }

                rvservicerequestPOreport.ProcessingMode = ProcessingMode.Local;
                rvservicerequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceRequestPOReport.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[3];
                p1[0] = new ReportParameter("ServiceRequestID", "0");
                p1[1] = new ReportParameter("SearchFilters", "test");
                p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

                this.rvservicerequestPOreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("SRPoReportDS", llstreview);
                rvservicerequestPOreport.LocalReport.DataSources.Clear();
                rvservicerequestPOreport.LocalReport.DataSources.Add(datasource);
                rvservicerequestPOreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                bytes = rvservicerequestPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
            return bytes;

        }
        protected void imgrevprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 SPRmasterID;
                Int64 SPRDetailsID;
                SPRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                SPRDetailsID = Convert.ToInt64(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", ""));
                byte[] bytes = DetailsOrderReport(SPRmasterID, SPRDetailsID);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path, "");
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        public byte[] DetailsOrderReport(Int64 ServiceRequestMasterID, Int64 ServiceRequestDetailID)
        {
            byte[] bytes = null;
            try
            {
                List<GetSROrderContentPOReports> llstreview = lclsservice.GetSROrderContentPOReports(ServiceRequestMasterID, ServiceRequestDetailID, defaultPage.UserId).ToList();
                //string vendorContactEmail = llstreview[0].ContactEmail;
                objemail.CorporateEmail = llstreview[0].FromEmail;
                objemail.vendorContactEmail = llstreview[0].ToEmail;
                objemail.UserName = llstreview[0].OrderBy;
                objemail.UserEmail = llstreview[0].UserEmail;
                objemail.UserPhoneNo = llstreview[0].UserPhoneNo;
                objemail.SPONo = llstreview[0].SPONo;
                objemail.vendorEmailcontent = string.Format("Please see the attached document for order details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + objemail.UserName + "<br/>" + objemail.UserEmail + "<br/>" + objemail.UserPhoneNo);
                rvservicerequestPOreport.ProcessingMode = ProcessingMode.Local;
                rvservicerequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceRequestPDFPO.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[1];

                p1[0] = new ReportParameter("ServiceRequestID", Convert.ToString(ServiceRequestMasterID));
                this.rvservicerequestPOreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("ServiceRequestPODS", llstreview);
                rvservicerequestPOreport.LocalReport.DataSources.Clear();
                rvservicerequestPOreport.LocalReport.DataSources.Add(datasource);
                rvservicerequestPOreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                List<GetServiceRequestPOGenerateDetails> llstOrderContent = lclsservice.GetServiceRequestPOGenerateDetails(Convert.ToString(ServiceRequestMasterID), defaultPage.UserId).ToList();

                if (llstOrderContent[0].Ordercontent == null)
                {
                    bytes = rvservicerequestPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                }
                else
                {
                    bytes = llstOrderContent[0].Ordercontent;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
            return bytes;
        }


        protected void btnGenerateOrder_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {
                string lstmessage = string.Empty;
                foreach (GridViewRow row in grdreviewreqpo.Rows)
                {
                    try
                    {
                        #region multi order generation start
                        string checkstatus = row.Cells[12].Text;
                        BALServiceRequest llstservicerequestgen = new BALServiceRequest();
                        llstservicerequestgen.ServiceRequestMasterID = Convert.ToInt64(row.Cells[1].Text);
                        llstservicerequestgen.ServiceRequestDetailID = Convert.ToInt64(row.Cells[2].Text);
                        llstservicerequestgen.Status = checkstatus;
                        Label lbServiceRequestRemarks = (Label)row.FindControl("lblRemarks");
                        llstservicerequestgen.Remarks = lbServiceRequestRemarks.Text;
                        llstservicerequestgen.CreatedBy = defaultPage.UserId;
                        llstservicerequestgen.LastModifiedBy = defaultPage.UserId;

                        if (checkstatus == Constant.OrderStatus)
                        {
                            lstmessage = lclsservice.InsertServcieRequestGenerateOrder(llstservicerequestgen);

                            byte[] bytes = DetailsOrderReport(llstservicerequestgen.ServiceRequestMasterID, llstservicerequestgen.ServiceRequestDetailID);

                            llstservicerequestgen.OrderContent = bytes;
                            MemoryStream attachstream = new MemoryStream(bytes);

                            LinkButton lkbtnSPOno = (LinkButton)row.FindControl("lbrevSRno");
                            string result = lkbtnSPOno.Text;
                            var SPONo = "SPO" + result.Substring(2);
                            objemail.vendoremailsubject = "Purchase Order – " + SPONo;
                            // throw new Exception(SPONo + "Manual generated error");
                            string displayfilename = "Purchase Order Content– " + SPONo + ".pdf";
                            lstmessage = lclsservice.UpdateServcieRequestGenerateOrder(llstservicerequestgen);

                            //byte[] bytes1 = DetailsContentOrderReport(llstservicerequestgen.ServiceRequestMasterID.ToString(), "No");
                            byte[] bytes1 = DetailsOrderAttachment(llstservicerequestgen.ServiceRequestDetailID);
                            MemoryStream attachstream1 = new MemoryStream(bytes1);
                            string displayfilename1 = "Purchase Order – " + SPONo + ".pdf";
                            #region SendEmail code block
                            try
                            {
                                objemail.SendEmailWithTwoPDFContent(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, displayfilename, attachstream1, displayfilename1);
                            }
                            catch (Exception sendemailce)
                            {
                                LinkButton lbrevSRno = (LinkButton)row.FindControl("lbrevSRno");
                                string result1 = lbrevSRno.Text;
                                var SPONumber = "SPONo" + result1.Substring(2);
                                errmsg = errmsg + "Error in SPONo[" + SPONumber + "] - Send Email- " + sendemailce.Message.ToString();
                            }
                            #endregion SendEmail code block
                        }
                        else
                        {
                            lstmessage = lclsservice.UpdateServiceRequestApproveAction(llstservicerequestgen);
                        }
                        #endregion multi order generation end
                    }
                    catch (Exception innerce)
                    {
                        LinkButton lbSRno = (LinkButton)row.FindControl("lbrevSRno");
                        string result = lbSRno.Text;
                        var SPONo = "SPONo" + result.Substring(2);
                        errmsg = errmsg + "Error in SPONo[" + SPONo + "] - " + innerce.Message.ToString();
                    }
                }
                if (errmsg != string.Empty) throw new Exception(errmsg);
                if (lstmessage == "Saved Successfully" || lstmessage == "Updated Successfully" && defaultPage.RoleID == 1)
                {
                    mpereview.Show();
                    DivReportMessage.Style.Add("display", "block");
                    lblReprotMessage.Text = "Service Request Order changes are  Generated/Approved successfully";
                    btnrevwcancel.Text = "Go Back";
                    btnGenerateOrder.Enabled = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOEmptyMessage.Replace("<<ServiceRequestPO>>", "Service Request Order changes are  Generated/Approved successfully"), true);
                }
                else if (lstmessage == "Updated Successfully")
                {
                    mpereview.Show();
                    DivReportMessage.Style.Add("display", "block");
                    lblReprotMessage.Text = "Service Request Order changes are  Approved successfully";
                    btnrevwcancel.Text = "Go Back";
                    btnGenerateOrder.Enabled = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOEmptyMessage.Replace("<<ServiceRequestPO>>", "Service Request Order changes are  Approved successfully"), true);
                }
                BindRequestPOGrid("");
                List<GetServiceRequestPOGenerateDetails> llstSRPOGen = new List<GetServiceRequestPOGenerateDetails>();
                llstSRPOGen = lclsservice.GetServiceRequestPOGenerateDetails(HddListofSRMasterID.Value, defaultPage.UserId).ToList();
                //lblCorp.Text = llstSRPOGen[0].CorporateName;
                //lblFac.Text = llstSRPOGen[0].FacilityDescription;

                grdreviewreqpo.DataSource = llstSRPOGen;
                grdreviewreqpo.DataBind();

                HddGeneratePOPCheck.Value = "1";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BALServiceRequest llstSRPOdetails = new BALServiceRequest();
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListCorporateID = "ALL";
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

                    llstSRPOdetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListFacilityID = "ALL";
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
                    llstSRPOdetails.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListStatus = "ALL";
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
                    llstSRPOdetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSRPOdetails.LoggedinBy = defaultPage.UserId;


                List<SearchServiceRequestPurchaseOrder> llstSRMasterDetails = lclsservice.SearchServiceRequestPurchaseOrder(llstSRPOdetails).ToList();

                GetServiceRequestDeatilsWithAction(llstSRMasterDetails);
                //DivShowError.Style.Add("display", "none");
                //lblShowError.Text = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void imgbtnEmail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                List<GetServiceRequestActionByMasterID> llstServiceRequestDetailsID = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(gvrow.Cells[1].Text), Convert.ToInt64(gvrow.Cells[1].Text)).Where(a => a.Status == Constant.OrderStatus).ToList();

                byte[] bytes = DetailsOrderReport(Convert.ToInt64(gvrow.Cells[1].Text), Convert.ToInt64(llstServiceRequestDetailsID[0].ServiceRequestDetailsID));

                string lstrPDFPath = "Purchase Order Content – " + objemail.SPONo + ".pdf";
                objemail.vendoremailsubject = "Purchase Order – " + objemail.SPONo;
                MemoryStream attachstream = new MemoryStream(bytes);

                //byte[] bytes1 = DetailsContentOrderReport(gvrow.Cells[1].Text, "No");
                byte[] bytes1 = DetailsOrderAttachment(llstServiceRequestDetailsID[0].ServiceRequestDetailsID);
                MemoryStream attachstream1 = new MemoryStream(bytes1);
                string displayfilename1 = "Purchase Order – " + objemail.SPONo + ".pdf";
                objemail.SendEmailWithTwoPDFContent(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, lstrPDFPath, attachstream1, displayfilename1);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServicePartsOrderemailMessage, true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServicePartsOrderemailMessage.Replace("<<ServiceRequestPO>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

                BALServiceRequest llstSRPOdetails = new BALServiceRequest();
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListCorporateID = "ALL";
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

                    llstSRPOdetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListFacilityID = "ALL";
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
                    llstSRPOdetails.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSRPOdetails.ListStatus = "ALL";
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
                    llstSRPOdetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSRPOdetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSRPOdetails.LoggedinBy = defaultPage.UserId;
               List<SearchServiceRequestPurchaseOrder> lstSRPOMaster = lclsservice.SearchServiceRequestPurchaseOrder(llstSRPOdetails).ToList();
                rvservicerequestPOreport.ProcessingMode = ProcessingMode.Local;
                rvservicerequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceOrderSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetServiceOrderSummaryDS", lstSRPOMaster);
                rvservicerequestPOreport.LocalReport.DataSources.Clear();
                rvservicerequestPOreport.LocalReport.DataSources.Add(datasource);
                rvservicerequestPOreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvservicerequestPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
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
                ShowPDFFile(path, "");






                //string ServiceRequestID = string.Empty;
                //foreach (GridViewRow row in grdSRPO.Rows)
                //{
                //    ServiceRequestID += row.Cells[1].Text + ",";
                //}

                //ServiceRequestID = ServiceRequestID.Substring(0, ServiceRequestID.Length - 1).ToString();
                //byte[] bytes = DetailsContentOrderReport(ServiceRequestID, "Yes");
                //Guid guid = Guid.NewGuid();
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //_sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                //path = Path.Combine(path, _sessionPDFFileName);
                //using (StreamWriter sw = new StreamWriter(File.Create(path)))
                //{
                //    sw.Write("");
                //}
                //FileStream fs = new FileStream(path, FileMode.Open);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();
                //ShowPDFFile(path, "");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void lbitono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string ServiceRequestMasterID = gvrow.Cells[1].Text;
                List<GetServiceRequestPOGenerateDetails> llstOrderContent = lclsservice.GetServiceRequestPOGenerateDetails(Convert.ToString(ServiceRequestMasterID), defaultPage.UserId).ToList();
                byte[] bytes = llstOrderContent[0].Ordercontent;
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                ShowPDFFile(path, "");
                if (HddReviewPOPCheck.Value == "1")
                {
                    mpereview.Show();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        protected void btnImgUploadQuote_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Label ServiceRequestDetailsID = (Label)gvrow.FindControl("lbServiceRequestDetailsID");
                divUploadFile.Style.Add("display", "block");
                UploadOpacity.Attributes["class"] = "Upopacity";
                HddServiceDetailsID.Value = ServiceRequestDetailsID.Text;
                txtDescrip.Text = "";
                BindServiceAttachment();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }


        public void BindServiceAttachment()
        {
            try
            {
                List<GetServiceAttachment> llstServiceAttachment = lclsservice.GetServiceAttachment(Convert.ToInt64(HddServiceDetailsID.Value)).ToList();
                GrdUploadFile.DataSource = llstServiceAttachment;
                GrdUploadFile.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void btnupclose_Click(object sender, EventArgs e)
        {
            try
            {
                divUploadFile.Style.Add("display", "none");
                UploadOpacity.Attributes["class"] = "";
                mpserviceorder.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void btnImgView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkbtn = sender as ImageButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvrow.Cells[2].Text;
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string extension = System.IO.Path.GetExtension(filePath);
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                //response.AddHeader("Content-Disposition", "attachment;filename=\"" + path + "\"");
                path = Path.Combine(path, filePath);
                byte[] data = req.DownloadData(path);
                if (extension == ".pdf")
                {
                    ShowPDFFile(path, "SR");
                }
                else
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.WriteFile(file.FullName);
                        Response.Flush();
                    }
                    else
                    {
                        //Response.Write("This file does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestErrorMessage.Replace("<<ServiceRequestDescription>>", ex.Message.ToString()), true);
            }
            //response.End();
        }

        protected void btnrevwcancel_Click(object sender, EventArgs e)
        {
            try
            {
                btnGenerateOrder.Text = "Approve/Generate Order";
                DivReportMessage.Style.Add("display", "none");
                lblReprotMessage.Text = "";
                btnrevwcancel.Text = "Cancel";
                btnGenerateOrder.Enabled = true;
                HddReviewPOPCheck.Value = "";
                HddGeneratePOPCheck.Value = "";
                //drpcorsearch.SelectedIndex = 0;
                //drpfacilitysearch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                DivShowError.Style.Add("display", "none");
                lblShowError.Text = "";
                btnGenerateOrder.Enabled = true;
                //drpcorsearch.SelectedIndex = 0;
                //drpfacilitysearch.SelectedIndex = 0;
                if (HddReviewPOPCheck.Value == "1")
                {
                    mpereview.Show();
                }
                if (HddGeneratePOPCheck.Value == "1")
                {
                    btnGenerateOrder.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        public void clearDetails()
        {
            drpcorsearch.ClearSelection();
            drpfacilitysearch.ClearSelection();
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            BindCorporate();
            BindFacility();
            BindStatus("Add");
            HddListCorpID.Value = "";

            //drpcorsearch.SelectedIndex = 0;
            //drpfacilitysearch.SelectedIndex = 0;

        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            clearDetails();
            BindRequestPOGrid("");
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
                UploadOpacity.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            UploadOpacity.Attributes["class"] = "mypanel-body";
        }
        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                UploadOpacity.Attributes["class"] = "Upopacity";
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
 
            HddListCorpID.Value = "";
            HddListFacID.Value = "";
        }

    }
}