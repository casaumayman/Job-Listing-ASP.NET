using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("name=conString") { }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<School> Schools { get; set; }
    }
}