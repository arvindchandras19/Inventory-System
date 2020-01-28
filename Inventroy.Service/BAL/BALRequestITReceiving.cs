using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALRequestITReceiving
    {
        #region private Declaration
        private Int64 _RequestITMasterID = 0;
        private Int64 _ITReceivingMasterID = 0;
        private Int64 _RequestITOrderID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int32 _SortOrder = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private string _User = string.Empty;
        private string _Audittrial = string.Empty;
        private string _Status = string.Empty;
        private string _ITRoNo = string.Empty;
        private string _ITNNo = string.Empty;
        private string _Remarks = string.Empty;
        private byte[] _OrderContent;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private Int64 _LoggedinBy = 0;
        private DateTime? _OrderDate;
        private string _CorporateIDs = string.Empty;
        private string _FacilityIDs = string.Empty;
        private string _VendorIDs = string.Empty;
        private string _PackingSlipNo = string.Empty;
        private string _InvoiceNo = string.Empty;
        private DateTime? _PackingDate;
        private DateTime? _ReceivedDate;
        private DateTime? _InvoiceDate;
        private Int64 _ITReceivingDetailsID = 0;
        private Int64 _EquimentSubCategory = 0;
        private Int64 _EquipmentList = 0;
        private string _SerialNo = string.Empty;
        private decimal _PricePerQty = 0;
        private Int32 _OrderQty = 0;
        private Int32 _BalacedQty = 0;
        private Int32 _ReceivedQty = 0;
        private decimal _TotalPrice = 0;
        private string _Comments = string.Empty;
        private string _Shippingcost = string.Empty;
        private string _Tax = string.Empty;
        private decimal _TotalCost = 0;
        private string _Receivingaction = string.Empty;
        private string _Reason = string.Empty;
        private string _OtherReason = string.Empty;
        private string _InvoiceStatus = string.Empty;
        private Int64 _InvoicedBy = 0;
        private Int64 _PartialBy = 0;
        private DateTime? _PartialOn;
        private Int64 _VoidBy = 0;
        private DateTime? _VoidOn;
        private Int64 _ClosedBy = 0;
        private DateTime? _ClosedOn;
        private string _FinalStatus = string.Empty;
        private string _Type = string.Empty;
        private string _Filter = string.Empty;
        private Int64 _InsertRecordID = 0;
        #endregion
        [DataMember]
        public Int64 RequestITMasterID
        {
            get { return _RequestITMasterID; }
            set { _RequestITMasterID = value; }
        }
        [DataMember]
        public Int64 RequestITOrderID
        {
            get { return _RequestITOrderID; }
            set { _RequestITOrderID = value; }
        }


        [DataMember]
        public Int64 CorporateID
        {
            get { return _CorporateID; }
            set { _CorporateID = value; }
        }
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
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }
        [DataMember]
        public string Audittrial
        {
            get { return _Audittrial; }
            set { _Audittrial = value; }
        }
        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [DataMember]
        public string ITRNo
        {
            get { return _ITRoNo; }
            set { _ITRoNo = value; }
        }
        [DataMember]
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
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
        public Int32 SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }
        [DataMember]
        public byte[] OrderContent
        {
            get { return _OrderContent; }
            set { _OrderContent = value; }
        }
        [DataMember]
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }
        [DataMember]
        public string ITNNo
        {
            get { return _ITNNo; }
            set { _ITNNo = value; }
        }
        [DataMember]
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; }
        }
        [DataMember]
        public Int64 LoggedinBy
        {
            get { return _LoggedinBy; }
            set { _LoggedinBy = value; }
        }
        [DataMember]
        public string CorporateIDs
        {
            get { return _CorporateIDs; }
            set { _CorporateIDs = value; }
        }
        [DataMember]
        public string FacilityIDs
        {
            get { return _FacilityIDs; }
            set { _FacilityIDs = value; }
        }
        [DataMember]
        public string VendorIDs
        {
            get { return _VendorIDs; }
            set { _VendorIDs = value; }
        }
        [DataMember]
        public DateTime? PackingDate
        {
            get { return _PackingDate; }
            set { _PackingDate = value; }
        }
        [DataMember]
        public DateTime? ReceivedDate
        {
            get { return _ReceivedDate; }
            set { _ReceivedDate = value; }
        }
        [DataMember]
        public DateTime? InvoiceDate
        {
            get { return _InvoiceDate; }
            set { _InvoiceDate = value; }
        }
        [DataMember]
        public Int64 ITReceivingDetailsID
        {
            get { return _ITReceivingDetailsID; }
            set { _ITReceivingDetailsID = value; }
        }
        [DataMember]
        public Int64 EquimentSubCategory
        {
            get { return _EquimentSubCategory; }
            set { _EquimentSubCategory = value; }
        }
        [DataMember]
        public Int64 EquipmentList
        {
            get { return _EquipmentList; }
            set { _EquipmentList = value; }
        }
        [DataMember]
        public decimal PricePerQty
        {
            get { return _PricePerQty; }
            set { _PricePerQty = value; }
        }
        [DataMember]
        public Int32 OrderQty
        {
            get { return _OrderQty; }
            set { _OrderQty = value; }
        }
        [DataMember]
        public Int32 BalacedQty
        {
            get { return _BalacedQty; }
            set { _BalacedQty = value; }
        }
        [DataMember]
        public Int32 ReceivedQty
        {
            get { return _ReceivedQty; }
            set { _ReceivedQty = value; }
        }
        [DataMember]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set { _TotalCost = value; }
        }
        [DataMember]
        public Int64 ITReceivingMasterID
        {
            get { return _ITReceivingMasterID; }
            set { _ITReceivingMasterID = value; }
        }
        [DataMember]
        public string PackingSlipNo
        {
            get { return _PackingSlipNo; }
            set { _PackingSlipNo = value; }
        }
        [DataMember]
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set { _InvoiceNo = value; }
        }
        [DataMember]
        public string Receivingaction
        {
            get { return _Receivingaction; }
            set { _Receivingaction = value; }
        }
        [DataMember]
        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; }
        }
        [DataMember]
        public string OtherReason
        {
            get { return _OtherReason; }
            set { _OtherReason = value; }
        }
        [DataMember]
        public string InvoiceStatus
        {
            get { return _InvoiceStatus; }
            set { _InvoiceStatus = value; }
        }
        [DataMember]
        public Int64 InvoicedBy
        {
            get { return _InvoicedBy; }
            set { _InvoicedBy = value; }
        }
        [DataMember]
        public Int64 PartialBy
        {
            get { return _PartialBy; }
            set { _PartialBy = value; }
        }
        [DataMember]
        public DateTime? PartialOn
        {
            get { return _PartialOn; }
            set { _PartialOn = value; }
        }
        [DataMember]
        public Int64 VoidBy
        {
            get { return _VoidBy; }
            set { _VoidBy = value; }
        }
        [DataMember]
        public DateTime? VoidOn
        {
            get { return _VoidOn; }
            set { _VoidOn = value; }
        }
        [DataMember]
        public Int64 ClosedBy
        {
            get { return _ClosedBy; }
            set { _ClosedBy = value; }
        }
        [DataMember]
        public DateTime? ClosedOn
        {
            get { return _ClosedOn; }
            set { _ClosedOn = value; }
        }
        [DataMember]
        public string FinalStatus
        {
            get { return _FinalStatus; }
            set { _FinalStatus = value; }
        }
        [DataMember]
        public string SerialNo
        {
            get { return _SerialNo; }
            set { _SerialNo = value; }
        }
        [DataMember]
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        [DataMember]
        public string Shippingcost
        {
            get { return _Shippingcost; }
            set { _Shippingcost = value; }
        }
        [DataMember]
        public string Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
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
        public Int64 InsertRecordID
        {
            get { return _InsertRecordID; }
            set { _InsertRecordID = value; }
        }
    }
}