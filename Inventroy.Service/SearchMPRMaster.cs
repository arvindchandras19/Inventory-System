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
    
    public partial class SearchMPRMaster
    {
        public long MPRMasterID { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public string CorporateName { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public string FacilityDescription { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string VendorDescription { get; set; }
        public string Hoursonmachine { get; set; }
        public string EquipmentCategory { get; set; }
        public string EquipementList { get; set; }
        public string SerialNo { get; set; }
        public string Shipping { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string MPRNo { get; set; }
        public Nullable<long> EquipmentCategoryID { get; set; }
        public Nullable<long> EquipementListID { get; set; }
        public string ShippingCost { get; set; }
        public string Tax { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public string status { get; set; }
        public Nullable<long> EquipementSubCategoryID { get; set; }
        public string EquipmentSubCategory { get; set; }
        public string Remark { get; set; }
        public string Audit { get; set; }
        public string FacilityShortName { get; set; }
        public string VendorShortName { get; set; }
    }
}
