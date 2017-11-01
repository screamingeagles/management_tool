using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public class task_base
    {
        public int ServiceId { get; set; }
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