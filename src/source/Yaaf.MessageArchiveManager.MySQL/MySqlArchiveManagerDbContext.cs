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

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.MySql {


	[DbConfigurationType (typeof (MySqlEFConfiguration))]
	public class MySqlArchiveManagerDbContext : AbstractMessageArchivingDbContext {
		protected override void Init ()
		{
			DbConfiguration.SetConfiguration (new MySqlEFConfiguration ());
			System.Data.Entity.Database.SetInitializer<MySqlArchiveManagerDbContext> (
					   new MigrateDatabaseToLatestVersion<MySqlArchiveManagerDbContext, Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations.Configuration> ());
		}

		public MySqlArchiveManagerDbContext ()
			: base ("ArchiveDb_MySQL")
		{
		}

		//public MySqlArchiveManagerDbContext (string nameOrConnection)
		//	: base (nameOrConnection)
		//{
		//}
	}
}
