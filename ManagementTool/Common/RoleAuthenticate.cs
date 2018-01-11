using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementTool.Common
{
    public abstract class RoleAuthenticate {
        public bool isViewAllowed  { get; set; }
        public bool isAddAllowed   { get; set; }
        public bool isEditAllowed  { get; set; }
        public bool isDeleteAllowed{ get; set; }
        
    }
}