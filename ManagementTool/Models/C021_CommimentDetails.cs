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
    
    public partial class C021_CommimentDetails
    {
        public int DetailId { get; set; }
        public int CommitmentId { get; set; }
        public string CommimentName { get; set; }
        public string CDescription { get; set; }
        public string CRemarks { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> TaskId { get; set; }
        public int GeneratedBy { get; set; }
        public System.DateTime GeneratedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual C020_CommitmentMaster C020_CommitmentMaster { get; set; }
    }
}
