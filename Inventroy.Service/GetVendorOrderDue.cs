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
    
    public partial class GetVendorOrderDue
    {
        public Nullable<long> VenOrderDueID { get; set; }
        public Nullable<long> CorporateID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string OrderType { get; set; }
        public Nullable<System.DateTime> OrderdueDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<long> DeliveryWindow { get; set; }
        public Nullable<long> DaysToNotify { get; set; }
        public string FacilityDescription { get; set; }
        public string VendorDescription { get; set; }
        public string CorporateDescription { get; set; }
        public string FacilityShortName { get; set; }
        public string VendorShortName { get; set; }
        public string CorporateName { get; set; }
    }
}
