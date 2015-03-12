// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using Yaaf.Database;
using Yaaf.Database.MySQL;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql {


	[DbConfigurationType (typeof (MySqlEFConfiguration))]
	public class MySqlArchiveManagerDbContext : AbstractMessageArchivingDbContext {
		public MySqlArchiveManagerDbContext (string nameOrConnection, bool doInit = true)
			: base (nameOrConnection)
		{
            if (doInit)
            {
                DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
                System.Data.Entity.Database.SetInitializer<MySqlArchiveManagerDbContext>(null);
                this.Upgrade();
            }
		}

        public override string FixScript(string s)
        {
            return DatabaseUpgrade.FixScript_MySQL(s);
        }

        public override DbMigrator GetMigrator()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            var config = new Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations.Configuration();
            config.TargetDatabase =
                new DbConnectionInfo(this.Database.Connection.ConnectionString, "MySql.Data.MySqlClient");
            return new DbMigrator(config);
        }
    }
}
