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
    public class BucketController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        
        public JsonResult GetBucketNames            (string query)              {
            string[] names = null;
            if (String.IsNullOrEmpty(query) == false) {                
                names = (from i in db.C007_BUCKET
                         where i.Name.StartsWith(query)
                            select i.Name).Distinct().ToArray();            
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetBucketListByCompany    (int CompanyId)             
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from b  in db.vw_BucketbyLCDASAP
                         join po in db.C004_PROJECT         on b.ProjectId  equals po.ProjectId
                         join ph in db.C005_PHASE           on b.PhaseId    equals ph.PhaseId
                         join sp in db.C006_SubPhase        on b.SubPhaseId equals sp.SubPhaseId
                         where (b.CompanyId == CompanyId)
                         select new { b.BucketId, po.ProjectName, ph.PhaseName, sp.SubPhaseName, b.BucketName, b.GeneratedUserName, b.GenerationDate }).ToList();

                return Json(new { data = q });

            }
        }

        [HttpPost]
        public JsonResult GetBucketListBySubArea    (int Area, int SubArea)     
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from b in db.vw_BucketbyLCDASAP
                         join po in db.C004_PROJECT on b.ProjectId equals po.ProjectId
                         join ph in db.C005_PHASE on b.PhaseId equals ph.PhaseId
                         join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         where (b.AreaId== Area) && (b.SubAreaId == SubArea)
                         select new { b.BucketId, po.ProjectName, ph.PhaseName, sp.SubPhaseName, b.BucketName, b.GeneratedUserName, b.GenerationDate }).ToList();


                var k = (from j in db.C004_PROJECT
                         join v in db.vw_PhasebyLCDASAP on j.ProjectId equals v.ProjectId
                         where (v.AreaId == Area) && (v.SubAreaId == SubArea)
                         select new { j.ProjectId, j.ProjectName }).ToList();

                return Json(new { data = k, list = q });

            }
        }

        [HttpPost]
        public JsonResult GetBucketListByProject    (int ProjectId)             
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from b in db.vw_BucketbyLCDASAP
                         join po in db.C004_PROJECT on b.ProjectId equals po.ProjectId
                         join ph in db.C005_PHASE on b.PhaseId equals ph.PhaseId
                         join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         where (b.ProjectId== ProjectId)
                         select new { b.BucketId, po.ProjectName, ph.PhaseName, sp.SubPhaseName, b.BucketName, b.GeneratedUserName, b.GenerationDate }).ToList();


                var pl = (from pr in db.C004_PROJECT
                          join ph in db.C005_PHASE on pr.ProjectId equals ph.ProjectId
                          where (pr.ProjectId == ProjectId)
                          select new { ph.PhaseId, ph.PhaseName }).ToList();
                return Json(new { data = q, list = pl });
            }
        }

        [HttpPost]
        public JsonResult GetBucketListByPhase      (int PhaseId)                {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from b in db.vw_BucketbyLCDASAP
                         join po in db.C004_PROJECT on b.ProjectId equals po.ProjectId
                         join ph in db.C005_PHASE on b.PhaseId equals ph.PhaseId
                         join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         where (b.PhaseId == PhaseId)
                         select new { b.BucketId, po.ProjectName, ph.PhaseName, sp.SubPhaseName, b.BucketName, b.GeneratedUserName, b.GenerationDate }).ToList();

                var spl = (from spc in db.C006_SubPhase
                           where spc.PhaseId == PhaseId
                           select new { spc.SubPhaseId, spc.SubPhaseName }).OrderBy(x => x.SubPhaseName).ToList();

                return Json(new { data = q , list = spl });
            }
        }

        [HttpPost]
        public JsonResult GetBucketListBySubPhase   (int SubPhaseId)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from b in db.vw_BucketbyLCDASAP
                         join po in db.C004_PROJECT on b.ProjectId equals po.ProjectId
                         join ph in db.C005_PHASE on b.PhaseId equals ph.PhaseId
                         join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         where (b.SubPhaseId == SubPhaseId)
                         select new { b.BucketId, po.ProjectName, ph.PhaseName, sp.SubPhaseName, b.BucketName, b.GeneratedUserName, b.GenerationDate }).ToList();
                return Json(new { data = q});
            }
        }


        // GET: Bucket
        public ActionResult Index()
        {
            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.OrderBy(l => l.LocationName), "LocationId", "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    .Take(0), "CompanyId", "CompanyName");

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AreaId      = new SelectList(db.C002_AREA       .Take(0), "AreaId", "AreaName");
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA   .Take(0), "SubAreaId", "SubAreaName");
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    .Take(0), "ProjectId", "ProjectName");
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      .Take(0), "PhaseId", "PhaseName");            
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   .Take(0), "SubPhaseId", "SubPhaseName");
            var q               = db.Database.SqlQuery<SP_BUCKET_LIST_Result>("SP_BUCKET_LIST").Take(0).ToList();
            return View(q);
        }

        // GET: Bucket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubPhaseName = Bhai.getSubPhase(c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Create
        public ActionResult Create()
        {
            #region P r o j e c t 
            int prjid = (Request.QueryString["project"] != null) ? Convert.ToInt32(Request.QueryString["project"].ToString()) : 0;
            if (prjid > 0) {
                ViewBag.ProjectId = new SelectList(db.C004_PROJECT.OrderBy(x => x.ProjectName), "ProjectId", "ProjectName", prjid);
            }
            else {
                ViewBag.ProjectId = new SelectList(db.C004_PROJECT.OrderBy(x => x.ProjectName), "ProjectId", "ProjectName");
            }
            #endregion

            #region P h a s e  
            int phid = (Request.QueryString["phase"] != null) ? Convert.ToInt32(Request.QueryString["phase"].ToString()) : 0;
            if (phid > 0) {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(x => x.ProjectId == prjid).OrderBy(x => x.PhaseName), "PhaseId", "PhaseName", phid);
            }
            else {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.OrderBy(x => x.PhaseName), "PhaseId", "PhaseName");
            }
            #endregion

            #region S u b  P h a s e 
            int sphid = (Request.QueryString["subphase"] != null) ? Convert.ToInt32(Request.QueryString["subphase"].ToString()) : 0;
            if (sphid > 0)
            {
                ViewBag.SubPhaseId = new SelectList(db.C006_SubPhase.Where(x => x.PhaseId == phid).OrderBy(x => x.SubPhaseName), "SubPhaseId", "SubPhaseName", sphid);
            }
            else
            {
                ViewBag.SubPhaseId = new SelectList(db.C006_SubPhase.OrderBy(x => x.SubPhaseName), "SubPhaseId", "SubPhaseName");
            }
            #endregion

            
            return View();
        }

        // POST: Bucket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseId,SubPhaseId,Name")] C007_BUCKET c007_BUCKET) {
            
            if ((ModelState.IsValid) || (ModelState.IsValid==false && c007_BUCKET.SubPhaseId==0)) {
                c007_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c007_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c007_BUCKET.IsActive        = true;

                db.C007_BUCKET.Add(c007_BUCKET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId       = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName" , c007_BUCKET.ProjectId);
            ViewBag.PhaseId         = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"   , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId      = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName", c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null) {
                return HttpNotFound();
            }

            if (id.HasValue) {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == c007_BUCKET.ProjectId)
                         select new { c.StartDate, c.EndDate }).FirstOrDefault();
                ViewBag.EndDate     = q.EndDate     .ToString("dd-MMM-yyyy");
                ViewBag.StartDate   = q.StartDate   .ToString("dd-MMM-yyyy");

                var ph = (from p in db.C005_PHASE
                         where (p.IsActive.Equals(true)) && (p.PhaseId == c007_BUCKET.PhaseId)
                         select new { p.StartDate, p.EndDate }).FirstOrDefault();
                ViewBag.PhEndDate   = ph.EndDate    .ToString("dd-MMM-yyyy");
                ViewBag.phStartDate = ph.StartDate  .ToString("dd-MMM-yyyy");
            }
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName" , c007_BUCKET.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"   , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName", c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // POST: Bucket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BucketId,ProjectId,PhaseId,SubPhaseId,Name")] C007_BUCKET c007_BUCKET)
        {
            if (ModelState.IsValid)
            {
                c007_BUCKET.GeneratedBy     = UserIdentity.UserId;
                c007_BUCKET.GenerationDate  = DateTime.Now.AddHours(4);
                c007_BUCKET.IsActive        = true;

                db.Entry(c007_BUCKET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    , "ProjectId"   , "ProjectName"     , c007_BUCKET.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      , "PhaseId"     , "PhaseName"       , c007_BUCKET.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   , "SubPhaseId"  , "SubPhaseName"    , c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // GET: Bucket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            if (c007_BUCKET == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubPhaseName = Bhai.getSubPhase(c007_BUCKET.SubPhaseId);
            return View(c007_BUCKET);
        }

        // POST: Bucket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C007_BUCKET c007_BUCKET = db.C007_BUCKET.Find(id);
            db.C007_BUCKET.Remove(c007_BUCKET);
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
