#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Net;
using System.Configuration;
using Inventory.Inventoryserref;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using Inventory.Class;
using System.Threading;
#endregion


#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<MachinePartsRequest>>
'' Type      :   C# File
'' Description  :<<To add,update the Machine Parts Request Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	09/25/2017		   V1.0				   Vivekanand.S		                  New
'' 10/25/2017           V.01              Vivekanand.S                  Locked the record.
 ''--------------------------------------------------------------------------------
'*/
#endregion



namespace Inventory
{
    public partial class CorpEquipmentMap : System.Web.UI.Page
    {
        #region Declarations
        Page_Controls defaultPage = new Page_Controls();
        #endregion
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        BALServiceRequest llstServiceRequest = new BALServiceRequest();
        string a = string.Empty;
        string b = string.Empty;
        string ErrorList = string.Empty;
        private string _sessionPDFFileName;
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.EquipCorpMapNoRecordMessage.Replace("ShowdelPopup('", "").Replace("');", "");
        string msgupdte = Constant.EquipCorpMapUpdateMessage.Replace("ShowPopup('", "").Replace("');", "");
        string msgdelte = Constant.EquipCorpMapDeleteMessage.Replace("ShowdelPopup('", "").Replace("');", "");
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                scriptManager.RegisterPostBackControl(this.GrdEquipmentSearch);
                if (!IsPostBack)
                {

                    //BindServiceList("Add");                   
                    if (defaultPage != null)
                    {
                        BindCorporate();

                        //BindFacility(1, "Add");

                        //BindServiceCategory(Convert.ToInt64(defaultPage.CorporateID), "Add");
                        //BindServiceList("Add");
                        //BindEquipcategory(Convert.ToInt64(defaultPage.CorporateID), "Add");
                        //GetEquipementList("Add");
                        //BindVendor(1);
                        if (defaultPage.RoleID == 1)
                        {
                            drpcor.Enabled = true;

                            ddlCorporate.Enabled = true;

                            BindGrid();
                        }
                        else
                        {
                            drpcor.Enabled = false;
                            ddlCorporate.Enabled = false;



                            if (defaultPage.CorpEquipmentMap_Edit == false && defaultPage.CorpEquipmentMap_View == true)
                            {
                                btnAdd.Visible = false;
                                btnSave.Visible = false;
                                drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                                SearchGrid("");
                            }
                            if (defaultPage.CorpEquipmentMap_Edit == false && defaultPage.CorpEquipmentMap_View == false)
                            {
                                updmain.Visible = false;
                                User_Permission_Message.Visible = true;
                                drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                                SearchGrid("");
                            }
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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }
        #endregion


        /// <summary>
        /// Bind the Corporate details to dropdown control 
        /// </summary>
        #region Bind Corporate Values
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstfacility = new List<BALUser>();
                lstfacility = lclsservice.GetCorporateMaster().ToList();

                // Search Drop Down
                drpcor.DataSource = lstfacility;
                drpcor.DataTextField = "CorporateName";
                drpcor.DataValueField = "CorporateID";
                drpcor.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select Corporate--";
                drpcor.Items.Insert(0, lst);
                drpcor.SelectedIndex = 0;

                ListItem lstDDl = new ListItem();
                lstDDl.Value = "0";
                lstDDl.Text = "--Select Corporate--";
                // Insert Drop Down
                ddlCorporate.DataSource = lstfacility;
                ddlCorporate.DataTextField = "CorporateName";
                ddlCorporate.DataValueField = "CorporateID";
                ddlCorporate.DataBind();
                ddlCorporate.Items.Insert(0, lstDDl);
                ddlCorporate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }
        #endregion

        /// <summary>
        /// Bind the Machine Parts Request Master details from MPRMaster table to Grid control 
        /// </summary>
        #region Bind Machine Parts Request Master Values
        public void BindGrid()
        {
            try
            {
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = "";
                ObjSearchEquipment.Active = rdbstatus.SelectedValue;
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Edit";
                ObjSearchEquipment.Filter = "";

                List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).ToList();
                //lblrowcount.Text = "No of records : " + lstMPRMaster.Count.ToString();
                GrdEquipmentSearch.DataSource = lstMPRMaster;
                GrdEquipmentSearch.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }
        #endregion



        private void SetAddScreen(string Add, int i)
        {
            try
            {
                if (i == 1)
                {

                    if (Add == "Add")
                    {
                        btnSave.Enabled = true;
                        DivAddEquipmentCategory.Style.Add("display", "block");
                        DivEditEquipmentCategory.Style.Add("display", "none");
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        DivAddEquipmentCategory.Style.Add("display", "none");
                        DivEditEquipmentCategory.Style.Add("display", "block");
                    }
                    if (defaultPage.RoleID == 1)
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        ddlCorporate.Enabled = false;
                        if (defaultPage.CorpEquipmentMap_Edit == true)
                        {
                            btnSave.Visible = true;
                            btnAdd.Visible = false;
                        }
                        else
                        {
                            btnSave.Enabled = false;
                            txtEquipmentCategory.Enabled = false;
                            txtEquipmentSubCategory.Enabled = false;
                            btnAddSave.Enabled = false;
                            if (GrdEditEquipmentsub.DataSource != null)
                            {
                                for (int j = 0; j < GrdEditEquipmentsub.Rows.Count; j++)
                                {
                                    ImageButton IbEditEdit = (ImageButton)GrdEditEquipmentsub.Rows[j].Cells[1].FindControl("IbEditEdit");
                                    IbEditEdit.Enabled = false;
                                }

                            }
                        }
                    }


                    btnPrint.Visible = false;
                    btnClose.Visible = true;
                    btnSearch.Visible = false;
                    //btnSave.Style.Add("display", "block");
                    //btnPrint.Style.Add("display", "none");
                    //btnSearch.Style.Add("display", "none");
                    //btnAdd.Style.Add("display", "none");
                    //btnClose.Style.Add("display", "block");                    


                    lblUpdateHeader.Visible = true;
                    lblMasterHeader.Visible = false;
                    lblseroutHeader.Visible = false;

                    divEdit.Style.Add("display", "none");
                    divSRMaster.Style.Add("display", "none");
                    divSRDetails.Style.Add("display", "block");


                }
                else
                {
                    if (defaultPage.RoleID == 1)
                    {
                        ddlCorporate.Enabled = true;
                        btnAdd.Visible = true;
                        btnSave.Visible = false;
                        BindGrid();
                    }
                    else
                    {
                        ddlCorporate.Enabled = false;
                        if (defaultPage.CorpEquipmentMap_Edit == true && defaultPage.CorpEquipmentMap_View == true)
                        {
                            btnSave.Visible = false;
                            btnAdd.Visible = true;
                        }
                        if (defaultPage.RoleID == 1)
                        {
                            BindGrid();
                        }
                        else
                        {


                        }
                    }

                    btnClose.Visible = true;
                    btnSearch.Visible = true;

                    //BindCorporate();
                    //drpcor.SelectedValue = Convert.ToString(defaultPage.CorporateID);


                    lblUpdateHeader.Visible = false;
                    lblMasterHeader.Visible = true;
                    lblEditHeader.Visible = false;
                    divSRDetails.Style.Add("display", "none");

                    lblseroutHeader.Visible = true;
                    divSRMaster.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }

        private void ClearMaster()
        {
            drpcor.SelectedIndex = 0;
            txtEquipmentSearch.Text = "";
            BindGrid();
        }

        private void ClearDetails()
        {
            try
            {
                ddlCorporate.SelectedIndex = 0;
                txtEquipmentCategory.Text = "";
                txtEquipmentSubCategory.Text = "";

                GrdAddEquipmentsub.DataSource = null;
                GrdAddEquipmentsub.DataBind();

                btnPrint.Visible = false;
                lbpopprint.Visible = false;
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
            }
            
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SetAddScreen("Add", 1);
                ClearDetails();
                HddEquiCancel.Value = "1";
                if (drpcor.SelectedValue != "0")
                {
                    ddlCorporate.SelectedValue = drpcor.SelectedValue;
                }
                HddEquipmentCategoryID.Value = "";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (HddEquiCancel.Value == "0")
                {
                    Response.Redirect("CorpEquipmentMap.aspx");
                }
                else
                {
                    SetAddScreen("", 0);
                    SearchGrid(txtEquipmentSearch.Text);
                }
                HddEquiCancel.Value = "0";
                HddEquipementSubCategoryID.Value = "";
                //ClearMaster();
                //ClearDetails();
                btnPrint.Visible = true;
                //btnPrint.Visible = false;
                lbpopprint.Visible = false;
                btnAddSave.Text = "Add";

                ViewState["EquipmentSubCat"] = null;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }




        public void InsertEquipmentCategory()
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALMachinemaster objbalEquipmentSubCatmaster = new BALMachinemaster();
                List<object> lstIDwithmessage = new List<object>();
                string a = string.Empty;
                objbalEquipmentSubCatmaster.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                objbalEquipmentSubCatmaster.FacilityID = 0;
                objbalEquipmentSubCatmaster.EquipementCategorydesc = txtEquipmentCategory.Text;
                objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = "";
                ObjSearchEquipment.Active = "";
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Filter = "";

                HddEquiSubCatList.Value = "";

                List<BindEquipement> lstMPRMaster1 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCatDescription == objbalEquipmentSubCatmaster.EquipementCategorydesc).ToList();

                int lincount = lstMPRMaster1.Count;

                if (lstMPRMaster1.Count <= 0)
                {
                    lstIDwithmessage = lclsservice.InsertEquipmentCategory(objbalEquipmentSubCatmaster).ToList();
                    a = lstIDwithmessage[0].ToString();
                }
                else
                {
                    log.LogWarning(msgwrn.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapNoRecordMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                }

                if (a == "Saved Successfully")
                {
                    if (GrdAddEquipmentsub.Rows.Count != 0)
                    {
                        foreach (GridViewRow grdfs in GrdAddEquipmentsub.Rows)
                        {
                            Label EquipmentSubCat = (Label)grdfs.FindControl("lblEquipmentSubCat");

                            objbalEquipmentSubCatmaster.EquipementSubCategorydesc = EquipmentSubCat.Text;
                            objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(lstIDwithmessage[1].ToString());
                            objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;

                            a = lclsservice.InsertEquipmentSubCategory(objbalEquipmentSubCatmaster);
                        }

                        if (txtEquipmentSubCategory.Text != "")
                        {
                            objbalEquipmentSubCatmaster.EquipementSubCategorydesc = txtEquipmentSubCategory.Text;
                            objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(lstIDwithmessage[1].ToString());
                            objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;

                            a = lclsservice.InsertEquipmentSubCategory(objbalEquipmentSubCatmaster);
                        }

                        if (a == "Saved Successfully")
                        {
                            string msg = Constant.EquipCorpMapMessage.Replace("ShowPopup('", "").Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text).Replace("');", "");
                            log.LogInformation(msg);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                            txtEquipmentSubCategory.Text = "";
                            btnAddSave.Text = "Add";
                            List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(b => b.EquipmentCategoryID == Convert.ToInt64(lstIDwithmessage[1]) && b.IsActive == true).ToList();
                            GrdEditEquipmentsub.DataSource = lstMPRMaster;
                            GrdEditEquipmentsub.DataBind();
                            SetAddScreen("", 0);

                            GrdAddEquipmentsub.DataSource = null;
                            GrdAddEquipmentsub.DataBind();
                            ViewState["EquipmentSubCat"] = null;                            
                        }
                    }
                    else
                    {
                        string msg = Constant.EquipCorpMapMessage.Replace("ShowPopup('", "").Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text).Replace("');", "");
                        log.LogInformation(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                        HddEquipmentCategoryID.Value = lstIDwithmessage[1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }

        }

        public void UpdateEquipmentCategory()
        {
            try
            {
                EventLogger log = new EventLogger(config);
                BALMachinemaster objbalEquipmentSubCatmaster = new BALMachinemaster();
                objbalEquipmentSubCatmaster.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                objbalEquipmentSubCatmaster.FacilityID = 0;
                objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(HddEquipmentCategoryID.Value);
                objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;
                objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;
                string c = string.Empty;
                if (chkactive.Checked == true)
                {
                    objbalEquipmentSubCatmaster.IsActive = true;
                }
                else
                {
                    objbalEquipmentSubCatmaster.IsActive = false;
                }
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = "";
                ObjSearchEquipment.Active = "";
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Edit";
                ObjSearchEquipment.Filter = "";
                HddEquiSubCatList.Value = "";
                List<BindEquipement> lstEquisubcatMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCategoryID == objbalEquipmentSubCatmaster.EquipementCategoryID).ToList();
                if(chkactive.Checked)
                {
                    foreach(GridViewRow Rows in GrdEditEquipmentsub.Rows)
                    {
                        Label lblEquipmentSubCatID = (Label)Rows.FindControl("lblEquipmentSubCatID");
                        Label lblEquipmentSubCat = (Label)Rows.FindControl("lblEquipmentSubCat");

                        BALMachinemaster objbalEquipmentSubCatmaster1 = new BALMachinemaster();

                        objbalEquipmentSubCatmaster.EquipementSubCategoryID = Convert.ToInt64(lblEquipmentSubCatID.Text);
                        objbalEquipmentSubCatmaster.EquipementSubCategorydesc = lblEquipmentSubCat.Text;
                        objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(HddEquipmentCategoryID.Value);
                        objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;


                        c = lclsservice.UpdateEquipmentSubCategory(objbalEquipmentSubCatmaster);


                    }
                }
                if (GrdAddEquipmentsub.Rows.Count != 0)
                {
                    objbalEquipmentSubCatmaster.EquipementCategorydesc = txtEquipmentCategory.Text;

                    List<BindEquipement> lstMPRMaster1 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCatDescription == objbalEquipmentSubCatmaster.EquipementCategorydesc).ToList();


                    if (lstMPRMaster1.Count <= 0)
                    {
                        a = lclsservice.UpdateEquipmentcategory(objbalEquipmentSubCatmaster);
                    }
                    if (lstEquisubcatMaster[0].EquipmentCatDescription == txtEquipmentCategory.Text || a == "Saved Successfully")
                    {
                        foreach (GridViewRow grdfs in GrdAddEquipmentsub.Rows)
                        {
                            Label EquipmentSubCat = (Label)grdfs.FindControl("lblEquipmentSubCat");

                            objbalEquipmentSubCatmaster.EquipementSubCategorydesc = EquipmentSubCat.Text;
                            objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(HddEquipmentCategoryID.Value);
                            objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;
                            List<BindEquipement> lstMPRMaster2 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCategoryID == objbalEquipmentSubCatmaster.EquipementCategoryID && o.EquipmentSubCategoryDescription == objbalEquipmentSubCatmaster.EquipementSubCategorydesc).ToList();

                            if (lstMPRMaster2.Count <= 0)
                            {
                                a = lclsservice.InsertEquipmentSubCategory(objbalEquipmentSubCatmaster);
                            }
                            else
                            {
                                HddEquiSubCatList.Value += objbalEquipmentSubCatmaster.EquipementSubCategorydesc + ",";
                            }
                        }

                        if (txtEquipmentSubCategory.Text != "")
                        {
                            objbalEquipmentSubCatmaster.EquipementSubCategorydesc = txtEquipmentSubCategory.Text;
                            objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(HddEquipmentCategoryID.Value);
                            objbalEquipmentSubCatmaster.CreatedBy = defaultPage.UserId;
                            List<BindEquipement> lstMPRMaster2 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCategoryID == objbalEquipmentSubCatmaster.EquipementCategoryID && o.EquipmentSubCategoryDescription == objbalEquipmentSubCatmaster.EquipementSubCategorydesc).ToList();

                            if (lstMPRMaster2.Count <= 0)
                            {
                                a = lclsservice.InsertEquipmentSubCategory(objbalEquipmentSubCatmaster);
                            }
                            else
                            {
                                HddEquiSubCatList.Value += objbalEquipmentSubCatmaster.EquipementSubCategorydesc + ",";
                            }
                        }
                    }


                    if (a == "Saved Successfully")
                    {
                        log.LogInformation(msgupdte.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapUpdateMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                        txtEquipmentSubCategory.Text = "";
                        btnAddSave.Text = "Add";
                        List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(b => b.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && b.IsActive == true).ToList();
                        GrdEditEquipmentsub.DataSource = lstMPRMaster;
                        GrdEditEquipmentsub.DataBind();
                        SetAddScreen("", 0);

                        GrdAddEquipmentsub.DataSource = null;
                        GrdAddEquipmentsub.DataBind();
                        ViewState["EquipmentSubCat"] = null;
                        HddEquiSubCatList.Value = "";
                       

                    }
                    else if (HddEquiSubCatList.Value != "")
                    {
                        log.LogWarning(msgwrn.Replace("<<CorpEquipmentMapCategory>>", HddEquiSubCatList.Value));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapNoRecordMessage.Replace("<<CorpEquipmentMapCategory>>", HddEquiSubCatList.Value), true);
                    }
                    else
                    {
                        log.LogWarning(msgwrn.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapNoRecordMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                        HddEquiSubCatList.Value = "";
                    }
                }
                else
                {
                    objbalEquipmentSubCatmaster.CorporateID = Convert.ToInt64(ddlCorporate.SelectedValue);
                    objbalEquipmentSubCatmaster.FacilityID = 0;
                    objbalEquipmentSubCatmaster.EquipementCategorydesc = txtEquipmentCategory.Text;
                    objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;

                    List<BindEquipement> lstMPRMaster1 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCatDescription == objbalEquipmentSubCatmaster.EquipementCategorydesc).ToList();


                    if (lstMPRMaster1.Count <= 0)
                    {
                        a = lclsservice.UpdateEquipmentcategory(objbalEquipmentSubCatmaster);
                    }

                    if ((lstEquisubcatMaster[0].EquipmentCatDescription == txtEquipmentCategory.Text || a == "Saved Successfully") && txtEquipmentSubCategory.Text != "")
                    {
                        objbalEquipmentSubCatmaster.EquipementSubCategorydesc = txtEquipmentSubCategory.Text;


                        List<BindEquipement> lstMPRMaster2 = lclsservice.BindEquipement(ObjSearchEquipment).Where(o => o.EquipmentCatDescription == objbalEquipmentSubCatmaster.EquipementCategorydesc && o.EquipmentSubCategoryDescription == objbalEquipmentSubCatmaster.EquipementSubCategorydesc).ToList();


                        if (lstMPRMaster2.Count <= 0)
                        {
                            a = lclsservice.InsertEquipmentSubCategory(objbalEquipmentSubCatmaster);
                            List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(b => b.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && b.IsActive == true).ToList();
                            GrdEditEquipmentsub.DataSource = lstMPRMaster;
                            GrdEditEquipmentsub.DataBind();
                            txtEquipmentSubCategory.Text = "";
                            SetAddScreen("", 0);
                        }
                    }
                    if (a == "Saved Successfully")
                    {
                        log.LogInformation(msgupdte.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapUpdateMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                        SetAddScreen("", 0);
                    }
                    else
                    {
                        if(c == "Saved Successfully")
                        {
                            log.LogInformation(msgupdte.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapUpdateMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                            SetAddScreen("", 0);
                        }
                        else
                        {
                            log.LogWarning(msgwrn.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapNoRecordMessage.Replace("<<CorpEquipmentMapCategory>>", txtEquipmentCategory.Text), true);
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if(GrdAddEquipmentsub.Rows.Count != 0)
                //{
                if (HddEquipmentCategoryID.Value == "")
                {
                    InsertEquipmentCategory();
                }
                else
                {
                    UpdateEquipmentCategory();
                }
                //}
                //else
                //{
                //    //RequiredFieldValidator1.Visible = true;
                //    //RequiredFieldValidator1.Validate();
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", "No Equipment Sub Category is Added"), true);
                //}            
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }

        private void EditDisplayControls()
        {
            try
            {
                SetAddScreen("", 1);

                divEdit.Style.Add("display", "block");
                ddlCorporate.Enabled = false;
                //  btnPrint.Visible = true;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string SearchData = string.Empty;
                List<BindEquipement> llstreview = null;
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = txtEquipmentSearch.Text;
                ObjSearchEquipment.Active = rdbstatus.SelectedValue;
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Edit";
                ObjSearchEquipment.Filter = "";

                if (drpcor.SelectedValue != "0")
                {
                    llstreview = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.CorporateID == Convert.ToInt64(drpcor.SelectedValue)).ToList();
                }
                else
                {
                    llstreview = lclsservice.BindEquipement(ObjSearchEquipment).ToList();
                }


                rvservicerequestreport.ProcessingMode = ProcessingMode.Local;
                rvservicerequestreport.LocalReport.ReportPath = Server.MapPath("~/Reports/EquipementCategoryReport.rdlc");

                Int64 r = defaultPage.UserId;

                ReportDataSource datasource = new ReportDataSource("EquipementCategoryReportDS", llstreview);
                rvservicerequestreport.LocalReport.DataSources.Clear();
                rvservicerequestreport.LocalReport.DataSources.Add(datasource);
                rvservicerequestreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvservicerequestreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);


                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "SessionFile" + Session.SessionID + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EndingInventoryErrorMessage.Replace("<<EndingInventory>>", ex.Message), true);
            }
        }

        protected void btnImgView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkbtn = sender as ImageButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvrow.Cells[2].Text;
                string filepdfpath = Server.MapPath(filePath);
                //string output = Regex.Replace(filepdfpath, "[\\]", "/");

                string extension = System.IO.Path.GetExtension(filePath);
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + Server.MapPath(filePath) + "\"");
                byte[] data = req.DownloadData(Server.MapPath(filePath));
                if (extension == ".pdf")
                {
                    // Open PDF File in Web Browser 
                    mpeimgview.Show();
                    frame1.Attributes["src"] = filePath;
                    //byte[] buffer = req.DownloadData(Server.MapPath(filePath));
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-length", buffer.Length.ToString());
                    //Response.BinaryWrite(buffer);                
                }
                else
                {

                    response.BinaryWrite(data);
                    Response.Redirect(filePath, false);

                    //mpeimgview.Show();
                    //frame1.Attributes["src"] = "http://docs.google.com/viewer?embedded=true&url=" + filePath;

                    //frame1.Src = "";
                    //frame1.Attributes.Clear();
                    //frame1.Dispose();
                    //frame1.Attributes["src"] = filePath;                
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
            //response.End();
        }


        protected void btnpopclose_Click(object sender, EventArgs e)
        {
            mpeimgview.Hide();
        }

        /// <summary>
        /// Search Service Request Master details gridview
        /// </summary>
        #region Bind Search Values
        public void SearchGrid(string SearchData)
        {
            try
            {
                List<BindEquipement> lstMPRMaster = null;
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = SearchData;
                ObjSearchEquipment.Active = rdbstatus.SelectedValue;
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Edit";
                ObjSearchEquipment.Filter = "";

                if(drpcor.SelectedValue != "0")
                {
                    lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.CorporateID == Convert.ToInt64(drpcor.SelectedValue)).ToList();
                    //lblrowcount.Text = "No of records : " + lstMPRMaster.Count.ToString();
                    GrdEquipmentSearch.DataSource = lstMPRMaster;
                    GrdEquipmentSearch.DataBind();
                }
                else
                {
                    lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).ToList();
                    //lblrowcount.Text = "No of records : " + lstMPRMaster.Count.ToString();
                    GrdEquipmentSearch.DataSource = lstMPRMaster;
                    GrdEquipmentSearch.DataBind();
                }
                    
                //if (rdbstatus.SelectedValue == "All")
                //    lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.CorporateID == Convert.ToInt64(drpcor.SelectedValue)).ToList();
                //else
               
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            SearchGrid(txtEquipmentSearch.Text);
        }


        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEquipmentSubCategory.Text != "")
                {
                    if (btnAddSave.Text == "Add")
                    {
                        DivAddEquipmentCategory.Style.Add("display", "block");
                        DataTable dt = new DataTable();
                        DataRow dr = null;
                        if (ViewState["EquipmentSubCat"] != null)
                        {
                            dt = (DataTable)ViewState["EquipmentSubCat"];
                            dr = dt.NewRow();
                            //dr["RowNumber"] = dt.Rows.Count + 1;
                            dr["Column1"] = txtEquipmentSubCategory.Text;
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
                            dr = dt.NewRow();
                            //dr["RowNumber"] = 1;
                            dr["Column1"] = txtEquipmentSubCategory.Text;
                            dt.Rows.Add(dr);
                        }



                        GrdAddEquipmentsub.DataSource = dt;
                        GrdAddEquipmentsub.DataBind();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            Label box1 = (Label)GrdAddEquipmentsub.Rows[i].Cells[1].FindControl("lblEquipmentSubCat");

                            box1.Text = dt.Rows[i]["Column1"].ToString();

                            dt.Rows[i]["RowNumber"] = i;
                        }
                        ViewState["EquipmentSubCat"] = dt;
                        txtEquipmentSubCategory.Text = "";
                        btnAddSave.Text = "Add";
                        //RequiredFieldValidator1.Visible = false;
                    }
                    else if (btnAddSave.Text == "Save" && HddAddorEdit.Value == "Add")
                    {
                        DataTable dt = new DataTable();
                        DataRow dr = null;
                        dt = (DataTable)ViewState["EquipmentSubCat"];

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToInt16(dt.Rows[i]["RowNumber"]) == Convert.ToInt16(HddGridIndex.Value))
                            {

                                //Label box1 = (Label)GrdAddEquipmentsub.Rows[Convert.ToInt16(HddGridIndex.Value)].Cells[1].FindControl("lblEquipmentSubCat");

                                dt.Rows[i]["Column1"] = txtEquipmentSubCategory.Text;

                            }

                        }
                        GrdAddEquipmentsub.DataSource = dt;
                        GrdAddEquipmentsub.DataBind();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            Label box1 = (Label)GrdAddEquipmentsub.Rows[i].Cells[1].FindControl("lblEquipmentSubCat");

                            box1.Text = dt.Rows[i]["Column1"].ToString();

                            dt.Rows[i]["RowNumber"] = i;
                        }
                        ViewState["EquipmentSubCat"] = dt;
                    }
                    else if (HddAddorEdit.Value == "Edit")
                    {
                        BALMachinemaster objbalEquipmentSubCatmaster = new BALMachinemaster();

                        objbalEquipmentSubCatmaster.EquipementSubCategoryID = Convert.ToInt64(HddEquipementSubCategoryID.Value);
                        objbalEquipmentSubCatmaster.EquipementSubCategorydesc = txtEquipmentSubCategory.Text;
                        objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(HddEquipmentCategoryID.Value);
                        objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;


                        string b = lclsservice.UpdateEquipmentSubCategory(objbalEquipmentSubCatmaster);
                        BALCorporate ObjSearchEquipment = new BALCorporate();
                        ObjSearchEquipment.SearchText = "";
                        ObjSearchEquipment.Active = "";
                        ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                        ObjSearchEquipment.Mode = "Add";
                        ObjSearchEquipment.Filter = "";
                        List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && a.IsActive == true).ToList();
                        GrdEditEquipmentsub.DataSource = lstMPRMaster;
                        GrdEditEquipmentsub.DataBind();
                    }
                }
                else
                {
                    EventLogger log = new EventLogger(config);
                    string msg = Constant.EquipCorpMapWarMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<CorpEquipmentMapCategory>>", "").Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapWarMessage.Replace("<<CorpEquipmentMapCategory>>", ""), true);
                }

                txtEquipmentSubCategory.Text = "";
                btnAddSave.Text = "Add";
            }
          catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
        }

        protected void lbEquipmentSearchedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                HddEquiCancel.Value = "1";
                ViewState["EquipmentSubCat"] = null;
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                HddEquipementSubCategoryID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                HddEquipmentCategoryID.Value = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                ddlCorporate.ClearSelection();
                if (gvrow.Cells[2].Text == "&nbsp;")
                {
                    ddlCorporate.Items.FindByText("--Select Corporate--").Selected = true;
                }
                else
                {
                    ddlCorporate.SelectedValue = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                }
                txtEquipmentCategory.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = "";
                ObjSearchEquipment.Active = "";
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Add";
                ObjSearchEquipment.Filter = "";
                List<BindEquipement> lstMPRMaster = new List<BindEquipement>();
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                    chkactive.Checked = false;
                    lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && a.IsActive == true).ToList();
                }
                else
                {
                    lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && (a.IsActive == true || a.EquipementSubCategoryID == Convert.ToInt64(HddEquipementSubCategoryID.Value))).ToList();
                    chkactive.Visible = true;
                    chkactive.Checked = true;
                }
                GrdEditEquipmentsub.DataSource = lstMPRMaster;
                GrdEditEquipmentsub.DataBind();
                txtEquipmentSubCategory.Text = "";
                SetAddScreen("", 1);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
        }

        protected void GrdAddEquipmentsub_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label box1 = (Label)GrdAddEquipmentsub.Rows[e.NewEditIndex].Cells[1].FindControl("lblEquipmentSubCat");
                HddGridIndex.Value = e.NewEditIndex.ToString();
                txtEquipmentSubCategory.Text = box1.Text;
                btnAddSave.Text = "Save";
                HddAddorEdit.Value = "Add";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
          
        }


        protected void IbAddEquipmentsubdelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton DelRow = (ImageButton)sender;
                GridViewRow gvRow = (GridViewRow)DelRow.NamingContainer;
                HddEquipmentCategory.Value = txtEquipmentCategory.Text.Trim().Replace("&nbsp;", "");
                int rowID = gvRow.RowIndex;
                if (ViewState["EquipmentSubCat"] != null)
                {
                    DataTable dt = (DataTable)ViewState["EquipmentSubCat"];
                    DataRow drCurrentRow = null;
                    int rowIndex = Convert.ToInt32(gvRow.RowIndex);
                    if (dt.Rows.Count > 1)
                    {
                        dt.Rows.Remove(dt.Rows[rowIndex]);
                        drCurrentRow = dt.NewRow();
                        ViewState["EquipmentSubCat"] = dt;
                        GrdAddEquipmentsub.DataSource = dt;
                        GrdAddEquipmentsub.DataBind();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            Label box1 = (Label)GrdAddEquipmentsub.Rows[i].Cells[1].FindControl("lblEquipmentSubCat");

                            box1.Text = dt.Rows[i]["Column1"].ToString();

                            dt.Rows[i]["RowNumber"] = i;
                        }
                        ViewState["EquipmentSubCat"] = dt;
                    }
                    else
                    {
                        GrdAddEquipmentsub.DataSource = null;
                        GrdAddEquipmentsub.DataBind();
                        ViewState["EquipmentSubCat"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }
        }

        protected void GrdEditEquipmentsub_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label box1 = (Label)GrdEditEquipmentsub.Rows[e.NewEditIndex].Cells[2].FindControl("lblEquipmentSubCatID");
                Label box2 = (Label)GrdEditEquipmentsub.Rows[e.NewEditIndex].Cells[1].FindControl("lblEquipmentSubCat");
                HddGridIndex.Value = e.NewEditIndex.ToString();
                HddEquipementSubCategoryID.Value = box1.Text;
                txtEquipmentSubCategory.Text = box2.Text;
                btnAddSave.Text = "Save";
                HddAddorEdit.Value = "Edit";
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }

        }

        protected void GrdEditEquipmentsub_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {
                Label box1 = (Label)GrdEditEquipmentsub.Rows[e.RowIndex].Cells[2].FindControl("lblEquipmentSubCatID");
                HddEquipementSubCategoryID.Value = box1.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }

        }



        protected void btnImgDeletePopUp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                BALMachinemaster objbalEquipmentSubCatmaster = new BALMachinemaster();
                objbalEquipmentSubCatmaster.EquipementSubCategoryID = Convert.ToInt64(HddEquipementSubCategoryID.Value);
                objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;
                string b = lclsservice.DeleteEquipSubCategoryMaster(objbalEquipmentSubCatmaster);
                BALCorporate ObjSearchEquipment = new BALCorporate();
                ObjSearchEquipment.SearchText = "";
                ObjSearchEquipment.Active = "";
                ObjSearchEquipment.LoggedinBy = defaultPage.UserId;
                ObjSearchEquipment.Mode = "Add";
                ObjSearchEquipment.Filter = "";
                List<BindEquipement> lstMPRMaster = lclsservice.BindEquipement(ObjSearchEquipment).Where(a => a.EquipmentCategoryID == Convert.ToInt64(HddEquipmentCategoryID.Value) && a.IsActive == true).ToList();
                GrdEditEquipmentsub.DataSource = lstMPRMaster;
                GrdEditEquipmentsub.DataBind();
                SearchGrid(txtEquipmentSearch.Text);
                if (b == "Deleted Successfully")
                {
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgdelte.Replace("<<CorpEquipmentMapCategory>>", HddEquipmentCategory.Value));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapDeleteMessage.Replace("<<CorpEquipmentMapCategory>>", HddEquipmentCategory.Value), true);
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "ShowError('" + ex.Message + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message.ToString()), true);
            }

        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
                CheckBox chkactive = (CheckBox)row.FindControl("chkActive");

                InventoryServiceClient lclsService = new InventoryServiceClient();

                BALMachinemaster objbalEquipmentSubCatmaster = new BALMachinemaster();

                objbalEquipmentSubCatmaster.EquipementSubCategoryID = Convert.ToInt64(row.Cells[1].Text);
                objbalEquipmentSubCatmaster.EquipementCategoryID = Convert.ToInt64(row.Cells[3].Text);
                objbalEquipmentSubCatmaster.EquipementSubCategorydesc = row.Cells[6].Text;
                objbalEquipmentSubCatmaster.LastModifiedBy = defaultPage.UserId;
                if (chkactive.Checked == true)
                {
                    lclsService.UpdateEquipmentSubCategory(objbalEquipmentSubCatmaster);
                }
                else
                {
                    lclsService.DeleteEquipSubCategoryMaster(objbalEquipmentSubCatmaster);
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgdelte.Replace("<<CorpEquipmentMapCategory>>", row.Cells[5].Text));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapDeleteMessage.Replace("<<CorpEquipmentMapCategory>>", row.Cells[5].Text), true);
                }
                BindGrid();
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
          
        }

        protected void btndeleteimg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                HddEquipementSubCategoryID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                HddEquipmentCategoryID.Value = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                HddEquipmentCategory.Value = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }

        }

        protected void GrdEquipmentSearch_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.EquipCorpMapErrorMessage.Replace("<<CorpEquipmentMapCategory>>", ex.Message), true);
            }
        }
    }

}