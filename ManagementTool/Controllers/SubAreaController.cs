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
    public class SubAreaController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: SubArea
        public ActionResult Index()
        {
            var c008_SUB_AREA = db.C008_SUB_AREA.Include(c => c.C007_AREA);
            return View(c008_SUB_AREA.ToList());
        }

        // GET: SubArea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_SUB_AREA c008_SUB_AREA = db.C008_SUB_AREA.Find(id);
            if (c008_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c008_SUB_AREA);
        }

        // GET: SubArea/Create
        public ActionResult Create()
        {
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName");
            return View();
        }

        // POST: SubArea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubAreaId,AreaId,SubAreaName,CreatedBy,CreatedDate,AreaActive")] C008_SUB_AREA c008_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                db.C008_SUB_AREA.Add(c008_SUB_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c008_SUB_AREA.AreaId);
            return View(c008_SUB_AREA);
        }

        // GET: SubArea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_SUB_AREA c008_SUB_AREA = db.C008_SUB_AREA.Find(id);
            if (c008_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c008_SUB_AREA.AreaId);
            return View(c008_SUB_AREA);
        }

        // POST: SubArea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubAreaId,AreaId,SubAreaName,CreatedBy,CreatedDate,AreaActive")] C008_SUB_AREA c008_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c008_SUB_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c008_SUB_AREA.AreaId);
            return View(c008_SUB_AREA);
        }

        // GET: SubArea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_SUB_AREA c008_SUB_AREA = db.C008_SUB_AREA.Find(id);
            if (c008_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c008_SUB_AREA);
        }

        // POST: SubArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C008_SUB_AREA c008_SUB_AREA = db.C008_SUB_AREA.Find(id);
            db.C008_SUB_AREA.Remove(c008_SUB_AREA);
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
