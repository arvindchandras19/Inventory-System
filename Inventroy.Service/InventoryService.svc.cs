using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Inventroy.Service.BAL;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Configuration;
using Inventory;
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   Inventroy.Service
'' Type      :   SCV FILE
'' Description  :<< Inventroy Service (SCV FILE) >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				   S.Vivekanand                       New
 *  08/11/2017         V1.0                s.Vivekanand                 Created new sevice function for facility supplies map
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InventoryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InventoryService.svc or InventoryService.svc.cs at the Solution Explorer and start debugging.
    public class InventoryService : IInventoryService
    {
        EventLoggerConfig config = new EventLoggerConfig("Inventory", "", 101);
        public List<GetLoginDetails> GetLoginDetails(BALUser argclsUser)
        {
            List<GetLoginDetails> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetLoginDetails(argclsUser.FacilityID, argclsUser.UserName, argclsUser.Password).ToList();
            }
            return llstDiagDetails;
        }
        public bool IsUserExist(string argstrUserName, string argstrUserPassword)
        {
            //BALUserDetails lclsUser = new BALUserDetails();
            bool IsExist = false;
            InventoryEntities inventory = new InventoryEntities();
            User lclsChargeUser = inventory.Users.Where(a => a.Password == argstrUserPassword && a.LastName == argstrUserName).FirstOrDefault();
            if (lclsChargeUser != null)
            {
                IsExist = true;
            }
            return IsExist;
            //return lclsUser.IsUserExist(argstrUserName, argstrUserPassword);
        }
        public List<BALUser> GetUserIDDetails(string argclsUserName)
        {
            List<BALUser> llstBindUserDetails = new List<BALUser>();
            return llstBindUserDetails;
        }
        public List<GetUserInformation> GetUserInformation()
        {
            List<GetUserInformation> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetUserInformation().ToList();
            }
            return llstDiagDetails;
        }

        public List<GetUserPagePermission> GetLoginPermissionDetails(Int64 UserRoleID)
        {
            List<GetUserPagePermission> PageLevelPermission = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                PageLevelPermission = dbo.GetUserPagePermission(UserRoleID).ToList();
            }
            return PageLevelPermission;
        }

        public string InsertUpdateVendorDetails(BALVendor argclsvendor)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateVendorDetails(argclsvendor.VendorID, argclsvendor.VendorUIID, argclsvendor.VendorName, argclsvendor.ContactName, argclsvendor.ContactPhone, argclsvendor.ContactEmail, argclsvendor.POEmail, argclsvendor.AlternateEmail, argclsvendor.Address1, argclsvendor.Address2, argclsvendor.City, argclsvendor.State, argclsvendor.Zip, argclsvendor.Phone, argclsvendor.Xtn, argclsvendor.Fax, argclsvendor.All, argclsvendor.RegularSupplies, argclsvendor.MachineParts, argclsvendor.ServiceOrder, argclsvendor.BuildingMaintenance, argclsvendor.IT, argclsvendor.CreatedBy, argclsvendor.UpdatedBy, argclsvendor.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetvendorDetails> GetvendorDetails(string SearchText)
        {
            List<GetvendorDetails> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetvendorDetails(SearchText).ToList();
            }
            return llstDiagDetails;
        }

        public string DeleteVendor(Int64 argvendorID, bool IsActive, Int64 UpdatedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteVendor(argvendorID, IsActive, UpdatedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }


        public string InsertUser(BALUser argclsUser, Int64 argintCreatedBy)
        {
            string lstrMessage = string.Empty;
            List<BALUser> llstUserDetails = new List<BALUser>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                // System.Data.Objects.ObjectParameter RoleUserID = new System.Data.Objects.ObjectParameter("out", typeof(string));
                ObjectParameter RUserID = new ObjectParameter("RoleUserID", typeof(Int64));
                linecount = dbo.InsertUpdateUserDetails(argclsUser.UserID, argclsUser.FirstName, argclsUser.LastName, argclsUser.UserName, argclsUser.Password, argclsUser.Email, argclsUser.Phone, argclsUser.Xtn, argclsUser.CorporateID, argclsUser.FacilityName, argclsUser.UserRoleID, argclsUser.IsActive, argintCreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }



        public List<BindExistuser> BindExistuser()
        {
            List<BindExistuser> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.BindExistuser().ToList();
            }
            return llstBindUserDetails;
        }

        //public List<BALUser> GetExistUsers()
        //{
        //    List<BALUser> llstBindUserDetails = new List<BALUser>();
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        System.Data.Entity.Core.Objects.ObjectParameter lcount = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));
        //        List<BindExistuser> llstUserDetails = dbo.BindExistuser().ToList();
        //        foreach (BindExistuser UserItem in llstUserDetails)
        //        {
        //            BALUser lclsUserDetails = new BALUser();
        //            lclsUserDetails.UserID = Convert.ToInt64(UserItem.UserID);
        //            lclsUserDetails.UserName = UserItem.UserName;
        //            llstBindUserDetails.Add(lclsUserDetails);
        //        }
        //    }
        //    return llstBindUserDetails;
        //}

        public List<BALUser> GetUserRole()
        {
            List<BALUser> llstBindUserDetails = new List<BALUser>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter lcount = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));
                List<BindUserRole> llstUserDetails = dbo.BindUserRole().ToList();
                foreach (BindUserRole UserItem in llstUserDetails)
                {
                    BALUser lclsUserDetails = new BALUser();
                    lclsUserDetails.UserRoleID = Convert.ToInt64(UserItem.UserRoleID);
                    lclsUserDetails.UserRole = UserItem.UserRole;
                    llstBindUserDetails.Add(lclsUserDetails);
                }
            }
            return llstBindUserDetails;
        }
        public List<GetUserDetails> GetUserDetails(Int64 UserID)
        {
            List<GetUserDetails> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.GetUserDetails(UserID).ToList();
            }
            return llstBindUserDetails;
        }

        public List<GetUserRoleandFacility> GetRoleandFacility(Int64 UserID)
        {
            List<GetUserRoleandFacility> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.GetUserRoleandFacility(UserID).ToList();
            }
            return llstBindUserDetails;
        }
        public string RemoveFacilityRole(Int64 arguserRoleID)
        {
            //BALUserDetails lclsDeleteUser = new BALUserDetails();
            //return lclsDeleteUser.DeleteUser(argintUserID, argintDeletedBy);
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.RemoveuserRole(arguserRoleID);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }

        public List<BindPermission> BindPermission()
        {
            List<BindPermission> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.BindPermission().ToList();
            }
            return llstBindUserDetails;
        }

        public List<GetPermission> GetPermission(Int64 MainmenuID, Int64 SubmenuID)
        {
            List<GetPermission> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.GetPermission(MainmenuID, SubmenuID).ToList();
            }
            return llstBindUserDetails;
        }
        public List<GetdrpMainMenu> GetdrpMainMenu()
        {
            List<GetdrpMainMenu> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetdrpMainMenu().ToList();
            }
            return llstDiagDetails;
        }
        public List<GetdrpSubMenu> GetdrpSubMenu(Int64 MainmenuID)
        {
            List<GetdrpSubMenu> llstSubMenuDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSubMenuDetails = dbo.GetdrpSubMenu(MainmenuID).ToList();
            }
            return llstSubMenuDetails;
        }
        public string InsertUpdatePermission(BALUser argclspermission)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                //linecount = dbo.InsertUpdatePermission(argclspermission.PermissionID, argclspermission.UserRoleID, argclspermission.MenuID, argclspermission.SubMenuId, argclspermission.PageName, argclspermission.IsActive, argclspermission.IsEdit, argclspermission.IsView,argclspermission.IsEmailNotification,argclspermission.CreatedBy);
                if (argclspermission.IsEdit == true)
                {
                    linecount = dbo.InsertUpdatePermission(argclspermission.PermissionID, argclspermission.UserRoleID, argclspermission.MenuID, argclspermission.SubMenuId, argclspermission.PageName, argclspermission.IsActive, argclspermission.IsEdit, true, argclspermission.IsEmailNotification, argclspermission.CreatedBy);
                }
                else
                {
                    linecount = dbo.InsertUpdatePermission(argclspermission.PermissionID, argclspermission.UserRoleID, argclspermission.MenuID, argclspermission.SubMenuId, argclspermission.PageName, argclspermission.IsActive, argclspermission.IsEdit, argclspermission.IsView, argclspermission.IsEmailNotification, argclspermission.CreatedBy);
                }
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public List<GetFacility> GetFacility()
        {
            List<GetFacility> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetFacility().ToList();
            }
            return llstDiagDetails;
        }

        public string InsertFacility(BALFacility argclsFacility, Int64 argintCreatedBy)
        {
            string lstrMessage = string.Empty;
            List<BALFacility> llstFacilityDetails = new List<BALFacility>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateFacility(argclsFacility.FacilityID, argclsFacility.FacilityShortName, argclsFacility.FacilityDescription, argclsFacility.Address1, argclsFacility.Address2, argclsFacility.City, argclsFacility.State, argclsFacility.Zipcode, argclsFacility.Phone, argclsFacility.Fax, argclsFacility.BillAddress1, argclsFacility.BillAddress2, argclsFacility.BillCity, argclsFacility.BillState, argclsFacility.BillZipCode, argclsFacility.BillPhone, argclsFacility.BillFax, argclsFacility.Xtn, argclsFacility.BillXtn, argclsFacility.FCorporate, argclsFacility.GPAccountCode, argclsFacility.EMRCode, argclsFacility.TechPerson, argclsFacility.TechPhone, argclsFacility.TechEmail, argclsFacility.AdminPerson, argclsFacility.AdminPhone, argclsFacility.AdminEmail, argclsFacility.PatientCensus, argclsFacility.EmployeeCensus, argclsFacility.TxWeek, argclsFacility.CreatedBy, argclsFacility.LastModifiededBy, argclsFacility.CopyFacilityID, argclsFacility.IsActive);

                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<BindSearchFacility> BindSearchFacility(string SearchFacility)
        {
            List<BindSearchFacility> llstbinditem = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbinditem = dbo.BindSearchFacility(SearchFacility).ToList();
            }
            return llstbinditem;
        }

        public List<BindFacility> BindFacility(BALFacility argFacility)
        {
            List<BindFacility> llstbindfac = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindfac = dbo.BindFacility(argFacility.FacilityShortName, argFacility.FacilityDescription, argFacility.Active, argFacility.LogginBy, argFacility.Filter).ToList();
            }
            return llstbindfac;
        }
        public string DeleteFacility(Int64 argclsFacilityID, Int64 argclsUserID, Boolean IsActive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteFacility(argclsFacilityID, argclsUserID, IsActive);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }

            }
            return lstrMessage;
        }


        public List<GetFacilityItemMap> GetFacilityItemMap()
        {
            List<GetFacilityItemMap> llstfacilityitemmap = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstfacilityitemmap = dbo.GetFacilityItemMap().ToList();
            }
            return llstfacilityitemmap;
        }

        public string DeleteFacilityItemMap(Int64 argclsFacilityItemID, Int64 argclsUserID, Boolean IsActive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteFacilityItemMap(argclsFacilityItemID, argclsUserID, IsActive);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }

            }
            return lstrMessage;
        }

        public string InsertUpdateFacilityItemMap(BALFaItemMap argclsFacility)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateFacilityItemMap(argclsFacility.FacilityItemMapID, argclsFacility.FacilityID, argclsFacility.ItemID, argclsFacility.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetItemsbyFacilityID> GetItemsbyFacilityID(Int64 FacilityID)
        {
            List<GetItemsbyFacilityID> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetItemsbyFacilityID(FacilityID).ToList();
            }
            return llstDiagDetails;
        }

        public List<GetFacilityShortName> GetFacilityShortName()
        {
            List<GetFacilityShortName> llstfacshort = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstfacshort = dbo.GetFacilityShortName().ToList();
            }
            return llstfacshort;
        }


        public List<GetState> GetState()
        {
            List<GetState> llstDiagDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDiagDetails = dbo.GetState().ToList();
            }
            return llstDiagDetails;
        }


        public List<IsUserExist> GetExistUser(string UserName)
        {
            List<IsUserExist> llstBindUserDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstBindUserDetails = dbo.IsUserExist(UserName).ToList();
            }
            return llstBindUserDetails;
        }

        public List<GetItemCategory> GetItemCategory()
        {
            List<GetItemCategory> llstproductgroup = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstproductgroup = dbo.GetItemCategory().ToList();
            }
            return llstproductgroup;
        }

        public List<SearchItemCategory> SearchItemCategory(BALPGroup argpgroup)
        {
            List<SearchItemCategory> llstproductgroup = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstproductgroup = dbo.SearchItemCategory(argpgroup.SearchItem, argpgroup.IsStrActive, argpgroup.LoggedinBy).ToList();
            }
            return llstproductgroup;
        }

        public string InsertUpdateCategory(BALPGroup argpgname)
        {
            string lstrMessage = string.Empty;
            List<BALPGroup> llstPgroup = new List<BALPGroup>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateCategory(argpgname.CategoryID, argpgname.CategoryName, argpgname.Usage, argpgname.CreatedBy, DateTime.Now, argpgname.LastModifiedBy, DateTime.Now, argpgname.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public List<ddlLoadValues> ddlLoadValues(Int64 TableID)
        {
            List<ddlLoadValues> llstDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstDetails = dbo.ddlLoadValues(TableID).ToList();
            }
            return llstDetails;
        }

        public string DeleteItemCategory(Int64 CategoryID, Int64 UserID, bool IsActive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteItemCategory(CategoryID, UserID, IsActive);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }


        public List<GetFacilityVendorAccount> GetFacilityVendorAccount(string SearchItem)
        {
            List<GetFacilityVendorAccount> llstfacilityitemmap = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstfacilityitemmap = dbo.GetFacilityVendorAccount(SearchItem).ToList();
            }
            return llstfacilityitemmap;
        }

        public string DeleteFacilityVendorAccount(Int64 argclsFacilityVendorAccID, Int64 argclsUserID, Boolean IsActive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteFacilityVendorAccount(argclsFacilityVendorAccID, argclsUserID, IsActive);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }

            }
            return lstrMessage;
        }

        public string InsertUpdateFacilityVendorAccount(BALFacilityVendorAccount argclsFacilityVendorAcc)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateFacilityVendorAccount(argclsFacilityVendorAcc.FacilityVendorAccID, argclsFacilityVendorAcc.FacilityID, argclsFacilityVendorAcc.VendorID, argclsFacilityVendorAcc.ShipAccount, argclsFacilityVendorAcc.BillAccount, argclsFacilityVendorAcc.CreatedBy, argclsFacilityVendorAcc.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<ddlCurrency> ddlCurrency()
        {
            List<ddlCurrency> llstCurrency = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCurrency = dbo.ddlCurrency().ToList();
            }
            return llstCurrency;
        }


        public List<BindItem> binditem(string SearchItem)
        {
            List<BindItem> llstbinditem = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbinditem = dbo.BindItem(SearchItem).ToList();
            }
            return llstbinditem;
        }

        public string InsertUpdateItem(BALItem argitem)
        {
            string lstrMessage = string.Empty;
            List<BALItem> llstItem = new List<BALItem>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateItem(argitem.ItemID, argitem.ItemShortName, argitem.CategoryID, argitem.ItemDescription, argitem.UOM, argitem.QtyPack, argitem.UnitPriceCurrency, argitem.UnitPriceValue, argitem.EachPrice, argitem.Standard, argitem.NonStandard, argitem.GPBillingCode, argitem.NDC, argitem.CreatedBy, argitem.LastModifiedBy, argitem.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string DeleteItem(Int64 ItemID, bool IsActive, Int64 LastModifiedBy)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteItem(ItemID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }

        public string InsertItemMap(BALItemMap argitemmap)
        {
            string lstrMessage = string.Empty;
            List<BALItemMap> llstItem = new List<BALItemMap>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertItemMap(argitemmap.ItemMapID, argitemmap.ItemID, argitemmap.ItemCategory, argitemmap.VendorID, argitemmap.VendorItemID, argitemmap.CreatedBy, DateTime.Now, argitemmap.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        //public string InsertCategory(BALItem argcat)
        //{
        //    string lstrMessage = string.Empty;
        //    List<BALItem> llstCategory = new List<BALItem>();
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        int linecount = 0;
        //        linecount = dbo.InsertCategory(argcat.CategoryName);
        //        if (linecount > 0)
        //        {
        //            lstrMessage = "Saved Successfully";
        //        }
        //        else
        //        {
        //            lstrMessage = "Error:Server issue";
        //        }

        //    }
        //    return lstrMessage;
        //}

       
        public List<GetGPBilling> GetGPBilling()
        {
            List<GetGPBilling> llstGp = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstGp = dbo.GetGPBilling().ToList();
            }
            return llstGp;
        }

        public string DeleteGPBilling(Int64 GPBillingID)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteGPBilling(GPBillingID);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }

        public string InsertGPBilling(BALGPBill arggpb)
        {
            string lstrMessage = string.Empty;
            List<BALUom> llstgp = new List<BALUom>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertGPBilling(arggpb.GPBillingID, arggpb.GPBillingCode, arggpb.CreatedBy, DateTime.Now, arggpb.LastModifiedBy, DateTime.Now);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string InsertCurrency(BALItem argcur)
        {
            string lstrMessage = string.Empty;
            List<BALItem> llstCurrency = new List<BALItem>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertCurrency(argcur.CurrencyName);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string DeleteItemMapping(long ItemMappID, bool IsActive, long LastModifiedBy)
        {
            string lstrMessage = string.Empty;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;

                linecount = dbo.DeleteItemMapping(ItemMappID, IsActive, LastModifiedBy);

                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetItemMap> GetItemMap()
        {
            List<GetItemMap> listItem = new List<GetItemMap>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetItemMap("").ToList();
            }

            return listItem;
        }

        public List<GetItemMapping> GetItemMapping(Int64 ItemID)
        {
            List<GetItemMapping> listItem = new List<GetItemMapping>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetItemMapping(ItemID).ToList();
            }

            return listItem;
        }

        public List<GetItemDRD> GetItemDRD()
        {
            List<GetItemDRD> listItem = new List<GetItemDRD>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetItemDRD().ToList();
            }

            return listItem;
        }

        public List<GetDrpItemsByCategory> GetDrpItemsByCategory(Int64 categoryID)
        {
            List<GetDrpItemsByCategory> listItem = new List<GetDrpItemsByCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetDrpItemsByCategory(categoryID).ToList();
            }

            return listItem;
        }

        public List<BALUser> GetCorporateMaster()
        {
            List<BALUser> llstBindUserDetails = new List<BALUser>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter lcount = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));
                List<GetCorporate> llstUserDetails = dbo.GetCorporate().ToList();
                foreach (GetCorporate UserItem in llstUserDetails)
                {
                    BALUser lclsUserDetails = new BALUser();
                    lclsUserDetails.CorporateID = Convert.ToInt64(UserItem.CorporateID);
                    lclsUserDetails.CorporateName = UserItem.CorporateName;
                    llstBindUserDetails.Add(lclsUserDetails);
                }
            }
            return llstBindUserDetails;
        }


        public List<BALUser> GetCorporateFacilityByUserID(Int64 UserID)
        {
            List<BALUser> llstBindUserDetails = new List<BALUser>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter lcount = new System.Data.Entity.Core.Objects.ObjectParameter("Count", typeof(int));
                List<GetCorporateFacilityByUserID> llstUserDetails = dbo.GetCorporateFacilityByUserID(UserID).ToList();
                foreach (GetCorporateFacilityByUserID UserItem in llstUserDetails)
                {
                    BALUser lclsUserDetails = new BALUser();
                    lclsUserDetails.CorporateID = Convert.ToInt64(UserItem.CorporateID);
                    lclsUserDetails.CorporateName = UserItem.CorporateName;
                    lclsUserDetails.FacilityID = Convert.ToInt64(UserItem.FacilityID);
                    lclsUserDetails.FacilityName = UserItem.FacilityName;
                    llstBindUserDetails.Add(lclsUserDetails);
                }
            }

            return llstBindUserDetails;
        }



        public string InsertRole(BALUser argrole)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                // System.Data.Objects.ObjectParameter RoleUserID = new System.Data.Objects.ObjectParameter("out", typeof(string));
                ObjectParameter RUserID = new ObjectParameter("RoleUserID", typeof(Int64));
                linecount = dbo.InsertRole(argrole.RoleName, argrole.MenuID, argrole.SubMenuId, argrole.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetCorporateFacility> GetCorporateFacility(Int64 CorporateID)
        {
            List<GetCorporateFacility> llstLoginDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstLoginDetails = dbo.GetCorporateFacility(CorporateID).ToList();
            }
            return llstLoginDetails;
        }

        public List<UserFacilityRole> GetFacilityRole()
        {
            InventoryEntities db = new InventoryEntities();
            var listitemsrecord = (from x in db.UserFacilityRoles select x).ToList<UserFacilityRole>();
            return listitemsrecord;
        }
        public string InsertUpdateVendorOrderDue(BALVendorOrderDue argclsvendorOrderDue)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateVendorOrderDue(argclsvendorOrderDue.VendorOrderdueID, argclsvendorOrderDue.CorporateID, argclsvendorOrderDue.FacilityID, argclsvendorOrderDue.VendorID, argclsvendorOrderDue.OrderType, argclsvendorOrderDue.OrderDueDate, argclsvendorOrderDue.DeliveryDate, argclsvendorOrderDue.DeliveryWindow, argclsvendorOrderDue.DaytoToNotify, argclsvendorOrderDue.CreatedBy, argclsvendorOrderDue.LastModifitedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<ValidateVendorOrderType> ValidateVendorOrderType(BALVendorOrderDue argclsvendorOrderDue)
        {
            List<ValidateVendorOrderType> llstVendorOrderType = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorOrderType = dbo.ValidateVendorOrderType(argclsvendorOrderDue.FacilityID, argclsvendorOrderDue.VendorID, argclsvendorOrderDue.OrderDueDate).ToList();
            }
            return llstVendorOrderType;
        }
        //public List<BindVendorOrderDue> BindVendorOrderDue()
        //{
        //    List<BindVendorOrderDue> llstVendorOrderDueDetails = null;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstVendorOrderDueDetails = dbo.BindVendorOrderDue().ToList();
        //    }
        //    return llstVendorOrderDueDetails;
        //}
        //Not Use [check sp]
        public List<BindVendorOrderDue> BindVendorOrderDue(BALFacilitySupply argclsvendorOrderDue)
        {
            List<BindVendorOrderDue> llstVendorOrderDueDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorOrderDueDetails = dbo.BindVendorOrderDue(argclsvendorOrderDue.VendorID, argclsvendorOrderDue.FacilityID).ToList();
            }
            return llstVendorOrderDueDetails;
        }


        public string InsertUpdateFacilitySupply(BALFacilitySupply argclsFacilitySupply)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUpdateFacilitySupply(argclsFacilitySupply.FacilitySupplyID, argclsFacilitySupply.CorporateID, argclsFacilitySupply.FacilityID, argclsFacilitySupply.VendorID, argclsFacilitySupply.ItemCategory, argclsFacilitySupply.ItemID, argclsFacilitySupply.Census, argclsFacilitySupply.IsEmp, argclsFacilitySupply.IsPatient, argclsFacilitySupply.Isboth, argclsFacilitySupply.Isothers, argclsFacilitySupply.Factor, argclsFacilitySupply.VendorOrderDate, argclsFacilitySupply.Parlevel, argclsFacilitySupply.IsActive, argclsFacilitySupply.CreatedBy, argclsFacilitySupply.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public List<GetFacilitySupply> GetFacilitySupply(BALFacilitySupply argclsFacilitySupply)
        {
            List<GetFacilitySupply> llstFacilitySupply = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstFacilitySupply = dbo.GetFacilitySupply(argclsFacilitySupply.FacilityID, argclsFacilitySupply.VendorID).ToList();
            }
            return llstFacilitySupply;
        }



        public List<GetFacilitySupplyGird> GetFacilitySupplyGird(BALFacilitySupply argclsFacilitySupply)
        {
            List<GetFacilitySupplyGird> llstFacilitySupply = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstFacilitySupply = dbo.GetFacilitySupplyGird(argclsFacilitySupply.CorporateID, argclsFacilitySupply.FacilityID, argclsFacilitySupply.ListVendorID, argclsFacilitySupply.ListItemCategory, argclsFacilitySupply.VendorOrderDate).ToList();
            }
            return llstFacilitySupply;
        }
        public List<GetEquipmentCategory> GetEquipmentCategory(Int64 CorporateID, string Mode)
        {
            List<GetEquipmentCategory> listItem = new List<GetEquipmentCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetEquipmentCategory(CorporateID, Mode).ToList();
            }

            return listItem;
        }
        public List<GetEquipementList> GetEquipementList(Int64 EquipSubCategoryID, string Mode)
        {
            List<GetEquipementList> listItem = new List<GetEquipementList>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetEquipementList(EquipSubCategoryID, Mode).ToList();
            }

            return listItem;
        }


        public string InsertMachineMaster(BALMachinemaster argclsmachinemaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMachineMaster(argclsmachinemaster.FacilityID, argclsmachinemaster.Manufacturer, argclsmachinemaster.Manufacturedyear, argclsmachinemaster.Hoursonthemachine, argclsmachinemaster.EquipementCategoryID, argclsmachinemaster.EquipementSubCategoryID, argclsmachinemaster.EquipementListID, argclsmachinemaster.Model, argclsmachinemaster.SerNo, argclsmachinemaster.Warranty, argclsmachinemaster.GpAccountCode, argclsmachinemaster.CreatedBy, argclsmachinemaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string UpdateMachineMaster(BALMachinemaster argclsmachinemaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMachineMaster(argclsmachinemaster.MachineID, argclsmachinemaster.FacilityID, argclsmachinemaster.Manufacturer, argclsmachinemaster.Manufacturedyear, argclsmachinemaster.Hoursonthemachine, argclsmachinemaster.EquipementCategoryID, argclsmachinemaster.EquipementSubCategoryID, argclsmachinemaster.EquipementListID, argclsmachinemaster.Model, argclsmachinemaster.SerNo, argclsmachinemaster.Warranty, argclsmachinemaster.GpAccountCode, argclsmachinemaster.CreatedBy, argclsmachinemaster.LastModifiedBy, argclsmachinemaster.IsActive);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMachineMasterbasedMachineID> GetMachinemasterbasedmachineID(long MachineID)
        {
            List<GetMachineMasterbasedMachineID> listItem = new List<GetMachineMasterbasedMachineID>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetMachineMasterbasedMachineID(MachineID).ToList();
            }

            return listItem;
        }


        public List<object> InsertEquipmentCategory(BALMachinemaster argclsmachinemaster)
        {
            List<object> lstrMessage = new List<object>();

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("EquipmentCategoryMasterID", typeof(Int64));
                linecount = dbo.InsertEquipmentCategory(argclsmachinemaster.CorporateID, argclsmachinemaster.EquipementCategorydesc, argclsmachinemaster.CreatedBy, outputParameter);
                if (linecount > 0)
                {
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, outputParameter.Value.ToString());
                }
                else
                {
                    lstrMessage.Insert(0, "Error:Server issue");
                }

            }
            return lstrMessage;
        }
        public string UpdateEquipmentcategory(BALMachinemaster argclsmachinemaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateEquipmentcategory(argclsmachinemaster.EquipementCategoryID, argclsmachinemaster.EquipementCategorydesc, argclsmachinemaster.CreatedBy, argclsmachinemaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string InsertEquipmentList(BALMachinemaster argclsmachinemaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertEquipmentList(argclsmachinemaster.EquipementSubCategoryID, argclsmachinemaster.EquipementListdesc, argclsmachinemaster.CreatedBy, argclsmachinemaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateEquipmentList(BALMachinemaster argclsmachinemaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateEquipmentList(argclsmachinemaster.EquipementListID, argclsmachinemaster.EquipementSubCategoryID, argclsmachinemaster.EquipementListdesc, argclsmachinemaster.CreatedBy, argclsmachinemaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMachinemasterDetails> GetMachinemasterDetails()
        {
            List<GetMachinemasterDetails> listItem = new List<GetMachinemasterDetails>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetMachinemasterDetails().ToList();
            }

            return listItem;
        }

        public List<SearchMachinemasterdetails> SearchMachinemasterdetails(BALMachinemaster argstrmach)
        {
            List<SearchMachinemasterdetails> listItem = new List<SearchMachinemasterdetails>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.SearchMachinemasterdetails(argstrmach.FacilityID, argstrmach.IstrActive, argstrmach.LastModifiedBy, argstrmach.Filter).ToList();
            }

            return listItem;
        }
        public string DeleteMachinemasterDetails(Int64 argmachineID, bool IsActive, Int64 UpdatedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteMachinemasterDetails(argmachineID, IsActive, UpdatedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public string DeleteEquipeCategoryMaster(Int64 argCategoryID, bool IsActive, Int64 UpdatedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteEquipeCategoryMaster(argCategoryID, IsActive, UpdatedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public string DeleteEquipListMaster(Int64 argListID, bool IsActive, Int64 UpdatedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteEquipListMaster(argListID, IsActive, UpdatedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<CheckEquipmentlist> GetCheckEquipmentlist(Int64 EquipcategoryID)
        {
            List<CheckEquipmentlist> listItem = new List<CheckEquipmentlist>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.CheckEquipmentlist(EquipcategoryID).ToList();
            }

            return listItem;
        }
        public List<SavedEquipmentCategory> SavedEquipmentCategory(Int64 CorporateID, Int64 EquipcategoryID)
        {
            List<SavedEquipmentCategory> listItem = new List<SavedEquipmentCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.SavedEquipmentCategory(CorporateID, EquipcategoryID).ToList();
            }

            return listItem;
        }
        public List<SavedEquipmentList> SavedEquipmentList(Int64 EquipmentListID)
        {
            List<SavedEquipmentList> listItem = new List<SavedEquipmentList>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.SavedEquipmentList(EquipmentListID).ToList();
            }

            return listItem;
        }
        public List<GetActiveEquipementListvalue> GetActiveEquipementListvalue(Int64 EquiplistID)
        {
            List<GetActiveEquipementListvalue> listItem = new List<GetActiveEquipementListvalue>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetActiveEquipementListvalue(EquiplistID).ToList();
            }

            return listItem;
        }

        // Machine Parts Request Page
        #region facility dropdown SelectedIndexChanged event

        public List<BindMachinePartsReport> BindMachinePartsReport(string MPRMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindMachinePartsReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMachinePartsReport(MPRMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }

        public List<GetMPRMaster> GetMPRMaster()
        {
            List<GetMPRMaster> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.GetMPRMaster().ToList();
            }
            return llstMPRMaster;
        }

        //public BALMPRMaster GetMPRDetailsbyMPRMasterID(Int64 MPRMasterID, Int64 LockedBy)
        //{
        //    List<GetMPRDetailsbyMPRMasterID> llstMPRDetails = null;
        //    var OutputLockParameter = new ObjectParameter("LockMessage", typeof(Int64));

        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstMPRDetails = dbo.GetMPRDetailsbyMPRMasterID(MPRMasterID, LockedBy, OutputLockParameter).ToList();
        //    }
        //    BALMPRMaster llstMPdetailsWithErrorMessage = new BALMPRMaster();
        //    llstMPdetailsWithErrorMessage.MPDetailsList = llstMPRDetails;
        //    llstMPdetailsWithErrorMessage.ErrorMsg = OutputLockParameter.Value.ToString();
        //    return llstMPdetailsWithErrorMessage;
        //}

        public List<GetMPRDetailsbyMPRMasterID> GetMPRDetailsbyMPRMasterID(Int64 MPRMasterID, Int64 LockedBy, Int64 LockTimeOut)
        {
            List<GetMPRDetailsbyMPRMasterID> llstMPRDetails = null;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetMPRDetailsbyMPRMasterID(MPRMasterID, LockedBy, LockTimeOut).ToList();
            }

            return llstMPRDetails;
        }

        public List<GetList> GetList(string ScreenName, string ListName, string Mode)
        {
            List<GetList> llstLookUpList = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstLookUpList = dbo.GetList(ScreenName, ListName, Mode).ToList();
            }
            return llstLookUpList;
        }


        public List<SearchMPRMaster> SearchMPRMaster(BALMPRMaster argclsMPRMaster)
        {
            List<SearchMPRMaster> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchMPRMaster(argclsMPRMaster.CorporateName, argclsMPRMaster.FacilityName, argclsMPRMaster.VendorName, argclsMPRMaster.DateFrom, argclsMPRMaster.DateTo, argclsMPRMaster.Status, argclsMPRMaster.loggedinBy).ToList();
            }
            return llstMPRMaster;
        }

        public List<object> InsertMPRMaster(BALMPRMaster argInsMPRMaster)
        {
            List<object> lstrMessage = new List<object>();

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("MPRMasterID", typeof(Int64));
                linecount = dbo.InsertMPRMaster(argInsMPRMaster.CorporateID, argInsMPRMaster.FacilityID, argInsMPRMaster.VendorID, argInsMPRMaster.EquipmentCategoryID, argInsMPRMaster.EquipementSubCategoryID, argInsMPRMaster.EquipementListID, argInsMPRMaster.SerialNo, argInsMPRMaster.Shipping, argInsMPRMaster.Hoursonthemachine, argInsMPRMaster.ShippingCost, argInsMPRMaster.Tax, argInsMPRMaster.TotalCost, argInsMPRMaster.CreatedBy, outputParameter);

                if (linecount > 0)
                {
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, outputParameter.Value.ToString());
                }
                else
                {
                    lstrMessage.Insert(0, "Error:Server issue");
                }

            }
            return lstrMessage;
        }

        public string InsertMPRDetails(BALMPRMaster argInsMPRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMPRDetails(argInsMPRDetails.MPRMasterID, argInsMPRDetails.ItemID, argInsMPRDetails.ItemDescription, argInsMPRDetails.UOM, argInsMPRDetails.PricePerUnit, argInsMPRDetails.OrderQuantity, argInsMPRDetails.TotalPrice, argInsMPRDetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateMPRMaster(BALMPRMaster argupdMPRMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMPRMaster(argupdMPRMaster.MPRMasterID, argupdMPRMaster.CorporateID, argupdMPRMaster.FacilityID, argupdMPRMaster.VendorID, argupdMPRMaster.EquipmentCategoryID, argupdMPRMaster.EquipementSubCategoryID, argupdMPRMaster.EquipementListID, argupdMPRMaster.SerialNo, argupdMPRMaster.Shipping, argupdMPRMaster.Hoursonthemachine, argupdMPRMaster.ShippingCost, argupdMPRMaster.Tax, argupdMPRMaster.TotalCost, argupdMPRMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateMPRDetails(BALMPRMaster argUpdMPRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMPRDetails(argUpdMPRDetails.MPRMasterID, argUpdMPRDetails.MPRDetailID, argUpdMPRDetails.ItemID, argUpdMPRDetails.ItemDescription, argUpdMPRDetails.UOM, argUpdMPRDetails.PricePerUnit, argUpdMPRDetails.OrderQuantity, argUpdMPRDetails.TotalPrice, argUpdMPRDetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string DeleteMPRDetails(Int64 argMPRDetailsID, bool IsActive, Int64 LastModifiedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteMPRDetails(argMPRDetailsID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }

        public List<ValidMachineEquipment> ValidMachineEquipment(Int64 EquipmentCategory, Int64 EquipementSubcategory, Int64 EquipmentList, Int64 FacilityID)
        {
            List<ValidMachineEquipment> listItem = new List<ValidMachineEquipment>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.ValidMachineEquipment(EquipmentCategory, EquipementSubcategory, EquipmentList, FacilityID).ToList();
            }

            return listItem;
        }

        #endregion

        // Medical Supplies Request Page
        #region Medical Supplies Request Function
        public List<GetmedicalsupplyReviewReport> GetmedicalsupplyReviewReport(string MedicalSupplyMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<GetmedicalsupplyReviewReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetmedicalsupplyReport(MedicalSupplyMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }


        public List<GetMedicalsuppliesItem> GetMedicalsuppliesItem(Int64 CorporateID, Int64 FacilityID, Int64 VendorID)
        {
            List<GetMedicalsuppliesItem> listItem = new List<GetMedicalsuppliesItem>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetMedicalsuppliesItem(CorporateID, FacilityID, VendorID).ToList();
            }

            return listItem;
        }
        public string InsertShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertShipping(argclsmedicalsuppliesrequest.InvenValue, argclsmedicalsuppliesrequest.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateShipping(argclsmedicalsuppliesrequest.InvenValue, argclsmedicalsuppliesrequest.Updatekeyvalue, argclsmedicalsuppliesrequest.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string DeleteShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteShipping(argclsmedicalsuppliesrequest.DeletedBy, argclsmedicalsuppliesrequest.InvenValue);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string InsertTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertTimeDelivery(argclsmedicalsuppliesrequest.InvenValue, argclsmedicalsuppliesrequest.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateTimeDelivery(argclsmedicalsuppliesrequest.InvenValue, argclsmedicalsuppliesrequest.Updatekeyvalue, argclsmedicalsuppliesrequest.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string DeleteTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteTimeDelivery(argclsmedicalsuppliesrequest.DeletedBy, argclsmedicalsuppliesrequest.InvenValue);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetLookUpList> GetLookUpList(Int64 LookUpID)
        {
            List<GetLookUpList> llstLookUpList = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstLookUpList = dbo.GetLookUpList(LookUpID).ToList();
            }
            return llstLookUpList;
        }
        public List<GetOrderPeriod> GetOrderPeriod(Int64 CorporateID, Int64 FacilityID, Int64 VendorID, Int32 Ordertype)
        {
            List<GetOrderPeriod> listItem = new List<GetOrderPeriod>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetOrderPeriod(CorporateID, FacilityID, VendorID, Ordertype).ToList();
            }

            return listItem;
        }
        public Int64 InsertMedicalsupplyMaster(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            Int64 MasterID = 0;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var OutputParameter = new ObjectParameter("MedicalSupplyMasterID", typeof(Int64));
                linecount = dbo.InsertMedicalsuppliesMaster(argclsmedicalsuppliesrequest.CorporateID, argclsmedicalsuppliesrequest.FacilityID, argclsmedicalsuppliesrequest.Vendor, argclsmedicalsuppliesrequest.OrderType, argclsmedicalsuppliesrequest.OrderPeriod, argclsmedicalsuppliesrequest.Shipping, argclsmedicalsuppliesrequest.TimeDelivery, argclsmedicalsuppliesrequest.Remarks, argclsmedicalsuppliesrequest.CreatedBy, argclsmedicalsuppliesrequest.LastModifiedBy, OutputParameter);
                BALMedicalSuppliesRequest llstMSR = new BALMedicalSuppliesRequest();
                MasterID = Convert.ToInt64(OutputParameter.Value);

                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return MasterID;

        }
        public string InsertMedicalSuppliesDetail(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMedicalSuppliesDetail(argclsmedicalsuppliesrequest.MedicalSupplyMasterID, argclsmedicalsuppliesrequest.SNGItemID, argclsmedicalsuppliesrequest.Itemcatgroup, argclsmedicalsuppliesrequest.Itemdescription, argclsmedicalsuppliesrequest.UOM, argclsmedicalsuppliesrequest.QuantityPack,
                    argclsmedicalsuppliesrequest.Parlevel, argclsmedicalsuppliesrequest.QuantityinHand, argclsmedicalsuppliesrequest.OrderQuantity, argclsmedicalsuppliesrequest.Price, argclsmedicalsuppliesrequest.TotalPrice, argclsmedicalsuppliesrequest.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMedicalSupplyRequestMaster> GetMedicalSupplyRequestMaster()
        {
            List<GetMedicalSupplyRequestMaster> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.GetMedicalSupplyRequestMaster().ToList();
            }
            return llstMPRMaster;
        }
        public List<SearchMedicalSupplyRequest> SearchMedicalSupplyRequest(BALMedicalSuppliesRequest argclsMedicalSuppliesReq)
        {
            List<SearchMedicalSupplyRequest> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchMedicalSupplyRequest(argclsMedicalSuppliesReq.CorporateName, argclsMedicalSuppliesReq.FacilityName, argclsMedicalSuppliesReq.VendorName, argclsMedicalSuppliesReq.DateFrom, argclsMedicalSuppliesReq.DateTo, argclsMedicalSuppliesReq.Status, argclsMedicalSuppliesReq.loggedinBy).ToList();
            }
            return llstMPRMaster;
        }
        public List<BindMedicalsupplymasterandDetail> BindMedicalsupplymasterandDetail(Int64 MedicalSupplyMasterID)
        {
            List<BindMedicalsupplymasterandDetail> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMedicalsupplymasterandDetail(MedicalSupplyMasterID).ToList();
            }
            return llstMPRDetails;
        }
        public string UpdateMedicalsupplyMaster(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMedicalsupplyMaster(argclsmedicalsuppliesrequest.MedicalSupplyMasterID, argclsmedicalsuppliesrequest.CorporateID, argclsmedicalsuppliesrequest.FacilityID, argclsmedicalsuppliesrequest.Vendor, argclsmedicalsuppliesrequest.OrderType, argclsmedicalsuppliesrequest.OrderPeriod, argclsmedicalsuppliesrequest.Shipping, argclsmedicalsuppliesrequest.TimeDelivery, argclsmedicalsuppliesrequest.Remarks, argclsmedicalsuppliesrequest.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateMedicalSupplyDetails(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMedicalSupplyDetails(argclsmedicalsuppliesrequest.MedicalSupplyMasterID, argclsmedicalsuppliesrequest.MedicalSupplyDetailID, argclsmedicalsuppliesrequest.SNGItemID, argclsmedicalsuppliesrequest.Itemcatgroup, argclsmedicalsuppliesrequest.Itemdescription, argclsmedicalsuppliesrequest.UOM, argclsmedicalsuppliesrequest.QuantityPack, argclsmedicalsuppliesrequest.Parlevel, argclsmedicalsuppliesrequest.QuantityinHand, argclsmedicalsuppliesrequest.OrderQuantity, argclsmedicalsuppliesrequest.Price, argclsmedicalsuppliesrequest.TotalPrice, argclsmedicalsuppliesrequest.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";

                }
            }
            return lstrMessage;
        }
        public string DeleteMedicalSuppliesDetails(Int64 argMSRID, Int64 UpdatedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.RemoveMedicalSupplyDetail(argMSRID, UpdatedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<AddMedicalsupplyitem> AddMedicalsupplyitem(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest)
        {
            List<AddMedicalsupplyitem> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.AddMedicalsupplyitem(argclsmedicalsuppliesrequest.CorporateID, argclsmedicalsuppliesrequest.FacilityID, argclsmedicalsuppliesrequest.Vendor, argclsmedicalsuppliesrequest.CombineKey).ToList();
            }
            return llstMPRMaster;
        }
        public List<SavedShippingValue> SavedShippingValue()
        {
            List<SavedShippingValue> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SavedShippingValue().ToList();
            }
            return llstMPRMaster;
        }
        public List<SavedTimeDeliveryValue> SavedTimeDeliveryValue()
        {
            List<SavedTimeDeliveryValue> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SavedTimeDeliveryValue().ToList();
            }
            return llstMPRMaster;
        }
        public List<GetNonsuperAdminMedicalSupplyMaster> GetNonsuperAdminMedicalSupplyMaster(Int64 CorporateID, Int64 FacilityID)
        {
            List<GetNonsuperAdminMedicalSupplyMaster> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.GetNonsuperAdminMedicalSupplyMaster(CorporateID, FacilityID).ToList();
            }
            return llstMPRMaster;
        }

        public List<BindMedicalsupplymaster> BindMedicalsupplymaster(Int64 MedicalSupplyMasterID)
        {
            List<BindMedicalsupplymaster> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMedicalsupplymaster(MedicalSupplyMasterID).ToList();
            }
            return llstMPRDetails;
        }
        //public BALMedicalSuppliesRequest BindMedicalsupplyDetail(Int64 MedicalSupplyMasterID, Int64 LockedBy)
        //{
        //    List<BindMedicalsupplyDetail> llstMPRDetails = null;
        //    var OutputLockParameter = new ObjectParameter("LockMessage", typeof(Int64));

        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstMPRDetails = dbo.BindMedicalsupplyDetail(MedicalSupplyMasterID, LockedBy, OutputLockParameter).ToList();
        //    }

        //    BALMedicalSuppliesRequest llstMSdetailsWithErrorMessage = new BALMedicalSuppliesRequest();
        //    llstMSdetailsWithErrorMessage.MSDetailsList = llstMPRDetails;
        //    llstMSdetailsWithErrorMessage.ErrorMsg = OutputLockParameter.Value.ToString();
        //    return llstMSdetailsWithErrorMessage;
        //}

        public List<BindMedicalsupplyDetail> BindMedicalsupplyDetail(Int64 MedicalSupplyMasterID, Int64 LockedBy, Int64 LockTimeOut)
        {
            List<BindMedicalsupplyDetail> llstMSRDetails = null;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDetails = dbo.BindMedicalsupplyDetail(MedicalSupplyMasterID, LockedBy, LockTimeOut).ToList();
            }

            return llstMSRDetails;
        }


        public List<FindDuplicateShippingValue> FindDuplicateShippingValue(string ShippingValue)
        {
            List<FindDuplicateShippingValue> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.FindDuplicateShippingValue(ShippingValue).ToList();
            }
            return llstMPRDetails;
        }
        public List<FindDuplicateTimeDeliveryValue> FindDuplicateTimeDeliveryValue(string TimeDeliveryValue)
        {
            List<FindDuplicateTimeDeliveryValue> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.FindDuplicateTimeDeliveryValue(TimeDeliveryValue).ToList();
            }
            return llstMPRDetails;
        }

        // Validate Medical Supply Item

        public List<ValidateMedicalSuppliesItem> ValidateMedicalSuppliesItem(BALMedicalSuppliesRequest argclsMedicalSup)
        {
            List<ValidateMedicalSuppliesItem> ValMSR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                ValMSR = dbo.ValidateMedicalSuppliesItem(argclsMedicalSup.FacilityID, argclsMedicalSup.Vendor, argclsMedicalSup.SNGItemID, argclsMedicalSup.CreatedBy).ToList();
            }
            return ValMSR;
        }

        #endregion

        // Service Request Page
        #region Service Request Function
        public List<BindServiceRequestReport> BindServiceRequestReport(string ServiceRequestMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindServiceRequestReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindServiceRequestReport(ServiceRequestMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }



        public List<GetServiceRequestMaster> GetServiceRequestMaster()
        {
            List<GetServiceRequestMaster> llstServiceRequestMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstServiceRequestMaster = dbo.GetServiceRequestMaster().ToList();
            }
            return llstServiceRequestMaster;
        }

        //public BALServiceRequest GetServiceRequestetailsbyServiceRequestMasterID(Int64 SRMasterID, Int64 LockedBy)
        //{
        //    List<GetServiceRequestetailsbyServiceRequestMasterID> llstSRDetails = null;

        //    var OutputLockParameter = new ObjectParameter("LockMessage", typeof(Int64));
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstSRDetails = dbo.GetServiceRequestetailsbyServiceRequestMasterID(SRMasterID, LockedBy, OutputLockParameter).ToList();
        //    }

        //    BALServiceRequest llstSRdetailsWithErrorMessage = new BALServiceRequest();
        //    llstSRdetailsWithErrorMessage.SRDetailsList = llstSRDetails;
        //    llstSRdetailsWithErrorMessage.ErrorMsg = OutputLockParameter.Value.ToString();
        //    return llstSRdetailsWithErrorMessage;
        //}

        public List<GetServiceRequestetailsbyServiceRequestMasterID> GetServiceRequestetailsbyServiceRequestMasterID(Int64 SRMasterID, Int64 LockedBy, Int64 LockTimeOut)
        {
            List<GetServiceRequestetailsbyServiceRequestMasterID> llstSRDetails = null;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRDetails = dbo.GetServiceRequestetailsbyServiceRequestMasterID(SRMasterID, LockedBy, LockTimeOut).ToList();
            }


            return llstSRDetails;
        }

        public List<GetServiceCategory> GetServiceCategory(Int64 CorporateID, string Mode)
        {
            List<GetServiceCategory> llstServiceCategory = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstServiceCategory = dbo.GetServiceCategory(CorporateID, Mode).ToList();
            }
            return llstServiceCategory;
        }

        public List<GetServiceList> GetServiceList(Int64 ServiceCategoryID, string Mode)
        {
            List<GetServiceList> llstServiceList = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstServiceList = dbo.GetServiceList(ServiceCategoryID, Mode).ToList();
            }
            return llstServiceList;
        }

        public List<SearchServiceRequestMaster> SearchServiceRequestMaster(BALServiceRequest argclsSRMaster)
        {
            List<SearchServiceRequestMaster> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchServiceRequestMaster(argclsSRMaster.CorporateName, argclsSRMaster.FacilityName, argclsSRMaster.DateFrom, argclsSRMaster.DateTo, argclsSRMaster.Status, argclsSRMaster.LoggedinBy).ToList();
            }
            return llstMPRMaster;
        }

        public List<object> InsertServiceRequestMaster(BALServiceRequest argclsSRMaster)
        {
            List<object> lstrMessage = new List<object>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("SRMasterID", typeof(Int64));
                linecount = dbo.InsertServiceRequestMaster(argclsSRMaster.CorporateID, argclsSRMaster.FacilityID, argclsSRMaster.ServiceType, argclsSRMaster.EquipmentCategoryID, argclsSRMaster.EquipementSubCategoryID, argclsSRMaster.EquipmentListID, argclsSRMaster.ServiceCategoryID, argclsSRMaster.ServiceListID, argclsSRMaster.CreatedBy, outputParameter);
                BALServiceRequest llstServiceRequest = new BALServiceRequest();
                llstServiceRequest.ServiceRequestMasterID = Convert.ToInt64(outputParameter.Value);

                if (linecount > 0)
                {
                    //lstrMessage.Add("Saved Successfully");
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, llstServiceRequest.ServiceRequestMasterID.ToString());
                    //lstrMessage.Add(llstServiceRequest.ServiceRequestMasterID.ToString());
                }
                else
                {
                    lstrMessage.Insert(1, "Error:Server issue");
                    //lstrMessage.Add("Error:Server issue");                   
                }

            }
            return lstrMessage;
        }

        public string InsertServcieRequestDetails(BALServiceRequest argclsSRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServcieRequestDetails(argclsSRDetails.ServiceRequestMasterID, argclsSRDetails.SNo, argclsSRDetails.VendorID, argclsSRDetails.Service, argclsSRDetails.Unit, argclsSRDetails.Price, argclsSRDetails.FileName, argclsSRDetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateServiceRequestMaster(BALServiceRequest argclsSRMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceRequestMaster(argclsSRMaster.ServiceRequestMasterID, argclsSRMaster.CorporateID, argclsSRMaster.FacilityID, argclsSRMaster.ServiceCategoryID, argclsSRMaster.ServiceListID, argclsSRMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateServiceRequestDetails(BALServiceRequest argclsSRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceRequestDetails(argclsSRDetails.ServiceRequestMasterID, argclsSRDetails.ServiceRequestDetailID, argclsSRDetails.SNo, argclsSRDetails.Service, argclsSRDetails.Unit, argclsSRDetails.Price, argclsSRDetails.FileName, argclsSRDetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string DeleteServiceRequestDetails(Int64 argSRDetailsID, bool IsActive, Int64 LastModifiedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteServiceRequestDetails(argSRDetailsID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public string InsertServiceCategoryMaster(BALServiceRequest argclsServiceCategory)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServiceCategoryMaster(argclsServiceCategory.CorporateID, argclsServiceCategory.FacilityID, argclsServiceCategory.ServiceCatDesc, argclsServiceCategory.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateServiceCategoryMaster(BALServiceRequest argclsServiceCategory)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServicecategory(argclsServiceCategory.ServiceCategoryID, argclsServiceCategory.ServiceCatDesc, argclsServiceCategory.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string DeleteServiceCategoryMaster(Int64 argCategoryID, Int64 LastModifiedBy)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteServiceCategoryMaster(argCategoryID, LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string InsertServiceListMaster(BALServiceRequest argclsServiceList)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServiceListMaster(argclsServiceList.ServiceCategoryID, argclsServiceList.ServiceListDesc, argclsServiceList.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateServiceListMaster(BALServiceRequest argclsServiceList)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceList(argclsServiceList.ServiceListID, argclsServiceList.ServiceCategoryID, argclsServiceList.ServiceListDesc, argclsServiceList.ServiceListID);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string DeleteServiceListMaster(Int64 argListID, Int64 LastModifiedBy)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteServiceListMaster(argListID, LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<CheckServicelist> GetCheckServicelist(Int64 ServiceCategoryID)
        {
            List<CheckServicelist> listItem = new List<CheckServicelist>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.CheckServicelist(ServiceCategoryID).ToList();
            }

            return listItem;
        }
        public List<SavedServiceCategory> SavedServiceCategory(Int64 CorporateID, Int64 ServiceCategoryID)
        {
            List<SavedServiceCategory> listItem = new List<SavedServiceCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.SavedServiceCategory(CorporateID, ServiceCategoryID).ToList();
            }

            return listItem;
        }
        public List<SavedServiceList> SavedServiceList(Int64 ServiceListID)
        {
            List<SavedServiceList> listItem = new List<SavedServiceList>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.SavedServiceList(ServiceListID).ToList();
            }

            return listItem;
        }

        public List<GetServiceAttachment> GetServiceAttachment(Int64 ServiceUploadID)
        {
            List<GetServiceAttachment> llstServiceAttachment = new List<GetServiceAttachment>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstServiceAttachment = dbo.GetServiceAttachment(ServiceUploadID).ToList();
            }
            return llstServiceAttachment;
        }

        public string InsertServiceAttachment(BALServiceRequest argclsServiceList)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServiceAttachment(argclsServiceList.ServiceRequestDetailID, argclsServiceList.LocationOfTheFile, argclsServiceList.Description, argclsServiceList.FileName, argclsServiceList.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateServiceAttachment(BALServiceRequest argclsServiceList)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceAttachment(argclsServiceList.ServiceUploadID, argclsServiceList.ServiceRequestDetailID, argclsServiceList.LocationOfTheFile, argclsServiceList.Description, argclsServiceList.FileName, argclsServiceList.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetActiveServiceListvalue> GetActiveServiceListvalue(Int64 ServicelistID)
        {
            List<GetActiveServiceListvalue> listItem = new List<GetActiveServiceListvalue>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetActiveServiceListvalue(ServicelistID).ToList();
            }

            return listItem;
        }
        public string DeleteServiceAttachment(Int64 argListID, Int64 LastModifiedBy)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteServiceAttachment(argListID, LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }



        public string InsertSRTempAttch(BALServiceRequest argclsServiceList)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertSRTempAttch(argclsServiceList.SNo, argclsServiceList.LocationOfTheFile, argclsServiceList.UploadedBy, argclsServiceList.Description, argclsServiceList.FileName, argclsServiceList.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string DeleteSRTempAttch(Int32 SIno, string Mode)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteSRTempAttch(SIno, Mode);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetServiceTempAttachment> GetServiceTempAttachment(Int32 SiNo)
        {
            List<GetServiceTempAttachment> llstServiceAttachment = new List<GetServiceTempAttachment>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstServiceAttachment = dbo.GetServiceTempAttachment(SiNo).ToList();
            }
            return llstServiceAttachment;
        }

        public string TruncateSRTempAttch()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.TruncateSRTempAttch();
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }
            }
            return lstrMessage;
        }

        public string AutoUpdateLockedOut(Int64 MasterID, Int64 LockedOutBy, string ScreenName)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.AutoUpdateLockedOut(MasterID, LockedOutBy, ScreenName);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        #endregion
        //RequestIT Page
        #region  functions of IT Request Page
        public List<object> InsertITRequestMaster(BALRequestIT argInsITRequestMaster)
        {
            List<object> lstrMessage = new List<object>();

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("ITRequestMasterID", typeof(Int64));
                linecount = dbo.InsertITRequestMaster(argInsITRequestMaster.VendorID, argInsITRequestMaster.CorporateID, argInsITRequestMaster.RequestType, argInsITRequestMaster.Shipping, argInsITRequestMaster.FacilityID, argInsITRequestMaster.DateFrom, argInsITRequestMaster.Audittrial, argInsITRequestMaster.Remarks, argInsITRequestMaster.ShippingCost, argInsITRequestMaster.Tax, argInsITRequestMaster.TotalCost, argInsITRequestMaster.CreatedBy, outputParameter);

                if (linecount > 0)
                {
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, outputParameter.Value.ToString());
                }
                else
                {
                    lstrMessage.Insert(0, "Error:Server issue");
                }

            }
            return lstrMessage;
        }

        public string InsertITRequestDetails(BALRequestIT argInsITRequestMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertITRequestDetails(argInsITRequestMaster.RequestITMasterID, argInsITRequestMaster.EquipmentCategoryID, argInsITRequestMaster.EquipementListID, argInsITRequestMaster.SerialNo, argInsITRequestMaster.User, argInsITRequestMaster.PricePerQty, argInsITRequestMaster.Qty, argInsITRequestMaster.TotalPrice, argInsITRequestMaster.Reason, argInsITRequestMaster.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetRequestITMaster> GetRequestITMaster()
        {
            List<GetRequestITMaster> llstITMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstITMaster = dbo.GetRequestITMaster().ToList();
            }
            return llstITMaster;
        }
        public List<GetNonsuperAdminRequestITMaster> GetNonsuperAdminRequestITMaster(Int64 CorporateID, Int64 FacilityID)
        {
            List<GetNonsuperAdminRequestITMaster> llstITMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstITMaster = dbo.GetNonsuperAdminRequestITMaster(CorporateID, FacilityID).ToList();
            }
            return llstITMaster;
        }
        public List<SearchITRequest> SearchITRequest(BALRequestIT argInsITRequestMaster)
        {
            List<SearchITRequest> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchITRequest(argInsITRequestMaster.CorporateName, argInsITRequestMaster.FacilityName, argInsITRequestMaster.VendorName, argInsITRequestMaster.DateFrom, argInsITRequestMaster.DateTo, argInsITRequestMaster.Status, argInsITRequestMaster.loggedinBy, argInsITRequestMaster.RequestType).ToList();
            }
            return llstMPRMaster;
        }
        public List<GetITRequestDetailsbyMasterID> GetITRequestDetailsbyMasterID(Int64 MPRMasterID, Int64 LockedBy, Int64 LockTimeOut)
        {
            List<GetITRequestDetailsbyMasterID> llstMPRDetails = null;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetITRequestDetailsbyMasterID(MPRMasterID, LockedBy, LockTimeOut).ToList();
            }

            return llstMPRDetails;
        }

        public string UpdateITRequestMaster(BALRequestIT argInsITRequestMaster)
        {
            string lstrMessage = string.Empty;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateITRequestMaster(argInsITRequestMaster.RequestITMasterID, argInsITRequestMaster.VendorID, argInsITRequestMaster.CorporateID, argInsITRequestMaster.RequestType, argInsITRequestMaster.Shipping, argInsITRequestMaster.FacilityID, argInsITRequestMaster.DateFrom, argInsITRequestMaster.Audittrial, argInsITRequestMaster.Remarks, argInsITRequestMaster.ShippingCost, argInsITRequestMaster.Tax, argInsITRequestMaster.TotalCost, argInsITRequestMaster.LastModifiedBy);

                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateITRequestDetails(BALRequestIT argInsITRequestMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateITRequestDetails(argInsITRequestMaster.RequestITDetailID, argInsITRequestMaster.RequestITMasterID, argInsITRequestMaster.SINo, argInsITRequestMaster.EquipmentCategoryID, argInsITRequestMaster.EquipementListID, argInsITRequestMaster.SerialNo, argInsITRequestMaster.User, argInsITRequestMaster.PricePerQty, argInsITRequestMaster.Qty, argInsITRequestMaster.TotalPrice, argInsITRequestMaster.Reason, argInsITRequestMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<BindEquipementsubcategoryFORIT> BindEquipementsubcategory(Int64 CorporateID, string Mode)
        {
            List<BindEquipementsubcategoryFORIT> listItem = new List<BindEquipementsubcategoryFORIT>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.BindEquipementsubcategoryFORIT(CorporateID, Mode).ToList();
            }

            return listItem;
        }
        public List<BindEquipementListFORIT> BindEquipementListFORIT(Int64 EquipSubcatID, string Mode)
        {
            List<BindEquipementListFORIT> listItem = new List<BindEquipementListFORIT>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.BindEquipementListFORIT(EquipSubcatID, Mode).ToList();
            }

            return listItem;
        }
        public List<GetSerialNo> GetSerialNo(Int64 EquipmentSubcategoryID, Int64 EquipListID)
        {
            List<GetSerialNo> listItem = new List<GetSerialNo>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetSerialNo(EquipmentSubcategoryID, EquipListID).ToList();
            }

            return listItem;
        }
        public string DeleteITRDetails(Int64 argITRDetailsID, bool IsActive, Int64 LastModifiedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteITRDetails(argITRDetailsID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<BindRequestITPartsReport> BindRequestITPartsReport(string RequestITMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindRequestITPartsReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindRequestITPartsReport(RequestITMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }

        public List<ValidITEquipment> ValidITEquipment(Int64 EquimentSubCategory, Int64 EquipmentList, Int64 FacilityID)
        {
            List<ValidITEquipment> listItem = new List<ValidITEquipment>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.ValidITEquipment(EquimentSubCategory, EquipmentList, FacilityID).ToList();
            }

            return listItem;
        }

        #endregion


        //CapitalItem Page
        #region  functions of CapitalItem Request Page
        public List<BindCapitalItemRequestReport> BindCapitalItemRequestReport(string CapitalItemMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindCapitalItemRequestReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.BindCapitalItemRequestReport(CapitalItemMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstCRDetails;
        }

        public List<GetCapitalItemMaster> GetCapitalItemMaster()
        {
            List<GetCapitalItemMaster> llstCRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRMaster = dbo.GetCapitalItemMaster().ToList();
            }
            return llstCRMaster;
        }
        public List<GetCapitalItemDetails> GetCapitalItemDetails(Int64 CapitalItemMasterID, Int64 LockedBy, Int64 LockTimeOut)
        {
            List<GetCapitalItemDetails> llstCRDetails = null;

            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCapitalItemDetails(CapitalItemMasterID, LockedBy, LockTimeOut).ToList();
            }

            return llstCRDetails;
        }


        public List<SearchCapitalItemRequestMaster> SearchCapitalItemRequestMaster(BALCapital argclsCRMaster)
        {
            List<SearchCapitalItemRequestMaster> llstCRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRMaster = dbo.SearchCapitalItemRequestMaster(argclsCRMaster.CorporateName, argclsCRMaster.FacilityName, argclsCRMaster.VendorName, argclsCRMaster.DateFrom, argclsCRMaster.DateTo, argclsCRMaster.Status, argclsCRMaster.loggedinBy).ToList();
            }
            return llstCRMaster;
        }
        public List<object> InsertCapitalItemMaster(BALCapital argclsCRMaster)
        {
            List<object> lstrMessage = new List<object>();

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("CapitalItemMasterID", typeof(Int64));
                linecount = dbo.InsertCapitalItemMaster(argclsCRMaster.CorporateID, argclsCRMaster.FacilityID, argclsCRMaster.VendorID, argclsCRMaster.RequestType, argclsCRMaster.Shipping, argclsCRMaster.Status, argclsCRMaster.ShippingCost, argclsCRMaster.Tax, argclsCRMaster.TotalCost, argclsCRMaster.Remarks, argclsCRMaster.CreatedBy, argclsCRMaster.ApprovedBy, argclsCRMaster.DeniedBy, outputParameter);

                if (linecount > 0)
                {
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, outputParameter.Value.ToString());
                }
                else
                {
                    lstrMessage.Insert(0, "Error:Server issue");
                }

            }
            return lstrMessage;
        }

        public string InsertCapitalItemDetails(BALCapital argclsCRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertCapitalItemDetails(argclsCRDetails.CapitalItemMasterID, argclsCRDetails.EquipmentSubCategoryID, argclsCRDetails.EquipementListID, argclsCRDetails.SerialNo, argclsCRDetails.PricePerUnit, argclsCRDetails.OrderQuantity, argclsCRDetails.TotalPrice, argclsCRDetails.Reason, argclsCRDetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateCapitalIemMaster(BALCapital argclsCRMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateCapitalIemMaster(argclsCRMaster.CapitalItemMasterID, argclsCRMaster.CorporateID, argclsCRMaster.FacilityID, argclsCRMaster.VendorID, argclsCRMaster.RequestType, argclsCRMaster.Shipping, argclsCRMaster.ShippingCost, argclsCRMaster.Tax, argclsCRMaster.TotalCost, argclsCRMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateCapitalItemDetails(BALCapital argclsCRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateCapitalItemDetails(argclsCRDetails.CapitalItemMasterID, argclsCRDetails.CapitalItemDetailsID, argclsCRDetails.OrderQuantity, argclsCRDetails.TotalPrice, argclsCRDetails.Reason, argclsCRDetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string DeleteCapitalItemMaster(Int64 CapitalItemMasterID, bool IsActive, Int64 LastModifiedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteCapitalItemMaster(CapitalItemMasterID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public string DeleteCapitalItemDetails(Int64 CapitalItemDetailsID, bool IsActive, Int64 LastModifiedBy)
        {

            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteCapitalItemDetails(CapitalItemDetailsID, IsActive, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }



        public List<GetEquipmentSubCategoryforCapital> GetEquipmentSubCategoryforCapital(Int64 CorporateID, string Mode)
        {
            List<GetEquipmentSubCategoryforCapital> listItem = new List<GetEquipmentSubCategoryforCapital>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetEquipmentSubCategoryforCapital(CorporateID, Mode).ToList();
            }

            return listItem;
        }
        public List<GetEquipementListforCapital> GetEquipementListforCapital(Int64 EquimentSubCategoryID, string Mode)
        {
            List<GetEquipementListforCapital> listItem = new List<GetEquipementListforCapital>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetEquipementListforCapital(EquimentSubCategoryID, Mode).ToList();
            }

            return listItem;
        }

        public List<ValidCapitalEquipment> ValidCapitalEquipment(Int64 EquipmentSubCategoryID, Int64 EquipmentList, Int64 FacilityID)
        {
            List<ValidCapitalEquipment> listItem = new List<ValidCapitalEquipment>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.ValidCapitalEquipment(EquipmentSubCategoryID, EquipmentList, FacilityID).ToList();
            }

            return listItem;
        }

        #endregion

        // MEdical Supply Request Po
        #region Medical SupplyRequest PO
        public List<GetMSRMultipleIDs> GetMSRMultipleIDs(string PRMasterID)
        {
            List<GetMSRMultipleIDs> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.GetMSRMultipleIDs(PRMasterID).ToList();
            }
            return llstMSRMaster;
        }
        public List<SearchMedicalSupplyRequestPo> SearchMedicalSupplyRequestPo(BALMedicalSupplyRequestPo argclsMedicalSuppliesReq)
        {
            List<SearchMedicalSupplyRequestPo> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchMedicalSupplyRequestPo(argclsMedicalSuppliesReq.CorporateName, argclsMedicalSuppliesReq.FacilityName, argclsMedicalSuppliesReq.VendorName, argclsMedicalSuppliesReq.DateFrom, argclsMedicalSuppliesReq.DateTo, argclsMedicalSuppliesReq.Status, argclsMedicalSuppliesReq.LoggedinBy).ToList();
            }
            return llstMPRMaster;
        }
        public string InsertMedicalsupplyPO(BALMedicalSupplyRequestPo argclsMSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMedicalsupplyPO(argclsMSRPODetails.PRMasterID, argclsMSRPODetails.PONo, argclsMSRPODetails.Status, argclsMSRPODetails.Remarks, argclsMSRPODetails.OrderContent, argclsMSRPODetails.CreatedBy, argclsMSRPODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMSROrderContentPO> GetMSROrderContentPO(Int64 PRmasterID, Int64 LoggedInBy)
        {
            List<GetMSROrderContentPO> llstMSRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDetails = dbo.GetMSROrderContentPO(PRmasterID, LoggedInBy).ToList();
            }
            return llstMSRDetails;
        }
        public List<GetMedicalSupplyPoReportDetails> GetMedicalSupplyPoReportDetails(string MedicalSupplyMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<GetMedicalSupplyPoReportDetails> llstMSRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDetails = dbo.GetMedicalSupplyPoReportDetails(MedicalSupplyMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMSRDetails;
        }
        public string UpdateMSRPoOrderContent(BALMedicalSupplyRequestPo argclsmsrpo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMSRPoOrderContent(argclsmsrpo.PRMasterID, argclsmsrpo.OrderContent, argclsmsrpo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateMSRPoStatus(BALMedicalSupplyRequestPo argclsmsrpo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMSRPoStatus(argclsmsrpo.PRMasterID, argclsmsrpo.Status, argclsmsrpo.Remarks, argclsmsrpo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetSuperAdminDetails> GetSuperAdminDetails(Int64 CorporateID, Int64 FacilityID)
        {
            List<GetSuperAdminDetails> llstMSRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDetails = dbo.GetSuperAdminDetails(CorporateID, FacilityID).ToList();
            }
            return llstMSRDetails;
        }
        #endregion


        // CORP EQUIPMENT MAP
        #region Corp Equipment Map

        public List<BindEquipement> BindEquipement(BALCorporate argEquipment)
        {
            List<BindEquipement> lstequipment = new List<BindEquipement>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstequipment = dbo.BindEquipement(argEquipment.SearchText, argEquipment.Active, argEquipment.Mode, argEquipment.LoggedinBy, argEquipment.Filter).ToList();
            }

            return lstequipment;
        }


        public string InsertEquipmentSubCategory(BALMachinemaster argEquipmentSubCategory)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertEquipmentSubCategory(argEquipmentSubCategory.EquipementCategoryID, argEquipmentSubCategory.EquipementSubCategorydesc, argEquipmentSubCategory.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string UpdateEquipmentSubCategory(BALMachinemaster argEquipmentSubCategory)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateEquipmentSubCategory(argEquipmentSubCategory.EquipementSubCategoryID, argEquipmentSubCategory.EquipementCategoryID, argEquipmentSubCategory.EquipementSubCategorydesc, argEquipmentSubCategory.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string DeleteEquipSubCategoryMaster(BALMachinemaster argEquipmentSubCategory)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.DeleteEquipSubCategoryMaster(argEquipmentSubCategory.EquipementSubCategoryID, argEquipmentSubCategory.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public List<GetEquipementSubCategory> GetEquipementSubCategory(Int64 EquipCategoryID, string Mode)
        {
            List<GetEquipementSubCategory> SubCategoryItem = new List<GetEquipementSubCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                SubCategoryItem = dbo.GetEquipementSubCategory(EquipCategoryID, Mode).ToList();
            }

            return SubCategoryItem;
        }


        public List<GetActiveEquipementSubCategoryvalue> GetActiveEquipementSubCategoryvalue(Int64 EquipSubCatID)
        {
            List<GetActiveEquipementSubCategoryvalue> SubCatItem = new List<GetActiveEquipementSubCategoryvalue>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                SubCatItem = dbo.GetActiveEquipementSubCategoryvalue(EquipSubCatID).ToList();
            }

            return SubCatItem;
        }

        public List<SavedEquipmentSubCategory> SavedEquipmentSubCategory(Int64 EquipSubCatID)
        {
            List<SavedEquipmentSubCategory> SubCatItem = new List<SavedEquipmentSubCategory>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                SubCatItem = dbo.SavedEquipmentSubCategory(EquipSubCatID).ToList();
            }

            return SubCatItem;
        }

        public List<BindEquipementSubCategoryReport> BindEquipementSubCategoryReport(string SearchFilter, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindEquipementSubCategoryReport> SubCatItem = new List<BindEquipementSubCategoryReport>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                SubCatItem = dbo.BindEquipementSubCategoryReport(SearchFilter, LockedBy, LoggedinBy).ToList();
            }
            return SubCatItem;
        }


        public List<GetUserApprovePermission> GetUserApprovePermission(Int64 MainmenuID, Int64 SubmenuID)
        {
            List<GetUserApprovePermission> llstApprovePermission = new List<GetUserApprovePermission>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstApprovePermission = dbo.GetUserApprovePermission(MainmenuID, SubmenuID).ToList();
            }
            return llstApprovePermission;
        }


        public string InsertUserApprovePermission(BALUser argUserApprovePermission)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertUserApprovePermission(argUserApprovePermission.UserRoleID, argUserApprovePermission.MenuID, argUserApprovePermission.SubMenuId, argUserApprovePermission.PageName, argUserApprovePermission.IsEdit, argUserApprovePermission.IsView, argUserApprovePermission.IsApprove, argUserApprovePermission.IsDeny, argUserApprovePermission.IsOrder, argUserApprovePermission.ApproveRangeFrom, argUserApprovePermission.ApproveRangeTo, argUserApprovePermission.MultipleRoles, argUserApprovePermission.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateUserApprovePermission(BALUser argUserApprovePermission)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateUserApprovePermission(argUserApprovePermission.PermissionID, argUserApprovePermission.UserRoleID, argUserApprovePermission.MenuID, argUserApprovePermission.SubMenuId, argUserApprovePermission.PageName, argUserApprovePermission.IsEdit, argUserApprovePermission.IsView, argUserApprovePermission.IsApprove, argUserApprovePermission.IsDeny, argUserApprovePermission.IsOrder, argUserApprovePermission.ApproveRangeFrom, argUserApprovePermission.ApproveRangeTo, argUserApprovePermission.MultipleRoles, argUserApprovePermission.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetMultiUserApprove> GetMultiUserApprove(Int64 PermissionID)
        {
            List<GetMultiUserApprove> llstMultiApprovePermission = new List<GetMultiUserApprove>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMultiApprovePermission = dbo.GetMultiUserApprove(PermissionID).ToList();
            }
            return llstMultiApprovePermission;
        }

        #endregion

        // RequestIT PO
        #region RequestIT PO
        public List<GETITRequestPODetails> GETITRequestPODetails()
        {
            List<GETITRequestPODetails> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.GETITRequestPODetails().ToList();
            }
            return llstMPRMaster;
        }
        public List<SearchRequestITPO> SearchRequestITPO(BALRequestITPO argInsITRequestMaster)
        {
            List<SearchRequestITPO> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchRequestITPO(argInsITRequestMaster.CorporateIDs, argInsITRequestMaster.FacilityIDs, argInsITRequestMaster.VendorIDs, argInsITRequestMaster.DateFrom, argInsITRequestMaster.DateTo, argInsITRequestMaster.Status, argInsITRequestMaster.LoggedinBy).ToList();
            }
            return llstMPRMaster;
        }
        public List<BindRequestITDetailsfromPO> BindRequestITDetailsfromPO(string ITRNO)
        {
            List<BindRequestITDetailsfromPO> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindRequestITDetailsfromPO(ITRNO).ToList();
            }
            return llstMPRMaster;
        }
        public string InsertrequestPO(BALRequestITPO argclsITPOMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertrequestPO(argclsITPOMaster.RequestITMasterID, argclsITPOMaster.ITRNo, argclsITPOMaster.Status, argclsITPOMaster.Remarks, argclsITPOMaster.SortOrder, argclsITPOMaster.OrderContent, argclsITPOMaster.CreatedBy, argclsITPOMaster.LastModifiedBy, argclsITPOMaster.FacilityID);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetITROrderContentPO> GetITROrderContentPO(Int64 RequestITMasterID, Int64 LoggedInBy)
        {
            List<GetITROrderContentPO> listItem = new List<GetITROrderContentPO>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetITROrderContentPO(RequestITMasterID, LoggedInBy).ToList();
            }

            return listItem;
        }

        public List<BindITRequestPOReport> BindITRequestPOReport(string RequestITMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindITRequestPOReport> llstITRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstITRDetails = dbo.BindITRequestPOReport(RequestITMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstITRDetails;
        }



        public string UpdateITRequestPO(BALRequestITPO argclsITPOMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateITRequestPO(argclsITPOMaster.RequestITOrderID, argclsITPOMaster.OrderContent, argclsITPOMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetRwdlsafterordergeneration> GetRwdlsafterordergeneration(string RequestITMasterID)
        {
            List<GetRwdlsafterordergeneration> listItem = new List<GetRwdlsafterordergeneration>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetRwdlsafterordergeneration(RequestITMasterID).ToList();
            }

            return listItem;
        }
        #endregion


        // Service Request PO

        #region Service Request PO

        public List<GetServiceRequestPODetails> GetServiceRequestPODetails()
        {
            List<GetServiceRequestPODetails> listSRPO = new List<GetServiceRequestPODetails>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listSRPO = dbo.GetServiceRequestPODetails().ToList();
            }

            return listSRPO;
        }

        public List<SearchServiceRequestPO> SearchServiceRequestPO(BALServiceRequest argclsSRMaster)
        {
            List<SearchServiceRequestPO> llstSRPO = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRPO = dbo.SearchServiceRequestPO(argclsSRMaster.CorporateID, argclsSRMaster.FacilityID, argclsSRMaster.DateFrom, argclsSRMaster.DateTo, argclsSRMaster.Status).ToList();
            }
            return llstSRPO;
        }


        public string UpdateServiceRequestDetailsAfterAction(BALServiceRequest argclsSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceRequestDetailsAfterAction(argclsSRPODetails.ServiceRequestMasterID, argclsSRPODetails.ServiceRequestDetailID, argclsSRPODetails.Status, argclsSRPODetails.Action, argclsSRPODetails.Remarks, argclsSRPODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetServiceRequestPOGenerateDetails> GetServiceRequestPOGenerateDetails(string ListServiceRequestID, Int64 LoggedinBy)
        {
            List<GetServiceRequestPOGenerateDetails> listSRPOGenerate = new List<GetServiceRequestPOGenerateDetails>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listSRPOGenerate = dbo.GetServiceRequestPOGenerateDetails(ListServiceRequestID, LoggedinBy).ToList();
            }

            return listSRPOGenerate;
        }

        public List<GetSROrderContentPOReports> GetSROrderContentPOReports(Int64 ServiceRequestID, Int64 ServiceRequestDetailsID, Int64 LoggedInBy)
        {
            List<GetSROrderContentPOReports> listSRPOReport = new List<GetSROrderContentPOReports>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listSRPOReport = dbo.GetSROrderContentPOReports(ServiceRequestID, ServiceRequestDetailsID, LoggedInBy).ToList();
            }

            return listSRPOReport;
        }

        public List<GetServiceRequestPoReportDetails> GetServiceRequestPoReportDetails(string ServiceRequestMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<GetServiceRequestPoReportDetails> listSRPOReport = new List<GetServiceRequestPoReportDetails>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listSRPOReport = dbo.GetServiceRequestPoReportDetails(ServiceRequestMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }

            return listSRPOReport;
        }

        public string InsertServcieRequestGenerateOrder(BALServiceRequest argclsSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServcieRequestGenerateOrder(argclsSRPODetails.ServiceRequestMasterID, argclsSRPODetails.ServiceRequestDetailID, argclsSRPODetails.Status, argclsSRPODetails.Remarks, argclsSRPODetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string InsertServiceRequestApproveAction(BALServiceRequest argclsSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertServiceRequestApproveAction(argclsSRPODetails.ServiceRequestMasterID, argclsSRPODetails.ServiceRequestDetailID, argclsSRPODetails.Action, argclsSRPODetails.Remarks, argclsSRPODetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetServiceRequestActionByMasterID> GetServiceRequestActionByMasterID(Int64 ServiceRequestMasterID, Int64 ServiceRequestDetailID)
        {
            List<GetServiceRequestActionByMasterID> listSRAction = new List<GetServiceRequestActionByMasterID>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listSRAction = dbo.GetServiceRequestActionByMasterID(ServiceRequestMasterID, ServiceRequestDetailID).ToList();
            }

            return listSRAction;
        }

        public string UpdateServiceRequestApproveAction(BALServiceRequest argclsSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServiceRequestApproveAction(argclsSRPODetails.ServiceRequestMasterID, argclsSRPODetails.ServiceRequestDetailID, argclsSRPODetails.Status, argclsSRPODetails.Remarks, argclsSRPODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public List<SearchServiceRequestPurchaseOrder> SearchServiceRequestPurchaseOrder(BALServiceRequest argclsSRMaster)
        {
            List<SearchServiceRequestPurchaseOrder> llstSRPO = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRPO = dbo.SearchServiceRequestPurchaseOrder(argclsSRMaster.ListCorporateID, argclsSRMaster.ListFacilityID, argclsSRMaster.DateFrom, argclsSRMaster.DateTo, argclsSRMaster.ListStatus, argclsSRMaster.LoggedinBy).ToList();
            }
            return llstSRPO;
        }



        public List<SearchServiceRequestPurchaseOrderDetails> SearchServiceRequestPurchaseOrderDetails(BALServiceRequest argclsSRDetails)
        {
            List<SearchServiceRequestPurchaseOrderDetails> llstSRPODetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRPODetails = dbo.SearchServiceRequestPurchaseOrderDetails(argclsSRDetails.ServiceRequestMasterID, argclsSRDetails.LoggedinBy).ToList();
            }
            return llstSRPODetails;
        }

        public string UpdateServcieRequestGenerateOrder(BALServiceRequest argclsSRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServcieRequestGenerateOrder(argclsSRPODetails.ServiceRequestMasterID, argclsSRPODetails.OrderContent, argclsSRPODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }



        // User Role Permission


        public List<BindMultiRolesPermission> BindMultiRolesPermission(string ListUserID, Int64 PermissionID)
        {
            List<BindMultiRolesPermission> llstUserId = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstUserId = dbo.BindMultiRolesPermission(ListUserID, PermissionID).ToList();
            }
            return llstUserId;
        }

        public string InsertMultiPermissionApprove(BALUser argUserApprovePermission)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMultiPermissionApprove(argUserApprovePermission.PermissionID, argUserApprovePermission.UserRoleID, argUserApprovePermission.ApproveRangeFrom, argUserApprovePermission.ApproveRangeTo, argUserApprovePermission.IsActive, argUserApprovePermission.Approveorder, argUserApprovePermission.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateMultiPermissionApprove(BALUser argUserApprovePermission)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMultiPermissionApprove(argUserApprovePermission.PageMasterPermissionMultiRoleID, argUserApprovePermission.PermissionID, argUserApprovePermission.Approveorder, argUserApprovePermission.IsActive, argUserApprovePermission.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        #endregion

        //Machine PO
        #region Machine PO
        public List<GetMPRMasterOrder> GetMPRMasterOrder()
        {
            List<GetMPRMasterOrder> llstMPRMasterOrder = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMasterOrder = dbo.GetMPRMasterOrder().ToList();
            }
            return llstMPRMasterOrder;
        }
        public List<SearchMPRMasterOrder> SearchMPRMasterOrder(BALMachinePartsOrder argclsMPRMasterOrder)
        {
            List<SearchMPRMasterOrder> llstMPRMasterOrder = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMasterOrder = dbo.SearchMPRMasterOrder(argclsMPRMasterOrder.ListCorporateID, argclsMPRMasterOrder.ListFacilityID, argclsMPRMasterOrder.ListVendorID, argclsMPRMasterOrder.DateFrom, argclsMPRMasterOrder.DateTo, argclsMPRMasterOrder.Status, argclsMPRMasterOrder.LoggedinBy).ToList();
            }
            return llstMPRMasterOrder;
        }

        public string InsertMachinePO(BALMachinePartsOrder argclsMPRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMachinePO(argclsMPRPODetails.MPRMasterID, argclsMPRPODetails.MPONo, argclsMPRPODetails.Status, argclsMPRPODetails.Remarks, argclsMPRPODetails.OrderContent, argclsMPRPODetails.CreatedBy, argclsMPRPODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMPOrderContentPO> GetMPOrderContentPO(Int64 MPRMasterID, Int64 LoggedInBy)
        {
            List<GetMPOrderContentPO> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetMPOrderContentPO(MPRMasterID, LoggedInBy).ToList();
            }
            return llstMPRDetails;
        }

        public List<BindMachinePOReport> BindMachinePOReport(string MPRMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindMachinePOReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMachinePOReport(MPRMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }
        public string UpdateMachinePO(BALMachinePartsOrder argclsmaPo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMachinePO(argclsmaPo.MPRMasterID, argclsmaPo.OrderContent, argclsmaPo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public string UpdateMPRPoStatus(BALMachinePartsOrder argclsmsrpo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMPRPoStatus(argclsmsrpo.MPRMasterID, argclsmsrpo.Status, argclsmsrpo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string InsertMachineApprove(BALMachinePartsOrder argclsmsrpo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertMachineApprove(argclsmsrpo.MPRMasterID, argclsmsrpo.Status, argclsmsrpo.Remarks, argclsmsrpo.CreatedBy, argclsmsrpo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetMachinePartsOrderMPONo> GetMachinePartsOrderMPONo(string MPRMasterID)
        {
            List<GetMachinePartsOrderMPONo> llstMRDetails = new List<GetMachinePartsOrderMPONo>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMRDetails = dbo.GetMachinePartsOrderMPONo(MPRMasterID).ToList();
            }
            return llstMRDetails;
        }

        #endregion



        #region CapitalPO
        //public List<GetCapitalOrder> GetCapitalOrder()
        //{
        //    List<GetCapitalOrder> llstCRMaster = null;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstCRMaster = dbo.GetCapitalOrder().ToList();
        //    }
        //    return llstCRMaster;
        //}
        public List<SearchCapitalPO> SearchCapitalPO(BALCapitalPO argInsCRMaster)
        {
            List<SearchCapitalPO> llstCRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRMaster = dbo.SearchCapitalPO(argInsCRMaster.ListCorporateID, argInsCRMaster.ListFacilityID, argInsCRMaster.ListVendorID, argInsCRMaster.DateFrom, argInsCRMaster.DateTo, argInsCRMaster.Status, argInsCRMaster.LoggedinBy).ToList();
            }
            return llstCRMaster;
        }

        public string InsertCapitalPO(BALCapitalPO argclsCRPODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertCapitalPO(argclsCRPODetails.CapitalItemMasterID, argclsCRPODetails.CPONo, argclsCRPODetails.Status, argclsCRPODetails.Remarks, argclsCRPODetails.OrderContent, argclsCRPODetails.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetCROrderContentPO> GetCROrderContentPO(BALCapitalPO argclsCRPO)
        {
            List<GetCROrderContentPO> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCROrderContentPO(argclsCRPO.CapitalItemMasterID, argclsCRPO.LoggedinBy).ToList();
            }
            return llstCRDetails;
        }

        //public string UpdateCapitalPO(BALCapitalPO argupdCRMaster)
        //{
        //    string lstrMessage = string.Empty;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        int linecount = 0;
        //        linecount = dbo.UpdateCapitalPO(argupdCRMaster.CapitalItemMasterID, argupdCRMaster.CPONo, argupdCRMaster.Status, argupdCRMaster.Remarks, argupdCRMaster.OrderContent, argupdCRMaster.LastModifiedBy);
        //        if (linecount > 0)
        //        {
        //            lstrMessage = "Updated Successfully";
        //        }
        //        else
        //        {
        //            lstrMessage = "Error:Server issue";
        //        }

        //    }
        //    return lstrMessage;
        //}

        public List<GetCapitalPOReport> GetCapitalPOReport(string CapitalItemMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<GetCapitalPOReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCapitalPOReport(CapitalItemMasterID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstCRDetails;
        }

        public string UpdateCRPOOrderContent(BALCapitalPO argclsmsrpo)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateCRPOOrderContent(argclsmsrpo.CapitalItemMasterID, argclsmsrpo.OrderContent, argclsmsrpo.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        //public string UpdateCRPOStatus(BALCapitalPO argclsmsrpo)
        //{
        //    string lstrMessage = string.Empty;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        int linecount = 0;
        //        linecount = dbo.UpdateCRPOStatus(argclsmsrpo.CapitalItemMasterID, argclsmsrpo.Status, argclsmsrpo.Remarks, argclsmsrpo.LastModifiedBy);
        //        if (linecount > 0)
        //        {
        //            lstrMessage = "Saved Successfully";
        //        }
        //        else
        //        {
        //            lstrMessage = "Error:Server issue";
        //        }

        //    }
        //    return lstrMessage;
        //}


        public string InsertCapitalApprove(BALCapitalPO argclsCRAODetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertCapitalApprove(argclsCRAODetails.CapitalItemMasterID, argclsCRAODetails.Status, argclsCRAODetails.Remarks, argclsCRAODetails.CreatedBy, argclsCRAODetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetCapitalOrderCPONo> GetCapitalOrderCPONo(string CapitalItemMasterID)
        {
            List<GetCapitalOrderCPONo> llstCRDetails = new List<GetCapitalOrderCPONo>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCapitalOrderCPONo(CapitalItemMasterID).ToList();
            }
            return llstCRDetails;
        }


        #endregion


        #region Service Request Receiving Order

        public List<SearchServiceRequestReceivingOrder> SearchServiceRequestReceivingOrder(BALServiceRequest argclsSRMaster)
        {
            List<SearchServiceRequestReceivingOrder> llstSRPO = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRPO = dbo.SearchServiceRequestReceivingOrder(argclsSRMaster.ListCorporateID, argclsSRMaster.ListFacilityID, argclsSRMaster.DateFrom, argclsSRMaster.DateTo, argclsSRMaster.ListStatus, argclsSRMaster.LoggedinBy).ToList();
            }
            return llstSRPO;
        }

        public string UpdateServcieRecevingOrder(BALServiceRequest argclsSRMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServcieRecevingOrder(argclsSRMaster.ServiceRequestMasterID, argclsSRMaster.Status, argclsSRMaster.Remarks, argclsSRMaster.OtherRemarks, argclsSRMaster.IsReceive, argclsSRMaster.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateServcieRecevinginvoice(BALServiceRequest argclsSRMaster)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateServcieRecevinginvoice(argclsSRMaster.ServiceRequestMasterID, argclsSRMaster.InvoiceNo, argclsSRMaster.InvoiceStatus, argclsSRMaster.InvoiceRemarks, argclsSRMaster.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string SyncServiceReceivingorder()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.SyncServiceReceivingorder();
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetServiceReceiveOrder> GetServiceReceiveOrder(BALServiceRequest argclsSRMaster)
        {
            List<GetServiceReceiveOrder> llstSRPO = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSRPO = dbo.GetServiceReceiveOrder(argclsSRMaster.ServiceRequestMasterID, argclsSRMaster.ServiceRequestDetailID, argclsSRMaster.LoggedinBy).ToList();
            }
            return llstSRPO;
        }

        #endregion


        //Machine Parts Receive
        public List<SearchMachinePartsReceive> SearchMachinePartsReceive(BALMachinePartsReceiveOrder argclsMPRMaster)
        {
            List<SearchMachinePartsReceive> llstMPR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPR = dbo.SearchMachinePartsReceive(argclsMPRMaster.ListCorporateID, argclsMPRMaster.ListFacilityID, argclsMPRMaster.ListVendorID, argclsMPRMaster.DateFrom, argclsMPRMaster.DateTo, argclsMPRMaster.FinalStatus, argclsMPRMaster.LoggedinBy).ToList();
            }
            return llstMPR;
        }
        public List<GETMPOValues> GETMPOValues(Int64 MachinePartsReceiveMasterID)
        {
            List<GETMPOValues> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GETMPOValues(MachinePartsReceiveMasterID).ToList();
            }
            return llstMPRDetails;
        }
        public List<GETMPROValues> GETMPROValues(Int64 MachinePartsReceiveMasterID)
        {
            List<GETMPROValues> llstMPRODetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRODetails = dbo.GETMPROValues(MachinePartsReceiveMasterID).ToList();
            }
            return llstMPRODetails;
        }
        public string SyncMachinePartsReceivingorder()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.SyncMachinePartsReceivingorder();
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<BindMachineReceiveSummaryReport> BindMachineReceiveSummaryReport(Int64 MachinePartsRequestOrderID, Int64 LockedBy, string Filters)
        {
            List<BindMachineReceiveSummaryReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMachineReceiveSummaryReport(MachinePartsRequestOrderID, LockedBy, Filters).ToList();
            }
            return llstMPRDetails;
        }
        public List<BindMachineReceivingDetailsReport> BindMachineReceivingDetailsReport(Int64 MachinrPartsRequestOrderID, Int64 MachinrPartsReceivingMasterID, Int64 LoggedInBy, String Filter)
        {
            List<BindMachineReceivingDetailsReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindMachineReceivingDetailsReport(MachinrPartsRequestOrderID, MachinrPartsReceivingMasterID, LoggedInBy, "").ToList();
            }
            return llstMPRMaster;
        }

        public List<BindMachinePartReceivingDetailsSubReport> BindMachinePartReceivingDetailsSubReport(Int64 MPRequestMasterID, Int64 LockedBy, string Filters)
        {
            List<BindMachinePartReceivingDetailsSubReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindMachinePartReceivingDetailsSubReport(MPRequestMasterID, LockedBy, Filters).ToList();
            }
            return llstMPRDetails;
        }

        public List<UpdateMachinePartsReceivingMaster> UpdateMachinePartsReceivingMaster(BALMachinePartsReceiveOrder argclsMPRMaster)
        {
            List<UpdateMachinePartsReceivingMaster> llstMPRMaster = new List<Service.UpdateMachinePartsReceivingMaster>();
            using (InventoryEntities dbo = new InventoryEntities())
            {

                llstMPRMaster = dbo.UpdateMachinePartsReceivingMaster(argclsMPRMaster.MachinePartsReceiveMasterID, argclsMPRMaster.MPRMasterID, argclsMPRMaster.MachinePartsRequestOrderID, argclsMPRMaster.MPONo, argclsMPRMaster.PackingSlipNo, argclsMPRMaster.PackingSlipDate, argclsMPRMaster.ReceivedDate, argclsMPRMaster.InvoiceNo,
                    argclsMPRMaster.InvoiceStatus, argclsMPRMaster.InvoicedBy, argclsMPRMaster.InvoiceDate, argclsMPRMaster.ReceivingAction, argclsMPRMaster.Reason, argclsMPRMaster.Others, argclsMPRMaster.FinalStatus, argclsMPRMaster.ShippingCost, argclsMPRMaster.Tax, argclsMPRMaster.TotalCost,
                    argclsMPRMaster.CreatedBy, argclsMPRMaster.Type, argclsMPRMaster.LoggedinBy, argclsMPRMaster.Filter).ToList();
            }
            return llstMPRMaster;
        }

        public string UpdateMachinePartsReceivingDetails(BALMachinePartsReceiveOrder argclsMPRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMachinePartsReceivingDetails(argclsMPRDetails.MachinePartsReceiveDetailsID, argclsMPRDetails.MachinePartsReceiveMasterID, argclsMPRDetails.MPRMasterID, argclsMPRDetails.MachinePartsRequestOrderID, argclsMPRDetails.FinalStatus, argclsMPRDetails.CreatedBy, argclsMPRDetails.Type, argclsMPRDetails.LoggedinBy, argclsMPRDetails.Filter, argclsMPRDetails.INSERTRECORDID,
                    argclsMPRDetails.BalanceQty, argclsMPRDetails.ReceivedQty, argclsMPRDetails.TotalPrice, argclsMPRDetails.Comments);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else if (linecount == -1)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        #region MedicalSuupliesReceivingOrders
        public List<SearchMedicalSuppliesReceiving> SearchMedicalSuppliesReceiving(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec)
        {
            List<SearchMedicalSuppliesReceiving> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.SearchMedicalSuppliesReceiving(argclsMedicalSuppliesRec.CorporateName, argclsMedicalSuppliesRec.FacilityName, argclsMedicalSuppliesRec.VendorName, argclsMedicalSuppliesRec.DateFrom, argclsMedicalSuppliesRec.DateTo, argclsMedicalSuppliesRec.Status, argclsMedicalSuppliesRec.LoggedinBy).ToList();
            }
            return llstMSRMaster;
        }
        public List<BindMedicalsupplyReceivingDetail> BindMedicalsupplyReceivingDetail(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec)
        {
            List<BindMedicalsupplyReceivingDetail> llstMSRDeatils = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDeatils = dbo.BindMedicalsupplyReceivingDetail(argclsMedicalSuppliesRec.MedicalSuppliesReceivingMasterID, argclsMedicalSuppliesRec.SearchFilters, argclsMedicalSuppliesRec.LoggedinBy).ToList();
            }
            return llstMSRDeatils;
        }
        public string SyncMedicalSuppliesReceivingorder()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.SyncMedicalSuppliesReceivingOrder();
                    lstrMessage = "Saved Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<UpdateMSRReceivingMaster> UpdateMSRReceivingMaster(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec)
        {
            List<UpdateMSRReceivingMaster> llstMSRDeatils = new List<UpdateMSRReceivingMaster>();
            //List<UpdateMSRReceivingMaster> llstMSRDeatils = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {

                llstMSRDeatils = dbo.UpdateMSRReceivingMaster(argclsMedicalSuppliesRec.MedicalSuppliesReceivingMasterID, argclsMedicalSuppliesRec.PRMasterID, argclsMedicalSuppliesRec.PONo, argclsMedicalSuppliesRec.MedicalSuppliesRequestOrderID, argclsMedicalSuppliesRec.PackingSlipNo, argclsMedicalSuppliesRec.PackingSlipDate, argclsMedicalSuppliesRec.ReceivedDate,
                                   argclsMedicalSuppliesRec.InvoiceNo, argclsMedicalSuppliesRec.InvoiceStatus, argclsMedicalSuppliesRec.InvoicedBy, argclsMedicalSuppliesRec.InvoiceDate, argclsMedicalSuppliesRec.ReceivingAction, argclsMedicalSuppliesRec.Reason, argclsMedicalSuppliesRec.OtherReason,
                                   argclsMedicalSuppliesRec.FinalStatus, argclsMedicalSuppliesRec.CreatedBy, argclsMedicalSuppliesRec.Type, argclsMedicalSuppliesRec.ShippingCost, argclsMedicalSuppliesRec.Tax, argclsMedicalSuppliesRec.TotalCost, argclsMedicalSuppliesRec.LoggedinBy, argclsMedicalSuppliesRec.Filter).ToList();
            }
            return llstMSRDeatils;
        }
        public string UpdateMSRReceivingDetails(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateMSRReceivingDetails(argclsMedicalSuppliesRec.MedicalSuppliesReceivingDetailsID, argclsMedicalSuppliesRec.MedicalSuppliesReceivingMasterID, argclsMedicalSuppliesRec.FinalStatus, argclsMedicalSuppliesRec.Type,
                   argclsMedicalSuppliesRec.INSERTRECORDID, argclsMedicalSuppliesRec.LoggedinBy, argclsMedicalSuppliesRec.Filter, argclsMedicalSuppliesRec.BalanceQty, argclsMedicalSuppliesRec.ReceivedQty, argclsMedicalSuppliesRec.TotalPrice,
                   argclsMedicalSuppliesRec.Comments);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else if (linecount == -1)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }
        public List<GetMSRReceivingsummaryReport> GetMSRReceivingsummaryReport(BALMedicalSupplyReceivingOrder argmsr)
        {
            List<GetMSRReceivingsummaryReport> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.GetMSRReceivingsummaryReport(argmsr.PRMasterID, argmsr.LoggedinBy, argmsr.SearchFilters).ToList();
            }
            return llstMSRMaster;
        }
        public List<BindMedicalSupplyDetailsReport> BindMedicalSupplyDetailsReport(BALMedicalSupplyReceivingOrder argmsr)
        {
            List<BindMedicalSupplyDetailsReport> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.BindMedicalSupplyDetailsReport(argmsr.PRMasterID, argmsr.MedicalSuppliesReceivingMasterID, argmsr.LoggedinBy, argmsr.SearchFilters).ToList();
            }
            return llstMSRMaster;
        }
        public List<BindMedicalSupplyDetailsSubReport> BindMedicalSupplyDetailsSubReport(BALMedicalSupplyReceivingOrder argmsr)
        {
            List<BindMedicalSupplyDetailsSubReport> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.BindMedicalSupplyDetailsSubReport(argmsr.PRMasterID, argmsr.LoggedinBy, argmsr.SearchFilters).ToList();
            }
            return llstMSRMaster;
        }
        #endregion

        #region ITReceiving

        public List<BindITReceivingDetailsReport> BindITReceivingDetailsReport(Int64 ITRequestMasterID, Int64 ITReceivingMasterID, Int64 LoggedInBy, String Filter)
        {
            List<BindITReceivingDetailsReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindITReceivingDetailsReport(ITRequestMasterID, ITReceivingMasterID, LoggedInBy, "").ToList();
            }
            return llstMPRMaster;
        }
        public List<BindITReceivingDetailsSubReport> BindITReceivingDetailsSubReport(Int64 ITRequestMasterID, Int64 LoggedInBy, String Filter)
        {
            List<BindITReceivingDetailsSubReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindITReceivingDetailsSubReport(ITRequestMasterID, LoggedInBy, "").ToList();
            }
            return llstMPRMaster;
        }

        public List<Getitronovalue> BindITRONo(Int64 ITRequestMasterID)
        {
            List<Getitronovalue> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.Getitronovalue(ITRequestMasterID).ToList();
            }
            return llstMPRMaster;
        }
        public List<BindITReceivingsummaryReport> BindITReceivingsummaryReport(String ITNNo, Int64 LoggedinBy, String Filter)
        {
            List<BindITReceivingsummaryReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindITReceivingsummaryReport(ITNNo, LoggedinBy, Filter).ToList();
            }
            return llstMPRMaster;
        }

        public string SynITReceivingorder()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.SyncITReceivingorder();
                    lstrMessage = "Saved Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<SearchITReceiving> SearchITReceiving(BALRequestITReceiving argInsITRequestMaster)
        {
            List<SearchITReceiving> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchITReceiving(argInsITRequestMaster.CorporateIDs, argInsITRequestMaster.FacilityIDs, argInsITRequestMaster.VendorIDs, argInsITRequestMaster.DateFrom, argInsITRequestMaster.DateTo, argInsITRequestMaster.FinalStatus, argInsITRequestMaster.LoggedinBy).ToList();
            }
            return llstMPRMaster;
        }

        public List<BindITNNOvalues> BindITNNOvalues(Int64 ITReceivingMasterID)
        {
            List<BindITNNOvalues> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindITNNOvalues(ITReceivingMasterID).ToList();
            }
            return llstMPRMaster;
        }

        public List<object> InsertITReceivingMaster(BALRequestITReceiving argITrecving)
        {
            List<object> lstrMessage = new List<object>();

            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                var outputParameter = new ObjectParameter("ITReceivingMasterID", typeof(Int64));
                linecount = dbo.InsertITReceivingMaster(argITrecving.RequestITMasterID, argITrecving.FacilityID, argITrecving.PackingSlipNo, argITrecving.PackingDate,
                      argITrecving.ReceivedDate, argITrecving.InvoiceNo, argITrecving.InvoiceDate, argITrecving.Receivingaction, argITrecving.Reason,
                      argITrecving.FinalStatus, argITrecving.CreatedBy, argITrecving.InvoiceStatus, argITrecving.InvoicedBy, argITrecving.PartialBy,
                      argITrecving.PartialOn, argITrecving.VoidBy, argITrecving.VoidOn, argITrecving.ClosedBy, argITrecving.ClosedOn, outputParameter);

                if (linecount > 0)
                {
                    lstrMessage.Insert(0, "Saved Successfully");
                    lstrMessage.Insert(1, outputParameter.Value.ToString());
                }
                else
                {
                    lstrMessage.Insert(0, "Error:Server issue");
                }

            }
            return lstrMessage;
        }

        public string InsertITReceivingDetails(BALRequestITReceiving argITrecving)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                //linecount = dbo.InsertITReceivingDetails(argITrecving.ITReceivingMasterID,argITrecving.RequestITMasterID,argITrecving.EquimentSubCategory,argITrecving.EquipmentList,argITrecving.SerialNo,argITrecving.PricePerQty,argITrecving.OrderQty,argITrecving.BalacedQty,argITrecving.ReceivedQty,argITrecving.TotalPrice,argITrecving.Comments,argITrecving.Shippingcost,argITrecving.Tax,argITrecving.TotalCost,argITrecving.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }



        public string UpdateITReceivingDetails(BALRequestITReceiving argclsItDetailsReceive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateITReceivingDetails(argclsItDetailsReceive.ITReceivingDetailsID, argclsItDetailsReceive.ITReceivingMasterID, argclsItDetailsReceive.RequestITMasterID, argclsItDetailsReceive.FinalStatus, argclsItDetailsReceive.CreatedBy, argclsItDetailsReceive.Type, argclsItDetailsReceive.LoggedinBy, argclsItDetailsReceive.Filter, argclsItDetailsReceive.InsertRecordID, argclsItDetailsReceive.BalacedQty, argclsItDetailsReceive.ReceivedQty,
                    argclsItDetailsReceive.TotalPrice, argclsItDetailsReceive.Comments, argclsItDetailsReceive.User);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else if (linecount == -1)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }



        public List<UpdateITRecevingMaster> UpdateITReceivingMaster(BALRequestITReceiving argclsITMasterReceive)
        {
            List<UpdateITRecevingMaster> llstMSRDeatils = new List<UpdateITRecevingMaster>();
            //List<UpdateMSRReceivingMaster> llstMSRDeatils = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRDeatils = dbo.UpdateITRecevingMaster(argclsITMasterReceive.ITReceivingMasterID, argclsITMasterReceive.RequestITMasterID, argclsITMasterReceive.RequestITOrderID, argclsITMasterReceive.ITNNo, argclsITMasterReceive.PackingSlipNo, argclsITMasterReceive.PackingDate, argclsITMasterReceive.ReceivedDate, argclsITMasterReceive.InvoiceNo, argclsITMasterReceive.InvoiceStatus, argclsITMasterReceive.InvoicedBy, argclsITMasterReceive.InvoiceDate,
                    argclsITMasterReceive.Receivingaction, argclsITMasterReceive.Reason, argclsITMasterReceive.OtherReason, argclsITMasterReceive.FinalStatus, argclsITMasterReceive.CreatedBy, argclsITMasterReceive.Type, argclsITMasterReceive.LoggedinBy, argclsITMasterReceive.Filter, argclsITMasterReceive.Shippingcost, argclsITMasterReceive.Tax, argclsITMasterReceive.TotalCost).ToList();
            }
            return llstMSRDeatils;
        }



        //public string UpdateITReceivingMaster(BALRequestITReceiving argclsITMasterReceive)
        //{
        //    string lstrMessage = string.Empty;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        int linecount = 0;
        //        linecount = dbo.UpdateITReceivingMaster(argclsITMasterReceive.ITReceivingMasterID, argclsITMasterReceive.RequestITMasterID,argclsITMasterReceive.PackingSlipNo,argclsITMasterReceive.PackingDate,argclsITMasterReceive.ReceivedDate,argclsITMasterReceive.InvoiceNo, argclsITMasterReceive.InvoiceStatus,argclsITMasterReceive.InvoicedBy, argclsITMasterReceive.InvoiceDate,
        //            argclsITMasterReceive.Receivingaction,argclsITMasterReceive.Reason,argclsITMasterReceive.OtherReason,argclsITMasterReceive.FinalStatus,argclsITMasterReceive.CreatedBy,argclsITMasterReceive.Type,argclsITMasterReceive.LoggedinBy,argclsITMasterReceive.Filter );
        //        if (linecount > 0)
        //        {
        //            lstrMessage = "Updated Successfully";
        //        }
        //        else
        //        {
        //            lstrMessage = "Error:Server issue";
        //        }

        //    }
        //    return lstrMessage;
        //}
        #endregion



        #region functions of CAPITALRO Page

        public List<SearchCapitalReceivingMaster> SearchCapitalReceivingMaster(BALCapitalReceiving argclsCRMasterReceive)
        {
            List<SearchCapitalReceivingMaster> llstCR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCR = dbo.SearchCapitalReceivingMaster(argclsCRMasterReceive.ListCorporateID, argclsCRMasterReceive.ListFacilityID, argclsCRMasterReceive.ListVendorID, argclsCRMasterReceive.DateFrom, argclsCRMasterReceive.DateTo, argclsCRMasterReceive.Status, argclsCRMasterReceive.LoggedinBy).ToList();
            }
            return llstCR;
        }

        public string SyncCapitalReceivingorder()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.SyncCapitalReceivingorder();
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateCapitalReceivingDetails(BALCapitalReceiving argclsCRMasterReceive)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateCapitalReceivingDetails(argclsCRMasterReceive.CapitalReceivingDetailsID, argclsCRMasterReceive.CapitalReceivingMasterID, argclsCRMasterReceive.CapitalItemMasterID, argclsCRMasterReceive.FinalStatus, argclsCRMasterReceive.CreatedBy, argclsCRMasterReceive.Type, argclsCRMasterReceive.LoggedinBy, argclsCRMasterReceive.Filter, argclsCRMasterReceive.INSERTRECORDID, argclsCRMasterReceive.BalenceQty, argclsCRMasterReceive.ReceivedQty,
                           argclsCRMasterReceive.TotalPrice, argclsCRMasterReceive.Comments);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else if (linecount == -1)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<UpdateCapitalRecevingMaster> UpdateCapitalReceivingMaster(BALCapitalReceiving argclsCRMasterReceive)
        {
            List<UpdateCapitalRecevingMaster> llsCRDeatils = new List<UpdateCapitalRecevingMaster>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llsCRDeatils = dbo.UpdateCapitalRecevingMaster(argclsCRMasterReceive.CapitalReceivingMasterID, argclsCRMasterReceive.CapitalItemMasterID, argclsCRMasterReceive.CapitalOrderID, argclsCRMasterReceive.CPONo, argclsCRMasterReceive.PackingSlipNo, argclsCRMasterReceive.PackingDate, argclsCRMasterReceive.ReceivedDate, argclsCRMasterReceive.InvoiceNo, argclsCRMasterReceive.InvoiceStatus, argclsCRMasterReceive.InvoicedBy, argclsCRMasterReceive.InvoiceDate, argclsCRMasterReceive.ReceivingAction, argclsCRMasterReceive.Reason,
                    argclsCRMasterReceive.OtherReason, argclsCRMasterReceive.FinalStatus, argclsCRMasterReceive.CreatedBy, argclsCRMasterReceive.Type, argclsCRMasterReceive.LoggedinBy, argclsCRMasterReceive.Filter, argclsCRMasterReceive.ShippingCost, argclsCRMasterReceive.Tax, argclsCRMasterReceive.TotalCost).ToList();
            }
            return llsCRDeatils;
        }

        public List<GetCpoDetails> GetCpoDetails(Int64 CapitalReceivingMasterID)
        {
            List<GetCpoDetails> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCpoDetails(CapitalReceivingMasterID).ToList();
            }
            return llstCRDetails;
        }

        public List<GetCPROMasterReview> GetCPROMasterReview(Int64 CapitalReceivingMasterID)
        {
            List<GetCPROMasterReview> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCPROMasterReview(CapitalReceivingMasterID).ToList();
            }
            return llstCRDetails;
        }

        public List<BindCapitalDetailsReport> BindCapitalDetailsReport(Int64 CapitalItemMasterID, Int64 CapitalItemReceivingMasterID, Int64 LoggedInBy, String Filter)
        {
            List<BindCapitalDetailsReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindCapitalDetailsReport(CapitalItemMasterID, CapitalItemReceivingMasterID, LoggedInBy, "").ToList();
            }
            return llstMPRMaster;
        }
        public List<BindCapitalReceivingDetailsSubReport> BindCapitalReceivingDetailsSubReport(Int64 CapitalItemMasterID, Int64 LoggedInBy, String Filter)
        {
            List<BindCapitalReceivingDetailsSubReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindCapitalReceivingDetailsSubReport(CapitalItemMasterID, LoggedInBy, "").ToList();
            }
            return llstMPRMaster;
        }

        public List<BindCapitalReceivingsummaryReport> BindCapitalReceivingsummaryReport(Int64 CapitalOrderID, Int64 LoggedinBy, String Filter)
        {
            List<BindCapitalReceivingsummaryReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.BindCapitalReceivingsummaryReport(CapitalOrderID, LoggedinBy, Filter).ToList();
            }
            return llstMPRMaster;
        }


        #endregion

        #region Multi Serarch DropDown

        public List<GetFacilityByListCorporateID> GetFacilityByListCorporateID(string ListCorporateID, Int64 UserID, Int64 RoleID)
        {
            List<GetFacilityByListCorporateID> llstFacilityDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstFacilityDetails = dbo.GetFacilityByListCorporateID(ListCorporateID, UserID, RoleID).ToList();
            }
            return llstFacilityDetails;
        }

        public List<GetVendorByFacilityID> GetVendorByFacilityID(string ListVendorID, Int64 LoggedInBy)
        {
            List<GetVendorByFacilityID> llstVendorDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorDetails = dbo.GetVendorByFacilityID(ListVendorID, LoggedInBy).ToList();
            }
            return llstVendorDetails;
        }

        #endregion

        #region Ending Inventory

        public List<SearchEndingInventory> SearchEndingInventory(BALEndingInventory argclsEndinginven)
        {
            List<SearchEndingInventory> llstEndingInventory = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstEndingInventory = dbo.SearchEndingInventory(argclsEndinginven.ListCorporateID, argclsEndinginven.ListFacilityID, argclsEndinginven.ListVendorID, argclsEndinginven.ListCategoryID, argclsEndinginven.MonthYear, argclsEndinginven.IsNewFacility, argclsEndinginven.LoggedinBy, argclsEndinginven.Filter, argclsEndinginven.ItemType).ToList();
            }
            return llstEndingInventory;
        }

        public string SyncEndingInventory()
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.SyncEndingInventory();
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }


        public string InsertEndingInventory(BALEndingInventory argclsEndinginven)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertEndingInventory(argclsEndinginven.CorporateID, argclsEndinginven.FacilityID, argclsEndinginven.CategoryID,
                argclsEndinginven.MonthYear, argclsEndinginven.ItemID, argclsEndinginven.ItemDescription, argclsEndinginven.QtyPack, argclsEndinginven.UOM,
                argclsEndinginven.BeggingInven, argclsEndinginven.ReceiveingInven, argclsEndinginven.TransferIn, argclsEndinginven.TransferOut, argclsEndinginven.ExpiredMeds,
                argclsEndinginven.EndingInven, argclsEndinginven.MonthlyUsage, argclsEndinginven.IsNewRecord, argclsEndinginven.IsNewFacility, argclsEndinginven.Noofvisit, argclsEndinginven.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public string UpdateEndingInventory(BALEndingInventory argclsEndinginven)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateEndingInventory(argclsEndinginven.EndingInvenID, argclsEndinginven.BeggingInven, argclsEndinginven.ReceiveingInven, argclsEndinginven.TransferIn,
                    argclsEndinginven.TransferOut, argclsEndinginven.ExpiredMeds, argclsEndinginven.EndingInven, argclsEndinginven.MonthlyUsage, argclsEndinginven.Noofvisit, argclsEndinginven.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetReceivedQtyInfo> GetReceivedQtyInfo(BALEndingInventory argclsEndinginven)
        {
            List<GetReceivedQtyInfo> llstReceivedQtyInfo = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstReceivedQtyInfo = dbo.GetReceivedQtyInfo(argclsEndinginven.ItemID, argclsEndinginven.FacilityID, argclsEndinginven.ReceiveDate).ToList();
            }
            return llstReceivedQtyInfo;
        }

        public List<GetTransferINQtyInfo> GetTransferINQtyInfo(BALEndingInventory argclsEndinginven)
        {
            List<GetTransferINQtyInfo> llstTransferINQtyInfo = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferINQtyInfo = dbo.GetTransferINQtyInfo(argclsEndinginven.ItemID, argclsEndinginven.FacilityID, argclsEndinginven.ReceiveDate).ToList();
            }
            return llstTransferINQtyInfo;
        }

        public List<GetTransferOutQtyInfo> GetTransferOutQtyInfo(BALEndingInventory argclsEndinginven)
        {
            List<GetTransferOutQtyInfo> llstTransferOutQtyInfo = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferOutQtyInfo = dbo.GetTransferOutQtyInfo(argclsEndinginven.ItemID, argclsEndinginven.FacilityID, argclsEndinginven.ReceiveDate).ToList();
            }
            return llstTransferOutQtyInfo;
        }

        public List<EndingInventoryReport> EndingInventoryReport(BALEndingInventory argclsEndinginven)
        {
            List<EndingInventoryReport> llstEndingInventoryReport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstEndingInventoryReport = dbo.EndingInventoryReport(argclsEndinginven.ListCorporateID, argclsEndinginven.ListFacilityID, argclsEndinginven.ListCategoryID, argclsEndinginven.MonthYear, argclsEndinginven.IsNewFacility, argclsEndinginven.LoggedinBy, argclsEndinginven.Filter).ToList();
            }
            return llstEndingInventoryReport;
        }

        public List<GetCatagoryByFacilityID> GetCatagoryByFacilityID(BALEndingInventory argclsEndinginven)
        {
            List<GetCatagoryByFacilityID> llstCatagory = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCatagory = dbo.GetCatagoryByFacilityID(argclsEndinginven.FacilityID).ToList();
            }
            return llstCatagory;
        }

        #endregion

        //Transfer Out
        #region functions of TransferOUT Page

        public List<SearchTransferOut> SearchTransferOut(BALTransferOut argclsTransferOut)
        {
            List<SearchTransferOut> llstCR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCR = dbo.SearchTransferOut(argclsTransferOut.TransferDate, argclsTransferOut.CorporateID, argclsTransferOut.FacilityIDFrom, argclsTransferOut.ItemCategory, argclsTransferOut.LoggedinBy, argclsTransferOut.Filter).ToList();
            }
            return llstCR;
        }
        public string InsertTransferDetails(BALTransferOut argInsTransfer)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertTransferOut(argInsTransfer.TransferNo, argInsTransfer.TransferDate, argInsTransfer.CorporateIDFrom, argInsTransfer.CorporateIDTo, argInsTransfer.FacilityIDFrom, argInsTransfer.FacilityIDTo,
                   argInsTransfer.ItemCategory, argInsTransfer.ItemID, argInsTransfer.ItemDescription, argInsTransfer.UOMID, argInsTransfer.QtyPack, argInsTransfer.Price, argInsTransfer.TransferQty, argInsTransfer.TotalPrice, argInsTransfer.ListStatus, argInsTransfer.CreatedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<GetTransferNo> GetTransferNo(Int64 FacilityIDFrom, Int64 FacilityIDTo)
        {
            List<GetTransferNo> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetTransferNo(FacilityIDFrom, FacilityIDTo).ToList();
            }
            return llstMPRDetails;
        }

        public List<GetEmailNotificationforTransfer> GetEmailNotificationforTransfer()
        {
            List<GetEmailNotificationforTransfer> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetEmailNotificationforTransfer().ToList();
            }
            return llstMPRDetails;
        }
        public List<GetFromEmailforTransfer> GetFromEmailforTransfer(string TransferNo)
        {
            List<GetFromEmailforTransfer> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.GetFromEmailforTransfer(TransferNo).ToList();
            }
            return llstMPRDetails;
        }
        #endregion

        //Transfer In 
        #region Transfer In Functions

        public List<SearchTransferIn> SearchTransferIn(BALTransferIn argclsTransferIn)
        {
            List<SearchTransferIn> llstTransferIn = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferIn = dbo.SearchTransferIn(argclsTransferIn.ListCorporateID, argclsTransferIn.ListFacilityID, argclsTransferIn.ListCategoryID, argclsTransferIn.DateFrom, argclsTransferIn.DateTo, argclsTransferIn.Status, argclsTransferIn.LoggedinBy).ToList();
            }
            return llstTransferIn;
        }
        public string InsertTransferIn(BALTransferIn argclstransferIn)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.InsertTransferIn(argclstransferIn.TransferOutID, argclstransferIn.TransferNo, argclstransferIn.TransferOutDate, argclstransferIn.CorporateIDfrom, argclstransferIn.CorporateIDTo, argclstransferIn.FacilityIDFrom, argclstransferIn.FacilityIDTo,
                    argclstransferIn.CategoryID, argclstransferIn.ItemID, argclstransferIn.ItemDescription, argclstransferIn.QtyPack, argclstransferIn.UOMID,
                    argclstransferIn.Transferqty, argclstransferIn.Price, argclstransferIn.TotalPrice, argclstransferIn.Status, argclstransferIn.LoggedinBy);
                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }
            }
            return lstrMessage;
        }

        #endregion

        #region Transfer History Page

        public List<SearchTransferInHistory> SearchTransferInHistory(BALTransferIn argclsTransferIn)
        {
            List<SearchTransferInHistory> llstTransferIn = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferIn = dbo.SearchTransferInHistory(argclsTransferIn.ListCorporateID, argclsTransferIn.ListFacilityID, argclsTransferIn.ListCategoryID, argclsTransferIn.DateFrom, argclsTransferIn.DateTo, argclsTransferIn.Status, argclsTransferIn.LoggedinBy).ToList();
            }
            return llstTransferIn;
        }

        public List<SearchTransferOutHistory> SearchTransferOutHistory(BALTransferOut argclsTransferOut)
        {
            List<SearchTransferOutHistory> llstTransferOut = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferOut = dbo.SearchTransferOutHistory(argclsTransferOut.ListCorporateID, argclsTransferOut.ListFacilityID, argclsTransferOut.ListCategoryID, argclsTransferOut.DateFrom, argclsTransferOut.DateTo, argclsTransferOut.ListStatus, argclsTransferOut.LoggedinBy).ToList();
            }
            return llstTransferOut;
        }
        public List<GetTransferInHistoryReport> GetTransferInHistoryReport(string TransferINID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<GetTransferInHistoryReport> llstTransferIn = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstTransferIn = dbo.GetTransferInHistoryReport(TransferINID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstTransferIn;
        }

        public string UpdateTransferDetails(BALTransferOut argclsTRDetails)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                int linecount = 0;
                linecount = dbo.UpdateTransferDetails(argclsTRDetails.TransferID, argclsTRDetails.TransferQty, argclsTRDetails.TotalPrice, argclsTRDetails.ListStatus, argclsTRDetails.Remarks, argclsTRDetails.LastModifiedBy);
                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

            }
            return lstrMessage;
        }

        public List<BindTransferOutHistoryReport> BindTransferOutHistoryReport(string TransferOutID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy)
        {
            List<BindTransferOutHistoryReport> llstMPRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRDetails = dbo.BindTransferOutHistoryReport(TransferOutID, SearchFilters, LockedBy, LoggedinBy).ToList();
            }
            return llstMPRDetails;
        }

        public List<GetCategoryByListFacilityID> GetCategoryByListFacilityID(string FacilityID)
        {
            List<GetCategoryByListFacilityID> llstCatagory = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCatagory = dbo.GetCategoryByListFacilityID(FacilityID).ToList();
            }
            return llstCatagory;
        }

        #endregion

     
        public List<BindFacilityReport> BindFacilityReport(BALFacility arguFacility)
        {
            List<BindFacilityReport> llstbindfacility = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindfacility = dbo.BindFacilityReport(arguFacility.SearchText, arguFacility.Active, arguFacility.LogginBy, arguFacility.Filter).ToList();
            }
            return llstbindfacility;
        }
        public List<BindFacilityDetailsReport> BindFacilityDetailsReport(BALFacility arguFacility)
        {
            List<BindFacilityDetailsReport> llstbindfacilityDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindfacilityDetails = dbo.BindFacilityDetailsReport(arguFacility.FacilityID, arguFacility.LogginBy).ToList();
            }
            return llstbindfacilityDetails;
        }

        public List<BindFacilityVendorAccountReport> BindFacilityVendorAccountReport(BALFacilityVendorAccount argclsFacilityVendorAcct)
        {
            List<BindFacilityVendorAccountReport> lclFacilityvendorAcc = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lclFacilityvendorAcc = dbo.BindFacilityVendorAccountReport(argclsFacilityVendorAcct.ListFacilityID, argclsFacilityVendorAcct.ListVendorID, argclsFacilityVendorAcct.IsStrActive, argclsFacilityVendorAcct.LoggedIN, argclsFacilityVendorAcct.Filter).ToList();
            }
            return lclFacilityvendorAcc;
        }

        public List<BindCorporateMasterReport> BindCorporateMasterReport(BALCorporate arguCorporate)
        {
            List<BindCorporateMasterReport> lstcorpreport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstcorpreport = dbo.BindCorporateMasterReport(arguCorporate.SearchText, arguCorporate.Active, arguCorporate.LoggedinBy, arguCorporate.Filter).ToList();
            }
            return lstcorpreport;
        }

        public List<BindVendorItemMapReport> BindVendorItemMapReport(BALItemMap argclsVendorItemMap)
        {
            List<BindVendorItemMapReport> lstvendoritemreport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstvendoritemreport = dbo.BindVendorItemMapReport(argclsVendorItemMap.ListVendorID, argclsVendorItemMap.ListItemCategory, argclsVendorItemMap.IsStrActive, argclsVendorItemMap.LoggedIN, argclsVendorItemMap.Filter).ToList();
            }
            return lstvendoritemreport;
        }

        public List<BindUserSummaryReport> BindUserSummaryReport(Int64 UserID, Int64 LoggedinBy, string Filter)
        {
            List<BindUserSummaryReport> lstUserSummaryReport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstUserSummaryReport = dbo.BindUserSummaryReport(UserID, LoggedinBy, Filter).ToList();
            }
            return lstUserSummaryReport;
        }

        public List<BindTransferOutReport> BindTransferOutReport(Int64 LoggedinBy)
        {
            List<BindTransferOutReport> lstTransferOutReport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstTransferOutReport = dbo.BindTransferOutReport(LoggedinBy).ToList();
            }
            return lstTransferOutReport;
        }

        public List<BindTransferInReport> BindTransferInReport(Int64 LoggedinBy)
        {
            List<BindTransferInReport> lstTransferInReport = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                lstTransferInReport = dbo.BindTransferInReport(LoggedinBy).ToList();
            }
            return lstTransferInReport;
        }
        public List<GetUserRoleForFacility> GetUserRoleForFacility(Int64 FacilityID, string ListRoleID)
        {
            List<GetUserRoleForFacility> llstbindfacility = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindfacility = dbo.GetUserRoleForFacility(FacilityID, ListRoleID).ToList();
            }
            return llstbindfacility;
        }

        public List<BindRolesForFacility> BindRolesForFacility(Int64 FacilityID)
        {
            List<BindRolesForFacility> llstbindfacility = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindfacility = dbo.BindRolesForFacility(FacilityID).ToList();
            }
            return llstbindfacility;
        }


        public List<GetVendorReport> GetVendorReport(BALVendor ardvendor)
        {
            List<GetVendorReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetVendorReport(ardvendor.VendorUIID, ardvendor.VendorName, ardvendor.IsStrActive, ardvendor.LoggedinBy, ardvendor.Filter).ToList();
            }
            return llstCRDetails;
        }
        public List<GETVendorUniqueName> GETVendorUniqueName(string VendorCode)
        {
            List<GETVendorUniqueName> llstbindVendorUName = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstbindVendorUName = dbo.GETVendorUniqueName(VendorCode).ToList();
            }
            return llstbindVendorUName;
        }
      
        public List<GetVendorItemMappingPage> GetVendorItemMappingPage()
        {
            List<GetVendorItemMappingPage> llstVendorDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorDetails = dbo.GetVendorItemMappingPage().ToList();
            }
            return llstVendorDetails;
        }



        public List<GetVendorDetailsReport> GetVendorDetailsReport(BALVendor argvendor)
        {
            List<GetVendorDetailsReport> llstVendetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendetails = dbo.GetVendorDetailsReport(argvendor.VendorID, argvendor.LoggedinBy, argvendor.Filter).ToList();
            }
            return llstVendetails;
        }


        public List<Validgpbillcode> Validgpbillcode(string GpBillingCode)
        {
            List<Validgpbillcode> listItem = new List<Validgpbillcode>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.Validgpbillcode(GpBillingCode).ToList();
            }

            return listItem;
        }

        public List<Validuseremail> Validuseremail(string Email)
        {
            List<Validuseremail> listItem = new List<Validuseremail>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.Validuseremail(Email).ToList();
            }

            return listItem;
        }

        #region Report Summary & Details

        public List<GetUserPermissionReport> GetUserPermissionReport(string MainMenuID, string SubMenuID, string SearchFilter, Int64 LoggedInBy)
        {
            List<GetUserPermissionReport> listUserPermissionReport = new List<GetUserPermissionReport>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listUserPermissionReport = dbo.GetUserPermissionReport(MainMenuID, SubMenuID, SearchFilter, LoggedInBy).ToList();
            }

            return listUserPermissionReport;
        }


        public List<GetVendorOrderDueReport> GetVendorOrderDueReport(BALVendorOrderDue argvendorder)
        {
            List<GetVendorOrderDueReport> listVendorOrderDueReport = new List<GetVendorOrderDueReport>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listVendorOrderDueReport = dbo.GetVendorOrderDueReport(argvendorder.ListCorporateID, argvendorder.ListFacilityID, argvendorder.ListVendorID, argvendorder.DateFrom, argvendorder.DateTo, argvendorder.OrderTypestr, argvendorder.Filter, argvendorder.LoggedInBy).ToList();
            }

            return listVendorOrderDueReport;
        }

        public List<GetFacilitySuppliesMapReport> GetFacilitySuppliesMapReport(Int64 CorporateID, Int64 FacilityID, string VendorID, string CategoryID, Int64 Parlevel, string SearchFilter, Int64 LoggedInBy)
        {
            List<GetFacilitySuppliesMapReport> listFacilitySuppliesMapReport = new List<GetFacilitySuppliesMapReport>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listFacilitySuppliesMapReport = dbo.GetFacilitySuppliesMapReport(CorporateID, FacilityID, VendorID, CategoryID, Parlevel, SearchFilter, LoggedInBy).ToList();
            }

            return listFacilitySuppliesMapReport;
        }



        #endregion


        public List<GetItemDescName> GetItemDescName(string ItemDescription)
        {
            List<GetItemDescName> listItem = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetItemDescName(ItemDescription).ToList();
            }

            return listItem;
        }

        public List<GetCategoryReport> GetCategoryReport(BALPGroup argitemCat)
        {
            List<GetCategoryReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetCategoryReport(argitemCat.SearchItem, argitemCat.IsStrActive, argitemCat.LoggedinBy, argitemCat.Filter).ToList();
            }
            return llstCRDetails;
        }
        
        public List<GetItemSummaryReport> GetItemSummaryReport(BALItem argitem)
        {
            List<GetItemSummaryReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetItemSummaryReport(argitem.CategorylistID, argitem.ItemID, argitem.ItemDescription, argitem.IsStrActive, argitem.LoggedinBy, argitem.Filter).ToList();
            }
            return llstCRDetails;
        }

        public List<GetItemDetailsReport> GetItemDetailsReport(BALItem argitem)
        {
            List<GetItemDetailsReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetItemDetailsReport(argitem.ItemID, argitem.LoggedinBy, argitem.Filter).ToList();
            }
            return llstCRDetails;
        }

        public List<GetMachineMasterReport> GetMachineMasterReport(BALMachinemaster argmach)
        {
            List<GetMachineMasterReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetMachineMasterReport(argmach.FacilityID, argmach.IstrActive, argmach.LastModifiedBy, argmach.Filter).ToList();
            }
            return llstCRDetails;
        }

        public List<GetMachineMasterDetailsReport> GetMachineMasterDetailsReport(BALMachinemaster argmachine)
        {
            List<GetMachineMasterDetailsReport> llstCRDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCRDetails = dbo.GetMachineMasterDetailsReport(argmachine.MachineID, argmachine.LoggedinBy, argmachine.Filter).ToList();
            }
            return llstCRDetails;
        }
        public List<ValidateMedicalSuppliesOrder> ValidateMedicalSuppliesOrder(Int64 PMasterID)
        {
            List<ValidateMedicalSuppliesOrder> llstValDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstValDetails = dbo.ValidateMedicalSuppliesOrder(PMasterID).ToList();
            }
            return llstValDetails;
        }

        public List<GetVendorOrderdueRemainderReport> GetVendorOrderdueRemainderReport(BALVendorOrderDue argclsVendorOrderDue)
        {
            List<GetVendorOrderdueRemainderReport> llstValDetails = null;
            argclsVendorOrderDue.AppType = "App";
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstValDetails = dbo.GetVendorOrderdueRemainderReport(argclsVendorOrderDue.CorporateName, argclsVendorOrderDue.FacilityName, argclsVendorOrderDue.DateFrom, argclsVendorOrderDue.DateTo, argclsVendorOrderDue.AppType, argclsVendorOrderDue.LoggedInBy).ToList();
            }
            return llstValDetails;
        }

        public List<SearchFacilityVendorAcct> GetFacilityVendorAcct(BALFacilityVendorAccount argclsFacilityVendorAcct)
        {
            List<SearchFacilityVendorAcct> llstFacilityVendorAcct = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstFacilityVendorAcct = dbo.SearchFacilityVendorAcct(argclsFacilityVendorAcct.ListFacilityID, argclsFacilityVendorAcct.ListVendorID, argclsFacilityVendorAcct.IsStrActive, argclsFacilityVendorAcct.LoggedIN, argclsFacilityVendorAcct.Filter).ToList();
            }
            return llstFacilityVendorAcct;
        }

        public List<SearchVendorItemMap> GetVendorItemMap(BALItemMap argclsVendorItemMap)
        {
            List<SearchVendorItemMap> llstVendorItemMap = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorItemMap = dbo.SearchVendorItemMap(argclsVendorItemMap.ListVendorID, argclsVendorItemMap.ListItemCategory, argclsVendorItemMap.IsStrActive, argclsVendorItemMap.LoggedIN, argclsVendorItemMap.Filter).ToList();
            }
            return llstVendorItemMap;
        }

        public List<SearchUserDetails> SearchUserDetails(BALUser argclsUserDetails)
        {
            List<SearchUserDetails> llstSearchUser = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstSearchUser = dbo.SearchUserDetails(argclsUserDetails.ListCorporateID, argclsUserDetails.ListFacilityID, argclsUserDetails.ListRoleID, argclsUserDetails.UserName, argclsUserDetails.Active, argclsUserDetails.LoggedinBy, argclsUserDetails.Filter).ToList();
            }
            return llstSearchUser;
        }

        public string DeleteUserDetails(Int64 UserID, Int64 LastModifiedBy)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteUserDetails(UserID, LastModifiedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }

        public List<chkvalidcorporate> chkvalidcorporate(BALCorporate argcorporate)
        {
            List<chkvalidcorporate> listItem = new List<chkvalidcorporate>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.chkvalidcorporate(argcorporate.CorporateID, argcorporate.LoggedinBy, argcorporate.Filter).ToList();
            }

            return listItem;
        }


        public List<GetCategoryByListVendorID> GetCategoryByListVendorID(string ListVendorID)
        {
            List<GetCategoryByListVendorID> listItem = new List<GetCategoryByListVendorID>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.GetCategoryByListVendorID(ListVendorID).ToList();
            }

            return listItem;
        }

        public List<SearchCapitalReceivingSummaryReport> SearchCapitalReceivingSummaryReport(BALCapitalReceiving argclsCRMasterReceive)
        {
            List<SearchCapitalReceivingSummaryReport> llstCR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstCR = dbo.SearchCapitalReceivingSummaryReport(argclsCRMasterReceive.ListCorporateID, argclsCRMasterReceive.ListFacilityID, argclsCRMasterReceive.ListVendorID, argclsCRMasterReceive.DateFrom, argclsCRMasterReceive.DateTo, argclsCRMasterReceive.Status, argclsCRMasterReceive.LoggedinBy).ToList();
            }
            return llstCR;
        }
        public List<SearchITReceivingSummaryReport> SearchITReceivingSummaryReport(BALRequestITReceiving argInsITRequestMaster)
        {
            List<SearchITReceivingSummaryReport> llstMPRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPRMaster = dbo.SearchITReceivingSummaryReport(argInsITRequestMaster.CorporateIDs, argInsITRequestMaster.FacilityIDs, argInsITRequestMaster.VendorIDs, argInsITRequestMaster.DateFrom, argInsITRequestMaster.DateTo, argInsITRequestMaster.FinalStatus, argInsITRequestMaster.LoggedinBy).ToList();
            }
            return llstMPRMaster;
        }
        public List<SearchMedicalSuppliesReceivingSummaryReport> SearchMedicalSuppliesReceivingSummaryReport(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec)
        {
            List<SearchMedicalSuppliesReceivingSummaryReport> llstMSRMaster = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMSRMaster = dbo.SearchMedicalSuppliesReceivingSummaryReport(argclsMedicalSuppliesRec.CorporateName, argclsMedicalSuppliesRec.FacilityName, argclsMedicalSuppliesRec.VendorName, argclsMedicalSuppliesRec.DateFrom, argclsMedicalSuppliesRec.DateTo, argclsMedicalSuppliesRec.Status, argclsMedicalSuppliesRec.LoggedinBy).ToList();
            }
            return llstMSRMaster;
        }
        public List<SearchMachinePartsReceiveSummaryReport> SearchMachinePartsReceiveSummaryReport(BALMachinePartsReceiveOrder argclsMPRMaster)
        {
            List<SearchMachinePartsReceiveSummaryReport> llstMPR = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstMPR = dbo.SearchMachinePartsReceiveSummaryReport(argclsMPRMaster.ListCorporateID, argclsMPRMaster.ListFacilityID, argclsMPRMaster.ListVendorID, argclsMPRMaster.DateFrom, argclsMPRMaster.DateTo, argclsMPRMaster.FinalStatus, argclsMPRMaster.LoggedinBy).ToList();
            }
            return llstMPR;
        }
        public List<ValidFaciliityAccountCode> ValidFaciliityAccountCode(string FacilityAccountCode)
        {
            List<ValidFaciliityAccountCode> llstValDetails = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstValDetails = dbo.ValidFaciliityAccountCode(FacilityAccountCode).ToList();
            }
            return llstValDetails;
        }

        public List<SearchVendorOrderType> SearchVendorOrderType(BALVendorOrderDue argclsvendorOrderDue)
        {
            List<SearchVendorOrderType> llstVendorOrderType = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstVendorOrderType = dbo.SearchVendorOrderType(argclsvendorOrderDue.ListFacilityID, argclsvendorOrderDue.ListVendorID, argclsvendorOrderDue.DateFrom, argclsvendorOrderDue.DateTo, argclsvendorOrderDue.Filter, argclsvendorOrderDue.LoggedInBy).ToList();
            }
            return llstVendorOrderType;
        }


        public List<GetVendorOrderDue> GetVendorOrderDue(BALVendorOrderDue argclsvendorOrderDue)
        {
            List<GetVendorOrderDue> listVendorOrderDueReport = new List<GetVendorOrderDue>();
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listVendorOrderDueReport = dbo.GetVendorOrderDue(argclsvendorOrderDue.VendorOrderdueID, argclsvendorOrderDue.CorporateID, argclsvendorOrderDue.FacilityID, argclsvendorOrderDue.VendorID, argclsvendorOrderDue.OrderType, argclsvendorOrderDue.OrderDueDate, argclsvendorOrderDue.DeliveryDate, argclsvendorOrderDue.DeliveryWindow, argclsvendorOrderDue.DaytoToNotify).ToList();
            }
            return listVendorOrderDueReport;
        }


        public List<CheckVendorOrderDue> CheckVendorOrderDue(Int64 CorporateID, Int64 FacilityID, Int64 VendorID)
        {
            List<CheckVendorOrderDue> listItem = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                listItem = dbo.CheckVendorOrderDue(CorporateID, FacilityID, VendorID).ToList();
            }

            return listItem;
        }

        public List<GetItem> GetItem()
        {
            List<GetItem> llstproductgroup = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstproductgroup = dbo.GetItem().ToList();
            }
            return llstproductgroup;
        }

        public List<GetVendor> GetVendor()
        {
            List<GetVendor> llstproductgroup = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                llstproductgroup = dbo.GetVendor().ToList();
            }
            return llstproductgroup;
        }

        //public List<GetMonthlyUsageReport> GetMonthlyUsageReport(BALReport argreport)
        //{
        //    List<GetMonthlyUsageReport> llstMPR = null;
        //    using (InventoryEntities dbo = new InventoryEntities())
        //    {
        //        llstMPR = dbo.GetMonthlyUsageReport(argreport.CorporateID,argreport.FacilityID,argreport.VendorID,argreport.ItemCategoryID,argreport.MonthYear,argreport.DateFrom,argreport.DateTo,argreport.LoggedInBy,argreport.Filter).ToList();
        //    }
        //    return llstMPR;
        //}

        private DataSourceCommunicator db = new DataSourceCommunicator(DataSourceCommunicator.ParamType.ServerCredentials, ConfigurationManager.AppSettings["dbServerName"], ConfigurationManager.AppSettings["dbUsername"], ConfigurationManager.AppSettings["dbPassword"]);


        public DataSet GetMonthlyUsageReport(BALReport argreport)
        {
            //List<object> llst = new List<object>();

            try
            {

                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
                db.AddParameter("@ItemID", argreport.ItemID);
                db.AddParameter("@OrderType", argreport.OrderType);
                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetMonthlyUsageReport", "");

                //llst.Insert(0, "Success");
                //llst.Insert(1, ds);

                //return llst;
                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);

                //return llst;
                throw;
            }





            //using (InventoryEntities dbo = new InventoryEntities())
            //{
            //    llstMPR = dbo.GetMonthlyUsageReport(argreport.CorporateID, argreport.FacilityID, argreport.VendorID, argreport.ItemCategoryID, argreport.MonthYear, argreport.DateFrom, argreport.DateTo, argreport.LoggedInBy, argreport.Filter).ToList();
            //}
            //return llstMPR;
        }


        public DataSet GetMonthlyPurchaseReport(BALReport argreport)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
                db.AddParameter("@ItemID", argreport.ItemID);
                db.AddParameter("@OrderType", argreport.OrderType);
                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetMonthlyPurchaseReport", "");

                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);
                throw;
            }


        }

        public DataSet GetMonthlyEndingReport(BALReport argreport)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
                db.AddParameter("@ItemID", argreport.ItemID);
                db.AddParameter("@OrderType", argreport.OrderType);
                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetMonthlyEndingReport", "");


                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);
                throw;
            }


        }

        public DataSet GetCumUsageReport(BALReport argreport)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetCumUsageReport", "");

                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);
                throw;
            }


        }

        public DataSet GetCumulativeUsageReportBySingleItem(BALReport argreport)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemID", argreport.ItemID);


                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetCumulativeusageBySingleItemReport", "");

                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);
                throw;
            }


        }

        public DataSet GetMonthlyInventoryReport(BALReport argreport)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@CorporateID", argreport.CorporateID);
                db.AddParameter("@FacilityID", argreport.FacilityID);
                db.AddParameter("@VendorID", argreport.VendorID);
                db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
                db.AddParameter("@DateFrom", argreport.DateFrom);
                db.AddParameter("@DateTo", argreport.DateTo);
                db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
                db.AddParameter("@Filter", argreport.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetMonthlyInventoryReport", "");

                return ds;
            }
            catch (Exception ex)
            {
                db.writeLogToFile("GetMonthlyUsageReport", ex.Message);
                throw;
            }


        }


        public DataSet GetCostPertxReport(BALReport argreport)
        {
            DataSet ds = new DataSet();

            db.AddParameter("@CorporateID", argreport.CorporateID);
            db.AddParameter("@FacilityID", argreport.FacilityID);
            db.AddParameter("@VendorID", argreport.VendorID);
            db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
            db.AddParameter("@ItemID", argreport.ItemID);
            db.AddParameter("@DateFrom", argreport.DateFrom);
            db.AddParameter("@DateTo", argreport.DateTo);
            db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
            db.AddParameter("@Filter", argreport.Filter);

            ds = db.ExecuteStoredProcedureAsDataSet("GetCostPertxReport", "");

            return ds;

        }

        public DataSet GetMonthlyDrugReport(BALReport argreport)
        {
            DataSet ds = new DataSet();

            db.AddParameter("@CorporateID", argreport.CorporateID);
            db.AddParameter("@FacilityID", argreport.FacilityID);
            db.AddParameter("@VendorID", argreport.VendorID);
            db.AddParameter("@ItemCategoryID", argreport.ItemCategoryID);
            db.AddParameter("@ItemID", argreport.ItemID);
            db.AddParameter("@DateFrom", argreport.DateFrom);
            db.AddParameter("@DateTo", argreport.DateTo);
            db.AddParameter("@LoggedinBy", argreport.LoggedInBy);
            db.AddParameter("@Filter", argreport.Filter);

            ds = db.ExecuteStoredProcedureAsDataSet("GetMonthlyDrugReport", "");

            return ds;

        }

        public string DeleteVendorOrderDue(BALVendorOrderDue argvendor)
        {
            string lstrMessage = string.Empty;
            using (InventoryEntities dbo = new InventoryEntities())
            {
                try
                {
                    dbo.DeleteVendorOrderDue(argvendor.CorporateID, argvendor.FacilityID, argvendor.VendorID, argvendor.OrderDueDate, argvendor.IsActive, argvendor.LastModifitedBy);
                    lstrMessage = "Deleted Successfully";
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }
            }
            return lstrMessage;
        }
        public List<BindPermissionforUser> BindPermissionforUser()
        {
            List<BindPermissionforUser> llstuser = null;
            using (InventoryEntities dbo = new InventoryEntities())
            {

                llstuser = dbo.BindPermissionforUser().ToList();
            }
            return llstuser;
        }


        #region Corporate Master

        public List<BindCorporateMaster> BindCorporateMaster()
        {
            try
            {

                DataSet ds = new DataSet();

                ds = db.ExecuteStoredProcedureAsDataSet("BindCorporateMaster", "");

                DataTable dtt = ds.Tables[0];

                List<BindCorporateMaster> llstCorDetails = ConvertDstoList.DataTableToList<BindCorporateMaster>(dtt);
                return llstCorDetails;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("BindCorporateMaster", ex.Message);
                throw;
            }

        }


        public List<SearchCorporateMaster> SearchCorporateMaster(BALCorporate argclsCorporate)
        {

            try
            {

                DataSet ds = new DataSet();

                db.AddParameter("@CorporateCode", argclsCorporate.CorporateName);
                db.AddParameter("@CorporateDescription", argclsCorporate.CorporateDescription);
                db.AddParameter("@IsActive", argclsCorporate.Active);
                db.AddParameter("@LoggedinBy", argclsCorporate.LoggedinBy);
                db.AddParameter("@Filter", argclsCorporate.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("SearchCorporateMaster", "");

                DataTable dtt = ds.Tables[0];

                List<SearchCorporateMaster> llstCorp = ConvertDstoList.DataTableToList<SearchCorporateMaster>(dtt);

                return llstCorp;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("SearchCorporateMaster", ex.Message);
                throw;
            }
        }

        public string InsertCorporateMaster(BALCorporate argclsCorporate)
        {
            try
            {
                string lstrMessage = string.Empty;
                int linecount = 0;

                db.AddParameter("@CorporateName", argclsCorporate.CorporateName);
                db.AddParameter("@CorporateDescription", argclsCorporate.CorporateDescription);
                db.AddParameter("@POEmail", argclsCorporate.POEmail);
                db.AddParameter("@CreatedBy", argclsCorporate.CreatedBy);

                linecount = db.ExecuteNonQuery("InsertCorporateMaster", "");

                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

                return lstrMessage;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("InsertCorporateMaster", ex.Message);
                throw;
            }

        }



        public string UpdateCorporateMaster(BALCorporate argclsCorporate)
        {
            try
            {
                string lstrMessage = string.Empty;
                int linecount = 0;
                db.AddParameter("@CorporateID", argclsCorporate.CorporateID);
                db.AddParameter("@CorporaeName", argclsCorporate.CorporateName);
                db.AddParameter("@CorporateDescription", argclsCorporate.CorporateDescription);
                db.AddParameter("@POEmail", argclsCorporate.POEmail);
                db.AddParameter("@LastModifiedBy", argclsCorporate.LastModifiededBy);
                db.AddParameter("@IsActive", argclsCorporate.IsActive);

                linecount = db.ExecuteNonQuery("UpdateCorporateMaster", "");

                if (linecount > 0)
                {
                    lstrMessage = "Updated Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

                return lstrMessage;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("UpdateCorporateMaster", ex.Message);
                throw;
            }

        }


        public string DeleteCorporateDetails(Int64 CorporateID, bool IsActive, Int64 LastModifiedBy)
        {
            try
            {
                string lstrMessage = string.Empty;
                int linecount = 0;
                db.AddParameter("@CorporateID", CorporateID);
                db.AddParameter("@LastModitifiedBy", LastModifiedBy);
                db.AddParameter("@IsActive", IsActive);

                linecount = db.ExecuteNonQuery("DeleteCorporateDetails", "");

                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

                return lstrMessage;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("DeleteCorporateDetails", ex.Message);
                throw;
            }
        }


        #endregion

        #region Uom
        public List<GetUomReport> GetUomReport(BALUom arguom)
        {

            try
            {
                DataSet ds = new DataSet();
                db.AddParameter("@UomName", arguom.UomName);
                db.AddParameter("@IsActive", arguom.IsActivestr);
                db.AddParameter("@LoggedinBy", arguom.LastModifiedBy);
                db.AddParameter("@Filter", arguom.Filter);

                ds = db.ExecuteStoredProcedureAsDataSet("GetUomReport", "");

                DataTable dtt = ds.Tables[0];
                List<GetUomReport> llstCorp = ConvertDstoList.DataTableToList<GetUomReport>(dtt);
                return llstCorp;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("GetUomReport", ex.Message);
                throw;
            }
        }


        public List<GetUOMName> GetUOMName(string VendorCode)
        {

            try
            {
                DataSet ds = new DataSet();
                db.AddParameter("@UOM", VendorCode);
                ds = db.ExecuteStoredProcedureAsDataSet("GetUOMName", "");

                DataTable dtt = ds.Tables[0];
                List<GetUOMName> llstCorp = ConvertDstoList.DataTableToList<GetUOMName>(dtt);
                return llstCorp;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("GetUOMName", ex.Message);
                throw;
            }
        }

        public List<GetUom> GetUom()
        {

            try
            {
                DataSet ds = new DataSet();

                ds = db.ExecuteStoredProcedureAsDataSet("GetUom", "");

                DataTable dtt = ds.Tables[0];
                List<GetUom> llstCorp = ConvertDstoList.DataTableToList<GetUom>(dtt);
                return llstCorp;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("GetUom", ex.Message);
                throw;
            }
        }
     
        public string InsertUOM(BALUom arguom)
        {
            try
            {
                string lstrMessage = string.Empty;
                int linecount = 0;

                db.AddParameter("@UomID", arguom.UomID);
                db.AddParameter("@UomName", arguom.UomName);
                db.AddParameter("@CreatedBy", arguom.CreatedBy);
                db.AddParameter("@CreatedOn", arguom.CreatedOn);
                db.AddParameter("@LastModifiedBy", arguom.LastModifiedBy);
                db.AddParameter("@LastModifiedOn", "");
                db.AddParameter("@IsActive", arguom.IsActive);

                linecount = db.ExecuteNonQuery("InsertUOM", "");

                if (linecount > 0)
                {
                    lstrMessage = "Saved Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

                return lstrMessage;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("InsertUOM", ex.Message);
                throw;
            }

        }


        public string DeleteUom(Int64 UomID, Int64 UserID, bool IsActive)
        {
            try
            {
                string lstrMessage = string.Empty;
                int linecount = 0;

                db.AddParameter("@UomID", UomID);
                db.AddParameter("@LastModifiedBy", UserID);
                db.AddParameter("@IsActive", IsActive);

                linecount = db.ExecuteNonQuery("DeleteUom", "");

                if (linecount > 0)
                {
                    lstrMessage = "Deleted Successfully";
                }
                else
                {
                    lstrMessage = "Error:Server issue";
                }

                return lstrMessage;
            }
            catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("DeleteUom", ex.Message);
                throw;
            }

        }


        public DataSet GetUserCredentials(BALUser argUser)
        {
            try
            {
                DataSet ds = new DataSet();

                db.AddParameter("@UserName", argUser.UserName);


                ds = db.ExecuteStoredProcedureAsDataSet("GetUserCredentials", "");

                return ds;
            }
             catch (Exception ex)
            {
                EventLogger log = new EventLogger(config);
                log.LogException(ex);
                //db.writeLogToFile("BindCorporateMaster", ex.Message);
                throw;
            }

        }

        #endregion


        public string ActivityTracker(BALActivityTracking argactivitytrack)
        {
            string lstmsg = string.Empty;
            db.AddParameter("@UserID", argactivitytrack.UserID);
            //db.AddParameter("@MainMenuID", argactivitytrack.MainMenuID);
            //db.AddParameter("@SubMenuID", argactivitytrack.SubMenuID);
            db.AddParameter("@AppFeature", argactivitytrack.AppFeature);
            db.AddParameter("@FacilityID", argactivitytrack.FacilityID);
            //db.AddParameter("@TimeIn", argactivitytrack.TimeIn);
            //db.AddParameter("@TimeOut", argactivitytrack.TimeOut);
            //db.AddParameter("@DurationMinutes", argactivitytrack.DurationMinutes);
            //db.AddParameter("@DateLoggedIn", argactivitytrack.DateLoggedIn);
            //db.AddParameter("@DateLoggedOut", argactivitytrack.DateLoggedOut);
            db.AddParameter("@MachineID", argactivitytrack.MachineID);
            db.AddParameter("@IPAddress", argactivitytrack.IPAddress);

            int lincount = db.ExecuteNonQuery("ActivityTracker");

            if(lincount> 0)
            {
                lstmsg = "Save Successfully";
            }

            return lstmsg;
        }
        
    }
}
