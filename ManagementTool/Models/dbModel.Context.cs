﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProjectEntities : DbContext
    {
        public ProjectEntities()
            : base("name=ProjectEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C002_PHASE> C002_PHASE { get; set; }
        public virtual DbSet<C003_SUB_PHASE> C003_SUB_PHASE { get; set; }
        public virtual DbSet<C004_BUCKET> C004_BUCKET { get; set; }
        public virtual DbSet<C005_TASK_DATA> C005_TASK_DATA { get; set; }
        public virtual DbSet<C006_SUB_TASKS> C006_SUB_TASKS { get; set; }
        public virtual DbSet<C007_LOCATION> C007_LOCATION { get; set; }
        public virtual DbSet<C008_COMPANY> C008_COMPANY { get; set; }
        public virtual DbSet<C009_DIVISION> C009_DIVISION { get; set; }
        public virtual DbSet<C010_AREA> C010_AREA { get; set; }
        public virtual DbSet<C011_SUB_AREA> C011_SUB_AREA { get; set; }
        public virtual DbSet<C012_SUB_SUB_AREA> C012_SUB_SUB_AREA { get; set; }
        public virtual DbSet<C013_PROJECT_TYPE> C013_PROJECT_TYPE { get; set; }
        public virtual DbSet<C014_TASK_TYPE> C014_TASK_TYPE { get; set; }
        public virtual DbSet<C015_STATUS> C015_STATUS { get; set; }
        public virtual DbSet<C016_CYCLED> C016_CYCLED { get; set; }
        public virtual DbSet<C017_MEETING> C017_MEETING { get; set; }
        public virtual DbSet<C018_coOwners> C018_coOwners { get; set; }
        public virtual DbSet<C019_Attachments> C019_Attachments { get; set; }
        public virtual DbSet<EndUser> EndUsers { get; set; }
        public virtual DbSet<EndUser_Details> EndUser_Details { get; set; }
        public virtual DbSet<EndUser_LoginDetails> EndUser_LoginDetails { get; set; }
        public virtual DbSet<Mailer_Details> Mailer_Details { get; set; }
        public virtual DbSet<Smart_Login> Smart_Login { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<LOGIN_DETAIL> LOGIN_DETAIL { get; set; }
        public virtual DbSet<vw_SessionUser> vw_SessionUser { get; set; }
        public virtual DbSet<C001_PROJECT> C001_PROJECT { get; set; }
    }
}
