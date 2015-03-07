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

	[Table ("ChatCol")]
	public class DbChatCollection {
		[Key, Column (Order = 0)]
		//[ForeignKey ("NextCollection")]
		//[ForeignKey ("PreviousCollection")]
		public string ArchivingUserId { get; set; }
		[ForeignKey ("ArchivingUserId ")]
		public virtual DbArchivingUser ArchivingUser { get; set; }

		/// <summary>
		/// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
		/// </summary>
		[Key, Column (Order = 1)]
		public DateTime StartDate { get; set; }
        
		[Key, Column (Order = 2)]
		public string WithJid { get; set; }

		public string Subject { get; set; }

		public string Thread { get; set; }

        /// <summary>
        /// This is for Replication: If this collection is in the list of changed elements the client will
        /// notice, that it has saved an older version locally and can request the updated version.
        /// </summary>
		public int Version { get; set; }

        ///// <summary>
        ///// This is for replication, to request all collections changed since a specific date.
        ///// </summary>
        //public DateTime LastChanged { get; set; }
		
        /// <summary>
		/// We do not delete DbChatCollection entries, because we need the deletion info for replication!
		/// Instead we set the IsDeleted flag for deletion (and delete all data/messages within this collection).
		/// </summary>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
		/// </summary>
		//[ForeignKey ("NextCollection")]
		public DateTime? NextStartDate { get; set; }

		//[ForeignKey ("NextCollection")]
		public string NextWithJid { get; set; }

		public virtual DbChatCollection NextCollection { get; set; }

		/// <summary>
		/// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
		/// </summary>
		//[ForeignKey ("PreviousCollection")]
		public DateTime? PreviousStartDate { get; set; }

		//[ForeignKey ("PreviousCollection")]
		public string PreviousWithJid { get; set; }
		public virtual DbChatCollection PreviousCollection { get; set; }

		/// <summary>
		/// See '5.7 Associating Attributes with a Collection'
		/// </summary>
		// MySql has no support for 'xml'
		//[Column ("XData", TypeName = "xml")]
		public string XData { get; set; }


		public virtual ICollection<DbChatMessage> ChatMessages { get; set; }
	}
}
