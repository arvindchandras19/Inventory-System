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
'' Name      :   BALVendor.cs
'' Type      :   C# File
'' Description  :<<To add,update the Vendor Details>>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Mahalakshmi.S		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALVendor
    {
        [DataMember]
        public long VendorID { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public string VendorUIID { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public long State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public long Xtn { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string ContactPhone { get; set; }
        [DataMember]
        public string ContactEmail { get; set; }
        [DataMember]
        public string POEmail { get; set; }
        [DataMember]
        public string AlternateEmail { get; set; }
        [DataMember]
        public bool All { get; set; }
        [DataMember]
        public bool RegularSupplies { get; set; }
        [DataMember]
        public bool MachineParts { get; set; }
        [DataMember]
        public bool ServiceOrder { get; set; }
        [DataMember]
        public bool BuildingMaintenance { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public bool IT { get; set; }
        [DataMember]
        public string IsStrActive { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public string SearchItem { get; set; }
        [DataMember]
        public bool IsActive { get; set; }



    }
}

