// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Model {
	public enum OtrMode {
		NotSet = 0,
		/// the user MUST explicitly approve off-the-record communication.
		Approve = 1,
		/// communications MAY be off the record if requested by another user.
		Concede = 2,
		/// communications MUST NOT be off the record.
		Forbid = 3,
		/// communications SHOULD NOT be off the record even if requested.
		Oppose = 4,
		/// communications SHOULD be off the record if possible.
		Prefer = 5,
		/// communications MUST be off the record. *
		Require = 6
	}

	public enum ArchivingMode {
		NotSet = 0,
		Concede = 1,
		Forbid  = 2,
		Prefer  = 3
	}

	public enum SaveMode {
		NotSet = 0,
		/// the saving entity SHOULD save only <body/> elements.
		/// Note: When archiving locally a client MAY save the full XML content of each <message/> element even if the Save Mode is 'body'.
		Body = 1,
		/// the saving entity MUST save nothing (false).
		Nothing = 2,
		/// the saving entity SHOULD save the full XML content of each <message/> element.
		/// Note: Support for the 'message' value is optional and, to conserve bandwidth and storage space, it is RECOMMENDED that client implementations do not specify the 'message' value.
		Message = 3,
		/// the saving entity SHOULD save every byte that passes over the stream in either direction.
		Stream = 4,
	}

	public class SavingModeEntity {

		[NotMapped]
		public SaveMode SaveMode
		{ get { return (SaveMode) DbSaveMode; } set { DbSaveMode = (int) value; } }

		public int DbSaveMode { get; set; }


		[NotMapped]
		public OtrMode OtrMode
		{ get { return (OtrMode) DbOtrMode; } set { DbOtrMode = (int) value; } }

		public int DbOtrMode { get; set; }

		public long? Expire { get; set; }
	}
	[Table("ArcUs")]
	public class DbArchivingUser : SavingModeEntity {
		[Key, Column (Order = 0)]
		public string UserId { get; set; }

		public bool? AutomaticArchiving { get; set; }
		
		// No Archiving scope, as we can ignore stream scope (only global scopes are saved in database)

		[NotMapped]
		public ArchivingMode AutoPreference
		{ get { return (ArchivingMode) DbAutoPreference; } set { DbAutoPreference = (int) value; } }

		public int DbAutoPreference { get; set; }


		[NotMapped]
		public ArchivingMode LocalPreference
		{ get { return (ArchivingMode) DbLocalPreference; } set { DbLocalPreference = (int) value; } }

		public int DbLocalPreference { get; set; }



		[NotMapped]
		public ArchivingMode ManualPreference
		{ get { return (ArchivingMode) DbManualPreference; } set { DbManualPreference = (int) value; } }

		public int DbManualPreference { get; set; }


		public virtual ICollection<DbUserItemPreference> ItemPreferences { get; set; }
		public virtual ICollection<DbChatCollection> ChatCollections { get; set; }
	}
}
