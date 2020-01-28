
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;


namespace Inventroy.Service.BAL
{
    public class BALUom
    {
        [DataMember]
        public Int64 UomID { get; set; }
        [DataMember]
        public string UomName { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public DateTime LastModifiedOn { get; set; }
        [DataMember]
        public Int64 LastModifiedBy { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public string IsActivestr { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}

