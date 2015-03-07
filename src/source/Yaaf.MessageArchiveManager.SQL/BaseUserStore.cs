// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaaf.Xmpp.MessageArchiveManager.Sql.Model;
using System.Data.Entity;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql {
	public class BaseContextCreator {

		private Func<AbstractMessageArchivingDbContext> contextCreator;

		internal BaseContextCreator (Func<AbstractMessageArchivingDbContext> contextCreator)
		{
			this.contextCreator = contextCreator;
		}


		protected AbstractMessageArchivingDbContext CreateContext ()
		{
			return contextCreator ();
		}
	}
	class BaseUserStore : BaseContextCreator {
		private string userId;
		private Func<AbstractMessageArchivingDbContext> contextCreator;

		internal BaseUserStore (Func<AbstractMessageArchivingDbContext> contextCreator, string userId)
			: base(contextCreator)
		{
			this.contextCreator = contextCreator;
			this.userId = userId;
		}

		protected string UserId { get { return userId; } }

		protected async Task<DbArchivingUser> GetUser (AbstractMessageArchivingDbContext context)
		{
			return await
				(from user in context.Users
				 where user.UserId == userId
				 select user).FirstAsync ();
		}

	}
}
