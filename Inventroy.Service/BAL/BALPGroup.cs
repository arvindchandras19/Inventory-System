using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALPGroup
    {
        [DataMember]
        public Int64 CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public DateTime LastModifiedOn { get; set; }
        [DataMember]
        public Int64 LastModifiedBy { get; set; }
        [DataMember]
        public string Usage { get; set; }
        public string IsStrActive { get; set; }

        public Int64 LoggedinBy { get; set; }
        public string SearchItem { get; set; }

        public string Filter { get; set; }
        public bool IsActive { get; set; }

    }
}