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
    
    public partial class GetNonsuperAdminMedicalSupplyMaster
    {
        public long MedicalSuppliesID { get; set; }
        public string Corporate { get; set; }
        public string Facility { get; set; }
        public string Vendor { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<System.DateTime> OrderPeriod { get; set; }
        public string Shipping { get; set; }
        public string TimeDelivery { get; set; }
        public string PRNo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public string LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public long CorporateID { get; set; }
        public long FacilityID { get; set; }
        public long VendorID { get; set; }
        public string Audit { get; set; }
    }
}
