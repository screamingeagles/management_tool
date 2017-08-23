﻿using System;
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
    public class DivisionController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Division
        public ActionResult Index()
        {
            return View(db.C006_DIVISION.ToList());
        }

        // GET: Division/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_DIVISION c006_DIVISION = db.C006_DIVISION.Find(id);
            if (c006_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c006_DIVISION);
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
        public ActionResult Create([Bind(Include = "DivisionId,DivisionName,CreatedBy,CreatedDatetime,IsActive")] C006_DIVISION c006_DIVISION)
        {
            if (ModelState.IsValid)
            {
                db.C006_DIVISION.Add(c006_DIVISION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c006_DIVISION);
        }

        // GET: Division/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_DIVISION c006_DIVISION = db.C006_DIVISION.Find(id);
            if (c006_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c006_DIVISION);
        }

        // POST: Division/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DivisionId,DivisionName,CreatedBy,CreatedDatetime,IsActive")] C006_DIVISION c006_DIVISION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c006_DIVISION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c006_DIVISION);
        }

        // GET: Division/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_DIVISION c006_DIVISION = db.C006_DIVISION.Find(id);
            if (c006_DIVISION == null)
            {
                return HttpNotFound();
            }
            return View(c006_DIVISION);
        }

        // POST: Division/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C006_DIVISION c006_DIVISION = db.C006_DIVISION.Find(id);
            db.C006_DIVISION.Remove(c006_DIVISION);
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