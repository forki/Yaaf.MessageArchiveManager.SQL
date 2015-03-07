namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ChatCol",
                c => new
                    {
                        ArchivingUserId = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        WithJid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Subject = c.String(unicode: false),
                        Thread = c.String(unicode: false),
                        Version = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        NextStartDate = c.DateTime(precision: 0),
                        NextWithJid = c.String(maxLength: 128, unicode: false, storeType: "nvarchar"),
                        PreviousStartDate = c.DateTime(precision: 0),
                        PreviousWithJid = c.String(maxLength: 128, unicode: false, storeType: "nvarchar"),
                        XData = c.String(unicode: false),
                    })
                .PrimaryKey(t => new { t.ArchivingUserId, t.StartDate, t.WithJid });
            
            CreateTable(
                "ArcUs",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        AutomaticArchiving = c.Boolean(),
                        DbAutoPreference = c.Int(nullable: false),
                        DbLocalPreference = c.Int(nullable: false),
                        DbManualPreference = c.Int(nullable: false),
                        DbSaveMode = c.Int(nullable: false),
                        DbOtrMode = c.Int(nullable: false),
                        Expire = c.Long(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "ItemPref",
                c => new
                    {
                        ArchivingUserId = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Jid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        DbSaveMode = c.Int(nullable: false),
                        DbOtrMode = c.Int(nullable: false),
                        Expire = c.Long(),
                    })
                .PrimaryKey(t => new { t.ArchivingUserId, t.Jid });
            
            CreateTable(
                "ChMsg",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        CollectionArchivingUserId = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        CollectionStartDate = c.DateTime(nullable: false, precision: 0),
                        CollectionWithJid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        DbMessageType = c.Int(nullable: false),
                        MessageDateTime = c.DateTime(nullable: false, precision: 0),
                        DbContentType = c.Int(nullable: false),
                        Content = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.MessageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ChatCol_ArchivingUserId_PreviousStartDate_PreviousWithJid");
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ChatCol_ArchivingUserId_NextStartDate_NextWithJid");
            DropForeignKey("ChMsg", "FK_dbo.ChMsg_dbo.ChatCol_CollectionArchivingUserId_CollectionStartDate_CollectionWithJid");
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ArcUs_ArchivingUserId");
            DropForeignKey("ItemPref", "FK_dbo.ItemPref_dbo.ArcUs_ArchivingUserId");
            DropIndex("ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" });
            DropIndex("ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" });
            DropIndex("ChMsg", new[] { "CollectionArchivingUserId", "CollectionStartDate", "CollectionWithJid" });
            DropIndex("ChatCol", new[] { "ArchivingUserId" });
            DropIndex("ItemPref", new[] { "ArchivingUserId" });
            DropTable("ChMsg");
            DropTable("ItemPref");
            DropTable("ArcUs");
            DropTable("ChatCol");
        }
    }
}
