//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManagementTool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_SAPLoginHistory
    {
        public int TID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<int> SinceUsed { get; set; }
    }
}
