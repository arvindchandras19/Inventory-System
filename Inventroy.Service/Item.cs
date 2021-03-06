//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventroy.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public long ItemID { get; set; }
        public string ItemShortName { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public Nullable<long> QtyPack { get; set; }
        public Nullable<long> UOM { get; set; }
        public Nullable<decimal> EachPrice { get; set; }
        public string UnitPriceCurrency { get; set; }
        public Nullable<decimal> UnitPriceValue { get; set; }
        public string NDC { get; set; }
        public Nullable<bool> Standard { get; set; }
        public Nullable<bool> NonStandard { get; set; }
        public string GPBillingCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
}
