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
    public class SubPhaseController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetSubPhase(int PhaseId)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C006_SubPhase
                         where (c.PhaseId == PhaseId)
                         select new { c.SubPhaseId, c.SubPhaseName}).ToList();
                return Json(new { data = q });
            }
        }



        // GET: SubPhase
        public ActionResult Index()
        {
            var c006_SubPhase = db.C006_SubPhase.Include(c => c.C005_PHASE).Include(c => c.EndUser);
            return View(c006_SubPhase.ToList());
        }

        // GET: SubPhase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Create
        public ActionResult Create()
        {
            //UserIdentity.UserId     = 1020;
            //UserIdentity.UserName   = "Arsalan Ahmed";

            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName");
            return View();
        }

        // POST: SubPhase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhaseId,SubPhaseName,StartDate,EndDate")] C006_SubPhase c006_SubPhase)
        {
            if (ModelState.IsValid)
            {
                c006_SubPhase.GeneratedBy = UserIdentity.UserId;
                c006_SubPhase.GeneratedDate = DateTime.Now.AddHours(4);                

                db.C006_SubPhase.Add(c006_SubPhase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // POST: SubPhase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubPhaseId,PhaseId,SubPhaseName,StartDate,EndDate")] C006_SubPhase c006_SubPhase)
        {
            if (ModelState.IsValid)
            {
                c006_SubPhase.GeneratedBy   = UserIdentity.UserId;
                c006_SubPhase.GeneratedDate = DateTime.Now.AddHours(4);

                db.Entry(c006_SubPhase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }
            return View(c006_SubPhase);
        }

        // POST: SubPhase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            db.C006_SubPhase.Remove(c006_SubPhase);
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
