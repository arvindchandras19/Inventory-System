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
    
    public partial class UserFacilityRole
    {
        public long UserFacilityRoleID { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public Nullable<long> RoleID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public string BudgetCurrency { get; set; }
        public Nullable<long> BudgetValue { get; set; }
    }
}
