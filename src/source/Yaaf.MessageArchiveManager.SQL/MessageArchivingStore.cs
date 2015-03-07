// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaaf.Xmpp.MessageArchiving;
using Yaaf.Xmpp.MessageArchiveManager.Sql.Model;
using System.Data.Entity;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql {
	public class MessageArchivingStore : BaseContextCreator, IMessageArchivingStore {
		
		public MessageArchivingStore (Func<AbstractMessageArchivingDbContext> contextCreator)
			: base(contextCreator)
		{
		}

		private async Task<string> GetOrCreateUserId (JabberId value)
		{
			using (var context = this.CreateContext()) {
				var userId = value.Localpart.Value;
				var currentUser = await
					(from user in context.Users
					 where user.UserId == userId
					 select user).FirstOrDefaultAsync ();
				if (currentUser == null)
				{
					// add new user
					currentUser = new DbArchivingUser () { UserId = userId };
					context.Users.Add (currentUser);
					await context.MySaveChanges ();
				}
				Debug.Assert (currentUser.UserId == userId);
				return currentUser.UserId;
			}
		}

		public async Task<IUserArchivingStore> GetArchiveStore (JabberId value)
		{
			return new MessageArchivingUserStore (this.CreateContext, await GetOrCreateUserId (value));
		}

		public async Task<IUserPreferenceStore> GetPreferenceStore (JabberId value)
		{
			return new MessageArchivingUserPreferenceStore (this.CreateContext, await GetOrCreateUserId (value));
		}
	}
}
