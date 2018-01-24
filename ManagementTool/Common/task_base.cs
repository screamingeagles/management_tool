using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public class task_base : RoleAuthenticate
    {
        public int ServiceId    { get; set; }
        public int LocationId   { get; set; }
        public int CompanyId    { get; set; }
        public int DivisionId   { get; set; }
        public int AreaId       { get; set; }
        public int SubAreaId    { get; set; }
        public int ProjectId     { get; set; }
        public int PhaseId      { get; set; }
        public int SubPhaseId   { get; set; }

        public string LocationName { get; set; }
        public string CompanyName { get; set; }
        public string DivisionName { get; set; }
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public string ProjectName { get; set; }
        public string PhaseName { get; set; }
        public string SubPhaseName { get; set; }
    }
}