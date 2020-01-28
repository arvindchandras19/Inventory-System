using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALMedicalSuppliesRequest
    {
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long DeletedBy { get; set; }
        [DataMember]
        public string InvenValue { get; set; }
        [DataMember]
        public string Updatekeyvalue { get; set; }
        [DataMember]
        public long CorporateID { get; set; }
        [DataMember]
        public long FacilityID { get; set; }
        [DataMember]
        public long Vendor { get; set; }
        [DataMember]
        public Int32 OrderType { get; set; }
        [DataMember]
        public DateTime? OrderPeriod { get; set; }
        [DataMember]
        public string Shipping { get; set; }
        [DataMember]
        public string TimeDelivery { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public long SNGItemID { get; set; }
        [DataMember]
        public string Itemcatgroup { get; set; }
        [DataMember]
        public string Itemdescription { get; set; }
        [DataMember]
        public long UOM { get; set; }
        [DataMember]
        public Int32 QuantityPack { get; set; }
        [DataMember]
        public string Parlevel { get; set; }
        [DataMember]
        public Int32 QuantityinHand { get; set; }
        [DataMember]
        public Int32 OrderQuantity { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public long MedicalSupplyMasterID { get; set; }
        [DataMember]
        public long MedicalSupplyDetailID { get; set; }
        [DataMember]
        public string PRNo { get; set; }
        [DataMember]
        public String CombineKey { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        private string _CorporateName = string.Empty;
        private string _FacilityName = string.Empty;
        private string _VendorName = string.Empty;
        private Int64 _loggedinBy = 0;


        // To Get the Lock Error Message 
        [DataMember]
        public List<BindMedicalsupplyDetail> MSDetailsList { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }

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
        public Int64 loggedinBy
        {
            get { return _loggedinBy; }
            set { _loggedinBy = value; }
        }

    }
}