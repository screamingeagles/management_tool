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
    
    public partial class Mailer_Details
    {
        public int TicketId { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<bool> Mailed { get; set; }
        public Nullable<System.DateTime> MailSentDate { get; set; }
    }
}
