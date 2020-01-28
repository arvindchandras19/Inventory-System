using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Inventroy.Service.BAL
{
    [DataContract]
    public class BALTransferIn
    {
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long LastModifiedBy { get; set; }
        [DataMember]
        public long DeletedBy { get; set; }
        [DataMember]
        public long LoggedinBy { get; set; }
        [DataMember]
        public string LockTimeOut { get; set; }
        [DataMember]
        public string InvenValue { get; set; }
        [DataMember]
        public string Updatekeyvalue { get; set; }
        [DataMember]
        public long CorporateID { get; set; }
        [DataMember]
        public long FacilityID { get; set; }
        [DataMember]
        public string ListCorporateID { get; set; }
        [DataMember]
        public string ListFacilityID { get; set; }
        [DataMember]
        public string ListCategoryID { get; set; }       
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }

        [DataMember]
        public Int64 TransferINID { get; set; }
        [DataMember]
        public Int64 TransferOutID { get; set; }
        [DataMember]
        public string TransferNo { get; set; }
        [DataMember]
        public DateTime? TransferInDate { get; set; }
        [DataMember]
        public DateTime? TransferOutDate { get; set; }
        [DataMember]
        public long CorporateIDfrom { get; set; }
        [DataMember]
        public long CorporateIDTo { get; set; }
        [DataMember]
        public long FacilityIDFrom { get; set; }
        [DataMember]
        public long FacilityIDTo { get; set; }
        [DataMember]
        public string CorporateFromName { get; set; }
        [DataMember]
        public string FacilityFromName { get; set; }
        [DataMember]
        public string CorporateToName { get; set; }
        [DataMember]
        public string FacilityToName { get; set; }       
        [DataMember]
        public long CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public Int64 ItemID { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public Int64 QtyPack { get; set; }
        [DataMember]
        public Int64 UOMID { get; set; }
        [DataMember]
        public string UOM { get; set; }
        [DataMember]
        public Decimal Price { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public Int64 Transferqty { get; set; }
        [DataMember]
        public Boolean IsActive { get; set; }        
    }
}