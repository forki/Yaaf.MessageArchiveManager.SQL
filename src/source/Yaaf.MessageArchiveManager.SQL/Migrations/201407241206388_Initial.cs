namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatCol",
                c => new
                    {
                        ArchivingUserId = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(nullable: false),
                        WithJid = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        Thread = c.String(),
                        Version = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        NextStartDate = c.DateTime(),
                        NextWithJid = c.String(maxLength: 128),
                        PreviousStartDate = c.DateTime(),
                        PreviousWithJid = c.String(maxLength: 128),
                        XData = c.String(),
                    })
                .PrimaryKey(t => new { t.ArchivingUserId, t.StartDate, t.WithJid })
                .ForeignKey("dbo.ArcUs", t => t.ArchivingUserId)
                .ForeignKey("dbo.ChatCol", t => new { t.ArchivingUserId, t.NextStartDate, t.NextWithJid })
                .ForeignKey("dbo.ChatCol", t => new { t.ArchivingUserId, t.PreviousStartDate, t.PreviousWithJid })
                .Index(t => t.ArchivingUserId)
                .Index(t => new { t.ArchivingUserId, t.NextStartDate, t.NextWithJid })
                .Index(t => new { t.ArchivingUserId, t.PreviousStartDate, t.PreviousWithJid });
            
            CreateTable(
                "dbo.ArcUs",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
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
                "dbo.ItemPref",
                c => new
                    {
                        ArchivingUserId = c.String(nullable: false, maxLength: 128),
                        Jid = c.String(nullable: false, maxLength: 128),
                        DbSaveMode = c.Int(nullable: false),
                        DbOtrMode = c.Int(nullable: false),
                        Expire = c.Long(),
                    })
                .PrimaryKey(t => new { t.ArchivingUserId, t.Jid })
                .ForeignKey("dbo.ArcUs", t => t.ArchivingUserId, cascadeDelete: true)
                .Index(t => t.ArchivingUserId);
            
            CreateTable(
                "dbo.ChMsg",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        CollectionArchivingUserId = c.String(nullable: false, maxLength: 128),
                        CollectionStartDate = c.DateTime(nullable: false),
                        CollectionWithJid = c.String(nullable: false, maxLength: 128),
                        DbMessageType = c.Int(nullable: false),
                        MessageDateTime = c.DateTime(nullable: false),
                        DbContentType = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.ChatCol", t => new { t.CollectionArchivingUserId, t.CollectionStartDate, t.CollectionWithJid }, cascadeDelete: true)
                .Index(t => new { t.CollectionArchivingUserId, t.CollectionStartDate, t.CollectionWithJid });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" }, "dbo.ChatCol");
            DropForeignKey("dbo.ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" }, "dbo.ChatCol");
            DropForeignKey("dbo.ChMsg", new[] { "CollectionArchivingUserId", "CollectionStartDate", "CollectionWithJid" }, "dbo.ChatCol");
            DropForeignKey("dbo.ChatCol", "ArchivingUserId", "dbo.ArcUs");
            DropForeignKey("dbo.ItemPref", "ArchivingUserId", "dbo.ArcUs");
            DropIndex("dbo.ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" });
            DropIndex("dbo.ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" });
            DropIndex("dbo.ChMsg", new[] { "CollectionArchivingUserId", "CollectionStartDate", "CollectionWithJid" });
            DropIndex("dbo.ChatCol", new[] { "ArchivingUserId" });
            DropIndex("dbo.ItemPref", new[] { "ArchivingUserId" });
            DropTable("dbo.ChMsg");
            DropTable("dbo.ItemPref");
            DropTable("dbo.ArcUs");
            DropTable("dbo.ChatCol");
        }
    }
}
