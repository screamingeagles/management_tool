﻿using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Models;
using ManagementTool.Common;
using System.Collections.Generic;

namespace ManagementTool.Controllers
{
    public class TasksController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetCompaniesByLocation(int SelectedLocation)
        {
            using (ProjectEntities _db = new ProjectEntities()) {
                var q = (from c in _db.C011_COMPANY
                         where (c.IsActive.Equals(true)) && (c.LocationId == SelectedLocation)
                         select new { c.CompanyId, c.CompanyName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetBucketDetails(int SelectedBucket)
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                var q = (from b  in _db.C007_BUCKET 
                         join p  in _db.C004_PROJECT  on b.ProjectId  equals p.ProjectId
                         join ph in _db.C005_PHASE    on b.PhaseId    equals ph.PhaseId
                         join sp in _db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         into jointable  from z in jointable.DefaultIfEmpty()
                         where (b.IsActive.Equals(true)) && (b.BucketId == SelectedBucket)
                         select new {   ProjectName = p.ProjectName,
                                        ProjectId   = p.ProjectId,
                                        PhaseName   = ph.PhaseName,
                                        PhaseId     = ph.PhaseId,
                                        SubPhaseName = (z.SubPhaseName == null) ? "" : z.SubPhaseName,
                                        StartDate = ph.StartDate, EndDate = ph.EndDate}).FirstOrDefault();
                
                return Json(new { data = q.ProjectName + "(" + q.ProjectId + ") >> " + q.PhaseName + "(" +  q.PhaseId + ") >> " + q.SubPhaseName, StartDate = q.StartDate, EndDate = q.EndDate});
            }
        }

        [HttpPost]
        public JsonResult GetSubAreaByArea(int SelectedArea)
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                var q = (from c in _db.C003_SUB_AREA
                         where (c.AreaActive.Equals(true)) && (c.AreaId == SelectedArea)
                         select new { c.SubAreaId, c.SubAreaName }).ToList();

                var dta = (from a in _db.C002_AREA 
                         join d in _db.C001_DIVISION on a.DivisionId equals d.DivisionId
                         where (a.isActive.Equals(true)) && (a.AreaId == SelectedArea)
                         select new { d.DivisionId, d.DivisionName }).FirstOrDefault();

                return Json(new { data = q, DvDetail = dta.DivisionName + " (" + dta.DivisionId + ")" });
            }
        }



        // GET: Tasks
        public ActionResult Index(){
            int uid = UserIdentity.UserId;
            List<Service> s = Service.GetServiceList(uid,0);
            return View(s);

            //var c008_TASK_DATA = db.C008_TASK_DATA.Include(c => c.C007_BUCKET).Include(c => c.C011_COMPANY).Include(c => c.C010_LOCATION).Include(c => c.EndUser).Include(c => c.EndUser1).Include(c => c.C015_STATUS).Include(c => c.C014_TASK_TYPE);
            //return View(c008_TASK_DATA.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            //UserIdentity.UserId = 1020;
            //UserIdentity.UserName= "Arsalan Ahmed (RTT)";
            // generated by name have to get 


            ViewBag.LocationId  = new SelectList(db.C010_LOCATION,  "LocationId",   "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY,   "CompanyId",    "CompanyName");

 
            var area = (from a in db.C002_AREA join
                         d in db.C001_DIVISION on a.DivisionId equals d.DivisionId
                         where ((a.isActive == true) && (d.IsActive == true))
                         select new { a, d }).OrderBy(x => x.d.DivisionName).ThenBy(x => x.a.AreaName).AsEnumerable()
                         .Select(x => new {
                             AreaId = x.a.AreaId,
                             DivId = x.a.DivisionId,
                             AreaName = x.a.AreaName,
                             DivName  = x.d.DivisionName,
                             Description = string.Format("{0} -- {1}", x.d.DivisionName, x.a.AreaName)
                         });

            ViewBag.AreaId      = new SelectList(area            ,  "AreaId"    ,    "Description");
            ViewBag.SSubAreaId  = new SelectList(db.C003_SUB_AREA,  "SubAreaId",    "SubAreaName");
            //SSubAreaId

            ViewBag.BucketId    = new SelectList(db.C007_BUCKET,    "BucketId",     "Name");
            ViewBag.OwnerId     = new SelectList(db.EndUsers,       "UID",          "UserName");
            ViewBag.StatusId    = new SelectList(db.C015_STATUS,    "StatusId",     "TaskStatus");
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE, "TypeId",       "TypeName");

            ViewBag.UserId      = UserIdentity.UserId;
            ViewBag.UserName    = UserIdentity.UserName;
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,CompanyId,BucketId,AreaId,SSubAreaId,SName,Description,StartDate,Deadline,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (c008_TASK_DATA.SName != "") //(ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy   = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive      = true;

                db.C008_TASK_DATA.Add(c008_TASK_DATA);
                db.SaveChanges();
                int ServiceId = c008_TASK_DATA.TaskId;
                return RedirectToAction("Edit","Tasks", new { id = ServiceId });
            }

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION   , "LocationId"  , "LocationName", c008_TASK_DATA.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    , "CompanyId"   , "CompanyName" , c008_TASK_DATA.CompanyId);
            var area = (from a in db.C002_AREA join 
                        d in db.C001_DIVISION on a.DivisionId equals d.DivisionId
                            where ((a.isActive == true) && (d.IsActive == true))
                            select new { a, d }).OrderBy(x => x.d.DivisionName).ThenBy(x => x.a.AreaName).AsEnumerable()
                            .Select(x => new {
                              AreaId = x.a.AreaId,
                              DivId = x.a.DivisionId,
                              AreaName = x.a.AreaName,
                              DivName = x.d.DivisionName,
                              Description = string.Format("{0} -- {1}", x.d.DivisionName, x.a.AreaName)
                          });
            ViewBag.AreaId      = new SelectList(area               , "AreaId"      , "Description" , c008_TASK_DATA.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA   , "SubAreaId"   , "SubAreaName" , c008_TASK_DATA.SSubAreaId);




            

            ViewBag.BucketId = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);            
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.UserId      = UserIdentity.UserId;
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null) {
                return HttpNotFound();
            }

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION   , "LocationId"  , "LocationName", c008_TASK_DATA.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    , "CompanyId"   , "CompanyName" , c008_TASK_DATA.CompanyId);

            var area = (from a in db.C002_AREA
                        join d in db.C001_DIVISION on a.DivisionId equals d.DivisionId
                        where ((a.isActive == true) && (d.IsActive == true))
                        select new { a, d }).OrderBy(x => x.d.DivisionName).ThenBy(x => x.a.AreaName).AsEnumerable()
                            .Select(x => new
                            {
                                AreaId = x.a.AreaId,
                                DivId = x.a.DivisionId,
                                AreaName = x.a.AreaName,
                                DivName = x.d.DivisionName,
                                Description = string.Format("{0} -- {1}", x.d.DivisionName, x.a.AreaName)
                            });
            ViewBag.AreaId      = new SelectList(area               , "AreaId"      , "Description" , c008_TASK_DATA.AreaId);
            ViewBag.SSubAreaId  = new SelectList(db.C003_SUB_AREA   , "SubAreaId"   , "SubAreaName" , c008_TASK_DATA.SSubAreaId);


            // 1450
            task_base tb = (from t  in db.C008_TASK_DATA
                            join b  in db.C007_BUCKET   on t.BucketId   equals b.BucketId
                            join p  in db.C004_PROJECT  on b.ProjectId  equals p.ProjectId
                            join ph in db.C005_PHASE    on b.PhaseId    equals ph.PhaseId
                            join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                            into joinx from x in joinx.DefaultIfEmpty()

                            where (t.TaskId == id)
                            select new task_base { ServiceId    = t.TaskId,
                                                    ProjectName = p.ProjectName,
                                                    PhaseName   = ph.PhaseName,
                                                    SubPhaseName= (x.SubPhaseName == null)? "": x.SubPhaseName,
                            }).FirstOrDefault();

            //var  ar = (from a in db.C002_AREA
            //                join s in db.C003_SUB_AREA on a.AreaId equals s.AreaId
            //                where (a.AreaId == c008_TASK_DATA.AreaId) && (s.SubAreaId == c008_TASK_DATA.SubAreaId)
            //                select new  { a.AreaName, s.SubAreaName }).FirstOrDefault();
            //tb.AreaName = ar.AreaName;
            //tb.SubAreaName = ar.SubAreaName;




            ViewBag.TaskBase = tb;
            ViewBag.BucketId    = new SelectList(db.C007_BUCKET     , "BucketId"    , "Name"        , c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.BucketDetial = Bhai.GetBucketDetail(c008_TASK_DATA.BucketId);
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,LocationId,CompanyId,BucketId,AreaId,SSubAreaId,SSubAreaId,SName,Description,StartDate,Deadline,OrderId,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy      = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate    = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive         = true;

                db.Entry(c008_TASK_DATA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION   , "LocationId"  , "LocationName", c008_TASK_DATA.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    , "CompanyId"   , "CompanyName" , c008_TASK_DATA.CompanyId);
            ViewBag.AreaId      = new SelectList(db.C002_AREA       , "AreaId"      , "AreaName"    , c008_TASK_DATA.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA   , "SubAreaId"   , "SubAreaName" , c008_TASK_DATA.SubAreaId);
            
            ViewBag.BucketId    = new SelectList(db.C007_BUCKET     , "BucketId"    , "Name"        , c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.UserName = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c008_TASK_DATA);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            db.C008_TASK_DATA.Remove(c008_TASK_DATA);
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
