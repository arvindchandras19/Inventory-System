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
    
    public partial class GetCPROMasterReview
    {
        public Nullable<long> CapitalItemMasterID { get; set; }
        public string PackingSlipNo { get; set; }
        public Nullable<System.DateTime> PackingDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string ReceivingAction { get; set; }
        public string Reason { get; set; }
        public string OtherReason { get; set; }
        public string EquimentSubCategory { get; set; }
        public string EquipmentList { get; set; }
        public string SerialNo { get; set; }
        public Nullable<decimal> PricePerQty { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<int> BalanceQty { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ReceivedQty { get; set; }
        public long CapitalReceivingDetailsID { get; set; }
        public Nullable<long> CapitalReceivingMasterID { get; set; }
        public string ShippingCost { get; set; }
        public string Tax { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public string FinalStatus { get; set; }
    }
}
