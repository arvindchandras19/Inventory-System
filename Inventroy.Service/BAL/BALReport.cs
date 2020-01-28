using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALReport
    {
        [DataMember]
        public string CorporateID { get; set; }
        [DataMember]
        public string CorporateName { get; set; }
        [DataMember]
        public string FacilityID { get; set; }
        [DataMember]
        public string FacilityName { get; set; }
        [DataMember]
        public string VendorID { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public string ItemCategoryID { get; set; }
        [DataMember]
        public string OrderType { get; set; }
        [DataMember]
        public string ItemID { get; set; }
        [DataMember]
        public Nullable<DateTime> MonthYear { get; set; }
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
        public Nullable<DateTime> DateFrom { get; set; }
        [DataMember]
        public Nullable<DateTime> DateTo { get; set; }
        [DataMember]
        public string Filter { get; set; }






    }
}
