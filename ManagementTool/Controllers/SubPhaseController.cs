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
    public class SubPhaseController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: SubPhase
        public ActionResult Index()
        {
            var c003_SUB_PHASE = db.C003_SUB_PHASE.Include(c => c.C001_PROJECT).Include(c => c.C002_PHASE).Include(c => c.EndUser);
            return View(c003_SUB_PHASE.ToList());
        }

        // GET: SubPhase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_PHASE c003_SUB_PHASE = db.C003_SUB_PHASE.Find(id);
            if (c003_SUB_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c003_SUB_PHASE);
        }

        // GET: SubPhase/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName");
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName");
            return View();
        }

        // POST: SubPhase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseId,SubPhaseName")] C003_SUB_PHASE c003_SUB_PHASE)
        {
            if (ModelState.IsValid)
            {
                c003_SUB_PHASE.GeneratedBy      = UserIdentity.UserId;
                c003_SUB_PHASE.GeneratedDate    = DateTime.Now.AddHours(4);
                c003_SUB_PHASE.IsActive         = true;

                db.C003_SUB_PHASE.Add(c003_SUB_PHASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c003_SUB_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName", c003_SUB_PHASE.PhaseId);            
            return View(c003_SUB_PHASE);
        }

        // GET: SubPhase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_PHASE c003_SUB_PHASE = db.C003_SUB_PHASE.Find(id);
            if (c003_SUB_PHASE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c003_SUB_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName", c003_SUB_PHASE.PhaseId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c003_SUB_PHASE.GeneratedBy);
            return View(c003_SUB_PHASE);
        }

        // POST: SubPhase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,PhaseId,SubPhaseId,SubPhaseName")] C003_SUB_PHASE c003_SUB_PHASE)
        {
            
            if (ModelState.IsValid)
            {
                c003_SUB_PHASE.GeneratedBy      = UserIdentity.UserId;
                c003_SUB_PHASE.GeneratedDate    = DateTime.Now.AddHours(4);
                c003_SUB_PHASE.IsActive         = true;

                db.Entry(c003_SUB_PHASE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c003_SUB_PHASE.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName", c003_SUB_PHASE.PhaseId);
            return View(c003_SUB_PHASE);
        }

        // GET: SubPhase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_PHASE c003_SUB_PHASE = db.C003_SUB_PHASE.Find(id);
            if (c003_SUB_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c003_SUB_PHASE);
        }

        // POST: SubPhase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C003_SUB_PHASE c003_SUB_PHASE = db.C003_SUB_PHASE.Find(id);
            db.C003_SUB_PHASE.Remove(c003_SUB_PHASE);
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
