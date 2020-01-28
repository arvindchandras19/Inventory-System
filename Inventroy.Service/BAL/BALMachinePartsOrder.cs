using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      : BALMPRMasterOrder
'' Type      :   C# File
'' Description  :<< Get / Set Business Machine Purchase Request Master Order >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	12/26/2017		   V.0				   Sairam.P		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALMachinePartsOrder
    {
        
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long DeletedBy { get; set; }
        [DataMember]
        public string InvenValue { get; set; }
        [DataMember]
        public string Updatekeyvalue { get; set; }
        [DataMember]
        public long CorporateID { get; set; }
        [DataMember]
        public long FacilityID { get; set; }
        [DataMember]
        public long VendorID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public long MachinePartsRequestOrderID { get; set; }
        [DataMember]
        public string MPRNo { get; set; }
        [DataMember]
        public string MPONo { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public byte[] OrderContent { get; set; }
        [DataMember]
        public long MPRMasterID { get; set; }
        [DataMember]
        public long LoggedinBy { get; set; }
        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListVendorID { get; set; }
    }
}