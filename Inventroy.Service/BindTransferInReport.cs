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
    
    public partial class BindTransferInReport
    {
        public long TransferOutID { get; set; }
        public string TransferNo { get; set; }
        public string TransferDate { get; set; }
        public long CorporateIDFrom { get; set; }
        public long CorporateIDTo { get; set; }
        public long FacilityIDFrom { get; set; }
        public long FacilityIDTo { get; set; }
        public string CorporateName { get; set; }
        public string FacilityFrom { get; set; }
        public string FacilityTo { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public long ItemID { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<long> QtyPack { get; set; }
        public Nullable<long> UOM { get; set; }
        public string UomName { get; set; }
        public Nullable<long> Transferqty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LoggedBy { get; set; }
    }
}
