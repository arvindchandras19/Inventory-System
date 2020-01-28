using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALCapitalReceiving
    {
        #region private Declaration
        private Int64 _CapitalItemMasterID = 0;
        private Int64 _CapitalOrderID = 0;
        private Int64 _CapitalReceivingMasterID = 0;
        private Int64 _CapitalReceivingDetailsID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private string _Audittrial = string.Empty;
        private string _Status = string.Empty;
        private string _CPONo = string.Empty;
        private string _CPRO = string.Empty;
        private string _PackingSlipNo = string.Empty;
        private DateTime? _PackingDate;
        private DateTime? _ReceivedDate;
        private DateTime? _InvoiceDate;
        private string _InvoiceNo = string.Empty;
        private string _ShippingCost = string.Empty;
        private string _Tax = string.Empty;
        private decimal _TotalCost = 0;
        private string _EquipmentSubCategory = string.Empty;
        private string _EquipementList = string.Empty;
        private string _SerialNo = string.Empty;
        private decimal _PricePerQty = 0;
        private Int32 _OrderQty = 0;
        private Int32 _BalenceQty = 0;
        private Int32 _ReceivedQty = 0;
        private decimal _Cost = 0;
        private string _Comments = string.Empty;
        private decimal _TotalPrice = 0;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private string _Remarks = string.Empty;
        private string _Reason = string.Empty;
        private string _ListCorporateID = string.Empty;
        private string _ListFacilityID = string.Empty;
        private string _ListVendorID = string.Empty;
        private Int64 _LoggedinBy = 0;
        private Int64 _InvoicedBy = 0;
        private Int64 _ReceivedBy = 0;
        private Int64 _PartialBy = 0;
        private Int64 _ClosedBy = 0;
        private Int64 _VoidBy = 0;
        private Int64 _ReceivingOrder = 0;
        private string _FinalStatus = string.Empty;
        private string _InvoiceStatus = string.Empty;
        private string _ReceivingAction = string.Empty;
        private string _Type = string.Empty;
        private string _Filter = string.Empty;
        private Int64 _INSERTRECORDID = 0;
        private string _OtherReason = string.Empty;


        #endregion

        [DataMember]
        public string ListCorporateID
        {
            get { return _ListCorporateID; }
            set { _ListCorporateID = value; }
        }
        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [DataMember]
        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }
        [DataMember]
        public string ReceivingAction
        {
            get { return _ReceivingAction; }
            set { _ReceivingAction = value; }
        }
        [DataMember]
        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; }
        }
        [DataMember]
        public string ListFacilityID
        {
            get { return _ListFacilityID; }
            set { _ListFacilityID = value; }
        }

        [DataMember]
        public string ListVendorID
        {
            get { return _ListVendorID; }
            set { _ListVendorID = value; }
        }
        [DataMember]
        public string OtherReason
        {
            get { return _OtherReason; }
            set { _OtherReason = value; }
        }

        [DataMember]
        public Int64 CapitalItemMasterID
        {
            get { return _CapitalItemMasterID; }
            set { _CapitalItemMasterID = value; }
        }

        [DataMember]
        public Int32 OrderQty
        {
            get { return _OrderQty; }
            set { _OrderQty = value; }
        }
        [DataMember]
        public Int32 BalenceQty
        {
            get { return _BalenceQty; }
            set { _BalenceQty = value; }
        }

        [DataMember]
        public Int32 ReceivedQty
        {
            get { return _ReceivedQty; }
            set { _ReceivedQty = value; }
        }
        [DataMember]
        public Int64 CapitalOrderID
        {
            get { return _CapitalOrderID; }
            set { _CapitalOrderID = value; }
        }
        [DataMember]
        public Int64 ReceivingOrder
        {
            get { return _ReceivingOrder; }
            set { _ReceivingOrder = value; }
        }
        [DataMember]
        public Int64 INSERTRECORDID
        {
            get { return _INSERTRECORDID; }
            set { _INSERTRECORDID = value; }
        }
        [DataMember]
        public Int64 CapitalReceivingMasterID
        {
            get { return _CapitalReceivingMasterID; }
            set { _CapitalReceivingMasterID = value; }
        }
        [DataMember]
        public Int64 CapitalReceivingDetailsID
        {
            get { return _CapitalReceivingDetailsID; }
            set { _CapitalReceivingDetailsID = value; }
        }

        [DataMember]
        public Int64 CorporateID
        {
            get { return _CorporateID; }
            set { _CorporateID = value; }
        }

        [DataMember]
        public byte[] OrderContent { get; set; }

        [DataMember]
        public Int64 FacilityID
        {
            get { return _FacilityID; }
            set { _FacilityID = value; }
        }
        [DataMember]
        public Int64 VendorID
        {
            get { return _VendorID; }
            set { _VendorID = value; }
        }
        [DataMember]
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { _DateFrom = value; }
        }
        [DataMember]
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { _DateTo = value; }
        }

        [DataMember]
        public string Audittrial
        {
            get { return _Audittrial; }
            set { _Audittrial = value; }
        }
        [DataMember]
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        [DataMember]
        public string EquipmentSubCategory
        {
            get { return _EquipmentSubCategory; }
            set { _EquipmentSubCategory = value; }
        }
        [DataMember]
        public string EquipementList
        {
            get { return _EquipementList; }
            set { _EquipementList = value; }
        }
        [DataMember]
        public string SerialNo
        {
            get { return _SerialNo; }
            set { _SerialNo = value; }
        }
        [DataMember]
        public string PackingSlipNo
        {
            get { return _PackingSlipNo; }
            set { _PackingSlipNo = value; }
        }

        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [DataMember]
        public string FinalStatus
        {
            get { return _FinalStatus; }
            set { _FinalStatus = value; }
        }
        [DataMember]
        public string InvoiceStatus
        {
            get { return _InvoiceStatus; }
            set { _InvoiceStatus = value; }
        }
        [DataMember]
        public string CPONo
        {
            get { return _CPONo; }
            set { _CPONo = value; }
        }

        [DataMember]
        public string CPRO
        {
            get { return _CPRO; }
            set { _CPRO = value; }
        }
        [DataMember]
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set { _InvoiceNo = value; }
        }

        [DataMember]
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        [DataMember]
        public string ShippingCost
        {
            get { return _ShippingCost; }
            set { _ShippingCost = value; }
        }
        [DataMember]
        public string Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
        }
        [DataMember]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set { _TotalCost = value; }
        }
        [DataMember]
        public decimal Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }
        [DataMember]
        public decimal PricePerQty
        {
            get { return _PricePerQty; }
            set { _PricePerQty = value; }
        }
        [DataMember]
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; }
        }
        [DataMember]
        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        [DataMember]
        public Int64 LastModifiedBy
        {
            get { return _LastModifiedBy; }
            set { _LastModifiedBy = value; }
        }

        [DataMember]
        public Int64 InvoicedBy
        {
            get { return _InvoicedBy; }
            set { _InvoicedBy = value; }
        }
        [DataMember]
        public Int64 ReceivedBy
        {
            get { return _ReceivedBy; }
            set { _ReceivedBy = value; }
        }
        [DataMember]
        public Int64 ClosedBy
        {
            get { return _ClosedBy; }
            set { _ClosedBy = value; }
        }
        [DataMember]
        public Int64 PartialBy
        {
            get { return _PartialBy; }
            set { _PartialBy = value; }
        }

        [DataMember]
        public Int64 VoidBy
        {
            get { return _VoidBy; }
            set { _VoidBy = value; }
        }

        [DataMember]
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public Int64 LoggedinBy { get; set; }

        [DataMember]
        public DateTime? PackingDate { get; set; }

        [DataMember]
        public DateTime? ReceivedDate { get; set; }

        [DataMember]
        public DateTime? InvoiceDate { get; set; }

        [DataMember]
        public DateTime? ClosedOn { get; set; }

        [DataMember]
        public DateTime? VoidOn { get; set; }

        [DataMember]
        public DateTime? PartialOn { get; set; }
    }

}
