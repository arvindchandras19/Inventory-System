#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      : BALServiceRequest
'' Type      :   C# File
'' Description  :<< Get / Set Business Ending Invenmtory >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 07/Mar/2017		   V2.0				   Vivekanand.S		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service.BAL
{
    public class BALEndingInventory
    {
        #region private Declaration

        // Private Ending Invenmtory

        [DataMember]
        public Int64 EndingInvenID { get; set; }
        [DataMember]
        public Int64 FacilitySupplyID { get; set; }
        [DataMember]
        public Int64 CorporateID { get; set; }
        [DataMember]
        public Int64 FacilityID { get; set; }
        [DataMember]
        public Int64 VendorID { get; set; }
        [DataMember]
        public Int64 CategoryID { get; set; }
        [DataMember]
        public Int64 ItemID { get; set; }        
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public Int64 QtyPack { get; set; }
        [DataMember]
        public Int64 UOM { get; set; }
        [DataMember]
        public Int64 BeggingInven { get; set; }
        [DataMember]
        public Int64 ReceiveingInven { get; set; }
        [DataMember]
        public Int64 TransferIn { get; set; }
        [DataMember]
        public Int64 TransferOut { get; set; }
        [DataMember]
        public Int64 ExpiredMeds { get; set; }
        [DataMember]
        public DateTime MonthYear { get; set; }
        [DataMember]
        public bool IsNewFacility { get; set; }

        [DataMember]
        public Int64 EndingInven { get; set; }
        [DataMember]
        public Int64 MonthlyUsage { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public DateTime LastModifiedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public Int64 LastModifiedBy { get; set; }

        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListVendorID { get; set; }
        [DataMember]
        public string ListCategoryID { get; set; }
        [DataMember]
        public string ListStatus { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public string ItemType { get; set; }
        [DataMember]
        public DateTime ReceiveDate { get; set; }
        [DataMember]
        public DateTime TransferINDate { get; set; }
        [DataMember]
        public DateTime TransferOutDate { get; set; }
        [DataMember]
        public bool IsNewRecord { get; set; }
        [DataMember]
        public Int64 Noofvisit { get; set; }

        #endregion
    }
}