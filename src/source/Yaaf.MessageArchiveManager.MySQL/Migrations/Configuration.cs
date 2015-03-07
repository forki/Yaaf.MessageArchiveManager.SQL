namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
{
	using global::MySql.Data.Entity;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.MySqlArchiveManagerDbContext>
    {
        public Configuration()
		{
			CodeGenerator = new MySqlMigrationCodeGenerator ();
			SetSqlGenerator ("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator ());
			AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.MySqlArchiveManagerDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
