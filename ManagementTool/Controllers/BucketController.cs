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
    public class BucketController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Bucket
        public ActionResult Index()
        {
            var c004_BUCKET = db.C004_BUCKET.Include(c => c.C001_PROJECT).Include(c => c.C002_PHASE).Include(c => c.C003_SUB_PHASE).Include(c => c.EndUser);
            return View(c004_BUCKET.ToList());
        }

        // GET: Bucket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_BUCKET c004_BUCKET = db.C004_BUCKET.Find(id);
            if (c004_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c004_BUCKET);
        }

        // GET: Bucket/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName");
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName");
            ViewBag.SubPhaseId = new SelectList(db.C003_SUB_PHASE, "SubPhaseId", "SubPhaseName");            
            return View();
        }

        // POST: Bucket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseId,SubPhaseId,Name")] C004_BUCKET c004_BUCKET)
        {
            if (ModelState.IsValid)
            {
                c004_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c004_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c004_BUCKET.IsActive        = true;

                db.C004_BUCKET.Add(c004_BUCKET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c004_BUCKET.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName", c004_BUCKET.PhaseId);
            ViewBag.SubPhaseId = new SelectList(db.C003_SUB_PHASE, "SubPhaseId", "SubPhaseName", c004_BUCKET.SubPhaseId);            
            return View(c004_BUCKET);
        }

        // GET: Bucket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_BUCKET c004_BUCKET = db.C004_BUCKET.Find(id);
            if (c004_BUCKET == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c004_BUCKET.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C002_PHASE, "PhaseId", "PhaseName", c004_BUCKET.PhaseId);
            ViewBag.SubPhaseId = new SelectList(db.C003_SUB_PHASE, "SubPhaseId", "SubPhaseName", c004_BUCKET.SubPhaseId);            
            return View(c004_BUCKET);
        }

        // POST: Bucket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BucketId,ProjectId,PhaseId,SubPhaseId,Name")] C004_BUCKET c004_BUCKET)
        {
            if (ModelState.IsValid)
            {
                c004_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c004_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c004_BUCKET.IsActive        = true;


                db.Entry(c004_BUCKET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId   = new SelectList(db.C001_PROJECT    , "ProjectId"   , "ProjectName" , c004_BUCKET.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C002_PHASE      , "PhaseId"     , "PhaseName"   , c004_BUCKET.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C003_SUB_PHASE  , "SubPhaseId"  , "SubPhaseName", c004_BUCKET.SubPhaseId);
            return View(c004_BUCKET);
        }

        // GET: Bucket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_BUCKET c004_BUCKET = db.C004_BUCKET.Find(id);
            if (c004_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c004_BUCKET);
        }

        // POST: Bucket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C004_BUCKET c004_BUCKET = db.C004_BUCKET.Find(id);
            db.C004_BUCKET.Remove(c004_BUCKET);
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
