namespace DoAnLapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDataSchool : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Schools");
        }
    }
}
