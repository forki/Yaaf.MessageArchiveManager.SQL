namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0_0_1 : DbMigration
    {
        public override void Up()
        {
            var now_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            //DropForeignKey("ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" }, "ChatCol");
            //DropForeignKey("ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" }, "ChatCol");
            //DropIndex("ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" });
            //DropIndex("ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" });
            RenameColumn(table: "ChatCol", name: "ArchivingUserId", newName: "UID");
            RenameColumn(table: "ChatCol", name: "StartDate", newName: "Start");
            RenameColumn(table: "ChatCol", name: "WithJid", newName: "WJid");
            RenameColumn(table: "ChatCol", name: "NextStartDate", newName: "NStart");
            RenameColumn(table: "ChatCol", name: "NextWithJid", newName: "NWJid");
            RenameColumn(table: "ChatCol", name: "PreviousStartDate", newName: "PStart");
            RenameColumn(table: "ChatCol", name: "PreviousWithJid", newName: "PWJid");
            RenameColumn(table: "ItemPref", name: "ArchivingUserId", newName: "UID");
            RenameColumn(table: "ChMsg", name: "CollectionArchivingUserId", newName: "UID");
            RenameColumn(table: "ChMsg", name: "CollectionStartDate", newName: "Start");
            RenameColumn(table: "ChMsg", name: "CollectionWithJid", newName: "WJid");
            AddColumn("ChatCol", "LastChanged", c => c.DateTime(nullable: false, precision: 0));
            Sql(string.Format("UPDATE `ChatCol` SET `LastChanged` = '{0}' WHERE `LastChanged` = '0000-00-00 00:00:00';",
                now_string));
            CreateIndex("ChatCol", new[] { "UID", "NStart", "NWJid" });
            Sql(";");
            CreateIndex("ChatCol", new[] { "UID", "PStart", "PWJid" });
            Sql(";");
            AddForeignKey("ChatCol", new[] { "UID", "NStart", "NWJid" }, "ChatCol", new[] { "UID", "Start", "WJid" });
            Sql(";");
            AddForeignKey("ChatCol", new[] { "UID", "PStart", "PWJid" }, "ChatCol", new[] { "UID", "Start", "WJid" });
            Sql(";");
            DropColumn("ChMsg", "MessageState");
            Sql(";");
        }
        
        public override void Down()
        {
            AddColumn("ChMsg", "MessageState", c => c.Int(nullable: false));
            DropForeignKey("ChatCol", new[] { "UID", "PStart", "PWJid" }, "ChatCol");
            DropForeignKey("ChatCol", new[] { "UID", "NStart", "NWJid" }, "ChatCol");
            DropIndex("ChatCol", new[] { "UID", "PStart", "PWJid" });
            DropIndex("ChatCol", new[] { "UID", "NStart", "NWJid" });
            DropColumn("ChatCol", "LastChanged");
            RenameColumn(table: "ChMsg", name: "WJid", newName: "CollectionWithJid");
            RenameColumn(table: "ChMsg", name: "Start", newName: "CollectionStartDate");
            RenameColumn(table: "ChMsg", name: "UID", newName: "CollectionArchivingUserId");
            RenameColumn(table: "ItemPref", name: "UID", newName: "ArchivingUserId");
            RenameColumn(table: "ChatCol", name: "PWJid", newName: "PreviousWithJid");
            RenameColumn(table: "ChatCol", name: "PStart", newName: "PreviousStartDate");
            RenameColumn(table: "ChatCol", name: "NWJid", newName: "NextWithJid");
            RenameColumn(table: "ChatCol", name: "NStart", newName: "NextStartDate");
            RenameColumn(table: "ChatCol", name: "WJid", newName: "WithJid");
            RenameColumn(table: "ChatCol", name: "Start", newName: "StartDate");
            RenameColumn(table: "ChatCol", name: "UID", newName: "ArchivingUserId");
            CreateIndex("ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" });
            CreateIndex("ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" });
            AddForeignKey("ChatCol", new[] { "ArchivingUserId", "PreviousStartDate", "PreviousWithJid" }, "ChatCol", new[] { "ArchivingUserId", "StartDate", "WithJid" });
            AddForeignKey("ChatCol", new[] { "ArchivingUserId", "NextStartDate", "NextWithJid" }, "ChatCol", new[] { "ArchivingUserId", "StartDate", "WithJid" });
        }
    }
}
