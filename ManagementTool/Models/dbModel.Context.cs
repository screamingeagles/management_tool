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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
    
        public virtual DbSet<C001_DIVISION> C001_DIVISION { get; set; }
        public virtual DbSet<C002_AREA> C002_AREA { get; set; }
        public virtual DbSet<C003_PROJECT> C003_PROJECT { get; set; }
        public virtual DbSet<C004_PHASE> C004_PHASE { get; set; }
        public virtual DbSet<C005_TASK_DATA> C005_TASK_DATA { get; set; }
        public virtual DbSet<C010_LOCATION> C010_LOCATION { get; set; }
        public virtual DbSet<C011_COMPANY> C011_COMPANY { get; set; }
        public virtual DbSet<C013_PROJECT_TYPE> C013_PROJECT_TYPE { get; set; }
        public virtual DbSet<C014_TASK_TYPE> C014_TASK_TYPE { get; set; }
        public virtual DbSet<C015_STATUS> C015_STATUS { get; set; }
        public virtual DbSet<C016_CYCLED> C016_CYCLED { get; set; }
        public virtual DbSet<C017_MEETING> C017_MEETING { get; set; }
        public virtual DbSet<C018_coOwners> C018_coOwners { get; set; }
        public virtual DbSet<C019_Attachments> C019_Attachments { get; set; }
        public virtual DbSet<C020_CommitmentMaster> C020_CommitmentMaster { get; set; }
        public virtual DbSet<C021_CommimentDetails> C021_CommimentDetails { get; set; }
        public virtual DbSet<C022_Notification_Master> C022_Notification_Master { get; set; }
        public virtual DbSet<C023_Notification_Detail> C023_Notification_Detail { get; set; }
        public virtual DbSet<C024_participants> C024_participants { get; set; }
        public virtual DbSet<DB_LoginHistory> DB_LoginHistory { get; set; }
        public virtual DbSet<DB_SAPUserActivity> DB_SAPUserActivity { get; set; }
        public virtual DbSet<EndUser> EndUsers { get; set; }
        public virtual DbSet<EndUser_Details> EndUser_Details { get; set; }
        public virtual DbSet<Mailer_Details> Mailer_Details { get; set; }
        public virtual DbSet<matched> matcheds { get; set; }
        public virtual DbSet<ROLE_DETAIL> ROLE_DETAIL { get; set; }
        public virtual DbSet<ROLE_MASTER> ROLE_MASTER { get; set; }
        public virtual DbSet<Smart_Login> Smart_Login { get; set; }
        public virtual DbSet<STATS_TABLE> STATS_TABLE { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<vw_SAPLoginHistory> vw_SAPLoginHistory { get; set; }
        public virtual DbSet<vw_SearchSource> vw_SearchSource { get; set; }
        public virtual DbSet<vw_SessionUser> vw_SessionUser { get; set; }
        public virtual DbSet<vw_Task_Details> vw_Task_Details { get; set; }
        public virtual DbSet<vw_ProjectsCountByArea> vw_ProjectsCountByArea { get; set; }
        public virtual DbSet<EndUser_LoginDetails> EndUser_LoginDetails { get; set; }
        public virtual DbSet<vw_ProjectsCountByCompany> vw_ProjectsCountByCompany { get; set; }
    
        public virtual ObjectResult<SP_BUCKET_LIST_Result> SP_BUCKET_LIST()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_BUCKET_LIST_Result>("SP_BUCKET_LIST");
        }
    }
}
