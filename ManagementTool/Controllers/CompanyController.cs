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
    public class CompanyController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Company
        public ActionResult Index()
        {
            return View(db.C005_COMPANY.ToList());
        }

        // GET: Company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_COMPANY c005_COMPANY = db.C005_COMPANY.Find(id);
            if (c005_COMPANY == null)
            {
                return HttpNotFound();
            }
            return View(c005_COMPANY);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyId,LocationId,CompanyName,CreatedBy,CreatedDatetime,IsActive")] C005_COMPANY c005_COMPANY)
        {
            if (ModelState.IsValid)
            {
                db.C005_COMPANY.Add(c005_COMPANY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c005_COMPANY);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_COMPANY c005_COMPANY = db.C005_COMPANY.Find(id);
            if (c005_COMPANY == null)
            {
                return HttpNotFound();
            }
            return View(c005_COMPANY);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,LocationId,CompanyName,CreatedBy,CreatedDatetime,IsActive")] C005_COMPANY c005_COMPANY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c005_COMPANY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c005_COMPANY);
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_COMPANY c005_COMPANY = db.C005_COMPANY.Find(id);
            if (c005_COMPANY == null)
            {
                return HttpNotFound();
            }
            return View(c005_COMPANY);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C005_COMPANY c005_COMPANY = db.C005_COMPANY.Find(id);
            db.C005_COMPANY.Remove(c005_COMPANY);
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
