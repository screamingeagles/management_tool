using System;
using System.Web;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Common;
using ManagementTool.Models;
using ManagementTool.Helpers;
using System.Collections.Generic;



namespace ManagementTool.Controllers
{
    public class CommitmentController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult AddNewMaster  (string CommitmentMaster)                                                           {

            if (string.IsNullOrEmpty(CommitmentMaster) == false) {
                C020_CommitmentMaster cm    = new C020_CommitmentMaster();
                cm.UserId                   = UserIdentity.UserId;
                cm.CommitmentHeader         = CommitmentMaster;                                
                cm.IsActive                 = true;
                cm.GeneratedBy              = UserIdentity.UserId;
                cm.GeneratedDate            = DateTime.Now.AddHours(4);
                db.C020_CommitmentMaster.Add(cm);
                db.SaveChanges();                
            }

            var q = (from dt in db.C020_CommitmentMaster
                     where (dt.UserId == UserIdentity.UserId) && (dt.IsActive == true)
                     select new { dt.CommitmentId, dt.CommitmentHeader }).ToList();

            return Json(new { data = q});
        }

        [HttpPost]
        public JsonResult AddNew        (int CommitmentID, string TCommit, string TDesc, string TRemarks)    
        {
            if (CommitmentID > 0) {
                C021_CommimentDetails cm= new C021_CommimentDetails();
                cm.CommitmentId         = CommitmentID;
                cm.CommimentName        = TCommit;
                cm.CDescription         = TDesc;
                cm.CRemarks             = TRemarks;
                cm.ProjectId            = 0; // nothing new
                cm.IsActive             = true;
                cm.GeneratedBy          = UserIdentity.UserId;
                cm.GeneratedDate        = DateTime.Now.AddHours(4);
                db.C021_CommimentDetails.Add(cm);
                db.SaveChanges();
                CommitmentID = cm.DetailId;
            }
            return Json(new { data = "Saved : " + CommitmentID });
        }


        // GET: Commitment
        public ActionResult Index()
        {
            return RedirectToAction("Create");
            //var c020_CommitmentMaster = db.C020_CommitmentMaster.Include(c => c.EndUser);
            //return View(c020_CommitmentMaster.ToList());
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
        public ActionResult Create() {
            int Userid = UserIdentity.UserId; //= 1020;
            //UserIdentity.UserName       = "Arsalan Ahmed";
            List<commitment_service> cs = commitment_service.GetUserCommitment(Userid);

            ViewBag.UserName            = UserIdentity.UserName;
            ViewBag.ProjectId           = new SelectList(db.C004_PROJECT            .Where(p => p.IsActive == true).OrderBy(p => p.ProjectName     ), "ProjectId", "ProjectName");
            ViewBag.CommitmentId        = new SelectList(db.C020_CommitmentMaster   .Where(x => x.IsActive == true).OrderBy(x => x.CommitmentHeader), "CommitmentId", "CommitmentHeader");
            return View(cs);
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

        
        [HttpPost]
        public JsonResult DeleteCommitment(int CommitmentDetail) {
            if (CommitmentDetail > 0 ) {
                C021_CommimentDetails cdt = db.C021_CommimentDetails.Find(CommitmentDetail);
                db.C021_CommimentDetails.Remove(cdt);
                db.SaveChanges();                
            }
            return Json(new { data = true });
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
