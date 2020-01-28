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
'' Name      : BALFacilitySupply
'' Type      :   C# File
'' Description  :<< Get / Set Business Facility Supply Map >>
'' Modification History :
''------------------------------------------------------------------------------
'' Date              Version                By                              Reason
'' ----              -------                ---								------
'' 	08/08/2017		   V1.0				   Vivekanand.S		                  New
 ''--------------------------------------------------------------------------------
'*/
#endregion
namespace Inventroy.Service.BAL
{

    public class BALFacilitySupply
    {

        private Int64 _FacilitySupplyID = 0;
        private Int64 _CorporateID = 0;
        private Int64 _FacilityID = 0;
        private Int64 _VendorID = 0;
        private Int64 _ItemCategory = 0;
        private Int64 _ItemID = 0;
        private Int64 _Census = 0;
        private bool _IsEmp = false;
        private bool _IsPatient = false;
        private decimal _Factor = 0;
        private DateTime? _VendorOrderDate;
        private Int64 _Parlevel = 0;
        private bool _IsActive = false;
        private Int64 _CreatedBy = 0;
        private Int64 _LastModifiedBy = 0;
        private string _BundleUpdate = string.Empty;

        private string _ListVendorID = string.Empty;
        private string _ListItemCategory = string.Empty;

        private bool _Isboth = false;
        private bool _Isothers = false;

        [DataMember]
        public string BundleUpdate
        {
            get { return _BundleUpdate; }
            set { _BundleUpdate = value; }
        }

        [DataMember]
        public Int64 FacilitySupplyID
        {
            get { return _FacilitySupplyID; }
            set { _FacilitySupplyID = value; }
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
        public Int64 ItemCategory
        {
            get { return _ItemCategory; }
            set { _ItemCategory = value; }
        }        
        [DataMember]
        public Int64 ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }
        [DataMember]
        public Int64 Census
        {
            get { return _Census; }
            set { _Census = value; }
        }        
        [DataMember]
        public bool IsEmp
        {
            get { return _IsEmp; }
            set { _IsEmp = value; }
        }
        [DataMember]
        public bool IsPatient
        {
            get { return _IsPatient; }
            set { _IsPatient = value; }
        }
        [DataMember]
        public decimal Factor
        {
            get { return _Factor; }
            set { _Factor = value; }
        }
        [DataMember]
        public DateTime? VendorOrderDate
        {
            get { return _VendorOrderDate; }
            set { _VendorOrderDate = value; }
        }
        [DataMember]
        public Int64 Parlevel
        {
            get { return _Parlevel; }
            set { _Parlevel = value; }
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
        public string ListVendorID
        {
            get { return _ListVendorID; }
            set { _ListVendorID = value; }
        }

        [DataMember]
        public string ListItemCategory
        {
            get { return _ListItemCategory; }
            set { _ListItemCategory = value; }
        }

        [DataMember]
        public bool Isboth
        {
            get { return _Isboth; }
            set { _Isboth = value; }
        }
        [DataMember]
        public bool Isothers
        {
            get { return _Isothers; }
            set { _Isothers = value; }
        }

    }
}