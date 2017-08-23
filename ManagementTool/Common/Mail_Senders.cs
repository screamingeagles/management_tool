using System;
using System.Web;
using System.Data;
using System.Linq;
using helpdesk.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Exchange.WebServices.Data;

using System.Threading;
using System.Threading.Tasks;

namespace helpdesk.Common
{
    public class Mail_Senders
    {

        public static int                   SendTicketResolvedEmail                 (int ticketid)      
        {
            string subject = "";
            string html_message = "";

            string ticketName = "";   // user who created ticket
            string ticketEmail = "";

            string teh_mail = "";  // tecnicain_email
            string tech_name = "";  // tech name
            string man_mail = "";  // manager name
            string man_name = "";  // manager email

            try
            {
                string _key = ConfigurationManager.AppSettings["emailKey"];        // email sender key
                string _pwd = ConfigurationManager.AppSettings["emailPassword"];    // password 

                using (TicketEntities db = new TicketEntities())
                {
                    var qry = (from m in db.vw_Ticket_ResolverDetails
                               where m.TicketId == ticketid
                               select new
                               {
                                   m.TicketId,
                                   m.TSubject,
                                   m.FromAddress,
                                   m.FromName,
                                   m.ResolveEmail,
                                   m.ResolveName,
                                   m.ManagerEmail,
                                   m.ManagerName
                               }).FirstOrDefault();
                    if (qry != null)
                    {
                        ticketName = qry.FromName;
                        ticketEmail = qry.FromAddress;
                        subject = qry.TSubject;
                        teh_mail = qry.ResolveEmail;  // tecnicain_email
                        tech_name = qry.ResolveName;   // tech name
                        man_mail = qry.ManagerEmail;  // manager email
                        man_name = qry.ManagerName;   // manager name
                    }
                }

                html_message = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                        "<div> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + ticketName + ",</span> " +
                        "<span style='font -family: Arial,Helvetica,sans-serif; font-size: 11pt;'>,</span> " +
                        "<br> " +
                        "<blockquote> " +
                        "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'><br /> " +
                        "       Your Ticket # " + ticketid.ToString() + " with title : <b>" + subject + "</b> has been resolved by our technician <br/> <br/> " +
                        "		<br> " +
                        "	    <br> " +
                        "	</span> " +
                        "	<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt; background-color: rgb(255, 255, 0);'> " +
                        "       If you have any comments or calarification on the same please feel free to contact <i>" + tech_name + "</i> at " + teh_mail +
                        "	</span>" +
                        "	<br> " +
                        "	<br>" +
                        "</blockquote> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                        "<br> " +
                        "<br> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>IT Helpdesk Team</span> " +
                        "<br> " +
                        "<br> " +
                        "</div> ";

                subject = "[Ticket # " + ticketid.ToString() + " Resolved] : " + subject;


                MailMessage mailMsg = new MailMessage();

                // To                
                mailMsg.To.Add(new MailAddress(ticketEmail, ticketName));       // ticket_owner
                mailMsg.CC.Add(new MailAddress(teh_mail, tech_name));           // resolver_tech
                mailMsg.CC.Add(new MailAddress(man_mail, man_name));            // resolver manager     
                mailMsg.Bcc.Add(new MailAddress("arsalan@rethinktechs.com", "Arsalan Ahmed (RTT)"));

                // From
                mailMsg.From = new MailAddress("helpdesk@alahligroup.com", "IT Helpdesk (RTT)");

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;

                string text = subject;
                string html = html_message;

                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_key, _pwd);
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private static bool                RedirectionUrlValidationCallback         (string redirectionUrl)    
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        public static System.Threading.Tasks.Task   SendEWSTicketOwnerReply         (int TId)           
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int ReplyNo = 0;
                int TicketId = 0;
                string Body = "";

                string Subject = "";
                string OwnerEmailTo = "";
                string OwnerFromName = "";

                string TechAddress = "";
                string TechFromName = "";

                string box_code = "Domadmin41";
                string box_address = "helpdesk@alahligroup.com";

                using (TicketEntities _db = new TicketEntities())
                {
                    var q = (from c in _db.Ticket_Communication
                             join t in _db.Tickets on c.TicketId equals t.TicketId
                             where (c.TID == TId)
                             select new
                             {
                                 TicketId = c.TicketId,
                                 ReplyNo = c.ReplyNo,
                                 TOwnerName = t.FromName,
                                 TOwnerAddress = t.FromAddress,
                                 TSubject = t.TSubject,
                                 TechName = c.FromUser,
                                 TechAddress = c.FromAddress
                             }).FirstOrDefault();

                    ReplyNo = q.ReplyNo;
                    TicketId = q.TicketId;
                    Subject = q.TSubject;
                    OwnerEmailTo = q.TOwnerAddress;
                    OwnerFromName = q.TOwnerName;
                    TechAddress = q.TechAddress;
                    TechFromName = q.TechName;
                }
                Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                               "<div> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + TechFromName + ",</span> " +
                               "<br> " +
                               "<br> " +
                               "<blockquote> " +
                               "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                               "       Requestor : " + OwnerFromName + " has sent you a Message for your Ticket # " + TicketId +
                               "       <br />" +
                               "       <br />You can check the message on link : http://helpdesk.alahligroup.com/ " +
                               "        <br> " +
                               "	    <br> " +
                               "	</span> " +
                               "</blockquote> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                               "<br> " +
                               "<br> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>IT Helpdesk</span> " +
                               "<br> " +
                               "<br> " +
                               "<b> " +
                               "	<i> " +
                               "		<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt; ; background-color: silver;'> " +
                               "			P.S : Please do not Copy IT Helpdesk email account for any further communication regarding this ticket " +
                               "		</span> " +
                               "	</i> " +
                               "</b> " +
                               "<br> " +
                               "</div> ";


                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(box_address, box_code);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(box_address, RedirectionUrlValidationCallback);

                EmailMessage email = new EmailMessage(service);
                email.ToRecipients.Add(TechAddress);
                email.CcRecipients.Add(OwnerEmailTo);
                email.BccRecipients.Add("sg.net123@gmail.com");
                email.Subject = "Re[" + ReplyNo + "]: " + Subject;
                email.Body = new MessageBody(Body);
                using (TicketEntities _db = new TicketEntities())
                {
                    var qc = (from c in _db.Ticket_Address
                              where (c.TicketId == TicketId)
                              select new { c.EmailAddress }).ToList();
                    foreach (var item in qc)
                    {
                        email.CcRecipients.Add(item.EmailAddress);
                    }
                }
                email.Send();
            });
        }
        public static System.Threading.Tasks.Task   SendEWSTechnicianReplyMail      (int TId)           
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int ReplyNo = 0;
                int TicketId = 0;
                string Body = "";

                string Subject = "";
                string OwnerEmailTo = "";
                string OwnerFromName = "";

                string TechAddress = "";
                string TechFromName = "";

                string box_code = "Domadmin41";
                string box_address = "helpdesk@alahligroup.com";

                using (TicketEntities _db = new TicketEntities())
                {
                    var q = (from c in _db.Ticket_Communication
                             join t in _db.Tickets on c.TicketId equals t.TicketId
                             where (c.TID == TId)
                             select new
                             {
                                 TicketId = c.TicketId,
                                 ReplyNo = c.ReplyNo,
                                 TOwnerName = t.FromName,
                                 TOwnerAddress = t.FromAddress,
                                 TSubject = t.TSubject,
                                 TechName = c.FromUser,
                                 TechAddress = c.FromAddress
                             }).FirstOrDefault();

                    ReplyNo = q.ReplyNo;
                    TicketId = q.TicketId;
                    Subject = q.TSubject;
                    OwnerEmailTo = q.TOwnerAddress;
                    OwnerFromName = q.TOwnerName;
                    TechAddress = q.TechAddress;
                    TechFromName = q.TechName;
                }
                Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                               "<div> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + OwnerFromName + ",</span> " +
                               "<br> " +
                               "<br> " +
                               "<blockquote> " +
                               "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                               "       Our Technician " + TechFromName + " has sent you a Message for your Ticket # " + TicketId +
                               "       <br />" +
                               "       <br />You can check the message on link : http://helpdesk.alahligroup.com/ " +
                               "        <br> " +
                               "	    <br> " +
                               "	</span> " +
                               "</blockquote> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                               "<br> " +
                               "<br> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>IT Helpdesk</span> " +
                               "<br> " +
                               "<br> " +
                               "<b> " +
                               "	<i> " +
                               "		<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt; ; background-color: silver;'> " +
                               "			P.S : Please do not Copy IT Helpdesk email account for any further communication regarding this ticket " +
                               "		</span> " +
                               "	</i> " +
                               "</b> " +
                               "<br> " +
                               "</div> ";


                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(box_address, box_code);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(box_address, RedirectionUrlValidationCallback);

                EmailMessage email = new EmailMessage(service);
                email.ToRecipients.Add(OwnerEmailTo);
                email.CcRecipients.Add(TechAddress);
                email.BccRecipients.Add("sg.net123@gmail.com");
                email.Subject = "Re[" + ReplyNo + "]: " + Subject;
                email.Body = new MessageBody(Body);
                using (TicketEntities _db = new TicketEntities())
                {
                    var qc = (from c in _db.Ticket_Address
                              where (c.TicketId == TicketId)
                              select new { c.EmailAddress }).ToList();
                    foreach (var item in qc)
                    {
                        email.CcRecipients.Add(item.EmailAddress);
                    }
                }
                email.Send();
            });
        }
        public static System.Threading.Tasks.Task   SendEWSCreateTicketMail         (int TicketId)      
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                string Body = "";
                string EmailTo = "";
                string Subject = "";
                string FromName = "";
                string box_code = "Domadmin41";
                string box_address = "helpdesk@alahligroup.com";

                using (TicketEntities _db = new TicketEntities())
                {
                    var q = (from m in _db.Tickets
                             where (m.TicketId == TicketId)
                             select new { m.FromName, m.FromAddress, m.TSubject }).FirstOrDefault();
                    FromName = q.FromName;
                    Subject = q.TSubject;
                    EmailTo = q.FromAddress;
                }
                Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                               "<div> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + FromName + ",</span> " +
                               "<br> " +
                               "<br> " +
                               "<blockquote> " +
                               "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                               "       Your Ticket Number " + TicketId + "  (<i>Subject : " + Subject + "</i>) has been successfully created" +
                               "       <br />Please wait while our technicians look into this ticket. " +
                               "       <br />You will soon hear from us on the matter " + 
                               "        <br> " +
                               "	    <br> " +
                               "	</span> " +
                               "</blockquote> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                               "<br> " +
                               "<br> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>IT Helpdesk</span> " +
                               "<br> " +
                               "<br> " +
                               "<b> " +
                               "	<i> " +
                               "		<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt; ; background-color: silver;'> " +
                               "			P.S : Please do not Copy IT Helpdesk email account for any further communication regarding this ticket " +
                               "		</span> " +
                               "	</i> " +
                               "</b> " +
                               "<br> " +
                               "</div> ";


                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(box_address, box_code);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(box_address, RedirectionUrlValidationCallback);

                EmailMessage email = new EmailMessage(service);
                email.ToRecipients.Add(EmailTo);
                email.Subject = Subject;
                email.Body = new MessageBody(Body);
                using (TicketEntities _db = new TicketEntities())
                {
                    var qc = (from c in _db.Ticket_Address
                              where (c.TicketId == TicketId)
                              select new { c.EmailAddress }).ToList();
                    foreach (var item in qc)
                    {
                        email.CcRecipients.Add(item.EmailAddress);
                    }
                }

                email.Send();
            });
        }
        public static System.Threading.Tasks.Task   SendEWSTicketResolvedEmail      (int TicketId)      
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                string Body = "";
                string EmailTo = "";
                string Subject = "";
                string FromName = "";

                string ResolvedUserAddress = "";
                string ResolvedUserName = "";                
                string ResolvedMessage  = "";
                string ResolvedDate = "";
                
                
                string box_code = "Domadmin41";
                string box_address = "helpdesk@alahligroup.com";

                using (TicketEntities _db = new TicketEntities())
                {
                    var q = (from m in _db.Tickets
                             join u in _db.EndUsers on m.ResolutionBy equals u.UID
                             where (m.TicketId == TicketId)
                             select new { m.FromName, m.FromAddress, m.TSubject, u.UserName, u.UserEmail, m.ResolutionDate, m.ResolutionMessage }).FirstOrDefault();
                    FromName            = q.FromName;
                    Subject             = q.TSubject;
                    EmailTo             = q.FromAddress;
                    ResolvedUserName    = q.UserName;
                    ResolvedMessage     = q.ResolutionMessage;
                    ResolvedUserAddress = q.UserEmail;
                    ResolvedDate        = (q.ResolutionDate.HasValue)? q.ResolutionDate.Value.ToString("dd-MMM-yyyy HH:mm") : "N/A";
                }
                Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                               "<div> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + FromName + ",</span> " +
                               "<br> " +
                               "<br> " +
                               "<blockquote> " +
                               "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                               "       Your Ticket Number " + TicketId + "  (<i>Subject : " + Subject + "</i>) has been successfully Resolved by Our Technician" +
                               "       <br />" +
                               "       <br />Technician " + ResolvedUserName +  " worked on ticket and market it as closed on date (" + ResolvedDate + ") <br />" +
                               "       <br />Technician closing message on ticket was : [<i>" + ResolvedMessage + "</i>] <br />" +
                               "       <br />We thank you for your patience and cooperation " +                               
                               "       <br>  <br> " +
                               "	</span> " +
                               "</blockquote> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                               "<br> " +
                               "<br> " +
                               "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>IT Helpdesk</span> " +
                               "<br> " +
                               "<br> " +
                               "<b> " +
                               "	<i> " +
                               "		<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt; ; background-color: silver;'> " +
                               "			P.S : Please do not hesitate to contact us regarding this ticket if work is not completed according to your statisfaction " +
                               "		</span> " +
                               "	</i> " +
                               "</b> " +
                               "<br> " +
                               "</div> ";


                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(box_address, box_code);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(box_address, RedirectionUrlValidationCallback);

                EmailMessage email = new EmailMessage(service);
                email.ToRecipients.Add(EmailTo);
                email.CcRecipients.Add(ResolvedUserAddress);
                email.Subject = "Resolved # " + TicketId + ": " + Subject;
                email.Body = new MessageBody(Body);
                using (TicketEntities _db = new TicketEntities())
                {
                    var qc = (from c in _db.Ticket_Address
                              where (c.TicketId == TicketId)
                              select new { c.EmailAddress }).ToList();
                    foreach (var item in qc)
                    {
                        email.CcRecipients.Add(item.EmailAddress);
                    }
                }

                email.Send();
            });
        }

        public static System.Threading.Tasks.Task SendServiceCreationEmail          (int ServiceId)     
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                string Body         = "";
                string Subject      = "";

                string OName    = "";
                string OEmail   = "";
                string CName    = "";
                string CEmail   = "";

                string EndDate  = "";
                string StartDate= "";

                string box_code     = "Domadmin41";
                string box_address  = "helpdesk@alahligroup.com";

                using (TicketEntities _db = new TicketEntities())
                {
                    var q = (from m in _db.C_service_data
                             join u in _db.EndUsers on m.ServiceOwnerId equals u.UID
                             join x in _db.EndUsers on m.CreatedById equals x.UID
                             where (m.ServiceId == ServiceId)
                             select new {   CreatorEmail = x.UserEmail, CreatorName = m.CreateByName,
                                            OwnerEmail   = u.UserEmail, OwnerName   = u.UserName,
                                            m.StartDate, m.Deadline   , ServiceName = m.SName }).FirstOrDefault();

                    Subject = q.ServiceName.Trim();

                    CEmail = q.CreatorEmail.Trim();
                    CName  = q.CreatorName.Trim();

                    OEmail = q.OwnerEmail.Trim();
                    OName  = q.OwnerName.Trim();

                    StartDate   = (q.StartDate.HasValue) ? q.StartDate.Value.ToString("dd-MMM-yyyy") : "N/A";
                    EndDate     = (q.Deadline .HasValue) ? q.Deadline .Value.ToString("dd-MMM-yyyy") : "N/A";
                }
            if (CName.Trim().ToLower().Equals(OName.Trim().ToLower())) {
                // YOU ARE CREATING
                Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                        "<div> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + OName + ",</span> " +
                        "<br> " +
                        "<br> " +
                        "<blockquote> " +
                        "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                        "       A new Service with Number : " + ServiceId + " has been created on " +
                        "       <span style='font-family: Arial,Helvetica,sans-serif; font-size: 9pt; font-style:italic;'>" + DateTime.Now.AddHours(4).ToString("dd-MMM-yyyy HH:mm") + "</span> " +
                        "       <br />" +
                        "       <br />Title of Service is (<i> Title : " + Subject + " </i>)<br />" +
                        "       <br />Service Start Date is [" + StartDate + "] and Service End Date is [<b>" + EndDate + "</b>] <br />" +
                        "       <br />For Details Please visit management Portal at : http://helpdesk.alahligroup.com/technician/ " +
                        "       <br>  <br> " +
                        "	</span> " +
                        "</blockquote> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                        "<br> " +
                        "<br> " +
                        "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Management Tool Devs.</span> " +
                        "<br>  <br> " +
                        "</div> ";
                }
                else {
                    // MANAGER IS CREATING
                    Body = "<div style='font-size:12pt;color:#000000;font-family:Calibri,Arial,Helvetica,sans-serif;'> " +
                           "<div> " +
                           "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Dear " + OName + ",</span> " +
                           "<br> " +
                           "<br> " +
                           "<blockquote> " +
                           "    <span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'> " +
                           "       <b>" + CName + "</b> has created a new Service with Number : " + ServiceId + "  on " +
                           "       <span style='font-family: Arial,Helvetica,sans-serif; font-size: 9pt; font-style:italic;'>" + DateTime.Now.AddHours(4).ToString("dd-MMM-yyyy HH:mm")  + "</span> " +
                           "       <br />" +
                           "       <br />Title of Service is (<i> Title : " + Subject + " </i>)<br />" +
                           "       <br />Service Start Date is [" + StartDate + "] and Service End Date is [<b>" + EndDate + "</b>] <br />" +
                           "       <br />For Details Please visit management Portal at : http://helpdesk.alahligroup.com/technician/ " +
                           "       <br>  <br> " +
                           "	</span> " +
                           "</blockquote> " +
                           "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>Regards, </span> " +
                           "<br> " +
                           "<br> " +
                           "<span style='font-family: Arial,Helvetica,sans-serif; font-size: 11pt;'>" + CName + "</span> " +
                           "<br>  <br> " +
                           "</div> ";
                }
                
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(box_address, box_code);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(box_address, RedirectionUrlValidationCallback);

                EmailMessage email = new EmailMessage(service);
                email.ToRecipients.Add(OEmail);
                email.CcRecipients.Add(CEmail);
                email.BccRecipients.Add("sg.net123@gmail.com");
                email.Subject = "Created : New Service # " + ServiceId;
                email.Body = new MessageBody(Body);
                //using (TicketEntities _db = new TicketEntities()) {
                //    var qc = (from c in _db.C_sv_co_owner
                //                join d in _db.EndUsers on c.CoOwnerId equals d.UID
                //                where (c.ServiceId== ServiceId)
                //                select new { EmailAddress = d.UserEmail }).ToList();
                //    foreach (var item in qc) {
                //        email.CcRecipients.Add(item.EmailAddress);
                //    }
                //}

                email.Send();
            });
        }

    }
}