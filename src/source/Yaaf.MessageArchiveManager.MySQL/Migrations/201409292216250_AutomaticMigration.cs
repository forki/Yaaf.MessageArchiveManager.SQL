namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutomaticMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("ChMsg", "MessageState", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("ChMsg", "DbMessageState", c => c.Int(nullable: false, defaultValue: 1));
        }

        public override void Down()
        {
            DropColumn("ChMsg", "MessageState");
            DropColumn("ChMsg", "DbMessageState");
        }
    }
}
