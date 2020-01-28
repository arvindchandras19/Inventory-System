#region Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
#endregion
#region DocumentHistory
/*
'****************************************************************************
'*
'' Itrope Technologies All rights reserved.
'' Copyright (C) 2017. Itrope Technologies
'' Name      : BALServiceRequest
'' Type      :   C# File
'' Description  :<< Get / Set Business Service Request >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	09/26/2017		   V2.0				   Sairam.P.		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion

namespace Inventroy.Service.BAL
{
    public class BALServiceRequest
    {

        #region private Declaration

        // Private Service Request

        private Int64 _ServiceRequestDetailID = 0;
        private Int64 _ServiceRequestMasterID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int64 _ServiceCategoryID = 0;
        private Int64 _ServiceListID = 0;
        private DateTime? _DateFrom;
        private DateTime? _DateTo;
        private string _Service = string.Empty;
        private decimal _Price = 0;
        private string _Unit = string.Empty;
        private string _SRNo = string.Empty;
        private string _Quotes = string.Empty;
        private Int64 _StatusID = 0;
        private string _CorporateName = string.Empty;
        private string _FacilityDescription = string.Empty;
        private string _VendorDescription = string.Empty;
        private string _ServiceCategory = string.Empty;
        private string _SeriviceList = string.Empty;
        private string _ServiceCatDesc = string.Empty;
        private string _ServiceListDesc = string.Empty;
        private Int32 _SNo = 0;
        private string _Remarks;
        private string _Status = string.Empty;
        private bool _IsActive = false;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private string _FacilityName = string.Empty;
        private string _VendorName = string.Empty;
        //private Int64 _loggedinBy = 0;

        // New Fields
        private Int64 _EquipmentCategoryID = 0;
        private Int64 _EquipmentListID = 0;
        private bool _ServiceType = false;



        //Private Service Attachment

        private Int64 _ServiceUploadID = 0;
        private string _LocationOfTheFile = string.Empty;
        private Int64 _UploadedBy = 0;
        private DateTime? _UploadedDateTime;
        private string _Description = string.Empty;
        private string _FileName = string.Empty;

        // To Get the Lock Error Message 
        [DataMember]
        public List<GetServiceRequestetailsbyServiceRequestMasterID> SRDetailsList { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }


        [DataMember]
        public long EquipementSubCategoryID { get; set; }

        [DataMember]
        public string EquipementSubCategorydesc { get; set; }

        [DataMember]
        public bool IsReceive { get; set; }
        [DataMember]
        public string InvoiceNo { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public string InvoiceStatus { get; set; }
        [DataMember]
        public string InvoiceRemarks { get; set; }
        [DataMember]
        public string OtherRemarks { get; set; }
         



        // Service Request Purchase Order

        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string PORemarks { get; set; }
        [DataMember]
        public DateTime ApproveDate { get; set; }

        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListStatus { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }

        [DataMember]
        public byte[] OrderContent { get; set; }




        #endregion

        #region public Declaration

        // Public Service Attachment

        [DataMember]
        public Int64 ServiceUploadID
        {
            get { return _ServiceUploadID; }
            set { _ServiceUploadID = value; }
        }
        [DataMember]
        public string LocationOfTheFile
        {
            get { return _LocationOfTheFile; }
            set { _LocationOfTheFile = value; }
        }
        [DataMember]
        public Int64 UploadedBy
        {
            get { return _UploadedBy; }
            set { _UploadedBy = value; }
        }

        [DataMember]
        public string FacilityName
        {
            get { return _FacilityName; }
            set { _FacilityName = value; }
        }
        //[DataMember]
        //public Int64 loggedinBy
        //{
        //    get { return _loggedinBy; }
        //    set { _loggedinBy = value; }
        //}

        [DataMember]
        public DateTime? UploadedDateTime
        {
            get { return _UploadedDateTime; }
            set { _UploadedDateTime = value; }
        }
        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        [DataMember]
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }


        // New Fields

        [DataMember]
        public Int64 EquipmentCategoryID
        {
            get { return _EquipmentCategoryID; }
            set { _EquipmentCategoryID = value; }
        }
        [DataMember]
        public Int64 EquipmentListID
        {
            get { return _EquipmentListID; }
            set { _EquipmentListID = value; }
        }
        [DataMember]
        public bool ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }


        // Public Service Request

        [DataMember]
        public Int64 ServiceRequestDetailID
        {
            get { return _ServiceRequestDetailID; }
            set { _ServiceRequestDetailID = value; }
        }
        [DataMember]
        public Int64 ServiceRequestMasterID
        {
            get { return _ServiceRequestMasterID; }
            set { _ServiceRequestMasterID = value; }
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
        public Int64 ServiceCategoryID
        {
            get { return _ServiceCategoryID; }
            set { _ServiceCategoryID = value; }
        }
        [DataMember]
        public Int64 ServiceListID
        {
            get { return _ServiceListID; }
            set { _ServiceListID = value; }
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
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        [DataMember]
        public string Service
        {
            get { return _Service; }
            set { _Service = value; }
        }
        [DataMember]
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        [DataMember]
        public Int64 StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        [DataMember]
        public string SRNo
        {
            get { return _SRNo; }
            set { _SRNo = value; }
        }
        [DataMember]
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
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
        public string ServiceCategory
        {
            get { return _ServiceCategory; }
            set { _ServiceCategory = value; }
        }
        [DataMember]
        public string ServiceList
        {
            get { return _SeriviceList; }
            set { _SeriviceList = value; }
        }
        [DataMember]
        public string ServiceCatDesc
        {
            get { return _ServiceCatDesc; }
            set { _ServiceCatDesc = value; }
        }
        [DataMember]
        public string ServiceListDesc
        {
            get { return _ServiceListDesc; }
            set { _ServiceListDesc = value; }
        }
        [DataMember]
        public Int32 SNo
        {
            get { return _SNo; }
            set { _SNo = value; }
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
        public Int64 LastModifiedBy
        {
            get { return _LastModifiedBy; }
            set { _LastModifiedBy = value; }
        }
        [DataMember]
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        #endregion

    }
}
