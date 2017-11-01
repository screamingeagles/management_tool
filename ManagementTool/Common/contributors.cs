using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public class contributors
    {
        public int      FieldId            { get; set; }
        public int   UID                 { get; set; }
        public string   UserName            { get; set; }
        public string   UserContribution    { get; set; }
        public string   ContributorAddedBy { get; set; }
        public DateTime ContributorAddDate { get; set; }

    }
}