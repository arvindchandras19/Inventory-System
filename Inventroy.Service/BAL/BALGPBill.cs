using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;


namespace Inventroy.Service.BAL
{
    public class BALGPBill
    {
        [DataMember]
        public Int64 GPBillingID { get; set; }
        [DataMember]
        public string GPBillingCode { get; set; }
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