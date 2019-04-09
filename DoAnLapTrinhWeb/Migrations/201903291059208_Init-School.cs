namespace DoAnLapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitSchool : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Schools");
        }
        
        public override void Down()
        {
            
        }
    }
}
