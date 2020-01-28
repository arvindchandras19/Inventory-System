using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALItem
    {
        [DataMember]
        public Int64 ItemID { get; set; }
        [DataMember]
        public string ItemShortName { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public Int64 CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public Int64 UOM { get; set; }
        [DataMember]
        public string UomName { get; set; }
        [DataMember]
        public Int64 QtyPack { get; set; }
        [DataMember]
        public string UnitPriceCurrency { get; set; }
        [DataMember]
        public decimal UnitPriceValue { get; set; }
        [DataMember]
        public decimal EachPrice { get; set; }
        [DataMember]
        public string NDC { get; set; }
        [DataMember]
        public bool Standard { get; set; }
        [DataMember]
        public bool NonStandard { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
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
        [DataMember]
        public Int64 CurrencyID { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        public string IsStrActive { get; set; }

        public Int64 LoggedinBy { get; set; }
        public string Filter { get; set; }

        public string CategorylistID { get; set; }
    }
}
