using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;

namespace Inventory.Class
{
    public class EmailController
    {
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public string CorporateEmail { get; set; }
        [DataMember]
        public string vendorContactEmail { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        [DataMember]
        public string UserPhoneNo { get; set; }
        [DataMember]
        public string vendorEmailcontent { get; set; }
        [DataMember]
        public string vendoremailsubject { get; set; }
        [DataMember]
        public string SPONo { get; set; }
        [DataMember]
        public byte[] Mbytes { get; set; }
        [DataMember]
        public string VoidRemarks { get; set; }


        //#region SEND EMAIL BY USING C# OFFICE 365

        //public void sendEmail()
        //{
        //    String userName = "from@domain.com";
        //    String password = "password for from address";
        //    MailMessage msg = new MailMessage("from@domain.com ", " to@domain.com ");
        //    msg.Subject = "Your Subject Name";
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("Name: S.vivekanand");
        //    sb.AppendLine("Mobile Number: 9600020247");
        //    sb.AppendLine("Email: svivek@gmail.com" );
        //    sb.AppendLine("Drop Downlist Name: Sample");
        //    msg.Body = sb.ToString();
        //    //Attachment attach = new Attachment(Microsoft.SqlServer.Server.MapPath("folder/" + ImgName));
        //    //msg.Attachments.Add(attach);
        //    SmtpClient SmtpClient = new SmtpClient();
        //    SmtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
        //    SmtpClient.Host = "smtp.office365.com";
        //    SmtpClient.Port = 587;
        //    SmtpClient.EnableSsl = true;
        //    SmtpClient.Send(objMailMessage);
        //}

        //#endregion





        public void SendEmail(String Emailid, String Content, String subject, String AttachmentPath)
        {
            try
            {
                string senderID = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();
                MailMessage ms = new MailMessage();
                ms.To.Add(Emailid);
                ms.From = new MailAddress(Emailid);
                ms.Subject = subject;
                ms.Body = Content;
                ms.IsBodyHtml = true;
                ms.Attachments.Add(new Attachment(AttachmentPath));
                SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                smtp.Host = SMTPServer;
                smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = System.Convert.ToInt32(SMTPPort);
                smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                smtp.Send(ms);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        public void SendEmailTransferOut(String FromEmailid, String ToEmailid, String Content, String subject)
        {
            try
            {
                string senderID = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                MailMessage ms = new MailMessage();
                bool IsValidMailID = false;
                string[] values = ToEmailid.Split(',');
                foreach (string mailid in values)
                {
                    if (mailid != "")
                    {
                        ms.To.Add(new MailAddress(mailid));
                        IsValidMailID = true;
                    }
                }
                if (IsValidMailID)
                {
                    ms.From = new MailAddress(FromEmailid);
                    ms.Subject = subject;
                    ms.Body = Content;
                    ms.IsBodyHtml = true;   
                    SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                    smtp.Host = SMTPServer;
                    smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = System.Convert.ToInt32(SMTPPort);
                    smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                    smtp.Send(ms);
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public void SendEmailPDFContent(String FromEmailid,String ToEmailid, String Content, String subject, System.IO.MemoryStream Attachcontent, string displayfilename)
        {
            try
            {
                string senderID = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(Attachcontent, ct);               
                attach.Name = displayfilename;
                MailMessage ms = new MailMessage();
                bool IsValidMailID = false;
                string[] values = ToEmailid.Split(',');
                foreach (string mailid in values)
                {
                    if (mailid != "")
                    {
                        ms.To.Add(new MailAddress(mailid));
                        IsValidMailID = true;
                    }
                }
                if (IsValidMailID)
                {
                    ms.From = new MailAddress(FromEmailid);
                    ms.Subject = subject;
                    ms.Body = Content;
                    ms.IsBodyHtml = true;
                    ms.Attachments.Add(new Attachment(Attachcontent, ct));
                    SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                    smtp.Host = SMTPServer;
                    smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = System.Convert.ToInt32(SMTPPort);
                    smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                    smtp.Send(ms);
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public void SendEmailWithPDFContent(String Emailid, String Content, String subject, System.IO.MemoryStream Attachcontent,string displayfilename)
        {
            try
            {
                string senderID = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();
                
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(Attachcontent, ct);
                attach.Name = displayfilename;

                MailMessage ms = new MailMessage();
                ms.To.Add(Emailid);
                ms.From = new MailAddress(senderID);
                ms.Subject = subject;
                ms.Body = Content;
                ms.IsBodyHtml = true;
                ms.Attachments.Add(new Attachment(Attachcontent,ct));
                SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                smtp.Host = SMTPServer;
                smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = System.Convert.ToInt32(SMTPPort);
                smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                smtp.Send(ms);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public void SendEmailWithTwoPDFContent(String FromEmailid, String Emailid, String Content, String subject, System.IO.MemoryStream Attachcontent, string displayfilename, System.IO.MemoryStream Attachcontent2, string displayfilename2)
        {
            try
            {
                string senderID = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();

                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(Attachcontent, ct);
                System.Net.Mail.Attachment attach2 = new System.Net.Mail.Attachment(Attachcontent2, ct);
                attach.Name = displayfilename;
                attach2.Name = displayfilename2;
                MailMessage ms = new MailMessage();
                bool IsValidMailID = false;
                string[] values = Emailid.Split(',');
                foreach (string mailid in values)
                {
                    if (mailid != "")
                    {
                        ms.To.Add(new MailAddress(mailid));
                        IsValidMailID = true;
                    }
                }
                if (IsValidMailID)
                {
                    ms.From = new MailAddress(FromEmailid);
                    ms.Subject = subject;
                    ms.Body = Content;
                    ms.IsBodyHtml = true;
                    ms.Attachments.Add(new Attachment(Attachcontent, ct));
                    ms.Attachments.Add(new Attachment(Attachcontent2, ct));
                    SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                    smtp.Host = SMTPServer;
                    smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = System.Convert.ToInt32(SMTPPort);
                    smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                    smtp.Send(ms);
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public void Emailsent(String FromEmail, String ToEmail, String Content, String subject, String AttachmentPath)
        {
            try
            {
                //string FromEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPSenderPwd"].ToString();
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"].ToString();
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString();
                MailMessage ms = new MailMessage();
                ms.To.Add(ToEmail);
                ms.From = new MailAddress(FromEmail);
                ms.Subject = subject;
                ms.Body = Content;
                ms.IsBodyHtml = true;
                ms.Attachments.Add(new Attachment(AttachmentPath));
                SmtpClient smtp = new SmtpClient(SMTPServer, 25);
                smtp.Host = SMTPServer;
                smtp.Credentials = new System.Net.NetworkCredential(FromEmail, senderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = System.Convert.ToInt32(SMTPPort);
                smtp.EnableSsl = System.Convert.ToBoolean(SMTPSSL);
                smtp.Send(ms);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
    }
}