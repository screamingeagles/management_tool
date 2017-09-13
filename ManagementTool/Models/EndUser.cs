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
    
    public partial class EndUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EndUser()
        {
            this.C002_AREA = new HashSet<C002_AREA>();
            this.C003_SUB_AREA = new HashSet<C003_SUB_AREA>();
            this.C005_PHASE = new HashSet<C005_PHASE>();
            this.C006_SubPhase = new HashSet<C006_SubPhase>();
            this.C008_TASK_DATA = new HashSet<C008_TASK_DATA>();
            this.C008_TASK_DATA1 = new HashSet<C008_TASK_DATA>();
            this.C020_CommitmentMaster = new HashSet<C020_CommitmentMaster>();
            this.C022_Notification_Master = new HashSet<C022_Notification_Master>();
            this.C023_Notification_Detail = new HashSet<C023_Notification_Detail>();
            this.C004_PROJECT = new HashSet<C004_PROJECT>();
        }
    
        public int UID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public bool IsValidLogin { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<int> UserType { get; set; }
        public System.DateTime UserCreated { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Company { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C002_AREA> C002_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C003_SUB_AREA> C003_SUB_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C005_PHASE> C005_PHASE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C006_SubPhase> C006_SubPhase { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C008_TASK_DATA> C008_TASK_DATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C008_TASK_DATA> C008_TASK_DATA1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C020_CommitmentMaster> C020_CommitmentMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C022_Notification_Master> C022_Notification_Master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C023_Notification_Detail> C023_Notification_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C004_PROJECT> C004_PROJECT { get; set; }
    }
}
