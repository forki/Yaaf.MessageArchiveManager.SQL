namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
    using global::MySql.Data.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Yaaf.Database.MySQL;

    // Public, so that Fix_MySQL_Script.fsx can access it.
    public sealed class Configuration : MySQLConfiguration<MySqlArchiveManagerDbContext>
    {
        public Configuration()
        {
        }
    }
    public class MigrationsContextFactory : IDbContextFactory<MySqlArchiveManagerDbContext> {
        static MigrationsContextFactory()
        {
            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
           DbConfiguration.SetConfiguration(new global::MySql.Data.Entity.MySqlEFConfiguration());
        }
        public MySqlArchiveManagerDbContext Create()
        {
            return new MySqlArchiveManagerDbContext("ArchiveDb_MySQL", false);
        }
    }
}
