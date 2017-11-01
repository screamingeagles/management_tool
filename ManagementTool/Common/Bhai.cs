using System;
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

            using (ProjectEntities db = new ProjectEntities())
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


       public static string GetUserName         (int userid)                                
        {
            string q = "";

            using (ProjectEntities db = new ProjectEntities()) {
                q = (from e in db.EndUsers
                     where e.UID == userid
                     select e.UserName).FirstOrDefault();
            }
            
            return q;
        }

        public static string GetUserEmail       (int userid)                                
        {
            string q = "";

            using (ProjectEntities db = new ProjectEntities())
            {
                q = (from e in db.EndUsers
                     where e.UID == userid
                     select e.UserEmail).FirstOrDefault();
            }

            return q;
        }



        private static int GetUserRole          (int userid)                                
        {
            int role = 103;
            try
            {
                using (ProjectEntities pe = new ProjectEntities())
                {
                    int? q = (from r in pe.EndUsers
                              where (r.UID == userid)
                              select r.UserType).FirstOrDefault();

                    if (q.HasValue)
                    {
                        role = q.Value;
                    }
                    else
                    {
                        role = 103;
                    }
                }
                return role;
            }
            catch
            {
                return role;
            }
        }

        public static int UpdateUserLogin       (int uid, string txtpass)                   
        {
            int q = 0;

            using (ProjectEntities db = new ProjectEntities())
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
            using (ProjectEntities db = new ProjectEntities()) {
                EndUser e = new EndUser();
                e.UserName = userid.Substring(0, userid.IndexOf("@"));
                e.UserEmail = userid;
                e.UserPassword = password;
                e.IsValidLogin = true;
                e.UserType = 103;
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
            using (ProjectEntities db = new ProjectEntities())
            {
                EndUser_LoginDetails ld = new EndUser_LoginDetails();
                ld.UserId = uid;
                ld.LoginDate = DateTime.Now;
                ld.LogOut = null;
                ld.SessionId = Guid.NewGuid().ToString();
                ld.UserIp = remoteip;
                ld.SessionDuration = 0;
                ld.LoginLocation = location;
                db.EndUser_LoginDetails.Add(ld);
                sid = db.SaveChanges();
                sid = ld.LID;

                #region Setting User Identity
                var q = (from u in db.EndUsers
                         where u.UID == uid
                         select new { u.UID, u.UserName, u.UserEmail, u.UserType }).FirstOrDefault();
                UserIdentity.UserId     = q.UID;
                UserIdentity.UserName   = q.UserName;
                UserIdentity.UserEmail  = q.UserEmail;
                UserIdentity.RoleId     = (q.UserType.HasValue)? q.UserType.Value : 103;
                #endregion
            }
            return sid;
        }

        public static int GetUserIdFromSession  (int Sessionid)                             
        {
            int ret = 0;
            try
            {
                using (ProjectEntities _db = new ProjectEntities())
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
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from e in db.EndUsers
                         where (e.UID == UserId)
                         select e.UserType).FirstOrDefault();
                r = (q.HasValue) ? q.Value : 0;
            }
            return r;
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




        public static string getSubArea         (int SubAreaId)                             {            

            if (SubAreaId == 0) { return "N/A"; }
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from e in db.C003_SUB_AREA
                         where (e.SubAreaId == SubAreaId)
                         select e.SubAreaName).FirstOrDefault();
                return q.ToString();
            }
            
        }

        public static string getProjectType     (int ProjectTypeId)                         
        {

            if (ProjectTypeId  == 0) { return "N/A"; }
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from e in db.C013_PROJECT_TYPE
                         where (e.ProjectTypeId == ProjectTypeId)
                         select e.ProjectType).FirstOrDefault();
                return q.ToString();
            }

        }

        public static string getSubPhase        (int SubPhaseId)                            
        {

            if (SubPhaseId == 0) { return "N/A"; }
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from e in db.C006_SubPhase
                         where (e.SubPhaseId == SubPhaseId)
                         select e.SubPhaseName).FirstOrDefault();
                return q.ToString();
            }

        }


        public static string GetBucketDetail(int BucketId)
        {

            if (BucketId == 0) { return "N/A"; }
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from b  in db.C007_BUCKET
                         join p  in db.C004_PROJECT     on b.ProjectId  equals p.ProjectId
                         join ph in db.C005_PHASE       on b.PhaseId    equals ph.PhaseId
                         join sp in db.C006_SubPhase    on b.SubPhaseId equals sp.SubPhaseId
                         into jointable from z in jointable.DefaultIfEmpty()
                         where (b.IsActive.Equals(true)) && (b.BucketId == BucketId)
                         select new {
                             ProjectName = p.ProjectName,
                             ProjectId = p.ProjectId,
                             PhaseName = ph.PhaseName,
                             PhaseId = ph.PhaseId,
                             SubPhaseName = (z.SubPhaseName == null) ? "" : z.SubPhaseName
                         }).FirstOrDefault();

                return q.ProjectName + "(" + q.ProjectId + ") >> " + q.PhaseName + "(" + q.PhaseId + ") >> " + q.SubPhaseName;
            }

        }


        public static List<SelectListItem>      GetTechnicianList   (int id)                {


            SelectListItem _SelectListItem;
            List<SelectListItem> tech = new List<SelectListItem>();
            try
            {
                using (ProjectEntities _db = new ProjectEntities())
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

        public static List<contributors>      GetContributorsList   (int ProjectId)         
        {
            List<contributors> tech = new List<contributors>();
            try
            {
                using (ProjectEntities _db = new ProjectEntities()) {
                    tech = (from t in _db.C018_coOwners
                                  join u in _db.EndUsers on t.UserId equals u.UID
                                  orderby t.CreatedDate descending
                                  where (t.ProjectId == ProjectId) 
                                  select new contributors { FieldId = t.CoOwnerId,
                                                            UID = t.UserId,
                                                            UserName = u.UserName,
                                                            UserContribution = t.OwnerContribution,
                                                            ContributorAddedBy = "",
                                                            ContributorAddDate = t.CreatedDate}).ToList();                   
                }
            }
            catch (Exception e) {
                contributors c = new contributors();
                c.FieldId            = 0;
                c.UID                = 0;
                c.UserName           = "Exception Occured";
                c.UserContribution   = e.Message;
                c.ContributorAddDate = DateTime.Now;
                tech.Add(c);
            }
            return tech;
        }

        public static DataTable GetDataTable    (string tablename, string sSQL)             
        {
            DataTable dt = new DataTable(tablename);
            using (ProjectEntities _db = new ProjectEntities())
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