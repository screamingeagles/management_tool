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


namespace ManagementTool.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Project
        public ActionResult Index()
        {
            int _uId = 0;
            int _sid = 0;
                _sid = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            if (_sid == 0) { return RedirectToAction("Index", "Login", new { x = 2 }); }
                _uId = Bhai.GetUserIdFromSession(_sid);


            var c001_PROJECT = db.C001_PROJECT.Include(c => c.C008_COMPANY).Include(c => c.C009_DIVISION).Include(c => c.C007_LOCATION).Include(c => c.C013_PROJECT_TYPE);
            return View(c001_PROJECT.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c001_PROJECT);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.C007_LOCATION, "LocationId", "LocationName");
            ViewBag.CompanyId = new SelectList(db.C008_COMPANY, "CompanyId", "CompanyName");
            ViewBag.DivisionId = new SelectList(db.C009_DIVISION, "DivisionId", "DivisionName");
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType");
            return View();
        }


        [HttpPost]
        public JsonResult GetCompaniesbyLocation(int SelectedLocation)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C008_COMPANY
                         where (c.IsActive.Equals(true)) && (c.LocationId == SelectedLocation)
                         select new { c.CompanyId, c.CompanyName}).ToList();
                return Json(new { data = q });
            }
        }



        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectName,LocationId,CompanyId,DivisionId,ProjectType,StartDate,EndDate")] C001_PROJECT c001_PROJECT)
        {
            int _uId = 0;
            int _sid = 0;
                _sid = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            if (_sid == 0) { return RedirectToAction("Index", "Login", new { x = 2 }); }
                _uId = Bhai.GetUserIdFromSession(_sid);

            if (ModelState.IsValid)
            {
                c001_PROJECT.GeneratedBy = _uId;
                c001_PROJECT.GeneratedDate = DateTime.Now.AddHours(4);
                c001_PROJECT.IsActive = true;
                db.C001_PROJECT.Add(c001_PROJECT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.C007_LOCATION, "LocationId", "LocationName", c001_PROJECT.LocationId);
            ViewBag.CompanyId = new SelectList(db.C008_COMPANY, "CompanyId", "CompanyName", c001_PROJECT.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C009_DIVISION, "DivisionId", "DivisionName", c001_PROJECT.DivisionId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c001_PROJECT.ProjectType);
            return View(c001_PROJECT);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.C008_COMPANY, "CompanyId", "CompanyName", c001_PROJECT.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C009_DIVISION, "DivisionId", "DivisionName", c001_PROJECT.DivisionId);
            ViewBag.LocationId = new SelectList(db.C007_LOCATION, "LocationId", "LocationName", c001_PROJECT.LocationId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c001_PROJECT.ProjectType);
            return View(c001_PROJECT);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectName,LocationId,CompanyId,DivisionId,ProjectType,StartDate,EndDate")] C001_PROJECT c001_PROJECT)
        {
            int _uId = 0;
            int _sid = 0;
                _sid = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            if (_sid == 0) { return RedirectToAction("Index", "Login", new { x = 2 }); }
                _uId = Bhai.GetUserIdFromSession(_sid);
            
            if (ModelState.IsValid)
            {
                c001_PROJECT.GeneratedBy    = _uId;
                c001_PROJECT.GeneratedDate  = DateTime.Now.AddHours(4);
                c001_PROJECT.IsActive       = true;

                db.Entry(c001_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.C008_COMPANY, "CompanyId", "CompanyName", c001_PROJECT.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C009_DIVISION, "DivisionId", "DivisionName", c001_PROJECT.DivisionId);
            ViewBag.LocationId = new SelectList(db.C007_LOCATION, "LocationId", "LocationName", c001_PROJECT.LocationId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c001_PROJECT.ProjectType);
            return View(c001_PROJECT);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c001_PROJECT);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            db.C001_PROJECT.Remove(c001_PROJECT);
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
    }
}
