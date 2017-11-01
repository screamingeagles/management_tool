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
    
    public partial class C005_PHASE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C005_PHASE()
        {
            this.C007_BUCKET = new HashSet<C007_BUCKET>();
            this.C006_SubPhase = new HashSet<C006_SubPhase>();
        }
    
        public int PhaseId { get; set; }
        public int ProjectId { get; set; }
        public string PhaseName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime GeneratedDate { get; set; }
        public int GeneratedBy { get; set; }
    
        public virtual EndUser EndUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C007_BUCKET> C007_BUCKET { get; set; }
        public virtual C004_PROJECT C004_PROJECT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C006_SubPhase> C006_SubPhase { get; set; }
    }
}
