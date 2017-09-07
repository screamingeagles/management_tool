using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using ManagementTool.Models;
using ManagementTool.Common;
using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;


namespace ManagementTool.Controllers
{
    public class HomeController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        public ActionResult Index()
        {            
            ViewData["SubTitle"] = "Welcome in ASP.NET MVC 5 INSPINIA SeedProject ";
            ViewData["Message"] = "It is an application skeleton for a typical MVC 5 project. You can use it to quickly bootstrap your webapp projects.";

            return View();
        }

        public ActionResult Welcome()
        {
            // only for this we are checking the session of user.
            int _sid = 0;
                _sid = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            if (_sid == 0) { return RedirectToAction("Index", "Home", new { x = 1 }); }

            return View();
        }

        public ActionResult Logout()
        {
            // only for this we are checking the session of user.
            int _sid = 0;
                _sid = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            if (_sid == 0) { return RedirectToAction("Index", "Login", new { x = 1 }); }

            UserIdentity.UserId     = 0;
            UserIdentity.RoleId     = 0;
            UserIdentity.UserEmail  = "";
            UserIdentity.UserName   = "";
            HttpContext.Session.Abandon();
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult process(string userid, string txtpass)
        {
            try
            {
                int? q = 0;
                int uid = 0;
                double daysLogin = 0;
                bool LoginResult = false;
                DateTime logindt = DateTime.Now;
                string remoteip = "Unspecified";
                string location = "";
                string encrypted_pwd = Bhai.encrypeText(txtpass);
                //txtpass = encrypted_pwd;


                remoteip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                location = Bhai.GetJsonLocation(remoteip);


                uid = Bhai.GetUserId(userid);
                var qry = (from e in db.EndUsers
                           where ((e.UserEmail == userid) && (e.UserPassword == encrypted_pwd))
                           select new { e.UID, e.LastLogin }).FirstOrDefault();
                if (qry == null)
                {
                    // means the user is not present in the database so check with outlook
                    // also check if his outlook password is changed
                    LoginResult = CheckUserLoginWithEWS(userid, txtpass);

                    if (LoginResult == true)
                    {
                        #region No Doubt he is a valid User
                        // here just update last login is DB
                        // either add new user or update new user passwrod
                        if (uid > 0)
                        {
                            // here update new user password and get userid
                            q = Bhai.UpdateUserLogin(uid, encrypted_pwd); //txtpass
                        }
                        else
                        {
                            // here add new user 
                            uid = Bhai.SaveNewUser(userid, encrypted_pwd); //txtpass
                        }
                        #endregion
                    }
                    else
                    {
                        // the user name and password is not present in the system
                        // its not matching with the ews as well .... return;
                        return RedirectToAction("Index", "Home", new { x = 3 });
                    }
                }
                else
                {
                    uid = qry.UID;
                    logindt = (qry.LastLogin.HasValue) ? qry.LastLogin.Value : DateTime.Now;
                    daysLogin = (DateTime.Now - logindt).TotalDays;
                    if (daysLogin > 20)
                    {
                        // he is still using old outlook password and not a new one
                        #region Check for outlook match
                        LoginResult = CheckUserLoginWithEWS(userid, txtpass);
                        if (LoginResult == false)
                        {
                            //return to home and tell him to use new password
                            return RedirectToAction("Index", "Home", new { x = 4 });
                        }
                        else
                        {
                            // update his new password 
                            q = Bhai.UpdateUserLogin(uid, encrypted_pwd);
                        }
                        #endregion
                    }
                }

                uid = Bhai.SaveToSession(uid, remoteip, location);
                HttpContext.Session["SessionId"] = uid;                

                #region forwarding to recieving link
                string hid = (Request.Form["hdnid"] == null) ? string.Empty : Request.Form["hdnid"].ToString();
                if (string.IsNullOrEmpty(hid) == false) { return RedirectToAction("Body", "Tickets", new { id = hid }); }
                #endregion

                return RedirectToAction("Welcome", "Home");
                //return RedirectToAction("Index", "Login", new { x = 3 });
            }
            catch (Exception exi)
            {
                string m = exi.Message;
                return RedirectToAction("Index", "Home", new { x = 1 });
            }
        }

        private static bool CheckUserLoginWithEWS(string userid, string pass)
        {
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(userid, pass);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl(userid, RedirectionUrlValidationCallback);

                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
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



        [ChildActionOnly]
        public ActionResult LoginPartial() {
            return PartialView("_LoginPartial");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}