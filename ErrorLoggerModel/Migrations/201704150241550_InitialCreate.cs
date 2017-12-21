namespace ErrorLoggerModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        appId = c.Int(nullable: false, identity: true),
                        appName = c.String(nullable: false, maxLength: 50),
                        appType = c.String(),
                        debugLevel_debugID = c.Int(),
                    })
                .PrimaryKey(t => t.appId)
                .ForeignKey("dbo.DebugLevels", t => t.debugLevel_debugID)
                .Index(t => t.debugLevel_debugID);
            
            CreateTable(
                "dbo.DebugLevels",
                c => new
                    {
                        debugID = c.Int(nullable: false, identity: true),
                        debugDescription = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.debugID);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        logID = c.Int(nullable: false, identity: true),
                        fileName = c.String(nullable: false, maxLength: 100),
                        timeStamp = c.DateTime(nullable: false),
                        Application_appId = c.Int(),
                        logType_typeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.logID)
                .ForeignKey("dbo.Applications", t => t.Application_appId)
                .ForeignKey("dbo.logTypes", t => t.logType_typeID, cascadeDelete: true)
                .Index(t => t.Application_appId)
                .Index(t => t.logType_typeID);
            
            CreateTable(
                "dbo.logTypes",
                c => new
                    {
                        typeID = c.Int(nullable: false, identity: true),
                        typeName = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.typeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false, maxLength: 100),
                        lastName = c.String(nullable: false, maxLength: 100),
                        emailID = c.String(nullable: false, maxLength: 100),
                        lastLoginDate = c.DateTime(nullable: false),
                        activeStatus = c.String(),
                        userType_userTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userID)
                .ForeignKey("dbo.UserTypes", t => t.userType_userTypeID, cascadeDelete: true)
                .Index(t => t.userType_userTypeID);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        userTypeID = c.Int(nullable: false, identity: true),
                        userType = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.userTypeID);
            
            CreateTable(
                "dbo.UserApplications",
                c => new
                    {
                        User_userID = c.Int(nullable: false),
                        Application_appId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_userID, t.Application_appId })
                .ForeignKey("dbo.Users", t => t.User_userID, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.Application_appId, cascadeDelete: true)
                .Index(t => t.User_userID)
                .Index(t => t.Application_appId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "userType_userTypeID", "dbo.UserTypes");
            DropForeignKey("dbo.UserApplications", "Application_appId", "dbo.Applications");
            DropForeignKey("dbo.UserApplications", "User_userID", "dbo.Users");
            DropForeignKey("dbo.ErrorLogs", "logType_typeID", "dbo.logTypes");
            DropForeignKey("dbo.ErrorLogs", "Application_appId", "dbo.Applications");
            DropForeignKey("dbo.Applications", "debugLevel_debugID", "dbo.DebugLevels");
            DropIndex("dbo.UserApplications", new[] { "Application_appId" });
            DropIndex("dbo.UserApplications", new[] { "User_userID" });
            DropIndex("dbo.Users", new[] { "userType_userTypeID" });
            DropIndex("dbo.ErrorLogs", new[] { "logType_typeID" });
            DropIndex("dbo.ErrorLogs", new[] { "Application_appId" });
            DropIndex("dbo.Applications", new[] { "debugLevel_debugID" });
            DropTable("dbo.UserApplications");
            DropTable("dbo.UserTypes");
            DropTable("dbo.Users");
            DropTable("dbo.logTypes");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.DebugLevels");
            DropTable("dbo.Applications");
        }
    }
}
