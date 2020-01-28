using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Class
{
    public static class Constant
    {
        //Required Field Validator
        public static string RequiredFieldValidator = "This information is required";
        public static string CompareValidator = "Password mismatch";
        public static string RequiredExpressionPhone = "Phone number you entered is not valid";
        public static string RequiredExpressionEmail = "Please enter valid email";
        public static string cusvalvendortype = "This information is Required";
        // public static string cusvalvendorname = "Please select at least one type";
        //public static string cusvalvendordesc = "Please select at least one type";
        public static string WarningUserAvailMessage = "ShowwarningLookupPopup('User(<<UserName>>) does not exists in the inventory system');";
        public static string WarningLookupMessage = "ShowwarningLookupPopup('The following field(s) are having inactive data : <<values>>');";

        //Add User Page
        public static string AddUserSaveMessage = "ShowPopup('User(<<AddUser>>) is saved successfully');";
        public static string AddUserUpdateMessage = "ShowPopup('User(<<AddUser>>) is updated successfully ');";
        public static string AddUserDeleteMessage = "ShowdelPopup('User Role (<<AddUser>>) is deleted successfully');";
        public static string AddUserErrorMessage = "ShowdelPopup('<<AddUser>>');";
        public static string WarningAddUserMessage = "ShowwarningLookupPopup('User(<<AddUser>>)is already exists');";
        public static string checkemailexists = "ShowwarningLookupPopup('Email is already exists for User (<<AddUser>>)');";
        public static string checkemailinactexists = "ShowwarningLookupPopup('Email is already exists for User (<<AddUser>>)');";
        public static string checkemailisactiveexists = "ShowwarningLookupPopup('User exists but inactive please reactivate user (<<AddUser>>)');";

        //Facility Page
        public static string FacilitySaveMessage = "ShowPopup('Facility(<<FacilityDescription>>) is saved successfully');";
        public static string FacilityDeleteMessage = "ShowdelPopup('Facility(<<FacilityDescription>>) is deleted successfully');";
        public static string FacilityMandMessageWarMessage = "ShowwarningLookupPopup('Facility <<FacilityDescription>>' is already exists);";
        public static string FacilityErrorMessage = "ShowdelPopup('<<FacilityDescription>>');";
        public static string Facility = "This information is Required";

        //Medical Supplies Page
        public static string MedicalSuppliesSaveMessage = "ShowPopup('Item Name(<<Itemname>>) is saved successfully');";
        public static string MedicalSuppliesSaveCurrencyMessage = "ShowPopup('Currency Name (<<Itemname>>) is saved successfully');";
        public static string MedicalSuppliesDeleteMessage = "ShowdelPopup('Item Name(<<Itemname>>) is deleted successfully');";
        public static string MedicalSuppliesErrorMessage = "ShowdelPopup('Item Name(<<Itemname>>)');";
        public static string MedicalSuppliesvaildgpMessage = "ShowwarningLookupPopup('Item Accounting Code (<<Itemname>>) is already exists');";
        public static string MedicalSuppliesvailddescMessage = "ShowwarningLookupPopup('Item Description (<<Itemname>>) is already exists');";
        public static string MedicalSuppliescategoryMessage = "ShowwarningLookupPopup('Please select Item Category');";
        public static string MedicalSuppliesItemMessage = "ShowwarningLookupPopup('Please enter ItemID or Item Description');";
        public static string MedicalSuppliesradioMessage = "ShowwarningLookupPopup('Please select ItemID or Item Description');";
        public static string Currency = "$";

        //Facility Vendor Account Page
        public static string FacilityVendorAccountSaveMessage = "ShowPopup('FacilityVendorAccount(<<FacilityDescription>>) is saved successfully');";
        public static string FacilityVendorAccountDeleteMessage = "ShowdelPopup('FacilityVendorAccount(<<FacilityDescription>>) is deleted successfully');";
        public static string FacilityVendorAccountErrorMessage = "ShowdelPopup('FacilityVendorAccount(<<FacilityDescription>>) is deleted successfully');";
        public static string WarningFacilityVendorAccountMessage = "ShowwarningLookupPopup('<<FacilityDescription>>');";

        //Item Category Page
        public static string ItemCategorySaveMessage = "ShowPopup('ItemCategory(<<ItemCategory>>) is saved successfully');";
        public static string ItemCategoryDeleteMessage = "ShowdelPopup('ItemCategory(<<ItemCategory>>) is deleted successfully');";
        public static string ItemCategoryErrorMessage = "ShowdelPopup('ItemCategory(<<ItemCategory>>) is deleted successfully');";
        public static string ItemCategoryWarningMessage = "ShowwarningPopup('ItemCategory(<<ItemCategory>>) is already exists');";
        //UOM Page
        public static string UOMSaveMessage = "ShowPopup('UOM(<<UOM>>) is saved successfully');";
        public static string UOMDeleteMessage = "ShowdelPopup('UOM(<<UOM>>) is deleted successfully');";
        public static string UOMErrorMessage = "ShowdelPopup('<<UOM>>');";

        //User Permission Page
        public static string UserPermissionSaveMessage = "ShowPopup('UserPermission(<<UserPermission>>) is saved successfully');";
        public static string UserPermissionMultiRoelSaveMessage = "ShowPopup('UserPermission Multi role Assigned successfully');";
        public static string UserPermissionDeleteMessage = "ShowdelPopup('UserPermission(<<UserPermission>>) is deleted successfully');";
        public static string UserPermissionErrorMessage = "ShowdelPopup('<<UserPermission>>');";
        public static string UserPerMandatoryWarningMsg = "ShowwarningPopup('(<<UserPermission>>) Approval is selected but Approval Range is not been entered');";
        public static string UserPerActionOrderWarningMsg = "ShowwarningPopup('Action Order Should be unique value');";
        public static string UserPerRepeatWarningMsg = "ShowwarningPopup('(<<UserPermission>>) role is already contains the same Approval range');";
        public static string UserPerApprovalWarningMsg = "ShowwarningPopup('(<<UserPermission>>) Lower limt value should be less than upper limit value');";
        public static string UserPerEmptyWarningMsg = "ShowwarningPopup('<<UserPermission>>');";
        public static string UserPerUniqueRoleNameWarningMsg = "Role Name Already Exists";
        public static string UseractioncheckboxWarningMsg = "ShowwarningPopup('Role action is required');";

        //Vendor Page
        public static string ReqfieldValVendorshortname = "Vendor Name is required";
        public static string ReqfieldValVendorDesc = "Vendor Description is required";
        public static string VendorSaveMessage = "ShowPopup('Vendor(<<Vendor>>) is saved successfully');";
        public static string VendorDeleteMessage = "ShowdelPopup('Vendor(<<Vendor>>) is deleted successfully');";
        public static string VendorErrorMessage = "ShowdelPopup('<<Vendor>>');";

        //Vendor Item Map Page
        public static string VendorItemMapSaveMessage = "ShowPopup('VendorItemMap(<<VendorItemMap>>) is saved successfully');";
        public static string VendorItemMapDeleteMessage = "ShowdelPopup('VendorItemMap(<<VendorItemMap>>) is deleted successfully');";
        public static string VendorItemMapErrorMessage = "ShowdelPopup('VendorItemMap(<<VendorItemMap>>)');";
        public static string WarningVendorItemMapMessage = "ShowwarningLookupPopup('<<VendorItemMap>>');";
        public static string WarningVendorMessage = "ShowwarningLookupPopup('Selected Item is already mapped with VendorName: <<VendorItemMap>>');";
        public static string WarningVendorItemIDMessage = "ShowwarningLookupPopup('Selected Vendor ItemID is already Exists with VendorName: <<VendorItemMap>>');";
        public static string WarningVendorInActiveLookupMessage = "ShowwarningLookupPopup('Selected Vendor ItemID is already Exists with VendorName: <<VendorItemMap>>(InActive)');";

        //Vendor Order Due Page
        public static string VendorOrderDueSaveMessage = "ShowPopup('VendorOrderDue(<<VendorOrderDue>>) is saved successfully');";
        public static string VendorOrderDueDeleteMessage = "ShowdelPopup('VendorOrderDue(<<VendorOrderDue>>) is deleted successfully');";
        public static string VendorOrderDueErrorMessage = "ShowdelPopup('<<VendorOrderDue>>');";
        public static string WarningVendorrecordexistMessage = "ShowwarningLookupPopup('<<VendorOrderDue>>');";
        public static string WarningVendorOrderDueMessage = "ShowwarningLookupPopup('<<VendorOrderDue>>');";
        public static string WarningVendorrecordMessage = "ShowwarningLookupPopup('Record is already exists in this year<<VendorOrderDue>>');";
        public static string WarningVendorOrderMessage = "ShowwarningOrderLookupPopup('This record is already exists with OrderType: <<VendorOrderDue>> Do you want to overwrite this record?');";
        public static string WarningVendoradhocMessage = "ShowwarningLookupPopup('This record is already exists with OrderType: <<VendorOrderDue>> can not overwrite this record');";

        //Facility Supplies Map Page
        public static string FacilitySuppliesMapSaveMessage = "ShowPopup('FacilitySuppliesMap(<<FacilitySuppliesMap>>) is saved successfully');";
        public static string FacilitySuppliesMapDeleteMessage = "ShowdelPopup('FacilitySuppliesMap(<<FacilitySuppliesMap>>) is deleted successfully');";
        public static string FacilitySuppliesMapErrorMessage = "ShowdelPopup('<<FacilitySuppliesMap>>');";
        public static string FacilitySuppliesEmptyWarMessage = "ShowwarningLookupPopup('Census Value in Grid Not Entered');";
        public static string FacilitySuppliesVendorOrderMessage = "ShowwarningLookupPopup('Vendor order due is not mapped');";
        public static string FacilitySuppliesVendorloadMessage = "ShowwarningLookupPopup('Census value is changed, So click on LoadGrid and then proceed for Review');";
        public static string WarningFacilitySuppliesMapMessage = "ShowwarningLookupPopup('<<FacilitySuppliesMap>>');";

        // Service Request Page
        public static string ServiceRequestSaveMessage = "ShowPopup('Service Request(<<ServiceRequestDescription>>) is saved successfully,Click here to  ');";
        public static string ServiceRequestUpdateMessage = "ShowPopup('Service Request(<<ServiceRequestDescription>>) is updated successfully,Click here to  ');";
        public static string ServiceRequestLockINMessage = "ShowwarningLookupPopup('Another user (<<ServiceRequestDescription>>) is updating this record , Please try after some time.');";
        public static string ServiceRequestOnlyDocMessage = "ShowwarningLookupPopup('Only Word or PDF formats documents can upload');";
        public static string ServiceRequestDocExistsMessage = "ShowwarningLookupPopup('Only one document allowed for per service.');";
        public static string ServiceRequestNoFileMessage = "ShowwarningLookupPopup('File is not selected');";
        public static string ServiceRequestSelectFacilityMessage = "ShowwarningLookupPopup('Should select a Facility before <<Action>> Service Category');";
        public static string ServiceRequestSelectCatMessage = "ShowwarningLookupPopup('Should select a Service Category');";
        public static string ServiceRequestRecordExistMessage = "ShowwarningLookupPopup('Record Exists');";
        public static string ServiceRequestSelectListMessage = "ShowwarningLookupPopup('Should select a Service Category with Service List Value');";
        public static string ServiceRequestListMessage = "ShowwarningLookupPopup('Should select a Service List');";
        public static string ServiceRequestListDelActMessage = "ShowwarningLookupPopup('Service List Should not allow to delete for Active ServiceRequest');";
        public static string ServiceRequestListValMessage = "ShowwarningLookupPopup('Selected category have Servicelist values');";
        public static string ServiceRequestNormalMessage = "ShowPopup('Service Request <<ServiceRequestDescription>>');";
        public static string ServiceRequestDeleteMessage = "ShowdelPopup('FacilityVendorAccount(<<FacilityDescription>>) is deleted successfully');";
        public static string ServiceRequestErrorMessage = "ShowdelPopup('<<ServiceRequestDescription>>');";
        public static string WarningMessage = "ShowwarningLookupPopup('<<ServiceRequestDescription>>');";
        // Medical Supplies Request Page
        public static string MedicalSuppliesRequestSaveMessage = "ShowPopup('Medical Supplies Request(<<MedicalSupplyRequestDescription>>) is saved successfully,Click here to  ');";
        public static string MedicalSuppliesRequestMessage = "ShowPopup('Medical Supplies Request (<<MedicalSupplyRequestDescription>>) is updated successfully,Click here to ');";
        public static string MedicalSuppliesRequestDeleteMessage = "ShowdelPopup('Medical Supplies Request(<<MedicalSupplyRequestDescription>>) is deleted successfully');";
        public static string MedicalSuppliesRequestErrorMessage = "ShowdelPopup('<<MedicalSupplyRequestDescription>>');";
        public static string WarningMedsupplyreqMessage = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>>');";
        public static string WarningMedsupplyAlreadyExitsShipping = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> This ShippingValue Already Exits');";
        public static string WarningMedsupplyAlreadyExitsTime = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> This TimeDelivery Value Already Exits');";
        public static string WarningMedsupplyReqShipping = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Please Select Shipping');";
        public static string WarningMedsupplyReqTime = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Please Select Time Delivery');";
        public static string WarningMedsupplyreqOrderPeriodExists = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Order Period Not Exists');";
        public static string WarningMedsupplyreqQtyinhandRequired = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Qtn In Hand is required');";
        public static string WarningMedsupplyreqNoRecordsReview = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> No Records Found in Review Grid');";
        public static string WarningMedsupplyreqItemNotMapped = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Items Not Mapped');";
        public static string WarningMedsupplyreqNoRecordsItemGrid = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> No Records Found in Items Grid');";
        public static string WarningMedsupplyreqQtyEditMode = "ShowwarningLookupPopup('<<MedicalSupplyRequestDescription>> Qty In Hand should not Empty in Edit Mode');";
        public static string WarningMedsupplyItemNotReceived = "ShowwarningLookupPopup('The following item : <<MedicalSupplyRequestDescription>> are pending on Receiving/Transferin workflow, So new request for this item(s) are not allowed.');";
        public static string MedicalSuppliesRequestShippingSaveMessage = "ShowPopup('<<MedicalSupplyRequestDescription>> Shipping Value is Saved Successfully');";
        public static string MedicalSuppliesRequestShippingUpdateMessage = "ShowPopup('<<MedicalSupplyRequestDescription>> Shipping Value is Updated Successfully');";
        public static string MedicalSuppliesReqShiptDeleteMessage = "ShowdelPopup('<<MedicalSupplyRequestDescription>> Shipping Value is Deleted Successfully');";
        public static string MedicalSuppliesRequestTimeSaveMessage = "ShowPopup('<<MedicalSupplyRequestDescription>> TimeDelivery Value is Saved Successfully');";
        public static string MedicalSuppliesRequestTimeUpdateMessage = "ShowPopup('<<MedicalSupplyRequestDescription>> TimeDelivery Value is Updated Successfully');";
        public static string MedicalSuppliesReqTimetDeleteMessage = "ShowdelPopup('<<MedicalSupplyRequestDescription>> TimeDelivery Value is Deleted Successfully');";

        // Machine Parts Request Page
        public static string MachinePartsRequestSaveMessage = "ShowPopup('Machine Parts Request(<<MachinePartsRequestDescription>>) is saved successfully,Click here to  ');";
        public static string MachinePartsRequestMessage = "ShowPopup('Machine Parts Request (<<MachinePartsRequestDescription>>) is updated successfully, Click here to    ');";
        public static string DeleteMPRDetailsDeleteMessage = "ShowdelPopup('Machine Parts Request <<DeleteMPRDetails>> is deleted successfully');";
        public static string MachinePartsRequestErrorMessage = "ShowdelPopup('<<MachinePartsRequestDescription>>');";
        public static string WarningMachinePartsreqMessage = "ShowwarningLookupPopup('<<MachinePartsRequestDescription>>');";
        public static string WarningMachinePartsReqMessage = "ShowwarningLookupPopup('<<MachinePartsRequestDescription>> Item grid fields are should not be empty');";
        public static string WarningMachinePartsReqMessagedate = "ShowwarningLookupPopup('<<MachinePartsRequestDescription>> Date from should be less than date to');";
        public static string WarningMachinePartsValidMessage = "ShowwarningLookupPopup('The following EquipmentCategory,EquipmentSubCategory or EquipmentList <<MachinePartsRequestDescription>> are pending on Receiving/BackOrder workflow, So new request for this EquipmentSubCategory are not allowed.');";

        // Machine Master Page
        public static string EquipMasterSaveMessage = "ShowPopup('Equipment Category (<<EquipmentCategory>>) is saved successfully');";
        public static string EquipMasterUpdateMessage = "ShowPopup('Equipment Category (<<EquipmentCategory>>) is updated successfully');";
        public static string EquipListSaveMessage = "ShowPopup('Equipment List (<<EquipList>>) is saved successfully');";
        public static string EquipSubCatSaveMessage = "ShowPopup('Equipment Sub Category (<<EquipSubCategory>>) is saved successfully');";
        public static string EquipListUpdateMessage = "ShowPopup('Equipment List (<<EquipList>>) is updated successfully');";
        public static string EquipSubCatUpdateMessage = "ShowPopup('Equipment Sub Category (<<EquipSubCategory>>) is updated successfully');";
        public static string EquipMasterDeleteMessage = "ShowdelPopup('Equipment Category (<<EquipmentCategory>>) is deleted successfully');";
        public static string EquipMasterDelActMessage = "ShowwarningLookupPopup('Selected equipment category has equipment list values');";
        public static string EquipListDeleteMessage = "ShowdelPopup('Equipment List (<<EquipList>>) is deleted successfully');";
        public static string EquipSubCatDeleteMessage = "ShowdelPopup('Equipment Sub Category (<<EquipSubCategory>>) is deleted successfully');";
        public static string EquiptListDelActMessage = "ShowwarningLookupPopup('Equipment List Should not allow to delete for Active MachineParts');";
        public static string EquiptSubCatDelActMessage = "ShowwarningLookupPopup('Equipment Sub Category Should not allow to delete for Active MachineParts');";
        public static string MachineMasterSaveMessage = "ShowPopup('Machine Master (<<MachineMaster>>) is saved successfully');";
        public static string MachineMasterUpdateMessage = "ShowPopup('Machine Master (<<MachineMaster>>) is updated successfully');";
        public static string MachineMasterDeleteMessage = "ShowdelPopup('Machine Master (<<MachineMaster>>) is deleted successfully');";
        public static string MachineMasterErrorMessage = "ShowdelPopup('<<MachineMaster>>');";
        public static string MachineMasterRestoreMessage = "ShowPopup('Machine Master (<<MachineMaster>>) is restored successfully');";
        public static string WarningMachineMasterMessage = "ShowwarningPopup('<<MachineMaster>>');";

        // IT Parts Request Page
        public static string ITRequestSaveMessage = "ShowPopup('IT Request(<<ITRequestDescription>>) is saved successfully,Click here to  ');";
        public static string ITRequestMessage = "ShowPopup('IT Request (<<ITRequestDescription>>) is updated successfully, Click here to    ');";
        public static string DeleteITRDetailsDeleteMessage = "ShowdelPopup('IT Request <<DeleteITRDetails>> is deleted successfully');";
        public static string ITRequestErrorMessage = "ShowdelPopup('<<ITRequestDescription>>');";
        public static string WarningITreqMessage = "ShowwarningLookupPopup('<<ITRequestDescription>>');";
        public static string WarningITReqMessage = "ShowwarningLookupPopup('<<ITRequestDescription>> Item grid fields are should not be empty');";
        public static string WarningITReqMessagedate = "ShowwarningLookupPopup('<<ITRequestDescription>> Date from should be less than date to');";
        public static string WarningITValidMessage = "ShowwarningLookupPopup('The following EquipmentSubCategory or EquipmentList <<ITRequestDescription>> are pending on Receiving/BackOrder workflow, So new request for this EquipmentSubCategory are not allowed.');";



        // Corporate Equipment Master

        public static string EquipCorpMapMessage = "ShowPopup('(<<CorpEquipmentMapCategory>>) is saved successfully and Corporate Equipment Mapped successfully');";
        public static string EquipCorpMapUpdateMessage = "ShowPopup('(<<CorpEquipmentMapCategory>>) is Updated successfully and Corporate Equipment Mapped successfully');";
        public static string EquipCorpMapDeleteMessage = "ShowdelPopup('Equipment Category Map (<<CorpEquipmentMapCategory>>) is deleted successfully');";
        public static string EquipCorpMapErrorMessage = "ShowdelPopup('<<CorpEquipmentMapCategory>>');";
        public static string EquipCorpMapNoRecordMessage = "ShowdelPopup('Equipment Category Map <<CorpEquipmentMapCategory>> is Already Exists');";
        public static string EquipCorpMapWarMessage = "ShowwarningLookupPopup('<<CorpEquipmentMapCategory>> Equipment sub category should contain valid text, Empty text is not allowed');";

        //Capital Item Request Page
        public static string CapitalItemSaveMessage = "ShowPopup('Major Item Request (<<MajorItem>>) is saved successfully, click here to  ');";
        public static string CapitalItemUpdateMessage = "ShowPopup('Major Item Request (<<MajorItem>>) is updated successfully, click here to  ');";
        public static string CapitalItemDeleteMessage = "ShowdelPopup('Major Item Request <<MajorItem>> is deleted successfully');";
        public static string CapitalItemErrorMessage = "ShowdelPopup('<<MajorItem>>');";
        public static string WarningCapitalItemMessage = "ShowwarningPopup('<<MajorItem>>');";
        public static string WarningCapitalItemReqMessage = "ShowwarningLookupPopup('<<MajorItem>> Item grid fields should not be empty');";
        public static string WarningCapitalItemQtyMessage = "ShowwarningLookupPopup('<<MajorItem>> Qty field should not be empty');";
        public static string WarningCapitalItemReasonMessage = "ShowwarningLookupPopup('<<MajorItem>> Please enter the Reason');";
        public static string WarningCapitalItemValidMessage = "ShowwarningLookupPopup('The following EquipmentSubCategory or EquipmentList <<MajorItem>> are pending on Receiving/BackOrder workflow, So new request for this EquipmentSubCategory are not allowed.');";


        // Medical Supplies RequestPo Page
        public static string MedicalSuppliesRequestPoSaveMsg = "ShowPopup('<<MedicalSupplyRequestPoDescription>>');";
        public static string MedicalSuppliesRequestPoSaveMessage = "ShowPopup('Medical Supplies RequestPo(<<MedicalSupplyRequestPoDescription>>) is saved successfully');";
        public static string MedicalSuppliesRequestPoMessage = "ShowPopup('Medical Supplies RequestPo (<<MedicalSupplyRequestPoDescription>>) is updated successfully');";
        public static string MedicalSuppliesRequestPoDeleteMessage = "ShowdelPopup('Medical Supplies RequestPo(<<MedicalSupplyRequestPoDescription>>) is deleted successfully');";
        public static string MedicalSuppliesRequestPoErrorMessage = "ShowdelPopup('<<MedicalSupplyRequestPoDescription>>');";
        public static string WarningMedsupplyPoreqMessage = "ShowwarningLookupPopup('<<MedicalSupplyRequestPoDescription>>');";
        public static string WarningMedsupplyPoNoaction = "ShowwarningLookupPopup('<<MedicalSupplyRequestPoDescription>> No action performed');";
        public static string WarningMedsupplyPoNorecordreview = "ShowwarningLookupPopup('<<MedicalSupplyRequestPoDescription>> No records found in review grid');";
        public static string MedicalSuppliesRequestPoemailMessage = "ShowPopup('<<MedicalSuppliesRequestPoemailMessage>> Email has resent to respective stakeholders');";

        // IT Parts Request PO Page
        public static string ITRequestPOSaveMessage = "ShowPopup('IT Request(<<ITRequestPODescription>>) is saved successfully,Click here to  ');";
        public static string ITRequestPOMessage = "ShowPopup('IT Request (<<ITRequestPODescription>>) is updated successfully, Click here to    ');";
        public static string DeleteITRDetailsPODeleteMessage = "ShowdelPopup('IT Request <<DeleteITRPODetails>> is deleted successfully');";
        public static string ITRequestPOErrorMessage = "ShowdelPopup('<<ITRequestPODescription>>');";
        public static string WarningITreqPOMessage = "ShowwarningLookupPopup('<<ITRequestPODescription>>');";
        public static string WarningITReqPOMessage = "ShowwarningLookupPopup('<<ITRequestPODescription>> Item grid fields are should not be empty');";
        public static string WarningITReqPOMessagedate = "ShowwarningLookupPopup('<<ITRequestPODescription>> Date from should be less than date to');";
        public static string ITPOemailMessage = "ShowPopup('<<ITRequestPODescription>> Email has resent to respective stakeholders');";
        public static string EmptyGridITOrderMessage = "ShowwarningPopup('<<ITRequestPODescription>> No records found in review grid ');";

        // Capital PO Page
        public static string CapitalOrderSaveMessage = "ShowPopup('Major Item Order <<MajorItemOrder>> is generated successfully');";
        public static string CapitalOrderupdateMessage = "ShowPopup('Major Item Order <<MajorItemOrder>> is updated successfully');";
        public static string CapitalOrderDeleteMessage = "ShowdelPopup('Major Item Order <MajorItemOrder>> is deleted successfully');";
        public static string CapitalOrderErrorMessage = "ShowdelPopup('<<MajorItemOrder>>');";
        public static string WarningCapitalOrderMessage = "ShowwarningPopup('<<MajorItemOrder>>');";
        public static string CapitalOrdervalidMessage = "ShowwarningLookupPopup('<<MajorItemOrder>> Dont have permission to order');";
        public static string CapitalOrderemailMessage = "ShowPopup('<<MajorItemOrder>> Email has resent to respective stakeholders');";
        public static string CapitalOrderReq = "Major Item Order changes are updated/approved successfully";
        public static string CapitalReq = "ShowPopup('<<MajorItemOrder>> Major Item Order changes are updated/approved successfully');";
        public static string WarngActionMessage = "ShowwarningPopup('<<MajorItemOrder>> No action performed');";
        public static string OldStatusPend = "Pending Approval";
        public static string EmptyGridCapitalOrderMessage = "ShowwarningPopup('<<MajorItemOrder>> No records found in review grid ');";
        public static string PendingApprovalstatusfornonuser = "Pending Approval ";



        //Machine PO Page
        public static string MachinePartsOrderSaveMessage = "ShowPopup('Machine Parts Order<<MachinePartsOrder>> is generated successfully');";
        public static string MachinePartsOrderupdateMessage = "ShowPopup('Machine Parts Order<<MachinePartsOrder>>changes are updated/approved successfully');";
        public static string MachinePartsOrderDeleteMessage = "ShowdelPopup('Machine Parts Order <<MachinePartsOrder>> is deleted successfully');";
        public static string MachinePartsOrderrErrorMessage = "ShowdelPopup('<<MachinePartsOrder>>');";
        public static string WarningMachinePartsOrderMessage = "ShowwarningPopup('<<MachinePartsOrder>>');";
        public static string MachinePartsOrdervalidMessage = "ShowwarningLookupPopup('<<MachinePartsOrder>> dont have permission to Order');";
        public static string WarningMachinePartsOrderPOMessage = "ShowwarningLookupPopup('<<MachinePartsOrder>> Item grid fields are should not be empty');";
        public static string WarningMachinePartsOrderMessagedate = "ShowwarningLookupPopup('<<MachinePartsOrder>> Date from should be less than date to');";
        public static string RfvRemarkMPO = "Please enter remark";
        public static string MachinePartsOrderReq = "Machine Parts Order is generated successfully";
        public static string MachinePartsOrderUpdateReq = "Machine Parts Order changes are updated/approved successfully";
        public static string PendingApprovalStatus = "Pending Approval";
        public static string btnGenerateApprove = "Approve Order";
        public static string btnGenerateOrder = "Generate/Approve Order";
        public static string MachinePartsOrderemailMessage = "ShowPopup('<<MachinePartsOrder>> Email has resent to respective stakeholders');";


        // Service Request PO Page.

        public static string ServiceRequestPOSaveMessage = "ShowPopup('Service Request PO (<<ServiceRequestPO>>) is saved successfully');";
        public static string ServiceRequestPOUpdateMessage = "ShowPopup('Service Request PO (<<ServiceRequestPO>>) is updated successfully');";
        public static string ServiceRequestPOOrderMessage = "ShowPopup('Service Request PO <<ServiceRequestPO>>');";
        public static string ServiceRequestPOEmptyMessage = "ShowPopup('<<ServiceRequestPO>>');";
        public static string ServiceRequestPODeleteMessage = "ShowdelPopup('Service Request PO <<ServiceRequestPO>> is deleted successfully');";
        public static string ServiceRequestPOErrorMessage = "ShowdelPopup('<<ServiceRequestPO>>');";
        public static string WarningServiceRequestPOMessage = "ShowwarningPopup('<<ServiceRequestPO>>');";
        public static string WarningServiceRequestPOReqMessage = "ShowwarningLookupPopup('<<ServiceRequestPO>> Item grid fields should not be empty');";
        public static string WarningServiceRequestPOQtyMessage = "ShowwarningLookupPopup('<<ServiceRequestPO>> Qty field should not be empty');";
        public static string WarningServiceRequestPOReasonMessage = "ShowwarningLookupPopup('<<ServiceRequestPO>> Please enter the Reason');";
        public static string ServiceRequestPOrderSaveMessage = "ShowPopup('Service request order mail sent is generated successfully');";
        public static string ServicePartsOrderemailMessage = "ShowPopup('<<ServiceRequestPO>> Email has resent to respective stakeholders');";


        // Service Request Receive
        public static string ServiceRequestROSaveMessage = "ShowPopup('Service Request RO (<<ServiceRequestRO>>) is saved successfully');";
        public static string ServiceRequestROemailMessage = "ShowPopup('<<ServiceRequestRO>> Void Email has sent to respective stakeholders');";

        //Status

        public static string OrderStatus = "Ordered and Pending Receive";
        public static string PendingOrderStatus = "Pending Order";
        public static string DeniedStatus = "Denied";
        public static string HoldStatus = "Hold";
        public static string actionOrder = "Order";
        public static string actionDeny = "Deny";
        public static string actionHold = "Hold";
        public static string actionApprove = "Approve";
        public static string Completed = "Completed";
        public static string Approved = "Approved";
        public static string PendingApproval = "Pending Approval";
        public static string PendingApprovalfornonuser = "Pending Approval";
        public static string PendingApprovalforreq = "Pending Approval";
        public static string PendingOrderforreq = "Pending Order";
        public static string JobCompleted = "Job Completed and Pending Invoice";
        public static string ReceivingStatus = "Received and Pending Invoice";
        public static string BackOrderStatus = "Back Order";
        public static string PartialOrderStatus = "Partial Order";
        public static string VoidOrderStatus = "Void Order";
        public static string CloseOrderStatus = "Closed";
        public static string OtherReason = "Other";


        public static string Actionvoid = "Void Order";
        public static string Actionclosed = "Closed";
        public static string ActionPartial = "Partial Order";
        public static string Statusnonuser = "Ordered and Pending Receive";
        public static string Statususer = "Received and Pending Invoice";


        // IT Parts Receiving Page
        public static string ITReceivingSaveMessage = "ShowPopup('IT receiving order<<ITRequestReceive>> is Saved successfully');";
        public static string ITReceivingMessage = "ShowPopup('IT receiving order (<<ITRequestReceive>>) is updated successfully');";
        public static string ITReceivingErrorMessage = "ShowdelPopup('<<ITRequestReceive>>');";
        public static string WarningITreceMessage = "ShowwarningLookupPopup('<<ITRequestReceive>>');";
        public static string WarningITreceMessagedate = "ShowwarningLookupPopup('<<ITRequestReceive>> Date from should be less than date to');";
        public static string ITReceiveemailMessage = "ShowPopup('<<ITRequestReceive>> Email has sent to respective stakeholders');";


        //Machine Receive Page
        public static string MachinePartsReceiveSaveMessage = "ShowPopup('Machine Parts Receiving Order<<MachinePartsReceive>> is generated successfully');";
        public static string MachinePartsReceiveupdateMessage = "ShowPopup('Machine Parts Receiving Order<<MachinePartsReceive>>changes are updated/approved successfully');";
        public static string MachinePartsReceiveDeleteMessage = "ShowdelPopup('Machine Parts Receiving Order <<MachinePartsReceive>> is deleted successfully');";
        public static string MachinePartsReceiveErrorMessage = "ShowdelPopup('<<MachinePartsReceive>>');";
        public static string WarningMachinePartsReceiveMessage = "ShowwarningPopup('<<MachinePartsReceive>> Date should not be greater than today');";
        public static string MachinePartsReceivevalidMessage = "ShowwarningLookupPopup('<<MachinePartsReceive>> dont have permission to Order');";
        public static string WarningMachinePartsReceivePOMessage = "ShowwarningLookupPopup('<<MachinePartsReceive>> Item grid fields are should not be empty');";
        public static string WarningMachinePartsReceiveMessagedate = "ShowwarningLookupPopup('<<MachinePartsReceive>> Date from should be less than date to');";
        public static string MachinePartsReceiveemailMessage = "ShowPopup('<<MachinePartsReceive>> Email has sent to respective stakeholders');";
        //Receiving Status
        public static string StatusOrderedPendingReceive = "Ordered and Pending Receive";
        public static string StatusReceivedPendingInvoice = "Received and Pending Invoice";
        public static string StatusBackOrder = "Back Order";
        public static string StatusVoidOrder = "Void Order";
        public static string StatusClosed = "Closed";

        //Medical suppliesReceiving Order
        public static string MedicalSuppliesReceivingSaveMsg = "ShowPopup('<<MedicalSupplyReceivingDescription>> Medical Supplies Receiving Order is saved successfully');";
        public static string MedicalSuppliesReceivingSaveMessage = "ShowPopup('Medical Supplies RequestPo(<<MedicalSupplyReceivingDescription>>) is saved successfully');";
        public static string MedicalSuppliesReceivingMessage = "ShowPopup('Medical Supplies RequestPo (<<MedicalSupplyReceivingDescription>>) is updated successfully');";
        public static string MedicalSuppliesReceivingDeleteMessage = "ShowdelPopup('Medical Supplies RequestPo(<<MedicalSupplyReceivingDescription>>) is deleted successfully');";
        public static string MedicalSuppliesReceivingErrorMessage = "ShowdelPopup('<<MedicalSupplyReceivingDescription>>');";
        public static string WarningValidInvoiceDateMessage = "Invoice date should be greater than purchase Order date";
        public static string WarningMedsupplyReceivingreqMessage = "ShowwarningLookupPopup('<<MedicalSupplyReceivingDescription>>');";
        public static string MedicalSuppliesReceivingemailMessage = "ShowPopup('<<MedicalSuppliesReceivingemailMessage>> Email has sent to respective stakeholders');";


        // Capital Receiving Page
        public static string CapitalReceivingSaveMessage = "ShowPopup('Major Item Receiving order <<MajorItemReceiving>> is saved successfully');";
        public static string CapitalReceivingMessage = "ShowPopup('Major Item Receiving order <<MajorItemReceiving>> is updated successfully');";
        public static string CapitalReceivingErrorMessage = "ShowdelPopup('<<MajorItemReceiving>>');";
        public static string WarningCapitalreceivMessage = "ShowwarningLookupPopup('<<MajorItemReceiving>>');";
        public static string WarningCapitalreceMessagedate = "ShowwarningLookupPopup('<<MajorItemReceiving>>');";
        public static string CapitalReceiveemailMessage = "ShowPopup('<<MajorItemReceiving>> Email has sent to respective stakeholders');";
        public static string Statemail = "Email has resent to respective stakeholders";


        // Ending Inventory Page

        public static string EndingInventorySaveMessage = "ShowPopup('Ending Inventory is saved successfully');";
        public static string EndingInventoryUpdateMessage = "ShowPopup('Ending Inventory is Updated successfully');";
        public static string EndingInventoryErrorMessage = "ShowdelPopup('<<EndingInventory>>');";
        public static string WarningEndingInventoryMessage = "ShowwarningLookupPopup('Ending Inventory for selected month not entered');";
        public static string WarningEndingInventoryMinusMessage = "ShowwarningLookupPopup('Month calculation should not be negative');";

        // Transfer IN page
        public static string StatusComplete = "Completed";
        public static string StatusPendingTransfer = "Pending Transfer";
        public static string StatusVoid = "Void";

        public static string TransferInSaveMsg = "ShowPopup('<<TransferInDescription>> Transfer-In is saved Successfully');";
        public static string TransferInSaveMessage = "ShowPopup('TransferIn(<<TransferInDescription>>) is saved successfully');";
        public static string TransferInMessage = "ShowPopup('Medical Supplies RequestPo (<<TransferInDescription>>) is updated successfully');";
        public static string TransferInDeleteMessage = "ShowdelPopup('TransferIn(<<TransferInDescription>>) is deleted successfully');";
        public static string TransferInErrorMessage = "ShowdelPopup('<<TransferInDescription>>');";
        public static string WarningTransferInreqMessage = "ShowwarningLookupPopup('<<TransferInDescription>>');";
        public static string WarningTransferInNoRecord = "ShowwarningLookupPopup('<<TransferInDescription>> No records found in review grid');";
        public static string WarningTransferInNoAction = "ShowwarningLookupPopup('<<TransferInDescription>> No action performed');";
        public static string TransferInemailMessage = "ShowPopup('<<TransferInDescriptionemailMessage>> Email has sent to respective stakeholders');";

        // TransferOut Page
        public static string TransferOutSaveMessage = "ShowPopup('Transfer Out<<TransferOut>> is saved successfully');";
        public static string TransferOutUpdateMessage = "ShowPopup('Transfer Details<<TransferOut>> is updated successfully');";
        public static string TransferOutVoidMessage = "ShowPopup('Transfer Details<<TransferOut>> is voided successfully');";
        public static string TransferOutErrorMessage = "ShowdelPopup('<<TransferOut>>');";
        public static string WarningTransferOutMessage = "ShowwarningPopup('<<TransferOut>> Date should not be allowed');";
        public static string WarningTransferOutGoBackMessage = "ShowwarningPopup('<<TransferOut>> Actions cannot be saved');";
        public static string WarningTransferOutRemarks = "ShowwarningPopup('<<TransferOut>> Please enter a reason');";
        public static string WarningTransferOutEmailNotify = "ShowwarningPopup('<<TransferOut>>Email notification should be enabled');";
        public static string WarningTransferOutTransferQtyMessage = "ShowwarningPopup('<<TransferOut>>Please enter transfer quantity');";

        //Transfer History Page
        public static string TransferHistorySaveMsg = "ShowPopup('<<TransferHistoryDescription>>');";
        public static string TransferHistorySaveMessage = "ShowPopup('TransferHistory(<<TransferHistoryDescription>>) is saved successfully');";
        public static string TransferHistoryMessage = "ShowPopup('Transfer History (<<TransferHistoryDescription>>) is updated successfully');";
        public static string TransferHistoryDeleteMessage = "ShowdelPopup('Transfer History(<<TransferHistoryDescription>>) is deleted successfully');";
        public static string TransferHistoryErrorMessage = "ShowdelPopup('<<TransferHistoryDescription>>');";
        public static string WarningTransferHistoryreqMessage = "ShowwarningLookupPopup('<<TransferHistoryDescription>>');";
        public static string TransferHistoryemailMessage = "ShowPopup('<<TransferHistoryDescriptionemailMessage>> Email has sent to respective stakeholders');";

        //Corporate Master Page

        public static string CorporateSaveMessage = "ShowPopup('Corporate<<Corporate>> is saved successfully');";
        public static string CorporateUpdateMessage = "ShowPopup('Corporate<<Corporate>> is updated successfully');";
        public static string CorporateDeleteMessage = "ShowPopup('Corporate (<<Corporate>>) is deleted successfully');";
        public static string CorporateRestoreMessage = "ShowPopup('Corporate (<<Corporate>>) is restored successfully');";
        public static string CorporateErrorMessage = "ShowdelPopup('<<Corporate>>');";
        public static string WarningCorporateMessage = "ShowwarningLookupPopup('No actions performed');";
        public static string WarningValidCorporateMessage = "ShowwarningLookupPopup('Corporate Code (<<Corporate>>) are aleady exist');";
        public static string WarningValidCorporatecdescMessage = "ShowwarningLookupPopup('Corporate Description (<<Corporate>>) are aleady exist');";
        public static string CorporatedeleteMessage = "ShowwarningLookupPopup('Corporate: <<Corporate>> is not allow to delete, its mapped with Facility');";


        // Report 
        //Vendor Order Due Remainder
        public static string VendorOrderDueRemainderTechMailSuccessMessage = "ShowPopup('Vendor order due email sent  to Tech Person of this (<<VendorOrderDueRemainder>>) facility');";
        public static string VendorOrderDueRemainderAdminMailSuccessMessage = "ShowPopup('Vendor order due email sent  to Admin Person of this (<<VendorOrderDueRemainder>>) Facility');";
        public static string VendorOrderDueRemainderDeleteMessage = "ShowdelPopup('VendorOrderDueRemainder(<<VendorOrderDueRemainder>>) is deleted successfully');";
        public static string VendorOrderDueRemainderErrorMessage = "ShowdelPopup('<<VendorOrderDueRemainder>>');";
        public static string WarningVendorOrderDueRemainderMessage = "ShowwarningLookupPopup('<<VendorOrderDueRemainder>>');";
        public static string VendorOrderDueRemainderMail = "Tech person is not configured to send a vendor overdue email, So Intimation email sent to super admin to add tech person for this (";
        public static string VendorOrderDueRemainderPreviewMail = "Tech person is not configured to send a vendor overdue email. Please contact Super admin/Facility Admin to add tech person for this (";
        public static string VendorOrderDueRemaindnovendordue = "No vendor order due for today.";


        //shipping

        public static string loadshipping = "Regular";


        //Report  Page
        public static string ReportErrorMessage = "ShowdelPopup('<<Report>>');";
        public static string WarningReportMessage = "ShowwarningPopup('<<Report>>');";
    }
}