using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventroy.Service;
using System.Runtime.Serialization;

namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALUser
    {
        private Int64 _UserID = 0;
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private string _UserName = string.Empty;
        private string _Password = string.Empty;
        private String _ConfirmPassword = string.Empty;
        private string _Email = string.Empty;
        private string _Phone = string.Empty;
        private DateTime? _DeletedDate;
        private bool _IsActive = false;
        private DateTime _CreatedOn;
        private Int64 _CreatedBy = 0;
        private bool? _IsDeleted = false;
        private Int64 _DeletedBy = 0;
        private DateTime? _LastLoginDate;
        private Int64 _UserRoleID = 0;
        private string _UserRole = string.Empty;
        private Int64 _FacilityID = 0;
        private String _FacilityName = string.Empty;
        private Int64 _RoleUserID = 0;
        private string _RoleName = string.Empty;
        private Int64 _CorporateID = 0;
        private Int64 _RegionID = 0;
        private String _CorporateName = string.Empty;
        private String _RegionName = string.Empty;
        private String _BudgetCurrency = string.Empty;
        private Int64 _BudgetValue = 0;
        private bool _IsEdit = false;
        private bool _IsView = false;
        private bool _IsEmailNotification = false;
        private Int64 _MenuID = 0;
        private Int64 _SubMenuId = 0;
        private String _PageName = string.Empty;
        private Int64 _PermissionID = 0;
        private Int64 _Xtn = 0;



        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }

        [DataMember]
        public string ListRoleID { get; set; }

        [DataMember]
        public string Active { get; set; }
        [DataMember]
        public Int64 LoggedinBy { get; set; }
        [DataMember]
        public string Filter { get; set; }


        [DataMember]
        public Int64 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        [DataMember]
        public Int64 RoleUserID
        {
            get { return _RoleUserID; }
            set { _RoleUserID = value; }
        }
        [DataMember]
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        [DataMember]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        [DataMember]
        public String ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { _ConfirmPassword = value; }
        }
        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        [DataMember]
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        [DataMember]
        public Int64 Xtn
        {
            get { return _Xtn; }
            set { _Xtn = value; }
        }
        [DataMember]
        public DateTime? LastLoginDate
        {
            get { return _LastLoginDate; }
            set { _LastLoginDate = value; }
        }
        [DataMember]
        public DateTime? DeletedDate
        {
            get { return _DeletedDate; }
            set { _DeletedDate = value; }
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
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        [DataMember]
        public Int64 DeletedBy
        {
            get { return _DeletedBy; }
            set { _DeletedBy = value; }
        }
        [DataMember]
        public bool? IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        [DataMember]
        public DateTime? DeletedOn { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public Int64 UserRoleID
        {
            get { return _UserRoleID; }
            set { _UserRoleID = value; }
        }
        [DataMember]
        public string UserRole
        {
            get { return _UserRole; }
            set { _UserRole = value; }
        }
        [DataMember]
        public Int64 FacilityID
        {
            get { return _FacilityID; }
            set { _FacilityID = value; }
        }
        [DataMember]
        public string FacilityName
        {
            get { return _FacilityName; }
            set { _FacilityName = value; }
        }
        [DataMember]
        public Int64 CorporateID
        {
            get { return _CorporateID; }
            set { _CorporateID = value; }
        }
        [DataMember]
        public Int64 RegionID
        {
            get { return _RegionID; }
            set { _RegionID = value; }
        }
        [DataMember]
        public string CorporateName
        {
            get { return _CorporateName; }
            set { _CorporateName = value; }
        }
        [DataMember]
        public string RegionName
        {
            get { return _RegionName; }
            set { _RegionName = value; }
        }
        [DataMember]
        public string BudgetCurrency
        {
            get { return _BudgetCurrency; }
            set { _BudgetCurrency = value; }
        }
        [DataMember]
        public Int64 BudgetValue
        {
            get { return _BudgetValue; }
            set { _BudgetValue = value; }
        }
        [DataMember]
        public bool IsEdit
        {
            get { return _IsEdit; }
            set { _IsEdit = value; }
        }
        [DataMember]
        public bool IsView
        {
            get { return _IsView; }
            set { _IsView = value; }
        }
        [DataMember]
        public bool IsEmailNotification
        {
            get { return _IsEmailNotification; }
            set { _IsEmailNotification = value; }
        }
        [DataMember]
        public Int64 MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }
        [DataMember]
        public Int64 SubMenuId
        {
            get { return _SubMenuId; }
            set { _SubMenuId = value; }
        }
        [DataMember]
        public String PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }
        [DataMember]
        public Int64 PermissionID
        {
            get { return _PermissionID; }
            set { _PermissionID = value; }
        }



        // User Approve Permission
                

        [DataMember]
        public bool IsApprove { get; set; }
        [DataMember]
        public bool IsDeny { get; set; }
        [DataMember]
        public bool IsOrder { get; set; }
        [DataMember]
        public decimal ApproveRangeFrom { get; set; }
        [DataMember]
        public decimal ApproveRangeTo { get; set; }
        [DataMember]
        public string MultipleRoles { get; set; }
        [DataMember]
        public Int32 Approveorder { get; set; }
        [DataMember]
        public Int64 PageMasterPermissionMultiRoleID { get; set; }
    }
}