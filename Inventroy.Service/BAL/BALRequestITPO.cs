using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALRequestITPO
    {

        #region private Declaration 
        private Int64 _RequestITMasterID = 0;
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
        private string _ITRNo = string.Empty;
        private string _ITNNo = string.Empty;
        private string _Remarks = string.Empty;
        private byte[] _OrderContent;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private Int64 _LoggedinBy = 0;
        private DateTime? _OrderDate;
        private decimal _TotalPrice;
        private string _CorporateIDs = string.Empty;
        private string _FacilityIDs = string.Empty;
        private string _VendorIDs = string.Empty;
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
            get { return _ITRNo; }
            set { _ITRNo = value; }
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
    }
}