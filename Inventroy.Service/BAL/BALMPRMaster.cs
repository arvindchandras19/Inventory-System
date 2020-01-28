#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
#endregion

#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      : BALMPRMaster
'' Type      :   C# File
'' Description  :<< Get / Set Business Machine Purchase Request Master >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	09/06/2017		   V2.0				   Vivekanand.S		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventroy.Service.BAL
{
    public class BALMPRMaster
    {

        #region private Declaration

        private Int64 _MPRDetailID = 0;
        private Int64 _MPRMasterID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int64 _EquipmentCategoryID = 0;
        private Int64 _EquipementListID = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private Int64 _ShippingID = 0;
        private decimal _PricePerUnit = 0;
        private Int32 _OrderQuantity = 0;
        private decimal _TotalPrice = 0;
        private string _MPRNo = string.Empty;
        private string _UOM = string.Empty;
        private string _Hoursonthemachine = string.Empty;
        private Int64 _StatusID = 0;
        private string _CorporateName = string.Empty;
        private string _FacilityDescription = string.Empty;
        private string _VendorDescription = string.Empty;
        private string _EquipmentCategory = string.Empty;
        private string _EquipementList = string.Empty;
        private string _ItemID = string.Empty;
        private string _ItemDescription = string.Empty;
        private string _SerialNo = string.Empty;
        private string _Shipping = string.Empty;
        private string _Status = string.Empty;
        private bool _IsActive = false;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private string _ShippingCost = string.Empty;
        private string _Tax = string.Empty;
        private decimal _TotalCost = 0;
        private string _FacilityName = string.Empty;
        private string _VendorName = string.Empty;
        private Int64 _loggedinBy = 0;


        // To Get the Lock Error Message 
        public List<GetMPRDetailsbyMPRMasterID> MPDetailsList { get; set; }
        public string ErrorMsg { get; set; }


        [DataMember]
        public long EquipementSubCategoryID { get; set; }

        [DataMember]
        public string EquipementSubCategorydesc { get; set; }


        #endregion

        #region public Declaration

        [DataMember]
        public Int64 MPRDetailID
        {
            get { return _MPRDetailID; }
            set { _MPRDetailID = value; }
        }
        [DataMember]
        public Int64 MPRMasterID
        {
            get { return _MPRMasterID; }
            set { _MPRMasterID = value; }
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
        public string ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
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
        public decimal PricePerUnit
        {
            get { return _PricePerUnit; }
            set { _PricePerUnit = value; }
        }
        public Int32 OrderQuantity
        {
            get { return _OrderQuantity; }
            set { _OrderQuantity = value; }
        }
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; }
        }
        [DataMember]
        public string Hoursonthemachine
        {
            get { return _Hoursonthemachine; }
            set { _Hoursonthemachine = value; }
        }
        [DataMember]
        public Int64 StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        [DataMember]
        public string Shipping
        {
            get { return _Shipping; }
            set { _Shipping = value; }
        }
        [DataMember]
        public string MPRNo
        {
            get { return _MPRNo; }
            set { _MPRNo = value; }
        }
        [DataMember]
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
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
        public string UOM
        {
            get { return _UOM; }
            set { _UOM = value; }
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
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
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
        public Int64 loggedinBy
        {
            get { return _loggedinBy; }
            set { _loggedinBy = value; }
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

        #endregion

    }
}
