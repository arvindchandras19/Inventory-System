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
using System.Data;
using System.Globalization;

namespace Inventory
{
    public partial class EndingInventory : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALEndingInventory llstEndingInventory = new BALEndingInventory();
        string lstmessage = string.Empty;
        bool Error = true;
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
                scriptManager.RegisterPostBackControl(this.grdEndingInven);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningServiceRequestPOMessage.Replace("<<ServiceRequestPO>>", "No changes made in the Service Request PO. "), true);
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        //lclsservice.SyncServiceReceivingorder();
                        BindCorporate();
                        drpcorsearch.SelectedValue = defaultPage.CorporateID.ToString();
                        BindFacility();
                        drpfacilitysearch.SelectedValue = defaultPage.FacilityID.ToString();
                        BindVendor();
                        BindCategory();
                        HddUserRoleID.Value = defaultPage.RoleID.ToString();
                        if (defaultPage.RoleID == 1)
                        {
                            drpcorsearch.Enabled = true;
                            drpfacilitysearch.Enabled = true;
                            chkNewFaciltiy.Enabled = true;
                        }
                        else
                        {
                            drpcorsearch.Enabled = false;
                            drpfacilitysearch.Enabled = false;
                            chkNewFaciltiy.Enabled = false;
                        }
                        txtMonth.Text = DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + "-" + DateTime.Now.Year.ToString();
                        BindEndingInvenGrid("");
                        if (defaultPage.RoleID != 1 && defaultPage.StocksOrInventory_Ending_Page_Edit == false && defaultPage.StocksOrInventory_Ending_Page_View == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                        if (defaultPage.RoleID != 1 && defaultPage.StocksOrInventory_Ending_Page_Edit == false && defaultPage.StocksOrInventory_Ending_Page_View == true)
                        {
                            btnSave.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
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
                    //drpcorsearch.DataTextField = "CorporateName";
                    //drpcorsearch.DataValueField = "CorporateID";
                    //drpcorsearch.DataBind();

                    //drpcorsearch.Items.Insert(0, lst);
                    //drpcorsearch.SelectedIndex = 0;
                }
                else
                {
                    lstcrop = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcorsearch.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();

                    //ListItem lst = new ListItem();
                    //lst.Value = "0";
                    //lst.Text = "--Select Corporate--";
                    //drpcorsearch.Items.Insert(0, lst);
                    //drpcorsearch.SelectedIndex = 0;
                }

                drpcorsearch.DataTextField = "CorporateName";
                drpcorsearch.DataValueField = "CorporateID";
                drpcorsearch.DataBind();
                //foreach (ListItem lst in drpcorsearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
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
                    //foreach (ListItem lst in drpcorsearch.Items)
                    //{
                    //    if (lst.Selected && drpcorsearch.SelectedValue != "All")
                    //    {
                    //        SB.Append(lst.Value + ',');
                    //    }
                    //}
                    //if (SB.Length > 0)
                    //    FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    if (defaultPage.RoleID == 1)
                    {
                        // Search Drop Down
                        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacility(Convert.ToInt64(drpcorsearch.SelectedValue)).ToList();
                        drpfacilitysearch.DataTextField = "FacilityDescription";
                        drpfacilitysearch.DataValueField = "FacilityID";
                        drpfacilitysearch.DataBind();
                    }
                    else
                    {
                        drpfacilitysearch.DataSource = lclsservice.GetCorporateFacilityByUserID(defaultPage.UserId).Where(a => a.CorporateID == Convert.ToInt64(drpcorsearch.SelectedValue)).ToList().Select(a => new { a.FacilityID, a.FacilityName }).Distinct();
                        drpfacilitysearch.DataTextField = "FacilityName";
                        drpfacilitysearch.DataValueField = "FacilityID";
                        drpfacilitysearch.DataBind();
                    }


                }
                //foreach (ListItem lst in drpfacilitysearch.Items)
                //{
                //    lst.Attributes.Add("class", "selected");
                //    lst.Selected = true;
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }
        #endregion

        protected void drpcorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindFacility();
                BindVendor();
                BindCategory();
                btnSave.Enabled = false;
                DIVgrdEndingInven.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void drpfacilitysearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindCategory();
            BindVendor();
            btnSave.Enabled = false;
            DIVgrdEndingInven.Style.Add("display", "none");
        }


        protected void lnkMultiVendor_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (drpfacilitysearch.SelectedValue != "")
                {
                    List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                    lstvendordetails = lclsservice.GetFacilityVendorAccount(drpfacilitysearch.SelectedItem.Text).Where(A => (A.RegularSupplies == true) && (A.IsActive == true)).ToList();

                    GrdMultiVendor.DataSource = lstvendordetails;                    
                    GrdMultiVendor.DataBind();

                    foreach (ListItem lst1 in drpvendorsearch.Items)
                    {
                        if (lst1.Selected == true)
                        {

                            foreach (GridViewRow row in GrdMultiVendor.Rows)
                            {
                                Label lblVendorID = (Label)row.FindControl("lblVendorID");
                                CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");
                                CheckBox ChkAllVendor = GrdMultiVendor.HeaderRow.FindControl("ChkAllVendor") as CheckBox;

                                if (lst1.Value == lblVendorID.Text)
                                {
                                    chkmultiVendor.Checked = true;
                                    i++;
                                }

                                if (i == drpvendorsearch.Items.Count)
                                {
                                    ChkAllVendor.Checked = true;
                                }

                            }
                        }
                    }

                    DivMultiVendor.Style.Add("display", "block");
                    UploadOpacity.Attributes["class"] = "Upopacity";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void lnkClearVendor_Click(object sender, EventArgs e)
        {
            BindVendor();
        }

        protected void lnkClearAllVendor_Click(object sender, EventArgs e)
        {

            foreach (ListItem lst in drpvendorsearch.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }
            foreach (ListItem lst in drpItemCategory.Items)
            {
                lst.Attributes.Add("class", "");
                lst.Selected = false;
            }

        }

        protected void ChkAllVendor_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkAllVendor = (CheckBox)GrdMultiVendor.HeaderRow.FindControl("ChkAllVendor");

            foreach (GridViewRow row in GrdMultiVendor.Rows)

            {

                CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");

                if (ChkAllVendor.Checked == true)

                {
                    chkmultiVendor.Checked = true;
                }
                else

                {
                    chkmultiVendor.Checked = false;
                }

            }
        }

        protected void btnMultiVendorselect_Click(object sender, EventArgs e)
        {
            foreach (ListItem lst1 in drpvendorsearch.Items)
            {
                lst1.Attributes.Add("class", "");
                lst1.Selected = false;
            }
            foreach (GridViewRow row in GrdMultiVendor.Rows)
            {
                CheckBox chkmultiVendor = (CheckBox)row.FindControl("chkmultiVendor");
                Label lblVendorID = (Label)row.FindControl("lblVendorID");

                if (chkmultiVendor.Checked == true)
                {
                    foreach (ListItem lst1 in drpvendorsearch.Items)
                    {
                        if (lst1.Value == lblVendorID.Text)
                        {
                            lst1.Attributes.Add("class", "selected");
                            lst1.Selected = true;
                        }
                    }
                }
            }
            BindCategory();
            DivMultiVendor.Style.Add("display", "none");
            UploadOpacity.Attributes["class"] = "mypanel-body";
        }

        protected void btnMultiVendorClose_Click(object sender, EventArgs e)
        {
            DivMultiVendor.Style.Add("display", "none");
            UploadOpacity.Attributes["class"] = "mypanel-body";
        }

        /// <summary>
        /// Bind the Vendor details from vendor master table to dropdown control 
        /// </summary>
        #region Bind Vendor Values
        public void BindVendor()
        {
            try
            {
                List<GetFacilityVendorAccount> lstvendordetails = new List<GetFacilityVendorAccount>();
                // Search Drop Down    
                if (drpfacilitysearch.SelectedValue != "")
                {
                    lstvendordetails = lclsservice.GetFacilityVendorAccount(drpfacilitysearch.SelectedItem.Text).Where(A => (A.RegularSupplies == true) && (A.IsActive == true)).ToList();                    
                    drpvendorsearch.DataSource = lstvendordetails.Select(a => new { a.VendorID, a.VendorDescription }).Distinct();
                    drpvendorsearch.DataTextField = "VendorDescription";
                    drpvendorsearch.DataValueField = "VendorID";
                    drpvendorsearch.DataBind();

                    //ListItem lst = new ListItem();
                    //lst.Value = "All";
                    //lst.Text = "All";
                    //drpvendorsearch.Items.Insert(0, lst);
                    //drpvendorsearch.SelectedIndex = 0;
                    foreach (ListItem lst in drpvendorsearch.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }

                    BindCategory();
                }
                else
                {
                    drpvendorsearch.DataSource = lstvendordetails;
                    drpvendorsearch.DataTextField = "VendorDescription";
                    drpvendorsearch.DataValueField = "VendorID";
                    drpvendorsearch.DataBind();
                }
               

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }
        #endregion

        protected void drpvendorsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ListItem lst in drpvendorsearch.Items)
            {
                if (lst.Selected == true)
                {
                    i++;
                }
            }
            if (i == 1)
            {
                BindCategory();
                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    if (lst.Selected == true)
                    {
                        HddListVendorID.Value = lst.Value;
                    }
                }
            }
            else if (i == 2)
            {
                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    if (HddListVendorID.Value == lst.Value)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }

                }
            }
            else
            {
                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "");
                    lst.Selected = false;
                    HddListVendorID.Value = "";
                }
                BindCategory();
            }            
            btnSave.Enabled = false;
            DIVgrdEndingInven.Style.Add("display", "none");
        }


        public void BindCategory()
        {
            try
            {
                if (drpvendorsearch.SelectedValue != "")
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

                    InventoryServiceClient lclsservice = new InventoryServiceClient();
                    List<GetCategoryByListVendorID> lstcategory = new List<GetCategoryByListVendorID>();
                    //llstEndingInventory.FacilityID = Convert.ToInt64(drpfacilitysearch.SelectedValue);
                    lstcategory = lclsservice.GetCategoryByListVendorID(FinalString).ToList();

                    drpItemCategory.DataSource = lstcategory;
                    drpItemCategory.DataValueField = "CategoryID";
                    drpItemCategory.DataTextField = "CategoryName";
                    drpItemCategory.DataBind();

                    foreach (ListItem lst in drpItemCategory.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }

        }



        public void BindEndingInvenGrid(string ChkAdd)
        {
            try
            {

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstEndingInventory.ListCorporateID = "ALL";
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

                    llstEndingInventory.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstEndingInventory.ListFacilityID = "ALL";
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
                    llstEndingInventory.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();

                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    if (lst.Selected && drpvendorsearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                llstEndingInventory.ListVendorID = FinalString;
                FinalString = "";
                SB.Clear();

                if (drpItemCategory.SelectedValue == "All")
                {
                    llstEndingInventory.ListStatus = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpItemCategory.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstEndingInventory.ListCategoryID = FinalString;
                }
                SB.Clear();
                DateTime MonthYear = new DateTime();
                if (txtMonth.Text == "")
                {
                    MonthYear = DateTime.Now;
                }
                else
                {
                    MonthYear = Convert.ToDateTime(txtMonth.Text);
                }
                llstEndingInventory.MonthYear = MonthYear;
                llstEndingInventory.IsNewFacility = false;
                llstEndingInventory.LoggedinBy = defaultPage.UserId;
                List<SearchEndingInventory> lstSRPOMaster = new List<SearchEndingInventory>();

                if (ChkAdd == "Add")
                {
                    llstEndingInventory.ItemType = "NEW";
                    lstSRPOMaster = lclsservice.SearchEndingInventory(llstEndingInventory).ToList();
                    GrdAddnewItem.DataSource = lstSRPOMaster;
                    GrdAddnewItem.DataBind();                    
                }
                else
                {
                    lstSRPOMaster = lclsservice.SearchEndingInventory(llstEndingInventory).ToList();
                    grdEndingInven.DataSource = lstSRPOMaster;
                    grdEndingInven.DataBind();
                   
                }

                if (lstSRPOMaster.Count > 0)
                {
                    if (lstSRPOMaster[0].EndingInvenID == 0)
                    {
                        if (llstEndingInventory.ItemType != "NEW")
                        {
                            txtnoofvisit.Text = "";
                        }
                    }                        
                    else
                        txtnoofvisit.Text = lstSRPOMaster[0].Noofvisit.ToString();
                }



                if (grdEndingInven.PageCount == 0)
                {
                    chkNewFaciltiy.Checked = false;
                    btnaddnewitem.Enabled = false;
                    btnSave.Enabled = false;
                    if (llstEndingInventory.ItemType != "NEW")
                    {
                        txtnoofvisit.Text = "";
                    }                    
                    txtnoofvisit.Enabled = false;
                }
                else
                {
                    foreach (GridViewRow row in grdEndingInven.Rows)
                    {
                        TextBox txtbeginv = (TextBox)row.FindControl("txtbeginv");
                        TextBox txtexpiredmeds = (TextBox)row.FindControl("txtexpiredmeds");
                        TextBox txtendinv = (TextBox)row.FindControl("txtendinv");
                        TextBox lblMonthlyUsage = (TextBox)row.FindControl("lblMonthlyUsage");
                        LinkButton lblReceiveingInven = (LinkButton)row.FindControl("lblReceiveingInven");
                        LinkButton lblTransferIn = (LinkButton)row.FindControl("lblTransferIn");
                        LinkButton lnkTransferOut = (LinkButton)row.FindControl("lnkTransferOut");
                        llstEndingInventory.ReceiveingInven = Convert.ToInt64(lblReceiveingInven.Text);
                        llstEndingInventory.TransferIn = Convert.ToInt64(lblTransferIn.Text);
                        llstEndingInventory.TransferOut = Convert.ToInt64(lnkTransferOut.Text);
                        if (txtbeginv.Text == "")
                        {
                            llstEndingInventory.BeggingInven = 0;
                        }
                        else
                        {
                            llstEndingInventory.BeggingInven = Convert.ToInt64(txtbeginv.Text);
                        }
                        txtbeginv.Text = llstEndingInventory.BeggingInven.ToString();
                        if (txtexpiredmeds.Text == "")
                        {
                            llstEndingInventory.ExpiredMeds = 0;
                        }
                        else
                        {
                            llstEndingInventory.ExpiredMeds = Convert.ToInt64(txtexpiredmeds.Text);
                        }
                        txtexpiredmeds.Text = llstEndingInventory.ExpiredMeds.ToString();
                        if (txtendinv.Text == "")
                        {
                            llstEndingInventory.EndingInven = 0;
                        }
                        else
                        {
                            llstEndingInventory.EndingInven = Convert.ToInt64(txtendinv.Text);
                        }
                        txtendinv.Text = llstEndingInventory.EndingInven.ToString();
                        if (lblMonthlyUsage.Text == "")
                        {
                            llstEndingInventory.MonthlyUsage = 0;
                        }
                        else
                        {
                            llstEndingInventory.MonthlyUsage = Convert.ToInt64(lblMonthlyUsage.Text);
                        }
                        llstEndingInventory.MonthlyUsage = llstEndingInventory.BeggingInven + llstEndingInventory.ReceiveingInven + llstEndingInventory.TransferIn -
                                llstEndingInventory.TransferOut - llstEndingInventory.ExpiredMeds - llstEndingInventory.EndingInven;

                        lblMonthlyUsage.Text = llstEndingInventory.MonthlyUsage.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = true;
                DivGrdErrorMessage.Style.Add("display", "none");
                DIVgrdEndingInven.Style.Add("display", "block");
                BindEndingInvenGrid("");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }



        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                MPAddnewItem.Hide();
                //drpcorsearch.SelectedIndex = 0;
                //drpfacilitysearch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        public void clearDetails()
        {
            try
            {
                drpcorsearch.ClearSelection();
                BindCorporate();
                drpcorsearch.SelectedValue = defaultPage.CorporateID.ToString();
                drpfacilitysearch.ClearSelection();
                BindFacility();
                drpfacilitysearch.SelectedValue = defaultPage.FacilityID.ToString();
                txtMonth.Text = DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + "-" + DateTime.Now.Year.ToString();
                BindCategory();
                DivGrdErrorMessage.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("EndingInventory.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorMessage = string.Empty;
                foreach (GridViewRow row in grdEndingInven.Rows)
                {
                    TextBox txtbeginv = (TextBox)row.FindControl("txtbeginv");
                    TextBox txtexpiredmeds = (TextBox)row.FindControl("txtexpiredmeds");
                    TextBox txtendinv = (TextBox)row.FindControl("txtendinv");
                    TextBox lblMonthlyUsage = (TextBox)row.FindControl("lblMonthlyUsage");
                    LinkButton lblReceiveingInven = (LinkButton)row.FindControl("lblReceiveingInven");
                    LinkButton lblTransferIn = (LinkButton)row.FindControl("lblTransferIn");
                    LinkButton lnkTransferOut = (LinkButton)row.FindControl("lnkTransferOut");
                    llstEndingInventory.ReceiveingInven = Convert.ToInt64(lblReceiveingInven.Text);
                    llstEndingInventory.TransferIn = Convert.ToInt64(lblTransferIn.Text);
                    llstEndingInventory.TransferOut = Convert.ToInt64(lnkTransferOut.Text);
                    if (txtbeginv.Text == "")
                    {
                        llstEndingInventory.BeggingInven = 0;
                    }
                    else
                    {
                        llstEndingInventory.BeggingInven = Convert.ToInt64(txtbeginv.Text);
                    }
                    txtbeginv.Text = llstEndingInventory.BeggingInven.ToString();
                    if (txtexpiredmeds.Text == "")
                    {
                        llstEndingInventory.ExpiredMeds = 0;
                    }
                    else
                    {
                        llstEndingInventory.ExpiredMeds = Convert.ToInt64(txtexpiredmeds.Text);
                    }
                    txtexpiredmeds.Text = llstEndingInventory.ExpiredMeds.ToString();
                    if (txtendinv.Text == "")
                    {
                        llstEndingInventory.EndingInven = 0;
                    }
                    else
                    {
                        llstEndingInventory.EndingInven = Convert.ToInt64(txtendinv.Text);
                    }
                    txtendinv.Text = llstEndingInventory.EndingInven.ToString();
                    if (lblMonthlyUsage.Text == "")
                    {
                        llstEndingInventory.MonthlyUsage = 0;
                    }
                    else
                    {
                        llstEndingInventory.MonthlyUsage = Convert.ToInt64(lblMonthlyUsage.Text);
                    }
                    llstEndingInventory.MonthlyUsage = llstEndingInventory.BeggingInven + llstEndingInventory.ReceiveingInven + llstEndingInventory.TransferIn -
                            llstEndingInventory.TransferOut - llstEndingInventory.ExpiredMeds - llstEndingInventory.EndingInven;

                    lblMonthlyUsage.Text = llstEndingInventory.MonthlyUsage.ToString();
                    if (Convert.ToInt64(llstEndingInventory.MonthlyUsage) < 0)
                    {
                        ErrorMessage = "Dsiplay Message";
                        DivGrdErrorMessage.Style.Add("display", "block");
                        lblMonthlyUsage.Style.Add("border", "2px solid red");
                    }
                }
                string lstmessage = string.Empty;
                if (ErrorMessage == "")
                {
                    foreach (GridViewRow row in grdEndingInven.Rows)
                    {
                        Label lblEndingInvenID = (Label)row.FindControl("lblEndingInvenID");
                        Label lblCorporateID = (Label)row.FindControl("lblCorporateID");
                        Label lblFacilityID = (Label)row.FindControl("lblFacilityID");
                        Label lblCatagoryID = (Label)row.FindControl("lblCatagoryID");
                        TextBox txtbeginv = (TextBox)row.FindControl("txtbeginv");
                        LinkButton lblReceiveingInven = (LinkButton)row.FindControl("lblReceiveingInven");
                        LinkButton lblTransferIn = (LinkButton)row.FindControl("lblTransferIn");
                        LinkButton lnkTransferOut = (LinkButton)row.FindControl("lnkTransferOut");
                        TextBox txtexpiredmeds = (TextBox)row.FindControl("txtexpiredmeds");
                        TextBox txtendinv = (TextBox)row.FindControl("txtendinv");
                        TextBox lblMonthlyUsage = (TextBox)row.FindControl("lblMonthlyUsage");

                        llstEndingInventory.EndingInvenID = Convert.ToInt64(lblEndingInvenID.Text);
                        llstEndingInventory.CorporateID = Convert.ToInt64(lblCorporateID.Text);
                        llstEndingInventory.FacilityID = Convert.ToInt64(lblFacilityID.Text);
                        llstEndingInventory.CategoryID = Convert.ToInt64(lblCatagoryID.Text);
                        llstEndingInventory.ItemID = Convert.ToInt64(row.Cells[1].Text);
                        llstEndingInventory.ItemDescription = HttpUtility.HtmlDecode(row.Cells[3].Text);
                        llstEndingInventory.QtyPack = Convert.ToInt64(row.Cells[4].Text);
                        llstEndingInventory.UOM = Convert.ToInt64(row.Cells[5].Text);
                        if (txtbeginv.Text == "")
                        {
                            llstEndingInventory.BeggingInven = 0;
                        }
                        else
                        {
                            llstEndingInventory.BeggingInven = Convert.ToInt64(txtbeginv.Text);
                        }
                        llstEndingInventory.ReceiveingInven = Convert.ToInt64(lblReceiveingInven.Text);
                        llstEndingInventory.TransferIn = Convert.ToInt64(lblTransferIn.Text);
                        llstEndingInventory.TransferOut = Convert.ToInt64(lnkTransferOut.Text);

                        if (txtexpiredmeds.Text == "")
                        {
                            llstEndingInventory.ExpiredMeds = 0;
                        }
                        else
                        {
                            llstEndingInventory.ExpiredMeds = Convert.ToInt64(txtexpiredmeds.Text);
                        }
                        if (txtendinv.Text == "")
                        {
                            llstEndingInventory.EndingInven = 0;
                        }
                        else
                        {
                            llstEndingInventory.EndingInven = Convert.ToInt64(txtendinv.Text);
                        }
                        if (txtbeginv.Text == "" || txtexpiredmeds.Text == "" || txtendinv.Text == "")
                        {
                            llstEndingInventory.MonthlyUsage = 0;
                        }
                        else
                        {

                            llstEndingInventory.MonthlyUsage = llstEndingInventory.BeggingInven + llstEndingInventory.ReceiveingInven + llstEndingInventory.TransferIn -
                                llstEndingInventory.TransferOut - llstEndingInventory.ExpiredMeds - llstEndingInventory.EndingInven;
                        }
                        if (txtMonth.Text == "")
                        {
                            llstEndingInventory.MonthYear = DateTime.Now;
                        }
                        else
                        {
                            string MonthYear = "01-" + txtMonth.Text;
                            llstEndingInventory.MonthYear = Convert.ToDateTime(MonthYear);
                        }

                        llstEndingInventory.CreatedBy = defaultPage.UserId;
                        llstEndingInventory.LastModifiedBy = defaultPage.UserId;

                        if(txtnoofvisit.Text != "")
                        llstEndingInventory.Noofvisit = Convert.ToInt64(txtnoofvisit.Text);
                        else
                            llstEndingInventory.Noofvisit = Convert.ToInt64(0);

                        if (chkNewFaciltiy.Checked == true)
                        {
                            llstEndingInventory.IsNewFacility = true;
                            llstEndingInventory.IsNewRecord = true;
                        }
                        else
                        {
                            llstEndingInventory.IsNewFacility = false;
                            llstEndingInventory.IsNewRecord = false;
                        }

                        if (lblEndingInvenID.Text == "0")
                        {
                            lstmessage = lclsservice.InsertEndingInventory(llstEndingInventory);
                        }
                        else
                        {
                            lstmessage = lclsservice.UpdateEndingInventory(llstEndingInventory);
                        }
                        lblMonthlyUsage.Style.Remove("border");
                    }

                    if (lstmessage == "Saved Successfully")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventorySaveMessage, true);
                        BindEndingInvenGrid("");
                        DivGrdErrorMessage.Style.Add("display", "none");
                    }
                    else if (lstmessage == "Updated Successfully")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryUpdateMessage, true);
                        BindEndingInvenGrid("");
                        DivGrdErrorMessage.Style.Add("display", "none");

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningEndingInventoryMinusMessage, true);
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void grdEndingInven_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lblReceiveingInven = (LinkButton)e.Row.FindControl("lblReceiveingInven");
                    LinkButton lblTransferIn = (LinkButton)e.Row.FindControl("lblTransferIn");
                    LinkButton lnkTransferOut = (LinkButton)e.Row.FindControl("lnkTransferOut");
                    TextBox txtbeginv = (TextBox)e.Row.FindControl("txtbeginv");
                    TextBox txtexpiredmeds = (TextBox)e.Row.FindControl("txtexpiredmeds");
                    TextBox txtendinv = (TextBox)e.Row.FindControl("txtendinv");
                    Label lblEndingInvenID = (Label)e.Row.FindControl("lblEndingInvenID");
                    Label lblNewFacility = (Label)e.Row.FindControl("lblNewFacility");

                    if (lblReceiveingInven.Text == "0")
                    {
                        lblReceiveingInven.Enabled = false;
                    }
                    if (lblTransferIn.Text == "0")
                    {
                        lblTransferIn.Enabled = false;
                    }
                    if (lnkTransferOut.Text == "0")
                    {
                        lnkTransferOut.Enabled = false;
                    }


                    if (defaultPage.RoleID == 1)
                    {
                        var today = DateTime.Today;
                        var month = new DateTime(today.Year, today.Month, 1);
                        var first = month.AddMonths(-1);
                        var last = first.AddDays(-1);
                        DateTime nextMonth = Convert.ToDateTime(last);

                        if (Convert.ToDateTime(txtMonth.Text) < nextMonth)
                        {
                            txtbeginv.Enabled = false;
                            txtexpiredmeds.Enabled = false;
                            txtendinv.Enabled = false;
                            txtnoofvisit.Enabled = false;
                            btnaddnewitem.Enabled = false;
                            btnSave.Enabled = false;                            
                            // grdEndingInven.Enabled = false;
                        }
                        else
                        {
                            txtbeginv.Enabled = true;
                            txtexpiredmeds.Enabled = true;
                            txtendinv.Enabled = true;
                            txtnoofvisit.Enabled = true;
                            btnaddnewitem.Enabled = true;
                            if (defaultPage.RoleID != 1 && defaultPage.StocksOrInventory_Ending_Page_Edit == false && defaultPage.StocksOrInventory_Ending_Page_View == true)
                                btnSave.Enabled = false;
                            else
                                btnSave.Enabled = true;
                            // grdEndingInven.Enabled = true;
                        }
                        if (txtMonth.Text == DateTime.Now.ToString("MMM-yyyy"))
                        {
                            if (lblNewFacility.Text == "True")
                            {
                                txtbeginv.Enabled = true;                                
                                chkNewFaciltiy.Checked = true;
                                chkNewFaciltiy.Enabled = true;
                            }
                            else
                            {
                                txtbeginv.Enabled = false;
                                chkNewFaciltiy.Checked = false;
                                chkNewFaciltiy.Enabled = false;
                            }
                        }
                        else
                        {
                            txtbeginv.Enabled = false;
                            chkNewFaciltiy.Checked = false;
                            chkNewFaciltiy.Enabled = false;
                        }
                    }
                    else
                    {
                        var today = DateTime.Today;
                        var month = new DateTime(today.Year, today.Month, 1);
                        var last = month.AddDays(4);
                        DateTime nextMonth = Convert.ToDateTime(last);
                        var Previous = month.AddMonths(-1);
                        txtbeginv.Enabled = false;
                        chkNewFaciltiy.Checked = false;
                        if (Convert.ToDateTime(txtMonth.Text) >= Previous)
                        {
                            if (month == today || Convert.ToDateTime(txtMonth.Text) == month || Convert.ToDateTime(txtMonth.Text) == today)
                            {
                                txtexpiredmeds.Enabled = true;
                                txtendinv.Enabled = true;
                                txtnoofvisit.Enabled = true;

                                if (defaultPage.RoleID != 1 && defaultPage.StocksOrInventory_Ending_Page_Edit == false && defaultPage.StocksOrInventory_Ending_Page_View == true)
                                {
                                    btnaddnewitem.Enabled = false;
                                    btnSave.Enabled = false;
                                }
                                else
                                {
                                    btnaddnewitem.Enabled = true;
                                    btnSave.Enabled = true;
                                }
                                // grdEndingInven.Enabled = true;
                            }
                            else if (today > nextMonth)
                            {
                                txtexpiredmeds.Enabled = false;
                                txtendinv.Enabled = false;
                                txtnoofvisit.Enabled = false;
                                btnaddnewitem.Enabled = false;
                                btnSave.Enabled = false;
                                // grdEndingInven.Enabled = false;
                            }
                            else
                            {
                                txtexpiredmeds.Enabled = true;
                                txtendinv.Enabled = true;
                                txtnoofvisit.Enabled = true;
                                btnaddnewitem.Enabled = true;
                                if (defaultPage.RoleID != 1 && defaultPage.StocksOrInventory_Ending_Page_Edit == false && defaultPage.StocksOrInventory_Ending_Page_View == true)
                                {
                                    btnaddnewitem.Enabled = false;
                                    btnSave.Enabled = false;
                                }
                                else
                                {
                                    btnaddnewitem.Enabled = true;
                                    btnSave.Enabled = true;
                                }
                                // grdEndingInven.Enabled = true;
                            }

                            if (lblEndingInvenID.Text == "0" && lblNewFacility.Text == "True")
                            {
                                btnaddnewitem.Enabled = false;
                                btnSave.Enabled = false;
                            }
                            else
                            {
                                btnaddnewitem.Enabled = true;
                                btnSave.Enabled = true;
                            }

                        }
                        else
                        {
                            txtexpiredmeds.Enabled = false;
                            txtendinv.Enabled = false;
                            txtnoofvisit.Enabled = false;
                            btnaddnewitem.Enabled = false;
                            btnSave.Enabled = false;
                            // grdEndingInven.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void lblReceiveingInven_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                Label lblReceiveDate = (Label)gvrow.FindControl("lblReceiveDate");
                Label lblFacilityID = (Label)gvrow.FindControl("lblFacilityID");
                llstEndingInventory.FacilityID = Convert.ToInt64(lblFacilityID.Text);

                llstEndingInventory.ItemID = Convert.ToInt64(gvrow.Cells[1].Text);
                llstEndingInventory.ReceiveDate = Convert.ToDateTime(lblReceiveDate.Text);

                MPReceivedQty.Show();

                List<GetReceivedQtyInfo> llstReceivedinfo = lclsservice.GetReceivedQtyInfo(llstEndingInventory).ToList();

                GrdReceiveqty.DataSource = llstReceivedinfo;
                GrdReceiveqty.DataBind();

                if (llstReceivedinfo.Count > 0)
                {
                    lblItemID.Text = llstReceivedinfo[0].ItemID.ToString();
                    lblItemDescript.Text = llstReceivedinfo[0].ItemDescription.ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }


        }

        protected void btnaddnewitem_Click(object sender, EventArgs e)
        {
            try
            {
                MPAddnewItem.Show();
                BindEndingInvenGrid("Add");
                if (GrdAddnewItem.Rows.Count > 0)
                {
                    btnAddtogrid.Enabled = true;
                }
                else
                {
                    btnAddtogrid.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }

        }

        private void BindItemsFromGridtoDT()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("EndingInvenID");
                dt.Columns.Add("CorporateID");
                dt.Columns.Add("FacilityID");
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("ItemID");
                dt.Columns.Add("VendorItemID");
                dt.Columns.Add("ItemDescription");
                dt.Columns.Add("QtyPack");
                dt.Columns.Add("UomID");
                dt.Columns.Add("BeginingInvQty");
                dt.Columns.Add("ReceiveingOrderInvQty");
                dt.Columns.Add("ReceiveDate");
                dt.Columns.Add("TransferInQty");
                dt.Columns.Add("TransferINDate");
                dt.Columns.Add("TransferOutQty");
                dt.Columns.Add("TransferOutDate");
                dt.Columns.Add("ExpiredMeds");
                dt.Columns.Add("EndingInvQty");
                dt.Columns.Add("MonthlyUsage");
                dt.Columns.Add("NewFacility");
                dt.AcceptChanges();

                foreach (GridViewRow row in grdEndingInven.Rows)
                {
                    // Declare Controls inn Grid

                    Label lblEndingInvenID = (Label)row.FindControl("lblEndingInvenID");
                    Label lblCorporateID = (Label)row.FindControl("lblCorporateID");
                    Label lblFacilityID = (Label)row.FindControl("lblFacilityID");
                    Label lblCatagoryID = (Label)row.FindControl("lblCatagoryID");
                    Label lblVendorItemID = (Label)row.FindControl("lblVendorItemID");
                    TextBox txtbeginv = (TextBox)row.FindControl("txtbeginv");
                    LinkButton lblReceiveingInven = (LinkButton)row.FindControl("lblReceiveingInven");
                    Label lblReceiveDate = (Label)row.FindControl("lblReceiveDate");
                    LinkButton lblTransferIn = (LinkButton)row.FindControl("lblTransferIn");
                    Label lblTransferINDate = (Label)row.FindControl("lblTransferINDate");
                    LinkButton lnkTransferOut = (LinkButton)row.FindControl("lnkTransferOut");
                    Label lblTransferOutDate = (Label)row.FindControl("lblTransferOutDate");
                    TextBox txtexpiredmeds = (TextBox)row.FindControl("txtexpiredmeds");
                    TextBox txtendinv = (TextBox)row.FindControl("txtendinv");
                    TextBox lblMonthlyUsage = (TextBox)row.FindControl("lblMonthlyUsage");

                    dr = dt.NewRow();

                    dr["EndingInvenID"] = lblEndingInvenID.Text;
                    dr["CorporateID"] = lblCorporateID.Text;
                    dr["FacilityID"] = lblFacilityID.Text;
                    dr["CategoryID"] = lblCatagoryID.Text;
                    dr["ItemID"] = row.Cells[1].Text;
                    dr["VendorItemID"] = lblVendorItemID.Text;
                    dr["ItemDescription"] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                    dr["QtyPack"] = row.Cells[4].Text;
                    dr["UomID"] = row.Cells[5].Text;
                    dr["BeginingInvQty"] = txtbeginv.Text;
                    dr["ReceiveingOrderInvQty"] = lblReceiveingInven.Text;
                    dr["ReceiveDate"] = lblReceiveDate.Text;
                    dr["TransferInQty"] = lblTransferIn.Text;
                    dr["TransferINDate"] = lblTransferINDate.Text;
                    dr["TransferOutQty"] = lnkTransferOut.Text;
                    dr["TransferOutDate"] = lblTransferOutDate.Text;
                    dr["ExpiredMeds"] = txtexpiredmeds.Text;
                    dr["EndingInvQty"] = txtendinv.Text;
                    dr["MonthlyUsage"] = lblMonthlyUsage.Text;

                    dt.Rows.Add(dr);
                }


                foreach (GridViewRow row in GrdAddnewItem.Rows)
                {
                    Label lblEndingInvenID = (Label)row.FindControl("lblEndingInvenID");
                    Label lblCorporateID = (Label)row.FindControl("lblCorporateID");
                    Label lblFacilityID = (Label)row.FindControl("lblFacilityID");
                    Label lblCatagoryID = (Label)row.FindControl("lblCatagoryID");
                    Label lblVendorItemID = (Label)row.FindControl("lblVendorItemID");
                    //TextBox txtbeginv = (TextBox)row.FindControl("txtbeginv");
                    Label lblReceiveingInven = (Label)row.FindControl("lblReceiveingInven");
                    Label lblReceiveDate = (Label)row.FindControl("lblReceiveDate");
                    Label lblTransferIn = (Label)row.FindControl("lblTransferIn");
                    Label lblTransferINDate = (Label)row.FindControl("lblTransferINDate");
                    Label lblTransferOut = (Label)row.FindControl("lblTransferOut");
                    Label lblTransferOutDate = (Label)row.FindControl("lblTransferOutDate");
                    //TextBox txtexpiredmeds = (TextBox)row.FindControl("txtexpiredmeds");
                    //TextBox txtendinv = (TextBox)row.FindControl("txtendinv");
                    Label lblMonthlyUsage = (Label)row.FindControl("lblMonthlyUsage");

                    dr = dt.NewRow();

                    dr["EndingInvenID"] = lblEndingInvenID.Text;
                    dr["CorporateID"] = lblCorporateID.Text;
                    dr["FacilityID"] = lblFacilityID.Text;
                    dr["CategoryID"] = lblCatagoryID.Text;
                    dr["ItemID"] = row.Cells[1].Text;
                    dr["VendorItemID"] = lblVendorItemID.Text;
                    dr["ItemDescription"] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                    dr["QtyPack"] = row.Cells[4].Text;
                    dr["UomID"] = row.Cells[5].Text;
                    dr["BeginingInvQty"] = "";
                    dr["ReceiveingOrderInvQty"] = lblReceiveingInven.Text;
                    dr["ReceiveDate"] = lblReceiveDate.Text;
                    dr["TransferInQty"] = lblTransferIn.Text;
                    dr["TransferINDate"] = lblTransferINDate.Text;
                    dr["TransferOutQty"] = lblTransferOut.Text;
                    dr["TransferOutDate"] = lblTransferOutDate.Text;
                    dr["ExpiredMeds"] = "0";
                    dr["EndingInvQty"] = "";
                    dr["MonthlyUsage"] = lblMonthlyUsage.Text;

                    dt.Rows.Add(dr);
                }

                ViewState["EndingItems"] = dt;

                grdEndingInven.DataSource = dt;
                grdEndingInven.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void btnAddtogrid_Click(object sender, EventArgs e)
        {
            BindItemsFromGridtoDT();
        }


        protected void lblTransferIn_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                Label lblTransferINDate = (Label)gvrow.FindControl("lblTransferINDate");
                Label lblFacilityID = (Label)gvrow.FindControl("lblFacilityID");
                llstEndingInventory.FacilityID = Convert.ToInt64(lblFacilityID.Text);

                llstEndingInventory.ItemID = Convert.ToInt64(gvrow.Cells[1].Text);
                llstEndingInventory.ReceiveDate = Convert.ToDateTime(lblTransferINDate.Text);

                MPTransferIN.Show();

                List<GetTransferINQtyInfo> llstReceivedinfo = lclsservice.GetTransferINQtyInfo(llstEndingInventory).ToList();

                GrdTransferIN.DataSource = llstReceivedinfo;
                GrdTransferIN.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }


        }

        protected void lnkTransferOut_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                Label lblTransferOutDate = (Label)gvrow.FindControl("lblTransferOutDate");
                Label lblFacilityID = (Label)gvrow.FindControl("lblFacilityID");
                llstEndingInventory.FacilityID = Convert.ToInt64(lblFacilityID.Text);

                llstEndingInventory.ItemID = Convert.ToInt64(gvrow.Cells[1].Text);
                llstEndingInventory.ReceiveDate = Convert.ToDateTime(lblTransferOutDate.Text);

                MPTransferOutQty.Show();

                List<GetTransferOutQtyInfo> llstReceivedinfo = lclsservice.GetTransferOutQtyInfo(llstEndingInventory).ToList();

                GrdTransferOutQty.DataSource = llstReceivedinfo;
                GrdTransferOutQty.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }

        }


        public void DetailEndingInventory()
        {
            byte[] bytes = null;
            try
            {
                if (drpcorsearch.SelectedValue == "All")
                {
                    llstEndingInventory.ListCorporateID = "ALL";
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

                    llstEndingInventory.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstEndingInventory.ListFacilityID = "ALL";
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
                    llstEndingInventory.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();                

                if (drpItemCategory.SelectedValue == "All")
                {
                    llstEndingInventory.ListStatus = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpItemCategory.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    llstEndingInventory.ListCategoryID = FinalString;
                }
                SB.Clear();
                DateTime MonthYear = new DateTime();
                if (txtMonth.Text == "")
                {
                    MonthYear = DateTime.Now;
                }
                else
                {
                    MonthYear = Convert.ToDateTime(txtMonth.Text);
                }
                llstEndingInventory.MonthYear = MonthYear;
                llstEndingInventory.IsNewFacility = false;
                llstEndingInventory.LoggedinBy = defaultPage.UserId;

                List<EndingInventoryReport> lstEndingInventoryReport = lclsservice.EndingInventoryReport(llstEndingInventory).ToList();

                if (lstEndingInventoryReport.Count > 0)
                {
                    rvEndingInventoryreport.ProcessingMode = ProcessingMode.Local;
                    rvEndingInventoryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/EndingInventory.rdlc");
                    Int64 r = defaultPage.UserId;
                    ReportDataSource datasource = new ReportDataSource("EndingInvenReport", lstEndingInventoryReport);
                    rvEndingInventoryreport.LocalReport.DataSources.Clear();
                    rvEndingInventoryreport.LocalReport.DataSources.Add(datasource);
                    rvEndingInventoryreport.LocalReport.Refresh();
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;

                    bytes = rvEndingInventoryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

                    Guid guid = Guid.NewGuid();
                    string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                    _sessionPDFFileName = "EndingInventory" + guid + ".pdf";
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningEndingInventoryMessage.Replace("<<EndingInventory>>", ""), true);
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
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
                    Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "PrintPdf.aspx?file=" + Server.UrlEncode(path)));
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DetailEndingInventory();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void GrdTransferOutQty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCreatedBy = (Label)e.Row.FindControl("lblCreatedBy");
                Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");

                if (lblCreatedBy.Text.Length > 100)
                {
                    lblCreatedBy.Text = lblCreatedBy.Text.Substring(0, 100) + "....";
                    imgreadmore1.Visible = true;
                }
                else
                {
                    imgreadmore1.Visible = false;
                }
            }
        }

       
    }
}