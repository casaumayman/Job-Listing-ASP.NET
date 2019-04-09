namespace DoAnLapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id_Candidate = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 16),
                        Gender = c.Boolean(nullable: false),
                        Password = c.String(nullable: false, maxLength: 16),
                        CVLink = c.String(),
                        YearBorn = c.Int(nullable: false),
                        Id_School = c.Int(nullable: false),
                        Id_Specialization = c.Int(nullable: false),
                        School_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Candidate)
                .ForeignKey("dbo.Schools", t => t.School_Id)
                .ForeignKey("dbo.Specializations", t => t.Id_Specialization, cascadeDelete: true)
                .Index(t => t.Id_Specialization)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        Id_Employer = c.Int(nullable: false),
                        Id_Candidate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id_Employer, t.Id_Candidate })
                .ForeignKey("dbo.Candidates", t => t.Id_Candidate, cascadeDelete: true)
                .ForeignKey("dbo.Employers", t => t.Id_Employer, cascadeDelete: true)
                .Index(t => t.Id_Employer)
                .Index(t => t.Id_Candidate);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id_Employer = c.Int(nullable: false),
                        Id_Candidate = c.Int(nullable: false),
                        DateOfApplication = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id_Employer, t.Id_Candidate })
                .ForeignKey("dbo.Candidates", t => t.Id_Candidate, cascadeDelete: true)
                .ForeignKey("dbo.Employers", t => t.Id_Employer, cascadeDelete: true)
                .Index(t => t.Id_Employer)
                .Index(t => t.Id_Candidate);
            
            CreateTable(
                "dbo.Employers",
                c => new
                    {
                        Id_Employer = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 16),
                        Password = c.String(nullable: false, maxLength: 16),
                        Email = c.String(nullable: false),
                        Address = c.String(),
                        ImgLogo = c.String(),
                    })
                .PrimaryKey(t => t.Id_Employer);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id_Post = c.Int(nullable: false, identity: true),
                        DatePost = c.DateTime(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Require = c.String(nullable: false),
                        Benefit = c.String(nullable: false),
                        Salary = c.Long(nullable: false),
                        Id_Employer = c.Int(nullable: false),
                        Id_Specialization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Post)
                .ForeignKey("dbo.Employers", t => t.Id_Employer, cascadeDelete: true)
                .ForeignKey("dbo.Specializations", t => t.Id_Specialization, cascadeDelete: true)
                .Index(t => t.Id_Employer)
                .Index(t => t.Id_Specialization);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id_Specialization = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id_Specialization);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Id_Specialization", "dbo.Specializations");
            DropForeignKey("dbo.Candidates", "Id_Specialization", "dbo.Specializations");
            DropForeignKey("dbo.Candidates", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.Posts", "Id_Employer", "dbo.Employers");
            DropForeignKey("dbo.JobApplications", "Id_Employer", "dbo.Employers");
            DropForeignKey("dbo.Followings", "Id_Employer", "dbo.Employers");
            DropForeignKey("dbo.JobApplications", "Id_Candidate", "dbo.Candidates");
            DropForeignKey("dbo.Followings", "Id_Candidate", "dbo.Candidates");
            DropIndex("dbo.Posts", new[] { "Id_Specialization" });
            DropIndex("dbo.Posts", new[] { "Id_Employer" });
            DropIndex("dbo.JobApplications", new[] { "Id_Candidate" });
            DropIndex("dbo.JobApplications", new[] { "Id_Employer" });
            DropIndex("dbo.Followings", new[] { "Id_Candidate" });
            DropIndex("dbo.Followings", new[] { "Id_Employer" });
            DropIndex("dbo.Candidates", new[] { "School_Id" });
            DropIndex("dbo.Candidates", new[] { "Id_Specialization" });
            DropTable("dbo.Specializations");
            DropTable("dbo.Schools");
            DropTable("dbo.Posts");
            DropTable("dbo.Employers");
            DropTable("dbo.JobApplications");
            DropTable("dbo.Followings");
            DropTable("dbo.Candidates");
        }
    }
}
