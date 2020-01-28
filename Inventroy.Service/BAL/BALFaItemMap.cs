using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALFaItemMap
    {
        [DataMember]
        public Int64 FacilityItemMapID { get; set; }        
        [DataMember]
        public String ItemID { get; set; }
        [DataMember]
        public Int64 FacilityID { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int64 CreatedBy { get; set; }

    }
}
