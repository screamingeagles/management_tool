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
    
    public partial class LOGIN_DETAIL
    {
        public int LID { get; set; }
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public Nullable<System.DateTime> LogOut { get; set; }
        public Nullable<decimal> SessionDuration { get; set; }
        public string UserIp { get; set; }
        public string LoginLocation { get; set; }
    }
}
