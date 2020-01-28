using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using Inventory.NotificationScheduler.Common;

namespace Inventory.NotificationScheduler
{
    class Scheduler
    {
        private string DBName = "";
        public DataSourceCommunicator GetDataBaseConnection()
        {
            try
            {
                DBServiceConfig objXmlConfig = new DBServiceConfig();
                Exception ce = new Exception();
                DBServiceConfig.LoadFromFile(ConstantStrings.ConfigXMLPath, out objXmlConfig, out ce);
                DataSourceCommunicator dbget = null;
                if (objXmlConfig != null)
                {
                    DBName = objXmlConfig.DBName;
                    dbget = new DataSourceCommunicator(DataSourceCommunicator.ParamType.ServerCredentials, objXmlConfig.DBServer, objXmlConfig.DBUser, objXmlConfig.DBPassword);
                    ce = null;
                }

                return dbget;
            }
            catch (System.Exception ex)
            {
                Program.log.Write("Scheduler - Error : " + ex.Message);
                return null;
            }

        }

        public Scheduler()
        { }
        public void Start()
        {

            try
            {
                Program.log.Write("Scheduler started ..");
                try
                {
                    ExecuteVendorOverDueRemainder();
                }
                catch (Exception ex1)
                {
                    Program.log.Write("Scheduler - Error : " + ex1.Message);
                }
                Program.log.Write("Scheduler ended ..");
            }
            catch (System.Exception ex)
            {
                Program.log.Write("Scheduler - Error : " + ex.Message);
            }

        }
        private void ExecuteVendorOverDueRemainder()
        {
            try
            {
                string corporateid = string.Empty;
                string facilityid = string.Empty;
                string corporatename = string.Empty;
                string facilityname = string.Empty;
                string adminemailaddress = string.Empty;
                string erroremailaddresss = string.Empty;
                DataSourceCommunicator db = GetDataBaseConnection();
                db.AddParameter("@LOGGEDINGBY", adminemailaddress);
                DataSet ds = db.ExecuteStoredProcedureAsDataSet("GetVendorOrderdueRemainderFacilityList", DBName);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        corporateid = dr["corporateid"].ToString();
                        corporatename = dr["corporatename"].ToString();
                        facilityid = dr["facilityid"].ToString();
                        facilityname = dr["facilitydescription"].ToString();
                        try
                        {
                            ProcessVendorOverDueByFacility(corporateid, facilityid, corporatename, facilityname, adminemailaddress, erroremailaddresss);
                        }
                        catch (Exception ce)
                        {
                            Program.log.Write("Scheduler - Error : " + ce.Message);
                        }

                    }
                }
            }
            catch (Exception ce)
            {
                Program.log.Write("Scheduler - Error start : " + ce.Message);
            }
        }

        private void ProcessVendorOverDueByFacility(string corporateid, string facilityid, string corporatename, string facilityname, string adminemailaddress, string erroremailaddresss)
        {
            try
            {
                string ErrorMsg = string.Empty;
                DataSourceCommunicator db = GetDataBaseConnection();
                db.AddParameter("@CorporateID", corporateid);
                db.AddParameter("@FacilityID", facilityid);
                db.AddParameter("@Application", "Schedular");
                db.AddParameter("@LOGGEDINGBY", 1);
                EmailController objemail = new EmailController();
                List<BALVendorOrderDue> VendorOrderDueList = new List<BALVendorOrderDue>();
                DataSet ds = db.ExecuteStoredProcedureAsDataSet("GetVendorOrderdueRemainderReport", DBName);

                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (i < 1)
                            {
                                if (dr["FromEmailID"].ToString() == null || dr["FromEmailID"].ToString() == "" || dr["ToEmailID"].ToString() == null || dr["ToEmailID"].ToString() == "")
                                {
                                    ErrorMsg = "No super admin assign to ";
                                    Program.log.Write("Scheduler - Error : " + ErrorMsg + dr["FacilityDescription"].ToString() + " facility");
                                }
                                else
                                {
                                    if (Int64.Parse(dr["VenOrderDueID"].ToString()) != 0)
                                    {
                                        objemail.vendorEmailcontent = dr["BodyContent1"].ToString() + dr["BodyContent2"].ToString() + "<div><br />Regards <br /> " + dr["BodyContent3"].ToString() + "</div>";
                                        Program.log.Write("Vendor order due email sent  to Tech Person[" + dr["ToEmailID"].ToString() + "] of this Corporate[" + corporatename + "] and Facility[" + dr["FacilityDescription"].ToString() + "]");
                                    }
                                    else
                                    {
                                        objemail.vendorEmailcontent = dr["BodyContent1"].ToString() + dr["BodyContent2"].ToString() + dr["BodyContent3"].ToString();
                                        Program.log.Write("Notification email sent  to Super Admin[" + dr["ToEmailID"].ToString() + "] to add tech role user for this Corporate[" + corporatename + "] and Facility[" + dr["FacilityDescription"].ToString() + "]");
                                    }
                                    objemail.vendoremailsubject = dr["SubjectContent"].ToString();
                                    objemail.CorporateEmail = dr["FromEmailID"].ToString();
                                    objemail.vendorContactEmail = dr["ToEmailID"].ToString();                                                                 
                                }
                                if (ErrorMsg == "")
                                {
                                    objemail.SendEmailTransferOut(objemail.CorporateEmail, objemail.vendorContactEmail, objemail.vendorEmailcontent, objemail.vendoremailsubject);
                                }

                            }
                            i++;
                        }
                    }
                    else
                    {
                        Program.log.Write("No Vendor due for this Corporate[" + corporatename + "] and Facility[" + facilityname + "] ");
                    }
                }
            }
            catch (Exception ce)
            {
                Program.log.Write("Scheduler - Error End : " + ce.Message);
            }

        }




    }
}
