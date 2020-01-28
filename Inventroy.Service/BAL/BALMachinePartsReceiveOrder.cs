using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALMachinePartsReceiveOrder
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
        public long VendorID { get; set; }
        [DataMember]
        public string FinalStatus { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public long MachinePartsRequestOrderID { get; set; }
        [DataMember]
        public long MachinePartsReceiveMasterID { get; set; }
        [DataMember]
        public long MachinePartsReceiveDetailsID { get; set; }
        [DataMember]
        public string MPONo { get; set; }
        [DataMember]
        public string MPRONo { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public Decimal TotalCost { get; set; }
        [DataMember]
        public string ShippingCost { get; set; }
        [DataMember]
        public string Tax { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public byte[] OrderContent { get; set; }
        [DataMember]
        public long MPRMasterID { get; set; }
        [DataMember]
        public long LoggedinBy { get; set; }
        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListVendorID { get; set; }
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
        public string Others { get; set; }
        [DataMember]
        public Int64 PartialBy { get; set; }
        [DataMember]
        public DateTime? PartialOn { get; set; }
        [DataMember]
        public DateTime? VoidOn { get; set; }
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
        public string Filter { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public long INSERTRECORDID { get; set; }
        [DataMember]
        public List<GetMPRDetailsbyMPRMasterID> MPDetailsList { get; set; }

    }
}