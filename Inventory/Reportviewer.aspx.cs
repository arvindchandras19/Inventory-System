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
using System.Web.Script.Serialization;

namespace Inventory
{
    public partial class Reportviewer : System.Web.UI.Page
    {
        InventoryServiceClient lclsservice = new InventoryServiceClient();
        Page_Controls defaultPage = new Page_Controls();
        StringBuilder SB = new StringBuilder();
        public static Int64 Roleid;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                defaultPage = (Page_Controls)Session["Permission"];
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                if (!IsPostBack)
                {
                    if (defaultPage != null)
                    {
                        BindCorporate();
                        BindFacility();
                        BindVendor();
                        BindCategory();
                        BindItem();
                        BindScreen();
                        BindReport("Add");
                        Roleid = Convert.ToInt64(defaultPage.RoleID);
                        if (defaultPage.Reports_Edit == false && defaultPage.Reports_View == true)
                        {

                        }

                        if (defaultPage.Reports_Edit == false && defaultPage.Reports_View == false)
                        {
                            updmain.Visible = true;
                            User_Permission_Message.Visible = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }
        }
        public void BindCorporate()
        {
            try
            {
                List<BALUser> lstcorp = new List<BALUser>();
                lstcorp = lclsservice.GetCorporateMaster().ToList();
                drpcorsearch.DataSource = lstcorp;
                drpcorsearch.DataTextField = "CorporateName";
                drpcorsearch.DataValueField = "CorporateID";
                drpcorsearch.DataBind();

                foreach (ListItem lst in drpcorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }

        }

        public void BindFacility()
        {
            try
            {
                drpfacilitysearch.DataSource = lclsservice.GetFacility().ToList();
                drpfacilitysearch.DataTextField = "FacilityDescription";
                drpfacilitysearch.DataValueField = "FacilityID";
                drpfacilitysearch.DataBind();

                foreach (ListItem lst in drpfacilitysearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }
        }

        public void BindVendor()
        {
            try
            {
                List<GetVendor> lstvendor = new List<GetVendor>();
                lstvendor = lclsservice.GetVendor().ToList();
                drpvendorsearch.DataSource = lstvendor;
                drpvendorsearch.DataTextField = "VendorDescription";
                drpvendorsearch.DataValueField = "VendorID";
                drpvendorsearch.DataBind();

                foreach (ListItem lst in drpvendorsearch.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }
        }

        public void BindCategory()
        {
            try
            {
                List<GetItemCategory> lstcategory = new List<GetItemCategory>();
                lstcategory = lclsservice.GetItemCategory().ToList();
                drpcategory.DataSource = lstcategory;
                drpcategory.DataTextField = "CategoryName";
                drpcategory.DataValueField = "CategoryID";
                drpcategory.DataBind();

                foreach (ListItem lst in drpcategory.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }

        }

        public void BindItem()
        {
            try
            {
                List<GetItem> lstitem = new List<GetItem>();
                lstitem = lclsservice.GetItem().ToList();
                drpitem.DataSource = lstitem;
                drpitem.DataTextField = "ItemDescription";
                drpitem.DataValueField = "ItemID";
                drpitem.DataBind();

                foreach (ListItem lst in drpitem.Items)
                {
                    lst.Attributes.Add("class", "selected");
                    lst.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }
        }

        public void BindScreen()
        {
            try
            {
                List<GetdrpSubMenu> lstitem = new List<GetdrpSubMenu>();
                lstitem = lclsservice.GetdrpSubMenu(7).ToList();
                drpordertype.DataSource = lstitem;
                drpordertype.DataTextField = "PageSubMenu";
                drpordertype.DataValueField = "PageSubMenuID";
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpordertype.DataBind();
                drpordertype.Items.Insert(0, lst);
                drpordertype.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }
        }

        public void BindReport(string Mode)
        {
            try
            {
                List<GetList> lstLookUp = new List<GetList>();
                lstLookUp = lclsservice.GetList("Report", "Type", Mode).ToList();
                drpreport.DataSource = lstLookUp;
                drpreport.DataTextField = "InvenValue";
                drpreport.DataValueField = "InvenValue";
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "--Select--";
                drpreport.DataBind();
                drpreport.Items.Insert(0, lst);
                drpreport.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", Constant.ReportErrorMessage.Replace("<<Report>>", ex.Message), true);
            }

        }
        
        [WebMethod]
        public static List<GetList> BindSubReport(string drpreporttext)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetList> lstLookUp = new List<GetList>();

                if(drpreporttext != "0")
                {
                    if (drpreporttext == "Usage")
                    {
                        lstLookUp = lclsservice.GetList("Usage", "SubType", "Add").ToList();
                    }
                    else if (drpreporttext == "Cumulative Usage")
                    {
                        lstLookUp = lclsservice.GetList("Cumulative", "SubType", "Add").ToList();
                    }

                }
                return lstLookUp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        [WebMethod]
        public static Dictionary<string, object> BindMonthlyUsage(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month,Nullable<DateTime>DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                //List<object> llstdata = new List<object>();
                //llstdata = lclsservice.GetMonthlyUsageReport(llstReport).ToList();
                DataSet ds = new DataSet();
                //ds = (DataSet)llstdata[1];

                ds = lclsservice.GetMonthlyUsageReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;
                Dictionary<string, object> list = new Dictionary<string, object>();

                if (ds != null)
                {
                    foreach (DataColumn dr in ds.Tables[0].Columns)
                    {
                        dtColumns.NewRow();
                        dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                    }

                    list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                    list["List02"] = GetDataTableDictionaryList(dtColumns);
                    //list["List03"] = llstdata[0].ToString();
                }

                return list;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    
        [WebMethod]
        public static Dictionary<string, object> BindMonthlyPurchase(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetMonthlyPurchaseReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [WebMethod]
        public static Dictionary<string, object> BindMonthlyEndingInventory(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetMonthlyEndingReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [WebMethod]
        public static Dictionary<string, object> BindCumulativebyItemCategory(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetCumUsageReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;


                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [WebMethod]
        public static Dictionary<string, object> BindCumulativebyItemDesc(string CorpID, string FacilityID, string VendorID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemID = ItemID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetCumulativeUsageReportBySingleItem(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [WebMethod]
        public static Dictionary<string, object> BindMonthlyInventory(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetMonthlyInventoryReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        [WebMethod]
        public static Dictionary<string, object> BindCostpertx(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetCostPertxReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [WebMethod]
        public static Dictionary<string, object> BindMonthlyDrugsUsage(string CorpID, string FacilityID, string VendorID, string CategoryID, Nullable<DateTime> Month, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, string ItemID, string ortype)
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                Page_Controls defaultPage = new Page_Controls();
                BALReport llstReport = new BALReport();
                string Status = string.Empty;
                llstReport.CorporateID = CorpID;
                llstReport.FacilityID = FacilityID;
                llstReport.VendorID = VendorID;
                llstReport.ItemCategoryID = CategoryID;
                if (Convert.ToString(Month) != "")
                {
                    llstReport.DateFrom = Month;
                }
                else
                {
                    llstReport.DateFrom = DateFrom;
                }

                llstReport.DateTo = DateTo;
                if (ItemID != "")
                    llstReport.ItemID = ItemID;
                if (ortype != "0")
                    llstReport.OrderType = ortype;
                llstReport.LoggedInBy = Roleid;
                llstReport.Filter = "";

                DataSet ds = new DataSet();
                ds = lclsservice.GetMonthlyDrugReport(llstReport);

                DataTable dtColumns = new DataTable();
                dtColumns.Columns.Add("title");
                dtColumns.Columns.Add("data");
                DataRow drr = null;

                foreach (DataColumn dr in ds.Tables[0].Columns)
                {
                    dtColumns.NewRow();
                    dtColumns.Rows.Add(dr.ToString(), dr.ToString());
                }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list["List01"] = GetDataTableDictionaryList(ds.Tables[0]);
                list["List02"] = GetDataTableDictionaryList(dtColumns);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public static List<Dictionary<string, string>> GetDataTableDictionaryList(DataTable dt)
        {
            try
            {
                return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column].ToString()
                )).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public static string DataSetToJSON(DataTable dt)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                //foreach (DataTable dt in ds.Tables)
                //{
                object[] arr = new object[dt.Rows.Count + 1];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }

                dict.Add(dt.TableName, arr);
                //}

                JavaScriptSerializer json = new JavaScriptSerializer();
                return json.Serialize(dict);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
       
    }
}
    
