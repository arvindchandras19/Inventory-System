#region Directives
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
#endregion

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<VendorItemMap>>
'' Type      :   C# File
'' Description  :<<To add,update the VendorItemMap Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Murali.M			                  New
''  08/16/2017         V1.0                Vivekanand.S                 Check the dupicates and existing record in DataBase while Save the Fields
 *  10/24/2017         V2.0                Sairam.P                     Validate mandatory fields and Check the active and inactive records in dropdown.
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventory
{
    public partial class VendorItemMap : System.Web.UI.Page
    {
        #region Declartions
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALItemMap listItemMap = new BALItemMap();
        StringBuilder SB = new StringBuilder();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.WarningVendorMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    //LoadLookups("Add");
                    BindCategory();
                    BindVendor();
                    BindGrid();
                    //BindItem();
                    if (defaultPage.VendorItemMap_Edit == false && defaultPage.VendorItemMap_View == true)
                    {
                        //SaveCancel.Visible = false;
                        btnsave.Visible = false;

                    }

                    if (defaultPage.VendorItemMap_Edit == false && defaultPage.VendorItemMap_View == false)
                    {
                        div_ADDContent.Visible = false;
                        gridItems.Visible = false;
                        btnsave.Visible = false;
                        //  AddDiv.Visible = false;
                        saveoredit.Visible = false;
                        updmain.Visible = false;
                        User_Permission_Message.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }
        private void RequiredFieldValidatorMessage()
        {
            try
            {
                string req = Constant.RequiredFieldValidator;
                ReqfielddrdItemCategory.ErrorMessage = req;
                ReqfielddrdItemName.ErrorMessage = req;
                ReqfielddrdVendorID.ErrorMessage = req;
                reqfieldtxtVendorItemID.ErrorMessage = req;
                //ReqfielddrpVendorSearch.ErrorMessage = req;
                //ReqfielddrpItemCategorySearch.ErrorMessage = req;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }
        public void BindGrid()
        {
            try
            {

                foreach (ListItem lst in drpVendorSearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    listItemMap.ListVendorID = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpVendorSearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                SB.Clear();

                foreach (ListItem lst in drpItemCategorySearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    listItemMap.ListItemCategory = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpItemCategorySearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                //list = (from p in service.GetItemMap() where DrpVendorSearch.Contains(inq[0]) select p).ToList();
                //list = service.GetItemMap().Where(a => DrpVendorSearch.Contains(a.CategoryID) || a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();

                SB.Clear();

                listItemMap.IsStrActive = rdbstatus.SelectedValue;
                listItemMap.LoggedIN = defaultPage.UserId;
                listItemMap.Filter = "";

                List<SearchVendorItemMap> list = new List<SearchVendorItemMap>();
                list = lclsservice.GetVendorItemMap(listItemMap).ToList();
                //lblrowcount.Text = "No of records : " + list.Count.ToString();
                gridItems.DataSource = list;
                gridItems.DataBind();


                //if (drpVendorSearch.SelectedIndex == 0 && drpItemCategorySearch.SelectedIndex == 0)
                //{
                //    InventoryServiceClient service = new InventoryServiceClient();
                //    List<GetItemMap> list = new List<GetItemMap>();
                //    list = service.GetItemMap().ToList();
                //    gridItems.DataSource = list;
                //    gridItems.DataBind();
                //}
                //else if (drpVendorSearch.SelectedIndex == 0 || drpItemCategorySearch.SelectedIndex == 0)
                //{
                //    InventoryServiceClient service = new InventoryServiceClient();
                //    List<GetItemMap> list = new List<GetItemMap>();

                //    list = service.GetItemMap().Where(a => a.CategoryID == Convert.ToInt64(drpItemCategorySearch.SelectedValue) || a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();
                //    gridItems.DataSource = list;
                //    gridItems.DataBind();
                //}
                //else
                //{
                //    InventoryServiceClient service = new InventoryServiceClient();
                //    List<GetItemMap> list = new List<GetItemMap>();
                //    list = service.GetItemMap().Where(a => a.CategoryID == Convert.ToInt64(drpItemCategorySearch.SelectedValue) && a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();
                //    gridItems.DataSource = list;
                //    gridItems.DataBind();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }


        public void BindCategoryadd(string mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetItemCategory> lstcategory = null;
                if (mode == "Add")
                {
                    lstcategory = lclsservice.GetItemCategory().Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    lstcategory = lclsservice.GetItemCategory().ToList();
                }
                drdItemCategory.DataSource = lstcategory;
                drdItemCategory.DataValueField = "CategoryID";
                drdItemCategory.DataTextField = "CategoryName";
                drdItemCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdItemCategory.Items.Insert(0, lst);


                // Search Item Category Dropdown


            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        public void BindCategory()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetItemCategory> lstcategory = null;
                lstcategory = lclsservice.GetItemCategory().ToList();
                drpItemCategorySearch.DataSource = lstcategory;
                drpItemCategorySearch.DataValueField = "CategoryID";
                drpItemCategorySearch.DataTextField = "CategoryName";
                drpItemCategorySearch.DataBind();


                foreach (ListItem lst1 in drpItemCategorySearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }
        public void BindItem(string mode)
        {
            try
            {
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetDrpItemsByCategory> listItem = null;
                if (mode == "Add")
                {
                    listItem = service.GetDrpItemsByCategory(Convert.ToInt64(drdItemCategory.SelectedValue)).Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    listItem = service.GetDrpItemsByCategory(Convert.ToInt64(drdItemCategory.SelectedValue)).ToList();
                }
                drdItemID.DataSource = listItem;
                drdItemID.DataTextField = "ItemShortName";
                drdItemID.DataValueField = "ItemID";
                drdItemID.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdItemID.Items.Insert(0, lst);

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }
        public void BindVendor()
        {
            try
            {
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetVendorItemMappingPage> listVendor = null;
                string SearchText = string.Empty;
                listVendor = service.GetVendorItemMappingPage().ToList();
                drpVendorSearch.DataSource = listVendor;
                drpVendorSearch.DataTextField = "VendorDescription";
                drpVendorSearch.DataValueField = "VendorID";
                drpVendorSearch.DataBind();


                foreach (ListItem lst1 in drpVendorSearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }


        public void BindVendoradd(string mode)
        {
            try
            {
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetVendorItemMappingPage> listVendor = null;
                string SearchText = string.Empty;
                if (mode == "Add")
                {
                    listVendor = service.GetVendorItemMappingPage().Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    listVendor = service.GetVendorItemMappingPage().ToList();
                }
                drdVendorID.DataSource = listVendor;
                drdVendorID.DataTextField = "VendorDescription";
                drdVendorID.DataValueField = "VendorID";
                drdVendorID.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drdVendorID.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }


        protected void drdItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnItemMapID.Value == "0")
                {
                    BindItem("Add");
                }
                else
                {
                    BindItem("Edit");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowContent", "editfacility();", true);
        }

        public void ToggleAdd(string Mode)
        {
            try
            {
                if (Mode == "Add")
                {
                    btnclose.Visible = true;
                    btnCancel.Visible = false;
                    btnSearch.Visible = false;
                    btnAdd.Visible = false;
                    btnPrint.Visible = false;
                    btnsave.Visible = true;
                    DivSearchControl.Style.Add("display", "none");
                    DivAdd.Style.Add("display", "block");
                    lblseroutHeader.Visible = false;
                    DivSearch.Style.Add("display", "none");
                }
                else
                {
                    btnclose.Visible = false;
                    btnCancel.Visible = true;
                    btnSearch.Visible = true;
                    btnAdd.Visible = true;
                    btnPrint.Visible = true;
                    btnsave.Visible = false;
                    DivSearchControl.Style.Add("display", "block");
                    DivAdd.Style.Add("display", "none");
                    lblseroutHeader.Visible = true;
                    DivSearch.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                BindCategoryadd("Add");
                BindVendoradd("Add");
                BindItem("Add");
                ToggleAdd("Add");
                HideAndShowControls("Edit");
                DivCheckBox.Style.Add("display", "none");
                clear();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient service = new InventoryServiceClient();
                string lstrMessage = string.Empty;
                string VendorItemMap = string.Empty;
                string vendordesc = string.Empty;
                string ErrMsg = string.Empty;
               
                BALItemMap obj = new BALItemMap();
                
                if (!ValidateLoookups(service)) return;
                obj.ItemMapID = Convert.ToInt64(hdnItemMapID.Value);
                obj.ItemID = Convert.ToInt64(drdItemID.SelectedValue);
                obj.ItemCategory = Convert.ToInt64(drdItemCategory.SelectedValue);
                obj.VendorID = Convert.ToInt64(drdVendorID.SelectedValue);
                obj.VendorItemID = txtVendorItemID.Text;
                obj.CreatedBy = defaultPage.UserId;
                obj.CreatedOn = DateTime.Now;
                if (chkactive.Checked == true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                List<GetItemMap> list = new List<GetItemMap>(); list = service.GetItemMap().Where(a => a.ItemIDCk == Convert.ToInt64(drdItemID.SelectedValue)).ToList();

                List<GetItemMap> listVendorItemid = new List<GetItemMap>(); listVendorItemid = service.GetItemMap().Where(a => a.VendorItemID == obj.VendorItemID).ToList();

                if (list.Count>0)
                {                    
                    if (list[0].ItemMapID != obj.ItemMapID)
                    {
                        ErrMsg = "VIC";
                    }
                }
                if (listVendorItemid.Count > 0)
                {
                    if (obj.VendorItemID == listVendorItemid[0].VendorItemID && listVendorItemid[0].ItemMapID != obj.ItemMapID)
                    {
                        ErrMsg = "VID";
                    }
                }      
                if (ErrMsg == "")
                {
                    lstrMessage = service.InsertItemMap(obj);
                }
                else
                {
                    if (ErrMsg == "VID" && ErrMsg == "VIC")
                    {
                        vendordesc = drdVendorID.SelectedItem.Text;
                        log.LogInformation(msgwrn.Replace("<<VendorItemMap>>", vendordesc));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorMessage.Replace("<<VendorItemMap>>", vendordesc), true);
                    }
                    else if (ErrMsg == "VIC")
                    {
                        vendordesc = drdVendorID.SelectedItem.Text;
                        log.LogInformation(msgwrn.Replace("<<VendorItemMap>>", vendordesc));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorMessage.Replace("<<VendorItemMap>>", vendordesc), true);
                    }
                    else if (ErrMsg == "VID")
                    {
                        List<GetItemMap> listVendorItemidInActive = new List<GetItemMap>(); listVendorItemidInActive = service.GetItemMap().Where(a => a.VendorItemID == obj.VendorItemID && a.IsActive==false).ToList();
                        vendordesc = drdVendorID.SelectedItem.Text;
                        if (listVendorItemidInActive.Count > 0)
                        {
                            string msg = Constant.WarningVendorInActiveLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<VendorItemMap>>", vendordesc).Replace("');", "");
                            log.LogInformation(msg);
                            log.LogInformation(Constant.WarningVendorInActiveLookupMessage.Replace("<<VendorItemMap>>", vendordesc));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorInActiveLookupMessage.Replace("<<VendorItemMap>>", vendordesc), true);
                        }
                        else
                        {
                            string msg = Constant.WarningVendorItemIDMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<VendorItemMap>>", vendordesc).Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorItemIDMessage.Replace("<<VendorItemMap>>", vendordesc), true);

                        }
                       
                    }
                  
                }



                if (lstrMessage == "Saved Successfully")
                {
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    string msg = Constant.VendorItemMapSaveMessage.Replace("ShowPopup('", "").Replace("<<VendorItemMap>>", txtVendorItemID.Text.ToString()).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapSaveMessage.Replace("<<VendorItemMap>>", txtVendorItemID.Text.ToString()), true);
                    ToggleAdd("Clear");
                    clear();
                    BindGrid();
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        public void HideAndShowControls(string Mode)
        {
            try
            {
                if (Mode == "Edit")
                {
                    drdItemCategory.Enabled = true;
                    drdItemID.Enabled = true;
                    drdVendorID.Enabled = true;
                    txtVendorItemID.Enabled = true;
                    btnsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = false;
                    drdItemCategory.Enabled = false;
                    drdItemID.Enabled = false;
                    drdVendorID.Enabled = false;
                    txtVendorItemID.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        //protected void imgbtnview_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ToggleAdd("Add");
        //        DivSearch.Style.Add("display", "block");
        //        BindGrid("NonSearch");
        //        InventoryServiceClient service = new InventoryServiceClient();
        //        ImageButton btndetails = sender as ImageButton;
        //        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        //        if (hdnItemMapID != null)
        //            hdnItemMapID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
        //        LoadLookups("Edit");
        //        drdItemCategory.ClearSelection();
        //        drdItemCategory.Items.FindByText(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "")).Selected = true;
        //        //if (drdItemID.se == null)
        //        //{
        //        //    drdItemID.SelectedIndex = 0;
        //        //}
        //        if (drdItemCategory.SelectedIndex > 0)
        //        {
        //            BindItem("Edit");
        //            drdItemID.ClearSelection();
        //            //drdItemID.Items.FindByText(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "")).Selected = true;
        //            drdItemID.Items.FindByText(HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", ""))).Selected = true;
        //        }
        //        drdVendorID.ClearSelection();
        //        drdVendorID.Items.FindByText(gvrow.Cells[4].Text.Trim().ToString()).Selected = true;
        //        txtVendorItemID.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
        //        BindGrid("Search");
        //        HideAndShowControls("View");
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowContent", "editfacility();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
        //    }
        //}

        protected void lbedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ToggleAdd("Add");
                HideAndShowControls("Edit");
                btnCancel.Visible = false;
                //DivSearch.Style.Add("display", "block");
                //BindGrid("NonSearch");
                InventoryServiceClient service = new InventoryServiceClient();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                if (hdnItemMapID != null)
                    hdnItemMapID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");

                //LoadLookups("Edit");
                BindCategoryadd("Edit");
                drdItemCategory.ClearSelection();
                if (drdItemCategory.Items.FindByText(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "")) != null)
                    drdItemCategory.Items.FindByValue(drdItemCategory.Items.FindByText(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "")).Value).Selected = true;


                //if (drdItemID.se == null)
                //{
                //    drdItemID.SelectedIndex = 0;
                //}
                if (drdItemCategory.SelectedIndex > 0)
                {
                    BindItem("Edit");

                    drdItemID.ClearSelection();
                    //drdItemID.Items.FindByText(gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "")).Selected = true;                   
                    if (drdItemID.Items.FindByText(HttpUtility.HtmlDecode(gvrow.Cells[8].Text.Trim().Replace("&nbsp;", ""))) != null)
                        drdItemID.Items.FindByValue(drdItemID.Items.FindByText(HttpUtility.HtmlDecode(gvrow.Cells[8].Text.Trim().Replace("&nbsp;", ""))).Value).Selected = true;
                }
                BindVendoradd("Edit");
                drdVendorID.ClearSelection();
                if (drdVendorID.Items.FindByText(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "")) != null)
                    drdVendorID.Items.FindByValue(drdVendorID.Items.FindByText(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "")).Value).Selected = true;
                //drdVendorID.SelectedValue = drdVendorID.Items.FindByText(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "")).Value;
                txtVendorItemID.Text = gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "");
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                    DivCheckBox.Style.Add("display", "none");
                }
                else
                {
                    chkactive.Visible = true;
                    DivCheckBox.Style.Add("display", "block");
                }
                BindGrid();

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowContent", "editfacility();", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        protected void lbdelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnItemMapID.Value = gvrow.Cells[2].Text;
                // Label2.Text = "Are you sure you want to delete this Record?";
                ModalPopupExtender2.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                lblmsg.Text = ex.Message;
            }
        }

        protected void btnYes_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{

            //    //InventoryServiceClient lclsService = new InventoryServiceClient();
            //    //string lstrMessage = lclsService.DeleteItemMapping(Convert.ToInt64(hdnItemMapID.Value));
            //    //if (lstrMessage == "Deleted Successfully")
            //    //{
            //    //    Functions objfun = new Functions();
            //    //    objfun.MessageDialog(this, "Deleted Successfully");
            //    //}

            //    //BindGrid();
            //}
            //catch (Exception ex)
            //{

            //    lblmsg.Text = ex.Message;
            //}

        }

        public void clear()
        {
            try
            {
                hdnItemMapID.Value = "0";
                drdItemID.SelectedIndex = -1;
                drdItemCategory.SelectedIndex = -1;
                drdVendorID.SelectedIndex = -1;
                txtVendorItemID.Text = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        //private void LoadLookups(string mode)
        //{
        //    try
        //    {
        //        BindCategory(mode);
        //        BindItem(mode);
        //        BindVendor(mode);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
        //    }
        //}

        private bool ValidateLoookups(InventoryServiceClient service)
        {
            try
            {
                bool result = true;
                string errmessage = "";
                //Item Category Lookup
                List<GetItemCategory> lstCat = service.GetItemCategory().Where(a => a.IsActive == true && a.CategoryID == Convert.ToInt64(drdItemCategory.SelectedValue)).ToList();
                if (lstCat.Count == 0)
                {
                    errmessage += "ItemCategory (" + drdItemCategory.SelectedItem.Text + ") , ";
                    result = false;
                }

                //Item based on Category Lookup
                List<GetDrpItemsByCategory> lstitemCat = service.GetDrpItemsByCategory(Convert.ToInt64(drdItemCategory.SelectedValue)).Where(a => a.IsActive == true && a.ItemID == Convert.ToInt64(drdItemID.SelectedValue)).ToList();
                if (lstitemCat.Count == 0)
                {
                    errmessage += "Item (" + drdItemID.SelectedItem.Text + ") , ";
                    result = false;
                }

                //Vendor Lookup
                List<GetvendorDetails> lstvendor = service.GetvendorDetails("").Where(a => a.IsActive == true && a.VendorID == Convert.ToInt64(drdVendorID.SelectedValue)).ToList();
                if (lstvendor.Count == 0)
                {
                    errmessage += "Vendor (" + drdVendorID.SelectedItem.Text + ") ";
                    result = false;
                }




                if (!result)
                {
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.WarningLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<values>>", errmessage).Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningLookupMessage.Replace("<<values>>", errmessage), true);
                }

                return result;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
                return false;
            }

        }

        protected void btndeleteimg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndelete = sender as ImageButton;
                GridViewRow row = (GridViewRow)btndelete.NamingContainer;

                hdnItemMapID.Value = row.Cells[1].Text.Trim();
                HddVendorItemCategory.Value = row.Cells[4].Text.Trim() + "," + row.Cells[2].Text.Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                lclsService.DeleteItemMapping(Convert.ToInt64(hdnItemMapID.Value), false, Convert.ToInt64(defaultPage.UserId));
                EventLogger log = new EventLogger(config);
                string msg = Constant.VendorItemMapDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<VendorItemMap>>", HddVendorItemCategory.Value).Replace("');", "");
                log.LogInformation(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapDeleteMessage.Replace("<<VendorItemMap>>", HddVendorItemCategory.Value), true);
                BindGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        //protected void chkActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string VendorItemMap = string.Empty;
        //        GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        //        CheckBox chkactive = (CheckBox)row.FindControl("chkActive");

        //        InventoryServiceClient lclsService = new InventoryServiceClient();

        //        if (chkactive.Checked == true)
        //        {
        //            lclsService.DeleteItemMapping(Convert.ToInt64(row.Cells[1].Text.Trim()), true, Convert.ToInt64(defaultPage.UserId));
        //        }
        //        else
        //        {
        //            lclsService.DeleteItemMapping(Convert.ToInt64(row.Cells[1].Text.Trim()), false, Convert.ToInt64(defaultPage.UserId));
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapDeleteMessage.Replace("<<VendorItemMap>>", row.Cells[4].Text.ToString()), true);
        //        }
        //        BindGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
        //    }
        //}

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                ToggleAdd("Clear");
                rdbstatus.SelectedValue = "1";
                BindCategory();
                BindVendor();
                BindGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = PrintVendorItemMapping();
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "VendorItemMap" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }
        public List<object> PrintVendorItemMapping()
        {
            try
            {
                List<object> llstarg = new List<object>();
                List<BindVendorItemMapReport> llstreview = null;
                BindGrid();
                foreach (ListItem lst in drpVendorSearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    listItemMap.ListVendorID = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpVendorSearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                SB.Clear();

                foreach (ListItem lst in drpItemCategorySearch.Items)
                {
                    if (lst.Selected && drpVendorSearch.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.ToString() != "")
                    listItemMap.ListItemCategory = SB.ToString().Substring(0, (SB.Length - 1));
                //var DrpItemCategorySearch = Array.ConvertAll<string, Int64>(FinalOut.Split(','), Convert.ToInt64);
                //list = (from p in service.GetItemMap() where DrpVendorSearch.Contains(inq[0]) select p).ToList();
                //list = service.GetItemMap().Where(a => DrpVendorSearch.Contains(a.CategoryID) || a.VendorID == Convert.ToInt64(drpVendorSearch.SelectedValue)).ToList();

                SB.Clear();
                listItemMap.IsStrActive = rdbstatus.SelectedValue;
                listItemMap.LoggedIN = defaultPage.UserId;
                listItemMap.Filter = "";

                llstreview = lclsservice.BindVendorItemMapReport(listItemMap).ToList();

                //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
                rvVendorItemMapreport.ProcessingMode = ProcessingMode.Local;
                rvVendorItemMapreport.LocalReport.ReportPath = Server.MapPath("~/Reports/VendorItemMapSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("VendorItemMapReportDS", llstreview);
                rvVendorItemMapreport.LocalReport.DataSources.Clear();
                rvVendorItemMapreport.LocalReport.DataSources.Add(datasource);
                rvVendorItemMapreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = null;
                bytes = rvVendorItemMapreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                llstarg.Insert(0, bytes);
                return llstarg;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
                return null;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (drpVendorSearch.SelectedIndex == 0 && drpItemCategorySearch.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorItemMapMessage.Replace("<<VendorItemMap>>", "Select any one of the search fields"), true);
                //}
                //else
                //{
                BindGrid();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }

        }

        protected void gridItems_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        chkactive.Visible = false;
                        DivCheckBox.Style.Add("display", "none");
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkactive.Visible = true;
                        DivCheckBox.Style.Add("display", "block");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorItemMapErrorMessage.Replace("<<VendorItemMap>>", ex.Message), true);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            ToggleAdd("Clear");
            BindGrid();
        }
    }
}


