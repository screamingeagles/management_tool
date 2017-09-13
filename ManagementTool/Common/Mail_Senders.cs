using System;
using System.Web;
using System.Data;
using System.Linq;
using ManagementTool.Models;
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

                using (ProjectEntities _db = new ProjectEntities())
                {
                    var q = (from m in _db.C008_TASK_DATA
                             join u in _db.EndUsers on m.OwnerId equals u.UID
                             join x in _db.EndUsers on m.GeneratedBy equals x.UID
                             where (m.TaskId == ServiceId)
                             select new {   CreatorEmail = x.UserEmail, CreatorName = m.GeneratedBy,
                                            OwnerEmail   = u.UserEmail, OwnerName   = u.UserName,
                                            m.StartDate, m.Deadline   , ServiceName = m.SName }).FirstOrDefault();

                    Subject = q.ServiceName.Trim();

                    CEmail = q.CreatorEmail.Trim();
                    CName  = q.CreatorName.ToString().Trim();

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