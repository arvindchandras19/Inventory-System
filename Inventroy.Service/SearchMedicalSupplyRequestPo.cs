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
    
    public partial class SearchMedicalSupplyRequestPo
    {
        public long MedicalSuppliesID { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string CorporateName { get; set; }
        public string FacilityShortName { get; set; }
        public string VendorShortName { get; set; }
        public string PRNo { get; set; }
        public Nullable<long> TotalPrice { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public string Audit { get; set; }
        public string PONo { get; set; }
        public string Action { get; set; }
        public string ShipAccount { get; set; }
        public string LoggedBy { get; set; }
        public string BillAccount { get; set; }
    }
}
