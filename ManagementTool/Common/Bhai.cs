﻿using System;
using System.Linq;
using System.Web;
using ManagementTool.Models;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ManagementTool.Common
{
    public class Bhai
    {
        public static string GetJsonLocation    (string ip)                                 
        {
            string cty = "Local";
            try
            {

                if (ip != "::1")
                {
                    string url = "http://freegeoip.net/json/" + ip;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    string response = streamReader.ReadToEnd();

                    JObject objects = JObject.Parse(response);      // parse as object 
                    cty = objects["city"].ToString() + " - " + objects["country_name"].ToString();


                    /* var objects = JArray.Parse(response); // parse as array  
                       foreach (JObject root in objects)                     {
                        foreach (KeyValuePair<String, JToken> app in root)  {
                            var appName = app.Key;
                            cty = (String)app.Value["city"];
                        }
                      }*/
                }
                return cty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string encrypeText        (string pwdText)                            
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(pwdText);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }

        public static int GetUserId             (string userid)                             
        {
            int? q = 0;

            using (TicketEntities db = new TicketEntities())
            {
                q = (from e in db.EndUsers
                     where e.UserEmail == userid
                     select e.UID).FirstOrDefault();
            }

            if (q.HasValue)
            {
                if (q.Value > 0) { return q.Value; }
                else { return 0; }
            }
            else
            {
                q = 0;
            }

            return q.Value;
        }


        public static int GetTicketAssignedUserId(int TicketId)
        {
            int? q = 0;

            using (TicketEntities db = new TicketEntities())
            {
                q = (from e in db.Tickets
                     where e.TicketId == TicketId
                     select e.AssignedTo).FirstOrDefault();
            }

            if (q.HasValue) {
                if (q.Value > 0) { return q.Value; }
                else { return 0; }
            }
            else {
                q = 0;
            }

            return q.Value;
        }



        public static string GetUserName        (int userid)                                
        {
            string q = "";

            using (TicketEntities db = new TicketEntities()) {
                q = (from e in db.EndUsers
                     where e.UID == userid
                     select e.UserName).FirstOrDefault();
            }
            
            return q;
        }

        public static string GetUserEmail(int userid)
        {
            string q = "";

            using (TicketEntities db = new TicketEntities())
            {
                q = (from e in db.EndUsers
                     where e.UID == userid
                     select e.UserEmail).FirstOrDefault();
            }

            return q;
        }

        public static int UpdateUserLogin       (int uid, string txtpass)                   
        {
            int q = 0;

            using (TicketEntities db = new TicketEntities())
            {
                EndUser e = db.EndUsers.Find(uid);
                e.UserPassword = txtpass;
                e.IsValidLogin = true;
                e.LastLogin = DateTime.Now;
                db.EndUsers.Attach(e);
                db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                q = db.SaveChanges();
            }
            return q;
        }

        public static int SaveNewUser           (string userid, string password)            
        {
            int uid = 0;
            using (TicketEntities db = new TicketEntities()) {
                EndUser e = new EndUser();
                e.UserName = userid.Substring(0, userid.IndexOf("@"));
                e.UserEmail = userid;
                e.UserPassword = password;
                e.IsValidLogin = true;
                e.UserType = 0;
                e.UserCreated = DateTime.Now;
                e.LastLogin = DateTime.Now;
                e.IsActive = true;
                db.EndUsers.Add(e);
                uid = db.SaveChanges();
                if (uid > 0) { uid = e.UID; }
            }

            return uid;
        }

        public static int SaveToSession         (int uid, string remoteip, string location) 
        {
            int sid = 0;
            using (TicketEntities db = new TicketEntities())
            {
                LOGIN_DETAIL ld = new LOGIN_DETAIL();
                ld.UserId = uid;
                ld.LoginDate = DateTime.Now;
                ld.LogOut = null;
                ld.SessionId = Guid.NewGuid().ToString();
                ld.UserIp = remoteip;
                ld.SessionDuration = 0;
                ld.LoginLocation = location;
                db.LOGIN_DETAIL.Add(ld);
                sid = db.SaveChanges();
                sid = ld.LID;
            }
            return sid;
        }

        public static int GetUserIdFromSmartGuid(Guid guid)                                 
        {
            int ret = 0;
            try
            {
                using (TicketEntities _db = new TicketEntities())
                {
                    ret = (from l in _db.SmartLogins
                           where (l.GUID == guid)
                           select l.UserId).FirstOrDefault();
                }
                return ret;
            }
            catch
            {
                return -1;
            }
        }

        public static int GetUserIdFromSession  (int Sessionid)                             
        {
            int ret = 0;
            try
            {
                using (TicketEntities _db = new TicketEntities())
                {
                    ret = (from l in _db.vw_SessionUser
                           where (l.LID.Equals(Sessionid))
                           select l.UID).FirstOrDefault();
                }
                return ret;
            }
            catch
            {
                return -1;
            }
        }

        public static int getUserTypeId         (int UserId)                                
        {
            int r = 0;
            using (TicketEntities db = new TicketEntities())
            {
                var q = (from e in db.EndUsers
                         where (e.UID == UserId)
                         select e.UserType).FirstOrDefault();
                r = (q.HasValue) ? q.Value : 0;
            }
            return r;
        }

        public static int getTicketId           (string guid)                               
        {

            int q = 0;
            Guid g = new Guid(guid);

            using (TicketEntities db = new TicketEntities()) {
                q = (from t in db.Tickets
                     where (t.emailguid == g)
                     select t.TicketId).FirstOrDefault();
            }
            return q;
        }

        public static int getTicketUser         (int Ticketid)                              
        {

            int q = 0;
            

            using (TicketEntities db = new TicketEntities())
            {
                q = (from t in db.Tickets
                     where (t.TicketId == Ticketid)
                     select t.UserId).FirstOrDefault();
            }
            return q;
        }

        public static int SetTicketRead         (int userid, int  TicketId)                 
        {

            int result = 0;
            using (TicketEntities db = new TicketEntities())
            {
                result = (from tk in db.Tickets
                          where (tk.TicketId == TicketId)
                          select tk.TicketId).FirstOrDefault();

                Ticket t = db.Tickets.Find(result);
                t.IsNew = false;
                t.IsRead = true;
                db.Tickets.Attach(t);
                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                result = db.SaveChanges();
            }

            return result;
        }

        public static bool IsNumeric            (string s)                                  
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    return false;
                }
            }

            return true;
        }
        
        public static int UpdateTicketSubject   (int Ticketid)                              
        {
            int q = 0;
            try
            {
                using (TicketEntities db = new TicketEntities())
                {
                    Ticket t = db.Tickets.Find(Ticketid);
                    t.TSubject = "[Helpdesk Ticket # : " + Ticketid + "] " + t.TSubject;
                    db.Tickets.Attach(t);
                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    q = db.SaveChanges();
                }
            }
            catch {
                q = 0;
            }            
            return q;
        }

        public static int GetReplyNo            (int TicketId)                              {

            int q = 0;           

            using (TicketEntities db = new TicketEntities())
            {
                q = (from c in db.Ticket_Communication
                     where (c.TicketId == TicketId)
                     select (int?)c.ReplyNo).Max() ?? 0;
            }
            return (q == 0) ? 1 : q + 1;
        }

        public static List<SelectListItem>      GetTechnicianList   (int id)                {


            SelectListItem _SelectListItem;
            List<SelectListItem> tech = new List<SelectListItem>();
            try
            {
                using (TicketEntities _db = new TicketEntities())
                {
                    var queryc = (from t in _db.EndUsers
                                  orderby t.UserName
                                  where (((t.UserType == 2) || (t.UserType == 1)) && ((t.IsActive == true) && (t.UID != 1017)))
                                  select new { t.UID, t.UserName }).ToList();
                    foreach (var litem in queryc)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = litem.UID.ToString();
                        _SelectListItem.Text = litem.UserName;
                         if (id > 0) {
                            if (litem.UID==id) { _SelectListItem.Selected = true; }
                        }
                        tech.Add(_SelectListItem);
                    }
                }
            }
            catch (Exception e)
            {
                _SelectListItem = new SelectListItem();
                _SelectListItem.Value = "0";
                _SelectListItem.Text = e.Message;
                tech.Add(_SelectListItem);
            }
            return tech;
        }

        public static List<SelectListItem>      GetStatusList       (int id)                
        {


            SelectListItem _SelectListItem;
            List<SelectListItem> status = new List<SelectListItem>();
            try
            {
                using (TicketEntities _db = new TicketEntities())
                {
                    var querys = (from s in _db.C_service_status
                                  orderby s.StatusName
                                  where s.IsActive == true
                                  select new { s.SID, s.StatusName }).ToList();
                    foreach (var sitem in querys)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = sitem.SID.ToString();
                        _SelectListItem.Text = sitem.StatusName;
                        if (id > 0) {
                            if (sitem.SID == id) { _SelectListItem.Selected = true; }
                        }
                        status.Add(_SelectListItem);
                    }
                }
            }
            catch (Exception e)
            {
                _SelectListItem = new SelectListItem();
                _SelectListItem.Value = "0";
                _SelectListItem.Text = e.Message;
                status.Add(_SelectListItem);
            }
            return status;
        }


        public static DataTable GetDataTable    (string tablename, string sSQL)             
        {
            DataTable dt = new DataTable(tablename);
            using (TicketEntities _db = new TicketEntities())
            {
                using (SqlConnection con = new SqlConnection(_db.Database.Connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sSQL, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}