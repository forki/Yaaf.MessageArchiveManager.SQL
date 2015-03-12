namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0_0_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ChatCol",
                c => new
                    {
                        UID = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Start = c.DateTime(nullable: false, precision: 0),
                        WJid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Subject = c.String(unicode: false),
                        Thread = c.String(unicode: false),
                        Version = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        NStart = c.DateTime(precision: 0),
                        NWJid = c.String(maxLength: 128, unicode: false, storeType: "nvarchar"),
                        PStart = c.DateTime(precision: 0),
                        PWJid = c.String(maxLength: 128, unicode: false, storeType: "nvarchar"),
                        XData = c.String(unicode: false),
                        LastChanged = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.UID, t.Start, t.WJid });
            
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
                        UID = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Jid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        DbSaveMode = c.Int(nullable: false),
                        DbOtrMode = c.Int(nullable: false),
                        Expire = c.Long(),
                    })
                .PrimaryKey(t => new { t.UID, t.Jid });
            
            CreateTable(
                "ChMsg",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        UID = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        Start = c.DateTime(nullable: false, precision: 0),
                        WJid = c.String(nullable: false, maxLength: 128, unicode: false, storeType: "nvarchar"),
                        DbMessageType = c.Int(nullable: false),
                        MessageDateTime = c.DateTime(nullable: false, precision: 0),
                        DbContentType = c.Int(nullable: false),
                        Content = c.String(unicode: false),
                        DbMessageState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ChatCol_UID_PStart_PWJid");
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ChatCol_UID_NStart_NWJid");
            DropForeignKey("ChMsg", "FK_dbo.ChMsg_dbo.ChatCol_UID_Start_WJid");
            DropForeignKey("ChatCol", "FK_dbo.ChatCol_dbo.ArcUs_UID");
            DropForeignKey("ItemPref", "FK_dbo.ItemPref_dbo.ArcUs_UID");
            DropIndex("ChatCol", new[] { "UID", "PStart", "PWJid" });
            DropIndex("ChatCol", new[] { "UID", "NStart", "NWJid" });
            DropIndex("ChMsg", new[] { "UID", "Start", "WJid" });
            DropIndex("ChatCol", new[] { "UID" });
            DropIndex("ItemPref", new[] { "UID" });
            DropTable("ChMsg");
            DropTable("ItemPref");
            DropTable("ArcUs");
            DropTable("ChatCol");
        }
    }
}
