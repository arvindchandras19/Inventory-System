using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Inventoryserref;
using System.Text;

namespace Inventory
{
    public partial class FacilityItemMap : System.Web.UI.Page
    {
        Page_Controls defaultPage = new Page_Controls();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    defaultPage = (Page_Controls)Session["Permission"];
                    BindGrid();
                    BindFacility();
                    BindItem();
                    if (defaultPage != null)
                    {
                        if (defaultPage.FacilityItemMap_Edit == false && defaultPage.FacilityItemMap_View == true)
                        {
                            //SaveCancel.Visible = false;
                            btnSave.Visible = false;

                        }
                        if (defaultPage.FacilityItemMap_Edit == false && defaultPage.FacilityItemMap_View == false)
                        {
                            div_AddContent.Visible = false;
                            gridItems.Visible = false;
                            btnSave.Visible = false;
                            User_Permission_Message.Visible = true;
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch
            {

            }
        }
   
        public void BindGrid()
        {
            InventoryServiceClient service = new InventoryServiceClient();
            List<GetFacilityItemMap> list = new List<GetFacilityItemMap>();
            list = service.GetFacilityItemMap().ToList();
            gridItems.DataSource = list;
            gridItems.DataBind();
        }

        public void BindFacility()
        {
            try
            {
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetFacilityShortName> lstcategory = lclsservice.GetFacilityShortName().ToList();
                ddlFacilityID.DataSource = lstcategory;
                ddlFacilityID.DataValueField = "FacilityID";
                ddlFacilityID.DataTextField = "FacilityShortName";
                ddlFacilityID.DataBind();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select";
                ddlFacilityID.Items.Insert(0, lst);
            }
            catch (Exception ex)
            {

            }
        }

        public void BindItem()
        {
            try
            {
                InventoryServiceClient service = new InventoryServiceClient();
                List<GetItemDRD> listItem = new List<GetItemDRD>();
                listItem = service.GetItemDRD().ToList();
                drdItemID.DataSource = listItem;
                drdItemID.DataTextField = "ItemShortName";
                drdItemID.DataValueField = "ItemID";
                drdItemID.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        public void RemoveDuplicates(string[] s)
        {
            HashSet<string> set = new HashSet<string>(s);
            string[] result = new string[set.Count];
            set.CopyTo(result);
            for (int i = 0; i < result.Length; i++)
            {
                HddItemActive.Value += result[i].ToString() + ',';
            }
        }

        public void clearDropDown()
        {
            ddlFacilityID.SelectedIndex = 0;
            drdItemID.SelectedIndex = 0;
        }


        protected void lbedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //div_AddContent.Visible = true;
                InventoryServiceClient service = new InventoryServiceClient();
                ImageButton btndetails = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                hiFacilityItemID.Value = gvrow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                drdItemID.ClearSelection();
                //drdItemID.Items.FindByText(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                ddlFacilityID.ClearSelection();
                ddlFacilityID.Items.FindByText(gvrow.Cells[2].Text.Trim().Replace("&nbsp;", "")).Selected = true;
                GetItemsbyFacilityID(gvrow.Cells[4].Text.Trim().Replace("&nbsp;", ""));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showContent", "editfacility();", true);
                BindGrid();
            }
            catch
            {

            }
        }

        public void GetItemsbyFacilityID(string FacilityID)
        {
            InventoryServiceClient lclsservice = new InventoryServiceClient();
            List<GetItemsbyFacilityID> lstfac = lclsservice.GetItemsbyFacilityID(Convert.ToInt64(FacilityID)).ToList();
            foreach (var lst in lstfac)
            {
                foreach (ListItem drplst in drdItemID.Items)
                {
                    if (drplst.Value == lst.ItemID.ToString())
                    {
                        if (lst.IsActive == true)
                        {
                            drdItemID.Items.FindByText(drplst.Text).Selected = true;
                            HddItemActive.Value += drplst.Value + '/' + 1 + ',';
                        }
                        else
                        {
                            HddItemActive.Value += drplst.Value + '/' + 0 + ',';
                        }
                    }
                }

            }
        }

        //protected void lbdelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton btndetails = sender as ImageButton;
        //        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        //        hiFacilityItemID.Value = gvrow.Cells[2].Text;
        //        //InventoryServiceClient lcls = new InventoryServiceClient();
        //        //string lstrMessage = lcls.DeleteFacilityItemMap(Convert.ToInt64(hiFacilityItemID.Value));
        //        ModalPopupExtender2.Show();
        //        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "anything", "alert('Deleted Successfully');", true);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}



        protected void btnYes_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{

            //    InventoryServiceClient lclsService = new InventoryServiceClient();
            //    string lstrMessage = lclsService.DeleteFacilityItemMap(Convert.ToInt64(hiFacilityItemID.Value), Convert.ToInt64(Session["User"]));
            //    lblalert.Text = "Deleted Successfully";
            //    if (lstrMessage.Equals("Deleted Successfully"))
            //    {
            //        Functions objfun = new Functions();
            //        objfun.MessageDialog(this, "Deleted Successfully");
            //    }


            //    BindGrid();
            //}
            //catch (Exception ex)
            //{
            //    lblmsg.Text = ex.Message;
            //}
        }

        protected void chkactive_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
            CheckBox chkactive = (CheckBox)row.FindControl("chkactive");

            InventoryServiceClient lclsService = new InventoryServiceClient();

            if (chkactive.Checked == true)
            {
                lclsService.DeleteFacilityItemMap(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(Session["User"]), true);
            }
            else
            {
                lclsService.DeleteFacilityItemMap(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(Session["User"]), false);
            }

            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //div_AddContent.Visible = false;
                InventoryServiceClient service = new InventoryServiceClient();
                BALFaItemMap obj = new BALFaItemMap();
                //if (ViewState["FacilityItemID"] != null)
                if (hiFacilityItemID.Value != null)
                    obj.FacilityItemMapID = Convert.ToInt64(hiFacilityItemID.Value);
                InventoryServiceClient lclsservice = new InventoryServiceClient();
                List<GetItemsbyFacilityID> lstfac = lclsservice.GetItemsbyFacilityID(Convert.ToInt64(drdItemID.SelectedValue)).ToList();
                if (lstfac.Count == 0)
                {
                    foreach (ListItem drplst in drdItemID.Items)
                    {
                        if (drplst.Selected == true)
                        {
                            HddItemActive.Value += drplst.Value + '/' + 1 + ',';
                        }
                    }
                }
                else
                {
                    HddItemActive.Value = HddItemActive.Value.Substring(0, (HddItemActive.Value.Length - 1));
                    string[] SplitID = HddItemActive.Value.Split(',');
                    HddItemActive.Value = "";
                    for (int i = 0; i < SplitID.Length; i++)
                    {
                        string[] SplitTempID = SplitID[i].Split('/');
                        foreach (ListItem drplst in drdItemID.Items)
                        {
                            if (drplst.Value == SplitTempID[0].ToString())
                            {
                                if (drplst.Selected == true)
                                {
                                    HddItemActive.Value += drplst.Value + '/' + 1 + ',';
                                }
                                else
                                {
                                    HddItemActive.Value += drplst.Value + '/' + 0 + ',';
                                }

                            }
                            else if (drplst.Selected == true)
                            {
                                HddItemActive.Value += drplst.Value + '/' + SplitTempID[1].ToString() + ',';
                            }
                        }
                    }
                    HddItemActive.Value = HddItemActive.Value.Substring(0, (HddItemActive.Value.Length - 1));
                    string[] DupSplit = HddItemActive.Value.Split(',');
                    HddItemActive.Value = "";
                    RemoveDuplicates(DupSplit);                    
                }
                HddItemActive.Value = HddItemActive.Value.Substring(0, (HddItemActive.Value.Length - 1));
                obj.ItemID = HddItemActive.Value.ToString();
                obj.FacilityID = Convert.ToInt64(ddlFacilityID.SelectedValue);
                obj.CreatedBy = Convert.ToInt64(Session["User"]);
                obj.CreatedOn = DateTime.Now;
                string lstrmsg = service.InsertUpdateFacilityItemMap(obj);
                Functions objfun = new Functions();
                objfun.MessageDialog(this, "Saved Successfully");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "anything", "clear();", true);
                HddItemActive.Value = "";
                BindGrid();
                clearDropDown();
            }
            catch
            {

            }
        }
     
    }
}


