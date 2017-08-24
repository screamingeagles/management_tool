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
    public class TypeController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Type
        public ActionResult Index()
        {
            return View(db.C009_TYPE.ToList());
        }

        // GET: Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C009_TYPE c009_TYPE = db.C009_TYPE.Find(id);
            if (c009_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(c009_TYPE);
        }

        // GET: Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeId,TypeName,CreatedBy,CreatedDate,IsActive")] C009_TYPE c009_TYPE)
        {
            if (ModelState.IsValid)
            {
                db.C009_TYPE.Add(c009_TYPE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c009_TYPE);
        }

        // GET: Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C009_TYPE c009_TYPE = db.C009_TYPE.Find(id);
            if (c009_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(c009_TYPE);
        }

        // POST: Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeId,TypeName,CreatedBy,CreatedDate,IsActive")] C009_TYPE c009_TYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c009_TYPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c009_TYPE);
        }

        // GET: Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C009_TYPE c009_TYPE = db.C009_TYPE.Find(id);
            if (c009_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(c009_TYPE);
        }

        // POST: Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C009_TYPE c009_TYPE = db.C009_TYPE.Find(id);
            db.C009_TYPE.Remove(c009_TYPE);
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
