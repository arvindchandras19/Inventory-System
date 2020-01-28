#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Inventory.Class;
using Inventory.Inventoryserref;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   <<Facility>>
'' Type      :   C# File
'' Description  :<<To add,update the Facility Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				   Sairam.P		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class Facility : System.Web.UI.Page
    {
        #region Declartions
        Page_Controls defaultPage = new Page_Controls();
        private string _sessionPDFFileName;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.FacilityDeleteMessage.Replace("ShowdelPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            defaultPage = (Page_Controls)Session["Permission"];
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnPrint);
            scriptManager.RegisterPostBackControl(this.btnDetailsPrint);
            try
            {
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    BindFacility();
                    BindUserRoles();
                    GetState();
                    GetFacility();
                    GetCorporate();
                    GetBillState();
                    if (defaultPage != null)
                    {
                        if (defaultPage.FacilityPage_Edit == false && defaultPage.FacilityPage_View == true)
                        {
                            //SaveCancel.Visible = false;
                            btnAdd.Visible = false;
                            btnSave.Visible = false;

                        }

                        if (defaultPage.FacilityPage_Edit == false && defaultPage.FacilityPage_View == false)
                        {
                            MainBoby.Visible = false;
                            //grdFacility.Visible = false;
                            //btnSave.Visible = false;
                            //pnlSearch.Visible = false;                                                        
                            User_Permission_Message.Visible = true;
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);

            }
        }
        private void RequiredFieldValidatorMessage()
        {
            string req = Constant.RequiredFieldValidator;
            reqfieldFacilityShortName.ErrorMessage = req;
            reqfieldFacilityDescription.ErrorMessage = req;
            Req_ID.ErrorMessage = req;
            //revEmail.ErrorMessage = Constant.RequiredExpressionEmail;
            //revadminEmail.ErrorMessage = Constant.RequiredExpressionEmail;
            //cusfacshortname.ErrorMessage = Constant.Facility;
            //cusvalfacdesc.ErrorMessage = Constant.Facility;
            //cusvalcorporate.ErrorMessage = Constant.Facility;

        }
        protected void ClearValues()
        {
            try
            {
                txtFacilityDescription.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtCity.Text = "";
                ddlState.SelectedIndex = 0;
                txtZipcode.Text = "";
                txtPhone.Text = "";
                txtFax.Text = "";
                ddlCorporate.SelectedIndex = 0;
                txtGPAccountCode.Text = "";
                txtEMRCode.Text = "";
                txtBillAddress1.Text = "";
                txtBillAddress2.Text = "";
                txtBillCity.Text = "";
                ddlBillState.SelectedIndex = 0;
                txtBillZip.Text = "";
                txtBillPhone.Text = "";
                txtBillFax.Text = "";
                //txttechperson.Text = "";
                //txttechph.Text = "";
                //txttechemail.Text = "";
                //txtadminperson.Text = "";
                //txtadminph.Text = "";
                //txtadminemail.Text = "";
                txtPatientCensus.Text = "";
                txtEmployeeCensus.Text = "";
                txttxperweek.Text = "";
                hiddenFacilityID.Value = "0";
                txtFacilityShortName.Text = "";
                txtZipcode1.Text = "";
                txtBillZip1.Text = "";
                hdnfield.Value = "0";
                txtxtn.Text = "";
                txtxtnbill.Text = "";
            }
           catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }


        }
        public void GetFacility()
        {
            try
            {
                List<GetFacility> lstfac = lclsservice.GetFacility().ToList();
                ddlCopyFromExistingFacility.DataSource = lstfac;
                ddlCopyFromExistingFacility.DataValueField = "FacilityID";
                ddlCopyFromExistingFacility.DataTextField = "FacilityDescription";
                ddlCopyFromExistingFacility.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "-1";
                lst.Text = "Select";
                ddlCopyFromExistingFacility.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }
        public void GetState()
        {
            try
            {
                List<GetState> lstpatin = lclsservice.GetState().ToList();
                ddlState.DataSource = lstpatin;
                ddlState.DataValueField = "Invenkey";
                ddlState.DataTextField = "InvenValue";
                ddlState.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlState.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
            
        }
        public void GetBillState()
        {
            try
            {
                List<GetState> lstpatin = lclsservice.GetState().ToList();
                ddlBillState.DataSource = lstpatin;
                ddlBillState.DataValueField = "Invenkey";
                ddlBillState.DataTextField = "InvenValue";
                ddlBillState.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlBillState.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        public void GetCorporate()
        {
            try
            {
                ddlCorporate.DataSource = lclsservice.GetCorporateMaster().ToList();
                ddlCorporate.DataValueField = "CorporateID";
                ddlCorporate.DataTextField = "CorporateName";
                ddlCorporate.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlCorporate.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BALFacility lclfacility = new BALFacility();
                string lstrmsg = string.Empty;
                string ErrMsg = string.Empty;
                lclfacility.FacilityID = Convert.ToInt64(hiddenFacilityID.Value);
                lclfacility.FacilityDescription = txtFacilityDescription.Text;
                lclfacility.FacilityShortName = txtFacilityShortName.Text;
                HddFacilityCode.Value = lclfacility.FacilityShortName;
                //if (txtDialysisSoftCode.Text == "")
                //{
                //    lclfacility.DialysisoftFacilityCode = 0;
                //}else
                //{
                //    lclfacility.DialysisoftFacilityCode = Convert.ToInt64(txtDialysisSoftCode.Text);
                //}

                lclfacility.Address1 = txtAddress1.Text;
                lclfacility.Address2 = txtAddress2.Text;
                lclfacility.City = txtCity.Text;
                lclfacility.State = Convert.ToInt64(ddlState.SelectedValue);
                if (txtZipcode1.Text == "")
                    lclfacility.Zipcode = txtZipcode.Text;
                else
                    lclfacility.Zipcode = txtZipcode.Text + "-" + txtZipcode1.Text;
                lclfacility.Phone = txtPhone.Text;
                lclfacility.Fax = txtFax.Text;
                lclfacility.BillAddress1 = txtBillAddress1.Text;
                lclfacility.BillAddress2 = txtBillAddress2.Text;
                lclfacility.BillCity = txtBillCity.Text;
                lclfacility.BillState = Convert.ToInt64(ddlBillState.SelectedValue);
                if (txtBillZip1.Text == "")
                    lclfacility.BillZipCode = txtBillZip.Text;
                else
                    lclfacility.BillZipCode = txtBillZip.Text + "-" + txtBillZip1.Text;
                lclfacility.BillPhone = txtBillPhone.Text;
                lclfacility.BillFax = txtBillFax.Text;
                if (txtxtn.Text == "")
                {
                    lclfacility.Xtn = 0;
                }
                else
                {
                    lclfacility.Xtn = Convert.ToInt64(txtxtn.Text);
                }
                if (txtxtnbill.Text == "")
                {
                    lclfacility.BillXtn = 0;
                }
                else
                {
                    lclfacility.BillXtn = Convert.ToInt64(txtxtnbill.Text);

                }
                lclfacility.FCorporate = Convert.ToInt64(ddlCorporate.SelectedValue);
                lclfacility.GPAccountCode = txtGPAccountCode.Text;
                HddFacilityAcctCode.Value = lclfacility.GPAccountCode;
                lclfacility.EMRCode = txtEMRCode.Text;
                //lclfacility.TechPerson = txttechperson.Text;
                //lclfacility.TechPhone = txttechph.Text;
                //lclfacility.TechEmail = txttechemail.Text;
                //lclfacility.AdminPerson = txtadminperson.Text;
                //lclfacility.AdminPhone = txtadminph.Text;
                //lclfacility.AdminEmail = txtadminemail.Text;
                if (txtPatientCensus.Text == "")
                    lclfacility.PatientCensus = 0;
                else
                    lclfacility.PatientCensus = Convert.ToInt64(txtPatientCensus.Text);

                if (txtEmployeeCensus.Text == "")
                    lclfacility.EmployeeCensus = 0;
                else
                    lclfacility.EmployeeCensus = Convert.ToInt64(txtEmployeeCensus.Text);


                if (txttxperweek.Text == "")
                    lclfacility.TxWeek = 0;
                else
                    lclfacility.TxWeek = Convert.ToInt64(txttxperweek.Text);
                lclfacility.CreatedBy = defaultPage.UserId;
                lclfacility.CreatedOn = DateTime.Now;
                lclfacility.LastModifiededBy = defaultPage.UserId;
                lclfacility.LastModifiededOn = DateTime.Now;

                lclfacility.CopyFacilityID = Convert.ToInt64(ddlCopyFromExistingFacility.SelectedValue);

                lclfacility.SearchText = "";
                lclfacility.Active = "";
                lclfacility.LogginBy = defaultPage.UserId;
                lclfacility.Filter = "";
                if (chkactive.Checked == true)
                {
                    lclfacility.IsActive = true;
                }
                else
                {
                    lclfacility.IsActive = false;
                }

                List<BindFacility> lstcol = lclsservice.BindFacility(lclfacility).Where(a => a.FacilityShortName == lclfacility.FacilityShortName).ToList();

                if (lstcol.Count > 0)
                {
                    if (lclfacility.FacilityID == 0)
                    {
                        ErrMsg = "FC";
                    }
                    else
                    {
                        if (lclfacility.FacilityID != lstcol[0].FacilityID)
                        {
                            ErrMsg = "FC";
                        }
                    }


                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityMandMessageWarMessage.Replace("<<FacilityDescription>>", "code(" + lclfacility.FacilityShortName + ")"), true);
                }

                if (ErrMsg == "")
                {
                    if (txtGPAccountCode.Text != "")
                    {
                        List<ValidFaciliityAccountCode> lstcolfac = lclsservice.ValidFaciliityAccountCode(txtGPAccountCode.Text).ToList();
                        if (lstcolfac.Count > 0)
                        {
                            if (lclfacility.FacilityID == 0)
                            {
                                ErrMsg = "FAC";
                            }
                            else
                            {
                                if (lclfacility.FacilityID != lstcolfac[0].FacilityID)
                                {
                                    ErrMsg = "FAC";
                                }
                            }

                        }
                    }
                }

                if (ErrMsg == "")
                {
                    lstrmsg = lclsservice.InsertFacility(lclfacility, '1');
                }
                EventLogger log = new EventLogger(config);
                if (lstrmsg == "Saved Successfully")
                {
                    string msg = Constant.FacilitySaveMessage.Replace("ShowPopup('", "").Replace("<<FacilityDescription>>", txtFacilityDescription.Text).Replace("');", "");
                    log.LogInformation(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilitySaveMessage.Replace("<<FacilityDescription>>", txtFacilityDescription.Text), true);
                    //// ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved Successfully')", true);
                    //Functions objfun = new Functions();
                    //objfun.MessageDialog(this, "Saved Successfully");
                    BindFacility();
                    ShowSearch();
                    //txtSearchFacility.Text = "";
                }
                else
                {
                    if (ErrMsg == "FC")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ValidatefacilityFC()", true);
                        log.LogWarning("Facility Code is already exists");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "Validatefacility(" + Constant.FacilityMandMessageWarMessage.Replace("<<FacilityDescription>>", "code(" + lclfacility.FacilityShortName + ")") + ");", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ValidatefacilityFAC()", true);
                        log.LogWarning("Facility Accounting Code is already exists");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "Validatefacility(" + Constant.FacilityMandMessageWarMessage.Replace("<<FacilityDescription>>", "Accounting code(" + lclfacility.GPAccountCode + ")") + ");", true);
                    }

                }


            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }
        public void BindFacility()
        {
            try
            {
                BALFacility lclfacility = new BALFacility();
                if (txtFacility.Text == "")
                    lclfacility.FacilityShortName = null;
                else
                    lclfacility.FacilityShortName = txtFacility.Text;
                if (txtfacilitydescr.Text == "")
                    lclfacility.FacilityDescription = null;
                else
                    lclfacility.FacilityDescription = txtfacilitydescr.Text;
                lclfacility.Active = rdbstatus.SelectedValue;
                lclfacility.LogginBy = defaultPage.UserId;
                lclfacility.Filter = "";
                List<BindFacility> lstcol = lclsservice.BindFacility(lclfacility).ToList();
                //lblrowcount.Text = "No of records : " + lstcol.Count.ToString();
                grdFacility.DataSource = lstcol;
                grdFacility.DataBind();
                //BALFacility lclfacility = new BALFacility();
                //lclfacility.SearchText = "";
                //lclfacility.Active = "";
                //lclfacility.LogginBy = defaultPage.UserId;
                //lclfacility.Filter = "";
                //List<BindFacility> lstcol = lclsservice.BindFacility(lclfacility).ToList();
                //grdFacility.DataSource = lstcol;
                //grdFacility.DataBind();

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }
        public void BindUserRoleFacility()
        {
            try
            {
                foreach (ListItem lst in drpUserRole.Items)
                {
                    if (lst.Selected && drpUserRole.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                List<GetUserRoleForFacility> lstcol = lclsservice.GetUserRoleForFacility(Convert.ToInt64(hiddenFacilityID.Value), FinalString).ToList();
                grdFacilityUserRole.DataSource = lstcol;
                grdFacilityUserRole.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hdnfield.Value = gvrow.Cells[2].Text;
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

            //    string lstrMessage = lclsservice.DeleteFacility(Convert.ToInt64(hdnfield.Value), Convert.ToInt64(Session["User"]));
            //    if (lstrMessage=="Deleted Successfully")
            //    {
            //        Functions objfun = new Functions();
            //        objfun.MessageDialog(this, "Deleted Successfully");
            //    }

            //    BindFacility();
            //}
            //catch (Exception ex)
            //{
            //    lblmsg.Text = ex.Message;
            //}

        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearValues();
                ShowAddNew();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                FacilityUserRole.Style.Add("display", "block");
                btnPrint.Visible = false;
                btnDetailsPrint.Visible = true;
                //btnDetailsPrint.Visible = true;
                //lblUser.Text = "Edit User";
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hiddenFacilityID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                //ViewState["FacilityID"] = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                txtFacilityShortName.Text = gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                txtFacilityDescription.Text = gvrow.Cells[3].Text.Trim().Replace("&nbsp;", "");
                txtAddress1.Text = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                txtAddress2.Text = gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "");
                txtCity.Text = gvrow.Cells[6].Text.Trim().Replace("&nbsp;", "");
                ddlState.ClearSelection();
                if (gvrow.Cells[7].Text == "&nbsp;")
                {
                    ddlState.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlState.Items.FindByText(gvrow.Cells[7].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                }
                //ddlState.SelectedItem.Text = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                txtZipcode.Text = gvrow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                if (txtZipcode.Text != "")
                {
                    string[] a = txtZipcode.Text.Split('-');
                    txtZipcode.Text = a[0].ToString();
                    if (a.Length >= 2)
                        txtZipcode1.Text = a[1].ToString();
                }

                txtPhone.Text = gvrow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                txtFax.Text = gvrow.Cells[10].Text.Trim().Replace("&nbsp;", "");
                txtBillAddress1.Text = gvrow.Cells[11].Text.Trim().Replace("&nbsp;", "");
                txtBillAddress2.Text = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                txtBillCity.Text = gvrow.Cells[13].Text.Trim().Replace("&nbsp;", "");
                ddlBillState.ClearSelection();
                if (gvrow.Cells[14].Text == "&nbsp;")
                {
                    ddlBillState.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlBillState.Items.FindByText(gvrow.Cells[14].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                }

                txtBillZip.Text = gvrow.Cells[15].Text.Trim().Replace("&nbsp;", "");
                if (txtBillZip.Text != "")
                {
                    string[] b = txtBillZip.Text.Split('-');
                    txtBillZip.Text = b[0].ToString();
                    if (b.Length >= 2)
                    {
                        txtBillZip1.Text = b[1].ToString();
                    }                        

                }
                txtBillPhone.Text = gvrow.Cells[16].Text.Trim().Replace("&nbsp;", "");
                txtBillFax.Text = gvrow.Cells[17].Text.Trim().Replace("&nbsp;", "");
                ddlCorporate.ClearSelection();
                if (gvrow.Cells[18].Text == "&nbsp;")
                {
                    ddlCorporate.Items.FindByText("Select").Selected = true;
                }
                else
                {
                    ddlCorporate.Items.FindByText(gvrow.Cells[18].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                }
                //ddlZone.SelectedItem.Text = gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "");
                txtGPAccountCode.Text = gvrow.Cells[19].Text.Trim().Replace("&nbsp;", "");
                txtEMRCode.Text = gvrow.Cells[20].Text.Trim().Replace("&nbsp;", "");
                //txttechperson.Text = gvrow.Cells[21].Text.Trim().Replace("&nbsp;", "");
                //txttechph.Text = gvrow.Cells[22].Text.Trim().Replace("&nbsp;", "");
                //txttechemail.Text = gvrow.Cells[23].Text.Trim().Replace("&nbsp;", "");
                //txtadminperson.Text = gvrow.Cells[24].Text.Trim().Replace("&nbsp;", "");
                //txtadminph.Text = gvrow.Cells[25].Text.Trim().Replace("&nbsp;", "");
                //txtadminemail.Text = gvrow.Cells[26].Text.Trim().Replace("&nbsp;", "");
                if (Convert.ToInt64(string.IsNullOrEmpty(gvrow.Cells[27].Text.Replace("&nbsp;", "")) == true ? "0" : gvrow.Cells[27].Text.Replace("&nbsp;", "")) == 0)
                    txtPatientCensus.Text = "";
                else
                    txtPatientCensus.Text = gvrow.Cells[27].Text.Trim().Replace("&nbsp;", "");

                if (Convert.ToInt64(string.IsNullOrEmpty(gvrow.Cells[32].Text.Replace("&nbsp;", "")) == true ? "0" : gvrow.Cells[32].Text.Replace("&nbsp;", "")) == 0)
                    txtEmployeeCensus.Text = "";
                else
                    txtEmployeeCensus.Text = gvrow.Cells[32].Text.Trim().Replace("&nbsp;", "");

                if (Convert.ToInt64(string.IsNullOrEmpty(gvrow.Cells[28].Text.Replace("&nbsp;", "")) == true ? "0" : gvrow.Cells[28].Text.Replace("&nbsp;", "")) == 0)
                    txttxperweek.Text = "";
                else
                    txttxperweek.Text = gvrow.Cells[28].Text.Trim().Replace("&nbsp;", "");

                // txttxperweek.Text = gvrow.Cells[28].Text.Trim().Replace("&nbsp;", "");
                if (Convert.ToInt64(string.IsNullOrEmpty(gvrow.Cells[29].Text.Replace("&nbsp;", "")) == true ? "0" : gvrow.Cells[29].Text.Replace("&nbsp;", "")) == 0)
                {
                    txtxtn.Text = "";
                }
                else
                    txtxtn.Text = gvrow.Cells[29].Text.Trim().Replace("&nbsp;", "");
                if (Convert.ToInt64(string.IsNullOrEmpty(gvrow.Cells[30].Text.Replace("&nbsp;", "")) == true ? "0" : gvrow.Cells[30].Text.Replace("&nbsp;", "")) == 0)
                {
                    txtxtnbill.Text = "";
                }
                else
                    txtxtnbill.Text = gvrow.Cells[30].Text.Trim().Replace("&nbsp;", "");
                Label lblActive = (Label)gvrow.FindControl("lblActive");
                if (lblActive.Text == "Yes")
                {
                    chkactive.Visible = false;
                }
                else
                {
                    chkactive.Visible = true;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
                BindUserRoles();
                BindUserRoleFacility();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ClearValues();
                ShowAddNew();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }

        public void ShowAddNew()
        {
            try
            {
                chkactive.Visible = false;
                btnPrint.Visible = false;
                btnDetailsPrint.Visible = false;
                btnSearchFacility.Visible = false;
                btnAdd.Visible = false;
                lblseroutHeader.Visible = false;
                btnSave.Visible = true;
                btncancelsave.Visible = true;
                btnCancel.Visible = false;
                FacilityUserRole.Style.Add("display", "none");
                div_ADDContent.Style.Add("display", "block");
                DivSearch.Style.Add("display", "none");
                div_Grid.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                rdbstatus.SelectedValue = "1";
                txtFacility.Text = "";
                txtfacilitydescr.Text = "";
                ShowSearch();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }

        public void ShowSearch()
        {
            try
            {
                btnPrint.Visible = true;
                btnDetailsPrint.Visible = false;
                btnSearchFacility.Visible = true;
                btnAdd.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = true;
                lblseroutHeader.Visible = true;
                btncancelsave.Visible = false;
                div_ADDContent.Style.Add("display", "none");
                DivSearch.Style.Add("display", "block");
                div_Grid.Style.Add("display", "block");
                BindFacility();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }

        protected void grdFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdFacility.PageIndex = e.NewPageIndex;
                BindFacility();

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                lblmsg.Text = ex.Message;
            }

        }
        protected void chkactive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
                CheckBox chkactive = (CheckBox)row.FindControl("chkactive");

                if (chkactive.Checked == true)
                {
                    lclsservice.DeleteFacility(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(defaultPage.UserId), true);
                }
                else
                {
                    lclsservice.DeleteFacility(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(defaultPage.UserId), false);
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgwrn.Replace("<<FacilityDescription>>", row.Cells[2].Text.Trim()));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityDeleteMessage.Replace("<<FacilityDescription>>", row.Cells[2].Text.Trim()), true);
                }

                BindFacility();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
            }
          
        }

        protected void btnSearchFacility_Click(object sender, EventArgs e)
        {
            BindFacility();
        }
        #endregion

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                BALFacility lclfacility = new BALFacility();
                if (txtFacility.Text == "")
                    lclfacility.FacilityShortName = null;
                else
                    lclfacility.FacilityShortName = txtFacility.Text;
                if (txtfacilitydescr.Text == "")
                    lclfacility.FacilityDescription = null;
                else
                    lclfacility.FacilityDescription = txtfacilitydescr.Text;
                lclfacility.Active = rdbstatus.SelectedValue;
                lclfacility.LogginBy = defaultPage.UserId;
                lclfacility.Filter = "";
                List<BindFacility> lstcol = lclsservice.BindFacility(lclfacility).ToList();
                rvFacilityreport.ProcessingMode = ProcessingMode.Local;
                rvFacilityreport.LocalReport.ReportPath = Server.MapPath("~/Reports/FacilitySummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("FacilityReportDS", lstcol);
                rvFacilityreport.LocalReport.DataSources.Clear();
                rvFacilityreport.LocalReport.DataSources.Add(datasource);
                rvFacilityreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvFacilityreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "FacilitySummary" + guid + ".pdf";
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

                //List<object> llstresult = PrintFacility();
                //byte[] bytes = (byte[])llstresult[0];
                //Guid guid = Guid.NewGuid();
                //string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                //_sessionPDFFileName = "Facility " + guid + ".pdf";
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<FacilityDescription>>", ex.Message), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintFacility()
        {
            List<object> llstarg = new List<object>();
            BALFacility lclfacility = new BALFacility();
            if (txtFacility.Text == "")
                lclfacility.FacilityShortName = null;
            else
                lclfacility.FacilityShortName = txtFacility.Text;
            if (txtfacilitydescr.Text == "")
                lclfacility.FacilityDescription = null;
            else
                lclfacility.FacilityDescription = txtfacilitydescr.Text;
            lclfacility.Active = rdbstatus.SelectedValue;
            lclfacility.LogginBy = defaultPage.UserId;
            lclfacility.Filter = "";
            List<BindFacility> llstreview = lclsservice.BindFacility(lclfacility).ToList();
            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvFacilityreport.ProcessingMode = ProcessingMode.Local;
            rvFacilityreport.LocalReport.ReportPath = Server.MapPath("~/Reports/FacilitySummary.rdlc");
            ReportDataSource datasource = new ReportDataSource("FacilityReportDS", llstreview);
            rvFacilityreport.LocalReport.DataSources.Clear();
            rvFacilityreport.LocalReport.DataSources.Add(datasource);
            rvFacilityreport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvFacilityreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }
        public void BindUserRoles()
        {
            try
            {
                List<BindRolesForFacility> lstRoles = new List<BindRolesForFacility>();
                lstRoles = lclsservice.BindRolesForFacility(Convert.ToInt64(hiddenFacilityID.Value)).ToList();
                drpUserRole.DataSource = lstRoles;
                drpUserRole.DataTextField = "UserRole";
                drpUserRole.DataValueField = "UserRoleID";
                drpUserRole.DataBind();

                foreach (ListItem lst in drpUserRole.Items)
                {
                    List<BindRolesForFacility> lstdrp = lclsservice.BindRolesForFacility(Convert.ToInt64(hiddenFacilityID.Value)).Where(a => a.UserRoleID == Convert.ToInt64(lst.Value) && a.UserType == "System").ToList();
                    if (lstdrp.Count > 0)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message), true);
            }
        }
        protected void drpUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem lst in drpUserRole.Items)
                {
                    if (lst.Selected && drpUserRole.SelectedValue != "All")
                    {
                        SB.Append(lst.Value + ',');
                    }
                }
                if (SB.Length > 0)
                    FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                List<GetUserRoleForFacility> lstcol = lclsservice.GetUserRoleForFacility(Convert.ToInt64(hiddenFacilityID.Value), FinalString).ToList();

                grdFacilityUserRole.DataSource = lstcol;
                grdFacilityUserRole.DataBind();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "editfacility();", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message), true);
            }
         
        }

        protected void btnDetailsPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = PrintFacilityDetails();
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "Facility Details" + guid + ".pdf";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message), true);
            }
           
        }

        public List<object> PrintFacilityDetails()
        {
            List<object> llstarg = new List<object>();
            BALFacility lclfacility = new BALFacility();
            lclfacility.FacilityID = Convert.ToInt64(hiddenFacilityID.Value);
            lclfacility.LogginBy = defaultPage.UserId;
            lclfacility.Filter = "";
            List<BindFacilityDetailsReport> llstreview = lclsservice.BindFacilityDetailsReport(lclfacility).ToList();
            //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
            rvFacilityDetaislReport.ProcessingMode = ProcessingMode.Local;
            rvFacilityDetaislReport.LocalReport.ReportPath = Server.MapPath("~/Reports/FacilityDetailsReport.rdlc");
            ReportDataSource datasource = new ReportDataSource("FacilityDetailsDS", llstreview);
            rvFacilityDetaislReport.LocalReport.DataSources.Clear();
            rvFacilityDetaislReport.LocalReport.DataSources.Add(datasource);
            rvFacilityDetaislReport.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = null;
            bytes = rvFacilityDetaislReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            llstarg.Insert(0, bytes);
            return llstarg;
        }

        protected void lbldelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hiddenFacilityID.Value = gvrow.Cells[1].Text;
                hdnfield.Value = gvrow.Cells[2].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message), true);
            }
        }
        protected void btndeletepop_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryServiceClient lclsService = new InventoryServiceClient();
                string lstrMessage = lclsService.DeleteFacility(Convert.ToInt64(hiddenFacilityID.Value), defaultPage.UserId, false);
                if (lstrMessage == "Deleted Successfully")
                {
                    BindFacility();
                    EventLogger log = new EventLogger(config);
                    log.LogInformation(msgwrn.Replace("<<FacilityDescription>>", hdnfield.Value));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityDeleteMessage.Replace("<<FacilityDescription>>", hdnfield.Value), true);
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message.ToString()), true);
            }
        }

        protected void btncancelsave_Click(object sender, EventArgs e)
        {
            ShowSearch();
        }

        protected void grdFacility_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        chkactive.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.FacilityErrorMessage.Replace("<<Facility>>", ex.Message), true);
            }
        }
    }

}
