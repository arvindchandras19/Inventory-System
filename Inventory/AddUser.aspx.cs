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
    public partial class AddUser : System.Web.UI.Page
    {
        InventoryServiceClient lclsService = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALUser lclsUser = new BALUser();
        string userrole = string.Empty;
        private string _sessionPDFFileName;
        string ErrorList = string.Empty;
        string FinalString = "";
        StringBuilder SB = new StringBuilder();
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        string msgwrn = Constant.AddUserSaveMessage.Replace("ShowPopup('", "").Replace("');", "");
        string msgext = Constant.checkemailexists.Replace("ShowwarningLookupPopup('", "").Replace("');", "");
        string msgdete = Constant.AddUserDeleteMessage.Replace("ShowdelPopup('", "").Replace("');", "");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnPrint);
                if (defaultPage != null)
                {
                    HddRoleID.Value = defaultPage.RoleID.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ShowPassword();", true);
                }
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    //BindExist("Add");
                    BindUserRole();
                    BindCorporateMaster();
                    BindSearchFacility();
                    BindRole();
                    SearchGrid();
                    //BindBudget();
                    if (defaultPage != null)
                    {
                        if (defaultPage.AddUserPage_Edit == false && defaultPage.AddUserPage_View == true)
                        {
                            btnSave.Visible = false;

                        }
                        if (defaultPage.AddUserPage_Edit == false && defaultPage.AddUserPage_View == false)
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
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
          
        }
        private void RequiredFieldValidatorMessage()
        {
            try
            {
                string req = Constant.RequiredFieldValidator;
                reqfieldFirstname.ErrorMessage = req;
                reqfieldLastname.ErrorMessage = req;
                cmpPwd.ErrorMessage = Constant.CompareValidator;
                //compphone.ErrorMessage = Constant.RequiredExpressionPhone;
                revEmail.ErrorMessage = Constant.RequiredExpressionEmail;
                reqfieldEmail.ErrorMessage = req;
                reqfieldUsername.ErrorMessage = req;
                Reqfieldcophone.ErrorMessage = req;
                reqfieldPwd.ErrorMessage = req;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }
        private void BindCorporateMaster()
        {
            try
            {
                List<BALUser> lstCorpSearch = new List<BALUser>();

                // Bind Add User Corporate
                lstCorpSearch = lclsService.GetCorporateMaster().ToList();
                drpcorp.DataSource = lstCorpSearch;
                drpcorp.DataTextField = "CorporateName";
                drpcorp.DataValueField = "CorporateID";
                drpcorp.DataBind();

                // Bind Search Corporate
                drpCorpSearch.DataSource = lstCorpSearch;
                drpCorpSearch.DataTextField = "CorporateName";
                drpCorpSearch.DataValueField = "CorporateID";
                drpCorpSearch.DataBind();

                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpcorp.Items.Insert(0, lst);
                drpcorp.SelectedIndex = 0;

                foreach (ListItem lst1 in drpCorpSearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }

            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }

        }

        private void BindFacility(string mode)
        {
            try
            {
                List<GetCorporateFacility> lstfacility = new List<GetCorporateFacility>();
                if (mode == "Add")
                {
                    lstfacility = lclsService.GetCorporateFacility(Convert.ToInt64(drpcorp.SelectedValue)).Where(a => a.IsActive == true).ToList();
                }
                else
                {
                    lstfacility = lclsService.GetCorporateFacility(Convert.ToInt64(drpcorp.SelectedValue)).ToList();
                }

                // Bind Add User Corporate
                drpFacilitys.DataSource = lstfacility;
                drpFacilitys.DataTextField = "FacilityDescription";
                drpFacilitys.DataValueField = "FacilityID";
                drpFacilitys.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
           
        }

        public void BindSearchFacility()
        {
            try
            {
                // Bind Search Corporate
                if (drpCorpSearch.SelectedValue != "")
                {
                    foreach (ListItem lst in drpCorpSearch.Items)
                    {
                        if (lst.Selected && drpCorpSearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    drpFacilitySearch.DataSource = lclsService.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                    drpFacilitySearch.DataTextField = "FacilityDescription";
                    drpFacilitySearch.DataValueField = "FacilityID";
                    drpFacilitySearch.DataBind();
                    foreach (ListItem lst in drpFacilitySearch.Items)
                    {
                        lst.Attributes.Add("class", "selected");
                        lst.Selected = true;
                    }
                }
                //else
                //{
                //    drpFacilitySearch.DataSource = 0;
                //    drpFacilitySearch.DataTextField = null;
                //    drpFacilitySearch.DataValueField = null;
                //    drpFacilitySearch.DataBind();
                //}
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }


        protected void drpCorpSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSearchFacility();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        public void BindRole()
        {
            try
            {
                List<BindPermissionforUser> lstrole = lclsService.BindPermissionforUser().ToList();

                drpRoleSearch.DataSource = lstrole;
                drpRoleSearch.DataTextField = "UserRole";
                drpRoleSearch.DataValueField = "UserRoleID";
                drpRoleSearch.DataBind();

                foreach (ListItem lst1 in drpRoleSearch.Items)
                {
                    lst1.Attributes.Add("class", "selected");
                    lst1.Selected = true;
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }

        }



        //private void BindExist(string Mode)
        //{
        //    InventoryServiceClient lclsservice = new InventoryServiceClient();
        //    List<BindExistuser> lstuserlist = new List<BindExistuser>();
        //    if (Mode == "Add")
        //    {
        //        lstuserlist = lclsService.BindExistuser().Where(a => a.IsActive == true).ToList();
        //    }
        //    else
        //    {
        //        lstuserlist = lclsService.BindExistuser().ToList();
        //    }
        //    drpexistuser.DataSource = lstuserlist;
        //    drpexistuser.DataTextField = "UserName";
        //    drpexistuser.DataValueField = "UserID";
        //    drpexistuser.DataBind();
        //    ListItem lst = new ListItem();
        //    lst.Value = "0";
        //    lst.Text = "--Select--";
        //    drpexistuser.Items.Insert(0, lst);
        //    drpexistuser.SelectedIndex = 0;
        //}
        private void BindUserRole()
        {
            try
            {
                drpUserrole.DataSource = lclsService.GetUserRole().ToList();
                drpUserrole.DataTextField = "UserRole";
                drpUserrole.DataValueField = "UserRoleID";
                drpUserrole.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpUserrole.Items.Insert(0, lst);
                drpUserrole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        public void BindfacilityRole()
        {
            try
            {
                List<GetUserRoleandFacility> lstrolefacility = lclsService.GetRoleandFacility(Convert.ToInt64(HddUserID.Value)).ToList();
                //List<GetUserRoleandFacility> lstrolefacility = lclsService.GetRoleandFacility(Convert.ToInt64(drpexistuser.SelectedValue)).ToList();
                grduserrole.DataSource = lstrolefacility;
                grduserrole.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        //private void BindBudget()
        //{
        //    drpcurrency.DataSource = lclsService.ddlCurrency().ToList();
        //    drpcurrency.DataTextField = "CurrencyName";
        //    drpcurrency.DataValueField = "CurrencyID";
        //    drpcurrency.DataBind();
        //    ListItem lst = new ListItem();
        //    lst.Value = "0";
        //    lst.Text = "--Select--";
        //    drpcurrency.Items.Insert(0, lst);
        //    drpcurrency.SelectedIndex = 0;
        //}
        public void ToggleAdd(string Mode)
        {
            try
            {
                if (Mode == "Add")
                {
                    btnSearch.Visible = false;
                    btnAdd.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = true;
                    DivSearch.Style.Add("display", "none");
                    DivAdd.Style.Add("display", "block");
                    lblseroutHeader.Visible = false;
                    DivSearch.Style.Add("display", "none");
                }
                else
                {
                    btnSearch.Visible = true;
                    btnAdd.Visible = true;
                    btnPrint.Visible = true;
                    btnSave.Visible = false;
                    DivSearch.Style.Add("display", "block");
                    DivAdd.Style.Add("display", "none");
                    lblseroutHeader.Visible = true;
                    DivSearch.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }

        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleAdd("Add");
                HddAddCancel.Value = "1";
                HddUserID.Value = "0";
                chkIsActive.Visible = false;
                DivCheckBox.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                List<IsUserExist> lstuser = lclsService.GetExistUser(txtUsername.Text).ToList();
                //if (lstuser.Count != 0 && drpexistuser.SelectedIndex == 0)
                if (lstuser.Count != 0 && 8 == 0)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "anything", "alert('User Already Exist');", true);
                    string msg = Constant.WarningAddUserMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<AddUser>>", txtUsername.Text).Replace("');", "");
                    log.LogWarning(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningAddUserMessage.Replace("<<AddUser>>", txtUsername.Text), true);
                }
                else
                {
                    lclsUser.FirstName = txtFirstname.Text;
                    lclsUser.LastName = txtLastname.Text;
                    lclsUser.UserName = txtUsername.Text;
                    lclsUser.Password = txtPwd.Text;
                    lclsUser.ConfirmPassword = txtComfirmPwd.Text;
                    lclsUser.Phone = txtPhone.Text;
                    if (txtxtn.Text != "")
                        lclsUser.Xtn = Convert.ToInt64(txtxtn.Text);
                    lclsUser.Email = txtEmail.Text;
                    lclsUser.CorporateID = Convert.ToInt64(drpcorp.SelectedValue);
                    var FinalString = "";
                    var SB = new StringBuilder();
                    foreach (ListItem lst in drpFacilitys.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    lclsUser.FacilityName = FinalString;
                    lclsUser.UserRoleID = Convert.ToInt64(drpUserrole.SelectedValue);
                    //lclsUser.BudgetCurrency = drpcurrency.SelectedValue;
                    //if (txtbudgetvalue.Text != "")
                    //    lclsUser.BudgetValue = Convert.ToInt64(txtbudgetvalue.Text);
                    //if (drpexistuser.SelectedValue ==null)
                    //lclsUser.UserID = Convert.ToInt64(drpexistuser.SelectedValue);
                    lclsUser.UserID = Convert.ToInt64(HddUserID.Value);
                    if(chkIsActive.Visible == true)
                    {
                        if (chkIsActive.Checked == true)
                        {
                            lclsUser.IsActive = true;
                        }
                        else
                        {
                            lclsUser.IsActive = false;
                        }
                    }
                    else
                    {
                        lclsUser.IsActive = true;
                    }
                    
                    lclsUser.CreatedOn = DateTime.Now;
                    //lclsUser.UserID = Convert.ToInt64(drpexistuser.SelectedValue);
                    Int64 lintCreatedBy = defaultPage.UserId;
                    if (!ValidateLoookups(lclsService)) return;

                    List<Validuseremail> lstMaster = lclsService.Validuseremail(txtEmail.Text).ToList();
                    if (lstMaster.Count <= 0)
                    {
                        string lstrMessage = lclsService.InsertUser(lclsUser, lintCreatedBy);
                        // if (lstrMessage == "Saved Successfully" && drpexistuser.SelectedIndex == 0)
                        if (lstrMessage == "Saved Successfully" && 1 == 0)
                        {
                            log.LogInformation(msgwrn.Replace("<<AddUser>>", txtUsername.Text));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserSaveMessage.Replace("<<AddUser>>", txtUsername.Text), true);
                            HddUserID.Value = "0";
                            ToggleAdd("Clear");
                            SearchGrid();
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "Saved Successfully");
                        }
                        else
                        {
                            //Functions objfun = new Functions();
                            //objfun.MessageDialog(this, "Updated Successfully");
                            log.LogInformation(msgwrn.Replace("<<AddUser>>", txtUsername.Text));
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserSaveMessage.Replace("<<AddUser>>", txtUsername.Text), true);
                            HddUserID.Value = "0";
                            ToggleAdd("Clear");
                            SearchGrid();
                        }

                        List<IsUserExist> lstuser1 = lclsService.GetExistUser(txtUsername.Text).ToList();
                        //ViewState["UserID"] = Convert.ToInt64(lstuser1[0].UserID);
                        grduserrole.DataSource = lclsService.GetRoleandFacility(Convert.ToInt64(lstuser1[0].UserID)).ToList();
                        grduserrole.DataBind();
                        List<GetUserDetails> objuser = lclsService.GetUserDetails(Convert.ToInt64(lstuser1[0].UserID)).ToList();
                        if (objuser.Count > 0)
                        {
                            txtFirstname.Text = objuser[0].FirstName;
                            txtLastname.Text = objuser[0].LastName;
                            txtUsername.Text = objuser[0].UserName;
                            string Pass = objuser[0].Password;
                            txtPwd.Attributes.Add("value", Pass);
                            txtComfirmPwd.Attributes.Add("Value", Pass);
                            txtEmail.Text = objuser[0].Email;
                            txtPhone.Text = objuser[0].PhoneNo;
                            if (objuser[0].Xtn != 0)
                                txtxtn.Text = Convert.ToString(objuser[0].Xtn);
                            if (Convert.ToInt32(objuser[0].IsActive) == 1)
                            {
                                chkIsActive.Checked = true;
                            }
                            else
                            {
                                chkIsActive.Checked = false;
                            }
                            txtUsername.Enabled = false;
                        }
                        //BindExist("Add");
                        BindUserRole();
                        BindCorporateMaster();
                        BindFacility("Add");
                        //BindBudget();
                        //txtbudgetvalue.Text = "";
                        if (lstuser.Count <= 0)
                        {
                            //drpexistuser.ClearSelection();
                            //drpexistuser.SelectedValue = Convert.ToString(lstuser1[0].UserID);
                        }
                        else
                        {
                            Clear();
                        }
                    }
                    else
                    {
                        //if(lstMaster[0].UserID == Convert.ToInt64(drpexistuser.SelectedValue))
                        if (lstMaster[0].UserID == Convert.ToInt64(HddUserID.Value))
                        {
                            string lstrMessage = lclsService.InsertUser(lclsUser, lintCreatedBy);
                            // if (lstrMessage == "Saved Successfully" && drpexistuser.SelectedIndex == 0)
                            if (lstrMessage == "Saved Successfully" && Convert.ToInt64(HddUserID.Value) == 0)

                            {
                                log.LogInformation(msgwrn.Replace("<<AddUser>>", txtUsername.Text));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserSaveMessage.Replace("<<AddUser>>", txtUsername.Text), true);
                                HddUserID.Value = "0";
                                ToggleAdd("Clear");
                                SearchGrid();
                                //Functions objfun = new Functions();
                                //objfun.MessageDialog(this, "Saved Successfully");
                            }
                            else
                            {
                                //Functions objfun = new Functions();
                                //objfun.MessageDialog(this, "Updated Successfully");
                                log.LogInformation(msgwrn.Replace("<<AddUser>>", txtUsername.Text));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserUpdateMessage.Replace("<<AddUser>>", txtUsername.Text), true);
                                HddUserID.Value = "0";
                                ToggleAdd("Clear");
                                SearchGrid();
                            }

                            List<IsUserExist> lstuser1 = lclsService.GetExistUser(txtUsername.Text).ToList();
                            //ViewState["UserID"] = Convert.ToInt64(lstuser1[0].UserID);
                            grduserrole.DataSource = lclsService.GetRoleandFacility(Convert.ToInt64(lstuser1[0].UserID)).ToList();
                            grduserrole.DataBind();
                            List<GetUserDetails> objuser = lclsService.GetUserDetails(Convert.ToInt64(lstuser1[0].UserID)).ToList();
                            if (objuser.Count > 0)
                            {
                                txtFirstname.Text = objuser[0].FirstName;
                                txtLastname.Text = objuser[0].LastName;
                                txtUsername.Text = objuser[0].UserName;
                                string Pass = objuser[0].Password;
                                txtPwd.Attributes.Add("value", Pass);
                                txtComfirmPwd.Attributes.Add("Value", Pass);
                                txtEmail.Text = objuser[0].Email;
                                txtPhone.Text = objuser[0].PhoneNo;
                                if (objuser[0].Xtn != 0)
                                    txtxtn.Text = Convert.ToString(objuser[0].Xtn);
                                if (Convert.ToInt32(objuser[0].IsActive) == 1)
                                {
                                    chkIsActive.Checked = true;
                                }
                                else
                                {
                                    chkIsActive.Checked = false;
                                }
                                txtUsername.Enabled = false;
                            }
                            //BindExist("Add");
                            BindUserRole();
                            BindCorporateMaster();
                            BindFacility("Add");
                            //BindBudget();
                            //txtbudgetvalue.Text = "";
                            if (lstuser.Count <= 0)
                            {
                                //drpexistuser.ClearSelection();
                                //drpexistuser.SelectedValue = Convert.ToString(lstuser1[0].UserID);
                            }
                            else
                            {
                                Clear();
                            }
                        }
                        else
                        {
                            List<Validuseremail> lstMastercount = lstMaster.Where(a => a.IsActive == true).ToList();
                            if (lstMastercount.Count > 0)
                            {
                                log.LogWarning(msgext.Replace("<<AddUser>>", lstMaster[0].UserName));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.checkemailexists.Replace("<<AddUser>>", lstMaster[0].UserName), true);
                            }
                            else
                            {
                                log.LogWarning(msgext.Replace("<<AddUser>>", lstMaster[0].UserName));
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.checkemailisactiveexists.Replace("<<AddUser>>", lstMaster[0].UserName), true);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        //protected void drpexistuser_SelectedIndexChanged(object sender, EventArgs e)
        //{
            

        //}
        public void Clear()
        {
            try
            {
                txtFirstname.Text = "";
                txtLastname.Text = "";
                txtUsername.Text = "";
                txtPwd.Attributes["value"] = "";
                txtComfirmPwd.Attributes["value"] = "";
                txtPhone.Text = "";
                txtxtn.Text = "";
                txtEmail.Text = "";
                drpFacilitys.SelectedIndex = -1;
                drpUserrole.SelectedValue = "0";
                //drpexistuser.SelectedValue = "0";
                txtUsername.Enabled = true;
                drpcorp.SelectedIndex = 0;
                //chkinact.Checked = false;
                //drpcurrency.SelectedIndex = 0;
                //txtbudgetvalue.Text = "";
                grduserrole.DataSource = null;
                grduserrole.DataBind();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        private void LoadLookups(string mode)
        {
            try
            {
                BindFacility(mode);
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        private bool ValidateLoookups(InventoryServiceClient service)
        {
            try
            {
                bool result = true;
                string errmessage = "";
                //Facility Lookup
                if (drpFacilitys.SelectedItem != null)
                {
                    List<GetCorporateFacility> lstCat = service.GetCorporateFacility((Convert.ToInt64(drpcorp.SelectedValue))).Where(a => a.IsActive == true && a.FacilityID == Convert.ToInt64(drpFacilitys.SelectedValue)).ToList();
                    if (lstCat.Count == 0)
                    {
                        errmessage += "Facility (" + drpFacilitys.SelectedItem.Text + ") , ";
                        result = false;
                    }
                    if (!result)
                    {
                        EventLogger log = new EventLogger(config);
                        string msg = Constant.WarningLookupMessage.Replace("ShowwarningLookupPopup('", "").Replace("<<values>>", errmessage).Replace("');", "");
                        log.LogWarning(msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningLookupMessage.Replace("<<values>>", errmessage), true);
                    }
                }


                return result;
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
                return false;
            }
            
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                if(HddAddCancel.Value == "0")
                {
                    Response.Redirect("AddUser.aspx");
                }
                else
                {
                    HddAddCancel.Value = "0";
                    HddUserID.Value = "0";
                    ToggleAdd("Clear");
                    Clear();
                    LoadLookups("Add");
                }
                
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        protected void lblbbudget_Click(object sender, EventArgs e)
        {
            try
            {
                txtbudget.Style.Add("border", "Solid 1px #d2d6de");
                mpebudget.Show();
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        protected void btnbudgetvalue_Click(object sender, EventArgs e)
        {
            try
            {
                BALItem objuser = new BALItem();
                if (txtbudget.Text == "")
                {
                    txtbudget.Style.Add("border", "Solid 1px red");
                    mpebudget.Show();
                }
                else
                {
                    objuser.CurrencyName = txtbudget.Text;
                    string a = lclsService.InsertCurrency(objuser);
                    if (a == "Saved Successfully")
                    {
                        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "anything", "alert('Saved Successfully');", true);  
                    }
                }
                txtbudget.Text = "";
                //BindBudget();
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }
        protected void drpcorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // if (drpexistuser.SelectedIndex == 0)
                if (1 == 0)
                {
                    BindFacility("Add");
                }
                else
                {
                    BindFacility("Edit");
                }
                string Password = txtPwd.Text;
                txtPwd.Attributes.Add("value", Password);
                string RePassword = txtPwd.Text;
                txtComfirmPwd.Attributes.Add("value", RePassword);
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        protected void lbremove_Click(object sender, EventArgs e)
        {
            try
            {
                HddMainDelete.Value = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                ViewState["UserRoleID"] = Convert.ToInt64(gvrow.Cells[1].Text);
                ViewState["userrole"] = gvrow.Cells[6].Text;
                ImageButton lbremovediag = sender as ImageButton;
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                EventLogger log = new EventLogger(config);
                string lstmsg = string.Empty;
                Int64 UserID = 0;
                if (HddMainDelete.Value == "Delete")
                {                    
                    Int64 LogInBy = defaultPage.UserId;

                    lstmsg = lclsService.DeleteUserDetails(Convert.ToInt64(HddUserID.Value), LogInBy);
                }
                else
                {
                    string a = lclsService.RemoveFacilityRole(Convert.ToInt64(ViewState["UserRoleID"]));
                }

                if (lstmsg == "Deleted Successfully")
                {
                    if (HddMainDelete.Value == "Delete")
                    {
                        log.LogInformation(msgdete.Replace("<<AddUser>>", HddUserID.Value));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserDeleteMessage.Replace("<<AddUser>>", HddUserID.Value), true);
                        SearchGrid();
                    }
                    else
                    {
                        //Functions objfun = new Functions();
                        //objfun.MessageDialog(this, "Deleted Successfully");
                        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "anything", "alert('Deleted Successfully');", true);
                        log.LogInformation(msgdete.Replace("<<AddUser>>", ViewState["userrole"].ToString()));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserDeleteMessage.Replace("<<AddUser>>", ViewState["userrole"].ToString()), true);
                        // grduserrole.DataSource = lclsService.GetRoleandFacility(Convert.ToInt64(drpexistuser.SelectedValue));
                        grduserrole.DataSource = lclsService.GetRoleandFacility(Convert.ToInt64(HddUserID.Value));
                        grduserrole.DataBind();
                    }
                    HddMainDelete.Value = "";
                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        protected void chkinact_CheckedChanged(object sender, EventArgs e)
        {
            //if(chkinact.Checked == true)
            //{
            //    BindExist("Edit");
            //}
            //else
            //{
            //    BindExist("Add");
            //}

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> llstresult = PrintUserSummary();
                byte[] bytes = (byte[])llstresult[0];
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "User Summary" + guid + ".pdf";
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
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }
        public List<object> PrintUserSummary()
        {
            try
            {
                List<object> llstarg = new List<object>();
                if (drpCorpSearch.SelectedValue == "All")
                {
                    lclsUser.ListCorporateID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpCorpSearch.Items)
                    {
                        if (lst.Selected && drpCorpSearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    lclsUser.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpFacilitySearch.SelectedValue == "All")
                {
                    lclsUser.ListFacilityID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpFacilitySearch.Items)
                    {
                        if (lst.Selected && drpFacilitySearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    lclsUser.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpRoleSearch.SelectedValue == "All")
                {
                    lclsUser.ListRoleID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpRoleSearch.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    lclsUser.ListRoleID = FinalString;
                }
                SB.Clear();

                lclsUser.UserName = txtSerchUserName.Text;
                lclsUser.Active = rdbstatus.SelectedValue;
                lclsUser.LoggedinBy = defaultPage.UserId;
                lclsUser.Filter = "";

                List<SearchUserDetails> llstreview = lclsService.SearchUserDetails(lclsUser).ToList();
                //List<BindUserSummaryReport> llstreview = lclsService.BindUserSummaryReport(Convert.ToInt64(ViewState["UserID"]),defaultPage.UserId,"").ToList();
                //List<GetMedicalSupplyPoReportDetails> llstreview = lclsservice.GetMedicalSupplyPoReportDetails(PRmasterID, null, defaultPage.UserId).ToList();
                rvUserSummaryreport.ProcessingMode = ProcessingMode.Local;
                rvUserSummaryreport.LocalReport.ReportPath = Server.MapPath("~/Reports/UserSummaryReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("UserSummaryReportDS", llstreview);
                rvUserSummaryreport.LocalReport.DataSources.Clear();
                rvUserSummaryreport.LocalReport.DataSources.Add(datasource);
                rvUserSummaryreport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = null;
                bytes = rvUserSummaryreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                llstarg.Insert(0, bytes);
                return llstarg;
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
                return null;
            }
           
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchGrid();
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
            
        }

        public void SearchGrid()
        {
            try
            {                
                if (drpCorpSearch.SelectedValue == "All")
                {
                    lclsUser.ListCorporateID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpCorpSearch.Items)
                    {
                        if (lst.Selected && drpCorpSearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    lclsUser.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpFacilitySearch.SelectedValue == "All")
                {
                    lclsUser.ListFacilityID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpFacilitySearch.Items)
                    {
                        if (lst.Selected && drpFacilitySearch.SelectedValue != "All")
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    lclsUser.ListFacilityID = FinalString;

                }
                FinalString = "";
                SB.Clear();
                if (drpRoleSearch.SelectedValue == "All")
                {
                    lclsUser.ListRoleID = "ALL";
                }
                else
                {
                    foreach (ListItem lst in drpRoleSearch.Items)
                    {
                        if (lst.Selected)
                        {
                            SB.Append(lst.Value + ',');
                        }
                    }
                    if (SB.Length > 0)
                        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                    lclsUser.ListRoleID = FinalString;
                }
                SB.Clear();

                lclsUser.UserName = txtSerchUserName.Text;
                lclsUser.Active = rdbstatus.SelectedValue;
                lclsUser.LoggedinBy = defaultPage.UserId;
                lclsUser.Filter = "";
                
                List<SearchUserDetails> llstSearchMaster = lclsService.SearchUserDetails(lclsUser).ToList();
                //lblrowcount.Text = "No of records : " + llstSearchMaster.Count.ToString();
                GrdUserDetails.DataSource = llstSearchMaster;
                GrdUserDetails.DataBind();
                
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                ViewState["UserID"] = Convert.ToInt64(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", ""));
                HddUserID.Value = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                HddAddCancel.Value = "1";                
                List<GetUserDetails> objuser = lclsService.GetUserDetails(Convert.ToInt64(ViewState["UserID"])).ToList();
                if (objuser.Count > 0)
                {
                    txtFirstname.Text = objuser[0].FirstName;
                    txtLastname.Text = objuser[0].LastName;
                    txtUsername.Text = objuser[0].UserName;
                    string Pass = objuser[0].Password;
                    txtPwd.Attributes.Add("value", Pass);
                    txtComfirmPwd.Attributes.Add("Value", Pass);
                    txtEmail.Text = objuser[0].Email;
                    txtPhone.Text = objuser[0].PhoneNo;
                    if (objuser[0].Xtn != 0)
                        txtxtn.Text = Convert.ToString(objuser[0].Xtn);
                    if(objuser[0].IsActive == false)
                    {
                        DivCheckBox.Style.Add("display", "block");
                        chkIsActive.Visible = true;
                    }
                    else
                    {
                        DivCheckBox.Style.Add("display", "none");
                        chkIsActive.Visible = false;
                    }
                    chkIsActive.Checked = true;
                    //if (Convert.ToInt32(objuser[0].IsActive) == 1)
                    //{
                        
                    //}
                    //else
                    //{
                    //    chkIsActive.Checked = false;
                    //}
                    txtUsername.Enabled = false;
                    BindCorporateMaster();
                    BindfacilityRole();
                    BindFacility("Edit");
                    BindUserRole();
                    ToggleAdd("Add");
                    //drpcurrency.SelectedIndex = 0;
                    //txtbudgetvalue.Text = "";
                }
                else
                {
                    Clear();
                }
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }

        protected void Imgdelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;                
                HddMainDelete.Value = "Delete";
                HddUserID.Value = gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowConfirmationPopup()", true);
            }
            catch(Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }

        protected void GrdUserDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton Imgdelete = (ImageButton)e.Row.FindControl("Imgdelete");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    if (lblActive.Text == "Yes")
                    {
                        Imgdelete.Enabled = true;
                        chkIsActive.Visible = false;
                        DivCheckBox.Style.Add("display", "none");
                    }
                    else
                    {
                        Imgdelete.Enabled = false;
                        //divactive.Style.Add("display", "block");
                        chkIsActive.Visible = true;
                        DivCheckBox.Style.Add("display", "block");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.AddUserErrorMessage.Replace("<<AddUser>>", ex.Message.ToString()), true);
            }
        }
    }
}