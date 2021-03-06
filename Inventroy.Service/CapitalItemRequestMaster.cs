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
    
    public partial class CapitalItemRequestMaster
    {
        public long CapitalItemMasterID { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string CRNo { get; set; }
        public Nullable<bool> RequestType { get; set; }
        public string Status { get; set; }
        public string Shipping { get; set; }
        public string ShippingCost { get; set; }
        public string Tax { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> LockedIn { get; set; }
        public Nullable<System.DateTime> LockedOut { get; set; }
        public Nullable<long> LockedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<long> DeniedBy { get; set; }
    }
}
