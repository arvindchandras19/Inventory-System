using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALMedicalSupplyReceivingOrder
    {
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long DeletedBy { get; set; }
        [DataMember]
        public string InvenValue { get; set; }
        [DataMember]
        public string Updatekeyvalue { get; set; }
        [DataMember]
        public long CorporateID { get; set; }
        [DataMember]
        public long FacilityID { get; set; }
        [DataMember]
        public long ItemID { get; set; }
        [DataMember]
        public long Vendor { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public long MedicalSupplyMasterID { get; set; }
        [DataMember]
        public string PRNo { get; set; }
        [DataMember]
        public string PONo { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public byte[] OrderContent { get; set; }
        [DataMember]
        public long PRMasterID { get; set; }
        [DataMember]
        public string CorporateName { get; set; }
        [DataMember]
        public string FacilityName { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public long LoggedinBy { get; set; }
        [DataMember]
        public string LockTimeOut { get; set; }

        [DataMember]
        public long MedicalSuppliesReceivingMasterID { get; set; }
        [DataMember]
        public long MedicalSuppliesReceivingDetailsID { get; set; }
        [DataMember]
        public long MedicalSuppliesRequestDetailsID { get; set; }
        [DataMember]
        public long MedicalSuppliesRequestOrderID { get; set; }
        [DataMember]
        public DateTime? PackingDate { get; set; }
        [DataMember]
        public DateTime? ReceivedDate { get; set; }
        [DataMember]
        public DateTime? PackingSlipDate { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public string PackingSlipNo { get; set; }
        [DataMember]
        public string InvoiceNo { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string InvoiceStatus { get; set; }
        [DataMember]
        public Int64 InvoicedBy { get; set; }
        [DataMember]
        public string ReceivingAction { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string OtherReason { get; set; }
        [DataMember]
        public Int64 PartialBy { get; set; }
        [DataMember]
        public DateTime? PartialOn { get; set; }
        [DataMember]
        public Int64 VoidBy { get; set; }
        [DataMember]
        public Int64 ClosedBy { get; set; }
        [DataMember]
        public DateTime? ClosedOn { get; set; }
        [DataMember]
        public int OrderQty { get; set; }
        [DataMember]
        public int BalanceQty { get; set; }
        [DataMember]
        public int ReceivedQty { get; set; }
        [DataMember]
        public Decimal TotalCost { get; set; }
        [DataMember]
        public Decimal ShippingCost { get; set; }
        [DataMember]
        public Decimal Tax { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public string FinalStatus { get; set; }
        [DataMember]
        public long INSERTRECORDID { get; set; }
        [DataMember]
        public string SearchFilters { get; set; }
    }
}