using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALFacility
    {

        private Int64 _FacilityID = 0;
        private string _FacilityDescription = string.Empty;
        private Int64 _DialysisoftFacilityCode = 0;
        private string _FacilityType = string.Empty;
        private string _Address1 = string.Empty;
        private string _City = string.Empty;
        private Int64 _State = 0;
        private string _Zipcode = string.Empty;
        private string _Phone = string.Empty;
        private string _Fax = string.Empty;
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public Int64 EmployeeCensus { get; set; }
        [DataMember]
        public Int64 LastModifiededBy { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public DateTime? LastModifiededOn { get; set; }
        [DataMember]
        public Int64 FacilityID
        {
            get { return _FacilityID; }
            set { _FacilityID = value; }
        }
        [DataMember]
        public string FacilityDescription
        {
            get { return _FacilityDescription; }
            set { _FacilityDescription = value; }
        }
       

        [DataMember]
        public string Address1
        {
            get { return _Address1; }
            set { _Address1 = value; }
        }

        [DataMember]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        [DataMember]
        public Int64 State
        {
            get { return _State; }
            set { _State = value; }
        }
        [DataMember]
        public string Zipcode
        {
            get { return _Zipcode; }
            set { _Zipcode = value; }
        }

        [DataMember]
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        [DataMember]
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        [DataMember]
        public string FacilityShortName { get; set; }
        [DataMember]
        public string BillAddress1 { get; set; }
        [DataMember]
        public string BillCity { get; set; }
        [DataMember]
        public Int64 BillState { get; set; }
        [DataMember]
        public string BillZipCode { get; set; }
        [DataMember]
        public string BillPhone{ get; set; }
        [DataMember]
        public string BillFax { get; set; }
        

        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public Int64 FCorporate { get; set; }
      
        [DataMember]
        public string GPAccountCode { get; set; }

        [DataMember]
        public string EMRCode { get; set; }
        [DataMember]
        public Int64 CopyFacilityID { get; set; }
        [DataMember]
        public string TechPerson { get; set; }
        [DataMember]
        public string TechPhone { get; set; }
        [DataMember]
        public string TechEmail { get; set; }
        [DataMember]
        public string AdminPerson { get; set; }
        [DataMember]
        public string AdminPhone { get; set; }
        [DataMember]
        public string AdminEmail { get; set; }
        [DataMember]
        public Int64 PatientCensus { get; set; }
        [DataMember]
        public Int64 TxWeek { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string BillAddress2 { get; set; }
        [DataMember]
        public Int64 Xtn { get; set; }
        [DataMember]
        public Int64 BillXtn { get; set; }


        // Search 
        [DataMember]
        public string SearchText { get; set; }

        [DataMember]
        public string Active { get; set; }

        [DataMember]
        public Int64 LogginBy { get; set; }

        [DataMember]
        public string Filter { get; set; }



    }
}