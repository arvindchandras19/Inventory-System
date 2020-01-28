﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALVendorOrderDue
    {
        [DataMember]
        public long VendorOrderdueID { get; set; }
        [DataMember]
        public Int64 CorporateID { get; set; }
        [DataMember]
        public string CorporateName { get; set; }
        [DataMember]
        public Int64 FacilityID { get; set; }
        [DataMember]
        public string FacilityName { get; set; }
        [DataMember]
        public long VendorID { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        //[DataMember]
        //public string OrderType { get; set; }
        [DataMember]
        public Int32 OrderType { get; set; }
        [DataMember]
        public string OrderTypestr { get; set; }
        [DataMember]
        public DateTime OrderDueDate { get; set; }
        [DataMember]
        public DateTime? DeliveryDate { get; set; }
        [DataMember]
        public long DeliveryWindow { get; set; }
        [DataMember]
        public long DaytoToNotify { get; set; }
        [DataMember]
        public Int64 LoggedInBy { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 LastModifitedBy { get; set; }
        [DataMember]
        public DateTime LastModifitedOn { get; set; }
        [DataMember]
        public DateTime DateFrom { get; set; }
        [DataMember]
        public DateTime DateTo { get; set; }
        public string AppType { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListVendorID { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public string ListCorporateID { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
 
    }
}
