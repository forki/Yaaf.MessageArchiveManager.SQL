// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Yaaf.Database.MySQL;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql {


	[DbConfigurationType (typeof (MySqlEFConfiguration))]
	public class MySqlArchiveManagerDbContext : AbstractMessageArchivingDbContext {
		public override void Init ()
		{
			DbConfiguration.SetConfiguration (new MySqlEFConfiguration ());
			System.Data.Entity.Database.SetInitializer<MySqlArchiveManagerDbContext> (
                       new MigrateDatabaseToLatestVersion<MySqlArchiveManagerDbContext, MySQLConfiguration<MySqlArchiveManagerDbContext>>());
		}

		public MySqlArchiveManagerDbContext (string nameOrConnection, bool doInit = true)
			: base (nameOrConnection, false)
		{
            if (doInit)
            {
                this.DoInit();
            }
		}

		//public MySqlArchiveManagerDbContext (string nameOrConnection)
		//	: base (nameOrConnection)
		//{
		//}
	}
}
