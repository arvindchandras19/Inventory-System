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
using System.Web.Services;

namespace Inventory
{
    public partial class CapitalOrder : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALCapitalPO obicapital = new BALCapitalPO();
        EmailController objemail = new EmailController();
        string ErrorList = string.Empty;
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
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.btnGenerateOrder);
            scriptManager.RegisterPostBackControl(this.grdCRRequestPO);
            scriptManager.RegisterPostBackControl(this.btnPrint);
            scriptManager.RegisterPostBackControl(this.grdreviewreqpo);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CorpDrop();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "FaciDrop();", true);
            if (!IsPostBack)
            {
                if (defaultPage != null)
                {
                    BindCorporate();
                    //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                    BindFacility();
                    //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                    BindVendor();
                    BindStatus("Add");
                    //drpcorsearch.SelectedIndex = 0;
                    //drpfacilitysearch.SelectedIndex = 0;
                    BindRequestPOGrid();



                    if (defaultPage.RoleID == 1)
                    {
                        drpcorsearch.Enabled = true;
                        drpfacilitysearch.Enabled = true;
                        btnorderall.Visible = true;
                        btnapproveall.Visible = false;
                    }
                    else
                    {
                        btnorderall.Visible = false;
                        btnapproveall.Visible = true;
                        btnGenerateOrder.Text = "Approve Order";
                    }
                    if (defaultPage.CapitalOrder_Edit == false && defaultPage.CapitalOrder_View == true)
                    {
                        btnorderall.Visible = false;
                    }
                    if (defaultPage.CapitalOrder_Edit == false && defaultPage.CapitalOrder_View == false)
                    {
                        updmain.Visible = false;
                        User_Permission_Message.Visible = true;
                    }
                }
            }
        }

        //protected void btnMultiCorpselect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        foreach (ListItem lst1 in drpcorsearch.Items)
        //        {
        //            lst1.Attributes.Add("class", "");
        //            lst1.Selected = false;
        //        }
        //        foreach (GridViewRow row in GrdMultiCorp.Rows)
        //        {
        //            CheckBox chkmultiCorp = (CheckBox)row.FindControl("chkmultiCorp");
        //            Label lblCorpID = (Label)row.FindControl("lblCorpID");

        //            if (chkmultiCorp.Checked == true)
        //            {
        //                foreach (ListItem lst1 in drpcorsearch.Items)
        //                {
        //                    if (lst1.Value == lblCorpID.Text)
        //                    {
        //                        lst1.Attributes.Add("class", "selected");
        //                        lst1.Selected = true;
        //                    }
        //                }
        //            }
        //        }
        //        BindFacility();
        //        DivMultiCorp.Style.Add("display", "none");
        //        DivCapitalorder.Attributes["class"] = "mypanel-body";
        //    }
        //    catch(Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
        //    }
        //}

        //protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        //{
        //    DivMultiCorp.Style.Add("display", "none");
        //    DivCapitalorder.Attributes["class"] = "mypanel-body";
        //}

        //protected void lnkMultiCorp_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<BALUser> lstcrop = new List<BALUser>();
        //        DivMultiCorp.Style.Add("display", "block");
        //        DivCapitalorder.Attributes["class"] = "Upopacity";

        //        if (defaultPage.RoleID == 1)
        //        {
        //            lstcrop = lclsservice.GetCorporateMaster().ToList();                                        
        //        }
        //        else
        //        {
        //            lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
        //            drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();                   
        //        }
        //        GrdMultiCorp.DataSource = lstcrop;
        //        GrdMultiCorp.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
        //    }
        //}


        //protected void lnkClearCorp_Click(object sender, EventArgs e)
        //{
        //    BindCorporate();
        //}


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
                //lst.Value = "0";
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
                foreach (ListItem lst1 in drpcorsearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        //protected void btnMultiFacselect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {                
        //        foreach (ListItem lst1 in drpfacilitysearch.Items)
        //        {
        //            lst1.Attributes.Add("class", "");
        //            lst1.Selected = false;
        //        }
        //        foreach (GridViewRow row in GrdMultiFac.Rows)
        //        {
        //            CheckBox chkmultiFac = (CheckBox)row.FindControl("chkmultiFac");
        //            Label lblFacID = (Label)row.FindControl("lblFacID");

        //            if (chkmultiFac.Checked == true)
        //            {
        //                foreach (ListItem lst1 in drpfacilitysearch.Items)
        //                {
        //                    if (lst1.Value == lblFacID.Text)
        //                    {
        //                        lst1.Attributes.Add("class", "selected");
        //                        lst1.Selected = true;
        //                    }
        //                }
        //            }
        //        }
        //        BindVendor();
        //        DivFacCorp.Style.Add("display", "none");
        //        DivCapitalorder.Attributes["class"] = "mypanel-body";
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
        //    }
        //}


        //protected void btnMultiFacClose_Click(object sender, EventArgs e)
        //{
        //    DivFacCorp.Style.Add("display", "none");
        //    DivCapitalorder.Attributes["class"] = "mypanel-body";
        //}

        //protected void lnkMultiFac_Click(object sender, EventArgs e)
        //{
        //    try
        //    {               
        //        if (drpcorsearch.SelectedValue != "")
        //        {
        //            foreach (ListItem lst in drpcorsearch.Items)
        //            {
        //                if (lst.Selected && drpcorsearch.SelectedValue != "All")
        //                {
        //                    SB.Append(lst.Value + ',');
        //                }
        //            }
        //            if (SB.Length > 0)
        //                FinalString = SB.ToString().Substring(0, (SB.Length - 1));

        //            // Search Drop Down
        //            GrdMultiFac.DataSource = lclsservice.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
        //            GrdMultiFac.DataBind();
        //            DivFacCorp.Style.Add("display", "block");
        //            DivCapitalorder.Attributes["class"] = "Upopacity";

        //        }
                
                
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
        //    }
        //}


        //protected void lnkClearFac_Click(object sender, EventArgs e)
        //{
        //    BindFacility();
        //}

        #region Bind Facility Values

        public void BindFacility()
        {
            try
            {
                //ListItem lst = new ListItem();
                //lst.Value = "0";
                //lst.Text = "All";
                if (drpcorsearch.SelectedValue != "")
                {
                    foreach (ListItem lst1 in drpcorsearch.Items)
                    {
                        if (lst1.Selected && drpcorsearch.SelectedValue != "All")
                        {
                            SB.Append(lst1.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    // Search Drop Down
                    drpfacilitysearch.DataSource = lclsservice.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                    drpfacilitysearch.DataTextField = "FacilityDescription";
                    drpfacilitysearch.DataValueField = "FacilityID";
                    drpfacilitysearch.DataBind();
                    //drpfacilitysearch.Items.Insert(0, lst);
                    //drpfacilitysearch.SelectedIndex = 0;
                    //if (defaultPage.RoleID == 1)
                    //{
                    //    if (drpcorsearch.SelectedValue != "All")
                    //    {
                    //        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).Where(a => a.IsActive == true).ToList();
                    //        drpfacilitysearch.DataTextField = "FacilityDescription";
                    //        drpfacilitysearch.DataValueField = "FacilityID";
                    //        drpfacilitysearch.DataBind();
                    //        //drpfacilitysearch.Items.Insert(0, lst);
                    //        //drpfacilitysearch.SelectedIndex = 0;
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
                    //        //drpfacilitysearch.Items.Insert(0, lst);
                    //        //drpfacilitysearch.SelectedIndex = 0;
                    //    }
                    //    else
                    //    {
                    //        drpfacilitysearch.SelectedIndex = 0;
                    //    }
                    //}
                }
                foreach (ListItem lst2 in drpfacilitysearch.Items)
                {
                    lst2.Attributes.Add("class", "selected");
                    lst2.Selected = true;
                }
                BindVendor();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        //[WebMethod]
        //public static List<BALUser> LoadData()
        //{
        //    try
        //    {
        //        List<BALUser> lstcrop = new List<BALUser>();
        //        InventoryServiceClient lclsservice = new InventoryServiceClient();
        //        lstcrop = lclsservice.GetCorporateMaster().ToList();
        //        //ListItem lst = new ListItem();
        //        //lst.Value = "All";
        //        //lst.Text = "All";

        //        //List<CountryList> lst = new List<CountryList>();
        //        //lst.Add(new CountryList() { CountryId = 1, CountryName = "India" });
        //        //lst.Add(new CountryList() { CountryId = 2, CountryName = "Nepal" });
        //        //lst.Add(new CountryList() { CountryId = 3, CountryName = "America" });
        //        return lstcrop;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //[WebMethod]
        //public static List<GetFacilityByListCorporateID>  BindFacilityClient(string[] ListCorporateID)
        //{
        //    try
        //    {
        //        List<GetFacilityByListCorporateID> ListFacility = new List<GetFacilityByListCorporateID>();
        //        InventoryServiceClient lclsservice = new InventoryServiceClient();
        //        string FinalString = "";
        //        StringBuilder SB = new StringBuilder();
        //        if (ListCorporateID != null)
        //        {
        //            foreach (var item in ListCorporateID)
        //            {
        //                SB.Append(item + ',');
        //            }
        //            if (SB.Length > 0)
        //                FinalString = SB.ToString().Substring(0, (SB.Length - 1));

        //            ListFacility = lclsservice.GetFacilityByListCorporateID(FinalString, 1, 1).ToList();


        //        }
        //        return ListFacility;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //[WebMethod]
        //public static List<GetVendorByFacilityID> BindVendorClient(string[] ListFacilityID)
        //{
        //    try
        //    {
        //        List<GetVendorByFacilityID> Listvendor = new List<GetVendorByFacilityID>();
        //        InventoryServiceClient lclsservice = new InventoryServiceClient();
        //        string FinalString = "";
        //        StringBuilder SB = new StringBuilder();
        //        if (ListFacilityID != null)
        //        {
        //            foreach (var item in ListFacilityID)
        //            {
        //                SB.Append(item + ',');
        //            }
        //            if (SB.Length > 0)
        //                FinalString = SB.ToString().Substring(0, (SB.Length - 1));

        //            Listvendor = lclsservice.GetVendorByFacilityID(FinalString, 1).Where(a => a.MachineParts == true).ToList();
        //        }
        //        return Listvendor;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        #endregion

        public void BindStatus(string mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("CapitalPO", "Status", mode).ToList();
                drpStatussearch.DataSource = lstLookUp;
                drpStatussearch.DataTextField = "InvenValue";
                drpStatussearch.DataValueField = "InvenValue";
                drpStatussearch.DataBind();
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
                // drpStatussearch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }

        }

        public void BindRequestPOGrid()
        {
            try
            {
                BALCapitalPO llstcr = new BALCapitalPO();

                //foreach (ListItem lst in drpcorsearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}

                //foreach (ListItem lst in drpfacilitysearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}

                //foreach (ListItem lst in drpvendorsearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}


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
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
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
                List<SearchCapitalPO> lstCRMaster = lclsservice.SearchCapitalPO(llstcr).ToList();
                grdCRRequestPO.DataSource = lstCRMaster;
                grdCRRequestPO.DataBind();
                int i = 0;
                foreach (GridViewRow row in grdCRRequestPO.Rows)
                {
                    DropDownList drp1 = (DropDownList)row.FindControl("drpaction");
                    List<string> SplitAction = new List<string>();
                    SplitAction = lstCRMaster[i].Action.Split(',').ToList();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            BindRequestPOGrid();
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
            BindStatus("Add");
            BindCorporate();
            BindFacility();
            BindVendor();
            //grdCRRequestPO.DataSource = null;
            //grdCRRequestPO.DataBind();
            ViewState["ReqID"] = "";
            HddListCorpID.Value = "";
            HddListFacID.Value = "";

        }
        protected void grdCRRequestPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                    //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                    //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                    //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                    DropDownList drpaction = (e.Row.FindControl("drpaction") as DropDownList);
                    ImageButton imgbtnEdit = (e.Row.FindControl("imgbtnEdit") as ImageButton);
                    ImageButton imgsend = (e.Row.FindControl("imgsend") as ImageButton);
                    string Status = e.Row.Cells[12].Text;

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


                    if (Status == StatusOrder || Status == StatusDeny)
                    {
                        drpaction.Enabled = false;
                        imgbtnEdit.Visible = false;
                    }
                    else
                    {
                        drpaction.Enabled = true;
                        imgbtnEdit.Visible = true;
                    }
                    if (defaultPage.RoleID == 1 && Status == StatusOrder)
                    {
                        imgsend.Visible = true;
                    }
                    else
                    {
                        imgsend.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnapproveall_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnapproveall.Text == "Approve All")
                {
                    HddDetailsID.Value = "";

                    foreach (GridViewRow grdfs in grdCRRequestPO.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HddDetailsID.Value += rowindex + ",";
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;
                        if (status != StatusOrder && status != StatusDeny)
                        {
                            drpaction.SelectedValue = actionApprove;
                        }
                        btnapproveall.Text = "Clear All";
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdCRRequestPO.Rows)
                    {
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        string status = string.Empty;
                        status = grdfs.Cells[12].Text;
                        if (status != StatusDeny && status != StatusOrder)
                        {
                            drpaction.SelectedIndex = 0;
                        }

                        btnapproveall.Text = "Approve All";
                    }
                    HddDetailsID.Value = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnorderall_Click(object sender, EventArgs e)
        {
            try
            {
                string status = string.Empty;

                if (btnorderall.Text == "Order All")
                {
                    HddDetailsID.Value = "";

                    foreach (GridViewRow grdfs in grdCRRequestPO.Rows)
                    {
                        status = grdfs.Cells[12].Text;
                        DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                        int rowindex = grdfs.RowIndex;
                        HddDetailsID.Value += rowindex + ",";
                        if (status != StatusOrder && drpaction.Enabled == true)
                        {
                            drpaction.SelectedValue = actionOrder;
                        }

                        btnorderall.Text = "Clear All";
                    }
                }
                else
                {
                    foreach (GridViewRow grdfs in grdCRRequestPO.Rows)
                    {
                        status = grdfs.Cells[12].Text;
                        if (status != StatusOrder)
                        {
                            DropDownList drpaction = (DropDownList)grdfs.FindControl("drpaction");
                            if (drpaction.Enabled == true)
                            {
                                drpaction.SelectedIndex = 0;
                            }

                            btnorderall.Text = "Order All";
                        }
                    }
                    HddDetailsID.Value = "";
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindRequestPOGrid();
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 CapitalItemMasterID = 0;
                CapitalItemMasterID = Convert.ToInt64(gvrow.Cells[1].Text);
                Response.Redirect("CapitalItem.aspx?CapitalItemMasterID=" + CapitalItemMasterID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }
        protected void btnCoreview_Click(object sender, EventArgs e)
        {
            bindreview();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CapitalItemMasterID");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("CorporateID");
            dt.Columns.Add("FacilityID");
            dt.Columns.Add("VendorID");
            dt.Columns.Add("CorporateName");
            dt.Columns.Add("FacilityShortName");
            dt.Columns.Add("VendorShortName");
            dt.Columns.Add("CRNo");
            dt.Columns.Add("CPONo");
            dt.Columns.Add("TotalCost");
            dt.Columns.Add("Status");
            dt.Columns.Add("Action");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("CapitalOrderID");
            dt.Columns.Add("OldStatus");

            dt.AcceptChanges();
            return dt;
        }

        public void bindreview()
        {
            try
            {
                string OrderIDS = Convert.ToString(HddDetailsID.Value);
                DataTable dt = CreateDataTable();
                if (OrderIDS != "")
                {
                    OrderIDS = OrderIDS.Substring(0, OrderIDS.Length - 1);
                    string[] values = OrderIDS.Split(',');
                    string[] c = values.Distinct().ToArray();
                    //for (int i = 0; i < c.Length; i++)
                    foreach (string val in c)
                    {
                        if (val != "")
                        {
                            GridViewRow row = grdCRRequestPO.Rows[Convert.ToInt32(val)];
                            string CapitalItemMasterID = row.Cells[1].Text;
                            string CreatedOn = row.Cells[2].Text;
                            string CorporateID = row.Cells[3].Text;
                            string FacilityID = row.Cells[4].Text;
                            string VendorID = row.Cells[5].Text;
                            string CorporateName = row.Cells[6].Text;
                            string FacilityShortName = row.Cells[7].Text;
                            string VendorShortName = row.Cells[8].Text;
                            LinkButton lbCRrno = (LinkButton)row.FindControl("lbCRrno");
                            LinkButton lblreviewcrono = (LinkButton)row.FindControl("lbcrono");
                            string TotalCost = row.Cells[11].Text;
                            string Status = row.Cells[12].Text;
                            Label CreatedBy = (Label)row.FindControl("CreatedBy");
                            Label lblaudit = (Label)row.FindControl("lblaudit");
                            Image imgreadmore1 = (Image)row.FindControl("imgreadmore1");
                            DropDownList drpaction = (DropDownList)row.FindControl("drpaction");

                            if (drpaction.SelectedItem.Text == actionApprove)
                            {
                                Status = StatusApprove;
                            }
                            else if (drpaction.SelectedItem.Text == actionOrder)
                            {
                                Status = StatusOrder;
                            }
                            else if (drpaction.SelectedItem.Text == actionDeny)
                            {
                                Status = StatusDeny;
                            }
                            else if (drpaction.SelectedItem.Text == actionHold)
                            {
                                Status = StatusHold;
                            }

                            DataRow dr = dt.NewRow();
                            if ((drpaction.SelectedIndex != 0))
                            {
                                dr["CapitalItemMasterID"] = CapitalItemMasterID;
                                dr["CorporateID"] = CorporateID;
                                dr["FacilityID"] = FacilityID;
                                dr["VendorID"] = VendorID;
                                dr["CreatedOn"] = CreatedOn;
                                dr["CorporateName"] = CorporateName;
                                dr["FacilityShortName"] = FacilityShortName;
                                dr["VendorShortName"] = VendorShortName;
                                dr["CRNo"] = lbCRrno.Text;
                                dr["CPONo"] = lblreviewcrono.Text;
                                dr["TotalCost"] = TotalCost;
                                dr["Status"] = Status;
                                dr["OldStatus"] = row.Cells[12].Text;
                                dr["Action"] = drpaction.SelectedItem.Text;
                                dr["Audit"] = lblaudit.Text;

                                dt.Rows.Add(dr);
                            }
                        }
                        ViewState["OldStatus"] = dt;
                        grdreviewreqpo.DataSource = dt;
                        grdreviewreqpo.DataBind();
                        btnrevwcancel.Text = "Cancel";
                        btnGenerateOrder.Visible = true;
                        lblmsg.Text = "";
                        mpereview.Show();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarngActionMessage.Replace("<<MajorItemOrder>>", ""), true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnGenerateOrder_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            try
            {
                BALCapitalPO obicapital = new BALCapitalPO();
                Functions objfun = new Functions();
                string CPOIns = string.Empty;
                string CPOUpd = string.Empty;
                int ReviewCount = grdreviewreqpo.Rows.Count;
                if (ReviewCount != 0)
                {
                    foreach (GridViewRow row in grdreviewreqpo.Rows)
                    {
                        try
                        {
                            #region multi order generation start
                            //GridViewRow row = grdreviewreqpo.Rows[i];
                            obicapital.Remarks = string.Empty;
                            obicapital.OrderDate = Convert.ToDateTime(row.Cells[2].Text);
                            obicapital.CorporateID = Convert.ToInt64(row.Cells[3].Text);
                            obicapital.FacilityID = Convert.ToInt64(row.Cells[4].Text);
                            obicapital.VendorID = Convert.ToInt64(row.Cells[5].Text);
                            string lbltotal = row.Cells[11].Text;
                            string totprice = lbltotal.Substring(1);
                            string Status = row.Cells[12].Text;
                            obicapital.CapitalItemMasterID = Convert.ToInt64(row.Cells[1].Text);
                            ViewState["ReqID"] += row.Cells[1].Text + ",";
                            LinkButton lblreviewcrono = (LinkButton)row.FindControl("lblreviewcrono");
                            LinkButton lblreviewCRrno = (LinkButton)row.FindControl("lblreviewCRrno");
                            byte[] data = new byte[0];
                            obicapital.OrderContent = data;
                            obicapital.TotalCost = Convert.ToDecimal(totprice);
                            obicapital.Status = Status;
                            if (Status != StatusOrder)
                            {
                                TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                                obicapital.Remarks = txtremarks.Text;
                            }
                            obicapital.CreatedBy = defaultPage.UserId;
                            //obicapital.LastModifiedBy = defaultPage.UserId;
                            if (Status == StatusOrder)
                            {
                                string result = lblreviewCRrno.Text;
                                var CPONo = "CPONo" + result.Substring(2);
                                Int64 CRmasterID = obicapital.CapitalItemMasterID;
                                obicapital.CPONo = CPONo;
                                CPOIns = lclsservice.InsertCapitalPO(obicapital);

                                List<object> llstresult = DetailsOrderReport(CRmasterID);
                                byte[] bytes = (byte[])llstresult[2];
                                obicapital.OrderContent = bytes;
                                MemoryStream attachstream = new MemoryStream(bytes);
                                Int64 CorporateID = obicapital.CorporateID;
                                Int64 FacilityID = obicapital.FacilityID;
                                objemail.FromEmail = llstresult[0].ToString();
                                objemail.ToEmail = llstresult[1].ToString();
                                EmailSetting(CorporateID, FacilityID, CPONo);

                                CPOIns = lclsservice.UpdateCRPOOrderContent(obicapital);
                                #region SendEmail code block
                                try
                                {
                                    objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");
                                }
                                catch (Exception sendemailce)
                                {
                                    LinkButton lblreviewcrono1 = (LinkButton)row.FindControl("lblreviewCRrno");
                                    string result1 = lblreviewcrono1.Text;
                                    var PONo1 = "CPO" + result1.Substring(2);
                                    errmsg = errmsg + "Error in CPO[" + PONo1 + "] - Send Email- " + sendemailce.Message.ToString();
                                }
                                #endregion SendEmail code block

                            }
                            else
                            {
                                CPOIns = lclsservice.InsertCapitalApprove(obicapital);
                                mpereview.Show();
                            }
                            #endregion multi order generation end
                        }
                        catch (Exception innerce)
                        {
                            LinkButton lblreviewcrono = (LinkButton)row.FindControl("lblreviewCRrno");
                            string result = lblreviewcrono.Text;
                            var PONo = "CPO" + result.Substring(2);
                            errmsg = errmsg + "Error in CPO[" + PONo + "] - " + innerce.Message.ToString();
                        }

                    }
                    if (errmsg != string.Empty) throw new Exception(errmsg);
                    if (CPOIns == "Saved Successfully")
                    {
                        string CapitalOrderReq = Constant.CapitalOrderReq;
                        lblmsg.Text = CapitalOrderReq;
                        btnrevwcancel.Text = "GoBack";
                        btnorderall.Text = "Order All";
                        btnGenerateOrder.Visible = false;
                        List<GetCapitalOrderCPONo> llstr = lclsservice.GetCapitalOrderCPONo(Convert.ToString(ViewState["ReqID"])).ToList();
                        grdreviewreqpo.DataSource = llstr;
                        grdreviewreqpo.DataBind();
                        mpereview.Show();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EmptyGridCapitalOrderMessage.Replace("<<MajorItemOrder>>", ""), true);
                }

                BindRequestPOGrid();
                ViewState["ReqID"] = "";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }


        public List<object> DetailsOrderReport(Int64 CRmasterID)
        {
            List<object> llstarg = new List<object>();
            BALCapitalPO llscr = new BALCapitalPO();

            llscr.CapitalItemMasterID = CRmasterID;
            llscr.LoggedinBy = defaultPage.UserId;
            List<GetCROrderContentPO> llstreview = lclsservice.GetCROrderContentPO(llscr).ToList();
            rvCRreportPDF.ProcessingMode = ProcessingMode.Local;
            rvCRreportPDF.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalPOPDF.rdlc");
            ReportParameter[] p1 = new ReportParameter[1];
            p1[0] = new ReportParameter("CapitalItemMasterID", Convert.ToString(CRmasterID));
            this.rvCRreportPDF.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("CapitalPOpdfDS", llstreview); 
            rvCRreportPDF.LocalReport.DataSources.Clear();
            rvCRreportPDF.LocalReport.DataSources.Add(datasource);
            rvCRreportPDF.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvCRreportPDF.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

            string FromEmail = llstreview[0].FromEmail;
            string ToEmail = llstreview[0].ToEmail;
            Int64 CorporateID = Convert.ToInt64(llstreview[0].CorporateID);
            Int64 FacilityID = Convert.ToInt64(llstreview[0].FacilityID);
            llstarg.Insert(0, FromEmail);
            llstarg.Insert(1, ToEmail);
            llstarg.Insert(2, bytes);
            llstarg.Insert(3, CorporateID);
            llstarg.Insert(4, FacilityID);
            return llstarg;
        }

        public void EmailSetting(Int64 CorporateID, Int64 FacilityID, string CPONo)
        {
            string Superadmin = string.Empty;
            List<GetSuperAdminDetails> lstsuperadmin = lclsservice.GetSuperAdminDetails(CorporateID, FacilityID).ToList();
            foreach (var value in lstsuperadmin)
            {
                Superadmin += "<br/>" + value.UserName + "<br/>" + value.Email + "<br/>" + value.PhoneNo;
            }
            objemail.vendorEmailcontent = string.Format("Please see the attached document for order details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br />Thank you for service <br/><br/>" + Superadmin);

            objemail.vendoremailsubject = "Major Item Order – " + CPONo;
            string displayfilename = "Major Item Order – " + CPONo + ".pdf";
        }

        protected void lbCRrno_Click(object sender, EventArgs e)
        {
            try
            {
                modalreviewreq.Show();
                string s = string.Empty;
                Int64 CRID = 0;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                CRID = Convert.ToInt64(gvrow.Cells[1].Text);
                HdnMasterID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetCapitalItemMaster> lstCR = lclsservice.GetCapitalItemMaster().Where(a => a.CapitalItemMasterID == Convert.ToInt64(HdnMasterID.Value)).ToList();
                lblcorporate.Text = lstCR[0].CorporateName;
                lblfacility.Text = lstCR[0].FacilityDescription;
                lblvendor.Text = lstCR[0].VendorDescription;
                lblship.Text = lstCR[0].Shipping;
                lblreqtype.Text = lstCR[0].Type;
                lblreqdate.Text = Convert.ToDateTime(lstCR[0].CreatedOn).ToString("MM/dd/yyyy");
                lblmprreview.Text = lstCR[0].CRNo;
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetCapitalItemDetails> llstCRDetails = lclsservice.GetCapitalItemDetails(Convert.ToInt64(HdnMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();
                grdreview.DataSource = llstCRDetails;
                grdreview.DataBind();
                int i = 0;
                foreach (GridViewRow grdfs in grdreview.Rows)
                {
                    Label lblEquipCategory = (Label)grdfs.FindControl("lblequrecat");
                    Label lblEquipList = (Label)grdfs.FindControl("lblequrelst");
                    Label lblser = (Label)grdfs.FindControl("lblser");
                    Label txtqty = (Label)grdfs.FindControl("lblOrderQuantity");
                    Label txtppq = (Label)grdfs.FindControl("lblrevppqty");
                    Label txttotprice = (Label)grdfs.FindControl("lblTotalPrice");
                    Label txtreason = (Label)grdfs.FindControl("lblreason");
                    Label lbCRMasterID = (Label)grdfs.FindControl("lbCRMasterID");
                    Label lbCRDetailsID = (Label)grdfs.FindControl("lbCRDetailsID");

                    lblEquipCategory.Text = llstCRDetails[i].EquipmentSubCategory;
                    lblEquipList.Text = llstCRDetails[i].EquipementList;
                    lblser.Text = llstCRDetails[i].SerialNo;
                    txtqty.Text = llstCRDetails[i].OrderQuantity.ToString();
                    txtppq.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].PricePerUnit));
                    txttotprice.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].TotalPrice));
                    txtreason.Text = llstCRDetails[i].Reason.ToString();
                    lbCRMasterID.Text = llstCRDetails[i].CapitalItemMasterID.ToString();
                    lbCRDetailsID.Text = llstCRDetails[i].CapitalItemDetailsID.ToString();
                    i++;
                }
                Label lblshippingcost = grdreview.FooterRow.FindControl("lblshippingcost") as Label;
                Label lbltax = grdreview.FooterRow.FindControl("lbltax") as Label;
                Label lbltotalcost = grdreview.FooterRow.FindControl("lbltotalcost") as Label;
                lblshippingcost.Text = lstCR[0].ShippingCost;
                lbltax.Text = lstCR[0].Tax;
                lbltotalcost.Text = Convert.ToString(string.Format("{0:F2}", lstCR[0].TotalCost));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void lbcrono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 CRmasterID;
                CRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbcrono = (LinkButton)gvrow.FindControl("lbcrono");
                List<object> llstresult = DetailsOrderReport(CRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "CapitalOrder" + guid + ".pdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, _sessionPDFFileName);
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.Write("");
                }
                FileStream fs = new FileStream(path, FileMode.Open);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                //path = Path.Combine(path,_sessionPDFFileName);
                ShowPDFFile(path);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
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
                    //Response.TransmitFile(_sessionPDFFileName);
                }
                else
                {
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }

        }
        protected void imgprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                string CRmasterID = string.Empty;
                CRmasterID = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<object> llstresult = SearchOrderReport(CRmasterID);
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Major Item Order" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }

        }

        public List<object> SearchOrderReport(string CRmasterID)
        {
            string smedmasterIds = string.Empty;
            List<object> llstarg = new List<object>();
            List<GetCapitalPOReport> llstreview = new List<GetCapitalPOReport>();
            if (CRmasterID == "")
            {
                foreach (GridViewRow row in grdCRRequestPO.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (smedmasterIds == string.Empty)
                            smedmasterIds = row.Cells[1].Text;
                        else
                            smedmasterIds = smedmasterIds + "," + row.Cells[1].Text;
                    }
                }
                llstreview = lclsservice.GetCapitalPOReport(null, smedmasterIds, defaultPage.UserId, defaultPage.UserId).ToList();
            }
            else
            {
                llstreview = lclsservice.GetCapitalPOReport(CRmasterID, null, defaultPage.UserId, defaultPage.UserId).ToList();
            }

            rvCRPOreport.ProcessingMode = ProcessingMode.Local;
            rvCRPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalPOReport.rdlc");
            Int64 r = defaultPage.UserId;
            ReportParameter[] p1 = new ReportParameter[3];
            p1[0] = new ReportParameter("CapitalItemMasterID", "0");
            p1[1] = new ReportParameter("SearchFilters", "test");
            p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

            this.rvCRPOreport.LocalReport.SetParameters(p1);
            ReportDataSource datasource = new ReportDataSource("CapitalPoReportDS", llstreview);
            rvCRPOreport.LocalReport.DataSources.Clear();
            rvCRPOreport.LocalReport.DataSources.Add(datasource);
            rvCRPOreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvCRPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                BALCapitalPO llstcr = new BALCapitalPO();

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
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-1)).ToString("MM/dd/yyyy");
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
                List<SearchCapitalPO> lstCRMaster = lclsservice.SearchCapitalPO(llstcr).ToList();
                rvCRPOreport.ProcessingMode = ProcessingMode.Local;
                rvCRPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/CapitalOrderSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetCapitalsummaryDS", lstCRMaster);
                rvCRPOreport.LocalReport.DataSources.Clear();
                rvCRPOreport.LocalReport.DataSources.Add(datasource);
                rvCRPOreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvCRPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "CapitalSummary" + guid + ".pdf";
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
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //_sessionPDFFileName = "Major Item Order" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
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

        protected void imgsend_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 CRmasterID;
                CRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lbcrono = (LinkButton)gvrow.FindControl("lbcrono");
                string CPONo = lbcrono.Text;
                List<object> llstresult = DetailsOrderReport(CRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                MemoryStream attachstream = new MemoryStream(bytes);
                Int64 CorporateID = Convert.ToInt64(llstresult[3].ToString());
                Int64 FacilityID = Convert.ToInt64(llstresult[4].ToString());
                objemail.FromEmail = llstresult[0].ToString();
                objemail.ToEmail = llstresult[1].ToString();
                EmailSetting(CorporateID, FacilityID, CPONo);
                objemail.SendEmailPDFContent(objemail.FromEmail, objemail.ToEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, objemail.vendoremailsubject + ".pdf");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderemailMessage.Replace("<<MajorItemOrder>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnrevwcancel_Click(object sender, EventArgs e)
        {
            mpereview.Hide();
            lblmsg.Text = "";
        }

        protected void imgreviewprint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 CRmasterID;
                CRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                List<object> llstresult = DetailsOrderReport(CRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();

                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Major Item Order" + guid + ".pdf";
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
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void lblreviewcrono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                Int64 CRmasterID;
                string result = string.Empty;
                CRmasterID = Convert.ToInt64(gvrow.Cells[1].Text.Trim().Replace("&nbsp;", ""));
                LinkButton lblreviewcrono = (LinkButton)gvrow.FindControl("lblreviewcrono");
                List<object> llstresult = DetailsOrderReport(CRmasterID);
                byte[] bytes = (byte[])llstresult[2];
                // MemoryStream attachstream = new MemoryStream(bytes);               
                Guid guid = Guid.NewGuid();
                _sessionPDFFileName = "Major Item Order" + guid + ".pdf";
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void grdreviewreqpo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtremarks = (TextBox)e.Row.FindControl("txtremarks");
                DropDownList drpaction = (DropDownList)e.Row.FindControl("drpaction");
                Label lblaction = (Label)e.Row.FindControl("lblaction");
                //Label lblaudit = (Label)e.Row.FindControl("lblaudit");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                DataTable dt = (DataTable)ViewState["OldStatus"];
                string Status = string.Empty;
                Status = e.Row.Cells[12].Text;
                string Oldstatus = dt.Rows[e.Row.RowIndex]["Oldstatus"].ToString();
                if ((Oldstatus == OldStatusPend && lblaction.Text == actionApprove) || (Oldstatus == OldStatusPend && lblaction.Text == actionOrder))
                {
                    txtremarks.Visible = false;
                }
                else if (Oldstatus == StatusApprove && lblaction.Text == actionOrder)
                {
                    txtremarks.Visible = false;
                }
                else
                {
                    txtremarks.Visible = true;
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

        protected void lblreviewCRrno_Click(object sender, EventArgs e)
        {
            try
            {
                modalreviewreq.Show();
                string s = string.Empty;
                Int64 CRID = 0;
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                CRID = Convert.ToInt64(gvrow.Cells[1].Text);
                HdnMasterID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                List<GetCapitalItemMaster> lstCR = lclsservice.GetCapitalItemMaster().Where(a => a.CapitalItemMasterID == Convert.ToInt64(HdnMasterID.Value)).ToList();
                lblcorporate.Text = lstCR[0].CorporateName;
                lblfacility.Text = lstCR[0].FacilityDescription;
                lblvendor.Text = lstCR[0].VendorDescription;
                lblship.Text = lstCR[0].Shipping;
                lblreqtype.Text = lstCR[0].Type;
                lblreqdate.Text = Convert.ToString(lstCR[0].CreatedOn);
                lblmprreview.Text = lstCR[0].CRNo;
                string LockTimeOut = "";
                LockTimeOut = System.Configuration.ConfigurationManager.AppSettings["LockTimeOut"].ToString();
                List<GetCapitalItemDetails> llstCRDetails = lclsservice.GetCapitalItemDetails(Convert.ToInt64(HdnMasterID.Value), defaultPage.UserId, Convert.ToInt64(LockTimeOut)).ToList();
                grdreview.DataSource = llstCRDetails;
                grdreview.DataBind();
                int i = 0;
                foreach (GridViewRow grdfs in grdreview.Rows)
                {
                    Label lblEquipCategory = (Label)grdfs.FindControl("lblequrecat");
                    Label lblEquipList = (Label)grdfs.FindControl("lblequrelst");
                    Label lblser = (Label)grdfs.FindControl("lblser");
                    Label txtqty = (Label)grdfs.FindControl("lblOrderQuantity");
                    Label txtppq = (Label)grdfs.FindControl("lblrevppqty");
                    Label txttotprice = (Label)grdfs.FindControl("lblTotalPrice");
                    Label txtreason = (Label)grdfs.FindControl("lblreason");
                    Label lbCRMasterID = (Label)grdfs.FindControl("lbCRMasterID");
                    Label lbCRDetailsID = (Label)grdfs.FindControl("lbCRDetailsID");

                    lblEquipCategory.Text = llstCRDetails[i].EquipmentSubCategory;
                    lblEquipList.Text = llstCRDetails[i].EquipementList;
                    lblser.Text = llstCRDetails[i].SerialNo;
                    txtqty.Text = llstCRDetails[i].OrderQuantity.ToString();
                    txtppq.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].PricePerUnit));
                    txttotprice.Text = Convert.ToString(string.Format("{0:F2}", llstCRDetails[i].TotalPrice));
                    txtreason.Text = llstCRDetails[i].Reason.ToString();
                    lbCRMasterID.Text = llstCRDetails[i].CapitalItemMasterID.ToString();
                    lbCRDetailsID.Text = llstCRDetails[i].CapitalItemDetailsID.ToString();
                    i++;
                }
                Label lblshippingcost = grdreview.FooterRow.FindControl("lblshippingcost") as Label;
                Label lbltax = grdreview.FooterRow.FindControl("lbltax") as Label;
                Label lbltotalcost = grdreview.FooterRow.FindControl("lbltotalcost") as Label;
                lblshippingcost.Text = lstCR[0].ShippingCost;
                lbltax.Text = lstCR[0].Tax;
                lbltotalcost.Text = Convert.ToString(string.Format("{0:F2}", lstCR[0].TotalCost));
                mpereview.Show();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClearDetails();
            BindRequestPOGrid();
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
                DivCapitalorder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalItemErrorMessage.Replace("<<MajorItem>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiCorpClose_Click(object sender, EventArgs e)
        {
            DivMultiCorp.Style.Add("display", "none");
            DivCapitalorder.Attributes["class"] = "mypanel-body";
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
                DivCapitalorder.Attributes["class"] = "mypanel-body";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.CapitalOrderErrorMessage.Replace("<<MajorItemOrder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnMultiFacClose_Click(object sender, EventArgs e)
        {
            DivFacCorp.Style.Add("display", "none");
            DivCapitalorder.Attributes["class"] = "mypanel-body";
        }

        protected void lnkMultiCorp_Click(object sender, EventArgs e)
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                DivMultiCorp.Style.Add("display", "block");
                DivCapitalorder.Attributes["class"] = "Upopacity";
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
                    DivCapitalorder.Attributes["class"] = "Upopacity";
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






