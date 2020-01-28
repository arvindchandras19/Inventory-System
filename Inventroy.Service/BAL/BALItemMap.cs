using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALItemMap
    {
        [DataMember]
        public Int64 ItemMapID { get; set; }
        [DataMember]
        public Int64 ItemID { get; set; }
        [DataMember]
        public Int64 VendorID { get; set; }
        [DataMember]
        public string VendorItemID { get; set; }
        [DataMember]
        public Int64 ItemCategory { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }


        [DataMember]
        public string ListVendorID { get; set; }
        [DataMember]
        public string ListItemCategory { get; set; }
        [DataMember]
        public string IsStrActive { get; set; }
        [DataMember]
        public Int64 LoggedIN { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}