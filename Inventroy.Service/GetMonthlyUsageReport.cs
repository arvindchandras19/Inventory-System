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
    
    public partial class GetMonthlyUsageReport
    {
        public long CorporateID { get; set; }
        public Nullable<long> FacilityID { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public long ItemID { get; set; }
        public string VendorItemID { get; set; }
        public string ItemDescription { get; set; }
        public long QtyPack { get; set; }
        public long UomID { get; set; }
        public Nullable<decimal> EachPrice { get; set; }
        public string UomName { get; set; }
        public Nullable<long> BeginingInvQty { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public long ReceiveingOrderInvQty { get; set; }
        public Nullable<System.DateTime> TransferINDate { get; set; }
        public long TransferInQty { get; set; }
        public Nullable<System.DateTime> TransferOutDate { get; set; }
        public long TransferOutQty { get; set; }
        public Nullable<long> ExpiredMeds { get; set; }
        public Nullable<long> EndingInvQty { get; set; }
        public Nullable<long> MonthlyUsage { get; set; }
        public Nullable<bool> NewFacility { get; set; }
        public long Noofvisit { get; set; }
        public string FACILITYSHORTNAME { get; set; }
    }
}
