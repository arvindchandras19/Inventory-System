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
    
    public partial class SearchMachinePartsReceive
    {
        public long MPRMasterID { get; set; }
        public Nullable<long> VendorID { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public string CorporateName { get; set; }
        public string VendorShortName { get; set; }
        public string FacilityShortName { get; set; }
        public string Audit { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string MPONo { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string MPRONo { get; set; }
        public long MachinePartsReceivingMasterID { get; set; }
        public Nullable<long> MachinePartsRequestOrderID { get; set; }
    }
}
