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
            var c002_AREA = db.C002_AREA.Include(c => c.C001_DIVISION).Include(c => c.EndUser);
            return View(c002_AREA.ToList());
        }

        // GET: Area/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_AREA c002_AREA = db.C002_AREA.Find(id);
            if (c002_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c002_AREA);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName");
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName");
            return View();
        }

        // POST: Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AreaId,DivisionId,AreaName,GeneratedBy,GeneratedDate,isActive")] C002_AREA c002_AREA)
        {
            if (ModelState.IsValid)
            {
                db.C002_AREA.Add(c002_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c002_AREA.DivisionId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c002_AREA.GeneratedBy);
            return View(c002_AREA);
        }

        // GET: Area/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_AREA c002_AREA = db.C002_AREA.Find(id);
            if (c002_AREA == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c002_AREA.DivisionId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c002_AREA.GeneratedBy);
            return View(c002_AREA);
        }

        // POST: Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AreaId,DivisionId,AreaName,GeneratedBy,GeneratedDate,isActive")] C002_AREA c002_AREA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c002_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c002_AREA.DivisionId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c002_AREA.GeneratedBy);
            return View(c002_AREA);
        }

        // GET: Area/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_AREA c002_AREA = db.C002_AREA.Find(id);
            if (c002_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c002_AREA);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C002_AREA c002_AREA = db.C002_AREA.Find(id);
            db.C002_AREA.Remove(c002_AREA);
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
