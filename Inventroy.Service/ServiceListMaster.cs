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
    
    public partial class ServiceListMaster
    {
        public ServiceListMaster()
        {
            this.ServiceRequestMasters = new HashSet<ServiceRequestMaster>();
        }
    
        public long ServiceListID { get; set; }
        public Nullable<long> ServiceCategoryID { get; set; }
        public string ServiceListDescription { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
    
        public virtual ServiceCategoryMaster ServiceCategoryMaster { get; set; }
        public virtual ICollection<ServiceRequestMaster> ServiceRequestMasters { get; set; }
    }
}
