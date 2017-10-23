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
    public class AreaController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Area
        public ActionResult Index()
        {
            var c002_AREA = db.C002_AREA.Include(c => c.C001_DIVISION).Include(c => c.EndUser).OrderBy(c => c.C001_DIVISION.DivisionName).ThenBy(c => c.AreaName);
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
            return View();
        }

        // POST: Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionId,AreaName")] C002_AREA c002_AREA)
        {
            if (ModelState.IsValid)
            {
                c002_AREA.GeneratedBy   = UserIdentity.UserId;
                c002_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c002_AREA.isActive      = true;

                db.C002_AREA.Add(c002_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c002_AREA.DivisionId);
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
            return View(c002_AREA);
        }

        // POST: Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AreaId,DivisionId,AreaName")] C002_AREA c002_AREA)
        {
            if (ModelState.IsValid)
            {
                c002_AREA.GeneratedBy   = UserIdentity.UserId;
                c002_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c002_AREA.isActive      = true;

                db.Entry(c002_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisionId = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c002_AREA.DivisionId);
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
