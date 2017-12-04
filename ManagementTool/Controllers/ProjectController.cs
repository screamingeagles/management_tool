using System;
using System.Web;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Common;
using ManagementTool.Models;
using System.Collections.Generic;
using System.IO;

namespace ManagementTool.Controllers
{
    public class ProjectController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetSubAreaByArea(int SelectedArea)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C003_SUB_AREA
                         where (c.AreaActive.Equals(true)) && (c.AreaId == SelectedArea)
                         select new { c.SubAreaId, c.SubAreaName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetAreaByDivision(int SelectedDivision)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C002_AREA
                         where (c.isActive.Equals(true)) && (c.DivisionId == SelectedDivision)
                         select new { c.AreaId, c.AreaName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetSubAreaByDivision(int SelectedDivision)       {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from s in db.vw_SubAreaListByDivision
                         where (s.DivisionId == SelectedDivision)
                         select new { s.AreaId, s.AreaName }).ToList();
                return Json(new { data = q });
            }
        }


        // GET: Project
        public ActionResult Index(int? AreaId)
        {
            if (AreaId.HasValue) {
                ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", AreaId.Value);
            } else {
                ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName");
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            var c004_PROJECT = db.C004_PROJECT.Include(c => c.C001_DIVISION).Include(c => c.C002_AREA).Include(c => c.EndUser).Include(c => c.C013_PROJECT_TYPE).Where(x => x.AreaId == ((AreaId.HasValue) ?AreaId.Value:0)).OrderBy(x => x.ProjectName);
            return View(c004_PROJECT.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubAreaId = Bhai.getSubArea(c004_PROJECT.SubAreaId);
            return View(c004_PROJECT);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            //UserIdentity.UserId = 1020;
            //UserIdentity.UserName = "Arsalan (RTT)";


            ViewBag.LocationId = new SelectList(db.C010_LOCATION.OrderBy(x => x.LocationName), "LocationId", "LocationName");
            ViewBag.CompanyId = new SelectList(db.C011_COMPANY.OrderBy(x => x.CompanyName), "CompanyId", "CompanyName");
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION.OrderBy(x => x.DivisionName), "DivisionId", "DivisionName");

            ViewBag.AreaId = new SelectList(db.C002_AREA.OrderBy(x => x.AreaName), "AreaId", "AreaName");
            ViewBag.SubAreaId = new SelectList(db.C003_SUB_AREA.Take(5), "SubAreaId", "SubAreaName");
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType");

            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,CompanyId,DivisionId,AreaId,SubAreaId,ProjectName,ProjectType,StartDate,EndDate")] C004_PROJECT c004_PROJECT)
        {
            if ((c004_PROJECT.DivisionId > 0) && (c004_PROJECT.AreaId > 0) && (c004_PROJECT.ProjectName != "")) //(ModelState.IsValid)
            {
                int? pid = null;
                c004_PROJECT.GeneratedBy    = UserIdentity.UserId;
                c004_PROJECT.GeneratedDate  = DateTime.Now.AddHours(4);
                c004_PROJECT.IsActive       = true;

                db.C004_PROJECT.Add(c004_PROJECT);
                db.SaveChanges();

                pid                         = c004_PROJECT.ProjectId;
                if (pid.HasValue) { return RedirectToAction("Edit", "Project", new { id = pid }); }
                else { return RedirectToAction("Index"); }
                
            }

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.OrderBy(x => x.LocationName), "LocationId",   "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY.OrderBy(x => x.CompanyName)  , "CompanyId",    "CompanyName");
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION.OrderBy(x => x.DivisionName) , "DivisionId",   "DivisionName");

            ViewBag.AreaId      = new SelectList(db.C002_AREA.OrderBy(x => x.AreaName)          , "AreaId",         "AreaName");
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Take(5)   , "SubAreaId",      "SubAreaName");            
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE       , "ProjectTypeId",  "ProjectType");
            return View(c004_PROJECT);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null) { return HttpNotFound(); }

            ViewBag.CoOwners     = Bhai.GetContributorsList(id.Value);
            ViewBag.AttachedFiles= new SelectList(db.C019_Attachments.Where(a => a.ProjectId == id).OrderBy(a => a.CreatedDate), "AttachId", "AName");

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.OrderBy(x => x.LocationName), "LocationId", "LocationName", c004_PROJECT.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY.Where(x => x.LocationId == c004_PROJECT.LocationId).OrderBy(x => x.CompanyName), "CompanyId", "CompanyName", c004_PROJECT.CompanyId);
            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.OrderBy(x => x.DivisionName), "DivisionId", "DivisionName", c004_PROJECT.DivisionId);

            ViewBag.AreaId      = new SelectList(db.C002_AREA.Where(c => c.DivisionId == c004_PROJECT.DivisionId).OrderBy(x => x.AreaName), "AreaId", "AreaName", c004_PROJECT.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Where(c => c.AreaId   == c004_PROJECT.AreaId).OrderBy(x => x.SubAreaName)   , "SubAreaId", "SubAreaName", c004_PROJECT.SubAreaId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c004_PROJECT.ProjectType);
            return View(c004_PROJECT);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,LocationId,CompanyId,DivisionId,AreaId,SubAreaId,ProjectName,ProjectType,StartDate,EndDate,GeneratedBy,GeneratedDate,IsActive")] C004_PROJECT c004_PROJECT)
        {
            if (ModelState.IsValid)
            {
                c004_PROJECT.GeneratedBy    = UserIdentity.UserId;
                c004_PROJECT.GeneratedDate  = DateTime.Now.AddHours(4);
                c004_PROJECT.IsActive       = true;

                db.Entry(c004_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Project", new { id = c004_PROJECT.ProjectId });
            }
            ViewBag.LocationId  = new SelectList(db.C010_LOCATION, "LocationId", "LocationName", c004_PROJECT.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY.Where(x => x.LocationId == c004_PROJECT.LocationId).OrderBy(x => x.CompanyName), "CompanyId", "CompanyName", c004_PROJECT.CompanyId);
            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.OrderBy(x => x.DivisionName), "DivisionId", "DivisionName", c004_PROJECT.DivisionId);

            ViewBag.AreaId      = new SelectList(db.C002_AREA.Where(c => c.DivisionId == c004_PROJECT.DivisionId).OrderBy(x => x.AreaName), "AreaId", "AreaName", c004_PROJECT.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Where(c => c.AreaId == c004_PROJECT.AreaId).OrderBy(x => x.SubAreaName), "SubAreaId", "SubAreaName", c004_PROJECT.SubAreaId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c004_PROJECT.ProjectType);
            return View(c004_PROJECT);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubAreaId = Bhai.getSubArea(c004_PROJECT.SubAreaId);
            return View(c004_PROJECT);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            db.C004_PROJECT.Remove(c004_PROJECT);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetUserNames(string query) {
            string[] names = null;

          if (String.IsNullOrEmpty(query) == false) {
            names = (from i in db.EndUsers
                        where (i.IsActive.Equals(true)) && i.UserName.StartsWith(query)
                        select i.UserName).Distinct().ToArray();            
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }





        /* <!--  Add new Owner --> */
        [HttpPost]
        public JsonResult AddNewOwner(int ProjectId, string OName, string ODesc)
        {
            if (ProjectId > 0) {
                var qry = (from e in db.EndUsers
                            where (e.UserName == OName)
                            select new { e.UID, e.UserName }).FirstOrDefault();

                if (qry != null) {
                    C018_coOwners co = new C018_coOwners();
                    co.ProjectId        = ProjectId;
                    co.UserId           = qry.UID;                    
                    co.OwnerContribution= ODesc;
                    co.CreatedBy        = UserIdentity.UserId;
                    co.CreatedDate      = DateTime.Now.AddHours(4);
                    db.C018_coOwners.Add(co);
                    db.SaveChanges();
                }
            }

            var q = (from o in db.C018_coOwners join 
                      u in db.EndUsers on o.UserId equals u.UID
                        where (o.ProjectId==ProjectId)
                            select new { o.CoOwnerId, u.UserName, o.OwnerContribution }).ToList();
            return Json(new { data = q });
            
        }

        /* <!--  Remove User --> */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OwnerRemove(int id)
        {
            int ProjectId= 0;
                C018_coOwners co    = db.C018_coOwners.Find(id);
                ProjectId           = co.ProjectId;
                db.C018_coOwners.Remove(co);
                db.SaveChanges();
            return RedirectToAction("Edit", "Project", new { id = ProjectId });
        }


        /* <!--  Add New file --> */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFile(FormCollection formCollection)
        {
            HttpPostedFileBase file = Request.Files["attachment"];
            string hdnid = (Request.Form["ProjectId"] != null) ? Request.Form["ProjectId"].ToString() : "0";            

            if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                
                C019_Attachments ath= new C019_Attachments();
                ath.ProjectId       = Convert.ToInt32(hdnid);
                ath.AName           = file.FileName;
                ath.AContentType    = file.ContentType;
                ath.ASize           = file.ContentLength;
                ath.AContent        = fileBytes;
                ath.CreatedBy       = UserIdentity.UserId;
                ath.CreatedDate     = DateTime.Now;
                ath.IsActive        = true;
                db.C019_Attachments.Add(ath);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", "Project", new { id = hdnid });
            //return View();
        }


        /* <!--  download file --> */
        [HttpGet]
        public ActionResult Download(int id)
        {

            /*
             *   share this as the download link
             <a href="@Url.Action("DownloadDocument", "Documents", new { id = "10003" })" >download</a>
             * 
             */
            string filename = "";
            string contenttype = "";
                        
            var qry = (from a in db.C019_Attachments
                       where a.AttachId == id
                       select new { a.AName, a.AContentType, a.AContent }).FirstOrDefault();
            filename = qry.AName;
            contenttype = qry.AContentType;
            var documentfile = qry.AContent;



            MemoryStream ms = new MemoryStream(documentfile, 0, 0, true, true);
            Response.ContentType = contenttype; // "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, contenttype); // "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        /* <!--  Remove file --> */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id)
        {
            int ProjectId = 0;

            C019_Attachments  at = db.C019_Attachments.Find(id);
            ProjectId  = at.ProjectId;
            db.C019_Attachments.Remove(at);
            db.SaveChanges();

            return RedirectToAction("Edit", "Project", new { id = ProjectId });
        }


    }
}
