using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public class STATS_TABLE_LOCAL
    {
        public int SID { get; set; }
        public string Server { get; set; }
        public string TCode { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}