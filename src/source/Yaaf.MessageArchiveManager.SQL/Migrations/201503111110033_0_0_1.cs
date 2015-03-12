namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0_0_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatCol", "LastChanged", c => c.DateTime(nullable: true));
            var now_string = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Sql(string.Format("UPDATE [dbo].[ChatCol] SET LastChanged = '{0}' WHERE LastChanged IS NULL", now_string));
            AlterColumn("dbo.ChatCol", "LastChanged", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatCol", "LastChanged");
        }
    }
}
