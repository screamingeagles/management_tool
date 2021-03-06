﻿using System;
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
    public class DivisionController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Division
        public ActionResult Index()
        {
            return View(db.C001_DIVISION.Where(d => d.IsActive == true).Include(c => c.EndUser).ToList());
        }

        // GET: Division/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_DIVISION c001_DIVISION = db.C001_DIVISION.Find(id);
            if (c001_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c001_DIVISION);
        }

        // GET: Division/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Division/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionName")] C001_DIVISION c001_DIVISION)
        {
            if (ModelState.IsValid)
            {
                c001_DIVISION.GeneratedBy   = UserIdentity.UserId;
                c001_DIVISION.GeneratedDate = DateTime.Now.AddHours(4);
                c001_DIVISION.IsActive      = true;
                db.C001_DIVISION.Add(c001_DIVISION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c001_DIVISION);
        }

        // GET: Division/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_DIVISION c001_DIVISION = db.C001_DIVISION.Find(id);
            if (c001_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c001_DIVISION);
        }

        // POST: Division/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DivisionId,DivisionName")] C001_DIVISION c001_DIVISION)
        {
            if (ModelState.IsValid)
            {
                c001_DIVISION.GeneratedBy       = UserIdentity.UserId;
                c001_DIVISION.GeneratedDate     = DateTime.Now.AddHours(4);
                c001_DIVISION.IsActive          = true;
                db.Entry(c001_DIVISION).State   = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c001_DIVISION);
        }

        // GET: Division/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_DIVISION c001_DIVISION = db.C001_DIVISION.Find(id);
            if (c001_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c001_DIVISION);
        }

        // POST: Division/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C001_DIVISION c001_DIVISION = db.C001_DIVISION.Find(id);
            db.C001_DIVISION.Remove(c001_DIVISION);
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
