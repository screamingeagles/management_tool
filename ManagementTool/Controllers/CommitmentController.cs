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
    public class CommitmentController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Commitment
        public ActionResult Index()
        {
            var c020_CommitmentMaster = db.C020_CommitmentMaster.Include(c => c.EndUser);
            return View(c020_CommitmentMaster.ToList());
        }

        // GET: Commitment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C020_CommitmentMaster c020_CommitmentMaster = db.C020_CommitmentMaster.Find(id);
            if (c020_CommitmentMaster == null)
            {
                return HttpNotFound();
            }
            return View(c020_CommitmentMaster);
        }

        // GET: Commitment/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.EndUsers, "UID", "UserName");
            return View();
        }

        // POST: Commitment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommitmentHeader")] C020_CommitmentMaster c020_CommitmentMaster)
        {
            if (ModelState.IsValid)
            {
                db.C020_CommitmentMaster.Add(c020_CommitmentMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.EndUsers, "UID", "UserName", c020_CommitmentMaster.UserId);
            return View(c020_CommitmentMaster);
        }

        // GET: Commitment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C020_CommitmentMaster c020_CommitmentMaster = db.C020_CommitmentMaster.Find(id);
            if (c020_CommitmentMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.EndUsers, "UID", "UserName", c020_CommitmentMaster.UserId);
            return View(c020_CommitmentMaster);
        }

        // POST: Commitment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommitmentHeader")] C020_CommitmentMaster c020_CommitmentMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c020_CommitmentMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.EndUsers, "UID", "UserName", c020_CommitmentMaster.UserId);
            return View(c020_CommitmentMaster);
        }

        // GET: Commitment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C020_CommitmentMaster c020_CommitmentMaster = db.C020_CommitmentMaster.Find(id);
            if (c020_CommitmentMaster == null)
            {
                return HttpNotFound();
            }
            return View(c020_CommitmentMaster);
        }

        // POST: Commitment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C020_CommitmentMaster c020_CommitmentMaster = db.C020_CommitmentMaster.Find(id);
            db.C020_CommitmentMaster.Remove(c020_CommitmentMaster);
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
