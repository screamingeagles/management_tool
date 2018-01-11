using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Common
{
    public class Service : RoleAuthenticate
    {
        public int      ServiceId     { get; set; }
        public string   LocationName  { get; set; }
        public string   CompanyName   { get; set; }
        public string   DivisionName  { get; set; }
        public string   AreaName      { get; set; }
        public string   SubAreaName   { get; set; }
        public string   ProjectName   { get; set; }
        public string   PhaseName     { get; set; }
        public string   SubPhaseName  { get; set; }
        public string   BucketName    { get; set; }

        
        public string       ServiceName         { get; set; }
        public DateTime?    StartDate           { get; set; }
        public DateTime?    EndDate             { get; set; }
        public int          StatusId            { get; set; }
        public string       Status              { get; set; }
        public int          TaskTypeId          { get; set; }
        public string       TaskTypeName        { get; set; }
        public int          ServiceOwnerId      { get; set; }
        public string       ServiceOwnerName    { get; set; }
        public int          CreatedById         { get; set; }
        public string       CreateByName        { get; set; }
        public double?      ManDays             { get; set; }
        public string       Description         { get; set; }
        public string       DocLink             { get; set; }
        public string       coOwners            { get; set; }
        public DateTime     GeneratedDate       { get; set; }        
        public bool         isActive            { get; set; }


        public static List<Service> GetServiceList(int UserId, int Status)
        {
            List<Service> s = new List<Service>();
            try {
                using (ProjectEntities db = new ProjectEntities()) {
                    if (Status == 0) {
                        s = (from td in db.vw_Task_Details
                            where (td.IsActive == true) && (td.OwnerId == UserId)
                             select new Service {
                                 ServiceId      = td.TaskId,
                                 LocationName   = td.LocationName,
                                 CompanyName    = td.CompanyName,
                                 DivisionName   = td.DivisionName,
                                 ProjectName    = td.ProjectName,
                                 PhaseName      = td.PhaseName,
                                 SubPhaseName   = td.SubPhaseName,
                                 BucketName     = td.BucketName,
                                 AreaName       = td.AreaName,
                                 SubAreaName    = td.SubAreaName,
                                 ServiceName    = td.SName,
                                 StartDate      = td.StartDate,
                                 EndDate        = td.Deadline,
                                 StatusId       = td.StatusId,
                                 Status         = td.TaskStatus,
                                 TaskTypeId     = td.TaskTypeId,
                                 TaskTypeName   = td.TypeName,
                                 ServiceOwnerId = td.OwnerId,
                                 ServiceOwnerName = td.OwnerName,
                                 CreatedById    = td.GeneratedBy,
                                 CreateByName   = td.GerneratedName,
                                 ManDays        = td.ManDays,
                                 Description    = td.Description,
                                 DocLink        = td.DocsLink,
                                 GeneratedDate  = td.GeneratedDate,
                                 isActive       = td.IsActive
                             }).OrderBy(x => x.StatusId).ThenByDescending(x => x.ServiceId).ToList();
                    }
                    else {
                        s = (from td in db.vw_Task_Details
                             where (td.IsActive == true) && (td.OwnerId == UserId) && (td.StatusId == Status)
                             select new Service {
                                 ServiceId      = td.TaskId,
                                 LocationName   = td.LocationName,
                                 CompanyName    = td.CompanyName,
                                 DivisionName   = td.DivisionName,
                                 ProjectName    = td.ProjectName,
                                 PhaseName      = td.PhaseName,
                                 SubPhaseName   = td.SubPhaseName,
                                 BucketName     = td.BucketName,
                                 AreaName       = td.AreaName,
                                 SubAreaName    = td.SubAreaName,
                                 ServiceName    = td.SName,
                                 StartDate      = td.StartDate,
                                 EndDate        = td.Deadline,
                                 StatusId       = td.StatusId,
                                 Status         = td.TaskStatus,
                                 TaskTypeId     = td.TaskTypeId,
                                 TaskTypeName   = td.TypeName,
                                 ServiceOwnerId = td.OwnerId,
                                 ServiceOwnerName = td.OwnerName,
                                 CreatedById    = td.GeneratedBy,
                                 CreateByName   = td.GerneratedName,
                                 ManDays        = td.ManDays,
                                 Description    = td.Description,
                                 DocLink        = td.DocsLink,
                                 GeneratedDate = td.GeneratedDate,
                                 isActive       = td.IsActive
                             }).OrderBy(x => x.StatusId).ThenByDescending(x => x.ServiceId).ToList();
                    }
                }
            }
            catch (Exception) {
                //string si = ex.Message;
                s = null;
            }
            return s;
        }
    }
}