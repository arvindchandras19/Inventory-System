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
'' Name      :   BALMachinemaster.cs
'' Type      :   C# File
'' Description  :<<To add,update the MachineMaster Details>>
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
    public class BALMachinemaster
    {
        [DataMember]
        public long MachineID { get; set; }
        [DataMember]
        public long CorporateID { get; set; }
        [DataMember]
        public long FacilityID { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Manufacturedyear { get; set; }
        [DataMember]
        public Int32 Hoursonthemachine { get; set; }
        [DataMember]
        public long EquipementCategoryID { get; set; }
        [DataMember]
        public long EquipementSubCategoryID { get; set; }
        [DataMember]
        public long EquipementListID { get; set; }
        [DataMember]
        public string EquipementCategorydesc { get; set; }
        [DataMember]
        public string EquipementSubCategorydesc { get; set; }
        [DataMember]
        public string EquipementListdesc { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string SerNo { get; set; }
        [DataMember]
        public string Warranty { get; set; }
        [DataMember]
        public string GpAccountCode { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string IstrActive { get; set; }
        [DataMember]
        public string Filter { get; set; }

    }
}