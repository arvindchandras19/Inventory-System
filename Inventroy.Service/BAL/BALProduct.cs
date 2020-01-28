using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALProduct
    {
        [DataMember]
        public Int64 GroupID { get; set; }
        [DataMember]
        public Int64 ProductID { get; set; }
        [DataMember]
        public Int64 VendorID { get; set; }
        [DataMember]
        public Int64 ItemID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string UoM { get; set; }
        [DataMember]
        public int QtyPack { get; set; }
        [DataMember]
        public decimal UnitPrice { get; set; }
        [DataMember]
        public decimal EachPrice { get; set; }
        [DataMember]
        public long Unit { get; set; }
        [DataMember]
        public string NDC { get; set; }
        [DataMember]
        public string DialysisoftPrimarykey { get; set; }
        [DataMember]
        public string DialysisoftUOM { get; set; }
        [DataMember]
        public long DialysisoftUnits { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public DateTime LastModifiedOn { get; set; }
        [DataMember]
        public Int64 LastModifiedBy { get; set; }
    }
}