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
    
    public partial class C007_AREA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C007_AREA()
        {
            this.C002_TASK_DATA = new HashSet<C002_TASK_DATA>();
            this.C008_SUB_AREA = new HashSet<C008_SUB_AREA>();
        }
    
        public int AreaId { get; set; }
        public int DivisionId { get; set; }
        public string AreaName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public bool isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C002_TASK_DATA> C002_TASK_DATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C008_SUB_AREA> C008_SUB_AREA { get; set; }
    }
}
