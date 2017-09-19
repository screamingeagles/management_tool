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
    public class BucketController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Bucket
        public ActionResult Index()
        {
            var c007_BUCKET = db.C007_BUCKET.Include(c => c.C004_PROJECT).Include(c => c.C005_PHASE).Include(c => c.EndUser);
            return View(c007_BUCKET.ToList());
        }

        // GET: Bucket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c007_BUCKET);
        }

        // GET: Bucket/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT, "ProjectId", "ProjectName");
            ViewBag.PhaseId = new SelectList(db.C005_PHASE, "PhaseId", "PhaseName");
            return View();
        }

        // POST: Bucket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseId,SubPhaseId,Name")] C007_BUCKET c007_BUCKET)
        {
            if (ModelState.IsValid)
            {
                db.C007_BUCKET.Add(c007_BUCKET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT, "ProjectId", "ProjectName", c007_BUCKET.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C005_PHASE, "PhaseId", "PhaseName", c007_BUCKET.PhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT, "ProjectId", "ProjectName", c007_BUCKET.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C005_PHASE, "PhaseId", "PhaseName", c007_BUCKET.PhaseId);
            return View(c007_BUCKET);
        }

        // POST: Bucket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BucketId,ProjectId,PhaseId,SubPhaseId,Name")] C007_BUCKET c007_BUCKET)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c007_BUCKET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT, "ProjectId", "ProjectName", c007_BUCKET.ProjectId);
            ViewBag.PhaseId = new SelectList(db.C005_PHASE, "PhaseId", "PhaseName", c007_BUCKET.PhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c007_BUCKET);
        }

        // POST: Bucket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            db.C007_BUCKET.Remove(c007_BUCKET);
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
