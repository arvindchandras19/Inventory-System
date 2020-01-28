using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    public class BALCorporate
    {
        private Int64 _CorporateID = 0;
        private string _CorporateDescription = string.Empty;
        private string _CorporateName = string.Empty;
        private string _POEmail = string.Empty;
        private string _SearchCorporate = string.Empty;
        [DataMember]
        public Int64 CreatedBy { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public Int64 LastModifiededBy { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public DateTime? LastModifiededOn { get; set; }
        [DataMember]
        public Int64 CorporateID
        {
            get { return _CorporateID; }
            set { _CorporateID = value; }
        }
        [DataMember]
        public string CorporateDescription
        {
            get { return _CorporateDescription; }
            set { _CorporateDescription = value; }
        }
        [DataMember]
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
        }
        [DataMember]
        public string POEmail
        {
            get { return _POEmail; }
            set { _POEmail = value; }
        }
        [DataMember]
        public string SearchCorporate
        {
            get { return _SearchCorporate; }
            set { _SearchCorporate = value; }
        }
        [DataMember]
        public bool IsActive { get; set; }



        // Search 
        [DataMember]
        public string SearchText { get; set; }

        [DataMember]
        public string Active { get; set; }
        
        [DataMember]
        public string Mode { get; set; }


    }
}