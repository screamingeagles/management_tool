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
    
    public partial class C011_COMPANY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C011_COMPANY()
        {
            this.C008_TASK_DATA = new HashSet<C008_TASK_DATA>();
        }
    
        public int CompanyId { get; set; }
        public int LocationId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C008_TASK_DATA> C008_TASK_DATA { get; set; }
    }
}
