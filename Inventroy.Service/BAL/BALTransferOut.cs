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
'' Name      : BALTransferOUT
'' Type      :   C# File
'' Description  :<< Get / Set Business Transfer Out >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 14/Mar/2017		   V2.0				   Sairam.P		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service.BAL
{
    public class BALTransferOut
    {
        #region private Declaration

        // Private Transfer Out

        [DataMember]
        public Int64 TransferID { get; set; }
        [DataMember]
        public Int64 FacilitySupplyID { get; set; }
        [DataMember]
        public Int64 CorporateID { get; set; }
        [DataMember]
        public Int64 CorporateIDFrom { get; set; }
        [DataMember]
        public Int64 CorporateIDTo { get; set; }
        [DataMember]
        public Int64 FacilityID { get; set; }
        [DataMember]
        public Int64 CategoryID { get; set; }
        [DataMember]
        public Int64 FacilityIDFrom { get; set; }
        [DataMember]
        public Int64 FacilityIDTo { get; set; }
        [DataMember]
        public Int64 ItemCategory { get; set; }
        [DataMember]
        public Int64 ItemID { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public Int64 QtyPack { get; set; }
        [DataMember]
        public string UOM { get; set; }
        [DataMember]
        public Int64 UOMID { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public string TransferNo { get; set; }
        [DataMember]
        public Int64 TransferOut { get; set; }
        [DataMember]
        public Int64 TransferQty { get; set; }
        [DataMember]
        public DateTime TransferDate { get; set; }
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
        public string ListCategoryID { get; set; }
        [DataMember]
        public string ListTransferFrom { get; set; }
        [DataMember]
        public string ListTransferTo { get; set; }
        [DataMember]
        public string ListStatus { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        #endregion
    }
}