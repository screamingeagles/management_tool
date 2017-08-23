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
    public class AreaController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Area
        public ActionResult Index()
        {
            return View(db.C007_AREA.ToList());
        }

        // GET: Area/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_AREA c007_AREA = db.C007_AREA.Find(id);
            if (c007_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c007_AREA);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AreaId,DivisionId,AreaName,CreatedBy,CreatedDatetime,isActive")] C007_AREA c007_AREA)
        {
            if (ModelState.IsValid)
            {
                db.C007_AREA.Add(c007_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c007_AREA);
        }

        // GET: Area/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_AREA c007_AREA = db.C007_AREA.Find(id);
            if (c007_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c007_AREA);
        }

        // POST: Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AreaId,DivisionId,AreaName,CreatedBy,CreatedDatetime,isActive")] C007_AREA c007_AREA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c007_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c007_AREA);
        }

        // GET: Area/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_AREA c007_AREA = db.C007_AREA.Find(id);
            if (c007_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c007_AREA);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C007_AREA c007_AREA = db.C007_AREA.Find(id);
            db.C007_AREA.Remove(c007_AREA);
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
