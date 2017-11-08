﻿using System;
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

        
        public JsonResult GetBucketNames(string query) {
            string[] names = null;
            if (String.IsNullOrEmpty(query) == false) {                
                names = (from i in db.C007_BUCKET
                         where i.Name.StartsWith(query)
                            select i.Name).Distinct().ToArray();            
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }



        // GET: Bucket
        public ActionResult Index(int? PhaseId)
        {
            if (PhaseId.HasValue) {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", PhaseId.Value);
            }
            else {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName");
            }

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true).OrderBy(p => p.ProjectName), "ProjectId", "ProjectName");
            var q = db.Database.SqlQuery<SP_BUCKET_LIST_Result>("SP_BUCKET_LIST").Where(x => x.PhaseId == ((PhaseId.HasValue) ?PhaseId.Value:0)).ToList();
            return View(q);
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
            ViewBag.SubPhaseName = Bhai.getSubPhase(c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT     , "ProjectId"   , "ProjectName");
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE       , "PhaseId"     , "PhaseName");
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase    , "SubPhaseId"  , "SubPhaseName");
            return View();
        }

        // POST: Bucket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseId,SubPhaseId,Name")] C007_BUCKET c007_BUCKET) {
            
            if ((ModelState.IsValid) || (ModelState.IsValid==false && c007_BUCKET.SubPhaseId==0)) {
                c007_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c007_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c007_BUCKET.IsActive        = true;

                db.C007_BUCKET.Add(c007_BUCKET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId       = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName" , c007_BUCKET.ProjectId);
            ViewBag.PhaseId         = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"   , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId      = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName", c007_BUCKET.SubPhaseId);
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
            if (c007_BUCKET == null) {
                return HttpNotFound();
            }

            if (id.HasValue) {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == c007_BUCKET.ProjectId)
                         select new { c.StartDate, c.EndDate }).FirstOrDefault();
                ViewBag.EndDate     = q.EndDate     .ToString("dd-MMM-yyyy");
                ViewBag.StartDate   = q.StartDate   .ToString("dd-MMM-yyyy");

                var ph = (from p in db.C005_PHASE
                         where (p.IsActive.Equals(true)) && (p.PhaseId == c007_BUCKET.PhaseId)
                         select new { p.StartDate, p.EndDate }).FirstOrDefault();
                ViewBag.PhEndDate   = ph.EndDate    .ToString("dd-MMM-yyyy");
                ViewBag.phStartDate = ph.StartDate  .ToString("dd-MMM-yyyy");
            }
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName" , c007_BUCKET.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"   , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName", c007_BUCKET.SubPhaseId);
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
                c007_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c007_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c007_BUCKET.IsActive        = true;

                db.Entry(c007_BUCKET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName"     , c007_BUCKET.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"       , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName"    , c007_BUCKET.SubPhaseId);
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
            ViewBag.SubPhaseName = Bhai.getSubPhase(c007_BUCKET.SubPhaseId);
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
