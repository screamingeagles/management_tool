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
    
    public partial class C003_PROJECT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C003_PROJECT()
        {
            this.C004_PHASE = new HashSet<C004_PHASE>();
            this.C019_Attachments = new HashSet<C019_Attachments>();
            this.C018_coOwners = new HashSet<C018_coOwners>();
        }
    
        public int ProjectId { get; set; }
        public int LocationId { get; set; }
        public int CompanyId { get; set; }
        public int DivisionId { get; set; }
        public int AreaId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectType { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int GeneratedBy { get; set; }
        public System.DateTime GeneratedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual C001_DIVISION C001_DIVISION { get; set; }
        public virtual C002_AREA C002_AREA { get; set; }
        public virtual EndUser EndUser { get; set; }
        public virtual C011_COMPANY C011_COMPANY { get; set; }
        public virtual C010_LOCATION C010_LOCATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C004_PHASE> C004_PHASE { get; set; }
        public virtual C013_PROJECT_TYPE C013_PROJECT_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C019_Attachments> C019_Attachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C018_coOwners> C018_coOwners { get; set; }
    }
}
