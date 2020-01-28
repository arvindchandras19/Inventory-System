using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
     [DataContract]
    public class BALMedicalSupplyRequestPo
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
        public long Vendor { get; set; }
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
        public long MedicalSupplyMasterID { get; set; }     
        [DataMember]
        public string PRNo { get; set; }
        [DataMember]
        public string PONo { get; set; } 
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public byte[] OrderContent { get; set; }
        [DataMember]
        public long PRMasterID { get; set; }
        [DataMember]
        public string CorporateName { get; set; }
        [DataMember]
        public string FacilityName { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public long LoggedinBy { get; set; } 
    }
}