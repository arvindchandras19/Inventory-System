using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    public class BALActivityTracking
    {
        #region private Declaration

        private Int64 _UserID = 0;
        private Int64 _MainMenuID = 0;
        private Int64 _SubMenuID = 0;
        private string _AppFeature = string.Empty;
        private Int64 _FacilityID = 0;
        private DateTime? _TimeIn;
        private DateTime? _TimeOut;
        private Int64 _DurationMinutes = 0;
        private DateTime? _DateLoggedIn;
        private DateTime? _DateLoggedOut;       
        private string _MachineID = string.Empty;
        private string _IPAddress = string.Empty;


        #endregion

        #region public Declaration

        [DataMember]
        public string AppFeature
        {
            get { return _AppFeature; }
            set { _AppFeature = value; }
        }
        [DataMember]
        public string MachineID
        {
            get { return _MachineID; }
            set { _MachineID = value; }
        }
        [DataMember]
        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }
        [DataMember]
        public Int64 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public Int64 MainMenuID
        {
            get { return _MainMenuID; }
            set { _MainMenuID = value; }
        }
        [DataMember]
        public Int64 SubMenuID
        {
            get { return _SubMenuID; }
            set { _SubMenuID = value; }
        }
        [DataMember]
        public Int64 FacilityID
        {
            get { return _FacilityID; }
            set { _FacilityID = value; }
        }
        
        [DataMember]
        public Int64 DurationMinutes
        {
            get { return _DurationMinutes; }
            set { _DurationMinutes = value; }
        }
        [DataMember]
        public DateTime? TimeIn
        {
            get { return _TimeIn; }
            set { _TimeIn = value; }
        }
        [DataMember]
        public DateTime? TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        [DataMember]
        public DateTime? DateLoggedIn
        {
            get { return _DateLoggedIn; }
            set { _DateLoggedIn = value; }
        }
        public DateTime? DateLoggedOut
        {
            get { return _DateLoggedOut; }
            set { _DateLoggedOut = value; }
        }
        
        #endregion

    }

}
