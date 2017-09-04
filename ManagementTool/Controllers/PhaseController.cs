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
    public class PhaseController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Phase
        public ActionResult Index()
        {
            var c002_PHASE = db.C002_PHASE.Include(c => c.C001_PROJECT).Include(c => c.EndUser);
            return View(c002_PHASE.ToList());
        }

        // GET: Phase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_PHASE c002_PHASE = db.C002_PHASE.Find(id);
            if (c002_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c002_PHASE);
        }

        // GET: Phase/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName");
            ViewBag.PhaseId = new SelectList(db.EndUsers, "UID", "UserName");
            return View();
        }

        // POST: Phase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhaseId,ProjectId,PhaseName,StartDate,EndDate,IsActive,GeneratedDate,GeneratedBy")] C002_PHASE c002_PHASE)
        {
            if (ModelState.IsValid)
            {
                db.C002_PHASE.Add(c002_PHASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.EndUsers, "UID", "UserName", c002_PHASE.PhaseId);
            return View(c002_PHASE);
        }

        // GET: Phase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_PHASE c002_PHASE = db.C002_PHASE.Find(id);
            if (c002_PHASE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.EndUsers, "UID", "UserName", c002_PHASE.PhaseId);
            return View(c002_PHASE);
        }

        // POST: Phase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhaseId,ProjectId,PhaseName,StartDate,EndDate,IsActive,GeneratedDate,GeneratedBy")] C002_PHASE c002_PHASE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c002_PHASE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.EndUsers, "UID", "UserName", c002_PHASE.PhaseId);
            return View(c002_PHASE);
        }

        // GET: Phase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_PHASE c002_PHASE = db.C002_PHASE.Find(id);
            if (c002_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c002_PHASE);
        }

        // POST: Phase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C002_PHASE c002_PHASE = db.C002_PHASE.Find(id);
            db.C002_PHASE.Remove(c002_PHASE);
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
