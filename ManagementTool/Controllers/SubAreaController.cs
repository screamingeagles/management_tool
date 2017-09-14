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
    public class SubAreaController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: SubArea
        public ActionResult Index()
        {
            var c003_SUB_AREA = db.C003_SUB_AREA.Include(c => c.C002_AREA).Include(c => c.EndUser);
            return View(c003_SUB_AREA.ToList());
        }

        // GET: SubArea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Create
        public ActionResult Create()
        {            
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName");
            return View();
        }

        // POST: SubArea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AreaId,SubAreaName")] C003_SUB_AREA c003_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                c003_SUB_AREA.GeneratedBy   = UserIdentity.UserId;
                c003_SUB_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c003_SUB_AREA.AreaActive    = true;
                db.C003_SUB_AREA.Add(c003_SUB_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // POST: SubArea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubAreaId,AreaId,SubAreaName")] C003_SUB_AREA c003_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                c003_SUB_AREA.GeneratedBy   = UserIdentity.UserId;
                c003_SUB_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c003_SUB_AREA.AreaActive    = true;

                db.Entry(c003_SUB_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c003_SUB_AREA);
        }

        // POST: SubArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            db.C003_SUB_AREA.Remove(c003_SUB_AREA);
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
