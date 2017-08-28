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
    public class BucketsController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Buckets
        public ActionResult Index()
        {
            return View(db.C015_BUCKET.ToList());
        }


        // GET: Buckets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C015_BUCKET c015_BUCKET = db.C015_BUCKET.Find(id);
            if (c015_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c015_BUCKET);
        }

        // GET: Buckets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buckets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BucketId,Name,StartDate,EndDate,BucketType,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] C015_BUCKET c015_BUCKET)
        {
            if (ModelState.IsValid)
            {
                db.C015_BUCKET.Add(c015_BUCKET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c015_BUCKET);
        }

        // GET: Buckets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C015_BUCKET c015_BUCKET = db.C015_BUCKET.Find(id);
            if (c015_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c015_BUCKET);
        }

        // POST: Buckets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BucketId,Name,StartDate,EndDate,BucketType,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] C015_BUCKET c015_BUCKET)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c015_BUCKET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c015_BUCKET);
        }

        // GET: Buckets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C015_BUCKET c015_BUCKET = db.C015_BUCKET.Find(id);
            if (c015_BUCKET == null)
            {
                return HttpNotFound();
            }
            return View(c015_BUCKET);
        }

        // POST: Buckets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C015_BUCKET c015_BUCKET = db.C015_BUCKET.Find(id);
            db.C015_BUCKET.Remove(c015_BUCKET);
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
