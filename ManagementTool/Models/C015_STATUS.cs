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
    
    public partial class C015_STATUS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C015_STATUS()
        {
            this.C005_TASK_DATA = new HashSet<C005_TASK_DATA>();
        }
    
        public int StatusId { get; set; }
        public string TaskStatus { get; set; }
        public bool TaskActive { get; set; }
        public int TaskCreatedBy { get; set; }
        public System.DateTime TaskCreatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C005_TASK_DATA> C005_TASK_DATA { get; set; }
    }
}
