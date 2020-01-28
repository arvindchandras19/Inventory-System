using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALFacilityVendorAccount
    {
        [DataMember]
        public Int64 FacilityVendorAccID { get; set; }
        [DataMember]
        public Int64 VendorID { get; set; }
        [DataMember]
        public Int64 FacilityID { get; set; }
        [DataMember]
        public String ShipAccount { get; set; }
        [DataMember]
        public String BillAccount { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }


        [DataMember]
        public string ListVendorID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        public string IsStrActive { get; set; }

        public Int64 LoggedIN { get; set; }
        public string Filter { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}