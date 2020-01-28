using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALCapitalPO
    {
        #region private Declaration
        private Int64 _CapitalItemMasterID = 0;
        private Int64 _CapitalOrderID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int32 _SortOrder = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private string _Audittrial = string.Empty;
        private string _Status = string.Empty;
        private string _CPONo = string.Empty;
        private string _Remarks = string.Empty;
        private byte[] _OrderContent;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private decimal _TotalCost = 0;
        private string _ListCorporateID = string.Empty;
        private string _ListFacilityID = string.Empty;
        private string _ListVendorID = string.Empty;
        private Int64 _LoggedinBy = 0;

        #endregion

        [DataMember]
        public string ListCorporateID
        {
            get { return _ListCorporateID; }
            set { _ListCorporateID = value; }
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
        public Int64 CapitalItemMasterID
        {
            get { return _CapitalItemMasterID; }
            set { _CapitalItemMasterID = value; }
        }
        [DataMember]
        public Int64 CapitalOrderID
        {
            get { return _CapitalOrderID; }
            set { _CapitalOrderID = value; }
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
        public string CPONo
        {
            get { return _CPONo; }
            set { _CPONo = value; }
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
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public Int64 LoggedinBy { get; set; }

        [DataMember]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set { _TotalCost = value; }
        }
    }
}