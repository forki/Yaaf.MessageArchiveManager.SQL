// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yaaf.Database;
using Yaaf.Xmpp.MessageArchiveManager.Sql.Model;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql {
	public abstract class AbstractMessageArchivingDbContext : Yaaf.Database.AbstractApplicationDbContext {


		public AbstractMessageArchivingDbContext (string nameOrConnection, bool doInit = true)
			: base (nameOrConnection, FSharpHelper.ToFSharpS<bool>(false))
		{
            if (doInit)
            {
                this.DoInit();
            }
		}

		/// <summary>
		/// The below Method is used to define the Maping
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating (DbModelBuilder modelBuilder)
		{
			base.OnModelCreating (modelBuilder);

			//modelBuilder.Entity<DbChatMessage> ()
			//	.HasKey(x => x.MessageId)
			//	.HasRequired (s => s.ChatCollection)
			//	.WithMany (r => r.ChatMessages)
			//	.HasForeignKey (s => new { s.CollectionArchivingUserId, s.CollectionStartDate, s.CollectionWithJid })
			//	//.HasKey(s => s.ApplicationUserId)
			//	.WillCascadeOnDelete (false);

			// Seems like there is no other way to specify the key for optional:optional bindings: http://stackoverflow.com/questions/11480083/ef-4-1-code-first-modelbuilder-hasforeignkey-for-one-to-one-relationships
			modelBuilder.Entity<DbChatCollection> ()
				.HasOptional (s => s.NextCollection).WithMany ().HasForeignKey (s => new { s.ArchivingUserId, s.NextStartDate, s.NextWithJid }).WillCascadeOnDelete(false);
			modelBuilder.Entity<DbChatCollection> ()
				.HasOptional (s => s.PreviousCollection).WithMany ().HasForeignKey (s => new { s.ArchivingUserId, s.PreviousStartDate, s.PreviousWithJid }).WillCascadeOnDelete (false);
			
			
			modelBuilder.Entity<DbChatCollection> ()
				.HasRequired(s => s.ArchivingUser)
				.WithMany(r => r.ChatCollections)
				.HasForeignKey (s => new { s.ArchivingUserId })
				//.HasKey(s => s.ApplicationUserId)
				.WillCascadeOnDelete (false);
		}

		public DbSet<DbArchivingUser> Users { get; set; }
		public DbSet<DbUserItemPreference> UserItemPreferences { get; set; }
		public DbSet<DbChatCollection> ChatCollections { get; set; }
		public DbSet<DbChatMessage> ChatMessages { get; set; }
	}

	[DbConfigurationType (typeof (EmptyConfiguration))]
	public class MSSQLMessageArchivingDbContext : AbstractMessageArchivingDbContext {
		public override void Init ()
		{
			DbConfiguration.SetConfiguration (new EmptyConfiguration ());
			System.Data.Entity.Database.SetInitializer<MSSQLMessageArchivingDbContext> (
				   new MigrateDatabaseToLatestVersion<
                       MSSQLMessageArchivingDbContext, 
                       MSSQLConfiguration<MSSQLMessageArchivingDbContext>> ());
		}

		public MSSQLMessageArchivingDbContext (string nameOrConnection, bool doInit = true)
			: base (nameOrConnection, false)
		{
            if (doInit)
            {
                this.DoInit();
            }
		}

		//public MessageArchivingDbContext (string nameOrConnection)
		//	: base (nameOrConnection)
		//{
		//}
	}
}
