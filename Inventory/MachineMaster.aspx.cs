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

namespace Inventory
{
    public partial class MachineMaster : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        Functions objfun = new Functions();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.WarningMachineMasterMessage.Replace("ShowwarningPopup('", "").Replace("');", "");
        #endregion
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnprint);
                scriptManager.RegisterPostBackControl(this.btnsearchprint);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();

                    if (defaultPage != null)
                    {
                        BindCorporate();
                        drpCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility("Add");
                        drpFacility.SelectedIndex = 0;

                        drpFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                        GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add");
                        GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add");
                        BindMachinemasterGrid();
                        hdnroleID.Value = Convert.ToString(defaultPage.RoleID);
                        hdnEditPermission.Value = Convert.ToString(defaultPage.MachineMaster_Edit);
                        hdnViewPermission.Value = Convert.ToString(defaultPage.MachineMaster_View);
                        if (defaultPage.RoleID == 1)
                        {
                            drpCorporate.Enabled = true;
                            drpFacility.Enabled = true;
                            lblsubfacility.Enabled = true;
                            lblsubfacility.Text = drpFacility.SelectedItem.Text;
                        }
                        else
                        {
                            drpCorporate.Enabled = false;
                            drpFacility.Enabled = false;
                            lblsubfacility.Enabled = false;
                            lblsubfacility.Text = drpFacility.SelectedItem.Text;
                        }
                        if (defaultPage.MachineMaster_Edit == false && defaultPage.MachineMaster_View == true)
                        {
                            //SaveCancel.Visible = false;
                            btnsave.Visible = false;
                            divmachine.Visible = true;
                            btnaddequipment.Style.Add("display", "none");
                            btnequiplistsave.Style.Add("display", "none");

                        }
                        if (defaultPage.MachineMaster_Edit == false && defaultPage.MachineMaster_View == false)
                        {
                            div_ADDContent.Visible = false;
                            divmachine.Visible = false;
                            btnsave.Visible = false;
                            deletebtn.Visible = false;
                            drpCorporate.Visible = false;
                            drpFacility.Visible = false;
                            Search.Visible = false;
                            searchdiv.Visible = false;
                            //div_SearchDiv.Visible = false;
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }

        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            ReqddlCorporateadd.ErrorMessage = req;
            reqfacility.ErrorMessage = req;
            reqequip.ErrorMessage = req;
            reqmanufacturer.ErrorMessage = req;
            reqequplst.ErrorMessage = req;
            reqmanyear.ErrorMessage = req;
            reqmodel.ErrorMessage = req;
            reqserialno.ErrorMessage = req;
        }
        #endregion
        #region Bind functions
        public void BindEquipcategory(Int64 CorporateID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipmentCategory> lstequipcat = lclsservice.GetEquipmentCategory(CorporateID, Mode).ToList();
                drpEquipment.DataSource = lstequipcat;
                drpEquipment.DataValueField = "EquipmentCategoryID";
                drpEquipment.DataTextField = "EquipmentCatDescription";
                drpEquipment.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpEquipment.Items.Insert(0, lst);
                drpEquipment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        public void GetEquipementSubCategory(long EquipCatID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementSubCategory> lstequiplist = lclsservice.GetEquipementSubCategory(EquipCatID, Mode).ToList();
                if (lstequiplist.Count > 0)
                {
                    drpEquipmentSubCategory.DataSource = lstequiplist;
                    drpEquipmentSubCategory.DataValueField = "EquipementSubCategoryID";
                    drpEquipmentSubCategory.DataTextField = "EquipmentSubCategoryDescription";
                    drpEquipmentSubCategory.DataBind();
                }
                else
                {
                    drpEquipmentSubCategory.Items.Clear();
                }
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpEquipmentSubCategory.Items.Insert(0, lst);
                drpEquipmentSubCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        public void GetEquipementList(long EquipSubCatID, string Mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetEquipementList> lstequiplist = lclsservice.GetEquipementList(EquipSubCatID, Mode).ToList();
                if (lstequiplist.Count > 0)
                {
                    drpEquipmentlist.DataSource = lstequiplist;
                    drpEquipmentlist.DataValueField = "EquipementListID";
                    drpEquipmentlist.DataTextField = "EquipmentListDescription";
                    drpEquipmentlist.DataBind();
                }
                else
                {
                    drpEquipmentlist.Items.Clear();
                }
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                drpEquipmentlist.Items.Insert(0, lst);
                drpEquipmentlist.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        public void BindCorporate()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<BALUser> lstfacility = new List<BALUser>();
                lstfacility = lclsservice.GetCorporateMaster().ToList();
                drpCorporate.DataSource = lstfacility;
                drpCorporate.DataTextField = "CorporateName";
                drpCorporate.DataValueField = "CorporateID";
                drpCorporate.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Corporate--";
                drpCorporate.Items.Insert(0, lst);
                drpCorporate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        private void BindFacility(string mode)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetCorporateFacility> lstfacility = new List<GetCorporateFacility>();
                if (mode == "Add")
                {
                    lstfacility = lclsservice.GetCorporateFacility(Convert.ToInt64(drpCorporate.SelectedValue)).Where(a => a.IsActive == true).ToList();
                }
                else
                {

                    lstfacility = lclsservice.GetCorporateFacility(Convert.ToInt64(drpCorporate.SelectedValue)).ToList();
                }
                drpFacility.DataSource = lstfacility;
                drpFacility.DataTextField = "FacilityDescription";
                drpFacility.DataValueField = "FacilityID";
                drpFacility.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Facility--";
                drpFacility.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        public void BindMachinemasterGrid()
        {
            try
            {
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                List<GetMachinemasterDetails> lstmachinemaster = new List<GetMachinemasterDetails>();
                List<SearchMachinemasterdetails> lstmachinemasterID = new List<SearchMachinemasterdetails>();
                objbalmachinemaster.FacilityID = Convert.ToInt64(drpFacility.SelectedValue);
                objbalmachinemaster.IstrActive = reactive.SelectedValue;
                objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                objbalmachinemaster.Filter = "";
                lstmachinemasterID = lclserinven.SearchMachinemasterdetails(objbalmachinemaster).ToList();
                gvmachinemaster.DataSource = lstmachinemasterID;
                gvmachinemaster.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        #endregion

        #region selectedIndexchanged Event
        protected void drpEquipmentSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpEquipment.SelectedValue == "1")
                {
                    txthours.Enabled = true;
                }
                else
                {
                    txthours.Enabled = false;
                    txthours.Text = "";
                    drpEquipment.Enabled = true;
                    drpEquipmentlist.Enabled = true;
                }

                GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add");
                div_ADDContent.Attributes.Add("display", "block");
                savebtn.Attributes.Add("display", "block");

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        #endregion


        #region selectedIndexchanged Event
        protected void drpEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpEquipment.SelectedValue == "1")
                {
                    txthours.Enabled = true;
                }
                else
                {
                    txthours.Enabled = false;
                    txthours.Text = "";
                    drpEquipment.Enabled = true;
                    drpEquipmentlist.Enabled = true;
                }

                GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add");
                div_ADDContent.Attributes.Add("display", "block");
                savebtn.Attributes.Add("display", "block");

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        protected void drpcor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindFacility("Add");
                BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                closediv();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
           
        }
        #endregion

        #region ValidateLookups
        private void LoadLookups(string Mode)
        {
          
            BindFacility(Mode);
        }

        private bool ValidateLoookups(InventoryServiceClient service)
        {
            bool result = true;
            string errmessage = "";
            //Equipment Category Lookup
           
            List<GetEquipmentCategory> lstCat = service.GetEquipmentCategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add").Where(a => a.EquipmentCategoryID == Convert.ToInt64(drpEquipment.SelectedValue)).ToList();
            if (lstCat.Count == 0)
            {
                errmessage += "Equipment Category (" + drpEquipment.SelectedItem.Text + ") , ";
                result = false;
            }
            
            //Equipment Sub Category Lookup
            List<GetEquipementSubCategory> lstEquipSub = service.GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add").Where(a => a.EquipementSubCategoryID == Convert.ToInt64(drpEquipmentSubCategory.SelectedValue)).ToList();
            if (lstCat.Count == 0)
            {
                errmessage += "Item (" + drpEquipmentSubCategory.SelectedItem.Text + ") , ";
                result = false;
            }

            //Equipment List Lookup
            List<GetEquipementList> lstEquiplist = service.GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add").Where(a => a.EquipementListID == Convert.ToInt64(drpEquipmentlist.SelectedValue)).ToList();
            if (lstCat.Count == 0)
            {
                errmessage += "Item (" + drpEquipmentlist.SelectedItem.Text + ") , ";
                result = false;
            }
            //Facility Lookup
            List<GetCorporateFacility> lstfac = service.GetCorporateFacility(Convert.ToInt64(drpCorporate.SelectedValue)).Where(a => a.IsActive == true && a.FacilityID == Convert.ToInt64(drpFacility.SelectedValue)).ToList();
            if (lstfac.Count == 0)
            {
                errmessage += "Facility (" + drpFacility.SelectedItem.Text + ") , ";
                result = false;
            }

            if (!result)
            {
                EventLogger log = new EventLogger(config);
                string msg = Constant.WarningLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<values>>", errmessage).Replace("');", "");
                log.LogWarning(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningLookupMessage.Replace("<<values>>", errmessage), true);
            }
            if (result == false)
            {
                editdiv();
            }
            return result;
        }
        #endregion
        #region Button Click Event
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclserinven1 = new InventoryServiceClient();
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                if (hiddenMachineID.Value != null)
                    objbalmachinemaster.MachineID = Convert.ToInt64(hiddenMachineID.Value);
                objbalmachinemaster.FacilityID = Convert.ToInt64(drpFacility.SelectedValue);
                objbalmachinemaster.Manufacturer = txtmanufacturer.Text;
                objbalmachinemaster.Manufacturedyear = txtyear.Text;
                if (txthours.Text != "")
                    objbalmachinemaster.Hoursonthemachine = Convert.ToInt32(txthours.Text);
                objbalmachinemaster.EquipementCategoryID = Convert.ToInt64(drpEquipment.SelectedValue);
                objbalmachinemaster.EquipementSubCategoryID = Convert.ToInt64(drpEquipmentSubCategory.SelectedValue);
                objbalmachinemaster.EquipementListID = Convert.ToInt64(drpEquipmentlist.SelectedValue);
                objbalmachinemaster.Model = txtmodel.Text;
                objbalmachinemaster.SerNo = txtserial.Text;
                objbalmachinemaster.Warranty = txtwarrenty.Text;
                objbalmachinemaster.GpAccountCode = txtgpcode.Text;
                objbalmachinemaster.CreatedBy = defaultPage.UserId;
                objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                if (chkactive.Checked == true)
                {
                    objbalmachinemaster.IsActive = true;
                }
                else
                {
                    objbalmachinemaster.IsActive = false;
                }
                if (objbalmachinemaster.MachineID == 0)
                {
                    List<GetMachinemasterDetails> lstmachinemaster = new List<GetMachinemasterDetails>();
                    lstmachinemaster = lclserinven1.GetMachinemasterDetails().Where(lst => lst.FacilityID == Convert.ToInt64(drpFacility.SelectedValue) && lst.EquipmentCategoryID == Convert.ToInt64(drpEquipment.SelectedValue) && lst.EquipementListID == Convert.ToInt64(drpEquipmentlist.SelectedValue)).ToList();
                    // --->Performing Check One to One Relationship for Faclity and Equipment
                    if (lstmachinemaster.Count == 0)
                    {
                        // --->Performing Normal Insert Opertaion
                        string a = lclserinven.InsertMachineMaster(objbalmachinemaster);
                        if (a == "Saved Successfully")
                        {
                            string msg = Constant.MachineMasterSaveMessage.Replace("ShowPopup('", "").Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text).Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterSaveMessage.Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text), true);
                            closediv();
                            BindMachinemasterGrid();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ClearContent();", true);
                            hiddenMachineID.Value = "0";

                        }
                    }
                    else
                    {
                        log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "EquipmentCategory already exists for this selected facility"));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "EquipmentCategory already exists for this selected facility"), true);
                        editdiv();
                    }

                }
                else
                {
                    // --->Performing Normal Update Opertaion
                    List<GetMachinemasterDetails> lstmachinemaster = new List<GetMachinemasterDetails>();
                    lstmachinemaster = lclserinven1.GetMachinemasterDetails().Where(lst => lst.FacilityID == Convert.ToInt64(drpFacility.SelectedValue) && lst.EquipmentCategoryID == Convert.ToInt64(drpEquipment.SelectedValue) && lst.EquipementListID == Convert.ToInt64(drpEquipmentlist.SelectedValue) && lst.MachineID != objbalmachinemaster.MachineID).ToList();
                    // --->Performing Check One to One Relationship for Faclity and Equipment
                    if (lstmachinemaster.Count == 0)
                    {
                        if (!ValidateLoookups(lclserinven)) return;
                        string b = lclserinven.UpdateMachineMaster(objbalmachinemaster);
                        if (b == "Saved Successfully")
                        {
                            string msg = Constant.MachineMasterUpdateMessage.Replace("ShowPopup('", "").Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text).Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterUpdateMessage.Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text), true);
                            closediv();
                            BindMachinemasterGrid();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ClearContent();", true);
                            hiddenMachineID.Value = "0";
                        }
                    }
                    else
                    {
                        log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "EquipmentCategory already exists for this selected facility"));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "EquipmentCategory already exists for this selected facility"), true);
                        editdiv();
                    }
                }
                
                hdnShowpanel.Value = "0";
                ViewState["Deletevalue"] = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        protected void Imageremoveyes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                StringBuilder sb = new StringBuilder();
                string d = string.Empty;
                string strname = string.Empty;
                InventoryServiceClient lclsService = new InventoryServiceClient();
                int i = 0;
                string deletevalues = Convert.ToString(ViewState["Deletevalue"]);
                List<CheckEquipmentlist> lstequiplist = new List<CheckEquipmentlist>();
                if (deletevalues == "equipdelete")
                {
                    Int64 Equipmentvalue = Convert.ToInt64(ViewState["drpEquipment"]);
                    lstequiplist = lclsService.GetCheckEquipmentlist(Equipmentvalue).ToList();
                    if (lstequiplist.Count > 0)
                    {
                        string msg = Constant.EquipMasterDelActMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipMasterDelActMessage, true);
                        editdiv();
                    }
                    else
                    {
                        d = lclsService.DeleteEquipeCategoryMaster(Convert.ToInt64(ViewState["drpEquipment"]), false, defaultPage.UserId);
                        if (d == "Deleted Successfully")
                        {
                            string msg = Constant.EquipMasterDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MachineMaster>>", txtEquipmentcategory.Text).Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipMasterDeleteMessage.Replace("<<EquipmentCategory>>", txtEquipmentcategory.Text), true);
                            hdnequipcat.Value = "0";
                            deletebtn.Style.Add("display", "none");
                            ViewState["Deletevalue"] = "";
                            ViewState["drpEquipment"] = 0;
                            BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                            drpEquipmentlist.Items.Clear();
                            drpEquipmentlist.DataSource = null;
                            drpEquipmentlist.DataBind();
                            editdiv();
                        }
                    }
                }
                else if (deletevalues == "equiplstdelete")
                {
                    d = lclsService.DeleteEquipListMaster(Convert.ToInt64(ViewState["drpEquipmentlist"]), false, defaultPage.UserId);
                    if (d == "Deleted Successfully")
                    {
                        string msg = Constant.EquipListDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MachineMaster>>", txtequilist.Text).Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipListDeleteMessage.Replace("<<EquipList>>", txtequilist.Text), true);
                        hdnequiplist.Value = "0";
                        deletebtn.Style.Add("display", "none");
                        ViewState["Deletevalue"] = "";
                        ViewState["drpEquipmentlist"] = "";
                        GetEquipementList(Convert.ToInt64(drpEquipment.SelectedValue), "Add");
                        editdiv();
                    }
                }
                else if (deletevalues == "equipSubCatdelete")
                {
                    BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                    objbalmachinemaster.EquipementSubCategoryID = Convert.ToInt64(ViewState["drpEquipmentSubCategory"]);
                    objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                    d = lclsService.DeleteEquipSubCategoryMaster(objbalmachinemaster);
                    if (d == "Deleted Successfully")
                    {
                        string msg = Constant.EquipSubCatDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<EquipSubCategory>>", txtequiSubCat.Text).Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipSubCatDeleteMessage.Replace("<<EquipSubCategory>>", txtequiSubCat.Text), true);
                        hdnequipSubCat.Value = "0";
                        deletebtn.Style.Add("display", "none");
                        ViewState["Deletevalue"] = "";
                        ViewState["drpEquipmentSubCategory"] = "";
                        GetEquipementSubCategory(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add");
                        editdiv();
                      
                    }
                }

                else
                {
                    InventoryServiceClient lclService = new InventoryServiceClient();
                    string lstrMessage = lclService.DeleteMachinemasterDetails(Convert.ToInt64(hiddenMachineID.Value), false, defaultPage.UserId);
                    if (lstrMessage == "Deleted Successfully")
                    {
                        BindMachinemasterGrid();
                        string msg = Constant.MachineMasterDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MachineMaster>>", hdnmachinename.Value).Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterDeleteMessage.Replace("<<MachineMaster>>", hdnmachinename.Value), true);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        #endregion
        #region Checkbox change Event
        protected void lbedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblseroutHeader.Visible = false;
                div_ADDContent.Style.Add("display", "block");
                savebtn.Style.Add("display", "block");
                deletebtn.Style.Add("display", "none");
                divmachine.Style.Add("display", "none");
                searchdiv.Style.Add("display", "none");
                btnprint.Visible = true;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                CheckBox chkRow = (gvrow.Cells[13].FindControl("chkactive") as CheckBox);
                InventoryServiceClient lclsService = new InventoryServiceClient();
                List<GetMachineMasterbasedMachineID> lstmachine = new List<GetMachineMasterbasedMachineID>();
                lstmachine = lclsService.GetMachinemasterbasedmachineID(Convert.ToInt64(gvrow.Cells[1].Text)).ToList();
                ViewState["MachineID"] = lstmachine[0].MachineID;
                hiddenMachineID.Value = Convert.ToString(lstmachine[0].MachineID);
                LoadLookups("Edit");
                if (lstmachine[0].FacilityID != 0)
                {
                    lblsubfacility.Text = lstmachine[0].FacilityDescription;
                    //drpFacility.SelectedItem.Text = lstmachine[0].FacilityDescription;
                    drpFacility.Items.FindByText(lstmachine[0].FacilityDescription).Selected = true;
                }

                txtmanufacturer.Text = lstmachine[0].Manufacturer;
                txtyear.Text = Convert.ToString(lstmachine[0].Manufacturedyear);
                BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Edit");
                drpEquipment.SelectedValue = Convert.ToString(lstmachine[0].EquipmentCategory);
                GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Edit");
                drpEquipmentSubCategory.SelectedValue = Convert.ToString(lstmachine[0].EquipementSubcategory);
                GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Edit");
                drpEquipmentlist.SelectedValue = Convert.ToString(lstmachine[0].EquipementList);
                if (drpEquipment.SelectedValue == "1")
                {
                    txthours.Text = Convert.ToString(lstmachine[0].Hoursonthemachine);
                    txthours.Enabled = true;
                }
                else
                {
                    txthours.Text = "";
                    txthours.Enabled = false;
                }
                txtmodel.Text = lstmachine[0].Model;
                txtserial.Text = lstmachine[0].SerialNo;
                txtwarrenty.Text = lstmachine[0].Warranty;
                txtgpcode.Text = lstmachine[0].GpAccountCode;
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                }
                else
                {
                    chkactive.Visible = true;
                }
                hdndelete.Value = "0";
                hdnShowpanel.Value = "1";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void btnaddequipment_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                List<object> lstIDwithmessage = new List<object>();
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                if (txtEquipmentcategory.Text == "")
                {
                    txtEquipmentcategory.Style.Add("border", "Solid 1px red");
                    mpeequipment.Show();
                }
                else
                {
                    if (hdnequipcat.Value != "1")
                    {
                        objbalmachinemaster.CorporateID = Convert.ToInt64(drpCorporate.SelectedValue);
                        objbalmachinemaster.FacilityID = Convert.ToInt64(drpFacility.SelectedValue);
                        if (Convert.ToString(drpEquipment.SelectedValue) != "0")
                            objbalmachinemaster.EquipementCategoryID = Convert.ToInt64(drpEquipment.SelectedValue);
                        ViewState["equipcatID"] = Convert.ToInt64(drpEquipment.SelectedValue);
                        objbalmachinemaster.EquipementCategorydesc = txtEquipmentcategory.Text;
                        hdnequipcat.Value = txtEquipmentcategory.Text;
                        objbalmachinemaster.CreatedBy = defaultPage.UserId;
                        objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                        if (objbalmachinemaster.EquipementCategoryID == 0)
                        {
                            List<GetEquipmentCategory> lstcat = lclserinven.GetEquipmentCategory(Convert.ToInt64(drpFacility.SelectedValue), "Add").Where(o => o.EquipmentCatDescription == txtEquipmentcategory.Text.ToString()).ToList();
                            if (lstcat.Count <= 0)
                            {
                                lstIDwithmessage = lclserinven.InsertEquipmentCategory(objbalmachinemaster).ToList();
                                string a = lstIDwithmessage[0].ToString();
                                if (a == "Saved Successfully")
                                {
                                    string msg = Constant.EquipMasterSaveMessage.Replace("ShowPopup('", "").Replace("<<EquipMasterSaveMessage>>", txtEquipmentcategory.Text).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipMasterSaveMessage.Replace("<<EquipMasterSaveMessage>>", txtEquipmentcategory.Text), true);
                                    BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                                }
                            }
                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }
                        }
                        else
                        {
                            List<GetEquipmentCategory> lstcat = lclserinven.GetEquipmentCategory(Convert.ToInt64(drpFacility.SelectedValue), "Add").Where(o => o.EquipmentCatDescription == txtEquipmentcategory.Text.ToString()).ToList();
                            if (lstcat.Count <= 0)
                            {
                                string b = lclserinven.UpdateEquipmentcategory(objbalmachinemaster);
                                if (b == "Saved Successfully")
                                {
                                    string msg = Constant.EquipMasterUpdateMessage.Replace("ShowPopup('", "").Replace("<<EquipmentCategory>>", txtEquipmentcategory.Text).Replace("');", "");
                                    log.LogWarning(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipMasterUpdateMessage.Replace("<<EquipmentCategory>>", txtEquipmentcategory.Text), true);
                                    BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                                }
                            }

                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }

                        }
                        txtEquipmentcategory.Text = "";
                        deletebtn.Style.Add("display", "none");
                        //divmachine.Style.Add("display", "none");
                        BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                        List<SavedEquipmentCategory> lstsave = lclserinven.SavedEquipmentCategory(Convert.ToInt64(drpCorporate.SelectedValue), Convert.ToInt64(ViewState["equipcatID"])).ToList();
                        drpEquipment.SelectedValue = Convert.ToString(lstsave[0].equipmentcategoryID);
                        drpEquipmentlist.ClearSelection();
                    }
                    else
                    {
                        //mpecatdel.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                        ViewState["Deletevalue"] = "equipdelete";
                        ViewState["drpEquipment"] = Convert.ToInt64(drpEquipment.SelectedValue);
                    }

                    divmachine.Style.Add("display", "none");
                    //drpEquipment.SelectedItem.Text = Convert.ToString(hdnequipcat.Value);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void lbeditequip_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtEquipmentcategory.Text = drpEquipment.SelectedItem.Text;
                ViewState["EquipmentCategoryID"] = drpEquipment.SelectedValue;
                //string n = drpEquipment.SelectedItem.Data;
                mpeequipment.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void btnequiplistsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                Functions objfun = new Functions();
                if (txtequilist.Text == "")
                {
                    txtequilist.Style.Add("border", "Solid 1px red");
                    mpeequiplist.Show();
                }
                else
                {
                    if (hdnequiplist.Value != "1")
                    {
                        if (drpEquipmentlist.SelectedValue != "0")
                            objbalmachinemaster.EquipementListID = Convert.ToInt64(drpEquipmentlist.SelectedValue);
                        objbalmachinemaster.EquipementSubCategoryID = Convert.ToInt64(drpEquipmentSubCategory.SelectedValue);
                        objbalmachinemaster.EquipementListdesc = txtequilist.Text;
                        hdnequiplist.Value = txtequilist.Text;
                        objbalmachinemaster.CreatedBy = defaultPage.UserId;
                        objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                        if (objbalmachinemaster.EquipementListID == 0)
                        {
                            List<GetEquipementList> lstEquiplist = lclserinven.GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add").Where(o => o.EquipmentListDescription == txtequilist.Text.ToString()).ToList();
                            if (lstEquiplist.Count <= 0)
                            {
                                string a = lclserinven.InsertEquipmentList(objbalmachinemaster);

                                if (a == "Saved Successfully")
                                {
                                    string msg = Constant.EquipListSaveMessage.Replace("ShowPopup('", "").Replace("<<EquipList>>", txtequilist.Text).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipListSaveMessage.Replace("<<EquipList>>", txtequilist.Text), true);
                                
                                }
                            }
                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }

                        }
                        else
                        {
                            List<GetEquipementList> lstEquiplist = lclserinven.GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add").Where(o => o.EquipmentListDescription == txtequilist.Text.ToString()).ToList();
                            if (lstEquiplist.Count <= 0)
                            {
                                string b = lclserinven.UpdateEquipmentList(objbalmachinemaster);
                                if (b == "Saved Successfully")
                                {
                                    string msg = Constant.EquipListUpdateMessage.Replace("ShowPopup('", "").Replace("<<EquipList>>", txtequilist.Text).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipListUpdateMessage.Replace("<<EquipList>>", txtequilist.Text), true);
                                   
                                }
                            }
                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }

                        }
                        txtequilist.Text = "";
                        deletebtn.Style.Add("display", "none");
                        List<SavedEquipmentList> lstequlst = lclserinven.SavedEquipmentList(Convert.ToInt64(drpEquipmentlist.SelectedValue)).ToList();
                        GetEquipementList(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue), "Add");
                        drpEquipmentlist.SelectedValue = Convert.ToString(lstequlst[0].EquipementListID);

                    }
                    else
                    {
                        // GetEquipementList(Convert.ToInt64(drpEquipment.SelectedValue));  
                        List<GetActiveEquipementListvalue> lstactiveeqlst = new List<GetActiveEquipementListvalue>();
                        lstactiveeqlst = lclserinven.GetActiveEquipementListvalue(Convert.ToInt64(drpEquipmentlist.SelectedValue)).ToList();
                        if (lstactiveeqlst.Count == 0)
                        {
                            //mpecatdel.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                            ViewState["Deletevalue"] = "equiplstdelete";
                            ViewState["drpEquipmentlist"] = Convert.ToInt64(drpEquipmentlist.SelectedValue);
                        }
                        else
                        {
                            // objfun.MessageDialog(this, "EquipmentList Should not allow to delete for Active MachineList");
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('EquipmentList Should not allow to delete for Active MachineList');", true);
                            string msg = Constant.EquiptListDelActMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
                            log.LogWarning(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquiptListDelActMessage, true);
                        }

                    }

                    //drpEquipmentlist.SelectedItem.Text = hdnequiplist.Value;
                }
                divmachine.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void imgequlstedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtequilist.Text = drpEquipmentlist.SelectedItem.Text;
                ViewState["EquipmentListID"] = drpEquipmentlist.SelectedValue;
                mpeequiplist.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }

        }

        protected void Ibaddequip_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mpeequipment.Show();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            try
            {
                hdndelete.Value = "0";
                hdnShowpanel.Value = "0";
                BindMachinemasterGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void imgdiagremove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                hdndelete.Value = "0";
                BindMachinemasterGrid();
                savebtn.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "none");
                deletebtn.Style.Add("display", "block");

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                closediv();
                BindMachinemasterGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        #endregion


        public void closediv()
        {
            try
            {
                lblseroutHeader.Visible = true;
                div_ADDContent.Style.Add("display", "none");
                savebtn.Style.Add("display", "none");
                deletebtn.Style.Add("display", "block");
                divmachine.Style.Add("display", "block");
                searchdiv.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        public void editdiv()
        {
            try
            {
                div_ADDContent.Style.Add("display", "block");
                savebtn.Style.Add("display", "block");
                deletebtn.Style.Add("display", "none");
                divmachine.Style.Add("display", "none");
                searchdiv.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
     

        protected void gvmachinemaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lbldelete = (ImageButton)e.Row.FindControl("lbldelete");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    if (lblActive.Text == "Yes")
                    {
                        lbldelete.Enabled = true;
                        chkactive.Visible = false;
                    }
                    else
                    {
                        lbldelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkactive.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ItemCategoryErrorMessage.Replace("<<ItemCategory>>", ex.Message), true);
            }
        }
        protected void btnrestore_Click(object sender, EventArgs e)
        {
            //mperestore.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowRestorePopup()", true);
            editdiv();
        }

        protected void imgreyes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                string b = string.Empty;
                // string b = lclserinven.UpdateMachineMaster(objbalmachinemaster);
                b = lclserinven.DeleteMachinemasterDetails(Convert.ToInt64(ViewState["MachineID"]), true, defaultPage.UserId);
                if (b == "Deleted Successfully")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "ShowSuccess('Record Restored Successfully');", true);
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.MachineMasterRestoreMessage.Replace("ShowPopup('", "").Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterRestoreMessage.Replace("<<MachineMaster>>", drpEquipment.SelectedItem.Text), true);
                    chkactive.Checked = true;
                }
                else
                {
                    chkactive.Checked = false;
                }
                hdndelete.Value = "0";
                ControlEnabletrue();
                BindMachinemasterGrid();
                closediv();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
          

        }

        protected void imgreno_Click(object sender, ImageClickEventArgs e)
        {
            editdiv();
            hdndelete.Value = "0";
            BindMachinemasterGrid();
           
        }
        public void ControlEnablefalse()
        {
            try
            {
                lblsubfacility.Enabled = false;
                Ibaddequip.Enabled = false;
                lbeditequip.Enabled = false;
                Ibdeleqip.Enabled = false;
                drpEquipment.Enabled = false;
                txtwarrenty.Enabled = false;
                txtmanufacturer.Enabled = false;
                imgeequipadd.Enabled = false;
                imgequlstedit.Enabled = false;
                imgequiplstdelete.Enabled = false;
                drpEquipmentlist.Enabled = false;
                txtgpcode.Enabled = false;
                txtyear.Enabled = false;
                txtmodel.Enabled = false;
                txthours.Enabled = false;
                txtserial.Enabled = false;
                RegularExpressionValidator1.Enabled = false;
                rvYear.Enabled = false;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
           
        }
        public void ControlEnabletrue()
        {
            try
            {
                if (defaultPage.RoleID == 1)
                {
                    lblsubfacility.Enabled = true;
                }
                else
                {
                    lblsubfacility.Enabled = false;
                }

                Ibaddequip.Enabled = true;
                lbeditequip.Enabled = true;
                Ibdeleqip.Enabled = true;
                drpEquipment.Enabled = true;
                txtwarrenty.Enabled = true;
                txtmanufacturer.Enabled = true;
                imgeequipadd.Enabled = true;
                imgequlstedit.Enabled = true;
                imgequiplstdelete.Enabled = true;
                drpEquipmentlist.Enabled = true;
                txtgpcode.Enabled = true;
                txtyear.Enabled = true;
                txtmodel.Enabled = true;
                txtserial.Enabled = true;
                RegularExpressionValidator1.Enabled = true;
                rvYear.Enabled = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
          
        }

    
        protected void drpFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblsubfacility.Text = drpFacility.SelectedItem.Text;
            closediv();
        }

        protected void btnequipSubCatsave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                InventoryServiceClient lclserinven = new InventoryServiceClient();
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                Functions objfun = new Functions();
                if (txtequiSubCat.Text == "")
                {
                    txtequiSubCat.Style.Add("border", "Solid 1px red");
                    mpeequipSubCat.Show();
                }
                else
                {
                    if (hdnequipSubCat.Value != "1")
                    {
                        if (drpEquipmentSubCategory.SelectedValue != "0")
                            objbalmachinemaster.EquipementSubCategoryID = Convert.ToInt64(drpEquipmentSubCategory.SelectedValue);
                        objbalmachinemaster.EquipementCategoryID = Convert.ToInt64(drpEquipment.SelectedValue);
                        objbalmachinemaster.EquipementSubCategorydesc = txtequiSubCat.Text;
                        hdnequipSubCat.Value = txtequiSubCat.Text;
                        objbalmachinemaster.CreatedBy = defaultPage.UserId;
                        objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                        if (objbalmachinemaster.EquipementSubCategoryID == 0)
                        {
                            List<GetEquipementSubCategory> lstEquiplist = lclserinven.GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add").Where(o => o.EquipmentSubCategoryDescription == txtequiSubCat.Text.ToString()).ToList();
                            if (lstEquiplist.Count <= 0)
                            {
                                string a = lclserinven.InsertEquipmentSubCategory(objbalmachinemaster);
                                if (a == "Saved Successfully")
                                {
                                    string msg = Constant.MachineMasterRestoreMessage.Replace("ShowPopup('", "").Replace("<<EquipSubCategory>>", txtequiSubCat.Text).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipSubCatSaveMessage.Replace("<<EquipSubCategory>>", txtequiSubCat.Text), true);
                                  
                                }
                            }
                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }

                        }
                        else
                        {
                            List<GetEquipementSubCategory> lstEquiplist = lclserinven.GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add").Where(o => o.EquipmentSubCategoryDescription == txtequiSubCat.Text.ToString()).ToList();
                            if (lstEquiplist.Count <= 0)
                            {
                                string b = lclserinven.UpdateEquipmentSubCategory(objbalmachinemaster);
                                if (b == "Saved Successfully")
                                {
                                    string msg = Constant.EquipSubCatUpdateMessage.Replace("ShowPopup('", "").Replace("<<EquipSubCategory>>", txtequiSubCat.Text).Replace("');", "");
                                    log.LogInformation(msg);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipSubCatUpdateMessage.Replace("<<EquipSubCategory>>", txtequiSubCat.Text), true);
                                   
                                }
                            }
                            else
                            {
                                log.LogWarning(msgwrn.Replace("<<MachineMaster>>", "Record exists"));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningMachineMasterMessage.Replace("<<MachineMaster>>", "Record exists"), true);
                            }

                        }
                        txtequiSubCat.Text = "";
                        deletebtn.Style.Add("display", "none");
                        List<SavedEquipmentSubCategory> lstequlst = lclserinven.SavedEquipmentSubCategory(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue)).ToList();
                        GetEquipementSubCategory(Convert.ToInt64(drpEquipment.SelectedValue), "Add");
                        drpEquipmentSubCategory.SelectedValue = Convert.ToString(lstequlst[0].EquipementSubCategoryID);

                    }
                    else
                    {
                        // GetEquipementList(Convert.ToInt64(drpEquipment.SelectedValue));  
                        List<GetActiveEquipementSubCategoryvalue> lstactiveeqlst = new List<GetActiveEquipementSubCategoryvalue>();
                        lstactiveeqlst = lclserinven.GetActiveEquipementSubCategoryvalue(Convert.ToInt64(drpEquipmentSubCategory.SelectedValue)).ToList();
                        if (lstactiveeqlst.Count == 0)
                        {
                            //mpecatdel.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                            ViewState["Deletevalue"] = "equipSubCatdelete";
                            ViewState["drpEquipmentSubCategory"] = Convert.ToInt64(drpEquipmentSubCategory.SelectedValue);
                        }
                        else
                        {
                            // objfun.MessageDialog(this, "EquipmentList Should not allow to delete for Active MachineList");
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "ShowWarning('EquipmentList Should not allow to delete for Active MachineList');", true);
                            string msg = Constant.EquiptSubCatDelActMessage.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
                            log.LogWarning(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquiptSubCatDelActMessage, true);
                        }
                    }

                    //drpEquipmentlist.SelectedItem.Text = hdnequiplist.Value;
                }
                divmachine.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
        protected void btnsearchprint_Click(object sender, EventArgs e)
        {
            try
            {
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                objbalmachinemaster.FacilityID = Convert.ToInt64(drpFacility.SelectedValue);
                objbalmachinemaster.IstrActive = reactive.SelectedValue;
                objbalmachinemaster.LastModifiedBy = defaultPage.UserId;
                objbalmachinemaster.Filter = "";
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetMachineMasterReport> llstreview = lclsservice.GetMachineMasterReport(objbalmachinemaster).ToList();
                rvmachinesummaryreport.ProcessingMode = ProcessingMode.Local;
                rvmachinesummaryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachineMasterSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetMachineMasterSummary", llstreview);
                rvmachinesummaryreport.LocalReport.DataSources.Clear();
                rvmachinesummaryreport.LocalReport.DataSources.Add(datasource);
                rvmachinesummaryreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvmachinesummaryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MachineMasterSummary" + guid + ".pdf";
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
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
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                BALMachinemaster objbalmachinemaster = new BALMachinemaster();
                objbalmachinemaster.MachineID = Convert.ToInt64(hiddenMachineID.Value);
                objbalmachinemaster.LoggedinBy = defaultPage.UserId;
                objbalmachinemaster.Filter = "";
                List<GetMachineMasterDetailsReport> llstreview = lclsservice.GetMachineMasterDetailsReport(objbalmachinemaster).ToList();
                rvmachineDetailreport.ProcessingMode = ProcessingMode.Local;
                rvmachineDetailreport.LocalReport.ReportPath = Server.MapPath("~/Reports/MachineMasterDetailReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("GetMachineMasterDetail", llstreview);
                rvmachineDetailreport.LocalReport.DataSources.Clear();
                rvmachineDetailreport.LocalReport.DataSources.Add(datasource);
                rvmachineDetailreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvmachineDetailreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "MachineMasterDetails" + guid + ".pdf";
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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Showadddiv", "ShowContent();", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
           
        }

        protected void lbldelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hiddenMachineID.Value = gvrow.Cells[1].Text;
                hdnmachinename.Value = gvrow.Cells[3].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }

        }
        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteMachinemasterDetails(Convert.ToInt64(hiddenMachineID.Value), false, defaultPage.UserId);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindMachinemasterGrid();
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.MachineMasterDeleteMessage.Replace("ShowdelPopup('", "").Replace("<<MachineMaster>>", hdnmachinename.Value).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterDeleteMessage.Replace("<<MachineMaster>>", hdnmachinename.Value), true);
                    hiddenMachineID.Value = "0";
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        protected void btncancelsch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadLookups("Add");
                BindCorporate();
                drpCorporate.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                BindFacility("Add");
                drpFacility.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                reactive.SelectedValue = "1";
                BindMachinemasterGrid();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }

        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                hiddenMachineID.Value = "0";
                lblseroutHeader.Visible = false;
                btnprint.Visible = false;
                div_ADDContent.Style.Add("display", "block");
                savebtn.Style.Add("display", "block");
                deletebtn.Style.Add("display", "none");
                divmachine.Style.Add("display", "none");
                searchdiv.Style.Add("display", "none");
                BindEquipcategory(Convert.ToInt64(drpCorporate.SelectedValue), "Add");
                Clear();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }

        public void Clear()
        {
            try
            {
                txtwarrenty.Text = "";
                txtmanufacturer.Text = "";
                txtgpcode.Text = "";
                txtyear.Text = "";
                txtmodel.Text = "";
                txthours.Text = "";
                txtserial.Text = "";
                drpEquipment.SelectedValue = "0";
                drpEquipmentSubCategory.SelectedValue = "0";
                drpEquipmentlist.SelectedValue = "0";
            }
           catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MachineMasterErrorMessage.Replace("<<MachineMaster>>", ex.Message), true);
            }
        }
    }
}
