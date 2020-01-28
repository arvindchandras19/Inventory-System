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

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   VendorOrderDue
'' Type      :   C# File
'' Description  :To add,update the VendorOrderDue Details
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				  Dhanasekaran.C	                  New
 *  10/24/2017         V2.0               Sairam.P                           Validate mandatory fields and Check the active and 
 *                                                                           inactive records in dropdown
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public partial class VendorOrderDueRemainder : System.Web.UI.Page
    {
        InventoryServiceClient lclsService = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        EmailController objemail = new EmailController();
        //private string _sessionPDFFileName;
        StringBuilder SB = new StringBuilder();
        string FinalString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                if (!IsPostBack)
                {
                    RequiredFieldValidatorMessage();
                    txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtDateTo.Text = (DateTime.Today.AddDays(+6)).ToString("MM/dd/yyyy");
                    BindCorporate();
                    BindFacility();
                    //BindVendor();
                    if (defaultPage != null)
                    {
                        if (defaultPage.VendorOrderDueRemainder_Edit == false && defaultPage.VendorOrderDueRemainder_View == true)
                        {
                            //btnSendmail.Visible = false;
                        }

                        if (defaultPage.VendorOrderDueRemainder_Edit == false && defaultPage.VendorOrderDueRemainder_View == false)
                        {
                            //grdvendororderue.Visible = false;
                            //btnSave.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }

        }

        private void RequiredFieldValidatorMessage()
        {
            try
            {
                string req = Constant.RequiredFieldValidator;
                Reqfielddrpcor.ErrorMessage = req;
                Reqdrpfacility.ErrorMessage = req;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }
            //ReqdrpVendor.ErrorMessage = req;
        }

        private void BindFacility()
        {
            try
            {
                if (drpcor.SelectedValue != "")
                {
                    drpfacility.DataSource = lclsService.GetFacilityByListCorporateID(drpcor.SelectedValue, defaultPage.UserId, defaultPage.RoleID).ToList();
                    drpfacility.DataTextField = "FacilityDescription";
                    drpfacility.DataValueField = "FacilityID";
                    drpfacility.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Facility--";
                    drpfacility.Items.Insert(0, lst);
                    drpfacility.SelectedIndex = 0;

                    //foreach (ListItem lsst in drpcor.Items)
                    //{
                    //    if (lsst.Selected && drpcor.SelectedValue != "All")
                    //    {
                    //        SB.Append(lsst.Value + ',');
                    //    }
                    //}
                    //if (SB.Length > 0)
                    //    FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                    //// Search Drop Down
                    //drpfacility.DataSource = lclsService.GetFacilityByListCorporateID(FinalString, defaultPage.UserId, defaultPage.RoleID).ToList();
                    //drpfacility.DataTextField = "FacilityDescription";
                    //drpfacility.DataValueField = "FacilityID";
                    //drpfacility.DataBind();
                }
                //foreach (ListItem lsst in drpfacility.Items)
                //{
                //    lsst.Attributes.Add("class", "selected");
                //    lsst.Selected = true;
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }
        }
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstcrop = new List<BALUser>();
                if (defaultPage.RoleID == 1)
                {
                    lstcrop = lclsService.GetCorporateMaster().ToList();
                    drpcor.DataSource = lstcrop;
                    drpcor.DataTextField = "CorporateName";
                    drpcor.DataValueField = "CorporateID";
                    drpcor.DataBind();
                    ListItem lst = new ListItem();
                    lst.Value = "0";
                    lst.Text = "--Select Corporate--";
                    drpcor.Items.Insert(0, lst);
                    drpcor.SelectedIndex = 0;
                }
                else
                {
                    lstcrop = lclsService.GetCorporateFacilityByUserID(defaultPage.UserId).ToList();
                    drpcor.DataSource = lstcrop.Select(a => new { a.CorporateID, a.CorporateName }).Distinct();
                    drpcor.DataTextField = "CorporateName";
                    drpcor.DataValueField = "CorporateID";
                    drpcor.DataBind();
                }
                //foreach (ListItem lsst in drpcor.Items)
                //{
                //    lsst.Attributes.Add("class", "selected");
                //    lsst.Selected = true;
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }

        }

        //public void BindVendor()
        //{
        //    try
        //    {
        //        if (drpfacility.SelectedValue != "")
        //        {
        //            List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
        //            lstvendor = lclsService.GetVendorByFacilityID(drpfacility.SelectedValue, defaultPage.UserId).ToList();
        //            drpvendor.DataSource = lstvendor;
        //            drpvendor.DataTextField = "VendorDescription";
        //            drpvendor.DataValueField = "VendorID";
        //            drpvendor.DataBind();
        //            ListItem lst = new ListItem();
        //            lst.Value = "0";
        //            lst.Text = "--Select Vendor--";
        //            drpvendor.Items.Insert(0, lst);
        //            drpvendor.SelectedIndex = 0;
        //            //foreach (ListItem lst in drpfacility.Items)
        //            //{
        //            //    if (lst.Selected && drpfacility.SelectedValue != "All")
        //            //    {
        //            //        SB.Append(lst.Value + ',');
        //            //    }
        //            //}
        //            //if (SB.Length > 0)
        //            //    FinalString = SB.ToString().Substring(0, (SB.Length - 1));

        //            //string SearchText = string.Empty;
        //            //List<GetVendorByFacilityID> lstvendor = new List<GetVendorByFacilityID>();
        //            //lstvendor = lclsService.GetVendorByFacilityID(FinalString, defaultPage.UserId).ToList();
        //            //drpvendor.DataSource = lstvendor;
        //            //drpvendor.DataTextField = "VendorDescription";
        //            //drpvendor.DataValueField = "VendorID";
        //            //drpvendor.DataBind();
        //        }
        //        //foreach (ListItem lst in drpvendor.Items)
        //        //{
        //        //    lst.Attributes.Add("class", "selected");
        //        //    lst.Selected = true;
        //        //}

        //    }

        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
        //    }
        //}


        protected void drpcor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindFacility();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }

        }

        
        //protected void txtoderDuedate_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox tb = (TextBox)sender;
        //    GridViewRow gvr = (GridViewRow)tb.NamingContainer;
        //    hdnvendororderID.Value += gvr.RowIndex + ",";
        //}
               

        //protected void drpfacility_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindVendor();
        //}
              
        /// <summary>
        /// Search the Vendor Order Due
        /// </summary>
        #region Bind Search Values
        public void SearchGrid()
        {
            try
            {
                BALVendorOrderDue lstMp = new BALVendorOrderDue();
                //if (drpcor.SelectedValue == "All")
                //{
                //    lstMp.CorporateName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpcor.Items)
                //    {
                //        if (lst.Selected && drpcor.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                //    lstMp.CorporateName = FinalString;
                //}
                //FinalString = "";
                //SB.Clear();
                //if (drpfacility.SelectedValue == "All")
                //{
                //    lstMp.FacilityName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpfacility.Items)
                //    {
                //        if (lst.Selected && drpfacility.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                //    lstMp.FacilityName = FinalString;

                //}
                //FinalString = "";
                //SB.Clear();
                //if (drpvendor.SelectedValue == "All")
                //{
                //    lstMp.VendorName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpvendor.Items)
                //    {
                //        if (lst.Selected && drpvendor.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                //    lstMp.VendorName = FinalString;
                //}
                //FinalString = "";
                //SB.Clear();

                //lstMp.CorporateID = Convert.ToInt64(drpcor.SelectedValue);
                //lstMp.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
                //lstMp.VendorID = Convert.ToInt64(drpvendor.SelectedValue);

                lstMp.CorporateName = drpcor.SelectedValue;
                lstMp.FacilityName = drpfacility.SelectedValue;
                //lstMp.VendorName = drpvendor.SelectedValue;
                txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                txtDateTo.Text = (DateTime.Today.AddDays(+6)).ToString("MM/dd/yyyy");
                lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);
                lstMp.LoggedInBy = defaultPage.UserId;
                List<GetVendorOrderdueRemainderReport> lstVendorOrderDue = lclsService.GetVendorOrderdueRemainderReport(lstMp).ToList();
                if (lstVendorOrderDue.Count > 0)
                {
                    if (lstVendorOrderDue[0].VenOrderDueID != 0)
                    {
                        grdvendororderueremainder.DataSource = lstVendorOrderDue;
                    }
                    else
                    {
                        grdvendororderueremainder.DataSource = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueRemainderMessage.Replace("<<VendorOrderDueRemainder>>", Constant.VendorOrderDueRemainderPreviewMail + drpfacility.SelectedItem.Text + ") facility"), true);
                    }

                }
                else
                {
                    grdvendororderueremainder.DataSource = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueRemainderMessage.Replace("<<VendorOrderDueRemainder>>", Constant.VendorOrderDueRemaindnovendordue), true);
                }
                grdvendororderueremainder.DataBind();

            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }
        }
        #endregion

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchGrid();
                lblcount.Visible = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                BALVendorOrderDue lstMp = new BALVendorOrderDue();
                //if (drpcor.SelectedValue == "All")
                //{
                //    lstMp.CorporateName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpcor.Items)
                //    {
                //        if (lst.Selected && drpcor.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));

                //    lstMp.CorporateName = FinalString;
                //}
                //FinalString = "";
                //SB.Clear();
                //if (drpfacility.SelectedValue == "All")
                //{
                //    lstMp.FacilityName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpfacility.Items)
                //    {
                //        if (lst.Selected && drpfacility.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                //    lstMp.FacilityName = FinalString;

                //}
                //FinalString = "";
                //SB.Clear();
                //if (drpvendor.SelectedValue == "All")
                //{
                //    lstMp.VendorName = "ALL";
                //}
                //else
                //{
                //    foreach (ListItem lst in drpvendor.Items)
                //    {
                //        if (lst.Selected && drpvendor.SelectedValue != "All")
                //        {
                //            SB.Append(lst.Value + ',');
                //        }
                //    }
                //    if (SB.Length > 0)
                //        FinalString = SB.ToString().Substring(0, (SB.Length - 1));
                //    lstMp.VendorName = FinalString;
                //}
                //FinalString = "";
                //SB.Clear();

                //lstMp.CorporateID = Convert.ToInt64(drpcor.SelectedValue);
                //lstMp.FacilityID = Convert.ToInt64(drpfacility.SelectedValue);
                //lstMp.VendorID = Convert.ToInt64(drpvendor.SelectedValue);

                lstMp.CorporateName = drpcor.SelectedValue;
                lstMp.FacilityName = drpfacility.SelectedValue;
                //lstMp.VendorName = drpvendor.SelectedValue;
                txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                txtDateTo.Text = (DateTime.Today.AddDays(+6)).ToString("MM/dd/yyyy");
                lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);
                lstMp.LoggedInBy = defaultPage.UserId;
                List<GetVendorOrderdueRemainderReport> lstVendorOrderDue = lclsService.GetVendorOrderdueRemainderReport(lstMp).ToList();
                if (lstVendorOrderDue.Count > 0)
                {
                    if (lstVendorOrderDue[0].VenOrderDueID != 0)
                    {
                        mpereview.Show();
                        DivBodyContent.InnerHtml = lstVendorOrderDue[0].BodyContent1;
                        DivBodyContent2.InnerHtml = lstVendorOrderDue[0].BodyContent2;
                        DivRegards.Style.Add("display", "block");
                        DivBodyContent3.InnerHtml = lstVendorOrderDue[0].BodyContent3;
                    }
                    else
                    {
                        mpereview.Show();
                        DivBodyContent.InnerHtml = lstVendorOrderDue[0].BodyContent1;
                        DivBodyContent2.InnerHtml = lstVendorOrderDue[0].BodyContent2;
                        DivRegards.Style.Add("display", "none");
                        DivBodyContent3.InnerHtml = lstVendorOrderDue[0].BodyContent3;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueRemainderMessage.Replace("<<VendorOrderDueRemainder>>", Constant.VendorOrderDueRemainderPreviewMail + drpfacility.SelectedItem.Text + ") facility"), true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("VendorOrderDueRemainder.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }

        }

        protected void btnSendmail_Click(object sender, EventArgs e)
        {
            try
            {

                BALVendorOrderDue lstMp = new BALVendorOrderDue();
                lstMp.CorporateName = drpcor.SelectedValue;
                lstMp.FacilityName = drpfacility.SelectedValue;
                //lstMp.VendorName = drpvendor.SelectedValue;
                txtDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lstMp.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                txtDateTo.Text = (DateTime.Today.AddDays(+6)).ToString("MM/dd/yyyy");
                lstMp.DateTo = Convert.ToDateTime(txtDateTo.Text);
                lstMp.LoggedInBy = defaultPage.UserId;
                List<GetVendorOrderdueRemainderReport> lstVendorOrderDue = lclsService.GetVendorOrderdueRemainderReport(lstMp).ToList();

                if (lstVendorOrderDue.Count > 0)
                {
                    if (lstVendorOrderDue[0].VenOrderDueID != 0)
                    {
                        objemail.vendorEmailcontent = lstVendorOrderDue[0].BodyContent1 + lstVendorOrderDue[0].BodyContent2 + "<div><br />Regards <br /> " + lstVendorOrderDue[0].BodyContent3 + "</div>";
                    }
                    else
                    {
                        objemail.vendorEmailcontent = lstVendorOrderDue[0].BodyContent1 + lstVendorOrderDue[0].BodyContent2 + lstVendorOrderDue[0].BodyContent3;
                    }
                    objemail.vendoremailsubject = lstVendorOrderDue[0].SubjectContent;
                    if (lstVendorOrderDue[0].FromEmailID == null || lstVendorOrderDue[0].ToEmailID == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueRemainderMessage.Replace("<<VendorOrderDueRemainder>>", Constant.VendorOrderDueRemainderMail + drpfacility.SelectedItem.Text +") facility"), true);
                    }
                    else
                    {
                        objemail.CorporateEmail = lstVendorOrderDue[0].FromEmailID;
                        objemail.vendorContactEmail = lstVendorOrderDue[0].ToEmailID;
                        objemail.SendEmailTransferOut(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject);
                        if(lstVendorOrderDue[0].VenOrderDueID == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderAdminMailSuccessMessage.Replace("<<VendorOrderDueRemainder>>", drpfacility.SelectedItem.Text), true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderTechMailSuccessMessage.Replace("<<VendorOrderDueRemainder>>", drpfacility.SelectedItem.Text), true);
                        }
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningVendorOrderDueRemainderMessage.Replace("<<VendorOrderDueRemainder>>", Constant.VendorOrderDueRemainderMail + drpfacility.SelectedItem.Text + ") facility"), true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.VendorOrderDueRemainderErrorMessage.Replace("<<VendorOrderDueRemainder>>", ex.Message.ToString()), true);
            }
        }
    }

}