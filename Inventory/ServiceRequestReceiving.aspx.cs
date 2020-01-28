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
    public partial class ServiceRequestReceiving : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        BALServiceRequest llstSReceivedetails = new BALServiceRequest();
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
                scriptManager.RegisterPostBackControl(this.grdSRReceiving);
                //scriptManager.RegisterPostBackControl(this.btnPrint);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.WarningServiceRequestPOMessage.Replace("<<ServiceRequestPO>>", "No changes made in the Service Request PO. "), true);
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        lclsservice.SyncServiceReceivingorder();
                        BindCorporate();
                        //drpcorsearch.SelectedValue = Convert.ToString(defaultPage.CorporateID);
                        BindFacility();
                        //drpfacilitysearch.SelectedValue = Convert.ToString(defaultPage.FacilityID);
                        BindStatus("Add");                       
                        BindRequestPOGrid();
                        HddUserRoleID.Value = defaultPage.RoleID.ToString();
                        if (defaultPage.RoleID != 1 && defaultPage.Rec_WorkOrServicePage_Edit == false && defaultPage.Rec_WorkOrServicePage_View == false)
                        {
                            updmain.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                        if (defaultPage.RoleID != 1 && defaultPage.Rec_WorkOrServicePage_Edit == false && defaultPage.Rec_WorkOrServicePage_View == true)
                        {
                            btnReceivingSave.Enabled = false;
                        }
                        
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
                    //    if (drpcorsearch.SelectedValue != "All")
                    //    {
                    //        // Search Drop Down
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
                }
                foreach (ListItem lst in drpfacilitysearch.Items)
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
                lstLookUp = lclsservice.GetList("ServiceRequestReceivingOrder", "Status", mode).ToList();

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
                    drpStatussearch.Items.FindByText(Constant.JobCompleted).Selected = true;
                }
                else
                {
                    drpStatussearch.Items.FindByText(Constant.OrderStatus).Selected = true;
                }


                List<GetList> lstreason = new List<GetList>();
                lstreason = lclsservice.GetList("ReceivingOrders", "Reason", mode).ToList();

                drpreason.DataSource = lstreason;
                drpreason.DataTextField = "InvenValue";
                drpreason.DataValueField = "InvenValue";
                drpreason.DataBind();
                //drpStatussearch.Items.Insert(0, lst1);

                ListItem lst1 = new ListItem();
                lst1.Value = "0";
                lst1.Text = "--Select Reason--";
                drpreason.Items.Insert(0, lst1);

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);

            }

        }



        public void BindRequestPOGrid()
        {
            try
            {
                var FinalString = "";
                var SB = new StringBuilder();

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListCorporateID = "ALL";
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

                    llstSReceivedetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListFacilityID = "ALL";
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
                    llstSReceivedetails.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListStatus = "ALL";
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
                    llstSReceivedetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSReceivedetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSReceivedetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSReceivedetails.LoggedinBy = defaultPage.UserId;

                List<SearchServiceRequestReceivingOrder> lstSRPOMaster = lclsservice.SearchServiceRequestReceivingOrder(llstSReceivedetails).ToList();
                grdSRReceiving.DataSource = lstSRPOMaster;
                grdSRReceiving.DataBind();

                //if (search == "")
                //{
                //    List<SearchServiceRequestReceivingOrder> lstSRPOMaster = lclsservice.SearchServiceRequestReceivingOrder(llstSReceivedetails).Where(a => a.isEdit != 0).ToList();
                //    grdSRReceiving.DataSource = lstSRPOMaster;
                //    grdSRReceiving.DataBind();
                //}
                //else
                //{
                //    List<SearchServiceRequestReceivingOrder> lstSRPOMaster = lclsservice.SearchServiceRequestReceivingOrder(llstSReceivedetails).ToList();
                //    grdSRReceiving.DataSource = lstSRPOMaster;
                //    grdSRReceiving.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }
        }

        protected void grdSRPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //Label lblCreatedBy = (Label)e.Row.FindControl("lblCreatedBy");
                //Image imgreadmore1 = (Image)e.Row.FindControl("imgreadmore1");
                //Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
                //Image imgreadmore = (Image)e.Row.FindControl("imgreadmore");
                LinkButton lbrevSRPROno = (LinkButton)e.Row.FindControl("lbrevSRPROno");
                LinkButton lbrevSPOno = (LinkButton)e.Row.FindControl("lbrevSPOno");
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (lblRemarks.Text != "")
                    //{
                    //    if (lblRemarks.Text.Length > 150)
                    //    {
                    //        lblRemarks.Text = lblRemarks.Text.Substring(0, 150) + "....";
                    //        imgreadmore.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        imgreadmore.Visible = false;
                    //    }
                    //}
                    //if (lblCreatedBy.Text != "")
                    //{
                    //    if (lblCreatedBy.Text.Length > 150)
                    //    {
                    //        lblCreatedBy.Text = lblCreatedBy.Text.Substring(0, 150) + "....";
                    //        imgreadmore1.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        imgreadmore1.Visible = false;
                    //    }
                    //}

                    if (lbrevSRPROno.Text != "" && defaultPage.RoleID != 1)
                    {
                        lbrevSRPROno.Enabled = false;
                    }
                    if (lbrevSRPROno.Text != "" && defaultPage.RoleID == 1)
                    {
                        lbrevSPOno.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message.ToString()), true);
            }

        }


        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("ServiceRequestReceiving.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindRequestPOGrid();
        }

        protected void lbrevQuote_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                byte[] bytes = DetailsOrderAttachment(Convert.ToInt64(gvrow.Cells[1].Text));
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

        //public byte[] DetailsContentOrderReport(string ListServiceRequestID, string MultiPrint)
        //{
        //    byte[] bytes = null;
        //    try
        //    {
        //        List<GetServiceRequestPoReportDetails> llstreview = null;
        //        if (MultiPrint == "Yes")
        //        {
        //            llstreview = lclsservice.GetServiceRequestPoReportDetails(null, ListServiceRequestID, defaultPage.UserId).ToList();
        //        }
        //        else if (MultiPrint == "No")
        //        {
        //            llstreview = lclsservice.GetServiceRequestPoReportDetails(ListServiceRequestID, null, defaultPage.UserId).ToList();
        //        }

        //        rvservicerequestPOreport.ProcessingMode = ProcessingMode.Local;
        //        rvservicerequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceRequestPOReport.rdlc");
        //        Int64 r = defaultPage.UserId;
        //        ReportParameter[] p1 = new ReportParameter[3];
        //        p1[0] = new ReportParameter("ServiceRequestID", "0");
        //        p1[1] = new ReportParameter("SearchFilters", "test");
        //        p1[2] = new ReportParameter("LockedBy", Convert.ToString(r));

        //        this.rvservicerequestPOreport.LocalReport.SetParameters(p1);
        //        ReportDataSource datasource = new ReportDataSource("SRPoReportDS", llstreview);
        //        rvservicerequestPOreport.LocalReport.DataSources.Clear();
        //        rvservicerequestPOreport.LocalReport.DataSources.Add(datasource);
        //        rvservicerequestPOreport.LocalReport.Refresh();
        //        Warning[] warnings;
        //        string[] streamids;
        //        string mimeType;
        //        string encoding;
        //        string extension;

        //        bytes = rvservicerequestPOreport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
        //    }
        //    return bytes;

        //}


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
                if(objemail.VoidRemarks!="Other")
                {
                    objemail.vendorEmailcontent = string.Format("Due to " + objemail.VoidRemarks +" "+ objemail.SPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + objemail.UserName + "<br/>" + objemail.UserEmail + "<br/>" + objemail.UserPhoneNo);
                }
                else
                {
                    objemail.vendorEmailcontent = string.Format("We are cancelling " + objemail.SPONo + ". Please see the attached document for details. If you have any questions or concerns please contact to any one below, else please send a confirmation.<br /><br /><br /><br />Thank you for service <br/>" + objemail.UserName + "<br/>" + objemail.UserEmail + "<br/>" + objemail.UserPhoneNo);
                }
                rvservicerequestPOreport.ProcessingMode = ProcessingMode.Local;
                rvservicerequestPOreport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceReceivePOPDF.rdlc");
                Int64 r = defaultPage.UserId;
                ReportParameter[] p1 = new ReportParameter[1];
                p1[0] = new ReportParameter("ServiceRequestID", Convert.ToString(ServiceRequestMasterID));
                this.rvservicerequestPOreport.LocalReport.SetParameters(p1);
                ReportDataSource datasource = new ReportDataSource("ServiceReceivePoDS", llstreview);
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

        //protected void imgbtnEmail_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton btndetails = sender as ImageButton;
        //        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

        //        List<GetServiceRequestActionByMasterID> llstServiceRequestDetailsID = lclsservice.GetServiceRequestActionByMasterID(Convert.ToInt64(gvrow.Cells[1].Text), Convert.ToInt64(gvrow.Cells[1].Text)).Where(a => a.Status == Constant.OrderStatus).ToList();

        //        byte[] bytes = DetailsOrderReport(Convert.ToInt64(gvrow.Cells[1].Text), Convert.ToInt64(llstServiceRequestDetailsID[0].ServiceRequestDetailsID));

        //        string lstrPDFPath = "Purchase Order Content – " + objemail.SPONo + ".pdf";
        //        objemail.vendoremailsubject = "Purchase Order – " + objemail.SPONo;
        //        MemoryStream attachstream = new MemoryStream(bytes);

        //        //byte[] bytes1 = DetailsContentOrderReport(gvrow.Cells[1].Text, "No");
        //        byte[] bytes1 = DetailsOrderAttachment(llstServiceRequestDetailsID[0].ServiceRequestDetailsID);
        //        MemoryStream attachstream1 = new MemoryStream(bytes1);
        //        string displayfilename1 = "Purchase Order – " + objemail.SPONo + ".pdf";
        //        objemail.SendEmailWithTwoPDFContent(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, lstrPDFPath, attachstream1, displayfilename1);

        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServicePartsOrderemailMessage, true);

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServicePartsOrderemailMessage.Replace("<<ServiceRequestPO>>", ""), true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
        //    }

        //}

        protected void lbitono_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btndetails = sender as LinkButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                Label lblservice1 = (Label)gvrow.FindControl("lblservice");
                Label lblunit = (Label)gvrow.FindControl("lblunit");
                Label lblprice = (Label)gvrow.FindControl("lblprice");
                LinkButton lbrevSRPROno = (LinkButton)gvrow.FindControl("lbrevSRPROno");

                HddServiceMasterID.Value = gvrow.Cells[0].Text;
                HddServiceDetailsID.Value = gvrow.Cells[1].Text;

                MPServiceReceivingOrder.Show();
                DivOtherreason.Style.Add("display", "none");
                DivJobComplete.Style.Add("display", "none");
                DivVoidSPO.Style.Add("display", "none");
                if (defaultPage.RoleID != 1 && defaultPage.Rec_WorkOrServicePage_Edit == false && defaultPage.Rec_WorkOrServicePage_View == true)
                {
                    btnReceivingSave.Enabled = false;
                }
                else
                {
                    btnReceivingSave.Enabled = true;
                }                
                rdbstatus.ClearSelection();
                txtInvoiceddate.Text = "";
                txtInvoiceno.Text = "";
                txtCompleteddate.Text = "";
                drpreason.ClearSelection();
                //if (rdbstatus.SelectedIndex == -1)
                //{
                //    rdbstatus.SelectedValue = "1";
                //}
                lblSRNo.Text = btndetails.Text;
                lblservice.Text = lblservice1.Text;
                lblUnit.Text = lblunit.Text;
                lblPrice.Text = lblprice.Text;

                if (defaultPage.RoleID == 1)
                {
                    DivInvoice.Style.Add("display", "none");
                    rdbstatus.Items[0].Enabled = false;
                    rdbstatus.Items[1].Enabled = true;
                }
                else
                {
                    DivInvoice.Style.Add("display", "none");
                    rdbstatus.Items[0].Enabled = true;
                    rdbstatus.Items[1].Enabled = false;
                }

                llstSReceivedetails.ServiceRequestMasterID = Convert.ToInt64(gvrow.Cells[0].Text);
                llstSReceivedetails.ServiceRequestDetailID = Convert.ToInt64(gvrow.Cells[1].Text);

                List<GetServiceReceiveOrder> llstSRecieveOrder = lclsservice.GetServiceReceiveOrder(llstSReceivedetails).ToList();

                if (llstSRecieveOrder[0].SPRONo != null || llstSRecieveOrder[0].Status == Constant.VoidOrderStatus)
                {
                    btnReceivingSave.Enabled = false;
                    rdbstatus.ClearSelection();
                    if (llstSRecieveOrder[0].IsReceive == true)
                    {
                        rdbstatus.SelectedValue = "1";
                        txtCompleteddate.Text = Convert.ToDateTime(llstSRecieveOrder[0].ReceivingDate).ToString("MM/dd/yyyy");
                        DivJobComplete.Style.Add("display", "block");
                        DivVoidSPO.Style.Add("display", "none");
                    }
                    else
                    {
                        rdbstatus.SelectedValue = "2";
                        drpreason.ClearSelection();
                        drpreason.Items.FindByText(llstSRecieveOrder[0].Remarks).Selected = true;
                        if (llstSRecieveOrder[0].Remarks == Constant.OtherReason)
                        {
                            DivOtherreason.Style.Add("display", "block");
                            txtotherreason.Text = llstSRecieveOrder[0].OtherRemarks;
                        }
                        DivJobComplete.Style.Add("display", "none");
                        DivVoidSPO.Style.Add("display", "block");
                        DivInvoice.Style.Add("display", "none");
                    }

                    if (llstSRecieveOrder[0].Status == Constant.JobCompleted && llstSRecieveOrder[0].InvoiceStatus != Constant.CloseOrderStatus && defaultPage.RoleID == 1)
                    {
                        txtInvoiceno.Text = llstSRecieveOrder[0].InvoiceNo;
                        txtInvoiceddate.Text = Convert.ToDateTime(llstSRecieveOrder[0].InvoiceDate).ToString("MM/dd/yyyy");
                        btnReceivingSave.Enabled = false;
                        DivInvoice.Style.Add("display", "none");
                        rdbstatus.Items[1].Enabled = false;
                    }
                    else if (llstSRecieveOrder[0].Status != Constant.VoidOrderStatus && llstSRecieveOrder[0].InvoiceStatus != Constant.CloseOrderStatus && defaultPage.RoleID == 1)
                    {                        
                        rdbstatus.Items[1].Enabled = false;
                    }
                    else if (llstSRecieveOrder[0].Status == Constant.VoidOrderStatus && defaultPage.RoleID != 1)
                    {
                        btnReceivingSave.Enabled = false;
                        rdbstatus.Items[0].Enabled = false;
                        rdbstatus.Items[1].Enabled = false;
                    }
                    else if (llstSRecieveOrder[0].InvoiceStatus == Constant.CloseOrderStatus)
                    {
                        btnReceivingSave.Enabled = false;
                        txtInvoiceno.Text = llstSRecieveOrder[0].InvoiceNo;
                        txtInvoiceddate.Text = Convert.ToDateTime(llstSRecieveOrder[0].InvoiceDate).ToString("MM/dd/yyyy");
                        DivInvoice.Style.Add("display", "block");
                        rdbstatus.Items[0].Enabled = false;
                        rdbstatus.Items[1].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }

        }

        protected void btnReceivingSave_Click(object sender, EventArgs e)
        {
            try
            {
                llstSReceivedetails.ServiceRequestMasterID = Convert.ToInt64(HddServiceMasterID.Value);
                llstSReceivedetails.CreatedBy = defaultPage.UserId;
                llstSReceivedetails.LastModifiedBy = defaultPage.UserId;
                //Error = ValidateReceivingOrder();
                //if (Error == true)
                //{
                llstSReceivedetails.ServiceRequestMasterID = Convert.ToInt64(HddServiceMasterID.Value);
                llstSReceivedetails.ServiceRequestDetailID = Convert.ToInt64(HddServiceDetailsID.Value);

                List<GetServiceReceiveOrder> llstSRecieveOrder = lclsservice.GetServiceReceiveOrder(llstSReceivedetails).ToList();

                if (llstSRecieveOrder[0].SPRONo == null)
                {
                    if (rdbstatus.SelectedIndex != -1)
                    {
                        if (rdbstatus.SelectedValue == "1")
                        {
                            llstSReceivedetails.Status = Constant.JobCompleted;
                            llstSReceivedetails.Remarks = "Gob Completed";
                            llstSReceivedetails.IsReceive = true;
                        }
                        else if (rdbstatus.SelectedValue == "2")
                        {
                            llstSReceivedetails.Status = Constant.VoidOrderStatus;
                            llstSReceivedetails.Remarks = drpreason.SelectedItem.Text;
                            if (drpreason.SelectedValue == Constant.OtherReason)
                            {
                                llstSReceivedetails.OtherRemarks = txtotherreason.Text;
                            }
                            else
                            {
                                llstSReceivedetails.OtherRemarks = null;
                            }
                            llstSReceivedetails.IsReceive = false;
                        }
                    }

                    lstmessage = lclsservice.UpdateServcieRecevingOrder(llstSReceivedetails);
                }
                else
                {
                    llstSReceivedetails.InvoiceNo = txtInvoiceno.Text;
                    llstSReceivedetails.InvoiceDate = Convert.ToDateTime(txtInvoiceddate.Text);
                    llstSReceivedetails.InvoiceStatus = Constant.CloseOrderStatus;
                    llstSReceivedetails.InvoiceRemarks = "Invoice Generated";

                    lstmessage = lclsservice.UpdateServcieRecevinginvoice(llstSReceivedetails);
                }

                //System.Threading.Thread.Sleep(5000);
                if (lstmessage == "Updated Successfully")
                {
                    BindRequestPOGrid();
                    if (rdbstatus.SelectedValue == "2")
                    {
                        objemail.SPONo = lblSRNo.Text;
                        if (drpreason.SelectedItem.Text == Constant.OtherReason)
                        {
                            objemail.VoidRemarks = txtotherreason.Text;
                        }
                        else
                        {
                            objemail.VoidRemarks = drpreason.SelectedItem.Text;
                        }                        

                        SendEmail();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestROSaveMessage.Replace("<<ServiceRequestRO>>", lblSRNo.Text), true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        public void SendEmail()
        {
            try
            {
                byte[] bytes = DetailsOrderReport(Convert.ToInt64(HddServiceMasterID.Value), Convert.ToInt64(HddServiceDetailsID.Value));


                objemail.vendoremailsubject = "Cancel Purchase Order – " + objemail.SPONo;
                MemoryStream attachstream = new MemoryStream(bytes);

                //byte[] bytes1 = DetailsContentOrderReport(gvrow.Cells[1].Text, "No");
                byte[] bytes1 = DetailsOrderAttachment(Convert.ToInt64(HddServiceDetailsID.Value));
                MemoryStream attachstream1 = new MemoryStream(bytes1);
                string displayfilename1 = "Purchase Void Order – " + objemail.SPONo + ".pdf";
                string displayfilename2 = "Service Receive Order – " + objemail.SPONo + ".pdf";
                objemail.SendEmailWithTwoPDFContent(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject, attachstream, displayfilename1, attachstream1, displayfilename2);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServicePartsOrderemailMessage, true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestROemailMessage.Replace("<<ServiceRequestRO>>", ""), true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        protected void lbrevSRPROno_Click(object sender, EventArgs e)
        {
            try
            {
                if (defaultPage.RoleID == 1)
                {
                    LinkButton btndetails = sender as LinkButton;
                    GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;

                    Label lblservice1 = (Label)gvrow.FindControl("lblservice");
                    Label lblunit = (Label)gvrow.FindControl("lblunit");
                    Label lblprice = (Label)gvrow.FindControl("lblprice");
                    LinkButton lbrevSRPROno = (LinkButton)gvrow.FindControl("lbrevSRPROno");

                    HddServiceMasterID.Value = gvrow.Cells[0].Text;
                    HddServiceDetailsID.Value = gvrow.Cells[1].Text;

                    MPServiceReceivingOrder.Show();
                    DivJobComplete.Style.Add("display", "block");
                    DivVoidSPO.Style.Add("display", "none");
                    DivInvoice.Style.Add("display", "block");
                    btnReceivingSave.Enabled = false;
                    rdbstatus.Items[0].Enabled = false;
                    rdbstatus.Items[1].Enabled = false;
                    rdbstatus.ClearSelection();
                    txtInvoiceddate.Text = "";
                    txtInvoiceno.Text = "";
                    txtCompleteddate.Text = "";
                    drpreason.ClearSelection();

                    lblSRNo.Text = btndetails.Text;
                    lblservice.Text = lblservice1.Text;
                    lblUnit.Text = lblunit.Text;
                    lblPrice.Text = lblprice.Text;


                    llstSReceivedetails.ServiceRequestMasterID = Convert.ToInt64(gvrow.Cells[0].Text);
                    llstSReceivedetails.ServiceRequestDetailID = Convert.ToInt64(gvrow.Cells[1].Text);

                    List<GetServiceReceiveOrder> llstSRecieveOrder = lclsservice.GetServiceReceiveOrder(llstSReceivedetails).ToList();

                    if (llstSRecieveOrder[0].SPRONo != null)
                    {
                        rdbstatus.ClearSelection();
                        if (llstSRecieveOrder[0].IsReceive == true)
                        {
                            rdbstatus.SelectedValue = "1";
                            txtCompleteddate.Text = Convert.ToDateTime(llstSRecieveOrder[0].ReceivingDate).ToString("MM/dd/yyyy");
                            btnReceivingSave.Enabled = true;
                        }

                        if (llstSRecieveOrder[0].InvoiceStatus == Constant.CloseOrderStatus)
                        {
                            txtInvoiceno.Text = llstSRecieveOrder[0].InvoiceNo;
                            txtInvoiceddate.Text = Convert.ToDateTime(llstSRecieveOrder[0].InvoiceDate).ToString("MM/dd/yyyy");
                            DivInvoice.Style.Add("display", "block");
                            btnReceivingSave.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ServiceRequestPOErrorMessage.Replace("<<ServiceRequestPO>>", ex.Message), true);
            }
        }

        public bool ValidateReceivingOrder()
        {
            if (rdbstatus.SelectedIndex != -1)
            {
                if (rdbstatus.SelectedValue == "1")
                {
                    if (txtCompleteddate.Text == "")
                    {
                        DivShowError.Style.Add("display", "block");
                        lblShowError.Text = "Compelted Date is Mandatory";
                        Error = false;
                    }
                }
                else if (rdbstatus.SelectedValue == "2")
                {
                    if (drpreason.SelectedValue == "0")
                    {
                        DivShowError.Style.Add("display", "block");
                        lblShowError.Text = "Reason is Mandatory";
                        Error = false;
                    }
                }
            }
            else
            {
                DivShowError.Style.Add("display", "block");
                lblShowError.Text = "No Action Performed";
                Error = false;
            }
            return Error;
        }



        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                DivShowError.Style.Add("display", "none");
                lblShowError.Text = "";
                //drpcorsearch.SelectedIndex = 0;
                //drpfacilitysearch.SelectedIndex = 0;
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
            BindRequestPOGrid();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var FinalString = "";
                var SB = new StringBuilder();

                if (drpcorsearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListCorporateID = "ALL";
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

                    llstSReceivedetails.ListCorporateID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpfacilitysearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListFacilityID = "ALL";
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
                    llstSReceivedetails.ListFacilityID = FinalString;
                }
                FinalString = "";
                SB.Clear();
                if (drpStatussearch.SelectedValue == "All")
                {
                    llstSReceivedetails.ListStatus = "ALL";
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
                    llstSReceivedetails.ListStatus = FinalString;
                }
                SB.Clear();
                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.Text = (DateTime.Today.AddMonths(-3)).ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSReceivedetails.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }
                if (txtDateTo.Text == "")
                {
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    llstSReceivedetails.DateTo = Convert.ToDateTime(txtDateTo.Text);
                }
                llstSReceivedetails.LoggedinBy = defaultPage.UserId;

                List<SearchServiceRequestReceivingOrder> lstSRPOMaster = lclsservice.SearchServiceRequestReceivingOrder(llstSReceivedetails).ToList();
                rvServiceReceivingSummaryReport.ProcessingMode = ProcessingMode.Local;
                rvServiceReceivingSummaryReport.LocalReport.ReportPath = Server.MapPath("~/Reports/ServiceReceivingSummaryPrintAllReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("ServiceReceivingSummaryPrintAllDS", lstSRPOMaster);
                rvServiceReceivingSummaryReport.LocalReport.DataSources.Clear();
                rvServiceReceivingSummaryReport.LocalReport.DataSources.Add(datasource);
                rvServiceReceivingSummaryReport.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                byte[] bytes = rvServiceReceivingSummaryReport.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Guid guid = Guid.NewGuid();
                string path = ConfigurationManager.AppSettings["TempFileLocation"].ToString();
                _sessionPDFFileName = "ServiceReceiveOrder" + guid + ".pdf";
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
                ShowPDFFile(path,""); 
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.MedicalSuppliesRequestPoErrorMessage.Replace("<<MedicalSupplyRequestPoDescription>>", ex.Message), true);
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