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
    
    public partial class GetCapitalItemDetails
    {
        public long CapitalItemMasterID { get; set; }
        public long CapitalItemDetailsID { get; set; }
        public Nullable<long> EquipementSubCategoryID { get; set; }
        public string EquipmentSubCategory { get; set; }
        public Nullable<long> EquipementListID { get; set; }
        public string EquipementList { get; set; }
        public Nullable<decimal> PricePerUnit { get; set; }
        public Nullable<long> OrderQuantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string SerialNo { get; set; }
        public string Reason { get; set; }
        public Nullable<long> Lockedby { get; set; }
        public int IsReadOnly { get; set; }
    }
}
