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
    
    public partial class C012_SUB_SUB_AREA
    {
        public int SSubAreaId { get; set; }
        public int AreaId { get; set; }
        public int SubAreaId { get; set; }
        public string SSAreaName { get; set; }
        public bool IsActive { get; set; }
        public int GenerationBy { get; set; }
        public System.DateTime GenerationDate { get; set; }
    
        public virtual C011_SUB_AREA C011_SUB_AREA { get; set; }
    }
}
