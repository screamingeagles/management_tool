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
    
    public partial class C022_Notification_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C022_Notification_Master()
        {
            this.C023_Notification_Detail = new HashSet<C023_Notification_Detail>();
        }
    
        public int NotificationId { get; set; }
        public string NotificationIcon { get; set; }
        public string Notification { get; set; }
        public System.DateTime GeneratedDate { get; set; }
        public int GeneratedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual EndUser EndUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C023_Notification_Detail> C023_Notification_Detail { get; set; }
    }
}