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
    public class PhaseController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetProjectDates(int SelectedProject)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == SelectedProject)
                         select new { c.StartDate, c.EndDate}).FirstOrDefault();
                return Json(new { data = q });
            }
        }


        // GET: Phase
        public ActionResult Index()
        {
            var c005_PHASE = db.C005_PHASE.Include(c => c.EndUser);
            return View(c005_PHASE.ToList());
        }

        // GET: Phase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c005_PHASE);
        }

        // GET: Phase/Create
        public ActionResult Create()
        {
            // UserIdentity.UserId     = 1020;
            // UserIdentity.UserName   = "Arsalan";


            ViewBag.ProjectId       = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName");
            return View();
        }

        // POST: Phase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseName,StartDate,EndDate")] C005_PHASE c005_PHASE)
        {
            if (ModelState.IsValid)
            {
                c005_PHASE.GeneratedBy      = UserIdentity.UserId;
                c005_PHASE.GeneratedDate    = DateTime.Now.AddHours(4);
                c005_PHASE.IsActive         = true;

                db.C005_PHASE.Add(c005_PHASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName");
            return View(c005_PHASE);
        }

        // GET: Phase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null) {
                return HttpNotFound();
            }

            ViewBag.EndDate     = "";
            ViewBag.StartDate   = "";
            if (id.HasValue)
            {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == id.Value)
                         select new { c.StartDate, c.EndDate }).FirstOrDefault();
                ViewBag.EndDate     = q.EndDate.ToString("dd-MMM-yyyy");
                ViewBag.StartDate   = q.StartDate.ToString("dd-MMM-yyyy");
            }
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName", c005_PHASE.PhaseId);
            return View(c005_PHASE);
        }

        // POST: Phase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhaseId,ProjectId,PhaseName,StartDate,EndDate")] C005_PHASE c005_PHASE)
        {
            if (ModelState.IsValid)
            {
                c005_PHASE.GeneratedBy = UserIdentity.UserId;
                c005_PHASE.GeneratedDate = DateTime.Now.AddHours(4);
                c005_PHASE.IsActive = true;

                db.Entry(c005_PHASE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var q = (from c in db.C004_PROJECT
                        where (c.IsActive.Equals(true)) && (c.ProjectId == c005_PHASE.ProjectId)
                        select new { c.StartDate, c.EndDate }).FirstOrDefault();
            ViewBag.EndDate = q.EndDate.ToString("dd-MMM-yyyy");
            ViewBag.StartDate = q.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName", c005_PHASE.PhaseId);
            return View(c005_PHASE);
        }

        // GET: Phase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c005_PHASE);
        }

        // POST: Phase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            db.C005_PHASE.Remove(c005_PHASE);
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
