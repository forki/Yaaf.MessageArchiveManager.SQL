// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using Yaaf.FSharp.Control;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaaf.Xmpp.MessageArchiving;
using System.Data.Entity;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql {
	class MessageArchivingUserStore : BaseUserStore, IExtendedUserArchivingStore {

		public MessageArchivingUserStore (Func<AbstractMessageArchivingDbContext> contextCreator, string user)
			: base (contextCreator, user)
		{
		}

		public FSharpAsync<AsyncSeqInner<ChatCollection>> QueryMessages (FSharpOption<Query> value)
		{
			throw new NotImplementedException ();
		}

		public FSharpAsync<Unit> RemoveCollections (FSharpOption<Query> value)
		{
			throw new NotImplementedException ();
		}

		public async Task<FSharpList<ChatCollectionHeader>> FilterMessages (CollectionFilter value)
		{
			throw new NotImplementedException ();
		}

		public async Task<FSharpList<ChangeItem>> GetChangesSince (DateTime value)
		{
			throw new NotImplementedException ();
		}

		public async Task<bool> RemoveCollection (ChatCollectionId value)
		{
			throw new NotImplementedException ();
		}

		public async Task<ChatCollection> RetrieveCollection (ChatCollectionId value)
		{
			throw new NotImplementedException ();
		}

		public async Task StoreCollection (ChatCollection col)
		{
			using (var context = CreateContext ()) {
				var header = col.Header;
				var chatId = header.Id;
				var strippedStart = StripDate (chatId.Start);
				var dbCollection = await
					(from cols in context.ChatCollections
					 where cols.ArchivingUserId == UserId && cols.StartDate == strippedStart && cols.WithJid == chatId.With.FullId
					 select cols).FirstOrDefaultAsync ();
				if (dbCollection == null) {
					// add new collection
					dbCollection = new Model.DbChatCollection ();
					dbCollection.ArchivingUserId = UserId;
					dbCollection.StartDate = strippedStart;
					dbCollection.WithJid = chatId.With.FullId;
					dbCollection.Version = 0;
					context.ChatCollections.Add (dbCollection);
					await context.MySaveChanges ();
				}

				if (!col.ChatItems.IsEmpty)
				{
					// add messages
					var lastDate = chatId.Start;
					foreach (var chatItem in col.ChatItems) {
						var chat = new Model.DbChatMessage ();
						chat.ChatCollection = dbCollection;
						chat.CollectionArchivingUserId = dbCollection.ArchivingUserId;
						chat.CollectionStartDate = strippedStart;
						chat.CollectionWithJid = dbCollection.WithJid;
						chat.ContentType = Model.MessageContentType.Body;
						chat.Content = chatItem.Message;
						chat.MessageType = GetMessageType (chatItem);

						lastDate = GetNextDate (lastDate, chatItem);
						chat.MessageDateTime = StripDate (lastDate);
						//dbCollection.ChatMessages.Add (chat);
						context.ChatMessages.Add (chat);
					}
				} else {
					// No messages, so check if we have to delete subject
					if (header.Subject == null) {
						// change subject
						dbCollection.Subject = null;
					}
				}

				if (header.Subject != null) {
					// change subject
					dbCollection.Subject = header.Subject.Value;
				}

				if (header.Thread != null) {
					// change thread
					dbCollection.Thread = header.Thread.Value;
				}

				if (col.SetNext) {
					// change next collection
					if (col.Next == null) {
						dbCollection.NextStartDate = null;
						dbCollection.NextWithJid = null;
					} else {
						var next = col.Next.Value;
						dbCollection.NextStartDate = StripDate (next.Start);
						dbCollection.NextWithJid = next.With.FullId;
					}
				}

				if (col.SetPrevious) {
					// change previous collection
					if (col.Next == null) {
						dbCollection.PreviousStartDate = null;
						dbCollection.PreviousWithJid = null;
					} else {
						var previous = col.Previous.Value;
						dbCollection.PreviousStartDate = StripDate (previous.Start);
						dbCollection.PreviousWithJid = previous.With.FullId;
					}
				}

				// Save changes to collection
				await context.MySaveChanges ();
			}
		}

		private DateTime StripDate (DateTime d)
		{
			return new DateTime (d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
		}

		private DateTime GetNextDate (DateTime lastDate, ChatItem chatItem)
		{
			switch (chatItem.Tag) {
			case ChatItem.Tags.From:
				var from = (ChatItem.From) chatItem;
				lastDate = GetNextDate (lastDate, from.Item1);
				break;
			case ChatItem.Tags.To:
				var to = (ChatItem.To) chatItem;
				lastDate = GetNextDate (lastDate, to.Item1);
				break;
			case ChatItem.Tags.Note:
				var note = (ChatItem.Note) chatItem;
				if (note.Item2 != null) {
					lastDate = note.Item2.Value;
				}
				break;
			default:
				throw new InvalidOperationException ("unknown chatitem type!");
				break;
			}
			return lastDate;
		}

		private DateTime GetNextDate (DateTime lastDate, MessageInfo messageInfo)
		{
			if (messageInfo.Utc != null) {
				// best case
				return messageInfo.Utc.Value;
			}
			if (messageInfo.Sec != null) {
				// Ok scenario
				return lastDate.AddSeconds (messageInfo.Sec.Value);
			}
			// not good...
			return lastDate;
		}

		private Model.MessageType GetMessageType (ChatItem chatItem)
		{
			switch (chatItem.Tag) {
			case ChatItem.Tags.From:
				return Model.MessageType.From;
			case ChatItem.Tags.To:
				return Model.MessageType.To;
			case ChatItem.Tags.Note:
				return Model.MessageType.Note;
			default:
				throw new InvalidOperationException ("unknown chatitem type!");
				break;
			}
		}
	}
}
