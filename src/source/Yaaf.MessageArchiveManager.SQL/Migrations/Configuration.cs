// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Yaaf.Database;

    internal sealed class Configuration : MSSQLConfiguration<MSSQLMessageArchivingDbContext>
    {

    }

    public class MigrationsContextFactory : IDbContextFactory<MSSQLMessageArchivingDbContext>
    {
        public MSSQLMessageArchivingDbContext Create()
        {
            return new MSSQLMessageArchivingDbContext("ArchiveDb_MSSQL");
        }
    }

}
