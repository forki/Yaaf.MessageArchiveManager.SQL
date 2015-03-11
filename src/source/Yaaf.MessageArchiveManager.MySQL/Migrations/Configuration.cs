namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using global::MySql.Data.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Yaaf.Database.MySQL;

    internal sealed class Configuration : MySQLConfiguration<MySqlArchiveManagerDbContext>
    {
    }
    public class MigrationsContextFactory : IDbContextFactory<MySqlArchiveManagerDbContext>
    {
        public MySqlArchiveManagerDbContext Create()
        {
            return new MySqlArchiveManagerDbContext("ArchiveDb_MySQL");
        }
    }
}
