using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Inventroy.Service.BAL;
using System.Data;

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   IInventoryService.cs
'' Type      :   C# File
'' Description  :<<To add,update the Facility Vendor Account Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				 S.Vivekanand                       New
 *  08/11/2017         V1.0              S.Vivekanand             Created new sevice function for facility supplies map
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInventoryService" in both code and config file together.
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        List<GetLoginDetails> GetLoginDetails(BALUser argclsUser);
        [OperationContract]
        bool IsUserExist(string argstrUserName, string argstrUserPassword);

        [OperationContract]
        List<BALUser> GetUserIDDetails(string argclsUserName);

        [OperationContract]
        List<GetUserInformation> GetUserInformation();

        [OperationContract]
        string InsertUpdateVendorDetails(BALVendor argclsvendor);

        [OperationContract]
        List<GetvendorDetails> GetvendorDetails(string SearchText);

        [OperationContract]
        string DeleteVendor(Int64 argvendorID, bool IsActive, Int64 UpdatedBy);

        [OperationContract]
        string InsertUser(BALUser argclsUser, Int64 argintCreatedBy);

        //[OperationContract]
        //List<BALUser> GetExistUsers();

        [OperationContract]
        List<BindExistuser> BindExistuser();

        [OperationContract]
        List<BALUser> GetUserRole();

        [OperationContract]
        List<GetUserDetails> GetUserDetails(Int64 UserID);

        [OperationContract]
        List<GetUserRoleandFacility> GetRoleandFacility(Int64 UserID);

        [OperationContract]
        string RemoveFacilityRole(Int64 arguserRoleID);

        [OperationContract]
        List<BindPermission> BindPermission();

        [OperationContract]
        List<GetPermission> GetPermission(Int64 MainmenuID, Int64 SubmenuID);

        [OperationContract]
        List<GetdrpMainMenu> GetdrpMainMenu();

        [OperationContract]
        List<GetdrpSubMenu> GetdrpSubMenu(Int64 MainmenuID);

        [OperationContract]
        string InsertUpdatePermission(BALUser argclspermission);

        [OperationContract]
        List<GetState> GetState();

        [OperationContract]
        List<IsUserExist> GetExistUser(string UserName);

        [OperationContract]
        List<GetItemCategory> GetItemCategory();

        [OperationContract]
        List<SearchItemCategory> SearchItemCategory(BALPGroup argpgroup);

        [OperationContract]
        string DeleteItemCategory(Int64 CategoryID, Int64 UserID, bool IsActive);

        [OperationContract]
        string InsertUpdateCategory(BALPGroup argpgname);

        [OperationContract]
        List<ddlLoadValues> ddlLoadValues(Int64 TableID);

        [OperationContract]
        List<BindItem> binditem(string SearchItem);

        [OperationContract]
        string InsertUpdateItem(BALItem argitem);

        [OperationContract]
        string DeleteItem(Int64 ItemUniqueID, bool IsActive, Int64 LastModifiedBy);

        //[OperationContract]
        //string InsertCategory(BALItem argcat);

        [OperationContract]
        string InsertUOM(BALUom arguom);

        [OperationContract]
        string InsertItemMap(BALItemMap argitemmap);

        [OperationContract]
        string DeleteItemMapping(long ItemMappID, bool IsActive, long LastModifiedBy);

        [OperationContract]
        List<GetItemMap> GetItemMap();

        [OperationContract]
        List<GetItemMapping> GetItemMapping(Int64 ItemID);

        [OperationContract]
        List<GetItemDRD> GetItemDRD();

        [OperationContract]
        List<GetDrpItemsByCategory> GetDrpItemsByCategory(Int64 categoryID);

        [OperationContract]
        List<ddlCurrency> ddlCurrency();

        [OperationContract]
        string InsertCurrency(BALItem argcur);

        [OperationContract]
        List<BALUser> GetCorporateMaster();

        [OperationContract]
        List<BALUser> GetCorporateFacilityByUserID(Int64 UserID);

        [OperationContract]
        string InsertRole(BALUser argrole);

        [OperationContract]
        List<GetUom> GetUom();

        [OperationContract]
        string DeleteUom(Int64 UomID, Int64 UserID, bool IsActive);

        [OperationContract]
        List<GetCorporateFacility> GetCorporateFacility(Int64 CorporateID);

        [OperationContract]
        List<UserFacilityRole> GetFacilityRole();

        [OperationContract]
        string DeleteGPBilling(Int64 GPBillingID);

        [OperationContract]
        string InsertGPBilling(BALGPBill arggpb);

        [OperationContract]
        List<GetGPBilling> GetGPBilling();

        [OperationContract]
        List<GetUserPagePermission> GetLoginPermissionDetails(Int64 UserRoleID);

        [OperationContract]
        List<GetFacility> GetFacility();

        [OperationContract]
        string DeleteFacilityItemMap(Int64 argclsFacilityItemID, Int64 argclsUserID, Boolean IsActive);

        [OperationContract]
        string InsertUpdateFacilityItemMap(BALFaItemMap argclsFacility);

        [OperationContract]
        List<GetFacilityItemMap> GetFacilityItemMap();

        [OperationContract]
        string InsertFacility(BALFacility argclsFacility, Int64 argintCreatedBy);

        [OperationContract]
        List<BindFacility> BindFacility(BALFacility argFacility);

        [OperationContract]
        string DeleteFacility(Int64 argclsFacilityID, Int64 argclsUserID, Boolean IsActive);

        [OperationContract]
        List<GetItemsbyFacilityID> GetItemsbyFacilityID(Int64 FacilityID);

        [OperationContract]
        List<GetFacilityShortName> GetFacilityShortName();

        //[OperationContract]
        //List<BindSearchItem> binditemSearch(string searchItem);

        //[OperationContract]
        //List<BindSearchFacility> BindSearchFacility(string searchItem);

        [OperationContract]
        string DeleteFacilityVendorAccount(Int64 argclsFacilityVendorAccID, Int64 argclsUserID, Boolean IsActive);

        [OperationContract]
        string InsertUpdateFacilityVendorAccount(BALFacilityVendorAccount argclsFacilityVendorAcc);

        [OperationContract]
        List<GetFacilityVendorAccount> GetFacilityVendorAccount(string SearchItem);

        [OperationContract]
        string InsertUpdateVendorOrderDue(BALVendorOrderDue argclsvendorOrderDue);
        [OperationContract]
        List<ValidateVendorOrderType> ValidateVendorOrderType(BALVendorOrderDue argclsvendorOrderDue);
        //[OperationContract]
        //List<BindVendorOrderDue> BindVendorOrderDue();

        [OperationContract]
        List<BindVendorOrderDue> BindVendorOrderDue(BALFacilitySupply argclsvendorOrderDue);
        [OperationContract]
        string InsertUpdateFacilitySupply(BALFacilitySupply argclsFacilitySupply);
        [OperationContract]
        List<GetFacilitySupply> GetFacilitySupply(BALFacilitySupply argclsFacilitySupply);

        [OperationContract]
        List<GetFacilitySupplyGird> GetFacilitySupplyGird(BALFacilitySupply argclsFacilitySupply);
        [OperationContract]
        List<GetEquipmentCategory> GetEquipmentCategory(Int64 CorporateID, string Mode);
        [OperationContract]
        List<GetEquipementList> GetEquipementList(Int64 EquipSubCategoryID, string Mode);
        [OperationContract]
        List<object> InsertEquipmentCategory(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        string UpdateEquipmentcategory(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        string InsertEquipmentList(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        string UpdateEquipmentList(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        List<GetMachinemasterDetails> GetMachinemasterDetails();
        [OperationContract]
        string DeleteMachinemasterDetails(Int64 argmachineID, bool IsActive, Int64 UpdatedBy);
        [OperationContract]
        string DeleteEquipeCategoryMaster(Int64 argCategoryID, bool IsActive, Int64 UpdatedBy);
        [OperationContract]
        string DeleteEquipListMaster(Int64 argListID, bool IsActive, Int64 UpdatedBy);
        [OperationContract]
        string InsertMachineMaster(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        string UpdateMachineMaster(BALMachinemaster argclsmachinemaster);
        [OperationContract]
        List<GetMachineMasterbasedMachineID> GetMachinemasterbasedmachineID(long MachineID);
        [OperationContract]
        List<SearchMachinemasterdetails> SearchMachinemasterdetails(BALMachinemaster argstrmach);
        [OperationContract]
        List<CheckEquipmentlist> GetCheckEquipmentlist(Int64 EquipcategoryID);
        [OperationContract]
        List<SavedEquipmentCategory> SavedEquipmentCategory(Int64 FacilityID, Int64 EquipcategoryID);
        [OperationContract]
        List<SavedEquipmentList> SavedEquipmentList(Int64 EquipmentListID);
        [OperationContract]
        List<GetActiveEquipementListvalue> GetActiveEquipementListvalue(Int64 EquiplistID);
        [OperationContract]
        List<GetNonsuperAdminMedicalSupplyMaster> GetNonsuperAdminMedicalSupplyMaster(Int64 CorporateID, Int64 FacilityID);


        // Machine Parts Request Page
        #region functions of Machine Parts Request Page
        [OperationContract]
        List<BindMachinePartsReport> BindMachinePartsReport(string MPRMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        List<GetMPRMaster> GetMPRMaster();
        [OperationContract]
        //BALMPRMaster GetMPRDetailsbyMPRMasterID(Int64 MPRMasterID, Int64 LockedBy);
        List<GetMPRDetailsbyMPRMasterID> GetMPRDetailsbyMPRMasterID(Int64 MPRMasterID, Int64 LockedBy, Int64 LockTimeOut);
        [OperationContract]
        List<GetList> GetList(string ScreenName, string ListName, string Mode);
        [OperationContract]
        List<SearchMPRMaster> SearchMPRMaster(BALMPRMaster argclsMPRMaster);
        [OperationContract]
        List<object> InsertMPRMaster(BALMPRMaster argInsMPRMaster);
        [OperationContract]
        string InsertMPRDetails(BALMPRMaster argInsMPRDetails);
        [OperationContract]
        string UpdateMPRMaster(BALMPRMaster argupdMPRMaster);
        [OperationContract]
        string UpdateMPRDetails(BALMPRMaster argUpdMPRDetails);
        [OperationContract]
        string DeleteMPRDetails(Int64 argMPRDetailsID, bool IsActive, Int64 LastModifiedBy);

        [OperationContract]
        List<ValidMachineEquipment> ValidMachineEquipment(Int64 EquipmentCategory, Int64 EquipementSubcategory, Int64 EquipmentList, Int64 FacilityID);
        #endregion

        // Medical Supplies Request Page
        #region functions of Medical Supplies Request Page
        [OperationContract]
        List<GetmedicalsupplyReviewReport> GetmedicalsupplyReviewReport(string MedicalSupplyMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        List<GetMedicalsuppliesItem> GetMedicalsuppliesItem(Int64 CorporateID, Int64 FacilityID, Int64 VendorID);
        [OperationContract]
        string InsertShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string UpdateShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string DeleteShipping(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string InsertTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string UpdateTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string DeleteTimeDelivery(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        List<GetLookUpList> GetLookUpList(Int64 LookUpID);
        [OperationContract]
        List<GetOrderPeriod> GetOrderPeriod(Int64 CorporateID, Int64 FacilityID, Int64 VendorID, Int32 Ordertype);
        [OperationContract]
        Int64 InsertMedicalsupplyMaster(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string InsertMedicalSuppliesDetail(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        List<GetMedicalSupplyRequestMaster> GetMedicalSupplyRequestMaster();
        [OperationContract]
        List<SearchMedicalSupplyRequest> SearchMedicalSupplyRequest(BALMedicalSuppliesRequest argclsMedicalSuppliesReq);
        [OperationContract]
        List<BindMedicalsupplymasterandDetail> BindMedicalsupplymasterandDetail(Int64 MedicalSupplyMasterID);
        [OperationContract]
        List<BindMedicalsupplymaster> BindMedicalsupplymaster(Int64 MedicalSupplyMasterID);
        [OperationContract]
        //BALMedicalSuppliesRequest BindMedicalsupplyDetail(Int64 MedicalSupplyMasterID, Int64 LockedBy);
        List<BindMedicalsupplyDetail> BindMedicalsupplyDetail(Int64 MedicalSupplyMasterID, Int64 LockedBy, Int64 LockTimeOut);
        [OperationContract]
        string UpdateMedicalsupplyMaster(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string UpdateMedicalSupplyDetails(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        string DeleteMedicalSuppliesDetails(Int64 argMSRID, Int64 UpdatedBy);
        [OperationContract]
        List<AddMedicalsupplyitem> AddMedicalsupplyitem(BALMedicalSuppliesRequest argclsmedicalsuppliesrequest);
        [OperationContract]
        List<SavedShippingValue> SavedShippingValue();
        [OperationContract]
        List<SavedTimeDeliveryValue> SavedTimeDeliveryValue();
        [OperationContract]
        List<FindDuplicateShippingValue> FindDuplicateShippingValue(string ShippingValue);
        [OperationContract]
        List<FindDuplicateTimeDeliveryValue> FindDuplicateTimeDeliveryValue(string TimeDeliveryValue);
        //[OperationContract]
        //List<GetmedicalsupplyReviewReport> GetmedicalsupplyReviewReport(Int64 MedicalSupplyMasterID);
        #endregion
        // Service Request Page
        #region functions of Service Request Page
        [OperationContract]
        List<BindServiceRequestReport> BindServiceRequestReport(string ServiceRequestMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        List<GetServiceRequestMaster> GetServiceRequestMaster();
        //[OperationContract]
        //BALServiceRequest GetServiceRequestetailsbyServiceRequestMasterID(Int64 SRMasterID, Int64 LockedBy);
        [OperationContract]
        List<GetServiceRequestetailsbyServiceRequestMasterID> GetServiceRequestetailsbyServiceRequestMasterID(Int64 SRMasterID, Int64 LockedBy, Int64 LockTimeOut);
        [OperationContract]
        List<GetServiceCategory> GetServiceCategory(Int64 CorporateID, string Mode);
        [OperationContract]
        List<GetServiceList> GetServiceList(Int64 ServiceCategoryID, string Mode);
        [OperationContract]
        List<SearchServiceRequestMaster> SearchServiceRequestMaster(BALServiceRequest argclsSRMaster);
        [OperationContract]
        List<object> InsertServiceRequestMaster(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string InsertServcieRequestDetails(BALServiceRequest argclsSRDetails);
        [OperationContract]
        string UpdateServiceRequestMaster(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string UpdateServiceRequestDetails(BALServiceRequest argclsSRDetails);
        [OperationContract]
        string DeleteServiceRequestDetails(Int64 argMPRDetailsID, bool IsActive, Int64 LastModifiedBy);
        [OperationContract]
        string InsertServiceCategoryMaster(BALServiceRequest argclsServiceCategory);
        [OperationContract]
        string UpdateServiceCategoryMaster(BALServiceRequest argclsServiceCategory);
        [OperationContract]
        string DeleteServiceCategoryMaster(Int64 argCategoryID, Int64 LastModifiedBy);
        [OperationContract]
        string InsertServiceListMaster(BALServiceRequest argclsServiceList);
        [OperationContract]
        string UpdateServiceListMaster(BALServiceRequest argclsServiceList);
        [OperationContract]
        string DeleteServiceListMaster(Int64 argListID, Int64 LastModifiedBy);
        [OperationContract]
        List<CheckServicelist> GetCheckServicelist(Int64 ServiceCategoryID);
        [OperationContract]
        List<SavedServiceCategory> SavedServiceCategory(Int64 CorporateID, Int64 ServiceCategoryID);
        [OperationContract]
        List<SavedServiceList> SavedServiceList(Int64 ServiceListID);
        // Service Attachment
        [OperationContract]
        List<GetServiceAttachment> GetServiceAttachment(Int64 ServiceUploadID);
        [OperationContract]
        string InsertServiceAttachment(BALServiceRequest argclsServiceList);
        [OperationContract]
        string UpdateServiceAttachment(BALServiceRequest argclsServiceList);
        [OperationContract]
        List<GetActiveServiceListvalue> GetActiveServiceListvalue(Int64 ServicelistID);
        [OperationContract]
        string DeleteServiceAttachment(Int64 argListID, Int64 LastModifiedBy);

        [OperationContract]
        string InsertSRTempAttch(BALServiceRequest argclsServiceList);
        [OperationContract]
        string DeleteSRTempAttch(Int32 SIno, string Mode);
        [OperationContract]
        List<GetServiceTempAttachment> GetServiceTempAttachment(Int32 SiNo);
        [OperationContract]
        string TruncateSRTempAttch();
        //[OperationContract]
        //List<BindServiceRequestReport> BindServiceRequestReport(Int64 ServiceRequestMasterID);
        [OperationContract]
        string AutoUpdateLockedOut(Int64 MasterID, Int64 LockedOutBy, string ScreenName);
        #endregion
        //RequestIT Page
        #region  functions of IT Request Page
        [OperationContract]
        List<object> InsertITRequestMaster(BALRequestIT argInsITRequestMaster);
        [OperationContract]
        string InsertITRequestDetails(BALRequestIT argInsITRequestMaster);
        [OperationContract]
        List<GetRequestITMaster> GetRequestITMaster();
        [OperationContract]
        List<GetNonsuperAdminRequestITMaster> GetNonsuperAdminRequestITMaster(Int64 CorporateID, Int64 FacilityID);
        [OperationContract]
        List<SearchITRequest> SearchITRequest(BALRequestIT argInsITRequestMaster);
        [OperationContract]
        List<GetITRequestDetailsbyMasterID> GetITRequestDetailsbyMasterID(Int64 MPRMasterID, Int64 LockedBy, Int64 LockTimeOut);
        [OperationContract]
        string UpdateITRequestMaster(BALRequestIT argInsITRequestMaster);
        [OperationContract]
        string UpdateITRequestDetails(BALRequestIT argInsITRequestMaster);
        [OperationContract]
        List<BindEquipementsubcategoryFORIT> BindEquipementsubcategory(Int64 CorporateID, string Mode);
        [OperationContract]
        List<BindEquipementListFORIT> BindEquipementListFORIT(Int64 EquipSubcatID, string Mode);
        [OperationContract]
        List<GetSerialNo> GetSerialNo(Int64 EquipmentSubcategoryID, Int64 EquipListID);
        [OperationContract]
        string DeleteITRDetails(Int64 argITRDetailsID, bool IsActive, Int64 LastModifiedBy);
        [OperationContract]
        List<BindRequestITPartsReport> BindRequestITPartsReport(string RequestITMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);

        [OperationContract]
        List<ValidITEquipment> ValidITEquipment(Int64 EquimentSubCategory, Int64 EquipmentList, Int64 FacilityID);
        #endregion

        #region functions of Capital Item Page
        [OperationContract]
        List<BindCapitalItemRequestReport> BindCapitalItemRequestReport(string CapitalItemMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        List<GetCapitalItemMaster> GetCapitalItemMaster();
        [OperationContract]
        List<GetCapitalItemDetails> GetCapitalItemDetails(Int64 CapitalItemMasterID, Int64 LockedBy, Int64 LockTimeOut);
        //[OperationContract]
        //List<GetList> GetList(string ScreenName, string ListName, string Mode);
        [OperationContract]
        List<SearchCapitalItemRequestMaster> SearchCapitalItemRequestMaster(BALCapital argclsCRMaster);
        [OperationContract]
        List<object> InsertCapitalItemMaster(BALCapital argInsCRMaster);
        [OperationContract]
        string InsertCapitalItemDetails(BALCapital argInsCRDetails);
        [OperationContract]
        string UpdateCapitalIemMaster(BALCapital argupdMPRMaster);
        [OperationContract]
        string UpdateCapitalItemDetails(BALCapital argUpdCRDetails);
        [OperationContract]
        string DeleteCapitalItemMaster(Int64 argCRMasterID, bool IsActive, Int64 LastModifiedBy);
        [OperationContract]
        string DeleteCapitalItemDetails(Int64 argCRDetailsID, bool IsActive, Int64 LastModifiedBy);
        [OperationContract]
        List<GetEquipmentSubCategoryforCapital> GetEquipmentSubCategoryforCapital(Int64 CorporateID, string Mode);
        [OperationContract]
        List<GetEquipementListforCapital> GetEquipementListforCapital(Int64 EquimentSubCategoryID, string Mode);

        [OperationContract]
        List<ValidCapitalEquipment> ValidCapitalEquipment(Int64 EquipmentSubCategoryID, Int64 EquipmentList, Int64 FacilityID);
        #endregion

        #region Medical supplyRequest PO
        [OperationContract]
        List<GetMSRMultipleIDs> GetMSRMultipleIDs(string PRMasterID);
        [OperationContract]
        List<SearchMedicalSupplyRequestPo> SearchMedicalSupplyRequestPo(BALMedicalSupplyRequestPo argclsMedicalSuppliesReq);
        [OperationContract]
        string InsertMedicalsupplyPO(BALMedicalSupplyRequestPo argclsMSRPODetails);
        [OperationContract]
        List<GetMSROrderContentPO> GetMSROrderContentPO(Int64 PRmasterID, Int64 LoggedInBy);
        [OperationContract]
        List<GetMedicalSupplyPoReportDetails> GetMedicalSupplyPoReportDetails(string MedicalSupplyMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string UpdateMSRPoOrderContent(BALMedicalSupplyRequestPo argclsmsrpo);
        [OperationContract]
        string UpdateMSRPoStatus(BALMedicalSupplyRequestPo argclsmsrpo);
        [OperationContract]
        List<GetSuperAdminDetails> GetSuperAdminDetails(Int64 CorporateID, Int64 FacilityID);
        #endregion
        // CORP EQUIPMENT MAP
        #region Corp Equipment Map

        [OperationContract]
        List<BindEquipement> BindEquipement(BALCorporate argEquipment);
        [OperationContract]
        string InsertEquipmentSubCategory(BALMachinemaster argEquipmentSubCategory);
        [OperationContract]
        string UpdateEquipmentSubCategory(BALMachinemaster argEquipmentSubCategory);
        [OperationContract]
        string DeleteEquipSubCategoryMaster(BALMachinemaster argEquipmentSubCategory);
        [OperationContract]
        List<GetEquipementSubCategory> GetEquipementSubCategory(Int64 EquipCategoryID, string Mode);
        [OperationContract]
        List<GetActiveEquipementSubCategoryvalue> GetActiveEquipementSubCategoryvalue(Int64 EquipSubCatID);
        [OperationContract]
        List<SavedEquipmentSubCategory> SavedEquipmentSubCategory(Int64 EquipSubCatID);
        [OperationContract]
        List<BindEquipementSubCategoryReport> BindEquipementSubCategoryReport(string SearchFilter, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        List<GetUserApprovePermission> GetUserApprovePermission(Int64 MainmenuID, Int64 SubmenuID);
        [OperationContract]
        List<GetMultiUserApprove> GetMultiUserApprove(Int64 PermissionID);

        [OperationContract]
        string InsertUserApprovePermission(BALUser argUserApprovePermission);
        [OperationContract]
        string UpdateUserApprovePermission(BALUser argUserApprovePermission);


        #endregion

        //RequestIT PO
        #region RequestIT PO
        [OperationContract]
        List<GETITRequestPODetails> GETITRequestPODetails();
        [OperationContract]
        List<SearchRequestITPO> SearchRequestITPO(BALRequestITPO argInsITRequestMaster);
        [OperationContract]
        List<BindRequestITDetailsfromPO> BindRequestITDetailsfromPO(string ITRNO);
        [OperationContract]
        string InsertrequestPO(BALRequestITPO argclsITPOMaster);
        [OperationContract]
        List<GetITROrderContentPO> GetITROrderContentPO(Int64 RequestITMasterID, Int64 LoggedInBy);
        [OperationContract]
        List<BindITRequestPOReport> BindITRequestPOReport(string RequestITMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string UpdateITRequestPO(BALRequestITPO argclsITPOMaster);
        [OperationContract]
        List<GetRwdlsafterordergeneration> GetRwdlsafterordergeneration(string RequestITMasterID);
        #endregion

        // Service Request PO

        #region Service Request PO

        [OperationContract]
        List<GetServiceRequestPODetails> GetServiceRequestPODetails();
        [OperationContract]
        List<SearchServiceRequestPO> SearchServiceRequestPO(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string UpdateServiceRequestDetailsAfterAction(BALServiceRequest argclsSRPODetails);
        [OperationContract]
        List<GetServiceRequestPOGenerateDetails> GetServiceRequestPOGenerateDetails(string ListServiceRequestID, Int64 LoggedinBy);
        [OperationContract]
        List<GetSROrderContentPOReports> GetSROrderContentPOReports(Int64 ServiceRequestID, Int64 ServiceRequestDetailsID, Int64 LoggedInBy);
        [OperationContract]
        List<GetServiceRequestPoReportDetails> GetServiceRequestPoReportDetails(string ServiceRequestMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string InsertServcieRequestGenerateOrder(BALServiceRequest argclsSRPODetails);
        [OperationContract]
        string InsertServiceRequestApproveAction(BALServiceRequest argclsSRPODetails);
        [OperationContract]
        List<GetServiceRequestActionByMasterID> GetServiceRequestActionByMasterID(Int64 ServiceRequestMasterID, Int64 ServiceRequestDetailID);
        [OperationContract]
        string UpdateServiceRequestApproveAction(BALServiceRequest argclsSRPODetails);
        [OperationContract]
        List<SearchServiceRequestPurchaseOrder> SearchServiceRequestPurchaseOrder(BALServiceRequest argclsSRMaster);
        [OperationContract]
        List<SearchServiceRequestPurchaseOrderDetails> SearchServiceRequestPurchaseOrderDetails(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string UpdateServcieRequestGenerateOrder(BALServiceRequest argclsSRPODetails);


        // User Role Permission

        [OperationContract]
        List<BindMultiRolesPermission> BindMultiRolesPermission(string ListUserID, Int64 PermissionID);
        [OperationContract]
        string InsertMultiPermissionApprove(BALUser argUserApprovePermission);
        [OperationContract]
        string UpdateMultiPermissionApprove(BALUser argUserApprovePermission);


        #endregion

        //Machine PO
        #region Machine PO
        [OperationContract]
        List<GetMPRMasterOrder> GetMPRMasterOrder();
        [OperationContract]
        List<SearchMPRMasterOrder> SearchMPRMasterOrder(BALMachinePartsOrder argclsMPRMasterOrder);
        [OperationContract]
        string InsertMachinePO(BALMachinePartsOrder argclsMPRPODetails);
        [OperationContract]
        List<GetMPOrderContentPO> GetMPOrderContentPO(Int64 MPRMasterID, Int64 LoggedInBy);
        [OperationContract]
        List<BindMachinePOReport> BindMachinePOReport(string MPRMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string UpdateMachinePO(BALMachinePartsOrder argclsmaPo);
        [OperationContract]
        string UpdateMPRPoStatus(BALMachinePartsOrder argclsmsrpo);
        [OperationContract]
        string InsertMachineApprove(BALMachinePartsOrder argclsmsrpo);
        [OperationContract]
        List<GetMachinePartsOrderMPONo> GetMachinePartsOrderMPONo(string MPRMasterID);
        #endregion


        #region functions of CapitalPO Page
        [OperationContract]
        List<SearchCapitalPO> SearchCapitalPO(BALCapitalPO argInsCRMaster);
        [OperationContract]
        string InsertCapitalPO(BALCapitalPO argclsCRPODetails);
        [OperationContract]
        List<GetCROrderContentPO> GetCROrderContentPO(BALCapitalPO argclsCRPO);
        [OperationContract]
        List<GetCapitalPOReport> GetCapitalPOReport(string CapitalItemMasterID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string UpdateCRPOOrderContent(BALCapitalPO argclsmsrpo);
        [OperationContract]
        string InsertCapitalApprove(BALCapitalPO argclsCRAODetails);
        [OperationContract]
        List<GetCapitalOrderCPONo> GetCapitalOrderCPONo(string CapitalItemMasterID);

        #endregion

        //Machine Receiving Order
        #region functions of MachineRO Page
        [OperationContract]
        List<SearchMachinePartsReceive> SearchMachinePartsReceive(BALMachinePartsReceiveOrder argclsMPRMasterReceive);
        [OperationContract]
        List<GETMPOValues> GETMPOValues(Int64 MachinePartsReceiveMasterID);
        [OperationContract]
        List<GETMPROValues> GETMPROValues(Int64 MachinePartsReceiveMasterID);
        [OperationContract]
        string SyncMachinePartsReceivingorder();
        [OperationContract]
        List<BindMachineReceivingDetailsReport> BindMachineReceivingDetailsReport(Int64 MachinrPartsRequestOrderID, Int64 MachinrPartsReceivingMasterID, Int64 LoggedInBy, String Filter);
        [OperationContract]
        List<BindMachinePartReceivingDetailsSubReport> BindMachinePartReceivingDetailsSubReport(Int64 MPRequestMasterID, Int64 LockedBy, string Filters);
        [OperationContract]
        List<BindMachineReceiveSummaryReport> BindMachineReceiveSummaryReport(Int64 MachinePartsRequestOrderID, Int64 LockedBy, string Filters);
        [OperationContract]
        List<UpdateMachinePartsReceivingMaster> UpdateMachinePartsReceivingMaster(BALMachinePartsReceiveOrder argclsMPRMaster);
        [OperationContract]
        string UpdateMachinePartsReceivingDetails(BALMachinePartsReceiveOrder argclsMPRDetails);
        #endregion
        #region MedicalSuupliesReceivingOrders
        [OperationContract]
        List<SearchMedicalSuppliesReceiving> SearchMedicalSuppliesReceiving(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec);
        [OperationContract]
        List<BindMedicalsupplyReceivingDetail> BindMedicalsupplyReceivingDetail(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec);
        [OperationContract]
        string SyncMedicalSuppliesReceivingorder();
        [OperationContract]
        List<UpdateMSRReceivingMaster> UpdateMSRReceivingMaster(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec);
        [OperationContract]
        string UpdateMSRReceivingDetails(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec);
        [OperationContract]
        List<GetMSRReceivingsummaryReport> GetMSRReceivingsummaryReport(BALMedicalSupplyReceivingOrder argmsr);
        [OperationContract]
        List<BindMedicalSupplyDetailsReport> BindMedicalSupplyDetailsReport(BALMedicalSupplyReceivingOrder argmsr);
        [OperationContract]
        List<BindMedicalSupplyDetailsSubReport> BindMedicalSupplyDetailsSubReport(BALMedicalSupplyReceivingOrder argmsr);
        #endregion

        //IT Receiving
        #region ITReceiving
        [OperationContract]
        List<BindITReceivingDetailsReport> BindITReceivingDetailsReport(Int64 ITRequestMasterID, Int64 ITReceivingMasterID, Int64 LoggedInBy, String Filter);
        [OperationContract]
        List<BindITReceivingDetailsSubReport> BindITReceivingDetailsSubReport(Int64 ITRequestMasterID, Int64 LoggedInBy, String Filter);
        [OperationContract]
        List<Getitronovalue> BindITRONo(Int64 ITRequestMasterID);
        [OperationContract]
        string SynITReceivingorder();
        [OperationContract]
        List<SearchITReceiving> SearchITReceiving(BALRequestITReceiving argInsITRequestMaster);
        [OperationContract]
        List<BindITNNOvalues> BindITNNOvalues(Int64 ITReceivingMasterID);
        [OperationContract]
        List<object> InsertITReceivingMaster(BALRequestITReceiving argITrecving);
        [OperationContract]
        string InsertITReceivingDetails(BALRequestITReceiving argITrecving);
        [OperationContract]
        List<BindITReceivingsummaryReport> BindITReceivingsummaryReport(String ITNNo, Int64 LoggedinBy, String Filter);
        [OperationContract]
        string UpdateITReceivingDetails(BALRequestITReceiving argclsItDetailsReceive);
        [OperationContract]
        List<UpdateITRecevingMaster> UpdateITReceivingMaster(BALRequestITReceiving argclsITMasterReceive);
        #endregion



        //CapitalReceiving
        #region functions of CAPITALRO Page

        [OperationContract]
        List<SearchCapitalReceivingMaster> SearchCapitalReceivingMaster(BALCapitalReceiving argclsCRMasterReceive);
        [OperationContract]
        List<UpdateCapitalRecevingMaster> UpdateCapitalReceivingMaster(BALCapitalReceiving argclsCRMasterReceive);
        [OperationContract]
        string UpdateCapitalReceivingDetails(BALCapitalReceiving argclsCRMasterReceive);
        [OperationContract]
        string SyncCapitalReceivingorder();
        [OperationContract]
        List<GetCpoDetails> GetCpoDetails(Int64 CapitalReceivingMasterID);
        [OperationContract]
        List<GetCPROMasterReview> GetCPROMasterReview(Int64 CapitalReceivingMasterID);

        [OperationContract]
        List<BindCapitalDetailsReport> BindCapitalDetailsReport(Int64 CapitalItemMasterID, Int64 CapitaltemReceivingMasterID, Int64 LoggedInBy, String Filter);
        [OperationContract]
        List<BindCapitalReceivingDetailsSubReport> BindCapitalReceivingDetailsSubReport(Int64 CapitalItemMasterID, Int64 LoggedInBy, String Filter);
        [OperationContract]
        List<BindCapitalReceivingsummaryReport> BindCapitalReceivingsummaryReport(Int64 CapitalOrderID, Int64 LoggedinBy, String Filter);

        #endregion

        #region Service Request Receiving Order

        [OperationContract]
        List<SearchServiceRequestReceivingOrder> SearchServiceRequestReceivingOrder(BALServiceRequest argclsSRMaster);

        [OperationContract]
        string UpdateServcieRecevingOrder(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string UpdateServcieRecevinginvoice(BALServiceRequest argclsSRMaster);
        [OperationContract]
        string SyncServiceReceivingorder();
        [OperationContract]
        List<GetServiceReceiveOrder> GetServiceReceiveOrder(BALServiceRequest argclsSRMaster);

        #endregion

        #region Multi Serarch DropDown

        [OperationContract]
        List<GetFacilityByListCorporateID> GetFacilityByListCorporateID(string ListCorporateID, Int64 UserID, Int64 RoleID);
        [OperationContract]
        List<GetVendorByFacilityID> GetVendorByFacilityID(string ListVendorID, Int64 LoggedInBy);

        #endregion

        #region Ending Inventory
        [OperationContract]
        List<SearchEndingInventory> SearchEndingInventory(BALEndingInventory argclsEndinginven);
        [OperationContract]
        string SyncEndingInventory();
        [OperationContract]
        string InsertEndingInventory(BALEndingInventory argclsEndinginven);
        [OperationContract]
        string UpdateEndingInventory(BALEndingInventory argclsEndinginven);
        [OperationContract]
        List<GetReceivedQtyInfo> GetReceivedQtyInfo(BALEndingInventory argclsEndinginven);
        [OperationContract]
        List<GetTransferINQtyInfo> GetTransferINQtyInfo(BALEndingInventory argclsEndinginven);
        [OperationContract]
        List<GetTransferOutQtyInfo> GetTransferOutQtyInfo(BALEndingInventory argclsEndinginven);
        [OperationContract]
        List<EndingInventoryReport> EndingInventoryReport(BALEndingInventory argclsEndinginven);
        [OperationContract]
        List<GetCatagoryByFacilityID> GetCatagoryByFacilityID(BALEndingInventory argclsEndinginven);

        #endregion

        //Transfer Out
        #region Transfer Out
        [OperationContract]
        List<SearchTransferOut> SearchTransferOut(BALTransferOut argclsEndinginven);
        [OperationContract]
        string InsertTransferDetails(BALTransferOut argInsTransfer);
        [OperationContract]
        List<GetTransferNo> GetTransferNo(Int64 FacilityIDFrom, Int64 FacilityIDTo);
        [OperationContract]
        List<GetEmailNotificationforTransfer> GetEmailNotificationforTransfer();
        [OperationContract]
        List<GetFromEmailforTransfer> GetFromEmailforTransfer(string TransferNo);
        #endregion

        //Transfer In
        #region Transfer In

        [OperationContract]
        List<SearchTransferIn> SearchTransferIn(BALTransferIn argclsTransferIn);
        [OperationContract]
        string InsertTransferIn(BALTransferIn argclstransferIn);
        #endregion

        //Transfer History
        #region Transfer History
        [OperationContract]
        List<SearchTransferInHistory> SearchTransferInHistory(BALTransferIn argclsTransferIn);

        [OperationContract]
        List<SearchTransferOutHistory> SearchTransferOutHistory(BALTransferOut argclsTransferOut);
        [OperationContract]
        List<GetTransferInHistoryReport> GetTransferInHistoryReport(string TransferINID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);
        [OperationContract]
        string UpdateTransferDetails(BALTransferOut argclsTRDetails);
        [OperationContract]
        List<BindTransferOutHistoryReport> BindTransferOutHistoryReport(string TransferOutID, string SearchFilters, Int64 LockedBy, Int64 LoggedinBy);

        [OperationContract]
        List<GetCategoryByListFacilityID> GetCategoryByListFacilityID(string FacilityID);
        #endregion

        // Validate Medical Supply Item
        [OperationContract]
        List<ValidateMedicalSuppliesItem> ValidateMedicalSuppliesItem(BALMedicalSuppliesRequest argclsMedicalSup);

        #region Corporate Master
        [OperationContract]
        List<BindCorporateMaster> BindCorporateMaster();
        [OperationContract]
        string InsertCorporateMaster(BALCorporate argclsCorporate);
        [OperationContract]
        string UpdateCorporateMaster(BALCorporate argclsCorporate);
        [OperationContract]
        List<SearchCorporateMaster> SearchCorporateMaster(BALCorporate argclsCorporate);
        [OperationContract]
        string DeleteCorporateDetails(Int64 CorporateID, bool IsActive, Int64 LastModifiedBy);
        #endregion

        //Reports
        #region Reports
        [OperationContract]
        List<BindFacilityReport> BindFacilityReport(BALFacility arguFacility);
        [OperationContract]
        List<BindFacilityDetailsReport> BindFacilityDetailsReport(BALFacility arguFacility);
        [OperationContract]
        List<GetUserRoleForFacility> GetUserRoleForFacility(Int64 FacilityID, string ListRoleID);
        [OperationContract]
        List<BindRolesForFacility> BindRolesForFacility(Int64 FacilityID);
        [OperationContract]
        List<BindFacilityVendorAccountReport> BindFacilityVendorAccountReport(BALFacilityVendorAccount argclsFacilityVendorAcct);
        [OperationContract]
        List<BindCorporateMasterReport> BindCorporateMasterReport(BALCorporate arguCorporate);
        [OperationContract]
        List<BindVendorItemMapReport> BindVendorItemMapReport(BALItemMap argclsVendorItemMap);
        [OperationContract]
        List<BindUserSummaryReport> BindUserSummaryReport(Int64 UserID, Int64 LoggedinBy, string Filter);
        [OperationContract]
        List<BindTransferOutReport> BindTransferOutReport(Int64 LoggedinBy);
        [OperationContract]
        List<BindTransferInReport> BindTransferInReport(Int64 LoggedinBy);
        #endregion

        [OperationContract]
        List<GetVendorReport> GetVendorReport(BALVendor ardvendor);
        [OperationContract]
        List<GETVendorUniqueName> GETVendorUniqueName(string VendorCode);
        [OperationContract]
        List<GetUOMName> GetUOMName(string VendorCode);
        [OperationContract]
        List<GetVendorItemMappingPage> GetVendorItemMappingPage();

        [OperationContract]
        List<GetVendorDetailsReport> GetVendorDetailsReport(BALVendor argvendor);

        [OperationContract]
        List<Validgpbillcode> Validgpbillcode(string GpBillingCode);

        [OperationContract]
        List<Validuseremail> Validuseremail(string Email);

        #region Report Summary & Details

        [OperationContract]
        List<GetUserPermissionReport> GetUserPermissionReport(string MainMenuID, string SubMenuID, string SearchFilter, Int64 LoggedInBy);

        [OperationContract]
        List<GetVendorOrderDueReport> GetVendorOrderDueReport(BALVendorOrderDue argvendorder);

        [OperationContract]
        List<GetFacilitySuppliesMapReport> GetFacilitySuppliesMapReport(Int64 CorporateID, Int64 FacilityID, string VendorID, string CategoryID,Int64 Parlevel,string SearchFilter, Int64 LoggedInBy);

        #endregion

        [OperationContract]
        List<GetItemDescName> GetItemDescName(string ItemDescription);
        [OperationContract]
        List<GetCategoryReport> GetCategoryReport(BALPGroup argitemCat);
        [OperationContract]
        List<GetUomReport> GetUomReport(BALUom arguom);
        [OperationContract]
        List<GetItemSummaryReport> GetItemSummaryReport(BALItem argitem);
        [OperationContract]
        List<GetItemDetailsReport> GetItemDetailsReport(BALItem argitem);
        [OperationContract]
        List<GetMachineMasterReport> GetMachineMasterReport(BALMachinemaster argmach);
        [OperationContract]
        List<GetMachineMasterDetailsReport> GetMachineMasterDetailsReport(BALMachinemaster argmachine);
        [OperationContract]
        List<ValidateMedicalSuppliesOrder> ValidateMedicalSuppliesOrder(Int64 PMasterID);
        [OperationContract]
        List<GetVendorOrderdueRemainderReport> GetVendorOrderdueRemainderReport(BALVendorOrderDue argclsVendorOrderDue);

        [OperationContract]
        List<SearchFacilityVendorAcct> GetFacilityVendorAcct(BALFacilityVendorAccount argclsFacilityVendorAcct);
        [OperationContract]
        List<SearchVendorItemMap> GetVendorItemMap(BALItemMap argclsVendorItemMap);
        [OperationContract]
        List<SearchUserDetails> SearchUserDetails(BALUser argclsUserDetails);
        [OperationContract]
        string DeleteUserDetails(Int64 UserID, Int64 LastModifiedBy);

        [OperationContract]
        List<chkvalidcorporate> chkvalidcorporate(BALCorporate argcorporate);
        [OperationContract]
        List<GetCategoryByListVendorID> GetCategoryByListVendorID(string ListVendorID);

        //Receiving Reports//
        [OperationContract]
        List<SearchCapitalReceivingSummaryReport> SearchCapitalReceivingSummaryReport(BALCapitalReceiving argclsCRMasterReceive);
        [OperationContract]
        List<SearchITReceivingSummaryReport> SearchITReceivingSummaryReport(BALRequestITReceiving argInsITRequestMaster);
        [OperationContract]
        List<SearchMachinePartsReceiveSummaryReport> SearchMachinePartsReceiveSummaryReport(BALMachinePartsReceiveOrder argclsMPRMaster);
        [OperationContract]
        List<SearchMedicalSuppliesReceivingSummaryReport> SearchMedicalSuppliesReceivingSummaryReport(BALMedicalSupplyReceivingOrder argclsMedicalSuppliesRec);
        [OperationContract]
        List<ValidFaciliityAccountCode> ValidFaciliityAccountCode(string FacilityAccountCode);

        [OperationContract]
        List<SearchVendorOrderType> SearchVendorOrderType(BALVendorOrderDue argclsvendorOrderDue);


        [OperationContract]
        List<GetVendorOrderDue> GetVendorOrderDue(BALVendorOrderDue argclsvendorOrderDue);

        [OperationContract]
        List<CheckVendorOrderDue> CheckVendorOrderDue(Int64 CorporateID, Int64 FacilityID, Int64 VendorID);

        [OperationContract]
        List<GetItem> GetItem();

        [OperationContract]
        List<GetVendor> GetVendor();
        
        [OperationContract]
        DataSet GetMonthlyUsageReport(BALReport argreport);

        [OperationContract]
        DataSet GetMonthlyPurchaseReport (BALReport argreport);
        [OperationContract]
        DataSet GetMonthlyEndingReport(BALReport argreport);
        [OperationContract]    
        DataSet GetCumUsageReport(BALReport argreport);
        [OperationContract]
        DataSet GetCumulativeUsageReportBySingleItem(BALReport argreport);
        [OperationContract]
        DataSet GetMonthlyInventoryReport(BALReport argreport);
        [OperationContract]
        DataSet GetCostPertxReport(BALReport argreport);
        [OperationContract]
        DataSet GetMonthlyDrugReport(BALReport argreport);

        [OperationContract]
        string DeleteVendorOrderDue (BALVendorOrderDue argvendor);
        [OperationContract]
        List<BindPermissionforUser> BindPermissionforUser();
        [OperationContract]
        DataSet GetUserCredentials(BALUser arguser);

        [OperationContract]
        string ActivityTracker(BALActivityTracking argactivitytrack);
    }

}
