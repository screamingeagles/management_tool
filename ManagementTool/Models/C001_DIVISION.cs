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
    
    public partial class C001_DIVISION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C001_DIVISION()
        {
            this.C002_AREA = new HashSet<C002_AREA>();
            this.C004_PROJECT = new HashSet<C004_PROJECT>();
        }
    
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int GeneratedBy { get; set; }
        public System.DateTime GeneratedDate { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C002_AREA> C002_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C004_PROJECT> C004_PROJECT { get; set; }
        public virtual EndUser EndUser { get; set; }
    }
}