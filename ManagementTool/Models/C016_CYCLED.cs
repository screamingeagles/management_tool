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
    
    public partial class C016_CYCLED
    {
        public int CycledId { get; set; }
        public int TaskId { get; set; }
        public int CycledType { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual C008_TASK_DATA C008_TASK_DATA { get; set; }
    }
}
