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
    
    public partial class MedicalsuppliesRequestApprove
    {
        public long MedicalSuppliesApproveID { get; set; }
        public long PRMasterID { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
    }
}
