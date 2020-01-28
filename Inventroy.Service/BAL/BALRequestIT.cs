using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALRequestIT
    {
        #region private Declaration

        private Int64 _RequestITDetailID = 0;
        private Int64 _RequestITMasterID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int64 _EquipmentCategoryID = 0;
        private Int64 _EquipementListID = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private Int64 _ShippingID = 0;
        private decimal _PricePerQty = 0;
        private Int32 _Qty = 0;
        private decimal _TotalPrice = 0;
        private string _ITRNo = string.Empty;
        private string _CorporateName = string.Empty;
        private string _FacilityDescription = string.Empty;
        private string _VendorDescription = string.Empty;
        private string _EquipmentCategory = string.Empty;
        private string _EquipementList = string.Empty;
        private string _SINo = string.Empty;
        private string _ItemDescription = string.Empty;
        private string _SerialNo = string.Empty;
        private string _Shipping = string.Empty;
        private string _Remarks = string.Empty;
        private bool _IsActive = false;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private string _ShippingCost = string.Empty;
        private string _Tax = string.Empty;
        private decimal _TotalCost = 0;
        private Nullable<bool> _RequestType = null;
        private string _User = string.Empty;
        private string _Audittrial = string.Empty;
        private string _Status = string.Empty;
        private string _Reason = string.Empty;
        private string _FacilityName = string.Empty;
        private string _VendorName = string.Empty;
        private Int64 _loggedinBy = 0;


        // To Get the Lock Error Message 
        public List<GetMPRDetailsbyMPRMasterID> MPDetailsList { get; set; }
        public string ErrorMsg { get; set; }

        #endregion

        #region public Declaration

        [DataMember]
        public Int64 RequestITDetailID
        {
            get { return _RequestITDetailID; }
            set { _RequestITDetailID = value; }
        }
        [DataMember]
        public Int64 RequestITMasterID
        {
            get { return _RequestITMasterID; }
            set { _RequestITMasterID = value; }
        }
        [DataMember]
        public string FacilityName
        {
            get { return _FacilityName; }
            set { _FacilityName = value; }
        }
        [DataMember]
        public string VendorName
        {
            get { return _VendorName; }
            set { _VendorName = value; }
        }
        [DataMember]
        public Int64 loggedinBy
        {
            get { return _loggedinBy; }
            set { _loggedinBy = value; }
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
        public string SINo
        {
            get { return _SINo; }
            set { _SINo = value; }
        }
        [DataMember]
        public Int64 EquipmentCategoryID
        {
            get { return _EquipmentCategoryID; }
            set { _EquipmentCategoryID = value; }
        }
        [DataMember]
        public Int64 EquipementListID
        {
            get { return _EquipementListID; }
            set { _EquipementListID = value; }
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
        public Int64 ShippingID
        {
            get { return _ShippingID; }
            set { _ShippingID = value; }
        }
        public decimal PricePerQty
        {
            get { return _PricePerQty; }
            set { _PricePerQty = value; }
        }
        public Int32 Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; }
        }
        
        [DataMember]
        public string Shipping
        {
            get { return _Shipping; }
            set { _Shipping = value; }
        }
        [DataMember]
        public string ITRNo
        {
            get { return _ITRNo; }
            set { _ITRNo = value; }
        }
        [DataMember]
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
        }
        [DataMember]
        public string ItemDescription
        {
            get { return _ItemDescription; }
            set { _ItemDescription = value; }
        }
        [DataMember]
        public string FacilityDescription
        {
            get { return _FacilityDescription; }
            set { _FacilityDescription = value; }
        }
        [DataMember]
        public string VendorDescription
        {
            get { return _VendorDescription; }
            set { _VendorDescription = value; }
        }
        [DataMember]
        public string EquipmentCategory
        {
            get { return _EquipmentCategory; }
            set { _EquipmentCategory = value; }
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
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        [DataMember]
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
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
        public Nullable<bool> RequestType
        {
            get { return _RequestType; }
            set { _RequestType = value; }
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
        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; } 
        }
        #endregion
    }
}