namespace ErrorLoggerModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "appStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "appStatus");
        }
    }
}
