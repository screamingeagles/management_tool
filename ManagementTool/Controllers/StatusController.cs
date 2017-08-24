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
    public class StatusController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Status
        public ActionResult Index()
        {
            return View(db.C010_STATUS.ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C010_STATUS c010_STATUS = db.C010_STATUS.Find(id);
            if (c010_STATUS == null)
            {
                return HttpNotFound();
            }
            return View(c010_STATUS);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusId,TaskStatus,TaskActive,TaskCreatedBy,TaskCreatedDate")] C010_STATUS c010_STATUS)
        {
            if (ModelState.IsValid)
            {
                db.C010_STATUS.Add(c010_STATUS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c010_STATUS);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C010_STATUS c010_STATUS = db.C010_STATUS.Find(id);
            if (c010_STATUS == null)
            {
                return HttpNotFound();
            }
            return View(c010_STATUS);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusId,TaskStatus,TaskActive,TaskCreatedBy,TaskCreatedDate")] C010_STATUS c010_STATUS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c010_STATUS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c010_STATUS);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C010_STATUS c010_STATUS = db.C010_STATUS.Find(id);
            if (c010_STATUS == null)
            {
                return HttpNotFound();
            }
            return View(c010_STATUS);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C010_STATUS c010_STATUS = db.C010_STATUS.Find(id);
            db.C010_STATUS.Remove(c010_STATUS);
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
