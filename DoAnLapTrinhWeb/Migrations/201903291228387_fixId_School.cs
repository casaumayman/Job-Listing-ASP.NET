namespace DoAnLapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixId_School : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidates", "School_Id", "dbo.Schools");
            DropIndex("dbo.Candidates", new[] { "School_Id" });
            DropColumn("dbo.Candidates", "Id_School");
            RenameColumn(table: "dbo.Candidates", name: "School_Id", newName: "Id_School");
            DropPrimaryKey("dbo.Schools");
            AddColumn("dbo.Schools", "Id_School", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Candidates", "Id_School", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Schools", "Id_School");
            CreateIndex("dbo.Candidates", "Id_School");
            AddForeignKey("dbo.Candidates", "Id_School", "dbo.Schools", "Id_School", cascadeDelete: true);
            DropColumn("dbo.Schools", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Candidates", "Id_School", "dbo.Schools");
            DropIndex("dbo.Candidates", new[] { "Id_School" });
            DropPrimaryKey("dbo.Schools");
            AlterColumn("dbo.Candidates", "Id_School", c => c.Int());
            DropColumn("dbo.Schools", "Id_School");
            AddPrimaryKey("dbo.Schools", "Id");
            RenameColumn(table: "dbo.Candidates", name: "Id_School", newName: "School_Id");
            AddColumn("dbo.Candidates", "Id_School", c => c.Int(nullable: false));
            CreateIndex("dbo.Candidates", "School_Id");
            AddForeignKey("dbo.Candidates", "School_Id", "dbo.Schools", "Id");
        }
    }
}
