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
    
    public partial class CorporateMaster
    {
        public CorporateMaster()
        {
            this.Facilities = new HashSet<Facility>();
            this.MachinePartsRequestMasters = new HashSet<MachinePartsRequestMaster>();
            this.ServiceRequestMasters = new HashSet<ServiceRequestMaster>();
            this.MedicalSuppliesRequestMasters = new HashSet<MedicalSuppliesRequestMaster>();
            this.MedicalSuppliesRequestMasters1 = new HashSet<MedicalSuppliesRequestMaster>();
        }
    
        public long CorporateID { get; set; }
        public string CorporateName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string POEmail { get; set; }
        public string CorporateDescription { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    
        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<MachinePartsRequestMaster> MachinePartsRequestMasters { get; set; }
        public virtual ICollection<ServiceRequestMaster> ServiceRequestMasters { get; set; }
        public virtual ICollection<MedicalSuppliesRequestMaster> MedicalSuppliesRequestMasters { get; set; }
        public virtual ICollection<MedicalSuppliesRequestMaster> MedicalSuppliesRequestMasters1 { get; set; }
    }
}
