using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      :   Page_Controls
'' Type      :   C# File
'' Description  :<<To add,update the Facility Vendor Account Details>>
'' Modification History :<<CorporateID,Machinemaster_Edit,Machinemaster_View added>>
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/09/2017		   V1.0				   s.Vivek                            New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventory
{
    public class Page_Controls
    {
        public bool Req_MedicalSuppliesPage_Edit;
        public bool Req_MedicalSuppliesPage_View;

        public bool Req_WorkOrServicePage_Edit;
        public bool Req_WorkOrServicePage_View;

        public bool Req_MachinePartsPage_Edit;
        public bool Req_MachinePartsPage_View;

        public bool Req_RequestITPage_Edit;
        public bool Req_RequestITPage_View;

        public bool Req_RequestITPOPage_Edit;
        public bool Req_RequestITPOPage_View;

        public bool Req_BuildingSuppliesPage_Edit;
        public bool Req_BuildingSuppliesPage_View;

        public bool Rec_MedicalSuppliesPage_Edit;
        public bool Rec_MedicalSuppliesPage_View;

        public bool Rec_WorkOrServicePage_Edit;
        public bool Rec_WorkOrServicePage_View;

        public bool Rec_MachinePartsPage_Edit;
        public bool Rec_MachinePartsPage_View;

        public bool Rec_BuildingSuppliesPage_Edit;
        public bool Rec_BuildingSuppliesPage_View;

        public bool StocksOrInventory_Daily_Page_Edit;
        public bool StocksOrInventory_Daily_Page_View;

        public bool StocksOrInventory_Ending_Page_Edit;
        public bool StocksOrInventory_Ending_Page_View;

        public bool CurrencyPage_Edit;
        public bool CurrencyPage_View;

        public bool CorporatePage_Edit;
        public bool CorporatePage_View;

        public bool FacilityPage_Edit;
        public bool FacilityPage_View;

        public bool FacilityReligionPage_Edit;
        public bool FacilityReligionPage_View;

        public bool ItemCategoryPage_Edit;
        public bool ItemCategoryPage_View;

        public bool MedicalSuppliesPage_Edit;
        public bool MedicalSuppliesPage_View;

        public bool VendorItemMap_Edit;
        public bool VendorItemMap_View;

        public bool FacilityItemMap_Edit;
        public bool FacilityItemMap_View;

        public bool UomMaster_Edit;
        public bool UomMaster_View;

        public bool AddUserPage_Edit;
        public bool AddUserPage_View;

        public bool UserPermission_Edit;
        public bool UserPermission_View;

        public bool VendorPage_Edit;
        public bool VendorPage_View;

        public bool FacilitySuppliesMap_Edit;
        public bool FacilitySuppliesMap_View;
               
        public bool VendorFacilityActPage_Edit;
        public bool VendorFacilityActPage_View;

        public bool VendorOrderDue_Edit;
        public bool VendorOrderDue_View;

        public bool TransferInPage_Edit;
        public bool TransferInPage_View;

        public bool TransferOutPage_Edit;
        public bool TransferOutPage_View;
        public bool TransferOutPage_Email;

        public bool TransferHistoryPage_Edit;
        public bool TransferHistoryPage_View;

        public bool ReportsPage_Edit;
        public bool ReportsPage_View;

        public bool CapitalItemRequest_Edit;
        public bool CapitalItemRequest_View;

        public bool CorpEquipmentMap_Edit;
        public bool CorpEquipmentMap_View;

        public bool CapitalOrder_Edit;
        public bool CapitalOrder_View;

        public bool MedicalSuppliesOrder_Edit;
        public bool MedicalSuppliesOrder_View;

        public bool MachinePartsOrder_Edit;
        public bool MachinePartsOrder_View;
        public bool MachinePartsOrder_Approve;
        public bool MachinePartsOrder_Order;
        public bool MachinePartsOrder_Deny;

        public bool Req_POServicePage_Edit;
        public bool Req_POServicePage_View;

        public bool CapitalReceiving_Edit;
        public bool CapitalReceiving_View;

        public bool ITReceiving_Edit;
        public bool ITReceiving_View;

        public bool VendorOrderDueRemainder_Edit;
        public bool VendorOrderDueRemainder_View;

        public bool Reports_Edit;
        public bool Reports_View;

        public Int64 UserId;
        public string UserName;
        public Int64 FacilityID;
        public Int64 RoleID;
        public string UserRoleName;
        public Int64 CorporateID;
        public bool MachineMaster_Edit;
        public bool MachineMaster_View;



    }
}