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
    public class LocationController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Location
        public ActionResult Index()
        {
            return View(db.C004_LOCATION.ToList());
        }

        // GET: Location/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_LOCATION c004_LOCATION = db.C004_LOCATION.Find(id);
            if (c004_LOCATION == null)
            {
                return HttpNotFound();
            }
            return View(c004_LOCATION);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,LocationName,CreatedBy,CreatedDatetime,IsActive")] C004_LOCATION c004_LOCATION)
        {
            if (ModelState.IsValid)
            {
                db.C004_LOCATION.Add(c004_LOCATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c004_LOCATION);
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_LOCATION c004_LOCATION = db.C004_LOCATION.Find(id);
            if (c004_LOCATION == null)
            {
                return HttpNotFound();
            }
            return View(c004_LOCATION);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocationId,LocationName,CreatedBy,CreatedDatetime,IsActive")] C004_LOCATION c004_LOCATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c004_LOCATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c004_LOCATION);
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_LOCATION c004_LOCATION = db.C004_LOCATION.Find(id);
            if (c004_LOCATION == null)
            {
                return HttpNotFound();
            }
            return View(c004_LOCATION);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C004_LOCATION c004_LOCATION = db.C004_LOCATION.Find(id);
            db.C004_LOCATION.Remove(c004_LOCATION);
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
