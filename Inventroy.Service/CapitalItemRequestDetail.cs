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
    
    public partial class CapitalItemRequestDetail
    {
        public long CapitalItemDetailsID { get; set; }
        public long CapitalItemMasterID { get; set; }
        public Nullable<long> EquipmentSubCategory { get; set; }
        public Nullable<long> EquipmentList { get; set; }
        public string SerialNo { get; set; }
        public Nullable<decimal> PricePerUnit { get; set; }
        public Nullable<long> OrderQuantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
    }
}
