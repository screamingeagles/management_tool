using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementTool.Models;

namespace ManagementTool.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Project
        public ActionResult Index()
        {
            var c003_PROJECT = db.C003_PROJECT.Include(c => c.C001_DIVISION).Include(c => c.C002_AREA).Include(c => c.EndUser).Include(c => c.C011_COMPANY).Include(c => c.C010_LOCATION).Include(c => c.C013_PROJECT_TYPE);
            return View(c003_PROJECT.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_PROJECT c003_PROJECT = db.C003_PROJECT.Find(id);
            if (c003_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c003_PROJECT);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName");
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName");
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName");
            ViewBag.CompanyId = new SelectList(db.C011_COMPANY, "CompanyId", "CompanyName");
            ViewBag.LocationId = new SelectList(db.C010_LOCATION, "LocationId", "LocationName");
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,LocationId,CompanyId,DivisionId,AreaId,ProjectName,ProjectType,StartDate,EndDate,GeneratedBy,GeneratedDate,IsActive")] C003_PROJECT c003_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.C003_PROJECT.Add(c003_PROJECT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c003_PROJECT.DivisionId);
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_PROJECT.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c003_PROJECT.GeneratedBy);
            ViewBag.CompanyId = new SelectList(db.C011_COMPANY, "CompanyId", "CompanyName", c003_PROJECT.CompanyId);
            ViewBag.LocationId = new SelectList(db.C010_LOCATION, "LocationId", "LocationName", c003_PROJECT.LocationId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c003_PROJECT.ProjectType);
            return View(c003_PROJECT);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_PROJECT c003_PROJECT = db.C003_PROJECT.Find(id);
            if (c003_PROJECT == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c003_PROJECT.DivisionId);
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_PROJECT.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c003_PROJECT.GeneratedBy);
            ViewBag.CompanyId = new SelectList(db.C011_COMPANY, "CompanyId", "CompanyName", c003_PROJECT.CompanyId);
            ViewBag.LocationId = new SelectList(db.C010_LOCATION, "LocationId", "LocationName", c003_PROJECT.LocationId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c003_PROJECT.ProjectType);
            return View(c003_PROJECT);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,LocationId,CompanyId,DivisionId,AreaId,ProjectName,ProjectType,StartDate,EndDate,GeneratedBy,GeneratedDate,IsActive")] C003_PROJECT c003_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c003_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c003_PROJECT.DivisionId);
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_PROJECT.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c003_PROJECT.GeneratedBy);
            ViewBag.CompanyId = new SelectList(db.C011_COMPANY, "CompanyId", "CompanyName", c003_PROJECT.CompanyId);
            ViewBag.LocationId = new SelectList(db.C010_LOCATION, "LocationId", "LocationName", c003_PROJECT.LocationId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c003_PROJECT.ProjectType);
            return View(c003_PROJECT);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_PROJECT c003_PROJECT = db.C003_PROJECT.Find(id);
            if (c003_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c003_PROJECT);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C003_PROJECT c003_PROJECT = db.C003_PROJECT.Find(id);
            db.C003_PROJECT.Remove(c003_PROJECT);
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
