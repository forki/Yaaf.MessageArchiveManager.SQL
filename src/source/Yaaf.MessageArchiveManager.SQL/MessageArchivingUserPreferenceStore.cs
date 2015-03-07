// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaaf.Database;
using Yaaf.Xmpp.MessageArchiving;
using System.Data.Entity;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql {
	class MessageArchivingUserPreferenceStore : BaseUserStore, IUserPreferenceStore {
		internal MessageArchivingUserPreferenceStore (Func<AbstractMessageArchivingDbContext> contextCreator, string user)
			: base(contextCreator, user)
		{
		}

		public async Task<FSharpOption<StoredPreferenceInfo>> GetUserPreferences ()
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);
				var isDefaultModeUnset =
						user.OtrMode == Model.OtrMode.NotSet && user.SaveMode == Model.SaveMode.NotSet && user.Expire == null;

				var isAnyArchivingModeUnset =
					user.LocalPreference == Model.ArchivingMode.NotSet || 
					user.ManualPreference == Model.ArchivingMode.NotSet ||
					user.AutoPreference == Model.ArchivingMode.NotSet;

				var isAllUnset =
					isDefaultModeUnset &&
					user.ItemPreferences.Count == 0 && isAnyArchivingModeUnset && user.AutomaticArchiving == null;
				if (isAllUnset) {
					return FSharpOption<StoredPreferenceInfo>.None;
				}

				var archivingModePrefs =
					!isAnyArchivingModeUnset
					? FSharpOption<AllMethodSettings>.Some (
						new AllMethodSettings (
							ToFSharp (user.AutoPreference),
							ToFSharp (user.LocalPreference),
							ToFSharp (user.ManualPreference)))
					: FSharpOption<AllMethodSettings>.None;

				var result = new StoredPreferenceInfo (
					FSharpHelper.ToFSharpS(user.AutomaticArchiving),
					new OtrSaveMode (
						FSharpHelper.ToFSharpS<Int64> (user.Expire),
						ToFSharp (user.OtrMode),
						ToFSharp (user.SaveMode)),
						ListModule.OfSeq<Tuple<JabberId, OtrSaveMode>> 
							(user.ItemPreferences.Select(item => 
								Tuple.Create(
									JabberId.Parse(item.Jid), 
									new OtrSaveMode(
										FSharpHelper.ToFSharpS<Int64> (item.Expire),
										ToFSharp(item.OtrMode),
										ToFSharp(item.SaveMode))))),
						archivingModePrefs
					);
				return FSharpOption<StoredPreferenceInfo>.Some(result);
			}
		}

		private UseType ToFSharp (Model.ArchivingMode archivingMode)
		{
			switch (archivingMode) {
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.ArchivingMode.Concede:
				return UseType.Concede;
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.ArchivingMode.Forbid:
				return UseType.Forbid;
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.ArchivingMode.Prefer:
				return UseType.Prefer;
				break;
			default:
				throw new InvalidOperationException (string.Format ("Invalid archiving mode %s", archivingMode.ToString ()));
				break;
			}
		}

		private FSharpOption<SaveMode> ToFSharp (Model.SaveMode saveMode)
		{
			switch (saveMode) {
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.SaveMode.NotSet:
				return FSharpOption<SaveMode>.None;
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.SaveMode.Body:
				return FSharpOption<SaveMode>.Some (SaveMode.Body);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.SaveMode.Nothing:
				return FSharpOption<SaveMode>.Some (SaveMode.Nothing);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.SaveMode.Message:
				return FSharpOption<SaveMode>.Some (SaveMode.Message);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.SaveMode.Stream:
				return FSharpOption<SaveMode>.Some (SaveMode.Stream);
				break;
			default:
				throw new InvalidOperationException (string.Format ("Unknown save mode %s", saveMode.ToString ()));
				break;
			}
		}

		public static FSharpOption<OtrMode> ToFSharp (Model.OtrMode otrMode)
		{
			switch (otrMode) {
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.NotSet:
				return FSharpOption<OtrMode>.None;
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Approve:
				return FSharpOption<OtrMode>.Some (OtrMode.Approve);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Concede:
				return FSharpOption<OtrMode>.Some (OtrMode.Concede);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Forbid:
				return FSharpOption<OtrMode>.Some (OtrMode.Forbid);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Oppose:
				return FSharpOption<OtrMode>.Some (OtrMode.Oppose);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Prefer:
				return FSharpOption<OtrMode>.Some (OtrMode.Prefer);
				break;
			case Yaaf.Xmpp.MessageArchiveManager.Sql.Model.OtrMode.Require:
				return FSharpOption<OtrMode>.Some (OtrMode.Require);
				break;
			default:
				throw new InvalidOperationException (string.Format ("Unknown otr mode %s", otrMode.ToString ()));
				break;
			}
		}

		//public void SetUserPreferences (StoredPreferenceInfo value)
		//{
		//	using (var context = this.CreateContext ()) {
		//		var user = GetUser (context);
		//		if (value.AutomaticArchiving != FSharpOption<bool>.None) {
		//			user.AutomaticArchiving = FSharpHelper.FromFSharpS (value.AutomaticArchiving);
		//		}
		//
		//		if (value.DefaultOtrSaveMode.Expire != FSharpOption<ulong>.None) {
		//			user.Expire = FSharpHelper.FromFSharpS (value.DefaultOtrSaveMode.Expire);
		//		}
		//		if (value.DefaultOtrSaveMode.OtrMode != FSharpOption<OtrMode>.None) {
		//			user.OtrMode = FromFSharp (value.DefaultOtrSaveMode.OtrMode.Value);
		//		}
		//		if (value.DefaultOtrSaveMode.SaveMode != FSharpOption<SaveMode>.None) {
		//			user.SaveMode = FromFSharp (value.DefaultOtrSaveMode.SaveMode.Value);
		//		}
		//
		//		if (value.MethodSetting != FSharpOption<AllMethodSettings>.None) {
		//			user.AutoPreference = FromFSharp (value.MethodSetting.Value.Auto);
		//			user.LocalPreference = FromFSharp (value.MethodSetting.Value.Local);
		//			user.ManualPreference = FromFSharp (value.MethodSetting.Value.Manual);
		//		}
		//		context.MySaveChanges ();
		//	}
		//}
		public async Task RemoveItem (JabberId jid)
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);

				var savedItem = await
					(from item in context.UserItemPreferences
					 where item.ArchivingUserId == UserId && item.Jid == jid.FullId
					 select item).FirstOrDefaultAsync ();

				if (savedItem != null) {
					context.UserItemPreferences.Remove (savedItem);
					await context.MySaveChanges ();
				}
			}
		}

		public async Task SetArchiving (FSharpOption<bool> value)
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);
				if (value != FSharpOption<bool>.None) {
					user.AutomaticArchiving = FSharpHelper.FromFSharpS (value);
				}
				await context.MySaveChanges ();
			}
		}

		public async Task SetDefaultOtrSaveMode (FSharpOption<OtrSaveMode> value)
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);
				if (value == FSharpOption<OtrSaveMode>.None) {
					user.Expire = null;
					user.OtrMode = Model.OtrMode.NotSet;
					user.SaveMode =  Model.SaveMode.NotSet;
				} else {
					var saveMode = value.Value;

					if (saveMode.Expire != FSharpOption<long>.None) {
						user.Expire = FSharpHelper.FromFSharpS (saveMode.Expire);
					}
					if (saveMode.OtrMode != FSharpOption<OtrMode>.None) {
						user.OtrMode = FromFSharp (saveMode.OtrMode.Value);
					}
					if (saveMode.SaveMode != FSharpOption<SaveMode>.None) {
						user.SaveMode = FromFSharp (saveMode.SaveMode.Value);
					}
				}

				await context.MySaveChanges ();
			}
		}

		public async Task SetItem (JabberId jid, OtrSaveMode saveMode)
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);

				var savedItem = await
					(from item in context.UserItemPreferences
					 where item.ArchivingUserId == UserId && item.Jid == jid.FullId
					 select item).FirstOrDefaultAsync ();
				if (savedItem == null) {
					savedItem = new Model.DbUserItemPreference ();
					savedItem.ArchivingUserId = UserId;
					savedItem.Jid = jid.FullId;
					context.UserItemPreferences.Add (savedItem);
				}

				if (saveMode.Expire != FSharpOption<long>.None) {
					savedItem.Expire = FSharpHelper.FromFSharpS (saveMode.Expire);
				}
				if (saveMode.OtrMode != FSharpOption<OtrMode>.None) {
					savedItem.OtrMode = FromFSharp (saveMode.OtrMode.Value);
				}
				if (saveMode.SaveMode != FSharpOption<SaveMode>.None) {
					savedItem.SaveMode = FromFSharp (saveMode.SaveMode.Value);
				}

				await context.MySaveChanges ();
			}
		}

		public async Task SetMethodPreferences (FSharpOption<AllMethodSettings> value)
		{
			using (var context = this.CreateContext ()) {
				var user = await GetUser (context);

				if (value != FSharpOption<AllMethodSettings>.None) {
					user.AutoPreference = FromFSharp (value.Value.Auto);
					user.LocalPreference = FromFSharp (value.Value.Local);
					user.ManualPreference = FromFSharp (value.Value.Manual);
				}
				await context.MySaveChanges ();
			}
		}


		private Model.ArchivingMode FromFSharp (UseType useType)
		{
			switch (useType.Tag) {
			case UseType.Tags.Concede:
				return Model.ArchivingMode.Concede;
				break;
			case UseType.Tags.Forbid:
				return Model.ArchivingMode.Forbid;
				break;
			case UseType.Tags.Prefer:
				return Model.ArchivingMode.Prefer;
				break;
			default:
				throw new InvalidOperationException (string.Format ("Invalid archiving mode %s", useType.ToString ()));
				break;
			}
		}

		private Model.SaveMode FromFSharp (SaveMode saveMode)
		{
			switch (saveMode.Tag) {
			case SaveMode.Tags.Body:
				return Model.SaveMode.Body;
				break;
			case SaveMode.Tags.Nothing:
				return Model.SaveMode.Nothing;
				break;
			case SaveMode.Tags.Message:
				return Model.SaveMode.Message;
				break;
			case SaveMode.Tags.Stream:
				return Model.SaveMode.Stream;
				break;
			default:
				throw new InvalidOperationException (string.Format ("Unknown save mode %s", saveMode.ToString ()));
				break;
			}
		}

		private Model.OtrMode FromFSharp (OtrMode otrMode)
		{
			switch (otrMode.Tag) {
			case OtrMode.Tags.Approve:
				return Model.OtrMode.Approve;
				break;
			case OtrMode.Tags.Concede:
				return Model.OtrMode.Concede;
				break;
			case OtrMode.Tags.Forbid:
				return Model.OtrMode.Forbid;
				break;
			case OtrMode.Tags.Oppose:
				return Model.OtrMode.Oppose;
				break;
			case OtrMode.Tags.Prefer:
				return Model.OtrMode.Prefer;
				break;
			case OtrMode.Tags.Require:
				return Model.OtrMode.Require;
				break;
			default:
				throw new InvalidOperationException (string.Format ("Unknown otr mode %s", otrMode.ToString ()));
				break;
			}
		}


	}
}
