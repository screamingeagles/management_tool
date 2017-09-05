using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public class UserIdentity
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static int RoleId { get; set; }
    }
}